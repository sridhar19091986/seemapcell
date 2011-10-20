using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    class LocatingCellBuffer
    {
        DataClasses2DataContext dc = new DataClasses2DataContext();
        OkumuraHata oh = new OkumuraHata();
        private double mobileHight = 1.5;    //移动台高度

        private string sCell { get; set; }
        private double sRxlev { get; set; }
        private double sPowerControl { get; set; }

        private double sPowerN { get; set; }
        private double sAntGain;
        private double sHeight;
        private double sPathLoss { get; set; }

        private SqlGeography sgeo;
        //private SqlGeometry mgeo;
        private SqlGeography ngeo;
        private SITE s;
        private CellTracing abc;
        private SqlGeometry cellgeo;
        private SqlGeography cgeo;

        public LocatingCellBuffer(string cell, double rxlev, double powerControl)
        {
            this.sCell = cell;
            this.sRxlev = rxlev;
            this.sPowerControl = powerControl;
        }
        public SqlGeography getCellGeo()
        {
            s = dc.SITE.Where(e => e.cell == this.sCell).FirstOrDefault();
            if (s == null) return SqlGeography.Point(0, 0, 4236);
            sgeo = SqlGeography.Point((double)s.latitude, (double)s.longitude, 4326);
            double.TryParse(s.ant_gain, out sAntGain);
            double.TryParse(s.height, out sHeight);
            sPathLoss = oh.PathLoss2Distance((double)s.band, (double)sHeight, (double)mobileHight, (double)(sPowerN + sAntGain - sPowerControl - sRxlev));

            Console.WriteLine(sPathLoss * 1000);

            ngeo = sgeo.STBuffer(sPathLoss * 1000);

            abc = dc.CellTracing.Where(e => e.cell == sCell).FirstOrDefault();

            //if (abc == null) return ngeo;

            cellgeo = abc.SP_GEOMETRY;

            cgeo = SqlGeography.STGeomFromWKB(cellgeo.STAsBinary(), 4326);

            Console.WriteLine(ngeo.STArea());
            Console.WriteLine(cgeo.STArea());

            if (cgeo.STArea() > 0)
                return ngeo.STIntersection(cgeo);
            else
                return ngeo;

        }
    }
}
