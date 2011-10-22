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
        private string pen = "Pen (1, 2,16711680)";    //???
        private string events = "lineLocating";   //???
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
                    mrPointsgeom = mrPointsgeom.STUnion(q.SP_GEOMETRY);
                }

                //？需要先生成包络线，再求中心点，最后算点集合。
                //？ 为何加了STBuffer(10)会有问题。
                mrPointsgeom = mrPointsgeom.STConvexHull().STCentroid().STPointN(1);
                tsgeog = SqlGeography.STGeomFromWKB(mrPointsgeom.STAsBinary(), 4326);
                tsgeog = SqlGeography.Point((double)tsgeog.Lat,(double)tsgeog.Long, 4326);
                tsgeog=tsgeog.STBuffer(1);

                mrLinesgeom = SqlGeometry.STGeomFromWKB(tsgeog.STAsBinary(), 4326);

                Console.WriteLine(mrLinesgeom.STArea());
                insertLocating2Sql(events, pen, mrLinesgeom);
            }

            //不知道怎么划线，暂时使用手工定位的方式，点多自然成线

            // mrline = SqlGeometry.STLineFromWKB(mrline.STAsBinary(), 4326);


        }

        //2011.10.21 需要解决的2个问题？


        //1.需要重新生成到新的table？ google earth 上识别

        //2.切换，变换小区的问题 ？？ 这个直接在m-trix切除？

        //3.style

        //3.brush color 渐进

        //4.color 递归

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
