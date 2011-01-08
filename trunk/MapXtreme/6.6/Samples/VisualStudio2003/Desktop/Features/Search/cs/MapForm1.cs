using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Reflection;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Styles;

namespace Search
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Windows.Forms.MenuItem menuItemMapSearchWithinScreenRect;
		private System.Windows.Forms.MenuItem menuItemMapSearchWithinScreenRadius;
		private System.Windows.Forms.MenuItem menuItemMapSearchNearest;
		private System.ComponentModel.Container components = null;

		private Map _map=null;					// will be set to map from mapcontrol
		private Catalog _catalog=Session.Current.Catalog;
		private Selection _selection=Session.Current.Selections.DefaultSelection;
		private System.Windows.Forms.MenuItem menuItemSearchWhere;
		private System.Windows.Forms.MenuItem menuItemSearchWithinFeature;
		private System.Windows.Forms.MenuItem menuItemSearchWithinRect;
		private System.Windows.Forms.MenuItem menuItemSearchIntersectsFeature;
		private System.Windows.Forms.MenuItem menuItemSearchWithinDistance;
		private System.Windows.Forms.MenuItem menuItemSearchNearest;
		private System.Windows.Forms.MenuItem menuItemSqlExpressionFilter;
		private System.Windows.Forms.MenuItem menuItemCustomProcessor;
		private System.Windows.Forms.MenuItem menuItemSearchMultipleTables;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItemLogicalFilter;
		private System.Windows.Forms.MenuItem menuItemCustomQueryFilter;
		private System.Windows.Forms.MenuItem menuItemIntersectFeature;
		private System.Windows.Forms.MenuItem menuItemContainsFilter;
		private System.Windows.Forms.MenuItem menuItemSetColumns;
		private System.Windows.Forms.MenuItem menuItem7;
		private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private Table _tempTable=null;

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);
			
			_map = mapControl1.Map;

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
			}
			_map=null;
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemSearchWhere = new System.Windows.Forms.MenuItem();
			this.menuItemSearchWithinFeature = new System.Windows.Forms.MenuItem();
			this.menuItemSearchWithinRect = new System.Windows.Forms.MenuItem();
			this.menuItemSearchIntersectsFeature = new System.Windows.Forms.MenuItem();
			this.menuItemSearchWithinDistance = new System.Windows.Forms.MenuItem();
			this.menuItemSearchNearest = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemSearchMultipleTables = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItemCustomProcessor = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemMapSearchWithinScreenRect = new System.Windows.Forms.MenuItem();
			this.menuItemMapSearchWithinScreenRadius = new System.Windows.Forms.MenuItem();
			this.menuItemMapSearchNearest = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemSqlExpressionFilter = new System.Windows.Forms.MenuItem();
			this.menuItemContainsFilter = new System.Windows.Forms.MenuItem();
			this.menuItemIntersectFeature = new System.Windows.Forms.MenuItem();
			this.menuItemCustomQueryFilter = new System.Windows.Forms.MenuItem();
			this.menuItemLogicalFilter = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItemSetColumns = new System.Windows.Forms.MenuItem();
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
			this.mapControl1.Size = new System.Drawing.Size(394, 244);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.LeftButtonTool = null;
			this.mapControl1.Tools.MiddleButtonTool = null;
			this.mapControl1.Tools.RightButtonTool = null;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Location = new System.Drawing.Point(4, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(398, 248);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 286);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(406, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem19,
																					  this.menuItem1,
																					  this.menuItem2,
																					  this.menuItem5});
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 0;
			this.menuItem19.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuFileExit});
			this.menuItem19.Text = "&File";
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 0;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemSearchWhere,
																					  this.menuItemSearchWithinFeature,
																					  this.menuItemSearchWithinRect,
																					  this.menuItemSearchIntersectsFeature,
																					  this.menuItemSearchWithinDistance,
																					  this.menuItemSearchNearest,
																					  this.menuItem4,
																					  this.menuItemSearchMultipleTables,
																					  this.menuItem11,
																					  this.menuItemCustomProcessor});
			this.menuItem1.Text = "Search";
			// 
			// menuItemSearchWhere
			// 
			this.menuItemSearchWhere.Index = 0;
			this.menuItemSearchWhere.Text = "SearchWhere";
			this.menuItemSearchWhere.Click += new System.EventHandler(this.menuItemSearchWhere_Click);
			// 
			// menuItemSearchWithinFeature
			// 
			this.menuItemSearchWithinFeature.Index = 1;
			this.menuItemSearchWithinFeature.Text = "SearchWithinGeometry";
			this.menuItemSearchWithinFeature.Click += new System.EventHandler(this.menuItemSearchWithinGeometry_Click);
			// 
			// menuItemSearchWithinRect
			// 
			this.menuItemSearchWithinRect.Index = 2;
			this.menuItemSearchWithinRect.Text = "SearchWithinRect";
			this.menuItemSearchWithinRect.Click += new System.EventHandler(this.menuItemSearchWithinRect_Click);
			// 
			// menuItemSearchIntersectsFeature
			// 
			this.menuItemSearchIntersectsFeature.Index = 3;
			this.menuItemSearchIntersectsFeature.Text = "SearchIntersectsFeature";
			this.menuItemSearchIntersectsFeature.Click += new System.EventHandler(this.menuItemSearchIntersectsFeature_Click);
			// 
			// menuItemSearchWithinDistance
			// 
			this.menuItemSearchWithinDistance.Index = 4;
			this.menuItemSearchWithinDistance.Text = "SearchWithinDistance";
			this.menuItemSearchWithinDistance.Click += new System.EventHandler(this.menuItemSearchWithinDistance_Click);
			// 
			// menuItemSearchNearest
			// 
			this.menuItemSearchNearest.Index = 5;
			this.menuItemSearchNearest.Text = "SearchNearest";
			this.menuItemSearchNearest.Click += new System.EventHandler(this.menuItemSearchNearest_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 6;
			this.menuItem4.Text = "-";
			// 
			// menuItemSearchMultipleTables
			// 
			this.menuItemSearchMultipleTables.Index = 7;
			this.menuItemSearchMultipleTables.Text = "Search Multiple Tables";
			this.menuItemSearchMultipleTables.Click += new System.EventHandler(this.menuItemSearchMultipleTables_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 8;
			this.menuItem11.Text = "-";
			// 
			// menuItemCustomProcessor
			// 
			this.menuItemCustomProcessor.Index = 9;
			this.menuItemCustomProcessor.Text = "CustomSearchResultProcessor";
			this.menuItemCustomProcessor.Click += new System.EventHandler(this.menuItemCustomProcessor_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemMapSearchWithinScreenRect,
																					  this.menuItemMapSearchWithinScreenRadius,
																					  this.menuItemMapSearchNearest});
			this.menuItem2.Text = "MapSearch";
			// 
			// menuItemMapSearchWithinScreenRect
			// 
			this.menuItemMapSearchWithinScreenRect.Index = 0;
			this.menuItemMapSearchWithinScreenRect.Text = "SearchWithinScreenRect";
			this.menuItemMapSearchWithinScreenRect.Click += new System.EventHandler(this.menuItemMapSearchWithinScreenRect_Click);
			// 
			// menuItemMapSearchWithinScreenRadius
			// 
			this.menuItemMapSearchWithinScreenRadius.Index = 1;
			this.menuItemMapSearchWithinScreenRadius.Text = "SearchWithinScreenRadius";
			this.menuItemMapSearchWithinScreenRadius.Click += new System.EventHandler(this.menuItemMapSearchWithinScreenRadius_Click);
			// 
			// menuItemMapSearchNearest
			// 
			this.menuItemMapSearchNearest.Index = 2;
			this.menuItemMapSearchNearest.Text = "SearchNearest";
			this.menuItemMapSearchNearest.Click += new System.EventHandler(this.menuItemMapSearchNearest_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemSqlExpressionFilter,
																					  this.menuItemContainsFilter,
																					  this.menuItemIntersectFeature,
																					  this.menuItemCustomQueryFilter,
																					  this.menuItemLogicalFilter,
																					  this.menuItem7,
																					  this.menuItemSetColumns});
			this.menuItem5.Text = "QueryDefinition";
			// 
			// menuItemSqlExpressionFilter
			// 
			this.menuItemSqlExpressionFilter.Index = 0;
			this.menuItemSqlExpressionFilter.Text = "SqlExpressionFilter";
			this.menuItemSqlExpressionFilter.Click += new System.EventHandler(this.menuItemSqlExpressionFilter_Click);
			// 
			// menuItemContainsFilter
			// 
			this.menuItemContainsFilter.Index = 1;
			this.menuItemContainsFilter.Text = "ContainsFilter";
			this.menuItemContainsFilter.Click += new System.EventHandler(this.menuItemContainsFilter_Click);
			// 
			// menuItemIntersectFeature
			// 
			this.menuItemIntersectFeature.Index = 2;
			this.menuItemIntersectFeature.Text = "IntersectFilter";
			this.menuItemIntersectFeature.Click += new System.EventHandler(this.menuItemIntersectFeature_Click);
			// 
			// menuItemCustomQueryFilter
			// 
			this.menuItemCustomQueryFilter.Index = 3;
			this.menuItemCustomQueryFilter.Text = "CustomQueryFilter";
			this.menuItemCustomQueryFilter.Click += new System.EventHandler(this.menuItemCustomQueryFilter_Click);
			// 
			// menuItemLogicalFilter
			// 
			this.menuItemLogicalFilter.Index = 4;
			this.menuItemLogicalFilter.Text = "LogicalFilter";
			this.menuItemLogicalFilter.Click += new System.EventHandler(this.menuItemLogicalFilter_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 5;
			this.menuItem7.Text = "-";
			// 
			// menuItemSetColumns
			// 
			this.menuItemSetColumns.Index = 6;
			this.menuItemSetColumns.Text = "Setting Columns";
			this.menuItemSetColumns.Click += new System.EventHandler(this.menuItemSetColumns_Click);
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
			this.mapToolBar1.ButtonSize = new System.Drawing.Size(25, 22);
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(8, 0);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(160, 26);
			this.mapToolBar1.TabIndex = 3;
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
			this.ClientSize = new System.Drawing.Size(406, 305);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "MapForm1";
			this.Text = "Search Sample";
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
		
			// load some layers, set csys and default view
			try 
			{
				_map.Load(new MapTableLoader( "usa.tab", "mexico.tab", "usa_caps.tab", "uscty_1k.tab", "grid15.tab"));
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Unable to load tables. Sample Data may not be installed.\r\n" +  ex.Message);
				this.Close();
				return;
			}
			_map.SetDisplayCoordSys((_map.Layers["grid15"] as FeatureLayer).CoordSys);
			FeatureLayer lyr = _map.Layers["uscty_1k"] as FeatureLayer;
			_map.SetView(lyr.DefaultBounds, lyr.CoordSys);

			// create and add temp layer
			TableInfo ti = TableInfoFactory.CreateTemp("temp"); // create tableinfo with just obj and style cols
			_tempTable = _catalog.CreateTable(ti);
			_map.Layers.Insert(0, new FeatureLayer(_tempTable));
		}

		private void menuFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		
		
		private void ShowSearchGeometry(FeatureGeometry g)
		{
			ShowSearchGeometry(g, true);
		}

		// add geometry to temp layer with hollow style
		private void ShowSearchGeometry(FeatureGeometry g, bool clear)
		{
			if (clear) 
			{
				// first clear out any other geometries from table
				(_tempTable as IFeatureCollection).Clear();
			}

			Style s=null;
			if (g is IGenericSurface) // closed geometry, use area style
			{
				s  = new AreaStyle(new SimpleLineStyle(new LineWidth(2, LineWidthUnit.Pixel), 2, Color.Red, false), new SimpleInterior(0));
			}
			else if (g is MapInfo.Geometry.Point) 
			{
				s = new SimpleVectorPointStyle(34, Color.Red, 18);
			}
			Feature f = new Feature(g, s);

			// Add feature to temp table
			_tempTable.InsertFeature(f);

		}

		// select features in feature collection
		private void SelectFeatureCollection(IResultSetFeatureCollection fc) 
		{
			// force map to update
			mapControl1.Update();

			_selection.Clear();
			_selection.Add(fc);
		}

		// this is similar to searchwithinrect, but the rect constructed is a screen rectangle
		// as opposed to a map rectangle (try both and see the difference)
		private void menuItemMapSearchWithinScreenRect_Click(object sender, System.EventArgs e)
		{

			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				rect.X += rect.Width/3;
				rect.Width = rect.Width/3;
				rect.Y += rect.Height/3;
				rect.Height = rect.Height/3;
				SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRect(_map, rect, ContainsType.Centroid);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);

				// show search geometry on screen for visual confirmation
				MapInfo.Geometry.MultiPolygon p = MapInfo.Mapping.SearchInfoFactory.CreateScreenRect(_map.Layers["temp"] as FeatureLayer, rect);
				ShowSearchGeometry(p);

				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find cities with 1/3 radius of center
		private void menuItemMapSearchWithinScreenRadius_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				System.Drawing.Point pt = new System.Drawing.Point(rect.Left, rect.Top);
				pt.X += rect.Width/2;
				pt.Y += rect.Height/2;
				SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(_map, pt, rect.Width/6, 20, ContainsType.Centroid);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);

				// show search geometry on screen for visual confirmation
				MapInfo.Geometry.MultiPolygon p = MapInfo.Mapping.SearchInfoFactory.CreateScreenCircle(_map.Layers["temp"] as FeatureLayer, pt, rect.Width/6, 20);
				ShowSearchGeometry(p);

				SelectFeatureCollection(fc);		
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find cities nearest to center within 3 pixel radius
		private void menuItemMapSearchNearest_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				System.Drawing.Point pt = new System.Drawing.Point(rect.Left, rect.Top);
				pt.X += rect.Width/2;
				pt.Y += rect.Height/2;


				SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt, 3);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);

				rect.X = pt.X;
				rect.Y = pt.Y;
				rect.Width = 0;
				rect.Height = 0;
				rect.Inflate(3, 3);
				// show search geometry on screen for visual confirmation
				MapInfo.Geometry.MultiPolygon p = MapInfo.Mapping.SearchInfoFactory.CreateScreenRect(_map, rect);
				ShowSearchGeometry(p);

				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find states whre pop 1990 < 4 million
		private void menuItemSearchWhere_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// find and select all states with pop > 2 million
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWhere("POP_90 > 2000000");
				IResultSetFeatureCollection fc = _catalog.Search("mexico", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find and select cities with Georgia and Florida using geometry
		private void menuItemSearchWithinGeometry_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// find and select cities with Georgia and Florida using geometry
				// also uses search for feature
				Feature fFlorida  = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"));
				Feature fGeorgia  = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='GA'"));
				FeatureGeometry g = fFlorida.Geometry.Combine(fGeorgia.Geometry);
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWithinGeometry(g, ContainsType.Centroid);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				ShowSearchGeometry(g);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// this is similar to searchwithinscreenrect, but the rect constructed is a map rectangle
		// as opposed to a screen rectangle (try both and see the difference)
		private void menuItemSearchWithinRect_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				rect.X += rect.Width/3;
				rect.Width = rect.Width/3;
				rect.Y += rect.Height/3;
				rect.Height = rect.Height/3;
				DRect mapRect=new DRect();

				// use csys and transform of feature layer, because that is the 
				// layer we are doing the search on
				FeatureLayer layer = _map.Layers["uscty_1k"] as FeatureLayer;
				layer.DisplayTransform.FromDisplay(rect, out mapRect);
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWithinRect(mapRect, layer.CoordSys, ContainsType.Centroid);
				IResultSetFeatureCollection fc = _catalog.Search(layer.Table, si);

				// show search geometry on screen for visual confirmation
				DPoint []pts = new DPoint[4];
				mapRect.GetCornersOfRect(pts);
				FeatureGeometry g = new MapInfo.Geometry.MultiPolygon(layer.CoordSys, CurveSegmentType.Linear, pts);
				ShowSearchGeometry(g);

				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find states that intersect KS
		private void menuItemSearchIntersectsFeature_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// find states that intersect KS
				// also uses search for feature
				Feature fKS = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='KS'"));
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchIntersectsFeature(fKS, IntersectType.Geometry);
				IResultSetFeatureCollection fc = _catalog.Search("usa", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				ShowSearchGeometry(fKS.Geometry);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		
		// find cities with distance of center of map
		private void menuItemSearchWithinDistance_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// to compare to SearchWithinScreenRadius, we are calculating
				// the search distance the same way it does
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				System.Drawing.Point pt = new System.Drawing.Point(rect.Left, rect.Top);
				pt.X += rect.Width/2;
				pt.Y += rect.Height/2;

				DPoint dpt1 = new DPoint();
				// convert center point to map coords (could use map.Center)
				_map.DisplayTransform.FromDisplay(pt, out dpt1);

				Distance d = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(_map, rect.Width/6);
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWithinDistance(dpt1, _map.GetDisplayCoordSys(), d, ContainsType.Centroid);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);

				// show search geometry on screen for visual confirmation
				MapInfo.Geometry.Point p = new MapInfo.Geometry.Point(_map.GetDisplayCoordSys(), dpt1);
				FeatureGeometry buffer = p.Buffer(d.Value, d.Unit, 20, DistanceType.Spherical);
				ShowSearchGeometry(buffer);

				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// find nearest city to center of map 
		private void menuItemSearchNearest_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// to compare to SearchWithinScreenRadius, we are calculating
				// the search distance the same way it does
				System.Drawing.Rectangle rect=mapControl1.Bounds;
				System.Drawing.Point pt = new System.Drawing.Point(rect.Left, rect.Top);
				pt.X += rect.Width/2;
				pt.Y += rect.Height/2;

				DPoint dpt1 = new DPoint();
				// convert center point to map coords (could use map.Center)
				_map.DisplayTransform.FromDisplay(pt, out dpt1);
				Distance d = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(_map, 3);

				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchNearest(dpt1, _map.GetDisplayCoordSys(), d);
				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);

				MapInfo.Geometry.Point p = new MapInfo.Geometry.Point(_map.GetDisplayCoordSys(), dpt1);
				FeatureGeometry buffer = p.Buffer(d.Value, d.Unit, 20, DistanceType.Spherical);
				ShowSearchGeometry(buffer);

				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// search through all tables to find objects that intersect
		// the states bordering KS
		private void menuItemSearchMultipleTables_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Cursor.Current = Cursors.WaitCursor;
				// find states that intersect KS
				// then combine them and search all layers within
				// also uses search for feature
				Feature fKS = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='KS'"));
				SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchIntersectsFeature(fKS, IntersectType.Geometry);
				IResultSetFeatureCollection fc = _catalog.Search("usa", si);

				MapInfo.FeatureProcessing.FeatureProcessor fp = new MapInfo.FeatureProcessing.FeatureProcessor();
				Feature f = fp.Combine(fc);

				si = MapInfo.Data.SearchInfoFactory.SearchWithinFeature(f, ContainsType.Centroid);
				MultiResultSetFeatureCollection mfc = _catalog.Search(_catalog.EnumerateTables(TableFilterFactory.FilterMappableTables()), si);

				// set map view to show search results
				_map.SetView(f);

				ShowSearchGeometry(f.Geometry);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void menuItemSqlExpressionFilter_Click(object sender, System.EventArgs e)
		{
			try 
			{
				// build up a search info by hand (not using the factory)
				QueryFilter filter = new SqlExpressionFilter("Buses_91 * 3 < Trucks_91");
				QueryDefinition qd = new QueryDefinition(filter, "*");
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("mexico", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}

		}

		// uses contains filter to get cities in florida
		private void menuItemContainsFilter_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Feature fFlorida  = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"));

				// build up a search info by hand (not using the factory)
				QueryFilter filter = new  ContainsFilter(fFlorida.Geometry, ContainsType.Geometry);
				QueryDefinition qd = new QueryDefinition(filter, "MI_Geometry", "MI_Style", "MI_Key");
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);
				// set map view to show search results
				_map.SetView(fFlorida);

				ShowSearchGeometry(fFlorida.Geometry);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// uses intersect filter to get states that intersect florida
		private void menuItemIntersectFeature_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Feature fFlorida  = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"));

				// build up a search info by hand (not using the factory)
				QueryFilter filter = new  IntersectFilter(fFlorida.Geometry, IntersectType.Bounds);
				QueryDefinition qd = new QueryDefinition(filter, "*");
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("usa", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				ShowSearchGeometry(fFlorida.Geometry);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// a custom query filter that includes objects at least
		// a certain distance from the query object and not greater than a second distance
		private class MyCustomFilter: SpatialFilter
		{
			private Distance _distanceInner = new Distance(100, DistanceUnit.Mile);
			private Distance _distanceOuter = new Distance(100, DistanceUnit.Mile);
			public MyCustomFilter(FeatureGeometry geometry, Distance innerDistance, Distance outerDistance)
				:base(geometry)
			{
				_distanceInner = innerDistance;
				_distanceOuter = outerDistance;
			}

			public override string GetSqlExpression()
			{
				return string.Format(
					"MI_CentroidDistance(MI_Geometry, {0}, '{1}', 'Spherical') > {2} and "
					+ "MI_CentroidDistance(MI_Geometry, {0}, '{3}', 'Spherical') < {4} ", 
					paramName, CoordSys.DistanceUnitAbbreviation(_distanceInner.Unit), _distanceInner.Value,
										 CoordSys.DistanceUnitAbbreviation(_distanceOuter.Unit), _distanceOuter.Value);
			}
		}

		// uses custom filter to select objects within a distance range from
		// chicago
		private void menuItemCustomQueryFilter_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Feature fChicago  = _catalog.SearchForFeature("uscty_1k", MapInfo.Data.SearchInfoFactory.SearchWhere("City='Chicago'"));

				// build up a search info by hand (not using the factory)
				Distance d1 = new Distance(35, DistanceUnit.Mile);
				Distance d2 = new Distance(125, DistanceUnit.Mile);
				QueryFilter filter = new  MyCustomFilter(fChicago.Geometry, d1, d2);
				QueryDefinition qd = new QueryDefinition(filter, "*");
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				// make a search geometry to show what we are doing
				FeatureGeometry buffer1 = fChicago.Geometry.Buffer(d1.Value, d1.Unit, 20, DistanceType.Spherical);
				FeatureGeometry buffer2 = fChicago.Geometry.Buffer(d2.Value, d2.Unit, 20, DistanceType.Spherical);
				ShowSearchGeometry(buffer1);
				ShowSearchGeometry(buffer2, false);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// shows how to combine filters using logical And
		private void menuItemLogicalFilter_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Feature fChicago  = _catalog.SearchForFeature("uscty_1k", MapInfo.Data.SearchInfoFactory.SearchWhere("City='Chicago'"));

				// build up a search info by hand (not using the factory)
				Distance d1 = new Distance(35, DistanceUnit.Mile);
				Distance d2 = new Distance(125, DistanceUnit.Mile);
				QueryFilter filterA = new  MyCustomFilter(fChicago.Geometry, d1, d2);

				// build up a search info by hand (not using the factory)
				QueryFilter filterB = new SqlExpressionFilter("State='IL'");
				QueryFilter filter = new LogicalFilter(LogicalOperation.And, filterA, filterB);
				QueryDefinition qd = new QueryDefinition(filter, "*");
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("uscty_1k", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				// make a search geometry to show what we are doing
				FeatureGeometry buffer1 = fChicago.Geometry.Buffer(d1.Value, d1.Unit, 20, DistanceType.Spherical);
				FeatureGeometry buffer2 = fChicago.Geometry.Buffer(d2.Value, d2.Unit, 20, DistanceType.Spherical);
				ShowSearchGeometry(buffer1);
				ShowSearchGeometry(buffer2, false);

				Feature fIL  = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='IL'"));
				ShowSearchGeometry(fIL.Geometry, false);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		// create a custom processor that returns only the first n rows
		public class MySearchResultProcessor: SearchResultProcessor
		{
			private int _maxRows=0;
			private int _rowCount=0;

			public MySearchResultProcessor(int maxRows)
			{
				_maxRows = maxRows;
			}
			public override void BeginProcessingTable(IResultSetFeatureCollection features)
			{
				_rowCount=0;
			}
			public override bool ProcessRow(MIDataReader reader, IResultSetFeatureCollection features)
			{
				if (_maxRows == 0 || _rowCount < _maxRows) {
					features.Add(reader.Current);
				}
				_rowCount++;
				if (_maxRows > 0 && _rowCount >= _maxRows) return false; // stop processing of this table
				return true;
			}
		}

		// return the first 10 rows from cities sorted by state in reverse
		private void menuItemCustomProcessor_Click(object sender, System.EventArgs e)
		{
			try 
			{
				QueryFilter filter = new SqlExpressionFilter(null); // all rows
				QueryDefinition qd = new QueryDefinition(filter, "*");
				string [] orderby = new string[1];
				orderby[0] = "State Desc";
				qd.OrderBy = orderby;
				SearchResultProcessor srp = new MySearchResultProcessor(10); // stop after 10 rows
				SearchInfo si = new SearchInfo(srp, qd);

				IResultSetFeatureCollection fc = _catalog.Search("usa", si);
				// set map view to show search results
				_map.SetView(fc.Envelope);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void menuItemSetColumns_Click(object sender, System.EventArgs e)
		{
			try 
			{
				// build up a search info by hand (not using the factory)
				QueryFilter filter = new SqlExpressionFilter("POP_90 < 1000000");
				QueryDefinition qd = new QueryDefinition(filter, "MI_Key");
				
				// to Add Columns
				qd.AppendColumns("State", "MI_Geometry");

				// to set all new set of columns
				// not using MI_Geometry here
				string[] cols = new string[] {"MI_Key", "MI_Style", "State_Name", "POP_90", "Households_90"};
				qd.Columns = cols;

				// Note: if you are doing a multi table search, the columns must apply to each table
				// alternatively, you can derive a new class from QueryDefinition and
				// override the GetColumns() method to return different columns for each table being searched
				
				SearchInfo si = new SearchInfo(null, qd);

				IResultSetFeatureCollection fc = _catalog.Search("mexico", si);
				// set map view to show search results
				_map.SetView(_map.Layers["mexico"] as FeatureLayer);

				// show results as selection
				SelectFeatureCollection(fc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}
}