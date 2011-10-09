using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Linq2SqlGeography.SqlGeography;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Linq2SqlGeography
{
    class Program
    {
        static void Main(string[] args)
        {

            var point = SqlGeometry.Point(107.04352, 28.870554, 4326);

            Console.WriteLine(point.STX);
            Console.WriteLine(point.STY);
            Console.WriteLine(point.ToString());



            var pointStart = SqlGeometry.Point(107.04352, 28.870554, 4326);
            var pointEnd = SqlGeometry.Point(103.84041, 29.170240, 4326);
            var result = pointStart.STDistance(pointEnd);
            Console.WriteLine("地理距离：" + result + "(米)");

            DataClasses1DataContext dc = new DataClasses1DataContext();
            var geo = dc.sqlspatial;
            var query = from p in geo
                        where p.SP_GEOMETRY !=null
                        let distance = SqlGeometry.Point((double)p.Latitude, (double)p.Longitude, 4326).STBuffer((double)p.Tilt).STArea()
                        //let dif=SqlGeometry.STLineFromWKB(p.SP_GEOMETRY,4326).STBuffer(1).STArea()
                        //where distance < 50
                        select new
                        {
                            p.SiteName,
                            distance,
                            b = p.SP_GEOMETRY.STArea()

                            // p.SP_GEOMETRY.
                        };
            var qq = query.ToList();
            Console.WriteLine(qq.Count());

            //try
            //{
            foreach (var m in qq)
                if (m != null)
                    Console.WriteLine("{0},,,,,{1},,,,{2}", m.SiteName, 0, 1
                        //m.distance.ToSqlDecimal(), m.b
                        );
            //}
            //catch 
            //{
            //    //Console.WriteLine(ex.ToString());
            //}
            //SqlGeography
            //var query = from p in geo
            //            let distance = SqlGeography.Point(p.Latitude, p.Longitude, 4326)
            //             .STDistance(yourLocation)
            //             .Value
            //            where distance <= 50
            //            select p; 

            //SqlGeography.
            Console.ReadKey();
        }
    }
}
