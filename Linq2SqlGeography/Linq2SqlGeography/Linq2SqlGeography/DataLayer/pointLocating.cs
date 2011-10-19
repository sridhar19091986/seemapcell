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
        private static int pencolor = 0;
        public pointLocating()
        {
            getLocating();
        }
        private  void getLocating()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
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

            int[] pencolors = { 16711680, 65280, 255, 65535, 16711935, 16776960, 0 };
            int pencolorindex = 0;


            HandleNeighbour hn = new HandleNeighbour();
            List<mrNeighbour> mrneighsNew = new List<mrNeighbour>();
            var mrr = dc.MR表格;
            foreach (var mr in mrr.Skip(0).Take(1))
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
                string events = mr.时间点;

                //string pen = "Pen (6, 2," + pencolor.ToString() + ")";

                //这里可以通过各种方式进行修改，以做前后点的失败

                //如何生成层连线的问题，定位位置点的问题，生成连续的路线走势图，需要再做一个图层，以表示

                //生成覆盖连线也需要再做一个图层

                string pen = "Pen (" + (1 + 2 * pencolorindex).ToString() + ", 2," + pencolor.ToString() + ")";

                foreach (var n in mrneighsNew)
                {
                    if (n.nBCCH == null && n.nBSIC == null)
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
                SqlGeography sgeo = mrl.sgeo;
                //Console.WriteLine(mgeo.STArea());
                //Console.WriteLine("{0}...{1}", mrl.sgeo.Long, mrl.sgeo.Lat);
            }

            // Console.ReadKey();
        }

        private  void insertLocatingGeo(string events, string pen, SqlGeography sgeo)
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326);
            string sql = @" INSERT INTO [EventLocating]([events],[MI_STYLE],[SP_GEOMETRY]) VALUES  ('"
                + events + "','" + pen + "','" + mgeo + "')";
            dc.ExecuteCommand(sql);
        }
    }
}
