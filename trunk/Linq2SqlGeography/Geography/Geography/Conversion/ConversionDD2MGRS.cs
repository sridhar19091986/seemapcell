///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-12
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// 将经纬度转换成MGRS坐标系的类
    /// </summary>
    public class ConversionDD2MGRS :ConversionBaseDD2<DD,MGRS>
    {
        public override  MGRS Convert(DD dd)
        {
            Validation(dd);
            setVariables(dd.Latitude, dd.Longitude);
            LatZones latZones = new LatZones();
            Digraphs digraphs = new Digraphs();
            int longZone=getLongZone(dd.Longitude);
            double _easting=getEasting();
            double _northing=getNorthing(dd.Latitude);

            string easting1 = System.Convert.ToString((int)Math.Round(_easting));
            if (easting1.Length < 5)
            {
                easting1 = "00000" + easting1;
            }
            easting1 = easting1.Substring(easting1.Length - 5);

            string northing1;
            northing1 = System.Convert.ToString((int)Math.Round(_northing));
            if (northing1.Length < 5)
            {
                northing1 = "0000" + northing1;
            }
            northing1 = northing1.Substring(northing1.Length - 5);


            return new MGRS()
            {
                Digraph1 = digraphs.getDigraph1(longZone,_easting),
                Digraph2 = digraphs.getDigraph2(longZone,_northing),
                Easting =System.Convert.ToDouble (easting1),
                Northing = System.Convert.ToDouble (northing1),
                ZoneLat = latZones.getLatZone(dd.Latitude),
                ZoneLong = longZone
            };
        }

    }
}
