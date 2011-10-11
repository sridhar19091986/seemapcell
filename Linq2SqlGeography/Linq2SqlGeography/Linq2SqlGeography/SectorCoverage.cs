using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;

namespace Linq2SqlGeography
{
    class SectorCoverage
    {
        public SqlGeography

        private  SqlGeography STSectorCoverage(SqlGeography AntennaPoint, double AntennaHight, double AntennaAngle,
    double VerticalBeamwidth, double HorizontalBeamwidth, double AntennaDownTilt)
        {
            var mainp=SqlGeography.st
            //var q=SqlGeography.Point(0,0，4326）；
            /*
             * capture
catch
seize
             * */
            return AntennaPoint;
        }

        #region  由传播模型生成点

        #endregion

        #region 由天线参数生成 ---->  生成点 ---->  生成覆盖范围
        public  SqlGeography CatchPoint(double lat, double lon, double angle, double distance)
        {
            double x = lat + distance / (60 * Math.Cos(lat) * Math.Cos(360 - angle + 90));
            double y = lon + (distance / 60) * Math.Sin(360 - angle + 90);
            var point = SqlGeography.Point(x, y, 4326);
            return point;
        }

        //距离和经纬度换算

        private  SqlGeography getPoint(SqlGeography center, double angle, double distance)
        {
            //Angles in java are measured clockwise from 3 o'clock.         
            //double theta = Math.(angle);        
            //Point2D.Double p = new Point2D.Double();  
            //var p=new SqlGeography();
            double x = (double)center.Lat + distance * Math.Cos(angle) / 110;
            double y = (double)center.Long + distance * Math.Sin(angle) / 110;
            var p = SqlGeography.Point(x, y, 4326);
            return p;
        }

        private  void getSectorRadius(double VerticalBeamwidth, double AntennaHight, double AntennaDownTilt)
        {
            var Dmain = AntennaHight / Math.Tan((AntennaDownTilt) * Math.PI / 180);
            var Dmin = AntennaHight / Math.Tan((AntennaDownTilt + 0.5 * VerticalBeamwidth) * Math.PI / 180);
            var Dmax = AntennaHight / Math.Tan((AntennaDownTilt - 0.5 * VerticalBeamwidth) * Math.PI / 180);
            Console.WriteLine("{0}...{1}...{2}...{3}...{4}", AntennaHight, AntennaDownTilt, Dmain, Dmin, Dmax);
            //=IF(RC[-2]>R1C4/2,R2C4/TAN((RC[-2]-0.5*R1C4)*PI()/180),"infinite")
            //var Dmin=SqlGeography.Point(AntennaPoint.Lat
            //var Dmax=
            var mainp=getPoint(
        }
        #endregion


    

    }
}
