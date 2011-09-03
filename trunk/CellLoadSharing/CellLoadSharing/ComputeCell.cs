using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CellLoadSharing
{
    public static class StaticTable
    {
        public static ComputeCell computecell;
    }
    public class ComputeCell : ComputeModel
    {
        private DataClasses1DataContext dc = new DataClasses1DataContext();

        public DataTable dtVar;
        public DataTable dtTree;
        public DataTable dtDetail;
        public ComputeCell()
        {
            ComputeCellData();
        }
        private void ComputeCellData()
        {
            //DataTable dt = new DataTable();

            //替换1
            var cellgprs2 = dc.CELLGPRS_0822_1;//FG_小区小时GPRS资源_0816s;

            //替换2
            var cellbase = dc.小区基础数据_0822;

            //替换5
            var celldatabase = dc.现网cdd_0822;

            var tbf = from p in cellgprs2
                      //where p.BSC == "SZ35B"
                      group p by p.小区名 into ttt
                      select new
            {
                Cell_name = ttt.Key,
                BSC = ttt.Select(e => e.BSC).FirstOrDefault(),
                下行TBF建立成功率 = ttt.Average(e => e.下行TBF建立成功率),
                FAILDLTBFEST = ttt.Average(e => e.FAILDLTBFEST),
                PDCH复用度 = ttt.Average(e => e.PDCH复用度),
                平均分配PDCH = ttt.Average(e => e.平均分配PDCH),
                T可用信道 = ttt.Average(e => e.T可用信道),
                T话务量 = ttt.Average(e => e.T话务量),
                H话务比 = ttt.Average(e => e.H话务比),
                GPRS下行激活信道 = ttt.Average(e => e.GPRS下行激活信道),
                EDGE下行激活信道 = ttt.Average(e => e.EDGE下行激活信道),
                EDGE终端使用EDGE比例 = ttt.Average(e => e.EDGE终端使用EDGE比例),
                GPRS每线下行用户 = ttt.Average(e => e.GPRS每线下行用户),
                EDGE每线下行用户 = ttt.Average(e => e.EDGE每线下行用户),
                T话务量tch_20h = erlangbinv(0.02, 0.95 * ttt.Average(e => e.T话务量)),   //半速率取10%
            };

            //var faildltbf=tbf.OrderByDescending(e=>e.FAILDLTBFEST).Take(10).Select(e=>e.Cell_name).ToList();
            //   var cdd_nrel=from p in Cdd_Nrel_Get()
            //                join q in 小区基础数据0803s on p.Cell_name equals q.小区名
            //				select new {p,q};

            var celbase = cellbase.ToLookup(e => e.小区名);

            var relcell = Ho_Nrel_Get().ToList();
            var nrelcell = relcell.ToLookup(e => e.Cell_name + e.N_cell_name);
            foreach (var m in Cdd_Nrel_Get())
                if (!nrelcell.Contains(m.Cell_name + m.N_cell_name))
                    relcell.Add(m);

            //替换6
            //var nrelcelldistinct=nrelcell.
            var mrr = from p in dc.MRR_0822
                      select new
              {
                  p.小区名,
                  dllevel = p.DL覆盖75 > 50 ? 75 : p.DL覆盖85 > 50 ? 85 : p.DL覆盖90 > 50 ? 90 : 94
              };

            //规划用EDGE复用度，取3.7么？
            int MsPerEdge = 6;

            var rel = from p in relcell
                      join q in tbf on p.N_cell_name equals q.Cell_name
                      join t in celldatabase on p.N_cell_name equals t.cell_name
                      join m in mrr on p.N_cell_name equals m.小区名
                      //join t in 小区基础数据0803s  on p.N_cell_name equals t.小区名
                      select new
            {
                BSC = q.BSC,
                p.Cell_name,
                p.N_cell_name,
                p.Handover,
                t.cro,
                t.pt,
                t.to,
                t.accmin,
                mrrRX = -m.dllevel,
                C2 = (t.pt == 31 ? (-m.dllevel - (-t.accmin) - 2 * t.cro) : (-m.dllevel - (-t.accmin) + 2 * t.cro - t.to * t.pt)),
                方向角 = celbase[p.N_cell_name].Select(e => e.方向角).FirstOrDefault(),
                下倾角 = celbase[p.N_cell_name].Select(e => e.下倾角).FirstOrDefault(),
                海拔高度 = celbase[p.N_cell_name].Select(e => e.海拔高度).FirstOrDefault(),

                T空闲信道 = ConvNullDouble(q.T可用信道 - q.平均分配PDCH * q.PDCH复用度 / MsPerEdge - q.T话务量tch_20h),  //调整公式
                T信道需求 = ConvNullDouble(q.平均分配PDCH * q.PDCH复用度 / MsPerEdge + q.T话务量tch_20h),
                T可用信道 = ConvNullDouble(q.T可用信道),

                FAILDLTBFEST = ConvNullDouble(q.FAILDLTBFEST),

                下行TBF建立成功率 = ConvNullDouble(q.下行TBF建立成功率),

                EDGE终端使用EDGE比例 = ConvNullDouble(q.EDGE终端使用EDGE比例),


                平均分配PDCH = ConvNullDouble(q.平均分配PDCH),
                PDCH复用度 = ConvNullDouble(q.PDCH复用度),
                GPRS每线下行用户 = ConvNullDouble(q.GPRS每线下行用户),
                EDGE每线下行用户 = ConvNullDouble(q.EDGE每线下行用户),


                T话务量 = ConvNullDouble(q.T话务量),
                H话务比 = ConvNullDouble(q.H话务比),
                H话务比T信道 = ConvNullDouble(q.T话务量tch_20h),

                GPRS下行激活信道 = ConvNullDouble(q.GPRS下行激活信道),
                EDGE下行激活信道 = ConvNullDouble(q.EDGE下行激活信道),
                EDGE信道数简易计算 = 4 * Math.Floor((decimal)(q.EDGE每线下行用户 < MsPerEdge ? 0 : q.EDGE下行激活信道 * (q.EDGE每线下行用户 / MsPerEdge)) / 4),
            };

            dtDetail = rel.ToList().ToDataTable();
            //var rellist = rel.ToList();
            //rellist.Where(e=>faildltbf.Contains(e.Cell_name)).OrderBy(e => e.Cell_name).ThenBy(e => e.FAILDLTBFEST).Dump();


            var variance = from p in rel
                           group p by p.Cell_name into ttt
                           select new
            {
                BSC = ttt.Select(e => e.BSC).FirstOrDefault(),
                Cell_name = ttt.Key,

                Balance_T空闲信道 = ttt.Where(e => e.Cell_name == ttt.Key).Sum(e => e.T空闲信道),

                FAILDLTBFEST = ttt.Where(e => e.N_cell_name == ttt.Key).Sum(e => e.FAILDLTBFEST),

                //Variance_FAILDLTBFEST = Variance(ttt.Select(e => e.FAILDLTBFEST/10000)),
                Variance_T空闲信道 = Variance(ttt.Select(e => e.T空闲信道)),
                Variance_PDCH复用度 = Variance(ttt.Select(e => e.PDCH复用度)),
                Variance_H话务比 = Variance(ttt.Select(e => e.H话务比 / 100)),
                Variance_EDGE终端使用EDGE比例 = Variance(ttt.Select(e => e.EDGE终端使用EDGE比例 / 100)),

                //Variance_detail = ttt.Where(e => e.Cell_name == ttt.Key).OrderByDescending(e => e.FAILDLTBFEST)
            };

            dtVar = variance.ToList().OrderByDescending(e => e.FAILDLTBFEST).Take(1000).ToDataTable();
            //variance.ToList().Where(e=>e.Cell_name.IndexOf("渔业村")!=-1).Dump();

            //variance.ToList().Where(e => e.Balance_T空闲信道 > 0).OrderByDescending(e => e.FAILDLTBFEST).Take(1000).Dump();

            //variance.ToList().OrderByDescending(e => e.FAILDLTBFEST).Skip(1000).Take(1000).Dump();

            //variance.ToList().OrderByDescending(e => e.Balance_T空闲信道).Take(1000).Dump();

            //	var dltbf=variance.ToList().OrderByDescending(e => e.FAILDLTBFEST).Take(2000);
            //	var tbf=from p in dltbf
            //	        group p by p.

            //var output = variance.ToList().OrderByDescending(e => e.FAILDLTBFEST).Skip(100);
            //dt = output.ToDataTable();
            //return dt;

        }
    }
}





