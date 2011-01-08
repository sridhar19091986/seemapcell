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
using MapInfo.Styles;
using MapInfo.Raster;
using MapInfo.Geometry;

namespace GridForm
{
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Button GridButton;
		private System.Windows.Forms.Button StyleButton;
		private System.ComponentModel.Container components = null;
		
		// gridInfo sample
		private Table _miTable =null;
		private GridStyle _mStyle = null;
		private FeatureLayer _lyr = null;
		private Key _mKey = null;
		private FeatureGeometry _mObject = null;
		private System.Windows.Forms.Button ReadTableColumnB;
		internal MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		internal System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private GridInfo _mGridInfo = null;

		public MapForm1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to appropriate map event to update status bar
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);
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
			this.GridButton = new System.Windows.Forms.Button();
			this.StyleButton = new System.Windows.Forms.Button();
			this.ReadTableColumnB = new System.Windows.Forms.Button();
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
			this.mapControl1.Size = new System.Drawing.Size(652, 369);
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
			this.panel1.Size = new System.Drawing.Size(656, 373);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 411);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(664, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// GridButton
			// 
			this.GridButton.Location = new System.Drawing.Point(336, 8);
			this.GridButton.Name = "GridButton";
			this.GridButton.TabIndex = 4;
			this.GridButton.Text = "2. GridInfo";
			this.GridButton.Click += new System.EventHandler(this.GridButton_Click);
			// 
			// StyleButton
			// 
			this.StyleButton.Location = new System.Drawing.Point(432, 8);
			this.StyleButton.Name = "StyleButton";
			this.StyleButton.TabIndex = 5;
			this.StyleButton.Text = "3. Style";
			this.StyleButton.Click += new System.EventHandler(this.StyleButton_Click);
			// 
			// ReadTableColumnB
			// 
			this.ReadTableColumnB.Location = new System.Drawing.Point(216, 8);
			this.ReadTableColumnB.Name = "ReadTableColumnB";
			this.ReadTableColumnB.Size = new System.Drawing.Size(104, 23);
			this.ReadTableColumnB.TabIndex = 6;
			this.ReadTableColumnB.Text = "1. ReadTable";
			this.ReadTableColumnB.Click += new System.EventHandler(this.ReadTableColumnB_Click);
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
			this.mapToolBar1.Location = new System.Drawing.Point(6, 7);
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
			this.ClientSize = new System.Drawing.Size(664, 430);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.ReadTableColumnB);
			this.Controls.Add(this.StyleButton);
			this.Controls.Add(this.GridButton);
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

		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

		private void InitParameters()
		{
			_lyr = null;
			_mStyle = null;
			_mKey = null;
			_mObject = null;
			_mGridInfo = null;
		}

		public void SelectAllIndivColumns()
		{
			MIDataReader rdr = null;
			string projectionlist = "obj, MI_Key, MI_Grid, MI_Style";
			rdr = _miTable.ExecuteReader(projectionlist);
			
			string name;
			string typename;
			int n = rdr.FieldCount;
			if(rdr.Read())
			{
				n = rdr.FieldCount;//shouldn't change
				for(int i =0; i <n; i++)
				{
					name = rdr.GetName(i);
					typename = rdr.GetDataTypeName(i);
					if(typename == "MapInfo.Styles.Style")
					{
						_mStyle = rdr.GetStyle(i) as GridStyle; 
					}
					else if(typename == "MapInfo.Geometry.FeatureGeometry")
					{
						_mObject = rdr.GetFeatureGeometry(i);
					}
					else if(typename == "MapInfo.Data.Key")
					{
						_mKey = rdr.GetKey(i);
					}
					else if (typename == "MapInfo.Raster.GridInfo")
					{
						_mGridInfo = rdr.GetGridInfo(i);
					}
				}
			}

			rdr.Close();
			rdr.Dispose();
			rdr = null;
		}

		private void GridButton_Click(object sender, System.EventArgs e)
		{
			if (_mGridInfo == null)
			{
				MessageBox.Show("Please click ReadTable before clicking GridInfo.");
				return;
			}

			string b = null;
			b += " Height=" + _mGridInfo.Height.ToString() + " Width=" + _mGridInfo.Width.ToString() + "\n";
			b += " MaxValue=" + _mGridInfo.MaxValue.ToString() + " MinValue=" + _mGridInfo.MinValue.ToString()+ "\n";
			b += " HasHillshade=" + _mGridInfo.HasHillshade.ToString() + "\n";
			b += " MBR=(" + _mGridInfo.MBR.x1.ToString() + "," + 
											_mGridInfo.MBR.y1.ToString() + ")\n(" +
											_mGridInfo.MBR.x2.ToString() + "," +
											_mGridInfo.MBR.y2.ToString() + ")\n";;
			for (int i=0; i<_mGridInfo.RasterControlPoints.Length; ++i)
			{
				b += " RasterControlPoints[" + i.ToString() + "]=(" + 
					_mGridInfo.RasterControlPoints[i].x.ToString() + "," +
					_mGridInfo.RasterControlPoints[i].y.ToString() + ")\n";
			}
			for (int i=0;i<_mGridInfo.RealWorldControlPoints.Length;++i)
			{
				b += " RealWorldControlPoints[" + i.ToString() + "]=(" + 
					_mGridInfo.RealWorldControlPoints[i].x.ToString() + "," + 
					_mGridInfo.RealWorldControlPoints[i].y.ToString() + ")\n";
			}

			MessageBox.Show(b);
		}

		private void StyleButton_Click(object sender, System.EventArgs e)
		{
			if (_mStyle == null)
			{
				MessageBox.Show("Please click GridInfo before clicking Style.");
				return;
			}

			GridStyle aStyle = _mStyle.Clone() as GridStyle;

			GridStyleForm f = new GridStyleForm();
			f.AlphaTextBox.Text = aStyle.Alpha.ToString();
			f.BrightnessBox.Text = aStyle.Brightness.ToString();
			f.ContrastBox.Text = aStyle.Contrast.ToString();
			f.GrayScale.CheckState = (aStyle.Grayscale) ? CheckState.Checked : CheckState.Unchecked;
			f.Transparency.CheckState = (aStyle.Transparent) ? CheckState.Checked : CheckState.Unchecked;
			f.DisplayHillshade.CheckState = (aStyle.DisplayHillshade) ? CheckState.Checked : CheckState.Unchecked;
			f.TransparentColor.BackColor = aStyle.TransparentColor;
			if (f.ShowDialog() == DialogResult.OK)
			{
				aStyle.Alpha = int.Parse(f.AlphaTextBox.Text);
				aStyle.Brightness = int.Parse(f.BrightnessBox.Text);
				aStyle.Contrast = int.Parse(f.ContrastBox.Text);
				aStyle.Grayscale = f.GrayScale.CheckState == CheckState.Checked ? true : false;
				aStyle.Transparent = (f.Transparency.CheckState == CheckState.Checked) ? true : false;
				aStyle.DisplayHillshade = (f.DisplayHillshade.CheckState == CheckState.Checked) ? true : false;
				aStyle.TransparentColor = f.TransparentColor.BackColor;

				// this composite style will affect the raster as intended 
				CompositeStyle csRaster = new CompositeStyle(aStyle); 
				FeatureOverrideStyleModifier fosm = 
					new FeatureOverrideStyleModifier("Style Mod", csRaster); 

				FeatureStyleModifiers modifiers = _lyr.Modifiers; 
				modifiers.Clear();
				modifiers.Append(fosm);			
			}
		}

		private void ReadTableColumnB_Click(object sender, System.EventArgs e)
		{
			InitParameters();
			if (mapControl1.Map.Layers.Count == 0)
			{
				MessageBox.Show("Please open a Grid table before clicking the ReadTable button.");
				return;
			}

			_lyr = mapControl1.Map.Layers[0] as FeatureLayer;
			_miTable = _lyr.Table;
			if (_miTable.TableInfo.TableType != TableType.Grid)
			{
				MessageBox.Show("Please open a Grid table before clicking the ReadTable button.");
				return;
			}

			SelectAllIndivColumns();
			MessageBox.Show("Table read complete.  Please click the GridInfo button.");
		}
	}
}
