using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using MapInfo.Data;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Windows.Dialogs;

namespace MapInfo.Samples.MapLoader
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.StatusBar statusBar1;
		internal System.Windows.Forms.MainMenu MainMenu1;
		internal System.Windows.Forms.MenuItem MenuItem13;
		internal System.Windows.Forms.MenuItem menuFileOpen;
		internal System.Windows.Forms.MenuItem MenuItem20;
		internal System.Windows.Forms.MenuItem menuClearMap;
		internal System.Windows.Forms.MenuItem menuTableSearchPath;
		internal System.Windows.Forms.MenuItem MenuItem18;
		internal System.Windows.Forms.MenuItem menuFileExit;
		internal System.Windows.Forms.MenuItem MenuItem1;
		internal System.Windows.Forms.MenuItem menuTableLoaderOptions;
		internal System.Windows.Forms.MenuItem MenuItem7;
		internal System.Windows.Forms.MenuItem menuTableLoaderOneFile;
		internal System.Windows.Forms.MenuItem menuTableLoaderMultipleFiles;
		internal System.Windows.Forms.MenuItem menuTableLoaderFileArray;
		internal System.Windows.Forms.MenuItem MenuItem10;
		internal System.Windows.Forms.MenuItem menuTableLoaderMultipleTables;
		internal System.Windows.Forms.MenuItem menuTableLoaderTableEnumerator;
		internal System.Windows.Forms.MenuItem menuTableLoaderTableInfo;
		internal System.Windows.Forms.MenuItem MenuItem6;
		internal System.Windows.Forms.MenuItem menuGeosetLoaderOptions;
		internal System.Windows.Forms.MenuItem menuGeosetLoaderLoadGeoset;
		internal System.Windows.Forms.MenuItem menuWorkspaceLoader;
		internal System.Windows.Forms.MenuItem menuWorkspaceLoaderOptions;
		internal System.Windows.Forms.MenuItem menuWorkspaceLoaderLoadWorkspace;
		private System.Windows.Forms.MenuItem menuItemFileCloseTables;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem menuItemPickFiles;
		private System.Windows.Forms.MenuItem LoadNamedConnectionMenuItem;
		private System.Windows.Forms.MenuItem AddNamedConnectionMenuItem;
		private System.Windows.Forms.MenuItem SaveNamedConnectionMenuItem;
		private System.Windows.Forms.Panel panel1;
		private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private MapLoaderOptions dlgMapLoaderOptions = new MapLoaderOptions();

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			Session.Current.Catalog.TableOpenedEvent += new TableOpenedEventHandler(TableOpened);
			Session.Current.Catalog.TableIsClosingEvent += new TableIsClosingEventHandler(TableIsClosing);

			// Assign the Pan tool to the middle mouse button
			mapControl1.Tools.MiddleButtonTool = "Pan";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				MapInfo.Engine.Session.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.MainMenu1 = new System.Windows.Forms.MainMenu();
			this.MenuItem13 = new System.Windows.Forms.MenuItem();
			this.menuFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemFileCloseTables = new System.Windows.Forms.MenuItem();
			this.MenuItem20 = new System.Windows.Forms.MenuItem();
			this.menuClearMap = new System.Windows.Forms.MenuItem();
			this.menuTableSearchPath = new System.Windows.Forms.MenuItem();
			this.MenuItem18 = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.MenuItem1 = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderOptions = new System.Windows.Forms.MenuItem();
			this.MenuItem7 = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderOneFile = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderMultipleFiles = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderFileArray = new System.Windows.Forms.MenuItem();
			this.menuItemPickFiles = new System.Windows.Forms.MenuItem();
			this.MenuItem10 = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderMultipleTables = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderTableEnumerator = new System.Windows.Forms.MenuItem();
			this.menuTableLoaderTableInfo = new System.Windows.Forms.MenuItem();
			this.MenuItem6 = new System.Windows.Forms.MenuItem();
			this.menuGeosetLoaderOptions = new System.Windows.Forms.MenuItem();
			this.menuGeosetLoaderLoadGeoset = new System.Windows.Forms.MenuItem();
			this.menuWorkspaceLoader = new System.Windows.Forms.MenuItem();
			this.menuWorkspaceLoaderOptions = new System.Windows.Forms.MenuItem();
			this.LoadNamedConnectionMenuItem = new System.Windows.Forms.MenuItem();
			this.AddNamedConnectionMenuItem = new System.Windows.Forms.MenuItem();
			this.SaveNamedConnectionMenuItem = new System.Windows.Forms.MenuItem();
			this.menuWorkspaceLoaderLoadWorkspace = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
			this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(375, 242);
			this.mapControl1.TabIndex = 1;
			this.mapControl1.Tools.LeftButtonTool = null;
			this.mapControl1.Tools.MiddleButtonTool = null;
			this.mapControl1.Tools.RightButtonTool = null;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 284);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(392, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// MainMenu1
			// 
			this.MainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							this.MenuItem13,
																																							this.MenuItem1,
																																							this.MenuItem6,
																																							this.menuWorkspaceLoader});
			// 
			// MenuItem13
			// 
			this.MenuItem13.Index = 0;
			this.MenuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							 this.menuFileOpen,
																																							 this.menuItemFileCloseTables,
																																							 this.MenuItem20,
																																							 this.menuClearMap,
																																							 this.menuTableSearchPath,
																																							 this.MenuItem18,
																																							 this.menuFileExit});
			this.MenuItem13.Text = "File";
			// 
			// menuFileOpen
			// 
			this.menuFileOpen.Index = 0;
			this.menuFileOpen.Text = "&Open...";
			this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// menuItemFileCloseTables
			// 
			this.menuItemFileCloseTables.Enabled = false;
			this.menuItemFileCloseTables.Index = 1;
			this.menuItemFileCloseTables.Text = "Close &Tables...";
			this.menuItemFileCloseTables.Click += new System.EventHandler(this.menuItemFileCloseTables_Click);
			// 
			// MenuItem20
			// 
			this.MenuItem20.Index = 2;
			this.MenuItem20.Text = "-";
			// 
			// menuClearMap
			// 
			this.menuClearMap.Index = 3;
			this.menuClearMap.Text = "&Clear Map";
			this.menuClearMap.Click += new System.EventHandler(this.menuClearMap_Click);
			// 
			// menuTableSearchPath
			// 
			this.menuTableSearchPath.Index = 4;
			this.menuTableSearchPath.Text = "Table Search Path...";
			this.menuTableSearchPath.Click += new System.EventHandler(this.menuTableSearchPath_Click);
			// 
			// MenuItem18
			// 
			this.MenuItem18.Index = 5;
			this.MenuItem18.Text = "-";
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 6;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// MenuItem1
			// 
			this.MenuItem1.Index = 1;
			this.MenuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							this.menuTableLoaderOptions,
																																							this.MenuItem7,
																																							this.menuTableLoaderOneFile,
																																							this.menuTableLoaderMultipleFiles,
																																							this.menuTableLoaderFileArray,
																																							this.menuItemPickFiles,
																																							this.MenuItem10,
																																							this.menuTableLoaderMultipleTables,
																																							this.menuTableLoaderTableEnumerator,
																																							this.menuTableLoaderTableInfo});
			this.MenuItem1.Text = "&Table Loader";
			// 
			// menuTableLoaderOptions
			// 
			this.menuTableLoaderOptions.Index = 0;
			this.menuTableLoaderOptions.Text = "&Options...";
			this.menuTableLoaderOptions.Click += new System.EventHandler(this.menuTableLoaderOptions_Click);
			// 
			// MenuItem7
			// 
			this.MenuItem7.Index = 1;
			this.MenuItem7.Text = "-";
			// 
			// menuTableLoaderOneFile
			// 
			this.menuTableLoaderOneFile.Index = 2;
			this.menuTableLoaderOneFile.Text = "One File";
			this.menuTableLoaderOneFile.Click += new System.EventHandler(this.menuTableLoaderOneFile_Click);
			// 
			// menuTableLoaderMultipleFiles
			// 
			this.menuTableLoaderMultipleFiles.Index = 3;
			this.menuTableLoaderMultipleFiles.Text = "Multiple Files";
			this.menuTableLoaderMultipleFiles.Click += new System.EventHandler(this.menuTableLoaderMultipleFiles_Click);
			// 
			// menuTableLoaderFileArray
			// 
			this.menuTableLoaderFileArray.Index = 4;
			this.menuTableLoaderFileArray.Text = "File Array";
			this.menuTableLoaderFileArray.Click += new System.EventHandler(this.menuTableLoaderFileArray_Click);
			// 
			// menuItemPickFiles
			// 
			this.menuItemPickFiles.Index = 5;
			this.menuItemPickFiles.Text = "Pick Files...";
			this.menuItemPickFiles.Click += new System.EventHandler(this.menuItemPickFiles_Click);
			// 
			// MenuItem10
			// 
			this.MenuItem10.Index = 6;
			this.MenuItem10.Text = "-";
			// 
			// menuTableLoaderMultipleTables
			// 
			this.menuTableLoaderMultipleTables.Index = 7;
			this.menuTableLoaderMultipleTables.Text = "Multiple Tables";
			this.menuTableLoaderMultipleTables.Click += new System.EventHandler(this.menuTableLoaderMultipleTables_Click);
			// 
			// menuTableLoaderTableEnumerator
			// 
			this.menuTableLoaderTableEnumerator.Index = 8;
			this.menuTableLoaderTableEnumerator.Text = "Table Enumerator";
			this.menuTableLoaderTableEnumerator.Click += new System.EventHandler(this.menuTableLoaderTableEnumerator_Click);
			// 
			// menuTableLoaderTableInfo
			// 
			this.menuTableLoaderTableInfo.Index = 9;
			this.menuTableLoaderTableInfo.Text = "TableInfo";
			this.menuTableLoaderTableInfo.Click += new System.EventHandler(this.menuTableLoaderTableInfo_Click);
			// 
			// MenuItem6
			// 
			this.MenuItem6.Index = 2;
			this.MenuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							this.menuGeosetLoaderOptions,
																																							this.menuGeosetLoaderLoadGeoset});
			this.MenuItem6.Text = "&Geoset Loader";
			// 
			// menuGeosetLoaderOptions
			// 
			this.menuGeosetLoaderOptions.Index = 0;
			this.menuGeosetLoaderOptions.Text = "&Options...";
			this.menuGeosetLoaderOptions.Click += new System.EventHandler(this.menuGeosetLoaderOptions_Click);
			// 
			// menuGeosetLoaderLoadGeoset
			// 
			this.menuGeosetLoaderLoadGeoset.Index = 1;
			this.menuGeosetLoaderLoadGeoset.Text = "Load Geoset...";
			this.menuGeosetLoaderLoadGeoset.Click += new System.EventHandler(this.menuGeosetLoaderLoadGeoset_Click);
			// 
			// menuWorkspaceLoader
			// 
			this.menuWorkspaceLoader.Index = 3;
			this.menuWorkspaceLoader.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																												this.menuWorkspaceLoaderOptions,
																																												this.LoadNamedConnectionMenuItem,
																																												this.AddNamedConnectionMenuItem,
																																												this.SaveNamedConnectionMenuItem,
																																												this.menuWorkspaceLoaderLoadWorkspace});
			this.menuWorkspaceLoader.Text = "&Workspace Loader";
			// 
			// menuWorkspaceLoaderOptions
			// 
			this.menuWorkspaceLoaderOptions.Index = 0;
			this.menuWorkspaceLoaderOptions.Text = "&Options...";
			this.menuWorkspaceLoaderOptions.Click += new System.EventHandler(this.menuWorkspaceLoaderOptions_Click);
			// 
			// LoadNamedConnectionMenuItem
			// 
			this.LoadNamedConnectionMenuItem.Index = 1;
			this.LoadNamedConnectionMenuItem.Text = "Load Named Connection...";
			this.LoadNamedConnectionMenuItem.Click += new System.EventHandler(this.LoadNamedConnectionMenuItem_Click);
			// 
			// AddNamedConnectionMenuItem
			// 
			this.AddNamedConnectionMenuItem.Index = 2;
			this.AddNamedConnectionMenuItem.Text = "Add Named Connection...";
			this.AddNamedConnectionMenuItem.Click += new System.EventHandler(this.AddNamedConnectionMenuItem_Click);
			// 
			// SaveNamedConnectionMenuItem
			// 
			this.SaveNamedConnectionMenuItem.Index = 3;
			this.SaveNamedConnectionMenuItem.Text = "Save Named Connection...";
			this.SaveNamedConnectionMenuItem.Click += new System.EventHandler(this.SaveNamedConnectionMenuItem_Click);
			// 
			// menuWorkspaceLoaderLoadWorkspace
			// 
			this.menuWorkspaceLoaderLoadWorkspace.Index = 4;
			this.menuWorkspaceLoaderLoadWorkspace.Text = "Load Workspace...";
			this.menuWorkspaceLoaderLoadWorkspace.Click += new System.EventHandler(this.menuWorkspaceLoaderLoadWorkspace_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Location = new System.Drawing.Point(8, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(379, 246);
			this.panel1.TabIndex = 4;
			// 
			// mapToolBar1
			// 
			this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																																									 this.mapToolBarButtonOpenTable,
																																									 this.toolBarButtonSeparator,
																																									 this.mapToolBarButtonSelect,
																																									 this.mapToolBarButtonZoomIn,
																																									 this.mapToolBarButtonZoomOut,
																																									 this.mapToolBarButtonPan});
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(8, 0);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(160, 26);
			this.mapToolBar1.TabIndex = 5;
			// 
			// mapToolBarButtonOpenTable
			// 
			this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
			this.mapToolBarButtonOpenTable.ToolTipText = "Open Table";
			// 
			// toolBarButtonSeparator
			// 
			this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// mapToolBarButtonSelect
			// 
			this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
			this.mapToolBarButtonSelect.ToolTipText = "Select";
			// 
			// mapToolBarButtonZoomIn
			// 
			this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
			this.mapToolBarButtonZoomIn.ToolTipText = "Zoom-in";
			// 
			// mapToolBarButtonZoomOut
			// 
			this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
			this.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out";
			// 
			// mapToolBarButtonPan
			// 
			this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
			this.mapToolBarButtonPan.ToolTipText = "Pan";
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 303);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.MainMenu1;
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "MapForm1";
			this.Text = "MapForm1";
			this.Load += new System.EventHandler(this.MapForm1_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MapForm1());
		}

		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) 
		{
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}


		private void TableOpened(object o, TableOpenedEventArgs e)
		{
			this.menuItemFileCloseTables.Enabled = Session.Current.Catalog.Count > 0;
		}

		private void TableIsClosing(object o, TableIsClosingEventArgs e)
		{
			this.menuItemFileCloseTables.Enabled = Session.Current.Catalog.Count > 1;
		}
		
		private void menuFileOpen_Click(object sender, System.EventArgs e)
		{
			LoadMapWizard loadMapWizard = new LoadMapWizard();
			loadMapWizard.ShowDbms = true;
			loadMapWizard.Run(this, mapControl1.Map);
		}

		private void menuClearMap_Click(object sender, System.EventArgs e)
		{
			mapControl1.Map.Clear();
		}

		private void menuFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void menuItemFileCloseTables_Click(object sender, System.EventArgs e)
		{
			SelectTablesDlg dlg = new SelectTablesDlg(Session.Current.Catalog.EnumerateTables());
			dlg.Text = "Close Tables";
			if (dlg.ShowDialog() == DialogResult.OK && dlg.SelectedTables != null)
			{
				foreach(Table t in dlg.SelectedTables)
				{
					t.Close();
				}
				mapControl1.Map.Invalidate();
			}
			dlg.Dispose();
		}

		private void menuTableSearchPath_Click(object sender, System.EventArgs e)
		{
			TableSearchPathDialog dlg = new TableSearchPathDialog();
			dlg.Path = Session.Current.TableSearchPath.Path;
			if (dlg.ShowDialog(this) == DialogResult.OK) 
			{
				Session.Current.TableSearchPath.Path = dlg.Path;
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
				key.SetValue("SampleDataSearchPath", Session.Current.TableSearchPath.Path);
				key.Close();
			}
			dlg.Dispose();
		}

		private void MapForm1_Load(object sender, System.EventArgs e)
		{
			// Set table search path to value sampledatasearch registry key
			// if not found, then just use the app's current directory
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			string s = (string)key.GetValue("SampleDataSearchPath");
			if (s != null && s.Length > 0) 
			{
				if (s.EndsWith("\\")==false) 
				{
					s += "\\";
				}
			}
			else 
			{
				s = Environment.CurrentDirectory;
			}
			key.Close();
	
			Session.Current.TableSearchPath.Path = s;			
		}

		private void menuTableLoaderOptions_Click(object sender, System.EventArgs e)
		{
			dlgMapLoaderOptions.ShowDialog();
		}

		/// <summary>
		/// Utility method to set table loader options and call Map.Load
		/// </summary>
		private void LoadTables(MapInfo.Mapping.MapLoader tl)
		{
			try 
			{
				// Set table loader options
				tl.AutoPosition = dlgMapLoaderOptions.AutoPosition;
				tl.StartPosition = dlgMapLoaderOptions.StartPosition;
				tl.EnableLayers = dlgMapLoaderOptions.EnableLayers?EnableLayers.Enable : EnableLayers.Disable;

				if (dlgMapLoaderOptions.ClearMapFirst) 
				{
					mapControl1.Map.Clear();
				}
				mapControl1.Map.Load(tl);
			}
			catch(Exception ex)
			{
				string s = ex.Message + "\r\nMake sure the TableSearchPath '" + Session.Current.TableSearchPath.Path + "' is set to point to the location where the sample data is installed";
				MessageBox.Show(s);
			}
		}
		private void menuTableLoaderOneFile_Click(object sender, System.EventArgs e)
		{
			MapTableLoader tl = new MapTableLoader("world.tab");
			LoadTables(tl);
		}

		private void menuTableLoaderMultipleFiles_Click(object sender, System.EventArgs e)
		{
			MapTableLoader tl = new MapTableLoader("ocean.tab", "usa.tab", "mexico.tab", "us_hiway.tab");
			LoadTables(tl);		
		}

		private void menuTableLoaderFileArray_Click(object sender, System.EventArgs e)
		{
			string[] tables = new string[3];
			tables[0]="us_cnty.tab";
			tables[1]="usa_caps.tab";
			tables[2]="uscty_1k.tab";

			MapTableLoader tl = new MapTableLoader(tables);
			LoadTables(tl);
		}

		private void menuItemPickFiles_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = true;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "TAB";
			openFileDialog1.Filter = "MapInfo Tables (*.tab)|*.tab||";
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				MapTableLoader tl = new MapTableLoader();
				foreach(string filename in openFileDialog1.FileNames)	
				{
					tl.Add(filename);
				}
				LoadTables(tl);
			}
		}

		private void menuTableLoaderMultipleTables_Click(object sender, System.EventArgs e)
		{
			string path;
			Session.Current.TableSearchPath.FileExists("world.tab", out path);
			Table t1 = Session.Current.Catalog.OpenTable(path);
			Session.Current.TableSearchPath.FileExists("usa.tab", out path);
			Table t2 = Session.Current.Catalog.OpenTable(path);
			Session.Current.TableSearchPath.FileExists("mexico.tab", out path);
			Table t3 = Session.Current.Catalog.OpenTable(path);
			MapTableLoader tl = new MapTableLoader(t1, t2 ,t3);
			LoadTables(tl);
		}

		private void menuTableLoaderTableEnumerator_Click(object sender, System.EventArgs e)
		{
			// load a map with all tables opened so far.
			// clearing the map before this one would be a good idea
			MapTableLoader tl = new MapTableLoader(Session.Current.Catalog.EnumerateTables());
			LoadTables(tl);
		}

		private void menuTableLoaderTableInfo_Click(object sender, System.EventArgs e)
		{
			string path;
			Session.Current.TableSearchPath.FileExists("world.tab", out path);
			TableInfo ti = TableInfo.CreateFromFile(path);
			MapTableLoader tl = new MapTableLoader(ti);		
			LoadTables(tl);
		}
		private void menuGeosetLoaderOptions_Click(object sender, System.EventArgs e)
		{
			menuTableLoaderOptions_Click(sender, e);
		}

		private void menuWorkspaceLoaderOptions_Click(object sender, System.EventArgs e)
		{
			menuTableLoaderOptions_Click(sender, e);
		}

		private void menuGeosetLoaderLoadGeoset_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = false;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "GST";
			openFileDialog1.Filter = "MapInfo Tables (*.gst)|*.gst||";
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				MapGeosetLoader gl = new MapGeosetLoader(openFileDialog1.FileName);
				// set geoset specific options
				gl.LayersOnly = dlgMapLoaderOptions.LayersOnly;
				gl.SetMapName = dlgMapLoaderOptions.SetMapName;
				LoadTables(gl);
			}		
		}

		private void menuWorkspaceLoaderLoadWorkspace_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = false;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "MWS";
			openFileDialog1.Filter = "MapInfo Tables (*.mws)|*.mws||";
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				MapWorkSpaceLoader mwl = new MapWorkSpaceLoader(openFileDialog1.FileName);
				// set geoset specific options
				mwl.LayersOnly = dlgMapLoaderOptions.LayersOnly;
				mwl.SetMapName = dlgMapLoaderOptions.SetMapName;
				LoadTables(mwl);
			}		
		
		}

		// This method is to show you, how to load a pre-defined named connection xml file.
		// sample_namedConnection.xml is the file that we loaded.
		// 
		private void LoadNamedConnectionMenuItem_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = false;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "XML";
			openFileDialog1.Filter = "Named Connection (*.xml)|*.xml||";
			openFileDialog1.FileName = "sample_namedConnection.xml";
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				try 
				{
					Session.Current.Catalog.NamedConnections.Load(openFileDialog1.FileName);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
				MessageBox.Show(openFileDialog1.FileName + " is loaded.");
			}
			// code shows, how to enumerate a collection of named connections
			System.Collections.IDictionaryEnumerator eu = Session.Current.Catalog.NamedConnections.GetEnumerator();
			int i = 0;
			while (eu.MoveNext())
			{
				NamedConnectionInfo info = eu.Value as NamedConnectionInfo;
				string aline = string.Format("named connection {0}:{1}:{2},{3},{4}", 
					i, eu.Key, info.DBType, info.ConnectionMethod, info.ConnectionString);
				MessageBox.Show(aline);
				++i;
			}
		}

		private void AddNamedConnectionMenuItem_Click(object sender, System.EventArgs e)
		{
			// create a named connection called "mylocalData" and point it to "c:\mylocal\maps"
			NamedConnectionInfo info = new NamedConnectionInfo("file", ConnectionMethod.FilePath, @"c:\mylocal\maps");
			Session.Current.Catalog.NamedConnections.Add("mylocalData", info);
			MessageBox.Show(@"A hard coded named connection, mylocalData (which points to 'c:\mylocal\maps'), is added");
		}

		private void SaveNamedConnectionMenuItem_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = false;
			openFileDialog1.CheckFileExists = false;
			openFileDialog1.DefaultExt = "XML";
			openFileDialog1.Filter = "Named Connection (*.xml)|*.xml||";
			openFileDialog1.FileName = "sample_namedConnection.xml";
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				Session.Current.Catalog.NamedConnections.Save(openFileDialog1.FileName);
				MessageBox.Show(openFileDialog1.FileName + " is saved.");
			}
		}		
	}
}
