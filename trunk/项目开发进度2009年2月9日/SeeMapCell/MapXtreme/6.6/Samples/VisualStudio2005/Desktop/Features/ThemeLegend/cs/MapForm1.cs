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
using MapInfo.Mapping.Legends;
using MapInfo.Engine;
using MapInfo.Styles;
using MapInfo.Windows.Dialogs;

namespace ThemeLegend
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
		internal MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		internal System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private Legend legend = null;

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set table search path to value sampledatasearch registry key
			string s = Environment.CurrentDirectory;
			Microsoft.Win32.RegistryKey keySamp = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			if ((keySamp != null) && (keySamp.GetValue("SampleDataSearchPath") != null))
			{
				s = (string)keySamp.GetValue("SampleDataSearchPath");
				if (s.EndsWith("\\")==false)
				{
					s += "\\";
				}
				keySamp.Close();
			}	
			Session.Current.TableSearchPath.Path = s;

			// Add the USA table to the map
			mapControl1.Map.Load(new MapTableLoader("usa.tab"));

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);
			mapControl1.Resize += new EventHandler(mapControl1_Resize);

			// Create a ranged theme on the USA layer.
			Map map = mapControl1.Map;
			FeatureLayer lyr = map.Layers["usa"] as MapInfo.Mapping.FeatureLayer;
			RangedTheme thm = new MapInfo.Mapping.Thematics.RangedTheme(
				lyr,
				"Round(MI_Area(Obj, 'sq mi', 'Spherical'), 1)",
				"Area (square miles)",
				5,
				MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange);
			lyr.Modifiers.Append(thm);

			// Change the default fill colors from Red->Gray to White->Blue
			AreaStyle ars;
			// Get the style from our first bin
			CompositeStyle cs = thm.Bins[0].Style;
			// Get the region -- Area -- style
			ars = cs.AreaStyle;
			// Change the fill color
			ars.Interior = StockStyles.WhiteFillStyle();
			// Update the CompositeStyle with the new region color
			cs.AreaStyle = ars;
			// Update the bin with the new CompositeStyle settings
			thm.Bins[0].Style = cs;

			// Change the style settings on the last bin
			int nLastBin = thm.Bins.Count - 1;
			cs = thm.Bins[nLastBin].Style;
			ars = cs.AreaStyle;
			ars.Interior = StockStyles.BlueFillStyle();
			thm.Bins[nLastBin].Style = cs;

			// Tell the theme how to fill in the other bins
			thm.SpreadBy = SpreadByPart.Color;
			thm.ColorSpreadBy = ColorSpreadMethod.Rgb;
			thm.RecomputeStyles();

			// Create a legend
			legend = map.Legends.CreateLegend(new Size(5, 5));
			legend.Border = true;
			ThemeLegendFrame frame = LegendFrameFactory.CreateThemeLegendFrame("Area", "Area", thm);
			legend.Frames.Append(frame);
			frame.Title = "Area (sq. mi.)";
			map.Adornments.Append(legend);
			// Set the initial legend location to be the lower right corner of the map control.
			System.Drawing.Point pt = new System.Drawing.Point(0, 0);
			pt.X = mapControl1.Size.Width - legend.Size.Width;
			pt.Y = mapControl1.Size.Height - legend.Size.Height;
			legend.Location = pt;
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
            this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
            this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
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
            this.mapControl1.IgnoreLostFocusEvent = false;
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(594, 340);
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
            this.panel1.Location = new System.Drawing.Point(5, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(598, 344);
            this.panel1.TabIndex = 1;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 385);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(608, 20);
            this.statusBar1.TabIndex = 2;
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
            this.mapToolBar1.Location = new System.Drawing.Point(7, 9);
            this.mapToolBar1.MapControl = this.mapControl1;
            this.mapToolBar1.Name = "mapToolBar1";
            this.mapToolBar1.ShowToolTips = true;
            this.mapToolBar1.Size = new System.Drawing.Size(192, 26);
            this.mapToolBar1.TabIndex = 8;
            // 
            // mapToolBarButtonOpenTable
            // 
            this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
            this.mapToolBarButtonOpenTable.Name = "mapToolBarButtonOpenTable";
            this.mapToolBarButtonOpenTable.ToolTipText = "Open Table";
            // 
            // mapToolBarButtonLayerControl
            // 
            this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
            this.mapToolBarButtonLayerControl.Name = "mapToolBarButtonLayerControl";
            this.mapToolBarButtonLayerControl.ToolTipText = "Layer Control";
            // 
            // toolBarButtonSeparator
            // 
            this.toolBarButtonSeparator.Name = "toolBarButtonSeparator";
            this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // mapToolBarButtonSelect
            // 
            this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
            this.mapToolBarButtonSelect.Name = "mapToolBarButtonSelect";
            this.mapToolBarButtonSelect.ToolTipText = "Select";
            // 
            // mapToolBarButtonZoomIn
            // 
            this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
            this.mapToolBarButtonZoomIn.Name = "mapToolBarButtonZoomIn";
            this.mapToolBarButtonZoomIn.ToolTipText = "Zoom-in";
            // 
            // mapToolBarButtonZoomOut
            // 
            this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
            this.mapToolBarButtonZoomOut.Name = "mapToolBarButtonZoomOut";
            this.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out";
            // 
            // mapToolBarButtonPan
            // 
            this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
            this.mapToolBarButtonPan.Name = "mapToolBarButtonPan";
            this.mapToolBarButtonPan.ToolTipText = "Pan";
            // 
            // MapForm1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(608, 405);
            this.Controls.Add(this.mapToolBar1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(300, 215);
            this.Name = "MapForm1";
            this.Text = "MapForm1";
            this.Load += new System.EventHandler(this.MapForm1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

		private void mapControl1_Resize(object sender, System.EventArgs e)
		{
			Control control = (Control)sender;

			// Move the Legend to the lower right corner...
			System.Drawing.Point pt = new System.Drawing.Point(0, 0);
			pt.X = control.Size.Width - legend.Size.Width;
			pt.Y = control.Size.Height - legend.Size.Height;
			legend.Location = pt;
		}

		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

        private void MapForm1_Load(object sender, EventArgs e)
        {

        }
	}
}
