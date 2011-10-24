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
        private SqlGeometry mrPointsgeom = new SqlGeometry();
        private SqlGeometry mrLinesgeom = new SqlGeometry();
        private SqlGeography tsgeog = new SqlGeography();

        private string pen;
        private int pencolor = 0;
        private int redindex = 1;
        private int greenindex = 1;
        private int blueindex = 1;

        private string events = "LineLocating";
        private const string tableName = "LineLocating";
        private string sql;
        private SqlGeometry sgeom;

        private ILookup<string, EventLocating> eventsLookup;
        private IEnumerable<string> eventsKey;

        public lineLocating()
        {

            #region //这里用的是通用方法
            ExcuteSqlScript es = new ExcuteSqlScript();
            //es.insertSqltableIntoMapinfo(tableName);
            es.createLocatingTable(tableName);
            #endregion

            eventsLookup = dc.EventLocating.ToLookup(e => e.events);

            eventsKey = eventsLookup.Select(e => e.Key);

            foreach (var p in eventsKey)
            {
                foreach (var q in eventsLookup[p])
                {
                    mrPointsgeom = mrPointsgeom.STUnion(q.SP_GEOMETRY);
                    events = q.events;
                }

                redindex++;
                if (redindex > 255) redindex = 0;
                pencolor = redindex * 65535 + greenindex * 256 + blueindex;
                pen = "Pen (1, 2," + pencolor.ToString() + ")";

                mrPointsgeom = mrPointsgeom.STConvexHull().STCentroid().STPointN(1);

                if (!mrPointsgeom.STIsValid()) continue;

                tsgeog = SqlGeography.STGeomFromWKB(mrPointsgeom.STAsBinary(), 4326);

                if (tsgeog.IsNull) continue;

                tsgeog = SqlGeography.Point((double)tsgeog.Lat, (double)tsgeog.Long, 4326);
                tsgeog = tsgeog.STBuffer(1);

                mrLinesgeom = SqlGeometry.STGeomFromWKB(tsgeog.STAsBinary(), 4326);

                Console.WriteLine(mrLinesgeom.STArea());
                insertLocating2Sql(events, pen, mrLinesgeom);
            }
        }


        private void insertLocating2Sql(string events, string pen, SqlGeometry sgeo)
        {
            sgeom = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326);
            sql = @" INSERT INTO [" + tableName + @"]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + sgeom + "')";
            dc.ExecuteCommand(sql);
        }
    }
}
