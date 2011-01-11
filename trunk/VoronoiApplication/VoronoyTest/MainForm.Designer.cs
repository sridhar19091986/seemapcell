namespace VoronoyTest
{
    partial class MainForm
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
            this.pbxVisialization = new System.Windows.Forms.PictureBox();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.grpDiagram = new System.Windows.Forms.GroupBox();
            this.pnlCold = new System.Windows.Forms.Panel();
            this.pnlHot = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPoints = new System.Windows.Forms.TextBox();
            this.lblPoints = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbxVisialization)).BeginInit();
            this.grpDiagram.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxVisialization
            // 
            this.pbxVisialization.Location = new System.Drawing.Point(12, 49);
            this.pbxVisialization.Name = "pbxVisialization";
            this.pbxVisialization.Size = new System.Drawing.Size(640, 443);
            this.pbxVisialization.TabIndex = 0;
            this.pbxVisialization.TabStop = false;
            // 
            // btnRebuild
            // 
            this.btnRebuild.Location = new System.Drawing.Point(12, 16);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(83, 22);
            this.btnRebuild.TabIndex = 1;
            this.btnRebuild.Text = "Regenerate";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.btnRebuild_Click);
            // 
            // grpDiagram
            // 
            this.grpDiagram.Controls.Add(this.pnlCold);
            this.grpDiagram.Controls.Add(this.pnlHot);
            this.grpDiagram.Controls.Add(this.label3);
            this.grpDiagram.Controls.Add(this.label2);
            this.grpDiagram.Controls.Add(this.txtPoints);
            this.grpDiagram.Controls.Add(this.lblPoints);
            this.grpDiagram.Location = new System.Drawing.Point(111, 7);
            this.grpDiagram.Name = "grpDiagram";
            this.grpDiagram.Size = new System.Drawing.Size(541, 36);
            this.grpDiagram.TabIndex = 2;
            this.grpDiagram.TabStop = false;
            // 
            // pnlCold
            // 
            this.pnlCold.BackColor = System.Drawing.Color.Blue;
            this.pnlCold.Location = new System.Drawing.Point(434, 11);
            this.pnlCold.Name = "pnlCold";
            this.pnlCold.Size = new System.Drawing.Size(28, 18);
            this.pnlCold.TabIndex = 7;
            this.pnlCold.Click += new System.EventHandler(this.pnlCold_Click);
            // 
            // pnlHot
            // 
            this.pnlHot.BackColor = System.Drawing.Color.Red;
            this.pnlHot.Location = new System.Drawing.Point(273, 11);
            this.pnlHot.Name = "pnlHot";
            this.pnlHot.Size = new System.Drawing.Size(28, 18);
            this.pnlHot.TabIndex = 6;
            this.pnlHot.Click += new System.EventHandler(this.pnlHot_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(353, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cold Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hot Color";
            // 
            // txtPoints
            // 
            this.txtPoints.Location = new System.Drawing.Point(99, 11);
            this.txtPoints.Name = "txtPoints";
            this.txtPoints.Size = new System.Drawing.Size(100, 21);
            this.txtPoints.TabIndex = 1;
            this.txtPoints.Text = "250";
            // 
            // lblPoints
            // 
            this.lblPoints.AutoSize = true;
            this.lblPoints.Location = new System.Drawing.Point(6, 15);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(101, 12);
            this.lblPoints.TabIndex = 0;
            this.lblPoints.Text = "Number of points";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 500);
            this.Controls.Add(this.grpDiagram);
            this.Controls.Add(this.btnRebuild);
            this.Controls.Add(this.pbxVisialization);
            this.Name = "MainForm";
            this.Text = "Voronoi Temperature Map";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxVisialization)).EndInit();
            this.grpDiagram.ResumeLayout(false);
            this.grpDiagram.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxVisialization;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.GroupBox grpDiagram;
        private System.Windows.Forms.Panel pnlHot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPoints;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.Panel pnlCold;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}

