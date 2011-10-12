///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// UTM转换成经纬度坐标系
    /// </summary>
    public class ConversionUTM2DD:ConversionBase2DD<UTM,DD>
    {
        public override DD Convert(UTM t)
        {
            string hemisphere = getHemisphere(t.ZoneLat);
            zone = t.ZoneLong;
            easting = t.Easting;
            northing = t.Northing;
            double latitude = 0.0;
            double longitude = 0.0;
            if (hemisphere.Equals("S"))
            {
                northing = 10000000 - northing;
            }
            setVariables();
            latitude = 180 * (phi1 - fact1 * (fact2 + fact3 + fact4)) / Math.PI;
            if (zone > 0)
            {
                zoneCM = 6 * zone - 183.0;
            }
            else
            {
                zoneCM = 3.0;
            }
            longitude = zoneCM - _a3;
            if (hemisphere.Equals("S"))
            {
                latitude = -latitude;
            }
            return new DD()
            {
                Latitude = latitude,
                Longitude = longitude
            };
        }
    }
}
