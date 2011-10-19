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
        public LocatingCellBuffer(string cell, double rxlev, double powerControl)
        {
            this.sCell = cell;
            this.sRxlev = rxlev;
            this.sPowerControl = powerControl;
        }
        public SqlGeography getCellGeo()
        {
            var s = dc.SITE.Where(e => e.cell == this.sCell).FirstOrDefault();
            if (s == null) return SqlGeography.Point(0, 0, 4236);
            SqlGeography sgeo = SqlGeography.Point((double)s.latitude, (double)s.longitude, 4326);
            double.TryParse(s.ant_gain, out sAntGain);
            double.TryParse(s.height, out sHeight);
            sPathLoss = oh.PathLoss2Distance((double)s.band, (double)sHeight, (double)mobileHight, (double)(sPowerN + sAntGain - sPowerControl - sRxlev));

            Console.WriteLine(sPathLoss * 1000);

            SqlGeography ngeo = sgeo.STBuffer(sPathLoss * 1000);

            SqlGeometry cellgeo = dc.CELLTRACING.Where(e => e.cell == sCell).FirstOrDefault().SP_GEOMETRY;

            SqlGeography cgeo = SqlGeography.STGeomFromWKB(cellgeo.STAsBinary(), 4326);

            Console.WriteLine(ngeo.STArea());
            Console.WriteLine(cgeo.STArea());

            if (cgeo.STArea() > 0)
                return ngeo.STIntersection(cgeo);
            else
                return ngeo;

        }
    }


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

    //这个是从bsic ,frequency获取邻小区

    public class HandleNeighbour
    {
        private ILookup<string, MCOMNEIGH> Neighbours;
        private ILookup<string, MCOMCARRIER> Carriers;
        private Text2Class tc;
        // Define other methods and classes here
        public HandleNeighbour()
        {
            DataClasses2DataContext dc = new DataClasses2DataContext();

            //此处留意数据库中含有空格

            Neighbours = dc.MCOMNEIGH.ToLookup(e => e.Cell.Trim());
            Carriers = dc.MCOMCARRIER.ToLookup(e => e.BCCH.Value.ToString() + "-" + e.BSIC.Trim());
            //只用频率匹配
            //Carriers = dc.MCOMCARRIER.ToLookup(e => e.BCCH.Value.ToString());
            //尝试只用bsic匹配
            //Carriers = dc.MCOMCARRIER.ToLookup(e => e.BSIC.Trim());
            tc = new Text2Class();
        }
        public string getNeighCell(string ServiceCell, string BCCH, string BSIC)
        {
            var bsiccells = Carriers[BCCH + "-" + BSIC].Select(e => e.Cell.Trim());
            //尝试只用bsic匹配
            //if (BSIC == null) return null;
            //只用频率匹配
            //var bsiccells = Carriers[BCCH ].Select(e => e.Cell);
            Console.WriteLine("{0}...{1}", BCCH, BSIC);
            Console.WriteLine("{0}...{1}", bsiccells.Count(), bsiccells.FirstOrDefault());
            var neighcells = Neighbours[ServiceCell].FirstOrDefault();

            //Console.WriteLine("{0}...{1}", neighcells.Cell, neighcells.ncell);
            Regex r = new Regex(@"\s+");
            string ncelllist = r.Replace(neighcells.ncell, @" ");
            var neighcell = ncelllist.Split(new Char[] { ' ' }).ToList();
            //foreach (var m in neighcell)
            //    Console.WriteLine(m);
            var intercell = bsiccells.Intersect(neighcell);

            var ncell = intercell.Count();

            Console.WriteLine(ncell);

            return intercell.FirstOrDefault();
        }

        public string getNeighBSIC(string BSIC)
        {
            //int bsic;
            //int.TryParse(BSIC, out bsic);
            //var ncc = (int)(bsic / 8);
            //var bcc = bsic % 8;
            //string nbsic = ncc.ToString() + bcc.ToString();
            //return nbsic;

            return BSIC;
        }

        //这个方法要要通过BA表获取到之后实现

        public string getNeighBCCH(string ServiceCell, string BaIndex)
        {
            if (BaIndex == null) return null;
            int index = Int32.Parse(BaIndex);
            var balist = tc.CellBaList.Where(e => e.cell == ServiceCell).Where(e => e.mode == "ACTIVE").FirstOrDefault();
            string bcch = balist.ba.ElementAt(index);
            Console.WriteLine("bcch.....{0}...index....{1}", bcch, index);
            return bcch;
        }

        private string tBCCH = null;
        private string tBSIC = null;
        public List<mrNeighbour> getNeighList(List<mrNeighbour> mrNeighs)
        {
            List<mrNeighbour> mrNeighsNew = new List<mrNeighbour>();
            foreach (var n in mrNeighs)
            {
                mrNeighbour mn = new mrNeighbour();

                tBCCH = getNeighBCCH(n.ServiceCell, n.nBaIndex);  // 通过计算获取到邻小区 BCCH
                tBSIC = getNeighBSIC(n.nBSIC);

                Console.WriteLine("bcch.....{0}..", tBCCH);

                mn.ServiceCell = n.ServiceCell;
                mn.NeighCell = getNeighCell(n.ServiceCell, tBCCH, tBSIC);  //通过计算获取到邻小区 名称

                Console.WriteLine("neigbour cell.....{0}..", mn.NeighCell);

                mn.nBaIndex = n.nBaIndex;
                mn.nBCCH = tBCCH;
                mn.nBSIC = tBSIC;
                mn.rxLev = n.rxLev;
                mn.powerControl = n.powerControl;
                mrNeighsNew.Add(mn);
            }
            return mrNeighsNew;
        }

       
        //如何生成单个用户的下属信息？
        //生成连线的问题？

        public List<mrNeighbour> setNeighList()
        {
            List<mrNeighbour> mrneighs = new List<mrNeighbour>();
            mrNeighbour scell = new mrNeighbour("JMJDEY1", -97, null, null, 0); //服务小区只记录功率控制和由dtx转换的接受电平
            mrneighs.Add(scell);
            mrNeighbour n0 = new mrNeighbour("JMJDEY1", -103, "9", "30", 0);
            mrneighs.Add(n0);
            mrNeighbour n1 = new mrNeighbour("JMJDEY1", -106, "1", "53", 0);
            mrneighs.Add(n1);
            mrNeighbour n2 = new mrNeighbour("JMJDEY1", -110, "6", "14", 0);
            mrneighs.Add(n2);
            mrNeighbour n3 = new mrNeighbour("JMJDEY1", -110, "4", "43", 0);
            mrneighs.Add(n3);
            mrNeighbour n4 = new mrNeighbour("JMJDEY1", -110, "0", "65", 0);
            mrneighs.Add(n4);
            mrNeighbour n5 = new mrNeighbour("JMJDEY1", -110, "11", "62", 0);
            mrneighs.Add(n5);
            return mrneighs;
        }

        //直接从数据库提取下述数据生成事件的GIS定位？

        public List<mrNeighbour> setNeighList(MR表格 mr)
        {
            List<mrNeighbour> mrneighs = new List<mrNeighbour>();
            double powercontrol = 0;
            //服务小区只记录功率控制和由dtx转换的接受电平
            if (mr.rxlev_sub_serv_cell != null)
            {
                double.TryParse(mr.powercontrol, out powercontrol);
                if (mr.DTX == null)
                {
                    mrNeighbour scell = new mrNeighbour(
                        mr.cell,
                        double.Parse(mr.rxlev_sub_serv_cell),
                        null, null,
                        powercontrol);
                    mrneighs.Add(scell);
                }
            }
            //邻小区不需要记录功率控制值，只需要测量的电平值即可
            if (mr.rxlev0 != null)
            {
                mrNeighbour n0 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev0), mr.bcch0, mr.bsic0, 0);
                mrneighs.Add(n0);
            }
            if (mr.rxlev1 != null)
            {
                mrNeighbour n1 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev1), mr.bcch1, mr.bsic1, 0);
                mrneighs.Add(n1);
            }
            if (mr.rxlev2 != null)
            {
                mrNeighbour n2 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev2), mr.bcch2, mr.bsic2, 0);
                mrneighs.Add(n2);
            }
            if (mr.rxlev3 != null)
            {
                mrNeighbour n3 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev3), mr.bcch3, mr.bsic3, 0);
                mrneighs.Add(n3);
            }
            if (mr.rxlev4 != null)
            {
                mrNeighbour n4 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev4), mr.bcch4, mr.bsic4, 0);
                mrneighs.Add(n4);
            }
            if (mr.rxlev5 != null)
            {
                mrNeighbour n5 = new mrNeighbour(mr.cell, double.Parse(mr.rxlev5), mr.bcch5, mr.bsic5, 0);
                mrneighs.Add(n5);
            }
            return mrneighs;
        }
    }

    public class mrNeighbour
    {
        public string ServiceCell;
        public string NeighCell = null;
        public string nBaIndex;
        public string nBCCH;
        public string nBSIC;
        public double rxLev;
        public double powerControl;
        public mrNeighbour(string serviceCell, double rxLev, string BaIndex, string nbsic, double powerControl)
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
