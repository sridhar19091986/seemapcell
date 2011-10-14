using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;

namespace Linq2SqlGeography
{
    class LocatingCellBuffer
    {
        DataClasses2DataContext dc = new DataClasses2DataContext();
        OkumuraHata oh = new OkumuraHata();
        private double mobileHight = 1.5;    //移动台高度

        private string Scell { get; set; }
        private double Srxlev { get; set; }
        private double Spowercontrol { get; set; }

        private double Spowern { get; set; }
        private double SantGain;
        private double Sheight;
        private double Spathloss { get; set; }
        public LocatingCellBuffer(string cell, double rxlev, double pc)
        {
            this.Scell = cell;
            this.Srxlev = rxlev;
            this.Spowercontrol = pc;
        }
        public SqlGeography getScellGeo()
        {
            var s = dc.SITE.Where(e => e.cell == this.Scell).FirstOrDefault();
            if (s == null) return SqlGeography.Point(0, 0, 4236);
            SqlGeography sgeo = SqlGeography.Point((double)s.latitude, (double)s.longitude, 4326);
            double.TryParse(s.ant_gain, out SantGain);
            double.TryParse(s.height, out Sheight);
            Spathloss = oh.PathLoss2Distance((double)s.band, (double)Sheight, (double)mobileHight, (double)(Spowern + SantGain - Spowercontrol - Srxlev));
            SqlGeography Sgeo = sgeo.STBuffer(Spathloss);
            return sgeo;
        }
    }


    //这个可以考虑用缓冲区做定位精度调整

    public class mrLocating
    {
        private List<SqlGeography> lgeo = new List<SqlGeography>();
        public SqlGeography sgeo = new SqlGeography();
        public void sLocating(string cell, double rxlev, double pc)
        {
            LocatingCellBuffer s = new LocatingCellBuffer(cell, rxlev, pc);
            sgeo = s.getScellGeo();
        }
        public void nLocating(string cell, double rxlev, double pc)
        {
            LocatingCellBuffer lc2 = new LocatingCellBuffer(cell, rxlev, pc);
            lgeo.Add(lc2.getScellGeo());
        }
        public void getLocating()
        {
            foreach (var geo in lgeo)
                if (sgeo.STIntersects(geo))
                    sgeo = sgeo.STIntersection(geo);
            sgeo = sgeo.EnvelopeCenter();
        }
    }
}
