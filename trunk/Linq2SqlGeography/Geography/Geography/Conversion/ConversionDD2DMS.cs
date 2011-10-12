///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-12
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 将经纬度转换成DMS坐标系的类
    /// </summary>
    public class ConversionDD2DMS:IConversion<DD,DMS>
    {
        #region private method
        private void dd2dms(double DD, out int dd, out int mm, out int ss)
        {
            DD = DD < 0 ? 0 - DD : DD;
            dd = (int)Math.Floor(DD);
            double ff = DD - dd;
            double mmgg = ff * 60;
            mm = (int)Math.Floor(mmgg);
            double gg = mmgg - mm;
            ss = (int)Math.Floor(60 * gg);
        }

        /// <summary>
        /// 验证经纬度是否合法
        /// </summary>
        /// <param name="dd"></param>
        private  void Validation(DD dd)
        {
            if (dd.Latitude < -90.0 || dd.Latitude > 90.0 || dd.Longitude < -180.0 || dd.Longitude >= 180.0)
            {
                throw new ArgumentOutOfRangeException("DD", "Legal ranges: latitude [-90,90], longitude [-180,180).");
            }

        }
        #endregion
        /// <summary>
        /// 将DD坐标转换为DMS坐标
        /// 例子: (61.44，25.40)->(61°26'24''N 25°24'00''E)
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public DMS Convert(DD dd)
        {
            int longdd, longmm, longss, latdd, latmm, latss;
            Validation(dd);
            DMS dms = new DMS();
            dd2dms(dd.Longitude, out longdd, out longmm, out longss);
            dd2dms(dd.Latitude, out latdd, out latmm, out latss);
            if (dd.Longitude < 0)
                dms.LongDirection = DMSDirection.West;
            else
                dms.LongDirection = DMSDirection.East;
            dms.LongDD = longdd;
            dms.LongMM = longmm;
            dms.LongSS = longss;

            if (dd.Latitude < 0)
                dms.LatDirection = DMSDirection.South;
            else
                dms.LatDirection = DMSDirection.North;

            dms.LatDD = latdd;
            dms.LatMM = latmm;
            dms.LatSS = latss;
            return dms;
        }
    }
}
