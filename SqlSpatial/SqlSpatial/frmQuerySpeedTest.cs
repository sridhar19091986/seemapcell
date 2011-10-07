namespace SqlSpatial
{
    using Microsoft.SqlServer.Types;
    using SharpGIS.Gis;
    using SharpGIS.Gis.Converters;
    using SharpGIS.Gis.Geometries;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class frmQuerySpeedTest : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private ComboBox cmbTables;
        private IContainer components;
        private string database = "GIS";
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lbSqlLookup;
        private Label lbSqlParse;
        private Label lbSqlRows;
        private Label lbWkbLookup;
        private Label lbWkbParse;
        private Label lbWkbRows;
        private QueryPerfCounter perf;
        private string server = "localhost";

        public frmQuerySpeedTest()
        {
            this.InitializeComponent();
            this.perf = new QueryPerfCounter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] numArray = this.Time("", delegate (SqlDataReader reader) {
                object obj2 = reader[0];
                if (obj2 is SqlGeometry)
                {
                    return (obj2 as SqlGeometry).ToGeometry();
                }
                if (!(obj2 is SqlGeography))
                {
                    throw new ArgumentException("Unknown type");
                }
                return (obj2 as SqlGeography).ToGeometry();
            });
            this.lbSqlLookup.Text = string.Format("{0:0} ns", numArray[0]);
            this.lbSqlParse.Text = string.Format("{0:0} ns", numArray[1]);
            this.lbSqlRows.Text = numArray[2].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wkb wkb = new Wkb();
            double[] numArray = this.Time(".STAsBinary()", delegate (SqlDataReader reader) {
                return wkb.Read(reader.GetSqlBinary(0).Value);
            });
            this.lbWkbLookup.Text = string.Format("{0:0} ns", numArray[0]);
            this.lbWkbParse.Text = string.Format("{0:0} ns", numArray[1]);
            this.lbWkbRows.Text = numArray[2].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string geometryColumn = string.Empty;
            using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                geometryColumn = this.GetGeometryColumn(connection, this.cmbTables.Text);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT [{1}] FROM [{0}]", this.cmbTables.Text, geometryColumn);
                List<Feature> list = new List<Feature>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Geometry geometry = null;
                    object obj2 = reader[0];
                    if (obj2 is SqlGeometry)
                    {
                        geometry = (obj2 as SqlGeometry).ToGeometry();
                    }
                    else if (obj2 is SqlGeography)
                    {
                        geometry = (obj2 as SqlGeography).ToGeometry();
                    }
                    if (geometry != null)
                    {
                        Feature item = new Feature();
                        item.Geometry = geometry;
                        list.Add(item);
                    }
                }
                connection.Close();
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

        private void frmQuerySpeedTest_Load(object sender, EventArgs e)
        {
            this.PopulateDropdown();
        }

        private string GetConnectionString()
        {
            return string.Format("Data Source = {0}; Initial Catalog = {1};Integrated Security=SSPI;", this.server, this.database);
        }

        private string GetGeometryColumn(SqlConnection connection, string table)
        {
            SqlCommand command = new SqlCommand(string.Format("SELECT COLUMN_NAME FROM information_schema.columns WHERE table_name='{0}' and [DATA_TYPE] is null", table), connection);
            string str2 = (string) command.ExecuteScalar();
            command.Dispose();
            return str2;
        }

        private void InitializeComponent()
        {
            this.button3 = new Button();
            this.label5 = new Label();
            this.cmbTables = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.lbWkbRows = new Label();
            this.lbSqlRows = new Label();
            this.label3 = new Label();
            this.lbWkbLookup = new Label();
            this.lbWkbParse = new Label();
            this.lbSqlParse = new Label();
            this.label6 = new Label();
            this.label4 = new Label();
            this.lbSqlLookup = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.button2 = new Button();
            this.button1 = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.button3.Location = new Point(5, 0x65);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 0x10;
            this.button3.Text = "Map";
            this.button3.UseVisualStyleBackColor = true;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(60, 9);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x22, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Table";
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new Point(0x74, 6);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new Size(0x79, 0x15);
            this.cmbTables.TabIndex = 14;
            this.groupBox1.Controls.Add(this.lbWkbRows);
            this.groupBox1.Controls.Add(this.lbSqlRows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbWkbLookup);
            this.groupBox1.Controls.Add(this.lbWkbParse);
            this.groupBox1.Controls.Add(this.lbSqlParse);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbSqlLookup);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(0x6a, 0x25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x14d, 0x61);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.lbWkbRows.AutoSize = true;
            this.lbWkbRows.Location = new Point(0x103, 0x3d);
            this.lbWkbRows.Name = "lbWkbRows";
            this.lbWkbRows.Size = new Size(0x1b, 13);
            this.lbWkbRows.TabIndex = 11;
            this.lbWkbRows.Text = "N/A";
            this.lbSqlRows.AutoSize = true;
            this.lbSqlRows.Location = new Point(0x103, 0x2a);
            this.lbSqlRows.Name = "lbSqlRows";
            this.lbSqlRows.Size = new Size(0x1b, 13);
            this.lbSqlRows.TabIndex = 10;
            this.lbSqlRows.Text = "N/A";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x103, 0x10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x22, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Rows";
            this.lbWkbLookup.AutoSize = true;
            this.lbWkbLookup.Location = new Point(80, 0x3d);
            this.lbWkbLookup.Name = "lbWkbLookup";
            this.lbWkbLookup.Size = new Size(0x1b, 13);
            this.lbWkbLookup.TabIndex = 8;
            this.lbWkbLookup.Text = "N/A";
            this.lbWkbParse.AutoSize = true;
            this.lbWkbParse.Location = new Point(170, 0x3d);
            this.lbWkbParse.Name = "lbWkbParse";
            this.lbWkbParse.Size = new Size(0x1b, 13);
            this.lbWkbParse.TabIndex = 7;
            this.lbWkbParse.Text = "N/A";
            this.lbSqlParse.AutoSize = true;
            this.lbSqlParse.Location = new Point(170, 0x2a);
            this.lbSqlParse.Name = "lbSqlParse";
            this.lbSqlParse.Size = new Size(0x1b, 13);
            this.lbSqlParse.TabIndex = 6;
            this.lbSqlParse.Text = "N/A";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(170, 0x10);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Parse time";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(80, 0x10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x41, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Lookup time";
            this.lbSqlLookup.AutoSize = true;
            this.lbSqlLookup.Location = new Point(80, 0x2a);
            this.lbSqlLookup.Name = "lbSqlLookup";
            this.lbSqlLookup.Size = new Size(0x1b, 13);
            this.lbSqlLookup.TabIndex = 2;
            this.lbSqlLookup.Text = "N/A";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(7, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "SqlGeometry";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(7, 0x3d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WKB";
            this.button2.Location = new Point(5, 0x48);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 12;
            this.button2.Text = "WKB";
            this.button2.UseVisualStyleBackColor = true;
            this.button1.Location = new Point(5, 0x2b);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 11;
            this.button1.Text = "CLR";
            this.button1.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1c8, 150);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.cmbTables);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Name = "frmQuerySpeedTest";
            this.Text = "frmQuerySpeedTest";
            base.Load += new EventHandler(this.frmQuerySpeedTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PopulateDropdown()
        {
            try
            {
                string selectCommandText = "select t.name, c.name, types.name from sys.tables t join sys.columns c on (t.object_id = c.object_id) join sys.types types on (c.user_type_id = types.user_type_id) where types.name = 'geometry' or types.name = 'geography'";
                using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
                {
                    connection.Open();
                    this.GetGeometryColumn(connection, (string) this.cmbTables.SelectedValue);
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    this.cmbTables.DataSource = dataTable;
                    this.cmbTables.DisplayMember = "name";
                }
            }
            catch
            {
            }
        }

        private double[] Time(string expression, ParseRow method)
        {
            double[] numArray = new double[3];
            int num = 0;
            using (SqlConnection connection = new SqlConnection(this.GetConnectionString()))
            {
                connection.Open();
                string geometryColumn = this.GetGeometryColumn(connection, this.cmbTables.Text);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT [{1}]{2} FROM [{0}]", this.cmbTables.Text, geometryColumn, expression);
                this.perf.Start();
                SqlDataReader reader = command.ExecuteReader();
                this.perf.Stop();
                numArray[0] = this.perf.Duration(1);
                this.perf.Start();
                while (reader.Read())
                {
                    method(reader);
                    num++;
                }
                this.perf.Stop();
                connection.Close();
                numArray[1] = this.perf.Duration(1);
                numArray[2] = num;
            }
            return numArray;
        }

        private delegate Geometry ParseRow(SqlDataReader reader);
    }
}

