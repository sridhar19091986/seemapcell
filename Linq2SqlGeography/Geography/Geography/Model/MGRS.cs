///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-11
///-----------------------------------


namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 军事格网参考系
    /// MGRS 是北约（NATO）军事组织使用的标准坐标系统。MGRS 以 UTM 为基础并进一步将每个区划分为 100 km × 100 km 的小方块。
    /// 这些方块使用两个相连的字母标识：第一个字母表示经度区的东西位置，而第二个字母表示南北位置。 
    /// 
    /// 
    /// 
    /// 例如，UTM 点 35 V 414668 6812844 等价于 MGRS 点 35VMJ1466812844。
    /// 该 MGRS 点精度为米，使用 15 个字符表示，其中最后 10 个字符表示指定网格中的以东和以北的值。
    /// 可以使用 15 个字符表示 MGRS 值（如前例），也可表示为 13、11、9 或 7 个字符；因此，所表示的值的精度分别为 1 米、10 米、100 米、1,000 米或 10,000 米。 
    /// </summary>
    public partial class MGRS
    {
        /// <summary>
        /// 区域号 经
        /// </summary>
        public int ZoneLong { get; set; }
        /// <summary>
        /// 区域标识 纬
        /// </summary>
        public string ZoneLat { get; set; }

        public string Digraph1 { get; set; }

        public string Digraph2 { get; set; }

        public double Easting { get; set; }

        public double Northing { get; set; }

        public override string ToString()
        {
            //string easting = Convert.ToString((int)Math.Round(Easting));
            //if (easting.Length < 5)
            //{
            //    easting = "00000" + easting;
            //}
            //easting = easting.Substring(easting.Length - 5);

            //string northing;
            //northing = Convert.ToString((int)Math.Round(Northing));
            //if (northing.Length < 5)
            //{
            //    northing = "0000" + northing;
            //}
            //northing = northing.Substring(northing.Length - 5);
            string easting = Convert.ToString((int)Math.Round(Easting));
            string northing = Convert.ToString((int)Math.Round(Northing));
            return ZoneLong.ToString () + ZoneLat + Digraph1 + Digraph2 + easting + northing;
        }

    }
}
