using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{

    public class mrNeighbour
    {
        public string ServiceCell;
        public string NeighCell = null;
        public int nBaIndex;
        public string nBCCH;
        public int nBSIC;
        public int rxLev;
        public int powerControl;
        public mrNeighbour(string serviceCell, int rxLev, int BaIndex, int nbsic, int powerControl)
        {
            this.ServiceCell = serviceCell;
            //this.NeighCell = neighCell;
            this.nBaIndex = BaIndex;
            //this.nBCCH = BaIndex;
            this.nBSIC = nbsic;
            this.rxLev = rxLev;
            this.powerControl = powerControl;
        }
        public mrNeighbour()
        {
        }
    }
}
