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
using MapInfo.Styles; 
using MapInfo.Windows.Dialogs;

namespace IndividualValueThematic
{
	/// <summary>
	/// This sample app demonstrates how to add IndividualValueThemes to 
	/// a layer, how to examine the properties of an IndividualValueTheme,
	/// and how to remove IndividualValueThemes programmatically.
	/// </summary>
	public class IndividualValueThematic : System.Windows.Forms.Form
	{
		// By default this demo adds themes to the World layer. 
		// TODO: If you want to add themes to a different layer, 
		// change the _layerName variable, and then change the two
		// _expression variables so that they represent valid expressions
		// for the layer you specified.  
		private string _layerName = "World"; 
		private string _expression1 = "Continent"; 
		private string _expression2 = "Country"; 

		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.StatusBar statusBar1;
		private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep1;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
		private System.Windows.Forms.ToolBarButton toolBarButtonSep2;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddContinentTheme;
		private System.Windows.Forms.ToolBarButton toolBarButtonAddCountryTheme;
		private System.Windows.Forms.ToolBarButton toolBarButtonThemeInfo;
		private System.Windows.Forms.ToolBarButton toolBarButtonClear;
		private System.ComponentModel.Container components = null;


		public IndividualValueThematic()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to some map events
			mapControl1.Map.ViewChangedEvent += new ViewChangedEventHandler(Map_ViewChanged);

			// If the map is blank -- which it is, unless you have modified
			// it in design mode -- load a sample geoset containing the World table
			if (mapControl1.Map == null || mapControl1.Map.Layers.Count == 0) 
			{
				SetupMap(); 
			}

		}

