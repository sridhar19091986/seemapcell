///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-11
///-----------------------------------


namespace Geography.CoordinateSystem
{
    /// <summary>
    /// 时分秒经纬度的方向 东南西北
    /// </summary>
    public enum DMSDirection
    {
        West,
        East,
        North,
        South
    }
    /// <summary>
    /// 时分秒坐标系
    /// </summary>
    public partial class DMS
    {
        /// <summary>
        /// 经度方向
        /// </summary>
        public DMSDirection LongDirection { get; set; }
        /// <summary>
        /// 经度 时
        /// </summary>
        public int LongDD { get; set; }

        /// <summary>
        /// 经度 分
        /// </summary>
        public int LongMM { get; set; }

        /// <summary>
        /// 经度 秒
        /// </summary>
        public int LongSS { get; set; }

        /// <summary>
        /// 维度方向
        /// </summary>
        public DMSDirection LatDirection { get; set; }

        /// <summary>
        /// 纬度 时
        /// </summary>
        public int LatDD { get; set; }

        /// <summary>
        /// 纬度 分
        /// </summary>
        public int LatMM { get; set; }

        /// <summary>
        /// 纬度 秒
        /// </summary>
        public int LatSS { get; set; }

        public override string ToString()
        {
            string LatDir;
            switch(LatDirection)
            {
                case DMSDirection.North:
                    LatDir = "N";
                    break;
                default:
                    LatDir = "S";
                    break;
            }
            string LongDir;
            switch(LongDirection)
            {
                case DMSDirection.East:
                    LongDir = "E";
                    break;
                default:
                    LongDir = "W";
                    break;
            }
            return string.Format("Lat: {1}°{2}'{3}\"{0} Long: {5}°{6}'{7}\"{4}", LatDir, LatDD, LatMM, LatSS, LongDir, LongDD, LongMM, LongSS);
        }
    }
}
