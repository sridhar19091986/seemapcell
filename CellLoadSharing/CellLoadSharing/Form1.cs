using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraGrid;
using System.Threading;
using DevExpress.XtraCharts;

namespace CellLoadSharing
{
    public partial class FormCellLoadSharing : Form
    {
        public FormCellLoadSharing()
        {
            InitializeComponent();
        }
        ComputeCell cc = new ComputeCell();
        private void btnExecuteCLS_Click(object sender, EventArgs e)
        {
            Form2 fr2 = new Form2();
            try
            {
                fr2.Show();

                Thread th = new Thread(ExecCls);
                th.Start();
                while (th.ThreadState == ThreadState.Running)
                    Application.DoEvents();

                gridControl1.DataSource = StaticTable.computecell.dtVar;
                //gridView1.PopulateColumns(); //会删除手动建立的字段，需要在窗口中定义其属性，一一映射

                gridView1.OptionsView.ColumnAutoWidth = true;
                ConditionsAdjustment();

                gridControl1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                fr2.Hide();
            }
        }
        private void ExecCls()
        {
            try
            {
                int top = (int)numericUpDown3.Value;
                double MsPerEdge = (double)numericUpDown2.Value / 10;
                double HalfRate = (double)numericUpDown1.Value / 100;
                cc.ComputeCellData(top, MsPerEdge, HalfRate);
                StaticTable.computecell = cc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private DataTable FilterDataTable(DataTable dataSource, string matchtypes)
        {
            DataView dv = dataSource.DefaultView;
            dv.RowFilter = "cell_name = '" + matchtypes + "'";
            dv.Sort = "FAILDLTBFEST desc";
            DataTable newTable1 = dv.ToTable();
            return newTable1;
        }
        private void gridControl1_Click(object sender, EventArgs e)
        {
            try
            {
                int[] a = this.gridView1.GetSelectedRows(); //传递实体类过去 获取选中的行
                string cellname = this.gridView1.GetRowCellValue(a[0], "Cell_name").ToString();//获取选中行的内容
                //string cellname = dataGridView1[1, e.RowIndex].Value.ToString();
                gridControl2.DataSource = FilterDataTable(StaticTable.computecell.dtDetail, cellname);

                //删除不能恢复原来的问题
                gridView2.PopulateColumns();

                gridView2.OptionsView.ColumnAutoWidth = false;

                ConditionsAdjustment_not_equal("accmin", 110);  //不等于110红色
                ConditionsAdjustment_equal("pt", 31);           //等于31红色
                ConditionsAdjustment_thr("mrrRX", -86, -84);   //小于-85红色
                ConditionsAdjustment_thr("T空闲信道", 0, 16); //小于0红色
                ConditionsAdjustment_not_equal("EDGE信道数简易计算", 0); //不等于0红色

                gridControl2.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ConditionsAdjustment_not_equal(string col, int thr)
        {
            StyleFormatCondition cn;
            cn = new StyleFormatCondition(FormatConditionEnum.NotEqual, gridView2.Columns[col], null, thr);
            cn.Appearance.BackColor = Color.Red;
            cn.Appearance.ForeColor = Color.White;
            gridView2.FormatConditions.Add(cn);
        }

        private void ConditionsAdjustment_equal(string col, int thr)
        {
            StyleFormatCondition cn;
            cn = new StyleFormatCondition(FormatConditionEnum.Equal, gridView2.Columns[col], null, thr);
            cn.Appearance.BackColor = Color.Red;
            cn.Appearance.ForeColor = Color.White;
            gridView2.FormatConditions.Add(cn);
        }
        private void ConditionsAdjustment_thr(string col, int min_thr, int max_thr)
        {
            StyleFormatCondition cn;
            cn = new StyleFormatCondition(FormatConditionEnum.Less, gridView2.Columns[col], null, min_thr);
            cn.Appearance.BackColor = Color.Red;
            cn.Appearance.ForeColor = Color.White;
            gridView2.FormatConditions.Add(cn);
            cn = new StyleFormatCondition(FormatConditionEnum.Greater, gridView2.Columns[col], null, max_thr);
            cn.Appearance.BackColor = Color.Green;
            gridView2.FormatConditions.Add(cn);
        }

        private void ConditionsAdjustment()
        {
            StyleFormatCondition cn;
            cn = new StyleFormatCondition(FormatConditionEnum.LessOrEqual, gridView1.Columns["Balance_T空闲信道"], null, 0);
            cn.Appearance.BackColor = Color.Red;
            cn.Appearance.ForeColor = Color.White;
            gridView1.FormatConditions.Add(cn);
        }

        private OleDbConnection SelectFile(RichTextBox obj, string title)
        {
            string connstr = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Access (*.mdb)|*.mdb|(*.mdb)|*.mdb";
            ofd.Title = title;
            ofd.FileName = title + ".mdb";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                obj.Text += ofd.FileName + "\r\n";
                connstr = "Provider=Microsoft.Jet.OleDb.4.0;Data Source= " + ofd.FileName;
                OleDbConnection conn = new OleDbConnection(connstr);
                return conn;
            }
            return null;
        }

        private void btnImportCellGPRS_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "CELLGPRS");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.cellgprs = dc.CELLGPRS.ToList();
                btnImportCellGPRS.Enabled = false;
                btnImportCellGPRS.Text += "-" + cc.cellgprs.Count().ToString();
                //MessageBox.Show(cc.cellgprs.Count().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnImportMRR_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "MRR小区小时常规统计");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.mrr = dc.MRR小区小时常规统计.ToList();
                //MessageBox.Show(cc.mrr.Count().ToString());
                btnImportMRR.Enabled = false;
                btnImportMRR.Text += "-" + cc.mrr.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnImportCDD_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "现网cdd");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.cdd = dc.现网cdd.ToList();
                //MessageBox.Show(cc.cdd.Count().ToString());
                btnImportCDD.Enabled = false;
                btnImportCDD.Text += "-" + cc.cdd.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnImportCDD_Nrel_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "现网cdd_Nrel");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.cdd_nrel = dc.现网cdd_Nrel.ToList();
                //MessageBox.Show(cc.cdd_nrel.Count().ToString());
                btnImportCDD_Nrel.Enabled = false;
                btnImportCDD_Nrel.Text += "-" + cc.cdd_nrel.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnImportBaseData_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "小区基础数据");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.cellbase = dc.小区基础数据.ToList();
                //MessageBox.Show(cc.cellbase.Count().ToString());
                btnImportBaseData.Enabled = false;
                btnImportBaseData.Text += "-" + cc.cellbase.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnImportHandover_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = SelectFile(richTextBox1, "小区切换查询");
                DataClasses1DataContext dc = new DataClasses1DataContext(conn);
                cc.cellho = dc.小区切换查询.ToList();
                //MessageBox.Show(cc.cellho.Count().ToString());
                btnImportHandover.Enabled = false;
                btnImportHandover.Text += "-" + cc.cellho.Count().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "/r/n"
                    + "1. mdb表名称=命令名称？" + "/r/n"
                    + "2. 和标准文档比有多余的字段？" + "/r/n");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                gridControl1.ExportToXls(@"CellBalanceList.xls");
                gridControl2.ExportToXls(@"CellBalanceDetail.xls");
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormCellLoadSharing_Load(object sender, EventArgs e)
        {

        }



        private void gridControl2_Click(object sender, EventArgs e)
        {
            chartControl1.Series.Clear();
            //chartControl1.
            // Create a new chart.
            //ChartControl polarAreaChart = new ChartControl();

            // Add a polar series to it.
            Series series1 = new Series("H话务比", ViewType.PolarPoint);
            Series series2 = new Series("PDCH复用度", ViewType.PolarPoint);


            double x, y, z;
            for (int i = 0; i < gridView2.RowCount; i++)
            {
                //for (int j = 0; j < gridView1.Columns.Count; j++)
                //{
                //    object val = 
                //}
                x = double.Parse(gridView2.GetRowCellValue(i, gridView2.Columns["方向角"]).ToString());
                y = double.Parse(gridView2.GetRowCellValue(i, gridView2.Columns["H话务比"]).ToString());
                z = double.Parse(gridView2.GetRowCellValue(i, gridView2.Columns["PDCH复用度"]).ToString());
                series1.Points.Add(new SeriesPoint(x, y));

                //放大5倍，不然看不清楚
                series2.Points.Add(new SeriesPoint(x, 5 * z));
            }



            // Populate the series with points.
            //series1.Points.Add(new SeriesPoint(0, 90));
            //series1.Points.Add(new SeriesPoint(90, 70));
            //series1.Points.Add(new SeriesPoint(180, 50));
            //series1.Points.Add(new SeriesPoint(270, 100));


            // Add the series to the chart.
            chartControl1.Series.Add(series1);
            chartControl1.Series.Add(series2);

            // Adjust the series options.
            series1.Label.Visible = false;
            series2.Label.Visible = false;

            // Flip the diagram (if necessary).
            ((PolarDiagram)chartControl1.Diagram).StartAngleInDegrees = 180;
            ((PolarDiagram)chartControl1.Diagram).RotationDirection =
                RadarDiagramRotationDirection.Counterclockwise;

            // Add a title to the chart and hide the legend.
            //ChartTitle chartTitle1 = new ChartTitle();
            //chartTitle1.Text = "Polar Area Chart";
            //chartControl1.Titles.Add(chartTitle1);
            //chartControl1.Legend.Visible = true;
            //chartControl1.Legend.
            // Add the chart to the form.
            //chartControl1.Dock = DockStyle.Fill;
            //this.Controls.Add(chartControl1);

        }

        private void ExportXls1_Click(object sender, EventArgs e)
        {
            //StaticTable.ExportToExcel(gridView1);

            try
            {
                StaticTable.ExportToExcel(gridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void ExportXls2_Click(object sender, EventArgs e)
        {
            try
            {
                StaticTable.ExportToExcel(gridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
