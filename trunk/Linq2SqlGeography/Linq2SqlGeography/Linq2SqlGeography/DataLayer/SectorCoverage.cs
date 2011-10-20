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
        public double DistanceOkumuraHata;  //由传播模型生成点

        //public SqlGeometry SectorPoint;  //基站位置 天线位置
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public double Hight { get; set; }  // 天线高度 
        public double Direction { get; set; }  // 天线方向角
        public double DownTilt { get; set; } //下倾角
        public double VerticalBeamwidth { get; set; }  //垂直半功率角
        public double HorizontalBeamwidth { get; set; } //水平半功率角


        public List<SqlGeography> STSectorCoverageRegion;  //扇区覆盖区域
        public List<double> STSectorCoverageRadius;  //扇区覆盖区域主要位置

        public SectorCoverage(double f, double hb, double hm, double pl)
        {
            //SectorPoint = new SqlGeography();
            STSectorCoverageRegion = new List<SqlGeography>();
            STSectorCoverageRadius = new List<double>();
            this.Hight = hb;
            this.DistanceOkumuraHata = 1000 * PathLoss2Distance(f, hb, hm, pl);
        }

        #region 由天线参数生成 ---->  生成点 ---->  生成覆盖范围
        /*
        http://stackoverflow.com/questions/877524/calculating-coordinates-given-a-bearing-and-a-distance
        1.You need to convert lat1 and lon1 to radians before calling your function.
         * */
        //距离和经纬度换算
        private double px;
        private double py;
        private SqlGeography psgeog;

        private SqlGeography getPoint(double angle, double distance)
        {
            px = Latitude + distance * Math.Cos(angle * Math.PI / 180) / (111 * 1000);
            py = Longtitude + distance * Math.Sin(angle * Math.PI / 180) / (111 * 1000);
            psgeog = SqlGeography.Point(px, py, 4326);
            return psgeog;
        }

        //半径、生成覆盖半径三元组（点位置，覆盖)  STSectorCoverageRadius
        private double vb;
        private double ah;
        private double at;
        private double dmain;
        private double dmin;
        private double dmax;

        public void getSectorRadius()
        {
            vb = this.VerticalBeamwidth;
            ah = this.Hight;
            at = this.DownTilt;

            dmain = ah / Math.Tan((at) * Math.PI / 180);
            dmin = ah / Math.Tan((at + 0.5 * vb) * Math.PI / 180);
            dmax = ah / Math.Tan((at - 0.5 * vb) * Math.PI / 180);

            //由传播模型生成点
            if (dmain > 0)
                STSectorCoverageRadius.Add(dmain < DistanceOkumuraHata ? dmain : DistanceOkumuraHata);
            if (dmin > 0)
                STSectorCoverageRadius.Add(dmin < DistanceOkumuraHata ? dmin : DistanceOkumuraHata);
            if (dmax > 0)
                STSectorCoverageRadius.Add(dmax < DistanceOkumuraHata ? dmax : DistanceOkumuraHata);
        }
        //9个点、生成覆盖半径的9个点      STSectorCoverageRegion
        public void getSectorCoveragePoint()
        {
            //由3个点生成
            foreach (var d in STSectorCoverageRadius)
            {
                //一次生成3个点
                STSectorCoverageRegion.Add(getPoint(Direction + HorizontalBeamwidth / 2, d));
                STSectorCoverageRegion.Add(getPoint(Direction, d));
                STSectorCoverageRegion.Add(getPoint(Direction - HorizontalBeamwidth / 2, d));
            }
        }
        #endregion
    }
}
