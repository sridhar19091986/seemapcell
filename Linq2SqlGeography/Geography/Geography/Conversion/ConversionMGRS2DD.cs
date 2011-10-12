///-----------------------------------
///作者: xgluxv@hotmail.com
///日期: 2010-8-13
///-----------------------------------

namespace Geography.CoordinateSystem
{
    using System;
    /// <summary>
    /// MGRS转换成经纬度坐标系
    /// </summary>
    class ConversionMGRS2DD:ConversionBase2DD<MGRS,DD>
    {
        public override DD Convert(MGRS t)
        {
            // 02CNR0634657742
            int zone = t.ZoneLong;
            string latZone = t.ZoneLat;
            string digraph1 = t.Digraph1;
            string digraph2 = t.Digraph2;
            //string easting1 = System.Convert.ToString((int)Math.Round(t.Easting));
            //if (easting1.Length < 5)
            //{
            //    easting1 = "00000" + easting1;
            //}
            //easting1 = easting1.Substring(easting1.Length - 5);

            //string northing1;
            //northing1 = System.Convert.ToString((int)Math.Round(t.Northing));
            //if (northing1.Length < 5)
            //{
            //    northing1 = "0000" + northing1;
            //}
            //northing1 = northing1.Substring(northing1.Length - 5);

            //easting = System.Convert.ToDouble(easting1);
            //northing = System.Convert.ToDouble(northing1);
            easting = t.Easting;
            northing=t.Northing;

            LatZones lz = new LatZones();
            double latZoneDegree = lz.getLatZoneDegree(latZone);

            double a1 = latZoneDegree * 40000000 / 360.0;
            double a2 = 2000000 * Math.Floor(a1 / 2000000.0);

            Digraphs digraphs = new Digraphs();

            double digraph2Index = digraphs.getDigraph2Index(digraph2);

            double startindexEquator = 1;
            if ((1 + zone % 2) == 1)
            {
                startindexEquator = 6;
            }

            double a3 = a2 + (digraph2Index - startindexEquator) * 100000;
            if (a3 <= 0)
            {
                a3 = 10000000 + a3;
            }
            northing = a3 + northing;

            zoneCM = -183 + 6 * zone;
            double digraph1Index = digraphs.getDigraph1Index(digraph1);
            int a5 = 1 + zone % 3;
            double[] a6 = { 16, 0, 8 };
            double a7 = 100000 * (digraph1Index - a6[a5 - 1]);
            easting = easting + a7;

            setVariables();

            double latitude = 0;
            latitude = 180 * (phi1 - fact1 * (fact2 + fact3 + fact4)) / Math.PI;

            if (latZoneDegree < 0)
            {
                latitude = 90 - latitude;
            }

            double d = _a2 * 180 / Math.PI;
            double longitude = zoneCM - d;

            if (getHemisphere(latZone).Equals("S"))
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
