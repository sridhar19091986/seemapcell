namespace Shape2Sql
{
    using SharpGIS.Gis;
    using SharpGIS.Gis.Data.ShapeClient;
    using SharpGIS.Gis.Data.SqlServer;
    using SharpGIS.Gis.Geometries;
    using SqlSpatial;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class d : Form
    {
        private IContainer a;
        private BackgroundWorker aa;
        private ToolStripStatusLabel ab;
        private Button ac;
        private TextBox ad;
        private ToolTip ae;
        private PictureBox af;
        private SqlSpatial.a.a ag;
        private Envelope ah;
        private SharpGIS.Gis.Data.SqlServer.a ai;
        private Button b;
        private Label c;
        private TextBox d;
        private GroupBox e;
        private Label f;
        private GroupBox g;
        public TextBox h;
        public CheckBox i;
        public CheckBox j;
        private Button k;
        private Button l;
        private RadioButton m;
        private RadioButton n;
        private Label o;
        public TextBox p;
        public CheckBox q;
        private Label r;
        private GroupBox s;
        private CheckedListBox t;
        private Label u;
        public TextBox v;
        private Label w;
        public TextBox x;
        private StatusStrip y;
        private ToolStripProgressBar z;

        public d()
        {
            this.d();
        }

        private void a()
        {
            if (this.ag == null)
            {
                this.ad.Text = "Please configure connection...";
            }
            else if (!string.IsNullOrEmpty(this.ag.b()))
            {
                this.ad.Text = this.ag.f() + " @ " + this.ag.h();
            }
            else
            {
                this.ad.Text = this.ag.d() + " @ " + this.ag.h();
            }
        }

        private static string a(Feature A_0)
        {
            StringBuilder builder = new StringBuilder();
            if ((A_0 != null) && (A_0.Attributes != null))
            {
                int num = 0;
                foreach (string str in A_0.Attributes.Keys)
                {
                    if (num > 10)
                    {
                        builder.Append("\r\n...");
                        break;
                    }
                    builder.AppendFormat("\r\n{0}: {1}", str, A_0.Attributes[str]);
                }
            }
            return builder.ToString();
        }

        protected override void a(bool A_0)
        {
            if (A_0 && (this.a != null))
            {
                this.a.Dispose();
            }
            base.Dispose(A_0);
        }

        private void a(object A_0, SharpGIS.Gis.Data.SqlServer.a.b A_1)
        {
            DialogResult result = MessageBox.Show(string.Format("Could not insert row #{0}:{1}\r\n\r\n{2}: {3}\r\n\r\nSelect OK to ignore or Cancel to abort", new object[] { A_1.b(), a(A_1.a()), A_1.c().GetType(), A_1.c().Message }), "Error inserting row", MessageBoxButtons.OKCancel);
            A_1.a(result == DialogResult.Cancel);
        }

        private void a(object A_0, ProgressChangedEventArgs A_1)
        {
            this.z.Minimum = 0;
            this.z.Value = A_1.ProgressPercentage;
            this.z.Visible = (A_1.ProgressPercentage > 0) && (A_1.ProgressPercentage < 100);
            if (A_1.UserState is Exception)
            {
                MessageBox.Show((A_1.UserState as Exception).Message, "\x00cbrror");
            }
            else
            {
                string userState = (string) A_1.UserState;
                this.ab.Text = userState;
            }
        }

        private void a(object A_0, RunWorkerCompletedEventArgs A_1)
        {
            this.z.Value = 0;
            this.ab.Text = "";
            this.k.Text = "Upload to Database";
            this.k.Enabled = true;
            this.ai = null;
        }

        private void a(object A_0, EventArgs A_1)
        {
            this.c();
        }

        private void a(object A_0, FormClosingEventArgs A_1)
        {
            Shape2Sql.b.a("LastFile", this.d.Text);
            Shape2Sql.b.a("GeomColumnName", this.x.Text);
            Shape2Sql.b.a("IDColumnName", this.v.Text);
            Shape2Sql.b.a("SRID", this.h.Text);
            if (this.ag != null)
            {
                if (!this.ag.i())
                {
                    this.ag.e(null);
                    this.ag.b((string) null);
                }
                Shape2Sql.b.a("Connection", this.ag);
            }
        }

        private string b()
        {
            return this.d.Text.Substring(this.d.Text.LastIndexOf('\\') + 1, (this.d.Text.LastIndexOf('.') - this.d.Text.LastIndexOf('\\')) - 1);
        }

        private void b(object A_0, EventArgs A_1)
        {
            this.c();
        }

        private void c()
        {
            this.af.Visible = false;
            this.ae.SetToolTip(this.n, null);
            this.ae.SetToolTip(this.af, "Data projection or extent is outside the bounds of what is supported by the SqlGeography type");
            if (this.n.Checked)
            {
                string toolTip = this.ae.GetToolTip(this.r);
                if (((this.ah != null) && (((this.ah.MinX < -180.0) || (this.ah.MaxX > 180.0)) || ((this.ah.MinY < -90.0) || (this.ah.MaxY > 90.0)))) || ((toolTip != null) && toolTip.StartsWith("PROJCS[")))
                {
                    this.af.Visible = true;
                    this.ae.SetToolTip(this.n, "Data projection or extent is outside the bounds of what is supported by the SqlGeography type");
                }
            }
        }

        private void c(object A_0, EventArgs A_1)
        {
            SqlSpatial.a a = new SqlSpatial.a(this.ag);
            if (a.ShowDialog() == DialogResult.OK)
            {
                this.ag = a.c();
                this.a();
            }
        }

        private void d()
        {
            this.a = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Shape2Sql.d));
            this.b = new Button();
            this.c = new Label();
            this.d = new TextBox();
            this.e = new GroupBox();
            this.ad = new TextBox();
            this.ac = new Button();
            this.f = new Label();
            this.g = new GroupBox();
            this.o = new Label();
            this.p = new TextBox();
            this.q = new CheckBox();
            this.n = new RadioButton();
            this.m = new RadioButton();
            this.h = new TextBox();
            this.i = new CheckBox();
            this.j = new CheckBox();
            this.k = new Button();
            this.l = new Button();
            this.r = new Label();
            this.s = new GroupBox();
            this.t = new CheckedListBox();
            this.u = new Label();
            this.v = new TextBox();
            this.w = new Label();
            this.x = new TextBox();
            this.y = new StatusStrip();
            this.ab = new ToolStripStatusLabel();
            this.z = new ToolStripProgressBar();
            this.aa = new BackgroundWorker();
            this.ae = new ToolTip(this.a);
            this.af = new PictureBox();
            this.e.SuspendLayout();
            this.g.SuspendLayout();
            this.s.SuspendLayout();
            this.y.SuspendLayout();
            ((ISupportInitialize) this.af).BeginInit();
            base.SuspendLayout();
            this.b.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.b.Location = new System.Drawing.Point(0x18d, 4);
            this.b.Name = "btnSelectShp";
            this.b.Size = new Size(0x1c, 0x17);
            this.b.TabIndex = 2;
            this.b.Text = "...";
            this.b.UseVisualStyleBackColor = true;
            this.b.Click += new EventHandler(this.i);
            this.c.AutoSize = true;
            this.c.Location = new System.Drawing.Point(12, 9);
            this.c.Name = "label1";
            this.c.Size = new Size(0x33, 13);
            this.c.TabIndex = 3;
            this.c.Text = "Shapefile";
            this.d.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.d.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.d.AutoCompleteSource = AutoCompleteSource.FileSystem;
            this.d.Location = new System.Drawing.Point(0x45, 6);
            this.d.Name = "tbShapefile";
            this.d.Size = new Size(0x142, 20);
            this.d.TabIndex = 4;
            this.d.TextChanged += new EventHandler(this.j);
            this.e.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.e.Controls.Add(this.ad);
            this.e.Controls.Add(this.ac);
            this.e.Controls.Add(this.f);
            this.e.Location = new System.Drawing.Point(14, 0x44);
            this.e.Name = "groupBox1";
            this.e.Size = new Size(0x19c, 0x36);
            this.e.TabIndex = 6;
            this.e.TabStop = false;
            this.e.Text = "Database properties";
            this.ad.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.ad.BackColor = SystemColors.Info;
            this.ad.Location = new System.Drawing.Point(0x37, 20);
            this.ad.Name = "tbDatabase";
            this.ad.ReadOnly = true;
            this.ad.Size = new Size(270, 20);
            this.ad.TabIndex = 0x18;
            this.ac.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.ac.Location = new System.Drawing.Point(0x14b, 0x13);
            this.ac.Name = "btnConfigureDatabase";
            this.ac.Size = new Size(0x4b, 0x17);
            this.ac.TabIndex = 0x17;
            this.ac.Text = "Configure...";
            this.ac.UseVisualStyleBackColor = true;
            this.ac.Click += new EventHandler(this.c);
            this.f.Location = new System.Drawing.Point(8, 0x16);
            this.f.Name = "label6";
            this.f.Size = new Size(40, 0x17);
            this.f.TabIndex = 20;
            this.f.Text = "Server";
            this.g.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.g.Controls.Add(this.af);
            this.g.Controls.Add(this.o);
            this.g.Controls.Add(this.p);
            this.g.Controls.Add(this.q);
            this.g.Controls.Add(this.n);
            this.g.Controls.Add(this.m);
            this.g.Controls.Add(this.h);
            this.g.Controls.Add(this.i);
            this.g.Controls.Add(this.j);
            this.g.Enabled = false;
            this.g.Location = new System.Drawing.Point(14, 0x80);
            this.g.Name = "groupBox2";
            this.g.Size = new Size(0xc7, 0xaf);
            this.g.TabIndex = 7;
            this.g.TabStop = false;
            this.g.Text = "Geometry properties";
            this.o.Location = new System.Drawing.Point(6, 0x94);
            this.o.Name = "label5";
            this.o.Size = new Size(0x47, 20);
            this.o.TabIndex = 0x1f;
            this.o.Text = "Table Name";
            this.p.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.p.Location = new System.Drawing.Point(0x4b, 0x92);
            this.p.Name = "tbTableName";
            this.p.Size = new Size(0x74, 20);
            this.p.TabIndex = 30;
            this.p.TextAlign = HorizontalAlignment.Right;
            this.q.Checked = true;
            this.q.CheckState = CheckState.Checked;
            this.q.Location = new System.Drawing.Point(6, 0x79);
            this.q.Name = "chbCreateIndex";
            this.q.Size = new Size(0x7c, 0x18);
            this.q.TabIndex = 0x1d;
            this.q.Text = "Create Spatial Index";
            this.n.AutoSize = true;
            this.n.Location = new System.Drawing.Point(6, 0x48);
            this.n.Name = "rbGeography";
            this.n.Size = new Size(0x7a, 0x11);
            this.n.TabIndex = 0x18;
            this.n.Text = "Geography (Spheric)";
            this.n.UseVisualStyleBackColor = true;
            this.n.CheckedChanged += new EventHandler(this.a);
            this.m.AutoSize = true;
            this.m.Checked = true;
            this.m.Location = new System.Drawing.Point(6, 0x31);
            this.m.Name = "rbGeometry";
            this.m.Size = new Size(0x67, 0x11);
            this.m.TabIndex = 0x17;
            this.m.TabStop = true;
            this.m.Text = "Planar Geometry";
            this.m.UseVisualStyleBackColor = true;
            this.m.CheckedChanged += new EventHandler(this.b);
            this.h.Enabled = false;
            this.h.Location = new System.Drawing.Point(0x4d, 0x61);
            this.h.Name = "tbSRID";
            this.h.Size = new Size(0x23, 20);
            this.h.TabIndex = 0x15;
            this.h.Text = "4326";
            this.h.TextAlign = HorizontalAlignment.Right;
            this.i.Location = new System.Drawing.Point(6, 0x5f);
            this.i.Name = "chbSRID";
            this.i.Size = new Size(0x47, 0x18);
            this.i.TabIndex = 20;
            this.i.Text = "Set SRID";
            this.i.CheckedChanged += new EventHandler(this.h);
            this.j.BackColor = Color.Transparent;
            this.j.Checked = true;
            this.j.CheckState = CheckState.Checked;
            this.j.Location = new System.Drawing.Point(6, 0x13);
            this.j.Name = "chbReplace";
            this.j.Size = new Size(0x84, 0x18);
            this.j.TabIndex = 0x13;
            this.j.Text = "Replace existing table";
            this.j.UseVisualStyleBackColor = false;
            this.k.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.k.Location = new System.Drawing.Point(12, 0x135);
            this.k.Name = "btnUpload";
            this.k.Size = new Size(0x7e, 0x17);
            this.k.TabIndex = 0x16;
            this.k.Text = "Upload to Database";
            this.k.UseVisualStyleBackColor = true;
            this.k.Click += new EventHandler(this.e);
            this.l.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.l.Location = new System.Drawing.Point(0x15f, 0x135);
            this.l.Name = "btnAbout";
            this.l.Size = new Size(0x4b, 0x17);
            this.l.TabIndex = 9;
            this.l.Text = "About";
            this.l.UseVisualStyleBackColor = true;
            this.l.Click += new EventHandler(this.g);
            this.r.AutoSize = true;
            this.r.Location = new System.Drawing.Point(0x45, 30);
            this.r.Name = "lbShapeInfo";
            this.r.Size = new Size(0x5b, 13);
            this.r.TabIndex = 0x17;
            this.r.Text = "Select a shapefile";
            this.s.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.s.Controls.Add(this.t);
            this.s.Controls.Add(this.u);
            this.s.Controls.Add(this.v);
            this.s.Controls.Add(this.w);
            this.s.Controls.Add(this.x);
            this.s.Location = new System.Drawing.Point(0xda, 0x80);
            this.s.Name = "groupBox3";
            this.s.Size = new Size(0xd0, 0xaf);
            this.s.TabIndex = 0x19;
            this.s.TabStop = false;
            this.s.Text = "Attributes";
            this.t.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.t.FormattingEnabled = true;
            this.t.Location = new System.Drawing.Point(6, 0x4d);
            this.t.Name = "clbColumns";
            this.t.Size = new Size(0xc4, 0x4f);
            this.t.TabIndex = 0x21;
            this.u.Location = new System.Drawing.Point(6, 50);
            this.u.Name = "label4";
            this.u.Size = new Size(0x57, 20);
            this.u.TabIndex = 0x20;
            this.u.Text = "ID Column Name";
            this.v.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.v.Location = new System.Drawing.Point(0x63, 0x2e);
            this.v.Name = "tbObjectIDColumn";
            this.v.Size = new Size(0x67, 20);
            this.v.TabIndex = 0x1f;
            this.v.Text = "OID";
            this.v.TextAlign = HorizontalAlignment.Right;
            this.w.Location = new System.Drawing.Point(6, 0x18);
            this.w.Name = "label2";
            this.w.Size = new Size(90, 20);
            this.w.TabIndex = 30;
            this.w.Text = "Geometry Name";
            this.x.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.x.Location = new System.Drawing.Point(0x63, 20);
            this.x.Name = "tbGeomColumnName";
            this.x.Size = new Size(0x67, 20);
            this.x.TabIndex = 0x1d;
            this.x.Text = "shape";
            this.x.TextAlign = HorizontalAlignment.Right;
            this.y.Items.AddRange(new ToolStripItem[] { this.ab, this.z });
            this.y.Location = new System.Drawing.Point(0, 340);
            this.y.Name = "statusStrip1";
            this.y.Size = new Size(0x1b5, 0x16);
            this.y.TabIndex = 0x1a;
            this.y.Text = "statusStrip1";
            this.ab.Name = "toolStripStatusLabel1";
            this.ab.Size = new Size(0, 0x11);
            this.z.Alignment = ToolStripItemAlignment.Right;
            this.z.Name = "toolStripProgressBar1";
            this.z.Size = new Size(200, 0x10);
            this.z.Step = 1;
            this.z.Visible = false;
            this.aa.WorkerReportsProgress = true;
            this.aa.WorkerSupportsCancellation = true;
            this.af.BackColor = Color.Transparent;
            this.af.Image = (Image) manager.GetObject("warningGeog.Image");
            this.af.Location = new System.Drawing.Point(0x7e, 0x48);
            this.af.Name = "warningGeog";
            this.af.Size = new Size(20, 20);
            this.af.SizeMode = PictureBoxSizeMode.Zoom;
            this.af.TabIndex = 0x20;
            this.af.TabStop = false;
            this.af.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1b5, 0x16a);
            base.Controls.Add(this.y);
            base.Controls.Add(this.s);
            base.Controls.Add(this.r);
            base.Controls.Add(this.l);
            base.Controls.Add(this.g);
            base.Controls.Add(this.k);
            base.Controls.Add(this.e);
            base.Controls.Add(this.d);
            base.Controls.Add(this.c);
            base.Controls.Add(this.b);
            this.MinimumSize = new Size(0x1c5, 400);
            base.Name = "Form1";
            this.Text = "Shapefile Uploader for SQL Server 2008";
            base.FormClosing += new FormClosingEventHandler(this.a);
            base.Load += new EventHandler(this.d);
            this.e.ResumeLayout(false);
            this.e.PerformLayout();
            this.g.ResumeLayout(false);
            this.g.PerformLayout();
            this.s.ResumeLayout(false);
            this.s.PerformLayout();
            this.y.ResumeLayout(false);
            this.y.PerformLayout();
            ((ISupportInitialize) this.af).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void d(object A_0, EventArgs A_1)
        {
            this.d.Text = Shape2Sql.b.a("LastFile") as string;
            this.x.Text = (Shape2Sql.b.a("GeomColumnName") as string) ?? "geom";
            this.v.Text = (Shape2Sql.b.a("IDColumnName") as string) ?? "ID";
            this.h.Text = (Shape2Sql.b.a("SRID") as string) ?? "4326";
            this.j(null, null);
            this.ag = Shape2Sql.b.GetValue<SqlSpatial.a.a>("Connection");
            if (this.ag == null)
            {
                this.c(null, null);
            }
            else
            {
                this.a();
            }
        }

        private void e(object A_0, EventArgs A_1)
        {
            if (this.n.Checked)
            {
                if (!this.i.Checked)
                {
                    MessageBox.Show("Geography type requires a valid Spatial Reference ID (SRID) to be set");
                    return;
                }
                if ((((this.ah.MinX < -180.0) || (this.ah.MaxX > 180.0)) || ((this.ah.MinY < -90.0) || (this.ah.MaxY > 90.0))) && (MessageBox.Show("Data seems to extend beyond the valid bounds supported by Geography type. Continue?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel))
                {
                    return;
                }
                string toolTip = this.ae.GetToolTip(this.r);
                if (((toolTip != null) && toolTip.StartsWith("PROJCS[")) && (MessageBox.Show("Shape projection data claims it contains projected data, which is not supported by the Geography type. Continue?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel))
                {
                    return;
                }
            }
            if (this.k.Text == "Cancel")
            {
                this.k.Text = "Cancelling...";
                this.k.Enabled = false;
                if (this.ai != null)
                {
                    this.ai.b();
                }
            }
            else
            {
                if (this.v.Text.Trim().Length > 0)
                {
                    for (int i = 0; i < this.t.Items.Count; i++)
                    {
                        if (this.t.GetItemChecked(i))
                        {
                            string str2 = (string) this.t.Items[i];
                            if (str2.Equals(this.v.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Name conflict on ID column and Shape data column");
                                return;
                            }
                        }
                    }
                }
                List<string> list = new List<string>();
                foreach (object obj2 in this.t.CheckedItems)
                {
                    list.Add((string) obj2);
                }
                this.ai = new SharpGIS.Gis.Data.SqlServer.a(this.ag.ToString(), this.p.Text, this.d.Text, this.i.Checked, int.Parse(this.h.Text), this.n.Checked, this.v.Text.Trim(), this.x.Text.Trim(), this.q.Checked, this.j.Checked, list);
                this.k.Text = "Cancel";
                this.ai.a(new RunWorkerCompletedEventHandler(this.a));
                this.ai.b(new ProgressChangedEventHandler(this.a));
                this.ai.a(new SharpGIS.Gis.Data.SqlServer.a.a(this.a));
                this.ai.a();
            }
        }

        private void f(object A_0, EventArgs A_1)
        {
            this.q.Enabled = !string.IsNullOrEmpty(this.v.Text.Trim());
        }

        private void g(object A_0, EventArgs A_1)
        {
            new Shape2Sql.a().ShowDialog();
        }

        private void h(object A_0, EventArgs A_1)
        {
            this.h.Enabled = this.i.Checked;
        }

        private void i(object A_0, EventArgs A_1)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (File.Exists(this.d.Text))
            {
                dialog.FileName = this.d.Text;
            }
            dialog.Filter = "Shapefile (*.shp)|*.shp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.d.Text = dialog.FileName;
            }
        }

        private void j(object A_0, EventArgs A_1)
        {
            bool flag = this.d.Text.EndsWith(".shp", StringComparison.OrdinalIgnoreCase) && File.Exists(this.d.Text);
            this.g.Enabled = flag;
            this.s.Enabled = flag;
            this.t.Items.Clear();
            this.ae.SetToolTip(this.r, null);
            this.af.Visible = false;
            this.ah = null;
            if (flag && File.Exists(this.d.Text))
            {
                try
                {
                    using (SharpGIS.Gis.Data.ShapeClient.b b = new SharpGIS.Gis.Data.ShapeClient.b(this.d.Text))
                    {
                        this.ah = b.h();
                        this.r.Text = string.Format("{0} {1} features in shapefile.\nExtent: {2:#.####},{3:#.####} -> {4:#.####},{5:#.####}", new object[] { b.d(), b.b(), b.h().MinX, b.h().MinY, b.h().MaxX, b.h().MaxY });
                        this.ae.SetToolTip(this.r, b.e());
                        DataTable schemaTable = b.GetSchemaTable();
                        b.b();
                        b.Close();
                        this.p.Text = this.b();
                        if (schemaTable != null)
                        {
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                this.t.Items.Add(row["ColumnName"], true);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.r.Text = exception.Message;
                }
            }
            else
            {
                this.r.Text = "Invalid shapefile";
            }
            this.c();
        }
    }
}

