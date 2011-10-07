namespace SqlSpatial
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class frmAbout : Form
    {
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox textBox1;

        public frmAbout()
        {
            this.InitializeComponent();
            this.label4.Text = this.label4.Text + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmAbout));
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x133, 0x19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sql Spatial Query Visualizer";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(14, 60);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2f, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "version: ";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(14, 0x59);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x85, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "(C) 2008 - Morten Nielsen -";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(14, 0x22);
            this.label1.Name = "label1";
            this.label1.Size = new Size(150, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "for Microsoft SQL Server 2008";
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Cursor = Cursors.Hand;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.Blue;
            this.label2.Location = new Point(0x90, 0x59);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x5b, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "www.sharpgis.net";
            this.label2.Click += new EventHandler(this.label2_Click);
            this.textBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.textBox1.BackColor = Color.White;
            this.textBox1.CausesValidation = false;
            this.textBox1.Location = new Point(12, 0x73);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new Size(0x129, 0x4c);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            this.textBox1.Text = manager.GetString("textBox1.Text");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x141, 0xcb);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmAbout";
            base.Opacity = 0.9;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.sharpgis.net/");
        }
    }
}

