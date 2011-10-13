using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;

namespace Linq2SqlGeography
{
    public class CellCoverage
    {
        private double pathLoss = 141;  //天线输出47，手机最低接受电平-94
        private double frequency = 900;
        private double mobileHight = 1.5;    //移动台高度
        private double verticalBeamwidth = 6;  //垂直波瓣
        private double power;
        private double minrxlev = -94;

        public CellCoverage() { }
        public SqlGeography MergePoint(SITE site)
        {
            //SectorCovarage(6, (double)site.Height, (double)site.Tilt);
            //if (site.SP_GEOMETRY != null)
            //{
            //基站覆盖范围的计算？？？？？
            //Enter Antenna Vertical (3dB) Beamwidth (? =6 °)
            double.TryParse(site.band, out frequency);
            this.power = (double)site.power;
            pathLoss = this.power - this.minrxlev;

            SectorCoverage sc = new SectorCoverage(frequency, (double)site.height, mobileHight, pathLoss);
            
            Console.WriteLine("{0}...{1}...",frequency, sc.DistanceOkumuraHata);
            //sc.SectorPoint = SqlGeometry.Point(0, 0, 4326);
            sc.Latitude = (double)site.latitude;
            sc.Longtitude = (double)site.longitude;
            //Console.WriteLine("{0}...", sc.SectorPoint.Long);
            sc.Direction = (double)site.dir;
            sc.DownTilt = (double)site.tilt;
            //sc.SectorPoint = SqlGeography.STGeomFromWKB(site.SP_GEOMETRY.STAsBinary(), 4326);
            sc.HorizontalBeamwidth = (double)site.ant_bw;
            sc.VerticalBeamwidth = verticalBeamwidth;
            sc.getSectorRadius();
            sc.getSectorCoveragePoint();

            SqlGeography sgeo = SqlGeography.Point(sc.Latitude, sc.Longtitude, 4326);
            //SqlGeometry sgeo = new SqlGeometry();
            Console.WriteLine("{0}...{1}...", site.latitude, site.longitude);
            foreach (var m in sc.STSectorCoverageRegion)
            {
                //Console.WriteLine("{0}...{1}", m.Lat, m.Long);
                //SqlGeometry mgeo=SqlGeometry.STPointFromWKB(m.STAsBinary(),4326);
                sgeo = sgeo.STUnion(m);

            }
            return sgeo;
        }
    }
}
