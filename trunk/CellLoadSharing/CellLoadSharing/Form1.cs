﻿using System;
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
                gridControl1.Refresh();
                gridView1.OptionsView.ColumnAutoWidth = true;
                ConditionsAdjustment();
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
                gridView2.OptionsView.ColumnAutoWidth = false;
                ConditionsAdjustment_thr("accmin", 110, 111);  //小于110红色
                ConditionsAdjustment_equal("pt", 31);           //等于31红色
                ConditionsAdjustment_thr("mrrRX", -86, -84);   //小于-85红色
                ConditionsAdjustment_thr("T空闲信道", 0, 16); //小于0红色

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            ofd.Filter = "Excel (*.mdb)|*.mdb|(*.mdb)|*.mdb";
            ofd.Title = title;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                obj.Text += ofd.FileName + "\r\n" + "\r\n";
                connstr = "Provider=Microsoft.Jet.OleDb.4.0;Data Source= " + ofd.FileName;
            }
            OleDbConnection conn = new OleDbConnection(connstr);
            return conn;
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
    }
}