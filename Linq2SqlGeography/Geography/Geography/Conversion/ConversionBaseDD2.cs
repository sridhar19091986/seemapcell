///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 将经纬度坐标系转换成UTM/MGRS的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class ConversionBaseDD2<T,TResult>:IConversion<T,TResult>
    {
        #region DD to UTM/MGRS variables

        // equatorial radius
        protected const double equatorialRadius = 6378137;

        // polar radius
        protected const double polarRadius = 6356752.314;

        // flattening
        protected const double flattening = 0.00335281066474748;// (equatorialRadius-polarRadius)/equatorialRadius;

        // inverse flattening 1/flattening
        protected const double inverseFlattening = 298.257223563;// 1/flattening;

        // Mean radius
        protected readonly double rm;// = Pow(equatorialRadius * polarRadius, 1 / 2.0);

        // scale factor
        protected const double k0 = 0.9996;

        // eccentricity
        protected readonly double e;// = Math.Sqrt(1 - Pow(polarRadius / equatorialRadius, 2));

        protected readonly double e1sq;// = e * e / (1 - e * e);

        protected readonly double n;// = (equatorialRadius - polarRadius) / (equatorialRadius + polarRadius);

        // r curv 1
        protected double rho = 6368573.744;

        // r curv 2
        protected double nu = 6389236.914;

        // Calculate Meridional Arc Length
        // Meridional Arc
        protected double S = 5103266.421;

        protected const double A0 = 6367449.146;

        protected const double B0 = 16038.42955;

        protected const double C0 = 16.83261333;

        protected const double D0 = 0.021984404;

        protected const double E0 = 0.000312705;

        // Calculation Constants
        // Delta Long
        protected double p = -0.483084;

        protected const double sin1 = 4.84814E-06;

        // Coefficients for UTM Coordinates
        protected double K1 = 5101225.115;

        protected double K2 = 3750.291596;

        protected double K3 = 1.397608151;

        protected double K4 = 214839.3105;

        protected double K5 = -2.995382942;

        protected double A6 = -1.00541E-07;
        #endregion
        #region Protected method
        /// <summary>
        /// 验证经纬度是否合法
        /// </summary>
        /// <param name="dd"></param>
        protected void Validation(DD dd)
        {
            if (dd.Latitude < -90.0 || dd.Latitude > 90.0 || dd.Longitude < -180.0 || dd.Longitude >= 180.0)
            {
                throw new ArgumentOutOfRangeException("DD", "Legal ranges: latitude [-90,90], longitude [-180,180).");
            }

        }
        /// <summary>
        /// 度转化为弧度
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        protected double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
        /// <summary>
        /// 弧度转为度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        protected double RadianToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }
        #endregion
        #region 数学公式 sin cos pow tan
        protected static  double Pow(double x,double y)
        {
            return Math.Pow(x, y);
        }

        protected static double Sin(double value)
        {
            return Math.Sin(value);
        }

        protected static double Cos(double value)
        {
            return Math.Cos(value);
        }

        protected static double Tan(double value)
        {
            return Math.Tan(value);
        }
        #endregion

        #region dd to utm/MGRS method      
        protected virtual void setVariables(double latitude, double longitude)
        {
            latitude = DegreeToRadian(latitude);
            rho = equatorialRadius * (1 - e * e)
                / Pow(1 - Pow(e * Sin(latitude), 2), 3 / 2.0);

            nu = equatorialRadius / Pow(1 - Pow(e * Sin(latitude), 2), (1 / 2.0));

            double var1;
            if (longitude < 0.0)
            {
                var1 = ((int)((180 + longitude) / 6.0)) + 1;
            }
            else
            {
                var1 = ((int)(longitude / 6)) + 31;
            }
            double var2 = (6 * var1) - 183;
            double var3 = longitude - var2;
            p = var3 * 3600 / 10000;

            S = A0 * latitude - B0 * Sin(2 * latitude) + C0 * Sin(4 * latitude) - D0
                * Sin(6 * latitude) + E0 * Sin(8 * latitude);

            K1 = S * k0;
            K2 = nu * Sin(latitude) * Cos(latitude) * Pow(sin1, 2) * k0 * (100000000)
                / 2;
            K3 = ((Pow(sin1, 4) * nu * Sin(latitude) *Pow(Cos(latitude), 3)) / 24)
                * (5 - Pow(Tan(latitude), 2) + 9 * e1sq * Pow(Cos(latitude), 2) + 4
                    * Pow(e1sq, 2) * Pow(Cos(latitude), 4))
                * k0
                * (10000000000000000L);

            K4 = nu * Cos(latitude) * sin1 * k0 * 10000;

            K5 = Pow(sin1 * Cos(latitude), 3) * (nu / 6)
                * (1 - Pow(Tan(latitude), 2) + e1sq * Pow(Cos(latitude), 2)) * k0
                * 1000000000000L;

            A6 = (Pow(p * sin1, 6) * nu * Sin(latitude) * Pow(Cos(latitude), 5) / 720)
                * (61 - 58 * Pow(Tan(latitude), 2) + Pow(Tan(latitude), 4) + 270
                    * e1sq * Pow(Cos(latitude), 2) - 330 * e1sq
                    * Pow(Sin(latitude), 2)) * k0 * (1E+24);

        }
        protected int getLongZone(double longitude)
        {
            double longZone = 0;
            if (longitude < 0.0)
            {
                longZone = ((180.0 + longitude) / 6) + 1;
            }
            else
            {
                longZone = (longitude / 6) + 31;
            }
            return (int)longZone;
            //String val = Convert.ToString((int)longZone);
            //if (val.Length == 1)
            //{
            //    val = "0" + val;
            //}
            //return val;
        }
        protected double getNorthing(double latitude)
        {
            double northing = K1 + K2 * p * p + K3 * Pow(p, 4);
            if (latitude < 0.0)
            {
                northing = 10000000 + northing;
            }
            return northing;
        }
        protected double getEasting()
        {
            return 500000 + (K4 * p + K5 * Pow(p, 3));
        }
        #endregion

        public ConversionBaseDD2()
        {
            rm = Pow(equatorialRadius * polarRadius, 1 / 2.0);
            e = Math.Sqrt(1 - Pow(polarRadius / equatorialRadius, 2));
            n = (equatorialRadius - polarRadius) / (equatorialRadius + polarRadius);
            e1sq = e * e / (1 - e * e);
        }
        public abstract TResult Convert(T t);
    }
}
