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
            this.btnImportCellGPRS = new System.Windows.Forms.Button();
            this.btnImportMRR = new System.Windows.Forms.Button();
            this.btnImportCDD = new System.Windows.Forms.Button();
            this.btnImportCDD_Nrel = new System.Windows.Forms.Button();
            this.btnImportBaseData = new System.Windows.Forms.Button();
            this.btnImportHandover = new System.Windows.Forms.Button();
            this.ImportAd = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnExecuteCLS = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.ImportAd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImportCellGPRS
            // 
            this.btnImportCellGPRS.Location = new System.Drawing.Point(0, 46);
            this.btnImportCellGPRS.Name = "btnImportCellGPRS";
            this.btnImportCellGPRS.Size = new System.Drawing.Size(193, 23);
            this.btnImportCellGPRS.TabIndex = 0;
            this.btnImportCellGPRS.Text = "ImportCellGPRS";
            this.btnImportCellGPRS.UseVisualStyleBackColor = true;
            // 
            // btnImportMRR
            // 
            this.btnImportMRR.Location = new System.Drawing.Point(0, 75);
            this.btnImportMRR.Name = "btnImportMRR";
            this.btnImportMRR.Size = new System.Drawing.Size(193, 23);
            this.btnImportMRR.TabIndex = 1;
            this.btnImportMRR.Text = "ImportMRR";
            this.btnImportMRR.UseVisualStyleBackColor = true;
            // 
            // btnImportCDD
            // 
            this.btnImportCDD.Location = new System.Drawing.Point(0, 104);
            this.btnImportCDD.Name = "btnImportCDD";
            this.btnImportCDD.Size = new System.Drawing.Size(193, 23);
            this.btnImportCDD.TabIndex = 2;
            this.btnImportCDD.Text = "ImportCDD";
            this.btnImportCDD.UseVisualStyleBackColor = true;
            // 
            // btnImportCDD_Nrel
            // 
            this.btnImportCDD_Nrel.Location = new System.Drawing.Point(0, 133);
            this.btnImportCDD_Nrel.Name = "btnImportCDD_Nrel";
            this.btnImportCDD_Nrel.Size = new System.Drawing.Size(193, 23);
            this.btnImportCDD_Nrel.TabIndex = 3;
            this.btnImportCDD_Nrel.Text = "ImportCDD_Nrel";
            this.btnImportCDD_Nrel.UseVisualStyleBackColor = true;
            // 
            // btnImportBaseData
            // 
            this.btnImportBaseData.Location = new System.Drawing.Point(0, 162);
            this.btnImportBaseData.Name = "btnImportBaseData";
            this.btnImportBaseData.Size = new System.Drawing.Size(193, 23);
            this.btnImportBaseData.TabIndex = 4;
            this.btnImportBaseData.Text = "ImportBaseData";
            this.btnImportBaseData.UseVisualStyleBackColor = true;
            // 
            // btnImportHandover
            // 
            this.btnImportHandover.Location = new System.Drawing.Point(0, 191);
            this.btnImportHandover.Name = "btnImportHandover";
            this.btnImportHandover.Size = new System.Drawing.Size(193, 23);
            this.btnImportHandover.TabIndex = 5;
            this.btnImportHandover.Text = "ImportHandover";
            this.btnImportHandover.UseVisualStyleBackColor = true;
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
            this.ImportAd.Location = new System.Drawing.Point(621, 178);
            this.ImportAd.Name = "ImportAd";
            this.ImportAd.Size = new System.Drawing.Size(212, 253);
            this.ImportAd.TabIndex = 6;
            this.ImportAd.TabStop = false;
            this.ImportAd.Text = "Import Analysis Data";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 178);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(153, 253);
            this.treeView1.TabIndex = 0;
            // 
            // btnExecuteCLS
            // 
            this.btnExecuteCLS.Location = new System.Drawing.Point(3, 3);
            this.btnExecuteCLS.Name = "btnExecuteCLS";
            this.btnExecuteCLS.Size = new System.Drawing.Size(124, 23);
            this.btnExecuteCLS.TabIndex = 1;
            this.btnExecuteCLS.Text = "ExecuteCLS";
            this.btnExecuteCLS.UseVisualStyleBackColor = true;
            this.btnExecuteCLS.Click += new System.EventHandler(this.btnExecuteCLS_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(162, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(453, 169);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(162, 178);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(453, 253);
            this.dataGridView2.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.81169F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.18831F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ImportAd, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnExecuteCLS, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnExportExcel, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.55299F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.44701F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(836, 434);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(621, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(107, 23);
            this.btnExportExcel.TabIndex = 10;
            this.btnExportExcel.Text = "ExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // FormCellLoadSharing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 434);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormCellLoadSharing";
            this.Text = "CellLoadSharing";
            this.Load += new System.EventHandler(this.FormCellLoadSharing_Load);
            this.ImportAd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnExportExcel;
    }
}

