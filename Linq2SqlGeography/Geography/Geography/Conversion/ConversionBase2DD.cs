///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------


namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 将UTM/MGRS转换成经纬度坐标系的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRsult"></typeparam>
    public abstract class ConversionBase2DD<T, TResult> : IConversion<T, TResult>
    {
        #region 变量
        protected double arc;
        protected double mu;
        protected double ei;
        protected double ca;
        protected double cb;
        protected double cc;
        protected double cd;
        protected double n0;
        protected double r0;
        protected double _a1;
        protected double dd0;
        protected double t0;
        protected double Q0;
        protected double lof1;
        protected double lof2;
        protected double lof3;
        protected double _a2;
        protected double phi1;
        protected double fact1;
        protected double fact2;
        protected double fact3;
        protected double fact4;
        protected double zoneCM;
        protected double _a3;
        protected double b = 6356752.314;
        protected double a = 6378137;
        protected double e = 0.081819191;
        protected double e1sq = 0.006739497;
        protected double k0 = 0.9996;
        #endregion

        protected double easting;
        protected double northing;
        protected int zone;
        string southernHemisphere = "ACDEFGHJKLM";

        protected string getHemisphere(string latZone)
        {
            string hemisphere = "N";
            if (southernHemisphere.IndexOf(latZone) > -1)
            {
                hemisphere = "S";
            }
            return hemisphere;
        }
        public abstract TResult Convert(T t);
        #region 数学公式 Sin Cos Pow Tan
        protected static double Pow(double x, double y)
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

        /// <summary>
        /// 设置变量值
        /// </summary>
        protected void setVariables()
        {
            arc = northing / k0;
            mu = arc/ (a * (1 - Pow(e, 2) / 4.0 - 3 * Pow(e, 4) / 64.0 - 5 * Pow(e, 6) / 256.0));

            ei = (1 - Pow((1 - e * e), (1 / 2.0)))
                / (1 + Pow((1 - e * e), (1 / 2.0)));

            ca = 3 * ei / 2 - 27 * Pow(ei, 3) / 32.0;

            cb = 21 * Pow(ei, 2) / 16 - 55 * Pow(ei, 4) / 32;
            cc = 151 * Pow(ei, 3) / 96;
            cd = 1097 * Pow(ei, 4) / 512;
            phi1 = mu + ca * Sin(2 * mu) + cb * Sin(4 * mu) + cc * Sin(6 * mu) + cd
                * Sin(8 * mu);

            n0 = a / Pow((1 - Pow((e * Sin(phi1)), 2)), (1 / 2.0));

            r0 = a * (1 - e * e) / Pow((1 - Pow((e * Sin(phi1)), 2)), (3 / 2.0));
            fact1 = n0 * Tan(phi1) / r0;

            _a1 = 500000 - easting;
            dd0 = _a1 / (n0 * k0);
            fact2 = dd0 * dd0 / 2;

            t0 = Pow(Tan(phi1), 2);
            Q0 = e1sq * Pow(Cos(phi1), 2);
            fact3 = (5 + 3 * t0 + 10 * Q0 - 4 * Q0 * Q0 - 9 * e1sq) * Pow(dd0, 4)
                / 24;

            fact4 = (61 + 90 * t0 + 298 * Q0 + 45 * t0 * t0 - 252 * e1sq - 3 * Q0
                * Q0)
                * Pow(dd0, 6) / 720;

            //
            lof1 = _a1 / (n0 * k0);
            lof2 = (1 + 2 * t0 + Q0) * Pow(dd0, 3) / 6.0;
            lof3 = (5 - 2 * Q0 + 28 * t0 - 3 * Pow(Q0, 2) + 8 * e1sq + 24 * Pow(t0, 2))
                * Pow(dd0, 5) / 120;
            _a2 = (lof1 - lof2 + lof3) / Cos(phi1);
            _a3 = _a2 * 180 / Math.PI;

        }
    }



}
