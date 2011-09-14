using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Windows.Forms;

namespace CellLoadSharing
{
    public static class StaticTable
    {
        public static ComputeCell computecell;

        public static void ExportToExcel(DataGridView DataGridView1)
        {
            // creating Excel Application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program
            //Funny
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.
            // store its reference to worksheet
            try
            {
                //Fixed:(Microsoft.Office.Interop.Excel.Worksheet)
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                // changing the name of active sheet
                worksheet.Name = "Exported from CLS";
                // storing header part in Excel
                for (int i = 1; i < DataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = DataGridView1.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet
                for (int i = 0; i < DataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < DataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = DataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                // save the application
                string fileName = String.Empty;
                SaveFileDialog saveFileExcel = new SaveFileDialog();

                saveFileExcel.Filter = "Excel files |*.xls|All files (*.*)|*.*";
                saveFileExcel.FilterIndex = 2;
                saveFileExcel.RestoreDirectory = true;

                if (saveFileExcel.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileExcel.FileName;
                    //Fixed-old code :11 para->add 1:Type.Missing
                    workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                }
                else
                    return;

                // Exit from the application
                //app.Quit();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                app.Quit();
                workbook = null;
                app = null;
            }
        }
    }
    public class ComputeCell : ComputeModel
    {
        public DataTable dtVar;
        public DataTable dtTree;
        public DataTable dtDetail;
        public List<CELLGPRS> cellgprs { get; set; }
        public List<小区基础数据> cellbase { get; set; }
        public List<现网cdd> cdd { get; set; }
        public List<现网cdd_Nrel> cdd_nrel { get; set; }
        public List<小区切换查询> cellho { get; set; }
        public List<MRR小区小时常规统计> mrr { get; set; }

        public ComputeCell(){}
        public void ComputeCellData(int top, double MsPerEdge, double HalfRate)
        {
            double hf = (1 - HalfRate) + HalfRate / 2;

            var tbf = from p in cellgprs
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
                T话务量tch_20h = erlangbinv(0.02, hf * ttt.Average(e => e.T话务量)),   //半速率取10%
            };

            var celbase = cellbase.ToLookup(e => e.小区名);

            var relcell = Ho_Nrel_Get(cellho).ToList();
            var nrelcell = relcell.ToLookup(e => e.Cell_name + e.N_cell_name);
            foreach (var m in Cdd_Nrel_Get(cdd_nrel))
                if (!nrelcell.Contains(m.Cell_name + m.N_cell_name))
                    relcell.Add(m);

            var mrrs = from p in mrr
                       group p by p.小区名 into ttt
                       select new
                       {
                           小区名 = ttt.Key,
                           DL覆盖75 = ttt.Average(e => e.DL覆盖75),
                           DL覆盖85 = ttt.Average(e => e.DL覆盖85),
                           DL覆盖90 = ttt.Average(e => e.DL覆盖90),
                       };

            var mrr_g = from p in mrrs
                        select new
                {
                    小区名 = p.小区名,
                    dllevel = p.DL覆盖75 > 50 ? 75 : p.DL覆盖85 > 50 ? 85 : p.DL覆盖90 > 50 ? 90 : 94
                };

            var rel = from p in relcell
                      join q in tbf on p.N_cell_name equals q.Cell_name
                      join t in cdd on p.N_cell_name equals t.cell_name
                      join m in mrr_g on p.N_cell_name equals m.小区名
                      select new
            {
                BSC = q.BSC,
                Cell_name = p.Cell_name,
                N_cell_name = p.N_cell_name,
                Handover = p.Handover,
                cro = t.cro,
                pt = t.pt,
                to = t.to,
                accmin = t.accmin,
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

            dtDetail = rel.ToDataTable();

            var variance = from p in rel
                           group p by p.Cell_name into ttt
                           select new
            {
                BSC = ttt.Select(e => e.BSC).FirstOrDefault(),
                Cell_name = ttt.Key,

                Balance_T空闲信道 = ttt.Where(e => e.Cell_name == ttt.Key).Sum(e => e.T空闲信道),

                FAILDLTBFEST = ttt.Where(e => e.N_cell_name == ttt.Key).Sum(e => e.FAILDLTBFEST),
                Variance_T空闲信道 = Variance(ttt.Select(e => e.T空闲信道)),
                Variance_PDCH复用度 = Variance(ttt.Select(e => e.PDCH复用度)),
                Variance_H话务比 = Variance(ttt.Select(e => e.H话务比 / 100)),
                Variance_EDGE终端使用EDGE比例 = Variance(ttt.Select(e => e.EDGE终端使用EDGE比例 / 100)),
            };

            dtVar = variance.OrderByDescending(e => e.FAILDLTBFEST).Take(top).ToDataTable();
        }
    }
}





