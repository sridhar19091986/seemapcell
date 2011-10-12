///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------

namespace Geography.CoordinateSystem
{
    /// <summary>
    /// 静态类 帮助调用功能
    /// </summary>
    public static class GeographyConvert
    {
        public static DD UTM2DD(UTM utm)
        {
            var Convert = new ConversionUTM2DD();
            return Convert.Convert(utm);
        }
        public static UTM DD2UTM(DD dd)
        {
            var Convert = new ConversionDD2UTM();
            return Convert.Convert(dd);
        }

        public static DD DMS2DD(DMS dms)
        {
            var Convert = new ConversionDMS2DD();
            return Convert.Convert(dms);
        }

        public static DMS DD2DMS(DD dd)
        {
            var Convert = new ConversionDD2DMS();
            return Convert.Convert(dd);
        }

        public static MGRS DD2MGRS(DD dd)
        {
            var Convert = new ConversionDD2MGRS();
            return Convert.Convert(dd);
        }

        public static DD MGRS2DD(MGRS mgrs)
        {
            var Convert = new ConversionMGRS2DD();
            return Convert.Convert(mgrs);
        }
    }
}
