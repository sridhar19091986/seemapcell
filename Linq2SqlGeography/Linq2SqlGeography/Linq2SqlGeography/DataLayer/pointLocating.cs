using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    public class pointLocating
    {

        private int pencolor = 0;
        private int[] pencolors = { 16711680, 65280, 255, 65535, 16711935, 16776960, 0 };
        private int pencolorindex = 0;
        IEnumerable<Abis_MR> mrr;
        private string events;
        private string pen;
        private SqlGeography sgeo;
        private SqlGeometry mgeo;
        private string sql;
        private DataClasses2DataContext dc = new DataClasses2DataContext();

        public pointLocating()
        {
            getLocating();
        }
        private void getLocating()
        {

            dc.ExecuteCommand(HandleTable.creventlocating);
            //HandleNeighbour hn = new HandleNeighbour();
            //List<mrNeighbour> mrneighsNew = new List<mrNeighbour>();
            //mrneighsNew = hn.getNeighList(hn.setNeighList());

            /*
            Red: 16711680
            Green: 65280
            Blue: 255
            Cyan: 65535
            Magenta: 16711935
            Yellow: 16776960
            Black: 0 
              * */

            HandleNeighbour hn = new HandleNeighbour();
            List<mrNeighbour> mrneighsNew = new List<mrNeighbour>();

            mrr = dc.Abis_MR.Where(e => e.bsic3 > 0).Take(10);

            //var mrr = dc.Abis_MR;

            foreach (var mr in mrr)
            {
                pencolor = pencolors[0];
                pencolorindex++;
                if (pencolorindex >= pencolors.Length - 1)
                    pencolorindex = 0;

                mrneighsNew = hn.getNeighList(hn.setNeighList(mr));
                mrLocating mrl = new mrLocating();
                //pencolor = HandleTable.getRandomPenColor(false,false,false);
                SqlGeography tgeo = new SqlGeography();

                //以时间点做索引比较妥当？
                events = mr.Measurement_Report_time.ToString();

                //string pen = "Pen (6, 2," + pencolor.ToString() + ")";

                //这里可以通过各种方式进行修改，以做前后点的失败

                //如何生成层连线的问题，定位位置点的问题，生成连续的路线走势图，需要再做一个图层，以表示

                //生成覆盖连线也需要再做一个图层

                pen = "Pen (" + (1 + 2 * pencolorindex).ToString() + ", 2," + pencolor.ToString() + ")";
                //var mrnews=mrneighsNew.Take(1);

                foreach (var n in mrneighsNew)
                {
                    if (n.nBaIndex == -1 && n.nBSIC == -1)
                    {
                        tgeo = mrl.sLocating(n.ServiceCell, n.rxLev, n.powerControl); //服务小区
                        insertLocatingGeo(events, pen, tgeo);
                        Console.WriteLine("服务小区：{0}...{1}...{2}", n.ServiceCell, n.rxLev, n.powerControl);
                    }
                    else
                    {
                        if (n.NeighCell != null)
                        {
                            tgeo = mrl.nLocating(n.NeighCell, n.rxLev, n.powerControl); //邻小区
                            insertLocatingGeo(events, pen, tgeo);
                        }
                        Console.WriteLine("邻小区：{0}...{1}...{2}", n.NeighCell, n.rxLev, n.powerControl);
                    }
                }
                //pencolor = HandleTable.getRandomPenColor();
                //这里要执行 locating 算法


                //这里做成一个再处理的过程 ？？？？？？？？？扩展？？？？

                mrl.getLocating();
                sgeo = mrl.sgeo;
                //Console.WriteLine(mgeo.STArea());
                //Console.WriteLine("{0}...{1}", mrl.sgeo.Long, mrl.sgeo.Lat);
            }

            // Console.ReadKey();
        }

        private void insertLocatingGeo(string events, string pen, SqlGeography sgeo)
        {
            mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326);
            sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + mgeo + "')";
            dc.ExecuteCommand(sql);
        }
    }
}
