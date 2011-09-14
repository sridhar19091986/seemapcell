using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellLoadSharing
{
    public class ComputeModel
    {
        //private DataClasses1DataContext dcc = new DataClasses1DataContext();
        public double Variance(IEnumerable<double> ncell)
        {
            List<double> source = ncell.ToList();
            double avg = source.Average();
            double d = source.Aggregate(0.0, (total, next) => total += Math.Pow(next - avg, 2));
            double vd = d;
            return Math.Sqrt(vd) / source.Count();
        }

        public double ConvNullDouble(double? ss)
        {
            if (ss == null) return 0;
            return (double)ss;
        }

       public double ConvNullDouble(int? ss)
        {
            if (ss == null) return 0;
            return (double)ss;
        }
       public List<CellName> Ho_Nrel_Get(IEnumerable<小区切换查询> cdd_nrel)
        {
            var nrelation = cdd_nrel.ToLookup(e => e.小区名);

            List<CellName> nrel = new List<CellName>();
            int thr = 0;
            foreach (var n in nrelation)
            {
                var nreltop = n.OrderByDescending(e => e.切换次数);
                foreach (var nn in nreltop)
                {
                    thr++;
                    if (thr > 5) continue; 
                    CellName cn = new CellName();
                    cn.Cell_name = n.Key;
                    cn.N_cell_name = nn.邻小区名;
                    cn.Handover = nn.切换次数;
                    nrel.Add(cn);
                }
                thr = 0;
            }
            foreach (var n in nrelation)
            {
                CellName cn = new CellName();
                cn.Cell_name = n.Key;
                cn.N_cell_name = n.Key;
                nrel.Add(cn);
            }
            return nrel;
        }

        public List<CellName> Cdd_Nrel_Get(IEnumerable<现网cdd_Nrel> cdd_nrel)
        {
            List<CellName> nrel = new List<CellName>();
            var nrelation = from p in cdd_nrel
                            select new { p.cell_name, p.n_cell_name };
            var nnative = cdd_nrel.Select(e => e.cell_name).Distinct();
            foreach (var n in nrelation)
            {
                CellName cn = new CellName();
                cn.Cell_name = n.cell_name;
                cn.N_cell_name = n.n_cell_name;
                if (cn.Cell_name.Length > 2 && cn.N_cell_name.Length > 2)
                {
                    if (cn.Cell_name.IndexOf(cn.N_cell_name.Substring(0, cn.N_cell_name.Length - 2)) != -1)
                        if (cn.Cell_name.Length == cn.N_cell_name.Length)   //比较长度即可
                            nrel.Add(cn);
                }
            }
            foreach (string n in nnative)
            {
                CellName cn = new CellName();
                cn.Cell_name = n;
                cn.N_cell_name = n;
                nrel.Add(cn);
            }
            return nrel;
        }

        public class CellName
        {
            public string Cell_name { get; set; }
            public string N_cell_name { get; set; }
            public int? Handover { get; set; }
        }
        public int erlangbinv(double p, double? erlang)
        {
            double rho = (double)erlang;
            double B = 1;
            int n = 1;
            while (1 == 1)
            {
                B = ((rho * B) / n) / (1 + rho * B / n);
                if (B <= p)
                    return n;
                n = n + 1;
            }
        }
    }
}
