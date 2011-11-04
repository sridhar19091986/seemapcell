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
        public string ServiceCell = null;
        public string NeighCell = null;
        public int nBaIndex = -1;
        public string nBCCH = null;
        public int nBSIC = -1;
        public int rxLev = -1;
        public int powerControl = -1;
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
