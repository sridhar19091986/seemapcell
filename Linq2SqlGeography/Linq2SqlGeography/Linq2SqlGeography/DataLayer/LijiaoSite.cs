//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Linq2SqlGeography;
//using Microsoft.SqlServer.Types;
//using System.Data.SqlTypes;
//using System.Data.SqlClient;

//namespace Linq2SqlGeography
//{
//    public class LiJiaoSite
//    {
//        public 立交ABC lj { get; set; }
//        public MCOMSITE site { get; set; }
//    }
//    public class GetLiJiaoSite
//    {

//        public void getLiJiaoSite(DataClasses2DataContext dc, MCOMSITE site, List<LiJiaoSite> lijiaosites)
//        {

//            //var geo = SqlGeography.STGeomFromWKB(site.SP_GEOMETRY.STAsBinary(), 4326);
//            //Console.WriteLine(geo.Lat);
//            //SqlGeometry ->SqlGeography
//            var sp = SqlGeography.Point((double)site.Latitude, (double)site.Longitude, 4326);
//            var sitebuffer = sp.STBuffer(50);   //类型转换以后没有问题，单位是米
//            foreach (var lj in dc.立交ABC)
//            {
//                if (lj.SP_GEOMETRY != null)
//                {
//                    //SqlGeometry ->SqlGeography
//                    var ljp = SqlGeography.STGeomFromWKB(lj.SP_GEOMETRY.STAsBinary(), 4326);

//                    /*
//                     1.掉话和立交的距离？
//                     2.切换失败和立交的距离？
//                     3.未接通和立交的距离？
                                         
//                        异常事件类型	触发次数
//                        SDCCH掉话	71
//                        TCH掉话	215
//                        未接通	208
//                        切换失败	633
//                        上行连续弱覆盖	5122
//                        下行连续弱覆盖	783
//                        上行连续质差	417
//                        下行连续质差	510
                             
//                     * */

//                    //基站和立交的距离？

//                    if (ljp.STIntersects(sitebuffer))
//                    {
//                        LiJiaoSite lijiaosite = new LiJiaoSite();
//                        lijiaosite.lj = lj;
//                        lijiaosite.site = site;
//                        lijiaosites.Add(lijiaosite);
//                    }
//                }
//            }
//            //Console.WriteLine(lijiaosites.Count());
//            //WriteConsoleLine(lijiaosites);
//        }

//        //Console.WriteLine(lijiaosites.Count());
//        //WriteConsoleLine(lijiaosites);
//    }
//}


        //private static void WriteConsoleLine(List<LiJiaoSite> lijiaosites)
        //{
        //    foreach (var m in lijiaosites)
        //        if (m != null)
        //            Console.WriteLine("{0},,,,,{1},,,,{2}",
        //                m.lj.高架桥ID,
        //                m.lj.名称,
        //                m.site.SiteName
        //                );
        //}
