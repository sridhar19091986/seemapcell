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
using MapInfo.Styles;



namespace RasterInfoSample {
	/// <summary>
	/// Summary description for MapForm1.
	/// </summary>
	public class MapForm1 : System.Windows.Forms.Form {
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.ComponentModel.Container components = null;
		private MapInfo.Mapping.Map _map = null;
		private Table _rasterTable = null;

		public MapForm1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			// Set table search path to value sampledatasearch registry key
			// if not found, then just use the app's current directory
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			string s = (string)key.GetValue("SampleDataSearchPath");
			if (s != null && s.Length > 0) 	{
				if (s.EndsWith("\\")==false) {
					s += "\\";
				}
			}	else 	{
				s = Environment.CurrentDirectory;
			}
			key.Close();
	
			Session.Current.TableSearchPath.Path = s;
		
			// load some layers, set csys and default view
			_map = mapControl1.Map;
			try {
				_map.Load(new MapTableLoader( "florida.tab"));
			}	catch(Exception ex)	{
				MessageBox.Show(this, "Unable to load tables. Sample Data may not be installed.\r\n" +  ex.Message);
				this.Close();
				return;
			}

			SetUpRasterLayer(); 
		}

		private void SetUpRasterLayer() {	
			FeatureLayer myRasterLayer = _map.Layers["florida"] as FeatureLayer;
			_rasterTable = myRasterLayer.Table;

				
			RasterStyle rs = new RasterStyle(); 
			rs.Contrast = 33; 
			rs.Grayscale = true; 

			// this composite style will affect the raster as intended 
			CompositeStyle csRaster = new CompositeStyle(rs); 
			FeatureOverrideStyleModifier fosm = 
				new FeatureOverrideStyleModifier("Style Mod", csRaster); 

			FeatureStyleModifiers modifiers = myRasterLayer.Modifiers; 
			modifiers.Append(fosm);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
		private void InitializeComponent() {
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
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
			this.mapControl1.Tools.LeftButtonTool = "ZoomIn";
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
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(24, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Get RasterInfo";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(128, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "Get RasterStyle";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// MapForm1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 306);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
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
		static void Main() {
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

		private void button1_Click(object sender, System.EventArgs e) {
			DoShowRasterInfo(_rasterTable);
		}

		// Gets RasterInfo from a given table.
		// Each raster table has exactly one associated raster image, and thus only one record.
		// Reading that record with a data reader, we can get 
		// a FeatureGeometry - a bounding rectangle,
		// a Style - the Raster style,
		// a key 
		// a RasterInfo object.
		private MapInfo.Raster.RasterInfo GetRasterInfo(Table table) {
			MapInfo.Raster.RasterInfo rasterInfo = null;
			MIDataReader rdr = null;
			string projectionlist = "obj, MI_Key, MI_Raster, MI_Style";
			rdr = table.ExecuteReader(projectionlist);
			
			string name;
			string typename;
			int n = rdr.FieldCount;
			MapInfo.Styles.Style style;
			MapInfo.Geometry.FeatureGeometry featureGeometry;
			MapInfo.Data.Key key;
			if(rdr.Read()) {
				for(int i =0; i < rdr.FieldCount; i++) {
					name = rdr.GetName(i);
					typename = rdr.GetDataTypeName(i);
					if(typename == "MapInfo.Styles.Style") {
						style = rdr.GetStyle(i); 
					}
					else if(typename == "MapInfo.Geometry.FeatureGeometry") {
						featureGeometry = rdr.GetFeatureGeometry(i);
					}
					else if(typename == "MapInfo.Data.Key") {
						key = rdr.GetKey(i);
					}
					else if (typename == "MapInfo.Raster.RasterInfo") {
						rasterInfo = rdr.GetRasterInfo(i);
					}
				}
			}
			rdr.Close();
			rdr.Dispose();
			rdr = null;
			return rasterInfo;
		}

		private MapInfo.Styles.RasterStyle GetRasterStyle(Table table) {
			MapInfo.Styles.RasterStyle rasterStyle = null;
			MIDataReader rdr = null;
			string projectionlist = "MI_Style";
			rdr = table.ExecuteReader(projectionlist);
			
			string name;
			string typename;
			int n = rdr.FieldCount;
			MapInfo.Styles.Style style = null;
			if(rdr.Read()) {
				for(int i =0; i < rdr.FieldCount; i++) {
					name = rdr.GetName(i);
					typename = rdr.GetDataTypeName(i);
					if(typename == "MapInfo.Styles.Style") {
						style = rdr.GetStyle(i); 
					}
				}
			}
			rdr.Close();
			rdr.Dispose();
			rdr = null;
			if (style != null) {
				rasterStyle = style as RasterStyle;
			}
			return rasterStyle;
		}

		private void DoShowRasterInfo(MapInfo.Data.Table table) {	
			// Get Raster info from the table.
			MapInfo.Raster.RasterInfo rasterInfo = GetRasterInfo(table);
			MessageBox.Show(this, String.Format("imageHeight = {0}\n imageWidth = {1}\n", 
				rasterInfo.Height,
				rasterInfo.Width));
		}

		private void DoShowRasterStyle(MapInfo.Data.Table table) {	
			// Get Raster info from the table.
			MapInfo.Styles.RasterStyle rasterStyle = GetRasterStyle(table);
			MessageBox.Show(this, String.Format("Brightness = {0}\n Contrast = {1}\n", 
				rasterStyle.Brightness,
				rasterStyle.Contrast));
		}

		private void button2_Click(object sender, System.EventArgs e) {
			DoShowRasterStyle(_rasterTable);
		}
	}
}
