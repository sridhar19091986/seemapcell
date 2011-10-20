using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    //这个可以考虑用缓冲区做定位精度调整

    public class mrLocating
    {
        private List<SqlGeography> lgeo = new List<SqlGeography>();
        public SqlGeography sgeo = new SqlGeography();
        public SqlGeography ngeo = new SqlGeography();
        public mrLocating()
        {
        }
        public SqlGeography sLocating(string serviceCell, double rxlev, double powerControl)
        {
            LocatingCellBuffer s = new LocatingCellBuffer(serviceCell, rxlev, powerControl);
            sgeo = s.getCellGeo();
            return sgeo;
        }
        public SqlGeography nLocating(string neighCell, double rxlev, double powerControl)
        {
            LocatingCellBuffer lc2 = new LocatingCellBuffer(neighCell, rxlev, powerControl);
            ngeo = lc2.getCellGeo();
            return ngeo;
            //lgeo.Add(lc2.getCellGeo());
        }
        public void getLocating()
        {
            foreach (var geo in lgeo)
            {
                // Console.WriteLine(sgeo.STIntersects(geo));
                //if (sgeo.STIntersects(geo))
                //   sgeo = sgeo.STIntersection(geo);
                sgeo = sgeo.STUnion(geo);

            }
            //Console.WriteLine(sgeo.STArea());
            //sgeo = sgeo.EnvelopeCenter();
        }
    }

   

}
