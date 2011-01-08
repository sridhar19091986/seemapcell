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
using MapInfo.Mapping;
using MapInfo.Mapping.Thematics;
using MapInfo.Tools;
using MapInfo.Windows;
using MapInfo.Windows.Dialogs;
using MapInfo.Windows.Controls;

namespace MapInfo.Samples.ThemeDialogs
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.StatusBar statusBar1;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuOpenTable;
		private System.Windows.Forms.MenuItem mnuOpenGeoset;
		private System.Windows.Forms.MenuItem mnuCloseTable;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuTheme;
		private System.Windows.Forms.MenuItem mnuAddTheme;
		private System.Windows.Forms.MenuItem mnuRemoveTheme;
		private System.Windows.Forms.MenuItem mnuModifyTheme;
		private System.Windows.Forms.MenuItem mnuCloseAll;
		private MapInfo.Windows.Controls.MapControl mapControl1;
		

		internal MapInfo.Windows.Controls.MapToolBar mapToolBar1;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
		private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
		internal System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
		internal MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;				

		// the theme created
		private ITheme _thm;		

		//keep track of the layer themed for use when modifiying or removing
		private FeatureLayer _themedFeatureLayer = null;
		private LabelLayer _themedLabelLayer = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Listen to the right map event to allow the status bar Zoom level to be updated
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
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuOpenTable = new System.Windows.Forms.MenuItem();
			this.mnuOpenGeoset = new System.Windows.Forms.MenuItem();
			this.mnuCloseTable = new System.Windows.Forms.MenuItem();
			this.mnuCloseAll = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuTheme = new System.Windows.Forms.MenuItem();
			this.mnuAddTheme = new System.Windows.Forms.MenuItem();
			this.mnuRemoveTheme = new System.Windows.Forms.MenuItem();
			this.mnuModifyTheme = new System.Windows.Forms.MenuItem();
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
			this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
			this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
			this.SuspendLayout();
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 286);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(447, 19);
			this.statusBar1.TabIndex = 2;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFile,
																					  this.mnuTheme});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuOpenTable,
																					this.mnuOpenGeoset,
																					this.mnuCloseTable,
																					this.mnuCloseAll,
																					this.mnuExit});
			this.mnuFile.Text = "File";
			// 
			// mnuOpenTable
			// 
			this.mnuOpenTable.Index = 0;
			this.mnuOpenTable.Text = "Open Table...";
			this.mnuOpenTable.Click += new System.EventHandler(this.mnuOpenTable_Click);
			// 
			// mnuOpenGeoset
			// 
			this.mnuOpenGeoset.Index = 1;
			this.mnuOpenGeoset.Text = "Open Geoset...";
			this.mnuOpenGeoset.Click += new System.EventHandler(this.mnuOpenGeoset_Click);
			// 
			// mnuCloseTable
			// 
			this.mnuCloseTable.Index = 2;
			this.mnuCloseTable.Text = "Close Table...";
			this.mnuCloseTable.Click += new System.EventHandler(this.mnuCloseTable_Click);
			// 
			// mnuCloseAll
			// 
			this.mnuCloseAll.Index = 3;
			this.mnuCloseAll.Text = "Close All";
			this.mnuCloseAll.Click += new System.EventHandler(this.mnuCloseAll_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 4;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuTheme
			// 
			this.mnuTheme.Index = 1;
			this.mnuTheme.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuAddTheme,
																					 this.mnuRemoveTheme,
																					 this.mnuModifyTheme});
			this.mnuTheme.Text = "Theme";
			// 
			// mnuAddTheme
			// 
			this.mnuAddTheme.Index = 0;
			this.mnuAddTheme.Text = "Add Theme...";
			this.mnuAddTheme.Click += new System.EventHandler(this.mnuAddTheme_Click);
			// 
			// mnuRemoveTheme
			// 
			this.mnuRemoveTheme.Enabled = false;
			this.mnuRemoveTheme.Index = 1;
			this.mnuRemoveTheme.Text = "Remove Theme";
			this.mnuRemoveTheme.Click += new System.EventHandler(this.mnuRemoveTheme_Click);
			// 
			// mnuModifyTheme
			// 
			this.mnuModifyTheme.Enabled = false;
			this.mnuModifyTheme.Index = 2;
			this.mnuModifyTheme.Text = "Modify Theme...";
			this.mnuModifyTheme.Click += new System.EventHandler(this.mnuModifyTheme_Click);
			// 
			// mapControl1
			// 
			this.mapControl1.Location = new System.Drawing.Point(8, 32);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(432, 248);
			this.mapControl1.TabIndex = 4;
			this.mapControl1.Text = "mapControl1";
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
			this.mapToolBar1.Location = new System.Drawing.Point(8, 0);
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
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(447, 305);
			this.Controls.Add(this.mapToolBar1);
			this.Controls.Add(this.mapControl1);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(250, 200);
			this.Name = "Form1";
			this.Text = "Theme Dialogs Sample";
			this.Resize += new System.EventHandler(this.Form1_Resize);
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

		// Handler function called when the active map's view changes
		private void Map_ViewChanged(object o, ViewChangedEventArgs e) 
		{
			// Get the map
			MapInfo.Mapping.Map map = (MapInfo.Mapping.Map) o;
			// Display the zoom level
			Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
			statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			SetupTablePath();
			Setup();
		}
		
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
		private void Setup()
		{
			// Load the layer
			try 
			{
				mapControl1.Map.Load(new MapTableLoader("mexico.tab"));
				FeatureLayer layer = mapControl1.Map.Layers["mexico"] as FeatureLayer;				
				LabelLayer labelLayer = new LabelLayer("Label Layer", "Label Layer");
				labelLayer.Sources.Append(new LabelSource(layer.Table));		
				mapControl1.Map.Layers.Add(labelLayer);
			}
			catch(Exception) 
			{
				mnuCloseTable.Enabled = false;
				mnuCloseAll.Enabled = false;
				mnuTheme.Enabled = false;								
			}						
		} 

		private void mnuAddTheme_Click(object sender, System.EventArgs e)
		{			
			try 
			{
				CreateThemeWizard createTheme = new CreateThemeWizard(mapControl1.Map,this);								
				_thm = createTheme.CreateTheme("theme1");
				MapLayer themedLayer = createTheme.SelectedLayer;
				if (themedLayer is FeatureLayer) 
				{
					_themedFeatureLayer = (FeatureLayer)themedLayer;
				}
				else 
				{
					_themedLabelLayer = (LabelLayer)themedLayer;
				}
				if (createTheme.WizardResult == WizardStepResult.Done)
				{		
					// Update the controls
					//mnuAddTheme.Enabled = false;
					mnuRemoveTheme.Enabled = true;
					mnuModifyTheme.Enabled = true;
				}	
			} 
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("Error creating theme: " + ex.Message);
			}
		}

		private void mnuRemoveTheme_Click(object sender, System.EventArgs e)
		{
			// Remove the theme

			RemoveTheme();

			// Update the controls
			//mnuAddTheme.Enabled = true;
			//mnuRemoveTheme.Enabled = false;
			//mnuModifyTheme.Enabled = false;					

			// if this was the last map layer, reset the close menus
			if (mapControl1.Map.Layers.Count == 0)
			{
				mnuCloseTable.Enabled = false;
				mnuCloseAll.Enabled = false;
				mnuTheme.Enabled = false;				
			}
		}

		private void RemoveTheme()
		{
			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			DialogResult result;
			try 
			{				
				if (_themedFeatureLayer != null)
				{
					FeatureStyleModifier modifier = _themedFeatureLayer.Modifiers["theme1"];
					
					if (modifier != null) 
					{
						result = System.Windows.Forms.MessageBox.Show(this, "Removing Theme : " + modifier.Name, "ThemeDialogSampleApp", buttons);
						if(result == DialogResult.Yes)
						{

							_themedFeatureLayer.Modifiers.Remove(modifier);
							mapControl1.Map.Invalidate();
						}
					}
				}
				ObjectThemeLayer thmLayer = mapControl1.Map.Layers["theme1"] as ObjectThemeLayer;
				if (thmLayer != null) 
				{
					result = System.Windows.Forms.MessageBox.Show(this, "Removing Theme : " + thmLayer.Name, "ThemeDialogSampleApp", buttons);
					if(result == DialogResult.Yes)
					{
						mapControl1.Map.Layers.Remove(thmLayer);
						mapControl1.Map.Invalidate();
					}
				}
				if (_themedLabelLayer != null)
				{
					LabelModifier labelModifier = _themedLabelLayer.Sources[0].Modifiers["theme1"];
					if (labelModifier != null) 
					{
						result = System.Windows.Forms.MessageBox.Show(this, "Removing Theme : " + labelModifier.Name, "ThemeDialogSampleApp", buttons);
						if(result == DialogResult.Yes)
						{
							_themedLabelLayer.Sources[0].Modifiers.Remove(labelModifier);
							mapControl1.Map.Invalidate();
						}
					}
				}
			}
			catch(MapException) 
			{
			}
		}

		private void mnuModifyTheme_Click(object sender, System.EventArgs e)
		{
			// Modify the theme
			ModifyTheme();
		}

		private void ModifyTheme()
		{
			try 
			{
				// Bring up the modify theme dialog for modifier themes	
				if (_themedFeatureLayer != null)
				{
					FeatureStyleModifier modifier = _themedFeatureLayer.Modifiers["theme1"];
					if (modifier != null) 
					{
						if (modifier is DotDensityTheme) 
						{
							DotDensityTheme thm = modifier as DotDensityTheme;
							ModifyDotDensityThemeDlg dlg = new ModifyDotDensityThemeDlg(mapControl1.Map, thm);
							dlg.ShowDialog();
						}
						else if (modifier is RangedTheme) 
						{
							RangedTheme thm = modifier as RangedTheme;
							ModifyRangedThemeDlg dlg = new ModifyRangedThemeDlg(mapControl1.Map, thm);
							dlg.ShowDialog();
						}
						else if (modifier is IndividualValueTheme) 
						{
							IndividualValueTheme thm = modifier as IndividualValueTheme;
							ModifyIndValueThemeDlg dlg = new ModifyIndValueThemeDlg(mapControl1.Map, thm);
							dlg.ShowDialog();
						}					
						_themedFeatureLayer.Invalidate();
					}
				}
				// Bring up the modify theme dialog for object themes
				ObjectThemeLayer thmLayer = mapControl1.Map.Layers["theme1"] as ObjectThemeLayer;
				if (thmLayer != null) 
				{
					if (thmLayer.Theme is GraduatedSymbolTheme) 
					{
						GraduatedSymbolTheme thm = thmLayer.Theme as GraduatedSymbolTheme;
						ModifyGradSymbolThemeDlg dlg = new ModifyGradSymbolThemeDlg(mapControl1.Map, thm);
						dlg.ShowDialog();
					}
					else if (thmLayer.Theme is BarTheme) 
					{
						BarTheme thm = thmLayer.Theme as BarTheme;
						ModifyBarThemeDlg dlg = new ModifyBarThemeDlg(mapControl1.Map, thm);
						dlg.ShowDialog();
					}
					else if (thmLayer.Theme is PieTheme) 
					{
						PieTheme thm = thmLayer.Theme as PieTheme;
						ModifyPieThemeDlg dlg = new ModifyPieThemeDlg(mapControl1.Map, thm);
						dlg.ShowDialog();
					}
					thmLayer.RebuildTheme();
				}
				if (_themedLabelLayer != null)
				{
					LabelModifier labelModifier = _themedLabelLayer.Sources[0].Modifiers["theme1"];
					if (labelModifier != null) 
					{
						if (labelModifier is RangedLabelTheme) 
						{
							RangedLabelTheme thm = labelModifier as RangedLabelTheme;
							ModifyRangedThemeDlg dlg = new ModifyRangedThemeDlg(mapControl1.Map, thm);
							dlg.ShowDialog();
						}
						else if (labelModifier is IndividualValueLabelTheme) 
						{
							IndividualValueLabelTheme thm = labelModifier as IndividualValueLabelTheme;
							ModifyIndValueThemeDlg dlg = new ModifyIndValueThemeDlg(mapControl1.Map, thm);
							dlg.ShowDialog();
						}
						_themedLabelLayer.Invalidate();
					}
				}
			}
			catch(MapException) 
			{
			}
		}

		private void mnuOpenGeoset_Click(object sender, System.EventArgs e)
		{
			CloseAll();
			OpenFileDialog openFileDlg = new OpenFileDialog();
			openFileDlg.InitialDirectory = Session.Current.TableSearchPath.Path;						
			openFileDlg.Filter = "Geoset (*.gst)|*.gst";			
			openFileDlg.FilterIndex = 1;

			if (openFileDlg.ShowDialog() == DialogResult.OK)
			{
				mapControl1.Map.Load(new MapGeosetLoader(openFileDlg.FileName));								

				mnuCloseTable.Enabled = true;
				mnuCloseAll.Enabled = true;
				mnuTheme.Enabled = true;
				mnuAddTheme.Enabled = true;
			}
		}
		private void CloseAll()
		{
			mapControl1.Map.Layers.Clear();

		}
		private void mnuOpenTable_Click(object sender, System.EventArgs e)
		{
			CloseAll();
		
			OpenFileDialog openFileDlg = new OpenFileDialog();
			openFileDlg.InitialDirectory = Session.Current.TableSearchPath.Path;			
			openFileDlg.Filter = "MapInfo Table (*.tab)|*.tab";			
			openFileDlg.FilterIndex = 1;

			if (openFileDlg.ShowDialog() == DialogResult.OK)
			{
				mapControl1.Map.Load(new MapTableLoader(openFileDlg.FileName));								

				mnuCloseTable.Enabled = true;
				mnuCloseAll.Enabled = true;
				mnuTheme.Enabled = true;
				mnuAddTheme.Enabled = true;
			}
		}

		private void mnuCloseTable_Click(object sender, System.EventArgs e)
		{
						 
			Catalog catalog = Session.Current.Catalog;			
			ITableEnumerator tables = catalog.EnumerateTables();
			ArrayList tablesList = new ArrayList();
			while (tables.MoveNext())
			{
				tablesList.Add(tables.Current);
			}
			CloseTableDlg closeTableDlg = new CloseTableDlg();
			closeTableDlg.Items.AddRange(tablesList);
			closeTableDlg.SelectedIndex = 0;
			if (closeTableDlg.ShowDialog() == DialogResult.OK)
			{
				ArrayList selectedTables = closeTableDlg.SelectedItems;
				IEnumerator tableToClose = selectedTables.GetEnumerator();
				while (tableToClose.MoveNext())
				{					
					catalog.CloseTable(((MapInfo.Data.Table)tableToClose.Current).Alias);					
				}

				// if all the tables were closed, reset the menus
				if (tablesList.Count == selectedTables.Count)
				{

					// remove the layers from the map so that theme layers get removed
					RemoveAllMapLayers();

					mnuCloseTable.Enabled = false;
					mnuCloseAll.Enabled = false;
					mnuTheme.Enabled = false;		
					mnuAddTheme.Enabled = false;
					mnuRemoveTheme.Enabled = false;
					mnuModifyTheme.Enabled = false;
				}
			}
		}
		
		// Removes all layers from the map
		private void RemoveAllMapLayers()
		{			
			MapLayerEnumerator enumer =  mapControl1.Map.Layers.GetMapLayerEnumerator();
			while (enumer.MoveNext())
			{
				mapControl1.Map.Layers.Remove(enumer.Current);
			}
		}

		private void mnuCloseAll_Click(object sender, System.EventArgs e)
		{		
			// remove the layers from the map so that theme layers get removed
			RemoveAllMapLayers();

			Session.Current.Catalog.CloseAll();						

			mnuCloseTable.Enabled = false;
			mnuCloseAll.Enabled = false;
			mnuTheme.Enabled = false;						
			mnuAddTheme.Enabled = false;
			mnuRemoveTheme.Enabled = false;
			mnuModifyTheme.Enabled = false;
		}		

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void Form1_Resize(object sender, System.EventArgs e)
		{
			mapControl1.Height = this.Height;
			mapControl1.Width = this.Width ;
		}
	
	}
}


