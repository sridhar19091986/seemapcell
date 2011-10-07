namespace Shape2Sql
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class a : Form
    {
        private IContainer a;
        private Label b;
        private Label c;
        private Label d;
        private Label e;

        public a()
        {
            this.a();
        }

        private void a()
        {
            this.b = new Label();
            this.c = new Label();
            this.d = new Label();
            this.e = new Label();
            base.SuspendLayout();
            this.b.AutoSize = true;
            this.b.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.b.Location = new Point(12, 9);
            this.b.Name = "label3";
            this.b.Size = new Size(0xff, 0x19);
            this.b.TabIndex = 2;
            this.b.Text = "ShapeFile to SqlServer";
            this.c.AutoSize = true;
            this.c.Location = new Point(14, 60);
            this.c.Name = "label4";
            this.c.Size = new Size(0x2f, 13);
            this.c.TabIndex = 4;
            this.c.Text = "version: ";
            this.c.TextAlign = ContentAlignment.MiddleLeft;
            this.d.AutoSize = true;
            this.d.Location = new Point(14, 0x59);
            this.d.Name = "label5";
            this.d.Size = new Size(220, 13);
            this.d.TabIndex = 5;
            this.d.Text = "(C) 2008 - Morten Nielsen - www.sharpgis.net";
            this.e.AutoSize = true;
            this.e.Location = new Point(14, 0x22);
            this.e.Name = "label1";
            this.e.Size = new Size(150, 13);
            this.e.TabIndex = 7;
            this.e.Text = "for Microsoft SQL Server 2008";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x132, 0x6f);
            base.Controls.Add(this.e);
            base.Controls.Add(this.d);
            base.Controls.Add(this.c);
            base.Controls.Add(this.b);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmAbout";
            base.Opacity = 0.9;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About";
            base.Load += new EventHandler(this.b);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void a(bool A_0)
        {
            if (A_0 && (this.a != null))
            {
                this.a.Dispose();
            }
            base.Dispose(A_0);
        }

        private void a(object A_0, EventArgs A_1)
        {
            base.Close();
        }

        private void b(object A_0, EventArgs A_1)
        {
            this.c.Text = this.c.Text + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}

