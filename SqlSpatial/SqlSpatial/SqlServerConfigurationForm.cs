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

    public class SqlServerConfigurationForm : Form
    {
        private Button btnBrowse;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox chbSavePassword;
        private ComboBox cmbDatabaseName;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label lbAttachDatabaseLogicalName;
        private Label lbPassword;
        private Label lbUsername;
        private OpenFileDialog openFileDialog1;
        private RadioButton rbDatabaseFile;
        private RadioButton rbDatabaseName;
        private RadioButton rbSqlServerAuth;
        private RadioButton rbWindowsAuth;
        private TextBox tbAttachDatabaseFileName;
        private TextBox tbAttachDatabaseLogicalName;
        private TextBox tbPassword;
        private TextBox tbServerName;
        private TextBox tbUsername;

        public SqlServerConfigurationForm(SqlServerConfiguration config)
        {
            this.InitializeComponent();
            if (config != null)
            {
                this.rbWindowsAuth.Checked = config.UseWindowsAuthentication;
                this.rbSqlServerAuth.Checked = !config.UseWindowsAuthentication;
                this.tbServerName.Text = config.ServerName;
                this.chbSavePassword.Checked = config.SavePassword;
                if (!config.UseWindowsAuthentication)
                {
                    this.tbUsername.Text = config.Username;
                    this.tbPassword.Text = config.Password;
                }
                if (!string.IsNullOrEmpty(config.DatabaseName))
                {
                    this.rbDatabaseName.Checked = true;
                    this.cmbDatabaseName.Text = config.DatabaseName;
                }
                else if (!string.IsNullOrEmpty(config.AttachDatabaseFileName))
                {
                    this.rbDatabaseFile.Checked = true;
                    this.tbAttachDatabaseFileName.Text = config.AttachDatabaseFileName;
                    this.tbAttachDatabaseLogicalName.Text = config.LogicalName;
                }
                else
                {
                    this.rbDatabaseName.Checked = true;
                }
            }
            else
            {
                this.rbWindowsAuth.Checked = this.rbDatabaseName.Checked = true;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.tbAttachDatabaseFileName.Text = this.openFileDialog1.FileName;
            }
        }

        private void cmbDatabaseName_DropDown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbServerName.Text))
            {
                StringBuilder builder = new StringBuilder();
                string cmdText = "SELECT NAME FROM [master].[sys].[databases]";
                builder.AppendFormat("Data Source = {0};", this.tbServerName.Text);
                builder.Append("Initial Catalog = master;");
                if (this.rbWindowsAuth.Checked)
                {
                    builder.Append("Integrated Security=SSPI;");
                }
                else
                {
                    builder.AppendFormat("User ID={0};Password={1};", this.tbUsername.Text, this.tbPassword.Text);
                }
                string connectionString = builder.ToString();
                this.cmbDatabaseName.Items.Clear();
                this.cmbDatabaseName.Cursor = Cursors.WaitCursor;
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
                            this.cmbDatabaseName.Items.Add(reader[0]);
                        }
                        connection.Close();
                    }
                }
                catch
                {
                }
                this.cmbDatabaseName.Cursor = Cursors.Default;
            }
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
            this.label1 = new Label();
            this.tbServerName = new TextBox();
            this.groupBox1 = new GroupBox();
            this.tbPassword = new TextBox();
            this.tbUsername = new TextBox();
            this.chbSavePassword = new CheckBox();
            this.lbPassword = new Label();
            this.lbUsername = new Label();
            this.rbSqlServerAuth = new RadioButton();
            this.rbWindowsAuth = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.tbAttachDatabaseLogicalName = new TextBox();
            this.lbAttachDatabaseLogicalName = new Label();
            this.tbAttachDatabaseFileName = new TextBox();
            this.btnBrowse = new Button();
            this.rbDatabaseFile = new RadioButton();
            this.cmbDatabaseName = new ComboBox();
            this.rbDatabaseName = new RadioButton();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.openFileDialog1 = new OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server name:";
            this.tbServerName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbServerName.Location = new Point(0x59, 10);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new Size(0xeb, 20);
            this.tbServerName.TabIndex = 1;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.tbPassword);
            this.groupBox1.Controls.Add(this.tbUsername);
            this.groupBox1.Controls.Add(this.chbSavePassword);
            this.groupBox1.Controls.Add(this.lbPassword);
            this.groupBox1.Controls.Add(this.lbUsername);
            this.groupBox1.Controls.Add(this.rbSqlServerAuth);
            this.groupBox1.Controls.Add(this.rbWindowsAuth);
            this.groupBox1.Location = new Point(12, 0x24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x13e, 0x94);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log on to the server";
            this.tbPassword.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbPassword.Location = new Point(0x62, 0x5f);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new Size(0xd6, 20);
            this.tbPassword.TabIndex = 6;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbUsername.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbUsername.Location = new Point(0x62, 0x44);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new Size(0xd6, 20);
            this.tbUsername.TabIndex = 5;
            this.chbSavePassword.AutoSize = true;
            this.chbSavePassword.Location = new Point(0x62, 0x79);
            this.chbSavePassword.Name = "chbSavePassword";
            this.chbSavePassword.Size = new Size(0x73, 0x11);
            this.chbSavePassword.TabIndex = 4;
            this.chbSavePassword.Text = "Save my password";
            this.chbSavePassword.UseVisualStyleBackColor = true;
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new Point(0x21, 0x62);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new Size(0x38, 13);
            this.lbPassword.TabIndex = 3;
            this.lbPassword.Text = "Password:";
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new Point(0x21, 0x47);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new Size(0x3d, 13);
            this.lbUsername.TabIndex = 2;
            this.lbUsername.Text = "User name:";
            this.rbSqlServerAuth.AutoSize = true;
            this.rbSqlServerAuth.Location = new Point(12, 0x2c);
            this.rbSqlServerAuth.Name = "rbSqlServerAuth";
            this.rbSqlServerAuth.Size = new Size(0xad, 0x11);
            this.rbSqlServerAuth.TabIndex = 1;
            this.rbSqlServerAuth.Text = "Use SQL Server Authentication";
            this.rbSqlServerAuth.UseVisualStyleBackColor = true;
            this.rbSqlServerAuth.CheckedChanged += new EventHandler(this.rbWindowsAuth_CheckedChanged);
            this.rbWindowsAuth.AutoSize = true;
            this.rbWindowsAuth.Location = new Point(12, 0x15);
            this.rbWindowsAuth.Name = "rbWindowsAuth";
            this.rbWindowsAuth.Size = new Size(0xa2, 0x11);
            this.rbWindowsAuth.TabIndex = 0;
            this.rbWindowsAuth.Text = "Use Windows Authentication";
            this.rbWindowsAuth.UseVisualStyleBackColor = true;
            this.rbWindowsAuth.CheckedChanged += new EventHandler(this.rbWindowsAuth_CheckedChanged);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.tbAttachDatabaseLogicalName);
            this.groupBox2.Controls.Add(this.lbAttachDatabaseLogicalName);
            this.groupBox2.Controls.Add(this.tbAttachDatabaseFileName);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.rbDatabaseFile);
            this.groupBox2.Controls.Add(this.cmbDatabaseName);
            this.groupBox2.Controls.Add(this.rbDatabaseName);
            this.groupBox2.Location = new Point(12, 0xbf);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x13e, 0xac);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connect to a database";
            this.tbAttachDatabaseLogicalName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbAttachDatabaseLogicalName.Location = new Point(30, 140);
            this.tbAttachDatabaseLogicalName.Name = "tbAttachDatabaseLogicalName";
            this.tbAttachDatabaseLogicalName.Size = new Size(0x119, 20);
            this.tbAttachDatabaseLogicalName.TabIndex = 6;
            this.lbAttachDatabaseLogicalName.AutoSize = true;
            this.lbAttachDatabaseLogicalName.Location = new Point(30, 0x7b);
            this.lbAttachDatabaseLogicalName.Name = "lbAttachDatabaseLogicalName";
            this.lbAttachDatabaseLogicalName.Size = new Size(0x49, 13);
            this.lbAttachDatabaseLogicalName.TabIndex = 5;
            this.lbAttachDatabaseLogicalName.Text = "Logical name:";
            this.tbAttachDatabaseFileName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbAttachDatabaseFileName.Location = new Point(30, 0x60);
            this.tbAttachDatabaseFileName.Name = "tbAttachDatabaseFileName";
            this.tbAttachDatabaseFileName.ReadOnly = true;
            this.tbAttachDatabaseFileName.Size = new Size(0xd4, 20);
            this.tbAttachDatabaseFileName.TabIndex = 4;
            this.btnBrowse.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnBrowse.Location = new Point(0xf8, 0x5e);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new Size(0x3f, 0x17);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
            this.rbDatabaseFile.AutoSize = true;
            this.rbDatabaseFile.Location = new Point(12, 0x48);
            this.rbDatabaseFile.Name = "rbDatabaseFile";
            this.rbDatabaseFile.Size = new Size(0x83, 0x11);
            this.rbDatabaseFile.TabIndex = 2;
            this.rbDatabaseFile.Text = "Attach a database file:";
            this.rbDatabaseFile.UseVisualStyleBackColor = true;
            this.rbDatabaseFile.CheckedChanged += new EventHandler(this.rbDatabaseName_CheckedChanged);
            this.cmbDatabaseName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.cmbDatabaseName.FormattingEnabled = true;
            this.cmbDatabaseName.Location = new Point(30, 0x2c);
            this.cmbDatabaseName.Name = "cmbDatabaseName";
            this.cmbDatabaseName.Size = new Size(0x11a, 0x15);
            this.cmbDatabaseName.TabIndex = 1;
            this.cmbDatabaseName.DropDown += new EventHandler(this.cmbDatabaseName_DropDown);
            this.rbDatabaseName.AutoSize = true;
            this.rbDatabaseName.Location = new Point(12, 20);
            this.rbDatabaseName.Name = "rbDatabaseName";
            this.rbDatabaseName.Size = new Size(0xb6, 0x11);
            this.rbDatabaseName.TabIndex = 0;
            this.rbDatabaseName.Text = "Select or enter a database name:";
            this.rbDatabaseName.UseVisualStyleBackColor = true;
            this.rbDatabaseName.CheckedChanged += new EventHandler(this.rbDatabaseName_CheckedChanged);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(260, 0x175);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x40, 0x17);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xc1, 0x175);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x3d, 0x17);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.openFileDialog1.FileName = "openFileDialog1";
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x156, 0x198);
            base.ControlBox = false;
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.tbServerName);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x15c, 0x1b2);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x15c, 0x1b2);
            base.Name = "SqlServerConfigurationForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Database Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void rbDatabaseName_CheckedChanged(object sender, EventArgs e)
        {
            this.tbAttachDatabaseFileName.Enabled = this.btnBrowse.Enabled = this.tbAttachDatabaseLogicalName.Enabled = this.lbAttachDatabaseLogicalName.Enabled = this.rbDatabaseFile.Checked;
            this.cmbDatabaseName.Enabled = !this.rbDatabaseFile.Checked;
        }

        private void rbWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            this.tbUsername.Enabled = this.tbPassword.Enabled = this.chbSavePassword.Enabled = this.lbUsername.Enabled = this.lbPassword.Enabled = this.rbSqlServerAuth.Checked;
        }

        public SqlServerConfiguration Configuration
        {
            get
            {
                SqlServerConfiguration configuration = new SqlServerConfiguration();
                configuration.UseWindowsAuthentication = this.rbWindowsAuth.Checked;
                if (this.rbSqlServerAuth.Checked)
                {
                    configuration.Username = this.tbUsername.Text;
                    configuration.Password = this.tbPassword.Text;
                    configuration.SavePassword = this.chbSavePassword.Checked;
                }
                configuration.ServerName = this.tbServerName.Text;
                if (this.rbDatabaseName.Checked)
                {
                    configuration.DatabaseName = this.cmbDatabaseName.Text;
                    return configuration;
                }
                if (this.rbDatabaseFile.Checked)
                {
                    configuration.AttachDatabaseFileName = this.tbAttachDatabaseFileName.Text;
                    configuration.LogicalName = this.tbAttachDatabaseLogicalName.Text;
                }
                return configuration;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.Configuration.ToString();
            }
        }

        [Serializable]
        public class SqlServerConfiguration : ISerializable
        {
            [CompilerGenerated]
            private string <AttachDatabaseFileName>k__BackingField;
            [CompilerGenerated]
            private string <DatabaseName>k__BackingField;
            [CompilerGenerated]
            private string <LogicalName>k__BackingField;
            [CompilerGenerated]
            private string <Password>k__BackingField;
            [CompilerGenerated]
            private bool <SavePassword>k__BackingField;
            [CompilerGenerated]
            private string <ServerName>k__BackingField;
            [CompilerGenerated]
            private string <Username>k__BackingField;
            [CompilerGenerated]
            private bool <UseWindowsAuthentication>k__BackingField;

            public SqlServerConfiguration()
            {
            }

            public SqlServerConfiguration(SerializationInfo info, StreamingContext context)
            {
                this.UseWindowsAuthentication = (bool) info.GetValue("a", typeof(bool));
                this.Username = (string) info.GetValue("b", typeof(string));
                this.ServerName = (string) info.GetValue("c", typeof(string));
                this.Password = (string) info.GetValue("d", typeof(string));
                this.DatabaseName = (string) info.GetValue("e", typeof(string));
                this.AttachDatabaseFileName = (string) info.GetValue("f", typeof(string));
                this.LogicalName = (string) info.GetValue("g", typeof(string));
                this.SavePassword = (bool) info.GetValue("h", typeof(bool));
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("a", this.UseWindowsAuthentication);
                info.AddValue("b", this.Username);
                info.AddValue("c", this.ServerName);
                info.AddValue("d", this.Password);
                info.AddValue("e", this.DatabaseName);
                info.AddValue("f", this.AttachDatabaseFileName);
                info.AddValue("g", this.LogicalName);
                info.AddValue("h", this.SavePassword);
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrEmpty(this.DatabaseName))
                {
                    builder.AppendFormat("Data Source = {0};", this.ServerName);
                    builder.AppendFormat("Initial Catalog = {0};", this.DatabaseName);
                    if (this.UseWindowsAuthentication)
                    {
                        builder.Append("Integrated Security=SSPI;");
                    }
                    else
                    {
                        builder.AppendFormat("User ID={0};Password={1};", this.Username, this.Password);
                    }
                }
                else if (!string.IsNullOrEmpty(this.AttachDatabaseFileName))
                {
                    builder.AppendFormat("Server={0};AttachDbFilename={1};Database={2};Trusted_Connection=Yes;", this.ServerName, this.AttachDatabaseFileName, this.LogicalName);
                    if (!this.UseWindowsAuthentication)
                    {
                        builder.AppendFormat("User ID={0};Password={1};", this.Username, this.Password);
                    }
                }
                return builder.ToString();
            }

            public string AttachDatabaseFileName
            {
                [CompilerGenerated]
                get
                {
                    return this.<AttachDatabaseFileName>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<AttachDatabaseFileName>k__BackingField = value;
                }
            }

            public string DatabaseName
            {
                [CompilerGenerated]
                get
                {
                    return this.<DatabaseName>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<DatabaseName>k__BackingField = value;
                }
            }

            public string LogicalName
            {
                [CompilerGenerated]
                get
                {
                    return this.<LogicalName>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<LogicalName>k__BackingField = value;
                }
            }

            public string Password
            {
                [CompilerGenerated]
                get
                {
                    return this.<Password>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Password>k__BackingField = value;
                }
            }

            public bool SavePassword
            {
                [CompilerGenerated]
                get
                {
                    return this.<SavePassword>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<SavePassword>k__BackingField = value;
                }
            }

            public string ServerName
            {
                [CompilerGenerated]
                get
                {
                    return this.<ServerName>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<ServerName>k__BackingField = value;
                }
            }

            public string Username
            {
                [CompilerGenerated]
                get
                {
                    return this.<Username>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Username>k__BackingField = value;
                }
            }

            public bool UseWindowsAuthentication
            {
                [CompilerGenerated]
                get
                {
                    return this.<UseWindowsAuthentication>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<UseWindowsAuthentication>k__BackingField = value;
                }
            }
        }
    }
}

