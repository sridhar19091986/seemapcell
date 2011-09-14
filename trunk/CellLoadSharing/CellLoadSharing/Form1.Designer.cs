namespace CellLoadSharing
{
    partial class FormCellLoadSharing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.btnImportCellGPRS = new System.Windows.Forms.Button();
            this.btnImportMRR = new System.Windows.Forms.Button();
            this.btnImportCDD = new System.Windows.Forms.Button();
            this.btnImportCDD_Nrel = new System.Windows.Forms.Button();
            this.btnImportBaseData = new System.Windows.Forms.Button();
            this.btnImportHandover = new System.Windows.Forms.Button();
            this.ImportAd = new System.Windows.Forms.GroupBox();
            this.btnExecuteCLS = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.BSC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Cell_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Balance_T空闲信道 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.FAILDLTBFEST = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Variance_T空闲信道 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Variance_PDCH复用度 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Variance_H话务比 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Variance_EDGE终端使用EDGE比例 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.staticTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ImportAd.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staticTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImportCellGPRS
            // 
            this.btnImportCellGPRS.Location = new System.Drawing.Point(0, 46);
            this.btnImportCellGPRS.Name = "btnImportCellGPRS";
            this.btnImportCellGPRS.Size = new System.Drawing.Size(193, 23);
            this.btnImportCellGPRS.TabIndex = 0;
            this.btnImportCellGPRS.Text = "CELLGPRS";
            this.btnImportCellGPRS.UseVisualStyleBackColor = true;
            this.btnImportCellGPRS.Click += new System.EventHandler(this.btnImportCellGPRS_Click);
            // 
            // btnImportMRR
            // 
            this.btnImportMRR.Location = new System.Drawing.Point(0, 75);
            this.btnImportMRR.Name = "btnImportMRR";
            this.btnImportMRR.Size = new System.Drawing.Size(193, 23);
            this.btnImportMRR.TabIndex = 1;
            this.btnImportMRR.Text = "MRR小区小时常规统计";
            this.btnImportMRR.UseVisualStyleBackColor = true;
            this.btnImportMRR.Click += new System.EventHandler(this.btnImportMRR_Click);
            // 
            // btnImportCDD
            // 
            this.btnImportCDD.Location = new System.Drawing.Point(0, 104);
            this.btnImportCDD.Name = "btnImportCDD";
            this.btnImportCDD.Size = new System.Drawing.Size(193, 23);
            this.btnImportCDD.TabIndex = 2;
            this.btnImportCDD.Text = "现网cdd";
            this.btnImportCDD.UseVisualStyleBackColor = true;
            this.btnImportCDD.Click += new System.EventHandler(this.btnImportCDD_Click);
            // 
            // btnImportCDD_Nrel
            // 
            this.btnImportCDD_Nrel.Location = new System.Drawing.Point(0, 133);
            this.btnImportCDD_Nrel.Name = "btnImportCDD_Nrel";
            this.btnImportCDD_Nrel.Size = new System.Drawing.Size(193, 23);
            this.btnImportCDD_Nrel.TabIndex = 3;
            this.btnImportCDD_Nrel.Text = "现网cdd_Nrel";
            this.btnImportCDD_Nrel.UseVisualStyleBackColor = true;
            this.btnImportCDD_Nrel.Click += new System.EventHandler(this.btnImportCDD_Nrel_Click);
            // 
            // btnImportBaseData
            // 
            this.btnImportBaseData.Location = new System.Drawing.Point(0, 162);
            this.btnImportBaseData.Name = "btnImportBaseData";
            this.btnImportBaseData.Size = new System.Drawing.Size(193, 23);
            this.btnImportBaseData.TabIndex = 4;
            this.btnImportBaseData.Text = "小区基础数据";
            this.btnImportBaseData.UseVisualStyleBackColor = true;
            this.btnImportBaseData.Click += new System.EventHandler(this.btnImportBaseData_Click);
            // 
            // btnImportHandover
            // 
            this.btnImportHandover.Location = new System.Drawing.Point(0, 191);
            this.btnImportHandover.Name = "btnImportHandover";
            this.btnImportHandover.Size = new System.Drawing.Size(193, 23);
            this.btnImportHandover.TabIndex = 5;
            this.btnImportHandover.Text = "小区切换查询";
            this.btnImportHandover.UseVisualStyleBackColor = true;
            this.btnImportHandover.Click += new System.EventHandler(this.btnImportHandover_Click);
            // 
            // ImportAd
            // 
            this.ImportAd.Controls.Add(this.btnImportCellGPRS);
            this.ImportAd.Controls.Add(this.btnImportHandover);
            this.ImportAd.Controls.Add(this.btnImportMRR);
            this.ImportAd.Controls.Add(this.btnImportBaseData);
            this.ImportAd.Controls.Add(this.btnImportCDD);
            this.ImportAd.Controls.Add(this.btnImportCDD_Nrel);
            this.ImportAd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImportAd.Location = new System.Drawing.Point(619, 240);
            this.ImportAd.Name = "ImportAd";
            this.ImportAd.Size = new System.Drawing.Size(214, 231);
            this.ImportAd.TabIndex = 6;
            this.ImportAd.TabStop = false;
            this.ImportAd.Text = "Import Analysis Data";
            // 
            // btnExecuteCLS
            // 
            this.btnExecuteCLS.Location = new System.Drawing.Point(9, 40);
            this.btnExecuteCLS.Name = "btnExecuteCLS";
            this.btnExecuteCLS.Size = new System.Drawing.Size(120, 23);
            this.btnExecuteCLS.TabIndex = 1;
            this.btnExecuteCLS.Text = "ExecuteCLS";
            this.btnExecuteCLS.UseVisualStyleBackColor = true;
            this.btnExecuteCLS.Click += new System.EventHandler(this.btnExecuteCLS_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControl2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ImportAd, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(836, 474);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExportExcel);
            this.groupBox3.Controls.Add(this.btnExecuteCLS);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 231);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Excute Command";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(9, 138);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(120, 23);
            this.btnExportExcel.TabIndex = 10;
            this.btnExportExcel.Text = "ExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(619, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 231);
            this.panel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Check File List";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 17);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(208, 211);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(153, 3);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(460, 231);
            this.gridControl1.TabIndex = 13;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.advBandedGridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.BSC,
            this.Cell_name,
            this.Balance_T空闲信道,
            this.FAILDLTBFEST,
            this.Variance_T空闲信道,
            this.Variance_PDCH复用度,
            this.Variance_H话务比,
            this.Variance_EDGE终端使用EDGE比例});
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, ""),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "", null, "")});
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // BSC
            // 
            this.BSC.Caption = "BSC";
            this.BSC.FieldName = "BSC";
            this.BSC.Name = "BSC";
            this.BSC.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            this.BSC.Visible = true;
            this.BSC.VisibleIndex = 0;
            // 
            // Cell_name
            // 
            this.Cell_name.Caption = "Cell_name";
            this.Cell_name.FieldName = "Cell_name";
            this.Cell_name.Name = "Cell_name";
            this.Cell_name.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
            this.Cell_name.Visible = true;
            this.Cell_name.VisibleIndex = 1;
            // 
            // Balance_T空闲信道
            // 
            this.Balance_T空闲信道.Caption = "Balance_T空闲信道";
            this.Balance_T空闲信道.FieldName = "Balance_T空闲信道";
            this.Balance_T空闲信道.Name = "Balance_T空闲信道";
            this.Balance_T空闲信道.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.Balance_T空闲信道.Visible = true;
            this.Balance_T空闲信道.VisibleIndex = 2;
            // 
            // FAILDLTBFEST
            // 
            this.FAILDLTBFEST.Caption = "FAILDLTBFEST";
            this.FAILDLTBFEST.FieldName = "FAILDLTBFEST";
            this.FAILDLTBFEST.Name = "FAILDLTBFEST";
            this.FAILDLTBFEST.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.FAILDLTBFEST.Visible = true;
            this.FAILDLTBFEST.VisibleIndex = 3;
            // 
            // Variance_T空闲信道
            // 
            this.Variance_T空闲信道.Caption = "Variance_T空闲信道";
            this.Variance_T空闲信道.FieldName = "Variance_T空闲信道";
            this.Variance_T空闲信道.Name = "Variance_T空闲信道";
            this.Variance_T空闲信道.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.Variance_T空闲信道.Visible = true;
            this.Variance_T空闲信道.VisibleIndex = 4;
            // 
            // Variance_PDCH复用度
            // 
            this.Variance_PDCH复用度.Caption = "Variance_PDCH复用度";
            this.Variance_PDCH复用度.FieldName = "Variance_PDCH复用度";
            this.Variance_PDCH复用度.Name = "Variance_PDCH复用度";
            this.Variance_PDCH复用度.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
            this.Variance_PDCH复用度.Visible = true;
            this.Variance_PDCH复用度.VisibleIndex = 5;
            // 
            // Variance_H话务比
            // 
            this.Variance_H话务比.Caption = "Variance_H话务比";
            this.Variance_H话务比.FieldName = "Variance_H话务比";
            this.Variance_H话务比.Name = "Variance_H话务比";
            this.Variance_H话务比.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
            this.Variance_H话务比.Visible = true;
            this.Variance_H话务比.VisibleIndex = 6;
            // 
            // Variance_EDGE终端使用EDGE比例
            // 
            this.Variance_EDGE终端使用EDGE比例.Caption = "Variance_EDGE终端使用EDGE比例";
            this.Variance_EDGE终端使用EDGE比例.FieldName = "Variance_EDGE终端使用EDGE比例";
            this.Variance_EDGE终端使用EDGE比例.Name = "Variance_EDGE终端使用EDGE比例";
            this.Variance_EDGE终端使用EDGE比例.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
            this.Variance_EDGE终端使用EDGE比例.Visible = true;
            this.Variance_EDGE终端使用EDGE比例.VisibleIndex = 7;
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.advBandedGridView1.GridControl = this.gridControl1;
            this.advBandedGridView1.Name = "advBandedGridView1";
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Name = "gridBand1";
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(153, 240);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(460, 231);
            this.gridControl2.TabIndex = 14;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown3);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 231);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Define Parameter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "FAILDLTBFEST TOP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "PDCH复用度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "H话务比";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(9, 182);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(9, 117);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(9, 49);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // staticTableBindingSource
            // 
            this.staticTableBindingSource.DataSource = typeof(CellLoadSharing.StaticTable);
            // 
            // FormCellLoadSharing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 474);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormCellLoadSharing";
            this.Text = "CellLoadSharing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormCellLoadSharing_Load);
            this.ImportAd.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staticTableBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportCellGPRS;
        private System.Windows.Forms.Button btnImportMRR;
        private System.Windows.Forms.Button btnImportCDD;
        private System.Windows.Forms.Button btnImportCDD_Nrel;
        private System.Windows.Forms.Button btnImportBaseData;
        private System.Windows.Forms.Button btnImportHandover;
        private System.Windows.Forms.GroupBox ImportAd;
        private System.Windows.Forms.Button btnExecuteCLS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Columns.GridColumn BSC;
        private DevExpress.XtraGrid.Columns.GridColumn Cell_name;
        private DevExpress.XtraGrid.Columns.GridColumn Balance_T空闲信道;
        private DevExpress.XtraGrid.Columns.GridColumn FAILDLTBFEST;
        private DevExpress.XtraGrid.Columns.GridColumn Variance_T空闲信道;
        private DevExpress.XtraGrid.Columns.GridColumn Variance_PDCH复用度;
        private DevExpress.XtraGrid.Columns.GridColumn Variance_H话务比;
        private DevExpress.XtraGrid.Columns.GridColumn Variance_EDGE终端使用EDGE比例;
        private System.Windows.Forms.BindingSource staticTableBindingSource;
    }
}

