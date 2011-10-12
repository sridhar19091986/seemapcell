///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-12
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 将DMS坐标系转换成经纬度的类
    /// </summary>
    public class ConversionDMS2DD:IConversion<DMS,DD>
    {
        private  double dms2dd(int dd, int mm, int ss, DMSDirection dirction)
        {
            if (dirction == DMSDirection.West || dirction == DMSDirection.South)
                return 0 - (dd + (double)mm / 60 + (double)ss / 3600);
            else
                return dd + (double)mm / 60 + (double)ss / 3600;
        }

        /// <summary>
        /// 将DMS坐标转换为DD坐标
        /// 例子: (61°26'24''N 25°24'00''E)->(61.44，25.40)
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public DD Convert(DMS dd)
        {
            DD result = new DD();

            result.Longitude = dms2dd(dd.LongDD, dd.LongMM, dd.LongSS, dd.LongDirection);
            result.Latitude = dms2dd(dd.LatDD, dd.LatMM, dd.LatSS, dd.LatDirection);

            return result;
        }
    }
}
