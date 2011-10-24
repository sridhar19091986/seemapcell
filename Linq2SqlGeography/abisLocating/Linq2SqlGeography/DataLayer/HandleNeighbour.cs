using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using Linq2SqlGeography.LinqSql;
using System.Text.RegularExpressions;

namespace Linq2SqlGeography
{
    //这个是从bsic ,frequency获取邻小区

    public class HandleNeighbour
    {
        private ILookup<string, MCOMNEIGH> Neighbours;
        private ILookup<string, MCOMCARRIER> Carriers;
        private Text2Class tc;

        private string bcch;  //frequency 变量
        private int baIndex;  //ba表索引
        private CellBA baList;  //ba表

        private string tBCCH = null;  //fequency 变量
        private int tBSIC = 0;  

        private int powercontrol = 0;

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
            Console.WriteLine("bcch...{0}...bsic...{1}", BCCH, BSIC);
            Console.WriteLine("{0}...{1}", bsiccells.Count(), bsiccells.FirstOrDefault());
            var neighcells = Neighbours[ServiceCell].FirstOrDefault();

            //Console.WriteLine("{0}...{1}", neighcells.Cell, neighcells.ncell);
            if (neighcells == null) return null;

            Regex r = new Regex(@"\s+");
            string ncelllist = r.Replace(neighcells.ncell, @" ");

            var neighcell = ncelllist.Split(new Char[] { ' ' }).ToList();
            //foreach (var m in neighcell)
            //    Console.WriteLine(m);
            var intercell = bsiccells.Intersect(neighcell);

            var ncell = intercell.Count();

            //Console.WriteLine(ncell);
            Console.WriteLine("find ....cell...{0}", ncell);

            return intercell.FirstOrDefault();
        }

        //mtrix 读入的都是10进制  ？  转换成8 进制
        public int getNeighBSIC(int BSIC)
        {
            if (BSIC == -1) return -1;

            int bccncc = Int32.Parse(Convert.ToString(BSIC, 8));

            Console.WriteLine("bsic....{0}...bccncc....{1}", BSIC, bccncc);

            return bccncc;
        }


        //这个方法要要通过BA表获取到之后实现

        public string getNeighBCCH(string ServiceCell, int BaIndex)
        {
            if (BaIndex == -1) return null;
            //int index = Int32.Parse(BaIndex);
            baIndex = BaIndex;
            baList = tc.CellBaList.Where(e => e.cell == ServiceCell).Where(e => e.mode == "ACTIVE").FirstOrDefault();

            if (baList == null) return null;
            if (baList.ba.Count() < BaIndex + 1) return null;

            bcch = baList.ba.ElementAt(baIndex);
            Console.WriteLine("bcch.....{0}...index....{1}", bcch, baIndex);
            return bcch;
        }

        //考虑用ref ，避免在内存生成多余的对象 ,但是似乎是一个集合，在迭代过程中不能修改？？

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
                mn.NeighCell = getNeighCell(n.ServiceCell, tBCCH, tBSIC.ToString());  //通过计算获取到邻小区 名称

                Console.WriteLine("neigbour cell....cell....{0}..bcch...{1}...bsic...{2}", mn.NeighCell, tBCCH, tBSIC);

                Console.WriteLine("--------------------------");

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
            mrNeighbour scell = new mrNeighbour("JMJDEY1", -97, 0, 0, 0); //服务小区只记录功率控制和由dtx转换的接受电平
            mrneighs.Add(scell);
            mrNeighbour n0 = new mrNeighbour("JMJDEY1", -103, 9, 30, 0);
            mrneighs.Add(n0);
            mrNeighbour n1 = new mrNeighbour("JMJDEY1", -106, 1, 53, 0);
            mrneighs.Add(n1);
            mrNeighbour n2 = new mrNeighbour("JMJDEY1", -110, 6, 14, 0);
            mrneighs.Add(n2);
            mrNeighbour n3 = new mrNeighbour("JMJDEY1", -110, 4, 43, 0);
            mrneighs.Add(n3);
            mrNeighbour n4 = new mrNeighbour("JMJDEY1", -110, 0, 65, 0);
            mrneighs.Add(n4);
            mrNeighbour n5 = new mrNeighbour("JMJDEY1", -110, 11, 62, 0);
            mrneighs.Add(n5);
            return mrneighs;
        }

        //直接从数据库提取下述数据生成事件的GIS定位？


        public List<mrNeighbour> setNeighList(Abis_MR mr)
        {
            List<mrNeighbour> mrneighs = new List<mrNeighbour>();
            powercontrol = 0;
            int.TryParse(mr.bs_power.Replace("Pn", ""), out powercontrol);  //功率去掉pn即可
            //服务小区只记录功率控制和由dtx转换的接受电平
            if (mr.dtx_used == "Set")
            {
                mrNeighbour scell = new mrNeighbour(mr.cell, (int)mr.rxlev_sub_serv_cell - 110, -1, -1, powercontrol);
                mrneighs.Add(scell);
            }
            else
            {
                mrNeighbour scell = new mrNeighbour(mr.cell, (int)mr.rxlev_full_serv_cell - 110, -1, -1, powercontrol);
                mrneighs.Add(scell);
            }

            //邻小区不需要记录功率控制值，只需要测量的电平值即可
            if (mr.bsic0 != 0)
            {
                mrNeighbour n0 = new mrNeighbour(mr.cell, (int)mr.rxlev0, (int)mr.bcch0, (int)mr.bsic0, 0);
                mrneighs.Add(n0);
            }
            if (mr.bsic1 != 0)
            {
                mrNeighbour n1 = new mrNeighbour(mr.cell, (int)mr.rxlev1, (int)mr.bcch1, (int)mr.bsic1, 0);
                mrneighs.Add(n1);
            }
            if (mr.bsic2 != 0)
            {
                mrNeighbour n2 = new mrNeighbour(mr.cell, (int)mr.rxlev2, (int)mr.bcch2, (int)mr.bsic2, 0);
                mrneighs.Add(n2);
            }
            if (mr.bsic3 != 0)
            {
                mrNeighbour n3 = new mrNeighbour(mr.cell, (int)mr.rxlev3, (int)mr.bcch3, (int)mr.bsic3, 0);
                mrneighs.Add(n3);
            }
            if (mr.bsic4 != 0)
            {
                mrNeighbour n4 = new mrNeighbour(mr.cell, (int)mr.rxlev4, (int)mr.bcch4, (int)mr.bsic4, 0);
                mrneighs.Add(n4);
            }
            if (mr.bsic5 != 0)
            {
                mrNeighbour n5 = new mrNeighbour(mr.cell, (int)mr.rxlev5, (int)mr.bcch5, (int)mr.bsic5, 0);
                mrneighs.Add(n5);
            }
            return mrneighs;
        }
    }
}
