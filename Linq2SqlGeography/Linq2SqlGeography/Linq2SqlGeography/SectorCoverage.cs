using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Geography.CoordinateSystem;

namespace Linq2SqlGeography
{
    public class SectorCoverage : OkumuraHata
    {
        //public SqlGeometry SectorPoint;  //基站位置 天线位置
        public double lat { get; set; }
        public double lon { get; set; } 
        public double AntennaHight { get; set; }  // 天线高度 
        public double AntennaAngle { get; set; }  // 天线方向角
        public double VerticalBeamwidth { get; set; }  //垂直半功率角
        public double HorizontalBeamwidth { get; set; } //水平半功率角
        public double AntennaDownTilt { get; set; } //下倾角

        public List<SqlGeography> STSectorCoverageRegion;  //扇区覆盖区域
        public List<double> STSectorCoverageRadius;  //扇区覆盖区域主要位置


        #region  由传播模型生成点
        public double DistanceOkumuraHata;
        #endregion
        public SectorCoverage(double f, double hb, double hm, double pl)
        {
            //SectorPoint = new SqlGeography();
            STSectorCoverageRegion = new List<SqlGeography>();
            STSectorCoverageRadius = new List<double>();
            this.AntennaHight = hb;
            this.DistanceOkumuraHata = 1000 * PathLoss2Distance(f, hb, hm, pl);
        }

        #region 由天线参数生成 ---->  生成点 ---->  生成覆盖范围
        //距离和经纬度换算
        private SqlGeography getPoint(double angle, double distance)
        {
            //Console.WriteLine(angle);
            //if (angle <= 90) angle = 90 - angle;
            //else angle = 360 - (angle - 90);
            //Console.WriteLine(angle);
            //angle = 360 - angle-90;
            //angle = (angle + 180) % 360;

            //1.You need to convert lat1 and lon1 to radians before calling your function.
        //http://stackoverflow.com/questions/877524/calculating-coordinates-given-a-bearing-and-a-distance

            double x = lat + distance * Math.Cos(angle * Math.PI / 180) / (111 * 1000);
            double y = lon + distance * Math.Sin(angle * Math.PI / 180) / (111 * 1000);
            var p = SqlGeography.Point(x, y, 4326);
            return p;

        }
        /*
        private double getPointY(double angle, double distance)
        {
            //Console.WriteLine(angle);
            //if (angle <= 90) angle = 90 - angle;
            //else angle = 360 - (angle - 90) ;
            //Console.WriteLine(angle);
            var R = 6371; // km
            var d = distance / 1000;
            var brng = angle;

            double lat1 = (double)this.SectorPoint.STStartPoint().Lat;
            double lon1 = (double)this.SectorPoint.STStartPoint().Long;


            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(d / R) + Math.Cos(lat1) * Math.Sin(d / R) * Math.Cos(brng));
            var lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(lat1), Math.Cos(d / R) - Math.Sin(lat1) * Math.Sin(lat2));




            var p = lon2;
            //var p = SqlGeography.Point(dd.Latitude, dd.Longitude, 4326);

            Console.WriteLine("{0},,,{1},,,{2},,,{3}", lat1, lon1, lat2, lon2);
            return p;
        }

        private double getPointX(double angle, double distance)
        {
                        //Console.WriteLine(angle);
            //if (angle <= 90) angle = 90 - angle;
            //else angle = 360 - (angle - 90) ;
            //Console.WriteLine(angle);


            DD dd = new DD();
            dd.Latitude = (double)this.SectorPoint.STStartPoint().Lat;
            dd.Longitude = (double)this.SectorPoint.STStartPoint().Long;
            var utm = GeographyConvert.DD2UTM(dd);
            double lat1 = utm.Easting;
            double lon1 = utm.Northing;
            var R = 6371; // km
            var d = distance / 1000;
            var brng = angle;

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(d / R) + Math.Cos(lat1) * Math.Sin(d / R) * Math.Cos(brng));
            var lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(lat1), Math.Cos(d / R) - Math.Sin(lat1) * Math.Sin(lat2));
            
            
            utm.Easting = lat2;
            utm.Northing = lon2;
            dd = GeographyConvert.UTM2DD(utm);

            var p = dd.Latitude;
            //var p = SqlGeography.Point(dd.Latitude, dd.Longitude, 4326);

            Console.WriteLine("{0},,,{1},,,{2},,,{3}", lat1, lon1, lat2, lon2);
            return p;
        
        }
         * 
         * */

        //半径、生成覆盖半径三元组（点位置，覆盖)  STSectorCoverageRadius
        public void getSectorRadius()
        {
            var vb = this.VerticalBeamwidth;
            var ah = this.AntennaHight;
            var at = this.AntennaDownTilt;

            var Dmain = ah / Math.Tan((at) * Math.PI / 180);
            var Dmin = ah / Math.Tan((at + 0.5 * vb) * Math.PI / 180);
            var Dmax = ah / Math.Tan((at - 0.5 * vb) * Math.PI / 180);

            //由传播模型生成点
            STSectorCoverageRadius.Add(Dmain < DistanceOkumuraHata ? Dmain : DistanceOkumuraHata);
            STSectorCoverageRadius.Add(Dmin < DistanceOkumuraHata ? Dmin : DistanceOkumuraHata);
            STSectorCoverageRadius.Add(Dmax < DistanceOkumuraHata ? Dmax : DistanceOkumuraHata);
        }
        //9个点、生成覆盖半径的9个点      STSectorCoverageRegion
        public void getSectorCoveragePoint()
        {
            //由3个点生成
            foreach (var distance in STSectorCoverageRadius)
            {
                Console.WriteLine("{0}...{1}...{2}", AntennaAngle, HorizontalBeamwidth, distance);
                //一次生成3个点
                STSectorCoverageRegion.Add(getPoint(AntennaAngle + HorizontalBeamwidth / 2, distance));
                STSectorCoverageRegion.Add(getPoint(AntennaAngle, distance));
                STSectorCoverageRegion.Add(getPoint(AntennaAngle - HorizontalBeamwidth / 2, distance));
            }
        }
        #endregion
    }
}
