namespace SeeMapCell
{
    partial class SeeMapCell
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mtipSeeMapCell = new System.Windows.Forms.MenuStrip();
            this.stipSeeMapCell = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // mtipSeeMapCell
            // 
            this.mtipSeeMapCell.Location = new System.Drawing.Point(0, 0);
            this.mtipSeeMapCell.Name = "mtipSeeMapCell";
            this.mtipSeeMapCell.Size = new System.Drawing.Size(811, 24);
            this.mtipSeeMapCell.TabIndex = 0;
            this.mtipSeeMapCell.Text = "mtipSeeMapCell";
            // 
            // stipSeeMapCell
            // 
            this.stipSeeMapCell.Location = new System.Drawing.Point(0, 502);
            this.stipSeeMapCell.Name = "stipSeeMapCell";
            this.stipSeeMapCell.Size = new System.Drawing.Size(811, 22);
            this.stipSeeMapCell.TabIndex = 1;
            this.stipSeeMapCell.Text = "stipSeeMapCell";
            // 
            // SeeMapCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 524);
            this.Controls.Add(this.stipSeeMapCell);
            this.Controls.Add(this.mtipSeeMapCell);
            this.MainMenuStrip = this.mtipSeeMapCell;
            this.Name = "SeeMapCell";
            this.Text = "SeeMapCell";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mtipSeeMapCell;
        private System.Windows.Forms.StatusStrip stipSeeMapCell;
    }
}

