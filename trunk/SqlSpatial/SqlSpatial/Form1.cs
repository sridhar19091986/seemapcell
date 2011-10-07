namespace SqlSpatial
{
    using Microsoft.SqlServer.Types;
    using SharpGIS.Gis;
    using SharpGIS.Gis.Converters;
    using SharpGIS.Gis.Data.ShapeClient;
    using SharpGIS.Gis.Geometries;
    using SqlSpatial.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;
    using Win32;

    public class Form1 : Form
    {
        private List<Feature> _currentFeatureSet;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem backgroundMapToolStripMenuItem;
        private ToolStripMenuItem blogsToolStripMenuItem;
        private ToolStripButton btnComment;
        private ToolStripButton btnDecreaseIndent;
        private ToolStripButton btnExecute;
        private ToolStripButton btnIncreaseIndent;
        private ToolStripButton btnNewQuery;
        private ToolStripButton btnOpenQuery;
        private ToolStripButton btnSaveQuery;
        private ToolStripButton btnSelectDatabase;
        private ToolStripButton btnUncomment;
        private ToolStripMenuItem closeToolStripMenuItem;
        private ToolStripMenuItem commentOutTheSelectedLinesToolStripMenuItem;
        private IContainer components;
        private SqlServerConfigurationForm.SqlServerConfiguration connectionSettings;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem craigDunnToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private DataGridView dataGridView1;
        private ToolStripMenuItem decreaseIndentToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ElementHost elementHost1;
        private ToolStripMenuItem executeToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem exportResultToShapefileToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem fontsizeToolStripMenuItem;
        private ToolStripMenuItem geographyDataTypeMethodReferenceToolStripMenuItem;
        private ToolStripMenuItem geometryDataTypeMethodReferenceToolStripMenuItem;
        private GeometryVisualizer geometryVisualizer1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem increaseIndentToolStripMenuItem;
        private ToolStripMenuItem isaacKunenToolStripMenuItem;
        private string m_customImageFilename;
        private bool m_useCustomImage;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem mSDNDocumentationToolStripMenuItem1;
        private ToolStripMenuItem mSDNForumsToolStripMenuItem;
        private ToolStripMenuItem newQueryToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private OpenFileDialog openImageFileDialog;
        private ToolStripMenuItem openQueryToolStripMenuItem;
        private ToolStripMenuItem overviewToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private PictureBox picCloseQueryButton;
        private ToolStripMenuItem queryToolStripMenuItem;
        private ToolStripMenuItem queryToolStripMenuItem1;
        private List<string> recentFiles = new List<string>();
        private ToolStripMenuItem recentFilesToolStripMenuItem;
        private SaveFileDialog saveFileDialog1;
        private SaveFileDialog saveFileDialogShp;
        private ToolStripMenuItem saveQueryToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem selectDatabaseToolStripMenuItem;
        private ToolStripMenuItem sharpGISToolStripMenuItem;
        private ToolStripMenuItem showBackgroundMapgeographicOnlyToolStripMenuItem;
        private ToolStripMenuItem simonsBlogToolStripMenuItem;
        private SplitContainer splitContainer1;
        private StatusStrip statusStrip1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabControl tabQueries;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem uncommentTheSelectedLinesToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem useCustomToolStripMenuItem;
        private ToolStripMenuItem useDefaultMapToolStripMenuItem;

        public Form1()
        {
            this.InitializeComponent();
            WinRegistry.GetValue("Server");
            WinRegistry.GetValue("Database");
            this.m_customImageFilename = WinRegistry.GetValue("BackgroundImage") as string;
            this.m_useCustomImage = (WinRegistry.GetValue("UseCustomBackgroundImage") as string) == "True";
            if (string.IsNullOrEmpty(this.m_customImageFilename) || !File.Exists(this.m_customImageFilename))
            {
                this.m_useCustomImage = false;
            }
            this.useCustomToolStripMenuItem.Checked = this.m_useCustomImage;
            this.useDefaultMapToolStripMenuItem.Checked = !this.m_useCustomImage;
            object obj2 = WinRegistry.GetValue("WindowWidth");
            object obj3 = WinRegistry.GetValue("WindowHeight");
            object obj4 = WinRegistry.GetValue("SplitterPosition");
            this.connectionSettings = WinRegistry.GetValue<SqlServerConfigurationForm.SqlServerConfiguration>("Connection");
            if (((obj2 is int) && (obj3 is int)) && (obj4 is int))
            {
                int width = (int) obj2;
                int height = (int) obj3;
                int num3 = (int) obj4;
                if (((width >= this.MinimumSize.Width) && (height >= this.MinimumSize.Height)) && ((num3 > 20) && (num3 < height)))
                {
                    base.Size = new Size(width, height);
                    this.splitContainer1.SplitterDistance = num3;
                }
            }
            if ((WinRegistry.GetValue("Maximized") as string) == "True")
            {
                base.WindowState = FormWindowState.Maximized;
            }
            this.tabQueries.TabPages.Clear();
            this.createNewTab("Untitled", "", "");
            string str = WinRegistry.GetValue("RecentFiles") as string;
            if (!string.IsNullOrEmpty(str))
            {
                this.recentFiles.AddRange(str.Split(new char[] { '|' }));
            }
            this.populateRecentFileList();
            if (this.connectionSettings == null)
            {
                this.btnConfigureDatabase_Click(null, null);
            }
            else
            {
                this.setDatabaseText();
            }
        }

        private void addToRecent(string filename)
        {
            if (this.recentFiles.Contains(filename))
            {
                this.recentFiles.Remove(filename);
            }
            this.recentFiles.Insert(0, filename);
            if (this.recentFiles.Count > 10)
            {
                this.recentFiles.RemoveRange(10, this.recentFiles.Count - 10);
            }
            this.populateRecentFileList();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().IndentIncrease("--", false);
        }

        private void btnConfigureDatabase_Click(object sender, EventArgs e)
        {
            SqlServerConfigurationForm form = new SqlServerConfigurationForm(this.connectionSettings);
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.connectionSettings = form.Configuration;
                this.setDatabaseText();
            }
        }

        private void btnDecreaseIndent_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().IndentDecrease("\t");
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            // This item is obfuscated and can not be translated.
            string str = this.GetActiveTextbox().Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                MessageBox.Show("No expression to execute");
            }
            else
            {
                this.toolStripStatusLabel1.Text = "Running query...";
                ((GeometryVisualizer) this.elementHost1.Child).Clear();
                base.Update();
                DataTable dataTable = new DataTable();
                HiPerfTimer timer = new HiPerfTimer();
                SqlConnection connection = new SqlConnection(this.GetConnectionString());
                try
                {
                    connection.Open();
                    SqlCommand selectCommand = new SqlCommand(str, connection);
                    selectCommand.CommandTimeout = 180;
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    timer.Start();
                    adapter.Fill(dataTable);
                    timer.Stop();
                    adapter.Dispose();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    ((GeometryVisualizer) this.elementHost1.Child).ShowBlankMap(this.useCustomToolStripMenuItem.Checked ? this.m_customImageFilename : null);
                    MessageBox.Show(exception.Message);
                    this.toolStripStatusLabel1.Text = "Ready";
                    return;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
                int num = -1;
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    Type dataType = dataTable.Columns[i].DataType;
                    if ((dataType == typeof(SqlGeometry)) || (dataType == typeof(SqlGeography)))
                    {
                        num = i;
                        break;
                    }
                }
                int num3 = 0;
                bool isGeographic = true;
                if (num < 0)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        this.tabControl1.SelectTab(1);
                    }
                }
                else
                {
                    List<Feature> features = new List<Feature>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Geometry geometry = null;
                        object obj2 = row[num];
                        if (obj2 is SqlGeometry)
                        {
                            SqlGeometry geo = obj2 as SqlGeometry;
                            geometry = geo.ToGeometry();
                            bool x = geo.get_STSrid() < 0xfa0;
                            if (SqlBoolean.op_True(x))
                            {
                                goto Label_0214;
                            }
                            if (SqlBoolean.op_True(x | (geo.get_STSrid() > 0x1387)))
                            {
                                isGeographic = false;
                            }
                        }
                        else if (obj2 is SqlGeography)
                        {
                            geometry = (obj2 as SqlGeography).ToGeometry();
                        }
                        if (geometry != null)
                        {
                            Dictionary<string, object> attributes = new Dictionary<string, object>();
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                if (j != num)
                                {
                                    attributes.Add(dataTable.Columns[j].ColumnName, row[j]);
                                }
                            }
                            features.Add(new Feature(geometry, attributes));
                            num3++;
                        }
                    }
                    ((GeometryVisualizer) this.elementHost1.Child).ShowMap(features, isGeographic, this.showBackgroundMapgeographicOnlyToolStripMenuItem.Checked, this.useCustomToolStripMenuItem.Checked ? this.m_customImageFilename : null);
                    this._currentFeatureSet = features;
                }
                this.dataGridView1.DataSource = dataTable;
                this.toolStripStatusLabel1.Text = string.Format("{0} {2} returned ({1} {3}) in {4} seconds", new object[] { dataTable.Rows.Count, num3, (dataTable.Rows.Count != 1) ? "rows" : "row", (num3 != 1) ? "geometries" : "geometry", timer.Duration.ToString("0.######") });
                this.exportResultToShapefileToolStripMenuItem.Enabled = num3 > 0;
            }
        }

        private void btnIncreaseIndent_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().IndentIncrease("\t", true);
        }

        private void btnNewQuery_Click(object sender, EventArgs e)
        {
            this.createNewTab("Untitled", "", "");
        }

        private void btnOpenQuery_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.loadFile(this.openFileDialog1.FileName);
            }
        }

        private void btnSaveQuery_Click(object sender, EventArgs e)
        {
            this.SaveActiveTab();
        }

        private void btnSaveToShape_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialogShp.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<ShapeType> list = new List<ShapeType>();
                    foreach (Feature feature in this._currentFeatureSet)
                    {
                        ShapeType item = feature.Geometry.ToShapeType();
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                        }
                    }
                    List<DataColumn> list2 = new List<DataColumn>();
                    foreach (string str in this._currentFeatureSet[0].Attributes.Keys)
                    {
                        list2.Add(new DataColumn(str, this._currentFeatureSet[0].Attributes[str].GetType()));
                    }
                    foreach (ShapeType type2 in list)
                    {
                        string str2 = "";
                        if (list.Count > 1)
                        {
                            switch (type2)
                            {
                                case ShapeType.Point:
                                    str2 = "_point";
                                    break;

                                case ShapeType.MultiPoint:
                                    str2 = "_points";
                                    break;

                                case ShapeType.PolyLine:
                                    str2 = "_lines";
                                    break;

                                case ShapeType.Polygon:
                                    str2 = "_regions";
                                    break;
                            }
                        }
                        ShapeWriter writer = new ShapeWriter(Path.Combine(Path.GetDirectoryName(this.saveFileDialogShp.FileName), Path.GetFileNameWithoutExtension(this.saveFileDialogShp.FileName) + str2 + ".shp"), type2, list2.ToArray());
                        writer.Open();
                        foreach (Feature feature2 in this._currentFeatureSet)
                        {
                            if (feature2.Geometry.ToShapeType() == type2)
                            {
                                writer.WriteFeature(feature2);
                            }
                        }
                        writer.Close();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Failed to export result:\r\n" + exception.Message);
                }
            }
        }

        private void btnUncomment_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().IndentDecrease("--");
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabQueries.SelectedTab.Text.EndsWith("*") && (this.GetActiveTextbox().Text.Trim() != ""))
            {
                switch (MessageBox.Show("Save changes to query?", "Save", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        if (this.SaveActiveTab() != DialogResult.Cancel)
                        {
                            goto Label_006C;
                        }
                        break;

                    case DialogResult.No:
                        goto Label_006C;

                    case DialogResult.Cancel:
                        break;

                    default:
                        return;
                }
                return;
            }
        Label_006C:
            this.tabQueries.TabPages.Remove(this.tabQueries.SelectedTab);
            if (this.tabQueries.TabPages.Count == 0)
            {
                this.createNewTab("Untitled", "", "");
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().Copy();
        }

        private void craigDunnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://conceptdev.blogspot.com/");
        }

        private void createNewTab(string title, string contents, string filename)
        {
            Textbox textbox = new Textbox();
            textbox.Dock = DockStyle.Fill;
            TabPage page = new TabPage(title);
            page.Controls.Add(textbox);
            textbox.Text = contents;
            page.ToolTipText = filename;
            textbox.TextChanged += new EventHandler(this.tbSqlQuery_TextChanged);
            this.tabQueries.TabPages.Add(page);
            this.tabQueries.SelectedIndex = this.tabQueries.TabPages.Count - 1;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().Cut();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().Delete();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = this.tabQueries.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage page = this.tabQueries.TabPages[i];
                this.tabQueries.SelectedTab = page;
                if (page.Text.EndsWith("*"))
                {
                    switch (MessageBox.Show("Save changes to query?", "Save", MessageBoxButtons.YesNoCancel))
                    {
                        case DialogResult.Yes:
                            if (this.SaveActiveTab() != DialogResult.Cancel)
                            {
                                goto Label_00A2;
                            }
                            e.Cancel = true;
                            return;

                        case DialogResult.No:
                        {
                            this.tabQueries.TabPages.Remove(page);
                            continue;
                        }
                        case DialogResult.Cancel:
                            goto Label_0074;
                    }
                }
                continue;
            Label_0074:
                e.Cancel = true;
                return;
            Label_00A2:
                this.tabQueries.TabPages.Remove(page);
            }
            string str = "";
            if (this.recentFiles.Count > 0)
            {
                str = this.recentFiles[0];
                for (int j = 1; j < this.recentFiles.Count; j++)
                {
                    str = str + "|" + this.recentFiles[j];
                }
            }
            WinRegistry.SetValue("RecentFiles", str);
            WinRegistry.SetValue("Maximized", base.WindowState == FormWindowState.Maximized);
            if (base.WindowState != FormWindowState.Normal)
            {
                base.WindowState = FormWindowState.Normal;
            }
            WinRegistry.SetValue("WindowWidth", base.Size.Width);
            WinRegistry.SetValue("WindowHeight", base.Size.Height);
            WinRegistry.SetValue("SplitterPosition", this.splitContainer1.SplitterDistance);
            if (this.connectionSettings != null)
            {
                if (!this.connectionSettings.SavePassword)
                {
                    this.connectionSettings.Username = null;
                    this.connectionSettings.Password = null;
                }
                WinRegistry.SetValue("Connection", this.connectionSettings);
            }
        }

        private void geographyDataTypeMethodReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://msdn2.microsoft.com/en-us/library/cc280766(SQL.100).aspx");
        }

        private void geometryDataTypeMethodReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://msdn2.microsoft.com/en-us/library/bb933973(SQL.100).aspx");
        }

        private Textbox GetActiveTextbox()
        {
            return (this.tabQueries.SelectedTab.Controls[0] as Textbox);
        }

        private string GetConnectionString()
        {
            if (this.connectionSettings != null)
            {
                return this.connectionSettings.ToString();
            }
            return null;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new SplitContainer();
            this.tabQueries = new TabControl();
            this.tabPage3 = new TabPage();
            this.tabPage4 = new TabPage();
            this.toolStrip1 = new ToolStrip();
            this.btnNewQuery = new ToolStripButton();
            this.btnOpenQuery = new ToolStripButton();
            this.btnSaveQuery = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.btnDecreaseIndent = new ToolStripButton();
            this.btnIncreaseIndent = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnComment = new ToolStripButton();
            this.btnUncomment = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.btnExecute = new ToolStripButton();
            this.toolStripSeparator11 = new ToolStripSeparator();
            this.toolStripLabel1 = new ToolStripLabel();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.dataGridView1 = new DataGridView();
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.openFileDialog1 = new OpenFileDialog();
            this.saveFileDialog1 = new SaveFileDialog();
            this.menuStrip2 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.newQueryToolStripMenuItem = new ToolStripMenuItem();
            this.openQueryToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.saveQueryToolStripMenuItem = new ToolStripMenuItem();
            this.closeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.recentFilesToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator12 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.undoToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator9 = new ToolStripSeparator();
            this.cutToolStripMenuItem = new ToolStripMenuItem();
            this.copyToolStripMenuItem = new ToolStripMenuItem();
            this.pasteToolStripMenuItem = new ToolStripMenuItem();
            this.deleteToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator8 = new ToolStripSeparator();
            this.commentOutTheSelectedLinesToolStripMenuItem = new ToolStripMenuItem();
            this.uncommentTheSelectedLinesToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.increaseIndentToolStripMenuItem = new ToolStripMenuItem();
            this.decreaseIndentToolStripMenuItem = new ToolStripMenuItem();
            this.queryToolStripMenuItem1 = new ToolStripMenuItem();
            this.executeToolStripMenuItem = new ToolStripMenuItem();
            this.queryToolStripMenuItem = new ToolStripMenuItem();
            this.backgroundMapToolStripMenuItem = new ToolStripMenuItem();
            this.showBackgroundMapgeographicOnlyToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator6 = new ToolStripSeparator();
            this.useDefaultMapToolStripMenuItem = new ToolStripMenuItem();
            this.useCustomToolStripMenuItem = new ToolStripMenuItem();
            this.fontsizeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.toolStripMenuItem3 = new ToolStripMenuItem();
            this.toolStripMenuItem4 = new ToolStripMenuItem();
            this.toolStripMenuItem5 = new ToolStripMenuItem();
            this.toolStripMenuItem6 = new ToolStripMenuItem();
            this.toolStripMenuItem7 = new ToolStripMenuItem();
            this.toolStripMenuItem8 = new ToolStripMenuItem();
            this.toolsToolStripMenuItem = new ToolStripMenuItem();
            this.exportResultToShapefileToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.mSDNForumsToolStripMenuItem = new ToolStripMenuItem();
            this.mSDNDocumentationToolStripMenuItem1 = new ToolStripMenuItem();
            this.overviewToolStripMenuItem = new ToolStripMenuItem();
            this.geometryDataTypeMethodReferenceToolStripMenuItem = new ToolStripMenuItem();
            this.geographyDataTypeMethodReferenceToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator7 = new ToolStripSeparator();
            this.blogsToolStripMenuItem = new ToolStripMenuItem();
            this.isaacKunenToolStripMenuItem = new ToolStripMenuItem();
            this.sharpGISToolStripMenuItem = new ToolStripMenuItem();
            this.craigDunnToolStripMenuItem = new ToolStripMenuItem();
            this.simonsBlogToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator10 = new ToolStripSeparator();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.saveFileDialogShp = new SaveFileDialog();
            this.openImageFileDialog = new OpenFileDialog();
            this.picCloseQueryButton = new PictureBox();
            this.btnSelectDatabase = new ToolStripButton();
            this.selectDatabaseToolStripMenuItem = new ToolStripMenuItem();
            this.elementHost1 = new ElementHost();
            this.geometryVisualizer1 = new GeometryVisualizer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabQueries.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((ISupportInitialize) this.dataGridView1).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            ((ISupportInitialize) this.picCloseQueryButton).BeginInit();
            base.SuspendLayout();
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.Location = new Point(0, 0x1b);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.tabQueries);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Padding = new Padding(3, 0, 3, 0);
            this.splitContainer1.Size = new Size(0x23c, 0x1d4);
            this.splitContainer1.SplitterDistance = 0xac;
            this.splitContainer1.TabIndex = 8;
            this.splitContainer1.TabStop = false;
            this.tabQueries.Controls.Add(this.tabPage3);
            this.tabQueries.Controls.Add(this.tabPage4);
            this.tabQueries.Dock = DockStyle.Fill;
            this.tabQueries.Location = new Point(0, 0x19);
            this.tabQueries.Name = "tabQueries";
            this.tabQueries.SelectedIndex = 0;
            this.tabQueries.Size = new Size(0x23c, 0x93);
            this.tabQueries.TabIndex = 4;
            this.tabQueries.SelectedIndexChanged += new EventHandler(this.tabQueries_SelectedIndexChanged);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new Padding(3);
            this.tabPage3.Size = new Size(0x234, 0x79);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new Padding(3);
            this.tabPage4.Size = new Size(0x234, 0x79);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.btnNewQuery, this.btnOpenQuery, this.btnSaveQuery, this.toolStripSeparator1, this.btnDecreaseIndent, this.btnIncreaseIndent, this.toolStripSeparator2, this.btnComment, this.btnUncomment, this.toolStripSeparator3, this.btnExecute, this.toolStripSeparator11, this.toolStripLabel1, this.btnSelectDatabase });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(0x23c, 0x19);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.btnNewQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnNewQuery.Image = (Image) manager.GetObject("btnNewQuery.Image");
            this.btnNewQuery.ImageScaling = ToolStripItemImageScaling.None;
            this.btnNewQuery.ImageTransparentColor = Color.Magenta;
            this.btnNewQuery.Name = "btnNewQuery";
            this.btnNewQuery.Size = new Size(0x17, 0x16);
            this.btnNewQuery.Text = "New Query";
            this.btnNewQuery.ToolTipText = "New Query (Ctrl+N)";
            this.btnNewQuery.Click += new EventHandler(this.btnNewQuery_Click);
            this.btnOpenQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnOpenQuery.Image = (Image) manager.GetObject("btnOpenQuery.Image");
            this.btnOpenQuery.ImageScaling = ToolStripItemImageScaling.None;
            this.btnOpenQuery.ImageTransparentColor = Color.Magenta;
            this.btnOpenQuery.Name = "btnOpenQuery";
            this.btnOpenQuery.Size = new Size(0x17, 0x16);
            this.btnOpenQuery.Text = "Open query";
            this.btnOpenQuery.ToolTipText = "Open query (Ctrl+O)";
            this.btnOpenQuery.Click += new EventHandler(this.btnOpenQuery_Click);
            this.btnSaveQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnSaveQuery.Image = (Image) manager.GetObject("btnSaveQuery.Image");
            this.btnSaveQuery.ImageScaling = ToolStripItemImageScaling.None;
            this.btnSaveQuery.ImageTransparentColor = Color.Magenta;
            this.btnSaveQuery.Name = "btnSaveQuery";
            this.btnSaveQuery.Size = new Size(0x17, 0x16);
            this.btnSaveQuery.Text = "&Save query";
            this.btnSaveQuery.ToolTipText = "Save query (Ctrl+S)";
            this.btnSaveQuery.Click += new EventHandler(this.saveToolStripMenuItem_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 0x19);
            this.btnDecreaseIndent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnDecreaseIndent.Image = Resources.DecreaseIndent;
            this.btnDecreaseIndent.ImageTransparentColor = Color.Magenta;
            this.btnDecreaseIndent.Name = "btnDecreaseIndent";
            this.btnDecreaseIndent.Size = new Size(0x17, 0x16);
            this.btnDecreaseIndent.Text = "toolStripButton1";
            this.btnDecreaseIndent.ToolTipText = "Decrease Indent (Ctrl+Shift+I)";
            this.btnDecreaseIndent.Click += new EventHandler(this.btnDecreaseIndent_Click);
            this.btnIncreaseIndent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnIncreaseIndent.Image = Resources.IncreaseIndent;
            this.btnIncreaseIndent.ImageTransparentColor = Color.Magenta;
            this.btnIncreaseIndent.Name = "btnIncreaseIndent";
            this.btnIncreaseIndent.Size = new Size(0x17, 0x16);
            this.btnIncreaseIndent.Text = "toolStripButton1";
            this.btnIncreaseIndent.ToolTipText = "Increase Indent (Ctrl+I)";
            this.btnIncreaseIndent.Click += new EventHandler(this.btnIncreaseIndent_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnComment.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnComment.Image = Resources.Comment;
            this.btnComment.ImageTransparentColor = Color.Magenta;
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new Size(0x17, 0x16);
            this.btnComment.Text = "Comment";
            this.btnComment.ToolTipText = "Comment out the selected lines (Ctrl+K)";
            this.btnComment.Click += new EventHandler(this.btnComment_Click);
            this.btnUncomment.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnUncomment.Image = Resources.Uncomment;
            this.btnUncomment.ImageTransparentColor = Color.Magenta;
            this.btnUncomment.Name = "btnUncomment";
            this.btnUncomment.Size = new Size(0x17, 0x16);
            this.btnUncomment.Text = "toolStripButton1";
            this.btnUncomment.ToolTipText = "Uncomment the selected lines (Ctrl+U)";
            this.btnUncomment.Click += new EventHandler(this.btnUncomment_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 0x19);
            this.btnExecute.Image = (Image) manager.GetObject("btnExecute.Image");
            this.btnExecute.ImageScaling = ToolStripItemImageScaling.None;
            this.btnExecute.ImageTransparentColor = Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(0x42, 0x16);
            this.btnExecute.Text = "Execute";
            this.btnExecute.ToolTipText = "Execute (F5)";
            this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new Size(6, 0x19);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new Size(0x3a, 0x16);
            this.toolStripLabel1.Text = "Database:";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x236, 0x124);
            this.tabControl1.TabIndex = 5;
            this.tabPage1.Controls.Add(this.elementHost1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0x22e, 0x10a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Map";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0x22e, 0x10a);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(0x228, 260);
            this.dataGridView1.TabIndex = 9;
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabel1 });
            this.statusStrip1.Location = new Point(0, 0x1f2);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x23c, 0x16);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(0x27, 0x11);
            this.toolStripStatusLabel1.Text = "Ready";
            this.openFileDialog1.Filter = "SQL query |*.sql|All files|*.*";
            this.saveFileDialog1.Filter = "SQL query |*.sql|All files|*.*";
            this.menuStrip2.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.editToolStripMenuItem, this.queryToolStripMenuItem1, this.queryToolStripMenuItem, this.toolsToolStripMenuItem, this.helpToolStripMenuItem });
            this.menuStrip2.Location = new Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new Size(0x23c, 0x18);
            this.menuStrip2.TabIndex = 14;
            this.menuStrip2.Text = "menuStrip2";
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.newQueryToolStripMenuItem, this.openQueryToolStripMenuItem, this.saveToolStripMenuItem, this.saveQueryToolStripMenuItem, this.closeToolStripMenuItem, this.selectDatabaseToolStripMenuItem, this.toolStripSeparator5, this.recentFilesToolStripMenuItem, this.toolStripSeparator12, this.exitToolStripMenuItem });
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(0x25, 20);
            this.fileToolStripMenuItem.Text = "&File";
            this.newQueryToolStripMenuItem.Image = Resources._new;
            this.newQueryToolStripMenuItem.Name = "newQueryToolStripMenuItem";
            this.newQueryToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.newQueryToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            this.newQueryToolStripMenuItem.Size = new Size(230, 0x16);
            this.newQueryToolStripMenuItem.Text = "&New Query";
            this.newQueryToolStripMenuItem.Click += new EventHandler(this.btnNewQuery_Click);
            this.openQueryToolStripMenuItem.Image = Resources.open;
            this.openQueryToolStripMenuItem.Name = "openQueryToolStripMenuItem";
            this.openQueryToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.openQueryToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            this.openQueryToolStripMenuItem.Size = new Size(230, 0x16);
            this.openQueryToolStripMenuItem.Text = "&Open Query...";
            this.openQueryToolStripMenuItem.Click += new EventHandler(this.btnOpenQuery_Click);
            this.saveToolStripMenuItem.Image = Resources.save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            this.saveToolStripMenuItem.Size = new Size(230, 0x16);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
            this.saveQueryToolStripMenuItem.Name = "saveQueryToolStripMenuItem";
            this.saveQueryToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.saveQueryToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            this.saveQueryToolStripMenuItem.Size = new Size(230, 0x16);
            this.saveQueryToolStripMenuItem.Text = "Save Query &As...";
            this.saveQueryToolStripMenuItem.Click += new EventHandler(this.btnSaveQuery_Click);
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F4;
            this.closeToolStripMenuItem.Size = new Size(230, 0x16);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new EventHandler(this.closeToolStripMenuItem_Click);
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new Size(0xe3, 6);
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new Size(230, 0x16);
            this.recentFilesToolStripMenuItem.Text = "Recent &Files";
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new Size(0xe3, 6);
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            this.exitToolStripMenuItem.Size = new Size(230, 0x16);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
            this.editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.undoToolStripMenuItem, this.toolStripSeparator9, this.cutToolStripMenuItem, this.copyToolStripMenuItem, this.pasteToolStripMenuItem, this.deleteToolStripMenuItem, this.toolStripSeparator8, this.commentOutTheSelectedLinesToolStripMenuItem, this.uncommentTheSelectedLinesToolStripMenuItem, this.toolStripSeparator4, this.increaseIndentToolStripMenuItem, this.decreaseIndentToolStripMenuItem });
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(0x27, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.undoToolStripMenuItem.Image = Resources.Undo;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl-Z";
            this.undoToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new EventHandler(this.undoToolStripMenuItem_Click);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new Size(280, 6);
            this.cutToolStripMenuItem.Image = Resources.Cut;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new EventHandler(this.cutToolStripMenuItem_Click);
            this.copyToolStripMenuItem.Image = Resources.Copy;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new EventHandler(this.copyToolStripMenuItem_Click);
            this.pasteToolStripMenuItem.Image = Resources.Paste;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            this.pasteToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new EventHandler(this.pasteToolStripMenuItem_Click);
            this.deleteToolStripMenuItem.Image = Resources.Delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = "Del";
            this.deleteToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new Size(280, 6);
            this.commentOutTheSelectedLinesToolStripMenuItem.Image = Resources.Comment;
            this.commentOutTheSelectedLinesToolStripMenuItem.Name = "commentOutTheSelectedLinesToolStripMenuItem";
            this.commentOutTheSelectedLinesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.K;
            this.commentOutTheSelectedLinesToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.commentOutTheSelectedLinesToolStripMenuItem.Text = "&Comment out the selected lines";
            this.commentOutTheSelectedLinesToolStripMenuItem.Click += new EventHandler(this.btnComment_Click);
            this.uncommentTheSelectedLinesToolStripMenuItem.Image = Resources.Uncomment;
            this.uncommentTheSelectedLinesToolStripMenuItem.Name = "uncommentTheSelectedLinesToolStripMenuItem";
            this.uncommentTheSelectedLinesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.U;
            this.uncommentTheSelectedLinesToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.uncommentTheSelectedLinesToolStripMenuItem.Text = "&Uncomment the selected lines";
            this.uncommentTheSelectedLinesToolStripMenuItem.Click += new EventHandler(this.btnUncomment_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(280, 6);
            this.increaseIndentToolStripMenuItem.Image = Resources.IncreaseIndent;
            this.increaseIndentToolStripMenuItem.Name = "increaseIndentToolStripMenuItem";
            this.increaseIndentToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            this.increaseIndentToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.increaseIndentToolStripMenuItem.Text = "&Increase Indent";
            this.increaseIndentToolStripMenuItem.Click += new EventHandler(this.btnIncreaseIndent_Click);
            this.decreaseIndentToolStripMenuItem.Image = Resources.DecreaseIndent;
            this.decreaseIndentToolStripMenuItem.Name = "decreaseIndentToolStripMenuItem";
            this.decreaseIndentToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.I;
            this.decreaseIndentToolStripMenuItem.Size = new Size(0x11b, 0x16);
            this.decreaseIndentToolStripMenuItem.Text = "&Decrease Indent";
            this.decreaseIndentToolStripMenuItem.Click += new EventHandler(this.btnDecreaseIndent_Click);
            this.queryToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { this.executeToolStripMenuItem });
            this.queryToolStripMenuItem1.Name = "queryToolStripMenuItem1";
            this.queryToolStripMenuItem1.Size = new Size(0x33, 20);
            this.queryToolStripMenuItem1.Text = "&Query";
            this.executeToolStripMenuItem.Image = Resources.Execute;
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.executeToolStripMenuItem.ShortcutKeys = Keys.F5;
            this.executeToolStripMenuItem.Size = new Size(0x85, 0x16);
            this.executeToolStripMenuItem.Text = "&Execute";
            this.executeToolStripMenuItem.Click += new EventHandler(this.btnExecute_Click);
            this.queryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.backgroundMapToolStripMenuItem, this.fontsizeToolStripMenuItem });
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new Size(0x2c, 20);
            this.queryToolStripMenuItem.Text = "&View";
            this.backgroundMapToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.showBackgroundMapgeographicOnlyToolStripMenuItem, this.toolStripSeparator6, this.useDefaultMapToolStripMenuItem, this.useCustomToolStripMenuItem });
            this.backgroundMapToolStripMenuItem.Name = "backgroundMapToolStripMenuItem";
            this.backgroundMapToolStripMenuItem.Size = new Size(0xae, 0x16);
            this.backgroundMapToolStripMenuItem.Text = "&Background map...";
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.Checked = true;
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.CheckOnClick = true;
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.CheckState = CheckState.Checked;
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.Name = "showBackgroundMapgeographicOnlyToolStripMenuItem";
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.Size = new Size(0x126, 0x16);
            this.showBackgroundMapgeographicOnlyToolStripMenuItem.Text = "Show &background map (geographic only)";
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new Size(0x123, 6);
            this.useDefaultMapToolStripMenuItem.Checked = true;
            this.useDefaultMapToolStripMenuItem.CheckState = CheckState.Checked;
            this.useDefaultMapToolStripMenuItem.Name = "useDefaultMapToolStripMenuItem";
            this.useDefaultMapToolStripMenuItem.Size = new Size(0x126, 0x16);
            this.useDefaultMapToolStripMenuItem.Text = "&Use default map";
            this.useDefaultMapToolStripMenuItem.Click += new EventHandler(this.useDefaultMapToolStripMenuItem_Click);
            this.useCustomToolStripMenuItem.CheckOnClick = true;
            this.useCustomToolStripMenuItem.Name = "useCustomToolStripMenuItem";
            this.useCustomToolStripMenuItem.Size = new Size(0x126, 0x16);
            this.useCustomToolStripMenuItem.Text = "Use &custom image...";
            this.useCustomToolStripMenuItem.Click += new EventHandler(this.useCustomToolStripMenuItem_Click);
            this.fontsizeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.toolStripMenuItem2, this.toolStripMenuItem3, this.toolStripMenuItem4, this.toolStripMenuItem5, this.toolStripMenuItem6, this.toolStripMenuItem7, this.toolStripMenuItem8 });
            this.fontsizeToolStripMenuItem.Name = "fontsizeToolStripMenuItem";
            this.fontsizeToolStripMenuItem.Size = new Size(0xae, 0x16);
            this.fontsizeToolStripMenuItem.Text = "&Font Size";
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem2.Text = "6";
            this.toolStripMenuItem2.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem3.Checked = true;
            this.toolStripMenuItem3.CheckState = CheckState.Checked;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem3.Text = "8.5";
            this.toolStripMenuItem3.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem4.Text = "10";
            this.toolStripMenuItem4.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem5.Text = "12";
            this.toolStripMenuItem5.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem6.Text = "14";
            this.toolStripMenuItem6.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem7.Text = "16";
            this.toolStripMenuItem7.Click += new EventHandler(this.onFontsizeSelect);
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(0x59, 0x16);
            this.toolStripMenuItem8.Text = "18";
            this.toolStripMenuItem8.Click += new EventHandler(this.onFontsizeSelect);
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.exportResultToShapefileToolStripMenuItem });
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new Size(0x30, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            this.exportResultToShapefileToolStripMenuItem.Enabled = false;
            this.exportResultToShapefileToolStripMenuItem.Name = "exportResultToShapefileToolStripMenuItem";
            this.exportResultToShapefileToolStripMenuItem.Size = new Size(0xd4, 0x16);
            this.exportResultToShapefileToolStripMenuItem.Text = "&Export result to shapefile...";
            this.exportResultToShapefileToolStripMenuItem.Click += new EventHandler(this.btnSaveToShape_Click);
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.mSDNForumsToolStripMenuItem, this.mSDNDocumentationToolStripMenuItem1, this.toolStripSeparator7, this.blogsToolStripMenuItem, this.toolStripSeparator10, this.aboutToolStripMenuItem });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(0x2c, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.mSDNForumsToolStripMenuItem.Image = Resources.PinNote;
            this.mSDNForumsToolStripMenuItem.Name = "mSDNForumsToolStripMenuItem";
            this.mSDNForumsToolStripMenuItem.Size = new Size(0xc2, 0x16);
            this.mSDNForumsToolStripMenuItem.Text = "MSDN &Forums";
            this.mSDNForumsToolStripMenuItem.Click += new EventHandler(this.mSDNForumsToolStripMenuItem_Click);
            this.mSDNDocumentationToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { this.overviewToolStripMenuItem, this.geometryDataTypeMethodReferenceToolStripMenuItem, this.geographyDataTypeMethodReferenceToolStripMenuItem });
            this.mSDNDocumentationToolStripMenuItem1.Image = Resources.WebHelp;
            this.mSDNDocumentationToolStripMenuItem1.Name = "mSDNDocumentationToolStripMenuItem1";
            this.mSDNDocumentationToolStripMenuItem1.Size = new Size(0xc2, 0x16);
            this.mSDNDocumentationToolStripMenuItem1.Text = "MSDN Documentation";
            this.overviewToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.overviewToolStripMenuItem.Name = "overviewToolStripMenuItem";
            this.overviewToolStripMenuItem.Size = new Size(0x120, 0x16);
            this.overviewToolStripMenuItem.Text = "Overview";
            this.overviewToolStripMenuItem.Click += new EventHandler(this.overviewToolStripMenuItem_Click);
            this.geometryDataTypeMethodReferenceToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.geometryDataTypeMethodReferenceToolStripMenuItem.Name = "geometryDataTypeMethodReferenceToolStripMenuItem";
            this.geometryDataTypeMethodReferenceToolStripMenuItem.Size = new Size(0x120, 0x16);
            this.geometryDataTypeMethodReferenceToolStripMenuItem.Text = "Geometry Data Type Method Reference";
            this.geometryDataTypeMethodReferenceToolStripMenuItem.Click += new EventHandler(this.geometryDataTypeMethodReferenceToolStripMenuItem_Click);
            this.geographyDataTypeMethodReferenceToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.geographyDataTypeMethodReferenceToolStripMenuItem.Name = "geographyDataTypeMethodReferenceToolStripMenuItem";
            this.geographyDataTypeMethodReferenceToolStripMenuItem.Size = new Size(0x120, 0x16);
            this.geographyDataTypeMethodReferenceToolStripMenuItem.Text = "Geography Data Type Method Reference";
            this.geographyDataTypeMethodReferenceToolStripMenuItem.Click += new EventHandler(this.geographyDataTypeMethodReferenceToolStripMenuItem_Click);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new Size(0xbf, 6);
            this.blogsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.isaacKunenToolStripMenuItem, this.sharpGISToolStripMenuItem, this.craigDunnToolStripMenuItem, this.simonsBlogToolStripMenuItem });
            this.blogsToolStripMenuItem.Name = "blogsToolStripMenuItem";
            this.blogsToolStripMenuItem.Size = new Size(0xc2, 0x16);
            this.blogsToolStripMenuItem.Text = "&Weblogs";
            this.isaacKunenToolStripMenuItem.Name = "isaacKunenToolStripMenuItem";
            this.isaacKunenToolStripMenuItem.Size = new Size(0x97, 0x16);
            this.isaacKunenToolStripMenuItem.Text = "Isaac @ MSDN";
            this.isaacKunenToolStripMenuItem.Click += new EventHandler(this.isaacKunenToolStripMenuItem_Click);
            this.sharpGISToolStripMenuItem.Name = "sharpGISToolStripMenuItem";
            this.sharpGISToolStripMenuItem.Size = new Size(0x97, 0x16);
            this.sharpGISToolStripMenuItem.Text = "SharpGIS";
            this.sharpGISToolStripMenuItem.Click += new EventHandler(this.sharpGISToolStripMenuItem_Click);
            this.craigDunnToolStripMenuItem.Name = "craigDunnToolStripMenuItem";
            this.craigDunnToolStripMenuItem.Size = new Size(0x97, 0x16);
            this.craigDunnToolStripMenuItem.Text = "Craig Dunn";
            this.craigDunnToolStripMenuItem.Click += new EventHandler(this.craigDunnToolStripMenuItem_Click);
            this.simonsBlogToolStripMenuItem.Name = "simonsBlogToolStripMenuItem";
            this.simonsBlogToolStripMenuItem.Size = new Size(0x97, 0x16);
            this.simonsBlogToolStripMenuItem.Text = "SimonS Blog";
            this.simonsBlogToolStripMenuItem.Click += new EventHandler(this.simonsBlogToolStripMenuItem_Click);
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new Size(0xbf, 6);
            this.aboutToolStripMenuItem.Image = (Image) manager.GetObject("aboutToolStripMenuItem.Image");
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = Keys.F1;
            this.aboutToolStripMenuItem.Size = new Size(0xc2, 0x16);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.btnAbout_Click);
            this.saveFileDialogShp.DefaultExt = "shp";
            this.saveFileDialogShp.Filter = "Shapefiles (*.shp)|*.shp";
            this.openImageFileDialog.Filter = "JPEG images (*.jpg)|*.jpg|PNG images (*.png)|*.png|GIF images (*.gif)|*.gif";
            this.picCloseQueryButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.picCloseQueryButton.Cursor = Cursors.Hand;
            this.picCloseQueryButton.Image = Resources.close;
            this.picCloseQueryButton.InitialImage = Resources.close;
            this.picCloseQueryButton.Location = new Point(0x228, 6);
            this.picCloseQueryButton.Name = "picCloseQueryButton";
            this.picCloseQueryButton.Size = new Size(0x10, 15);
            this.picCloseQueryButton.TabIndex = 5;
            this.picCloseQueryButton.TabStop = false;
            this.picCloseQueryButton.MouseLeave += new EventHandler(this.picCloseQueryButton_MouseLeave);
            this.picCloseQueryButton.Click += new EventHandler(this.closeToolStripMenuItem_Click);
            this.picCloseQueryButton.MouseEnter += new EventHandler(this.picCloseQueryButton_MouseEnter);
            this.btnSelectDatabase.BackColor = SystemColors.Info;
            this.btnSelectDatabase.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.btnSelectDatabase.Image = (Image) manager.GetObject("btnSelectDatabase.Image");
            this.btnSelectDatabase.ImageTransparentColor = Color.Magenta;
            this.btnSelectDatabase.Name = "btnSelectDatabase";
            this.btnSelectDatabase.Size = new Size(0x65, 0x16);
            this.btnSelectDatabase.Text = "Select database...";
            this.btnSelectDatabase.ToolTipText = "Click to change database";
            this.btnSelectDatabase.Click += new EventHandler(this.btnConfigureDatabase_Click);
            this.selectDatabaseToolStripMenuItem.Name = "selectDatabaseToolStripMenuItem";
            this.selectDatabaseToolStripMenuItem.Size = new Size(230, 0x16);
            this.selectDatabaseToolStripMenuItem.Text = "Select database...";
            this.selectDatabaseToolStripMenuItem.Click += new EventHandler(this.btnConfigureDatabase_Click);
            this.elementHost1.Dock = DockStyle.Fill;
            this.elementHost1.Location = new Point(3, 3);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new Size(0x228, 260);
            this.elementHost1.TabIndex = 7;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.geometryVisualizer1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x23c, 520);
            base.Controls.Add(this.picCloseQueryButton);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.menuStrip2);
            base.Controls.Add(this.splitContainer1);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MinimumSize = new Size(0x24c, 0x113);
            base.Name = "Form1";
            this.Text = "Sql Spatial Query Visualizer";
            base.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabQueries.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((ISupportInitialize) this.dataGridView1).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            ((ISupportInitialize) this.picCloseQueryButton).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void isaacKunenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://blogs.msdn.com/isaac/");
        }

        private void loadFile(string filename)
        {
            if (string.IsNullOrEmpty(this.GetActiveTextbox().Text) && !this.tabQueries.SelectedTab.Text.EndsWith("*"))
            {
                this.tabQueries.TabPages.Remove(this.tabQueries.SelectedTab);
            }
            this.createNewTab(Path.GetFileName(filename), File.ReadAllText(filename), filename);
            this.addToRecent(filename);
        }

        private void loadRecentFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string toolTipText = item.ToolTipText;
            if (!File.Exists(toolTipText))
            {
                MessageBox.Show("File not found");
            }
            else
            {
                this.loadFile(toolTipText);
            }
        }

        private void mSDNForumsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://forums.microsoft.com/MSDN/ShowForum.aspx?ForumID=1629&SiteID=1");
        }

        private void onFontsizeSelect(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            float emSize = float.Parse(item.Text, CultureInfo.InvariantCulture);
            Font font = new Font(FontFamily.GenericMonospace, emSize, FontStyle.Regular);
            foreach (TabPage page in this.tabQueries.TabPages)
            {
                (page.Controls[0] as Textbox).Font = font;
            }
            foreach (ToolStripMenuItem item2 in (item.OwnerItem as ToolStripMenuItem).DropDownItems)
            {
                item2.Checked = false;
            }
            item.Checked = true;
        }

        private void overviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://msdn2.microsoft.com/en-us/library/bb933790(SQL.100).aspx");
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().Paste();
        }

        private void picCloseQueryButton_MouseEnter(object sender, EventArgs e)
        {
            this.picCloseQueryButton.Image = Resources.closeHover;
        }

        private void picCloseQueryButton_MouseLeave(object sender, EventArgs e)
        {
            this.picCloseQueryButton.Image = Resources.close;
        }

        private void populateRecentFileList()
        {
            this.recentFilesToolStripMenuItem.DropDownItems.Clear();
            this.recentFilesToolStripMenuItem.Enabled = (this.recentFiles != null) && (this.recentFiles.Count > 0);
            int num = 0;
            foreach (string str in this.recentFiles)
            {
                num++;
                ToolStripMenuItem item = new ToolStripMenuItem(num.ToString() + " " + Path.GetFileName(str));
                item.ToolTipText = str;
                item.Click += new EventHandler(this.loadRecentFile_Click);
                this.recentFilesToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private DialogResult SaveActiveTab()
        {
            TabPage selectedTab = this.tabQueries.SelectedTab;
            if (!string.IsNullOrEmpty(selectedTab.ToolTipText))
            {
                this.saveFileDialog1.FileName = selectedTab.ToolTipText;
            }
            DialogResult result = this.saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                File.WriteAllText(this.saveFileDialog1.FileName, this.GetActiveTextbox().Text);
                selectedTab.Text = Path.GetFileName(this.saveFileDialog1.FileName);
                selectedTab.ToolTipText = this.saveFileDialog1.FileName;
                this.saveToolStripMenuItem.Enabled = false;
                this.addToRecent(this.saveFileDialog1.FileName);
            }
            return result;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage selectedTab = this.tabQueries.SelectedTab;
            if (string.IsNullOrEmpty(selectedTab.ToolTipText) || !File.Exists(selectedTab.ToolTipText))
            {
                this.SaveActiveTab();
            }
            else
            {
                File.WriteAllText(selectedTab.ToolTipText, this.GetActiveTextbox().Text);
                if (selectedTab.Text.EndsWith("*"))
                {
                    selectedTab.Text = selectedTab.Text.Substring(0, selectedTab.Text.Length - 1);
                }
                this.saveToolStripMenuItem.Enabled = false;
                this.addToRecent(selectedTab.ToolTipText);
            }
        }

        private void setDatabaseText()
        {
            if (this.connectionSettings == null)
            {
                this.btnSelectDatabase.Text = "Configure connection...";
            }
            else if (!string.IsNullOrEmpty(this.connectionSettings.AttachDatabaseFileName))
            {
                this.btnSelectDatabase.Text = this.connectionSettings.LogicalName + " @ " + this.connectionSettings.ServerName;
            }
            else
            {
                this.btnSelectDatabase.Text = this.connectionSettings.DatabaseName + " @ " + this.connectionSettings.ServerName;
            }
        }

        private void sharpGISToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.sharpgis.net/");
        }

        private void simonsBlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://sqlblogcasts.com/blogs/simons/default.aspx");
        }

        private void tabQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabQueries.SelectedTab != null)
            {
                if (!string.IsNullOrEmpty(this.tabQueries.SelectedTab.ToolTipText))
                {
                    string fileName = Path.GetFileName(this.tabQueries.SelectedTab.ToolTipText);
                    this.saveQueryToolStripMenuItem.Text = "Save " + fileName + " &As...";
                    this.saveToolStripMenuItem.Text = "&Save " + fileName;
                    this.saveToolStripMenuItem.Enabled = this.tabQueries.SelectedTab.Text.EndsWith("*");
                }
                else
                {
                    this.saveQueryToolStripMenuItem.Text = "Save Untitled &As...";
                    this.saveToolStripMenuItem.Text = "&Save";
                    this.saveToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void tbSqlQuery_TextChanged(object sender, EventArgs e)
        {
            Textbox textbox = sender as Textbox;
            TabPage parent = textbox.Parent as TabPage;
            if (!parent.Text.EndsWith("*"))
            {
                parent.Text = parent.Text + "*";
                parent.Refresh();
                if (!string.IsNullOrEmpty(this.tabQueries.SelectedTab.ToolTipText))
                {
                    this.saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.GetActiveTextbox().Undo();
        }

        private void useCustomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.m_customImageFilename))
            {
                this.openImageFileDialog.FileName = this.m_customImageFilename;
            }
            if (this.openImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.m_customImageFilename = this.openImageFileDialog.FileName;
                WinRegistry.SetValue("BackgroundImage", this.m_customImageFilename);
                WinRegistry.SetValue("UseCustomBackgroundImage", true);
                this.useDefaultMapToolStripMenuItem.Checked = false;
                this.useCustomToolStripMenuItem.Checked = true;
            }
        }

        private void useDefaultMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.useDefaultMapToolStripMenuItem.Checked = true;
            this.useCustomToolStripMenuItem.Checked = false;
            WinRegistry.SetValue("UseCustomBackgroundImage", false);
        }
    }
}

