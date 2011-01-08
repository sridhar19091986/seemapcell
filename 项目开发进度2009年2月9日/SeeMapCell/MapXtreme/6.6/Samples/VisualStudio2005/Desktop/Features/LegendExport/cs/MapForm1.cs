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
using MapInfo.Windows.Dialogs;

namespace LegendExport
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		internal MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		internal System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private System.ComponentModel.Container components = null;

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

			// Listen to the appropriate map event to allow the status bar to be updated
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

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

			// Create a legend
			Legend legend = map.Legends.CreateLegend(new Size(5, 5));
			// Create a LegendFrame that contains the theme and add that frame to the Legend.
			ThemeLegendFrame frame = LegendFrameFactory.CreateThemeLegendFrame("Area", "Area", thm);
			legend.Frames.Append(frame);
			frame.Title = "Area (sq. mi.)";

			// Create a LegendExport and export the legend to a bitmap file.
			MapInfo.Mapping.LegendExport legendExport = new MapInfo.Mapping.LegendExport(map, legend);
			legendExport.Format = ExportFormat.Bmp;
			legendExport.ExportSize = new ExportSize(300, 200);
			legendExport.Export("legend.bmp");

			// Display the legend in a window.
			System.Windows.Forms.Form legendForm = new LegendForm();
			legendForm.BackgroundImage = System.Drawing.Image.FromFile("legend.bmp");
			legendForm.Size = new Size(300, 200);
			legendForm.Show();

			// Alternatively, you can add the legend as a child window of the map
			//  by appending it to the Adornments collection.  In this case, most likely
			//  a smaller size should be used when the Legend object is created.
			//
			//legend.Border = true;
			//map.Adornments.Append(legend);
			
			// Set the initial legend location to be the upper left corner of the map control.
			//legend.Location = new System.Drawing.Point(mapControl1.Left, mapControl1.Top);

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
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(596, 340);
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
			this.panel1.Size = new System.Drawing.Size(600, 344);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 386);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(608, 19);
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
			this.mapToolBar1.Location = new System.Drawing.Point(6, 8);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(160, 26);
			this.mapToolBar1.TabIndex = 8;
			// 
			// mapToolBarButtonOpenTable
			// 
			this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
			this.mapToolBarButtonOpenTable.ToolTipText = "Open Table";
			// 
			// mapToolBarButtonLayerControl
			// 
			this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
			this.mapToolBarButtonLayerControl.ToolTipText = "Layer Control";
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
			this.ClientSize = new System.Drawing.Size(608, 405);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "MapForm1";
			this.Text = "Legend Export Sample";
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
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}
	}
}
