using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    using Geography.CoordinateSystem;
    class Program
    {
        static void Main(string[] args)
        {
            DD dd = new DD()
            {
                Latitude = -45.6456,
                Longitude = 23.3545
            };
            Console.WriteLine("原始数据: " + dd.ToString());
            var dms = GeographyConvert.DD2DMS(dd);
            Console.WriteLine("1.DD2DMS: " + dms.ToString());
            dd = GeographyConvert.DMS2DD(dms);
            Console.WriteLine("2.DMS2DD: " + dd.ToString());
            var utm = GeographyConvert.DD2UTM(dd);
            Console.WriteLine("3.DD2UTM: " + utm.ToString());
            dd = GeographyConvert.UTM2DD(utm);
            Console.WriteLine("4.UTM2DD: " + dd.ToString());
            var mgrs = GeographyConvert.DD2MGRS(dd);
            Console.WriteLine("5.DD2MGRS: " + mgrs.ToString());
            dd = GeographyConvert.MGRS2DD(mgrs);
            Console.WriteLine("6.MGRS2DD: " + dd.ToString());




            //var utm2 = new UTM()
            //{
            //    ZoneLat = "V",
            //    ZoneLong = 35,
            //    Easting = 414668,
            //    Northing = 6812844
            //};
            //Console.WriteLine("原始数据: " + utm2.ToString());
            //var dd2 = GeographyConvert.UTM2DD(utm2);
            //Console.WriteLine("1.UTM2DD: " + dd2.ToString());
            //var mgrs2 = GeographyConvert.DD2MGRS(dd2);
            //Console.WriteLine("2.DD2MGRS: " + mgrs2.ToString());
            //dd2 = GeographyConvert.MGRS2DD(mgrs2);
            //Console.WriteLine("3.MGRS2DD: " + dd2.ToString());


            Console.ReadKey();
        }
    }
}