		// Initialize the map using the sample World geoset. 
		private void SetupMap()
		{
			// Set table search path to value sampledatasearch registry key
			// if not found, then just use the app's current directory
			Microsoft.Win32.RegistryKey key = 
				Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
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
			string geoSetName = "world.gst"; 
			try 
			{
				mapControl1.Map.Load(new MapGeosetLoader(s + geoSetName));			
			}
			catch(Exception) 
			{
				MessageBox.Show("Geoset " + geoSetName + " not found."); 
			}	
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
			this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSep1 = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSep2 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddContinentTheme = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonAddCountryTheme = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonThemeInfo = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonClear = new System.Windows.Forms.ToolBarButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mapControl1
			// 
			this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapControl1.Location = new System.Drawing.Point(0, 0);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(420, 252);
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
			this.panel1.Location = new System.Drawing.Point(4, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(424, 256);
			this.panel1.TabIndex = 1;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 287);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(432, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// mapToolBar1
			// 
			this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.mapToolBarButtonLayerControl,
																						   this.toolBarButtonSep1,
																						   this.mapToolBarButtonSelect,
																						   this.mapToolBarButtonZoomIn,
																						   this.mapToolBarButtonZoomOut,
																						   this.mapToolBarButtonPan,
																						   this.toolBarButtonSep2,
																						   this.toolBarButtonAddContinentTheme,
																						   this.toolBarButtonAddCountryTheme,
																						   this.toolBarButtonThemeInfo,
																						   this.toolBarButtonClear});
			this.mapToolBar1.Divider = false;
			this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None;
			this.mapToolBar1.DropDownArrows = true;
			this.mapToolBar1.Location = new System.Drawing.Point(0, 0);
			this.mapToolBar1.MapControl = this.mapControl1;
			this.mapToolBar1.Name = "mapToolBar1";
			this.mapToolBar1.ShowToolTips = true;
			this.mapToolBar1.Size = new System.Drawing.Size(280, 26);
			this.mapToolBar1.TabIndex = 3;
			this.mapToolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.mapToolBar1_ButtonClick);
			// 
			// mapToolBarButtonLayerControl
			// 
			this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
			this.mapToolBarButtonLayerControl.ToolTipText = "Layer Control";
			// 
			// toolBarButtonSep1
			// 
			this.toolBarButtonSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
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
			// toolBarButtonSep2
			// 
			this.toolBarButtonSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonAddContinentTheme
			// 
			this.toolBarButtonAddContinentTheme.ImageIndex = 34;
			this.toolBarButtonAddContinentTheme.ToolTipText = "Add \'Continent\' Individual Value Theme";
			// 
			// toolBarButtonAddCountryTheme
			// 
			this.toolBarButtonAddCountryTheme.ImageIndex = 34;
			this.toolBarButtonAddCountryTheme.ToolTipText = "Add \'Country\' Individual Value Theme";
			// 
			// toolBarButtonThemeInfo
			// 
			this.toolBarButtonThemeInfo.ImageIndex = 8;
			this.toolBarButtonThemeInfo.ToolTipText = "Show Information about Theme";
			// 
			// toolBarButtonClear
			// 
			this.toolBarButtonClear.ImageIndex = 42;
			this.toolBarButtonClear.ToolTipText = "Clear all IndividualValueThemes";
			// 
			// IndividualValueThematic
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 306);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.panel1);
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "IndividualValueThematic";
			this.Text = "Individual Value Theme demo";
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
			Application.Run(new IndividualValueThematic());
		}

		// Add an IndividualValueTheme to the specified layer.
		private void DoAddTheme(Map myMap, string layerToTheme, string expression) 
		{
			// First obtain a reference to the layer that will own the theme
			FeatureLayer lyr = (FeatureLayer)myMap.Layers[layerToTheme]; 
			if (lyr == null) 
			{
				MessageBox.Show("Layer " + layerToTheme + " not found; theme cannot be added."); 
				return;
			}

			IndividualValueTheme ivTheme = null; 
			try 
			{
				// Create the theme.  Pass null for the theme's alias. 
				ivTheme = new IndividualValueTheme(lyr, expression, null); 
			}
			catch (System.NullReferenceException) 
			{
				// This exception can occur if you specify an expression 
				// that is not valid for this layer.
				MessageBox.Show("Unable to create theme; check expression syntax. " 
					+ "Layer: " + layerToTheme + ", Expression: " + expression, 
					"Error"); 
				return;
			}

			// Set the theme's ApplyStylePart property to control which display
			// attributes are overridden by the theme. If you specify StylePart.All: 
			//     ivTheme.ApplyStylePart = StylePart.All; 
			// the theme will control each country's fill pattern and color. 
			// If you specify StylePart.Color, the theme will control each country's
			// color, but will not alter each region's fill pattern.  
			// If you have specified a fill pattern using a display style override,
			// and you do not want the individual value theme to interfere with that
			// fill pattern, set ApplyStylePart to StylePart.Color.  
			ivTheme.ApplyStylePart = StylePart.Color; 

			// Now add the theme to the layer's collection of style modifiers.
			// Insert it at the top of the list of modifiers, so that it will
			// definitely be visible.  (If the layer has other modifiers, 
			// such as style overrides, and you append the theme to the 
			// bottom of the list, the style overrides might override the theme.)
			lyr.Modifiers.Insert(0, ivTheme); 
			MessageBox.Show("Added new IndividualValueTheme to layer " + _layerName 
				+ " using expression: " + expression,  "Theme Added"); 
		}

		// Examine the layer's IndividualValueTheme, if there is one,
		// and display information about some of the theme's properties.
		private void DoThemeInfo() 
		{	
			// First obtain a reference to the layer
			FeatureLayer lyr = (FeatureLayer)mapControl1.Map.Layers[_layerName]; 
			if (lyr == null) 
			{
				MessageBox.Show(
					"Layer " + _layerName + " not found; no theme information to display."); 
				return;
			}	

			// Reference the layer's collection of themes and other modifiers
			FeatureStyleModifiers modifiers = lyr.Modifiers; 

			// See if the collection contains any IndividualValueTheme objects
			IndividualValueTheme ivTheme = null; 
			int counter = 0; 
			foreach (FeatureStyleModifier mod in modifiers) 
			{
				if (mod is IndividualValueTheme) 
				{
					// We did find an IndividualValueTheme in the collection
					counter++; 
					if (ivTheme == null) 
					{
						// Keep a reference to the top-most theme we find
						ivTheme = mod as IndividualValueTheme; 
					}
				}
			}
			if (counter > 1) 
			{
				MessageBox.Show("Found " + counter + " IndividualValueThemes on layer " 
					+ _layerName + "; displaying information for topmost theme."); 
			}
			if (ivTheme != null) 
			{
				string str = "Individual Value Theme properties: "; 
				str += "Theme name: '" + ivTheme.Name + "'. "; 
				str += "Theme expression: '" + ivTheme.Expression + "'. "; 
				str += "Theme has " + ivTheme.Bins.Count + " bins. "; 
				if (ivTheme.Visible) 
				{
					str += " Theme is visible at the current zoom level. "; 
				}
				else 
				{
					str += " Theme is not currently visible. "; 
				}
				MessageBox.Show(str); 
			}
			else 
			{
				MessageBox.Show("No IndividualValueTheme objects found for layer " + _layerName); 
			}
		}

		// Remove all IndividualValueThemes from the World layer
		private void DoClearThemes() 
		{	
			// First obtain a reference to the layer
			FeatureLayer lyr = (FeatureLayer)mapControl1.Map.Layers[_layerName]; 
			if (lyr == null) 
			{
				MessageBox.Show(
					"Layer " + _layerName + " not found; no themes to remove."); 
				return;
			}	

			// Reference the layer's collection of themes and other modifiers
			FeatureStyleModifiers modifiers = lyr.Modifiers; 

			// remove any IndividualValueTheme objects
			ArrayList aliases = new ArrayList();  
			foreach (FeatureStyleModifier mod in modifiers) 
			{
				IndividualValueTheme ivTheme = mod as IndividualValueTheme; 
				if (ivTheme != null) 
				{
					// We found a modifier that is an IndividualValueTheme. 
					// Remember its name so we can remove it after this loop.
					aliases.Add(ivTheme.Alias); 
				}
			}
			foreach (string s in aliases) 
			{
				modifiers.Remove(s); 
			}
			MessageBox.Show("Removed " + aliases.Count 
				+ " IndividualValueTheme(s) from layer: " + _layerName); 
		}
		
		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) {
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

		private void MapForm1_Load(object sender, System.EventArgs e)
		{
			MessageBox.Show("Click the toolbar buttons to add, remove, or " 
				+ "display information about individual value themes.", 
				"About this demo...");
		}

		// This method handles the toolbar's button click event
		//  (only the custom PushButtons need extra code -- Layer Control
		//  and Tool clicks are automatically handled by the MapToolBar control).
		private void mapToolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (object.ReferenceEquals(e.Button, toolBarButtonAddContinentTheme)) 
			{
				DoAddTheme(mapControl1.Map, _layerName, _expression1);
			}
			if (object.ReferenceEquals(e.Button, toolBarButtonAddCountryTheme)) 
			{
				DoAddTheme(mapControl1.Map, _layerName, _expression2);
			}
			else if (object.ReferenceEquals(e.Button, toolBarButtonThemeInfo)) 
			{
				DoThemeInfo();
			}
			else if (object.ReferenceEquals(e.Button, toolBarButtonClear)) 
			{
				DoClearThemes();
			}
		}	
	}
}
