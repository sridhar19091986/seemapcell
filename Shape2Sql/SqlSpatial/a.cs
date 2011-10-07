namespace SqlSpatial
{
    using System;
    using System.ComponentModel;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows.Forms;

    public class a : Form
    {
        private IContainer a;
        private Label b;
        private TextBox c;
        private GroupBox d;
        private RadioButton e;
        private RadioButton f;
        private TextBox g;
        private TextBox h;
        private CheckBox i;
        private Label j;
        private Label k;
        private GroupBox l;
        private RadioButton m;
        private TextBox n;
        private Label o;
        private TextBox p;
        private Button q;
        private RadioButton r;
        private ComboBox s;
        private Button t;
        private Button u;
        private OpenFileDialog v;

        public a(SqlSpatial.a.a A_0)
        {
            this.a();
            if (A_0 != null)
            {
                this.f.Checked = A_0.c();
                this.e.Checked = !A_0.c();
                this.c.Text = A_0.h();
                this.i.Checked = A_0.i();
                if (!A_0.c())
                {
                    this.h.Text = A_0.e();
                    this.g.Text = A_0.a();
                }
                if (!string.IsNullOrEmpty(A_0.d()))
                {
                    this.m.Checked = true;
                    this.s.Text = A_0.d();
                }
                else if (!string.IsNullOrEmpty(A_0.b()))
                {
                    this.r.Checked = true;
                    this.p.Text = A_0.b();
                    this.n.Text = A_0.f();
                }
                else
                {
                    this.m.Checked = true;
                }
            }
            else
            {
                this.f.Checked = this.m.Checked = true;
            }
        }

        private void a()
        {
            this.b = new Label();
            this.c = new TextBox();
            this.d = new GroupBox();
            this.g = new TextBox();
            this.h = new TextBox();
            this.i = new CheckBox();
            this.j = new Label();
            this.k = new Label();
            this.e = new RadioButton();
            this.f = new RadioButton();
            this.l = new GroupBox();
            this.n = new TextBox();
            this.o = new Label();
            this.p = new TextBox();
            this.q = new Button();
            this.r = new RadioButton();
            this.s = new ComboBox();
            this.m = new RadioButton();
            this.t = new Button();
            this.u = new Button();
            this.v = new OpenFileDialog();
            this.d.SuspendLayout();
            this.l.SuspendLayout();
            base.SuspendLayout();
            this.b.AutoSize = true;
            this.b.Location = new Point(13, 13);
            this.b.Name = "label1";
            this.b.Size = new Size(70, 13);
            this.b.TabIndex = 0;
            this.b.Text = "Server name:";
            this.c.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.c.Location = new Point(0x59, 10);
            this.c.Name = "tbServerName";
            this.c.Size = new Size(0xeb, 20);
            this.c.TabIndex = 1;
            this.d.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.d.Controls.Add(this.g);
            this.d.Controls.Add(this.h);
            this.d.Controls.Add(this.i);
            this.d.Controls.Add(this.j);
            this.d.Controls.Add(this.k);
            this.d.Controls.Add(this.e);
            this.d.Controls.Add(this.f);
            this.d.Location = new Point(12, 0x24);
            this.d.Name = "groupBox1";
            this.d.Size = new Size(0x13e, 0x94);
            this.d.TabIndex = 2;
            this.d.TabStop = false;
            this.d.Text = "Log on to the server";
            this.g.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.g.Location = new Point(0x62, 0x5f);
            this.g.Name = "tbPassword";
            this.g.Size = new Size(0xd6, 20);
            this.g.TabIndex = 6;
            this.g.UseSystemPasswordChar = true;
            this.h.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.h.Location = new Point(0x62, 0x44);
            this.h.Name = "tbUsername";
            this.h.Size = new Size(0xd6, 20);
            this.h.TabIndex = 5;
            this.i.AutoSize = true;
            this.i.Location = new Point(0x62, 0x79);
            this.i.Name = "chbSavePassword";
            this.i.Size = new Size(0x73, 0x11);
            this.i.TabIndex = 4;
            this.i.Text = "Save my password";
            this.i.UseVisualStyleBackColor = true;
            this.j.AutoSize = true;
            this.j.Location = new Point(0x21, 0x62);
            this.j.Name = "lbPassword";
            this.j.Size = new Size(0x38, 13);
            this.j.TabIndex = 3;
            this.j.Text = "Password:";
            this.k.AutoSize = true;
            this.k.Location = new Point(0x21, 0x47);
            this.k.Name = "lbUsername";
            this.k.Size = new Size(0x3d, 13);
            this.k.TabIndex = 2;
            this.k.Text = "User name:";
            this.e.AutoSize = true;
            this.e.Location = new Point(12, 0x2c);
            this.e.Name = "rbSqlServerAuth";
            this.e.Size = new Size(0xad, 0x11);
            this.e.TabIndex = 1;
            this.e.Text = "Use SQL Server Authentication";
            this.e.UseVisualStyleBackColor = true;
            this.e.CheckedChanged += new EventHandler(this.c);
            this.f.AutoSize = true;
            this.f.Location = new Point(12, 0x15);
            this.f.Name = "rbWindowsAuth";
            this.f.Size = new Size(0xa2, 0x11);
            this.f.TabIndex = 0;
            this.f.Text = "Use Windows Authentication";
            this.f.UseVisualStyleBackColor = true;
            this.f.CheckedChanged += new EventHandler(this.c);
            this.l.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.l.Controls.Add(this.n);
            this.l.Controls.Add(this.o);
            this.l.Controls.Add(this.p);
            this.l.Controls.Add(this.q);
            this.l.Controls.Add(this.r);
            this.l.Controls.Add(this.s);
            this.l.Controls.Add(this.m);
            this.l.Location = new Point(12, 0xbf);
            this.l.Name = "groupBox2";
            this.l.Size = new Size(0x13e, 0xac);
            this.l.TabIndex = 3;
            this.l.TabStop = false;
            this.l.Text = "Connect to a database";
            this.n.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.n.Location = new Point(30, 140);
            this.n.Name = "tbAttachDatabaseLogicalName";
            this.n.Size = new Size(0x119, 20);
            this.n.TabIndex = 6;
            this.o.AutoSize = true;
            this.o.Location = new Point(30, 0x7b);
            this.o.Name = "lbAttachDatabaseLogicalName";
            this.o.Size = new Size(0x49, 13);
            this.o.TabIndex = 5;
            this.o.Text = "Logical name:";
            this.p.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.p.Location = new Point(30, 0x60);
            this.p.Name = "tbAttachDatabaseFileName";
            this.p.ReadOnly = true;
            this.p.Size = new Size(0xd4, 20);
            this.p.TabIndex = 4;
            this.q.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.q.Location = new Point(0xf8, 0x5e);
            this.q.Name = "btnBrowse";
            this.q.Size = new Size(0x3f, 0x17);
            this.q.TabIndex = 3;
            this.q.Text = "Browse...";
            this.q.UseVisualStyleBackColor = true;
            this.q.Click += new EventHandler(this.a);
            this.r.AutoSize = true;
            this.r.Location = new Point(12, 0x48);
            this.r.Name = "rbDatabaseFile";
            this.r.Size = new Size(0x83, 0x11);
            this.r.TabIndex = 2;
            this.r.Text = "Attach a database file:";
            this.r.UseVisualStyleBackColor = true;
            this.r.CheckedChanged += new EventHandler(this.b);
            this.s.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.s.FormattingEnabled = true;
            this.s.Location = new Point(30, 0x2c);
            this.s.Name = "cmbDatabaseName";
            this.s.Size = new Size(0x11a, 0x15);
            this.s.TabIndex = 1;
            this.s.DropDown += new EventHandler(this.d);
            this.m.AutoSize = true;
            this.m.Location = new Point(12, 20);
            this.m.Name = "rbDatabaseName";
            this.m.Size = new Size(0xb6, 0x11);
            this.m.TabIndex = 0;
            this.m.Text = "Select or enter a database name:";
            this.m.UseVisualStyleBackColor = true;
            this.m.CheckedChanged += new EventHandler(this.b);
            this.t.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.t.DialogResult = DialogResult.OK;
            this.t.Location = new Point(260, 0x175);
            this.t.Name = "btnOK";
            this.t.Size = new Size(0x40, 0x17);
            this.t.TabIndex = 4;
            this.t.Text = "OK";
            this.t.UseVisualStyleBackColor = true;
            this.u.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.u.DialogResult = DialogResult.Cancel;
            this.u.Location = new Point(0xc1, 0x175);
            this.u.Name = "btnCancel";
            this.u.Size = new Size(0x3d, 0x17);
            this.u.TabIndex = 5;
            this.u.Text = "Cancel";
            this.u.UseVisualStyleBackColor = true;
            this.v.FileName = "openFileDialog1";
            base.AcceptButton = this.t;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.u;
            base.ClientSize = new Size(0x156, 0x198);
            base.ControlBox = false;
            base.Controls.Add(this.u);
            base.Controls.Add(this.t);
            base.Controls.Add(this.l);
            base.Controls.Add(this.d);
            base.Controls.Add(this.c);
            base.Controls.Add(this.b);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x15c, 0x1b2);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x15c, 0x1b2);
            base.Name = "SqlServerConfigurationForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Database Configuration";
            this.d.ResumeLayout(false);
            this.d.PerformLayout();
            this.l.ResumeLayout(false);
            this.l.PerformLayout();
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
            if (this.v.ShowDialog() == DialogResult.OK)
            {
                this.p.Text = this.v.FileName;
            }
        }

        private void b(object A_0, EventArgs A_1)
        {
            this.p.Enabled = this.q.Enabled = this.n.Enabled = this.o.Enabled = this.r.Checked;
            this.s.Enabled = !this.r.Checked;
        }

        private void c(object A_0, EventArgs A_1)
        {
            this.h.Enabled = this.g.Enabled = this.i.Enabled = this.k.Enabled = this.j.Enabled = this.e.Checked;
        }

        private void d(object A_0, EventArgs A_1)
        {
            if (!string.IsNullOrEmpty(this.c.Text))
            {
                StringBuilder builder = new StringBuilder();
                string cmdText = "SELECT NAME FROM [master].[sys].[databases]";
                builder.AppendFormat("Data Source = {0};", this.c.Text);
                builder.Append("Initial Catalog = master;");
                if (this.f.Checked)
                {
                    builder.Append("Integrated Security=SSPI;");
                }
                else
                {
                    builder.AppendFormat("User ID={0};Password={1};", this.h.Text, this.g.Text);
                }
                string connectionString = builder.ToString();
                this.s.Items.Clear();
                this.s.Cursor = Cursors.WaitCursor;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(cmdText, connection);
                        command.CommandTimeout = 15;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            this.s.Items.Add(reader[0]);
                        }
                        connection.Close();
                    }
                }
                catch
                {
                }
                this.s.Cursor = Cursors.Default;
            }
        }

        public SqlSpatial.a.a Configuration
        {
            get
            {
                SqlSpatial.a.a a = new SqlSpatial.a.a();
                a.a(this.f.Checked);
                if (this.e.Checked)
                {
                    a.e(this.h.Text);
                    a.b(this.g.Text);
                    a.b(this.i.Checked);
                }
                a.a(this.c.Text);
                if (this.m.Checked)
                {
                    a.f(this.s.Text);
                    return a;
                }
                if (this.r.Checked)
                {
                    a.c(this.p.Text);
                    a.d(this.n.Text);
                }
                return a;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.c().ToString();
            }
        }

        [Serializable]
        public class a : ISerializable
        {
            [CompilerGenerated]
            private bool a;
            [CompilerGenerated]
            private string b;
            [CompilerGenerated]
            private string c;
            [CompilerGenerated]
            private string d;
            [CompilerGenerated]
            private string e;
            [CompilerGenerated]
            private string f;
            [CompilerGenerated]
            private string g;
            [CompilerGenerated]
            private bool h;

            public a()
            {
            }

            public a(SerializationInfo A_0, StreamingContext A_1)
            {
                this.a((bool) A_0.GetValue("a", typeof(bool)));
                this.e((string) A_0.GetValue("b", typeof(string)));
                this.a((string) A_0.GetValue("c", typeof(string)));
                this.b((string) A_0.GetValue("d", typeof(string)));
                this.f((string) A_0.GetValue("e", typeof(string)));
                this.c((string) A_0.GetValue("f", typeof(string)));
                this.d((string) A_0.GetValue("g", typeof(string)));
                this.b((bool) A_0.GetValue("h", typeof(bool)));
            }

            public void a(SerializationInfo A_0, StreamingContext A_1)
            {
                A_0.AddValue("a", this.c());
                A_0.AddValue("b", this.e());
                A_0.AddValue("c", this.h());
                A_0.AddValue("d", this.a());
                A_0.AddValue("e", this.d());
                A_0.AddValue("f", this.b());
                A_0.AddValue("g", this.f());
                A_0.AddValue("h", this.i());
            }

            public override string g()
            {
                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrEmpty(this.d()))
                {
                    builder.AppendFormat("Data Source = {0};", this.h());
                    builder.AppendFormat("Initial Catalog = {0};", this.d());
                    if (this.c())
                    {
                        builder.Append("Integrated Security=SSPI;");
                    }
                    else
                    {
                        builder.AppendFormat("User ID={0};Password={1};", this.e(), this.a());
                    }
                }
                else if (!string.IsNullOrEmpty(this.b()))
                {
                    builder.AppendFormat("Server={0};AttachDbFilename={1};Database={2};Trusted_Connection=Yes;", this.h(), this.b(), this.f());
                    if (!this.c())
                    {
                        builder.AppendFormat("User ID={0};Password={1};", this.e(), this.a());
                    }
                }
                return builder.ToString();
            }

            public string AttachDatabaseFileName
            {
                [CompilerGenerated]
                get
                {
                    return this.f;
                }
                [CompilerGenerated]
                set
                {
                    this.f = value;
                }
            }

            public string DatabaseName
            {
                [CompilerGenerated]
                get
                {
                    return this.e;
                }
                [CompilerGenerated]
                set
                {
                    this.e = value;
                }
            }

            public string LogicalName
            {
                [CompilerGenerated]
                get
                {
                    return this.g;
                }
                [CompilerGenerated]
                set
                {
                    this.g = value;
                }
            }

            public string Password
            {
                [CompilerGenerated]
                get
                {
                    return this.d;
                }
                [CompilerGenerated]
                set
                {
                    this.d = value;
                }
            }

            public bool SavePassword
            {
                [CompilerGenerated]
                get
                {
                    return this.h;
                }
                [CompilerGenerated]
                set
                {
                    this.h = value;
                }
            }

            public string ServerName
            {
                [CompilerGenerated]
                get
                {
                    return this.c;
                }
                [CompilerGenerated]
                set
                {
                    this.c = value;
                }
            }

            public string Username
            {
                [CompilerGenerated]
                get
                {
                    return this.b;
                }
                [CompilerGenerated]
                set
                {
                    this.b = value;
                }
            }

            public bool UseWindowsAuthentication
            {
                [CompilerGenerated]
                get
                {
                    return this.a;
                }
                [CompilerGenerated]
                set
                {
                    this.a = value;
                }
            }
        }
    }
}

