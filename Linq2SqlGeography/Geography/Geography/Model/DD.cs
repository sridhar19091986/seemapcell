///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-11
///-----------------------------------


namespace Geography.CoordinateSystem
{
    /// <summary>
    /// 经纬度坐标系 使用正负表示
    /// </summary>
    public partial class DD
    {
        /// <summary>
        /// 精度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        public override string ToString()
        {
            return string.Format("Lat: {0} Long: {1}", Latitude, Longitude);
        }
    }
}
