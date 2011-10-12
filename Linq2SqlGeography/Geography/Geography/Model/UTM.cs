///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-11
///-----------------------------------

namespace Geography.CoordinateSystem
{
    /// <summary>
    /// UTM 坐标系统使用基于网格的方法表示坐标。UTM 系统将地球分为 60 个区，每个区基于横轴墨卡托投影。
    /// Universal Transverse Mercator，UTM
    /// 维基: http://en.wikipedia.org/wiki/Universal_Transverse_Mercator_coordinate_system
    /// </summary>
    public partial class UTM
    {
        /// <summary>
        /// 区域号 经
        /// </summary>
        public int ZoneLong { get; set; }
        /// <summary>
        /// 区域标识 纬
        /// </summary>
        public string ZoneLat { get; set; }

        public double Easting { get; set; }
        public double Northing { get; set; }

        public override string ToString()
        {
            return ZoneLong.ToString() + " " + ZoneLat + " " + ((int)Easting).ToString() + " " + ((int)Northing).ToString();
        }
    }
}
