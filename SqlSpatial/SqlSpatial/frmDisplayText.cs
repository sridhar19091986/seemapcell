namespace SqlSpatial
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDisplayText : Form
    {
        private IContainer components;
        private TextBox textBox1;

        public frmDisplayText(string text, bool wordWrap)
        {
            this.InitializeComponent();
            this.textBox1.WordWrap = wordWrap;
            this.textBox1.Text = text;
            this.textBox1.Select(0, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.textBox1.Location = new Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = ScrollBars.Both;
            this.textBox1.Size = new Size(0x1b3, 0x14e);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1b3, 0x14e);
            base.Controls.Add(this.textBox1);
            base.Name = "frmDisplayText";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Text Viewer";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

