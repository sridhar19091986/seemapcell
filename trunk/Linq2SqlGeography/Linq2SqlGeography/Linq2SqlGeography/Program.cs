using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linq2SqlGeography;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Linq2SqlGeography
{

    class Program
    {
        static void Main(string[] args)
        {
            test();
            DisplayLiJiaoSite();
            Console.ReadKey();
        }

        private static void DisplayLiJiaoSite()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();
            dc.ExecuteCommand("delete  from  CELLTRACING");
            List<LiJiaoSite> lijiaosites = new List<LiJiaoSite>();
            Console.WriteLine(dc.MCOMSITE.Count());
            foreach (var site in dc.MCOMSITE.Take(3))
            {
                //SectorCovarage(6, (double)site.Height, (double)site.Tilt);
                if (site.SP_GEOMETRY != null)
                {
                //基站覆盖范围的计算？？？？？
                //Enter Antenna Vertical (3dB) Beamwidth (? =6 °)
                SectorCoverage sc = new SectorCoverage(900, (double)site.Height, 1.5, 150);
                Console.WriteLine("{0}...", sc.DistanceOkumuraHata);
                //sc.SectorPoint = SqlGeometry.Point(0, 0, 4326);
                sc.lat = (double)site.Latitude;
                sc.lon =(double) site.Longitude;
                //Console.WriteLine("{0}...", sc.SectorPoint.Long);
                sc.AntennaAngle =(double) site.Dir;
                sc.AntennaDownTilt = (double)site.Tilt;
                //sc.SectorPoint = SqlGeography.STGeomFromWKB(site.SP_GEOMETRY.STAsBinary(), 4326);
                sc.HorizontalBeamwidth = (double)site.Ant_BW;
                sc.VerticalBeamwidth = 6;
                sc.getSectorRadius();
                sc.getSectorCoveragePoint();

                SqlGeography sgeo = SqlGeography.Point(sc.lat, sc.lon, 4326);
                //SqlGeometry sgeo = new SqlGeometry();
                Console.WriteLine("{0}...{1}...", site.Latitude,site.Longitude);
                foreach (var m in sc.STSectorCoverageRegion)
                {
                    //Console.WriteLine("{0}...{1}", m.Lat, m.Long);
                    //SqlGeometry mgeo=SqlGeometry.STPointFromWKB(m.STAsBinary(),4326);
                    sgeo=sgeo.STUnion(m);
                    
                }

                SqlGeometry mgeo = SqlGeometry.STGeomFromWKB(sgeo.STAsBinary(), 4326).STConvexHull();

                Console.WriteLine("SqlGeometry   {0}        mgeo.STEnvelope();",
                    mgeo.STAsBinary());

                CELLTRACING ct = new CELLTRACING();
                ct.MI_STYLE = site.SiteName;
                ct.SP_GEOMETRY = mgeo;
                ct.Latitude = site.Latitude;
                ct.Longitude = site.Longitude;

                
                string sql=@" INSERT INTO [SqlSpatialTest].[dbo].[CELLTRACING]
           ([MI_STYLE],[SP_GEOMETRY]) VALUES  ('"+ct.MI_STYLE+"','"+ct.SP_GEOMETRY+"')";

                dc.ExecuteCommand(sql);

                //dc.CELLTRACING.InsertOnSubmit(ct);
               // dc.SubmitChanges();
          
                    var geo = SqlGeography.STGeomFromWKB(site.SP_GEOMETRY.STAsBinary(), 4326);
                    Console.WriteLine(geo.Lat);
                    //SqlGeometry ->SqlGeography
                    var sp = SqlGeography.Point((double)site.Latitude, (double)site.Longitude, 4326);
                    var sitebuffer = sp.STBuffer(50);   //类型转换以后没有问题，单位是米
                    foreach (var lj in dc.立交ABC)
                    {
                        if (lj.SP_GEOMETRY != null)
                        {
                            //SqlGeometry ->SqlGeography
                            var ljp = SqlGeography.STGeomFromWKB(lj.SP_GEOMETRY.STAsBinary(), 4326);

                            /*
                             1.掉话和立交的距离？
                             2.切换失败和立交的距离？
                             3.未接通和立交的距离？
                                         
                                异常事件类型	触发次数
                                SDCCH掉话	71
                                TCH掉话	215
                                未接通	208
                                切换失败	633
                                上行连续弱覆盖	5122
                                下行连续弱覆盖	783
                                上行连续质差	417
                                下行连续质差	510
                             
                             * */

                            //基站和立交的距离？

                            if (ljp.STIntersects(sitebuffer))
                            {
                                LiJiaoSite lijiaosite = new LiJiaoSite();
                                lijiaosite.lj = lj;
                                lijiaosite.site = site;
                                lijiaosites.Add(lijiaosite);
                            }
                        }
                    }
                    Console.WriteLine(lijiaosites.Count());
                    WriteConsoleLine(lijiaosites);
                }
            }
            Console.WriteLine(lijiaosites.Count());
            WriteConsoleLine(lijiaosites);
        }
        private static void WriteConsoleLine(List<LiJiaoSite> lijiaosites)
        {
            foreach (var m in lijiaosites)
                if (m != null)
                    Console.WriteLine("{0},,,,,{1},,,,{2}",
                        m.lj.高架桥ID,
                        m.lj.名称,
                        m.site.SiteName
                        );
        }

        private static void test()
        {
            var g = SqlGeometry.STGeomFromText(new SqlChars("MULTIPOINT(0 0, 13.5 2, 7 19)"), 0);
            Console.WriteLine(g.InstanceOf("GEOMETRYCOLLECTION"));
        }


    }
}
