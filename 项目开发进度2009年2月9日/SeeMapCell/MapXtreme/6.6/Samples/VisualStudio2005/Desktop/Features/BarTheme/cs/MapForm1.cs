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
using MapInfo.Mapping.Thematics;
using MapInfo.Engine;
using MapInfo.Windows.Dialogs;

namespace BarThemeSample
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.ComponentModel.Container components = null;

		//members used to create theme
		private Table _table;
		private FeatureLayer _layer;
		private System.Windows.Forms.Button btnBarTheme;
		private System.Windows.Forms.Button btnStacked;
		internal MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		internal System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		private ObjectThemeLayer _thmLayer;

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to the right map event to allow the status bar Zoom level to be updated
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			// Load a map
			LoadMap();
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
			this.btnBarTheme = new System.Windows.Forms.Button();
			this.btnStacked = new System.Windows.Forms.Button();
			this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
			this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(394, 245);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.MiddleButtonTool = null;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.mapControl1);
			this.panel1.Location = new System.Drawing.Point(4, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(398, 249);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 287);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(406, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// btnBarTheme
			// 
			this.btnBarTheme.Location = new System.Drawing.Point(200, 8);
			this.btnBarTheme.Name = "btnBarTheme";
			this.btnBarTheme.Size = new System.Drawing.Size(88, 23);
			this.btnBarTheme.TabIndex = 4;
			this.btnBarTheme.Text = "BarTheme";
			this.btnBarTheme.Click += new System.EventHandler(this.btnBarTheme_Click);
			// 
			// btnStacked
			// 
			this.btnStacked.Location = new System.Drawing.Point(312, 8);
			this.btnStacked.Name = "btnStacked";
			this.btnStacked.Size = new System.Drawing.Size(88, 23);
			this.btnStacked.TabIndex = 5;
			this.btnStacked.Text = "Stacked";
			this.btnStacked.Click += new System.EventHandler(this.btnStacked_Click);
			// 
			// mapToolBar1
			// 
			this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.mapToolBarButtonOpenTable,
																						   this.mapToolBarButtonLayerControl,
																						   this.toolBarButtonSeparator,
																						   this.mapToolBarButtonSelect,
																						   this.mapToolBarButtonZoomIn,
																						   this.mapToolBarButtonZoomOut,
																						   this.mapToolBarButtonPan});
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(8, 8);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(160, 26);
			this.mapToolBar1.TabIndex = 7;
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
			// mapToolBarButtonLayerControl
			// 
			this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
			this.mapToolBarButtonLayerControl.ToolTipText = "Layer Control";
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 306);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.btnStacked);
			this.Controls.Add(this.btnBarTheme);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "MapForm1";
			this.Text = "MapForm1";
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
	
		private void LoadMap()
		{
			// Set table search path to value of SampleDataSearchPath registry key
			string path = Environment.CurrentDirectory;
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			if ((key != null) && (key.GetValue("SampleDataSearchPath") != null))
			{
				path = (string)key.GetValue("SampleDataSearchPath");
				if (path.EndsWith("\\")==false) 
				{
					path += "\\";
				}
				key.Close();
			}	
			Session.Current.TableSearchPath.Path = path;

			// Open a table and put it on map
			_table = Session.Current.Catalog.OpenTable("Mexico.TAB");
			_layer = new FeatureLayer(_table); 
			mapControl1.Map.Layers.Add(_layer);
			btnStacked.Enabled=false;
		}

		private void btnBarTheme_Click(object sender, System.EventArgs e)
		{
			ObjectThemeLayer thmLayer=(ObjectThemeLayer) mapControl1.Map.Layers["VehicleTheme"];
			
			// remove any existing theme with the name we want to use:
			if (thmLayer != null) mapControl1.Map.Layers.Remove(thmLayer);

            BarTheme thm = new BarTheme(mapControl1.Map,_table,
				"Cars_91", "Buses_91", "Trucks_91");

			_thmLayer = new ObjectThemeLayer("Vehicles By Type, 1991", "VehicleTheme", thm);
			mapControl1.Map.Layers.Add(_thmLayer);
			btnStacked.Enabled=true;
		}

		private void btnStacked_Click(object sender, System.EventArgs e)
		{
			BarTheme thm = (BarTheme) _thmLayer.Theme;
			thm.Stacked = true;
			thm.GraduatedStacked=true;

			_thmLayer.RebuildTheme();
			// The ObjectThemeLayer will dispose itself when it got removed from Layers collection.
			// so you can not re-add it into Layers collection.
			//mapControl1.Map.Layers.Remove("VehicleTheme");
			//mapControl1.Map.Layers.Add(_thmLayer);
			btnStacked.Enabled=false;
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
	}
}
