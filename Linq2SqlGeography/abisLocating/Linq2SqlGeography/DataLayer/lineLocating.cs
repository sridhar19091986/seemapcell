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
        private SqlGeometry mrPoint = new SqlGeometry();
        private SqlGeometry mrLine = new SqlGeometry();
        private string pen = "Pen (60, 2,16711680)";
        private string events = "lineLocating";
        private string sql;
        private SqlGeometry sgeom;

        private ILookup<string, EventLocating> eventsLookup;
        private IEnumerable<string> eventsKey;

        public lineLocating()
        {
            eventsLookup = dc.EventLocating.Where(e => e.events != this.events).ToLookup(e => e.events);

            eventsKey = eventsLookup.Select(e => e.Key);

            foreach (var p in eventsKey)
            {
                foreach (var q in eventsLookup[p])
                {
                    mrPoint = mrPoint.STUnion(q.SP_GEOMETRY);
                }

                //？需要先生成包络线，再求中心点，最后算点集合。
                //？ 为何加了STBuffer(10)会有问题。
                mrPoint = mrPoint.STConvexHull().STCentroid().STPointN(1);
                mrLine = mrLine.STUnion(mrPoint);

                Console.WriteLine(mrLine.STArea());
                insertLocating2Sql(events, pen, mrLine);
            }

            //不知道怎么划线，暂时使用手工定位的方式，点多自然成线

            // mrline = SqlGeometry.STLineFromWKB(mrline.STAsBinary(), 4326);


        }


        private void insertLocating2Sql(string events, string pen, SqlGeometry sgeo)
        {
            sgeom = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326);
            sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + sgeom + "')";
            dc.ExecuteCommand(sql);

            Console.WriteLine(sql);
        }
    }
}
