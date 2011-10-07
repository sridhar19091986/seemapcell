namespace Shape2Sql
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class f : Form
    {
        private IContainer a;
        public TextBox b;

        public f()
        {
            this.a();
        }

        private void a()
        {
            this.b = new TextBox();
            base.SuspendLayout();
            this.b.BackColor = Color.Black;
            this.b.Dock = DockStyle.Fill;
            this.b.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.b.ForeColor = Color.White;
            this.b.Location = new Point(0, 0);
            this.b.Multiline = true;
            this.b.Name = "textBox1";
            this.b.ReadOnly = true;
            this.b.ScrollBars = ScrollBars.Both;
            this.b.Size = new Size(0x24d, 0x155);
            this.b.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x24d, 0x155);
            base.Controls.Add(this.b);
            base.Name = "Console";
            this.Text = "Shape2SQL";
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

        public void a(string A_0)
        {
            int num = this.b.Text.LastIndexOf("\r\n");
            if (num > 0)
            {
                this.b.Text = this.b.Text.Substring(0, num + 2);
            }
            this.c(A_0);
        }

        public void b(string A_0)
        {
            this.c(A_0 + "\r\n");
        }

        public void c(string A_0)
        {
            this.b.AppendText(A_0);
            this.b.ScrollToCaret();
        }
    }
}

