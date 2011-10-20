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
        private List<SqlGeography> listgeog = new List<SqlGeography>();
        public SqlGeography servicecellgeog = new SqlGeography();
        public SqlGeography neighcellgeog = new SqlGeography();
        public mrLocating()
        {
        }
        public SqlGeography sLocating(string serviceCell, double rxlev, double powerControl)
        {
            LocatingCellBuffer ser = new LocatingCellBuffer(serviceCell, rxlev, powerControl);
            servicecellgeog = ser.getCellGeo();
            return servicecellgeog;
        }
        public SqlGeography nLocating(string neighCell, double rxlev, double powerControl)
        {
            LocatingCellBuffer neigh = new LocatingCellBuffer(neighCell, rxlev, powerControl);
            neighcellgeog = neigh.getCellGeo();
            return neighcellgeog;
            //lgeo.Add(lc2.getCellGeo());
        }
        public void getLocating()
        {
            foreach (var geog in listgeog)
            {
                // Console.WriteLine(sgeo.STIntersects(geo));
                //if (sgeo.STIntersects(geo))
                //   sgeo = sgeo.STIntersection(geo);
                servicecellgeog = servicecellgeog.STUnion(geog);

            }
            //Console.WriteLine(sgeo.STArea());
            //sgeo = sgeo.EnvelopeCenter();
        }
    }

   

}
