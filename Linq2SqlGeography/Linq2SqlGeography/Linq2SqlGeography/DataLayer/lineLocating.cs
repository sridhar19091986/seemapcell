using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    public class lineLocating
    {
        private DataClasses2DataContext dc = new DataClasses2DataContext();
        private SqlGeometry mrpoint = new SqlGeometry();
        public SqlGeometry mrline = new SqlGeometry();
        string pen = "Pen (60, 2,16711680)";
        string events = "lineLocating";
        public lineLocating()
        {
            var eventslookup = dc.EventLocating
                .Where(e => e.events != this.events)
                .ToLookup(e => e.events);

            var eventskey = eventslookup.Select(e => e.Key);
            foreach (var p in eventskey)
            {
                foreach (var q in eventslookup[p])
                {
                    mrpoint = mrpoint.STUnion(q.SP_GEOMETRY);
                }
                mrpoint = mrpoint.STCentroid();
                mrline = mrline.STUnion(mrpoint);
            }

           //不知道怎么划线，暂时使用手工定位的方式，点多自然成线

           // mrline = SqlGeometry.STLineFromWKB(mrline.STAsBinary(), 4326);

            Console.WriteLine(mrline.STArea());
            insertLocating2Sql(events, pen, mrline);
        }


        private void insertLocating2Sql(string events, string pen, SqlGeometry sgeo)
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326);
            string sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + mgeo + "')";
            dc.ExecuteCommand(sql);

            Console.WriteLine(sql);
        }
    }
}
