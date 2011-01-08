using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MapInfo.Engine;
using MapInfo.Mapping;
using MapInfo.Mapping.Thematics;

namespace GraduatedSymbolThemeSample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Location = new System.Drawing.Point(8, 40);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(320, 192);
			this.mapControl1.TabIndex = 0;
			this.mapControl1.Text = "mapControl1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 272);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(320, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Simple theme";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 296);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(320, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Theme Graduated by Log value";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(8, 368);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(320, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Theme that shows negative values";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(8, 320);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(320, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Theme that sets DataValueAtSize property";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(8, 344);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(320, 23);
			this.button5.TabIndex = 5;
			this.button5.Text = "Theme that uses non-default symbol";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 248);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(312, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Click buttons to display the described theme:";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(8, 8);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(136, 23);
			this.button6.TabIndex = 7;
			this.button6.Text = "Open Mexico.tab...";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 398);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.mapControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		// Sets up data search path
		private void SetupTablePath()
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

		// Trys to load mexico.tab.  If not successful, then
		// the button controls that create themes are disabled.
		private void Setup()
		{
			// Load the layer
			try 
			{
				mapControl1.Map.Load(new MapTableLoader("mexico.tab"));
				this.button6.Visible = false;
			}
			catch(Exception) 
			{
				this.button1.Enabled = false;
				this.button2.Enabled = false;
				this.button3.Enabled = false;
				this.button4.Enabled = false;
				this.button5.Enabled = false;
			}						
		} 

		// Creates a simple theme using the default values
		private void SimpleTheme(Map map) {
			// Get table from the feature layer
			MapInfo.Mapping.FeatureLayer mexicoLayer = map.Layers["mexico"] as MapInfo.Mapping.FeatureLayer;
			MapInfo.Data.Table mexicoTable = mexicoLayer.Table;
			// Create the theme
			GraduatedSymbolTheme theme = new GraduatedSymbolTheme(mexicoTable, "Pop_90");
			// Create the theme layer
			ObjectThemeLayer themeLayer = new ObjectThemeLayer("Population1990", "Population1990", theme);
			// Add theme layer to the map
			map.Layers.Add(themeLayer);
		}

		// Creates a theme whose symbol sizes are graduated logarithmically
		private void GraduateByLogTheme(Map map) {
			// Get table from the feature layer
			MapInfo.Mapping.FeatureLayer mexicoLayer = map.Layers["mexico"] as MapInfo.Mapping.FeatureLayer;
			MapInfo.Data.Table mexicoTable = mexicoLayer.Table;
			// Create the theme
			GraduatedSymbolTheme theme = new GraduatedSymbolTheme(mexicoTable, "Pop_90");
			// Graduate the size logarithmically
			theme.GraduateSizeBy = GraduateSizeBy.Log;
			// Create the theme layer
			ObjectThemeLayer themeLayer = new ObjectThemeLayer("Population1990", "Population1990", theme);
			// Add theme layer to the map
			map.Layers.Add(themeLayer);
		}

		// Creates a theme with a custom DataValueAtSize property
		private void DataValueAtSizeTheme(Map map) {
			// Get table from the feature layer
			MapInfo.Mapping.FeatureLayer mexicoLayer = map.Layers["mexico"] as MapInfo.Mapping.FeatureLayer;
			MapInfo.Data.Table mexicoTable = mexicoLayer.Table;
			// Create the theme
			GraduatedSymbolTheme theme = new GraduatedSymbolTheme(mexicoTable, "Pop_90");
			// Decreasing DataValueAtSize value will increase the size of the symbols.
			theme.DataValueAtSize /= 4;
			// Create the theme layer
			ObjectThemeLayer themeLayer = new ObjectThemeLayer("Population1990", "Population1990", theme);
			// Add theme layer to the map
			map.Layers.Add(themeLayer);
		}

		// Creates a theme with a non-default symbol
		private void CustomPositiveSymbolTheme(Map map) {
			// Get table from the feature layer
			MapInfo.Mapping.FeatureLayer mexicoLayer = map.Layers["mexico"] as MapInfo.Mapping.FeatureLayer;
			MapInfo.Data.Table mexicoTable = mexicoLayer.Table;
			// Create the theme
			GraduatedSymbolTheme theme = new GraduatedSymbolTheme(mexicoTable, "Pop_90");
			// Replace the default symbol of the theme
			theme.PositiveSymbol = new MapInfo.Styles.SimpleVectorPointStyle(35, Color.Blue, 36);
			// Create the theme layer
			ObjectThemeLayer themeLayer = new ObjectThemeLayer("Population1990", "Population1990", theme);
			// Add theme layer to the map
			map.Layers.Add(themeLayer);
		}

		// Creates a theme that shows symbols for negative values.
		// Also uses the NumericNull property to hide the symbol of
		// the greatest theme value.
		private void NegativeValueTheme(Map map) {
			// Get table from the feature layer
			MapInfo.Mapping.FeatureLayer mexicoLayer = map.Layers["mexico"] as MapInfo.Mapping.FeatureLayer;
			MapInfo.Data.Table mexicoTable = mexicoLayer.Table;
			// Add a temporary column to the mexico table,
			// calulating the difference between Pop_90 and Pop_80
			MapInfo.Data.Columns tempColumns = new MapInfo.Data.Columns();
			tempColumns.Add(new MapInfo.Data.Column("CarsMinusTrucks", "(Cars_91-Trucks_91)"));
			mexicoTable.AddColumns(tempColumns);
			// Create the theme
			GraduatedSymbolTheme theme = new GraduatedSymbolTheme(mexicoTable, "CarsMinusTrucks");
			// Show Negative symbols
			theme.ShowNegativeSymbol = true;
			// Setup symbols
			theme.PositiveSymbol = new MapInfo.Styles.SimpleVectorPointStyle(35, Color.Blue, 18);
			theme.NegativeSymbol = new MapInfo.Styles.SimpleVectorPointStyle(35, Color.Red, 36);
			// Get the larget value in the theme column
			MapInfo.Data.MIDataReader reader = mexicoTable.ExecuteReader("MAX(CarsMinusTrucks)");
			reader.MoveNext();
			double maxValue = reader.GetDouble(0);
			reader.Dispose();
			// Setup the numeric null value to exclude the largest value
			theme.NumericNull = maxValue;
			theme.HasNumericNull = true;
			// These end up being small values, so use the DataValueAtSize 
			// property to increase the size of the symbols
			theme.DataValueAtSize = 50000;
			// Create the theme layer
			ObjectThemeLayer themeLayer = new ObjectThemeLayer("CarOrTruck", "CarOrTruck", theme);
			// Add theme layer to the map
			map.Layers.Add(themeLayer);
		}

		// Removes all theme layers from the map.
		private void ClearThemes(Map map) {
			// Create a list of theme layers in the map
			ArrayList themeLayers = new ArrayList();
			foreach (IMapLayer layer in map.Layers) {
				if (layer is ObjectThemeLayer) {
					themeLayers.Add(layer);
				}
			}
			// Remove each theme layer from the map
			foreach (object targetLayer in themeLayers) {
				map.Layers.Remove((IMapLayer) targetLayer);
			}
		}

		// Enables or disables the button controls.
		private void EnableButtons(bool enable) {
			this.button1.Enabled = enable;
			this.button2.Enabled = enable;
			this.button3.Enabled = enable;
			this.button4.Enabled = enable;
			this.button5.Enabled = enable;
		}

		private void button1_Click(object sender, System.EventArgs e) {
			ClearThemes(mapControl1.Map);
			SimpleTheme(mapControl1.Map);
		}

		private void button2_Click(object sender, System.EventArgs e) {
			ClearThemes(mapControl1.Map);
			this.GraduateByLogTheme(mapControl1.Map);
		}

		private void button3_Click(object sender, System.EventArgs e) {
			ClearThemes(mapControl1.Map);
			this.NegativeValueTheme(mapControl1.Map);
		}

		private void button4_Click(object sender, System.EventArgs e) {
			ClearThemes(mapControl1.Map);
			this.DataValueAtSizeTheme(mapControl1.Map);
		}

		private void button5_Click(object sender, System.EventArgs e) {
			ClearThemes(mapControl1.Map);
			this.CustomPositiveSymbolTheme(mapControl1.Map);
		}

		private void button6_Click(object sender, System.EventArgs e) {
			
			OpenFileDialog openFileDlg = new OpenFileDialog();
			openFileDlg.Title = "Open mexico.tab";
			openFileDlg.Filter = "Mexico Table (mexico.tab)|mexico.tab";			
			openFileDlg.FilterIndex = 1;
			if (openFileDlg.ShowDialog() == DialogResult.OK) {
				try {
					mapControl1.Map.Load(new MapTableLoader(openFileDlg.FileName));
					FeatureLayer layer = (FeatureLayer) mapControl1.Map.Layers["mexico"];
					EnableButtons(true);
					this.button6.Visible = false;
				}
				catch {
					EnableButtons(false);
				}
			}
		}

		private void Form1_Load(object sender, System.EventArgs e) {
			SetupTablePath();
			Setup();
		}
	}
}
