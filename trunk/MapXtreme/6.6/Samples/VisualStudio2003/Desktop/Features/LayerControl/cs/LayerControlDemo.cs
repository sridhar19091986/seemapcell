using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MapInfo.Engine; 
using MapInfo.Geometry; 
using MapInfo.Mapping; 
using MapInfo.Windows.Controls; 
using MapInfo.Windows.Dialogs; 

namespace MapInfo.Samples.LayerControl
{
	/// <summary>
	/// A sample application that demonstrates various ways of 
	/// customizing the MapInfo.Windows.Controls.LayerControl class. 
	/// The form contains a docked LayerControl and a MapControl, 
	/// as well as various checkboxes and buttons that modify the 
	/// appearance and/or behavior of the LayerControl. 
	/// </summary>
	public class LayerControlDemoForm : System.Windows.Forms.Form
	{
		private MapInfo.Windows.Controls.LayerControl layerControl1;
		private MapInfo.Windows.Controls.MapControl mapControl1;
		private System.Windows.Forms.CheckBox checkBoxShowAddButton;
		private System.Windows.Forms.CheckBox checkBoxToolTips;
		private System.Windows.Forms.CheckBox checkBoxShowMapNode;
		private System.Windows.Forms.CheckBox checkBoxConfirmationPrompt;
		private System.Windows.Forms.Button buttonLabelLayers;
		private System.Windows.Forms.Button buttonDisableRemovals;
		private System.Windows.Forms.Button buttonDisableMapViewTab;
		private System.Windows.Forms.Button buttonRemoveMapViewTab;
		private System.Windows.Forms.Button buttonAddMenuItem;
		private System.Windows.Forms.Button buttonCustomStyleTab;
		private MapInfo.Windows.Controls.CheckBox checkBoxShowContextMenu;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		// Constructor that creates the sample app form. 
		public LayerControlDemoForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// For this application, we'll reset Resize Method, because
			// the form is resizable, and the app is more responsive if
			// it doesn't redraw the app repeatedly as the user resizes. 
			mapControl1.Map.ResizeMethod = ResizeMethod.PreserveScale; 

			// Load sample data if the map is blank
			if (mapControl1.Map == null || mapControl1.Map.Layers.Count == 0) 
			{
				SetupMap(); 
			}

			// Now assign the map, which will populate the nodes of
			// the layer tree.  
			layerControl1.Map = mapControl1.Map;
			layerControl1.Tools = mapControl1.Tools; 
		}

		// Initialize the map using the sample World geoset
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

		// Method that places restrictions on what types of objects  
		// the user is allowed to remove from the layer tree.  
		// This example shows how to prevent the user from removing 
		// FeatureLayer, GroupLayer, and LabelLayer nodes; the same 
		// technique could be used to prohibit any or all types of 
		// layers and themes from being removed by the user.  
		private void RestrictLayerRemoval() 
		{
			// Declare a LayerNodeHelper object, which has properties 
			// that dictate the appearance and behavior of the specified
			// type of layer.  
			ILayerNodeHelper helper; 

			// Get the LayerNodeHelper that is currently being used to 
			// dictate how FeatureLayer nodes behave in LayerControl.  
			helper = layerControl1.GetLayerTypeHelper(typeof(FeatureLayer)); 

			// Reconfigure the helper to not allow the removal of FeatureLayers
			helper.AllowRemoval = false; 

			// Now use the same technique to reconfigure other types of layers
			helper = layerControl1.GetLayerTypeHelper(typeof(GroupLayer)); 
			helper.AllowRemoval = false; 

			helper = layerControl1.GetLayerTypeHelper(typeof(LabelLayer)); 
			helper.AllowRemoval = false; 

			// Apply the same technique to the Map node.  If you right-click
			// the Map node, there is no Remove menuitem.  However, the Map
			// node's context menu does have a "clear all" menuitem.  In the 
			// case of the Map node, the helper's AllowRemoval property 
			// controls whether the clear all menuitem is shown. 
			helper = layerControl1.GetLayerTypeHelper(typeof(Map)); 
			helper.AllowRemoval = false; 			
		}

		// Show or hide the Add button on the LayerControl's ToolBar. 
		// Also show or hide the separator that separates the Add button
		// from the Remove button. 
		private void ShowAddButton(bool bShow) 
		{
			ToolBar tb = layerControl1.ToolBar; 
			ToolBarButton spacer = tb.Buttons[1]; 
			ToolBarButton addButton = tb.Buttons[0]; 
			spacer.Visible = addButton.Visible = bShow; 
		}

		// Customize the appearance and behavior of all LabelLayer nodes
		private void CustomizeLabelLayerNodes() 
		{
			// Obtain the helper object that is being used to dictate the
			// behavior of LabelLayer nodes in the layer tree.
			ILayerNodeHelper labelLayerHelper = 
				layerControl1.GetLayerTypeHelper(typeof(LabelLayer)); 

			// Reconfigure the helper to specify that it should NOT display 
			// any "child" nodes.  By default, each LabelLayer does show a  
			// child node for each LabelSource in the layer.  Setting 
			// ShowChildren to false will cause LayerControl to not display
			// any nodes for LabelSource objects.  This simplifies the layer
			// tree, and prevents the user from seeing properties of 
			// individual LabelSources (which might or might not be 
			// appropriate for your application). 
			labelLayerHelper.ShowChildren = false; 

			// Change the ToolTip text used for LabelLayer nodes
			labelLayerHelper.ToolTipText = "Labels for various map layers"; 

			// Change which image is displayed for LabelLayer nodes.  
			// In this example we will set all LabelLayer nodes to display
			// with the icon that is ordinarly used for LabelSource nodes.
			ILayerNodeHelper labelSourceHelper = 
				layerControl1.GetLayerTypeHelper(typeof(LabelSource)); 

			labelLayerHelper.Image = labelSourceHelper.Image; 
		}

		// Disable all options on the Map node's View tab, so that the user can
		// still see the Map's zoom width, etc., but the user cannot type in 
		// new values. 
		private void DisableMapViewTab() 
		{
			// Call a method on LayerControl class that returns the
			// collection of PropertiesUserControl objects currently
			// associated with the Map class.  This collection of 
			// PropertiesUserControl objects is what populates the 
			// TabControl whenever the Map node is selected. 
			IList controlList = 
				layerControl1.GetLayerTypeControls(typeof(MapInfo.Mapping.Map));

			// Determine which of the controls in the collection is 
			// the View control.  
			foreach (object obj in controlList) 
			{
				// Try to cast to a MapViewControl class, which might not work
				MapViewControl mvc = obj as MapViewControl;  
				if (mvc != null) 
				{
					// The current control is the one shown on the View tab.
					// Disable everything on the tab. 
					mvc.Enabled = false; 

					// The MapViewControl has an Apply button; there is no
					// point showing a button that is disabled.  
					// So hide the button:  
					mvc.ShowApplyButton = false; 

					break;
				}
			}
		}

		// Remove the Map node's View tab, so that when the user selects 
		// the Map node, the TabControl will not contain a View tab.  
		// The same technique could be used to remove any or all of the tabs
		// associated with any type of node in the layers tree. 
		private void RemoveMapViewTab() 
		{
			// Call a method on LayerControl class that returns the
			// collection of PropertiesUserControl objects currently
			// associated with the Map class. 
			IList controlList = 
				layerControl1.GetLayerTypeControls(typeof(MapInfo.Mapping.Map));

			// Determine which of the controls in the collection is 
			// the View control.  
			foreach (object obj in controlList) 
			{
				MapViewControl mvc = obj as MapViewControl;  
				if (mvc != null) 
				{
					// The current control is the one shown on the View tab.
					// Remove it from the collection. 
					controlList.Remove(obj);
					break;
				}
			}
		}

		// Create a custom "Style" tab which will appear on the TabControl
		// whenever the user selects the Map node at the root of the tree. 
		private void AddCustomMapStyleTab() 
		{
			// Instantiate a custom control, MapBackgroundControl, the 
			// source code for which is provided as part of this project.  
			MapBackgroundControl bgControl = new MapBackgroundControl(); 

			// Call a method on LayerControl class that returns the
			// collection of PropertiesUserControl objects currently
			// associated with the Map class. 
			IList controlList = 
				layerControl1.GetLayerTypeControls(typeof(MapInfo.Mapping.Map));

			// Add the custom control to the collection.  The next time the
			// user selects the Map node at the root of the layer tree, 
			// the TabControl will contain an extra, non-standard "Style" tab
			// that displays a MapBackgroundControl object (which allows the
			// user to choose a background color for the map).
			controlList.Add(bgControl); 
		}

		// Create a custom menuitem, "Choose Coordinate System", which is available 
		// when the user right-clicks the Map node.  Choosing this menuitem
		// displays the Choose Coordinate System dialog.  
		private void AddChooseCoordSysMenuitem() 
		{
			// Create a new menuitem, which calls the MenuItemChooseCoordSys
			// method (see below). 
			MenuItem chooseCoordSysMenuItem = new MenuItem("&Choose Coordinate System...", 
				new System.EventHandler(this.MenuItemChooseCoordSys)); 

			// Each type of object that can appear in the layer tree 
			// has a collection of menuitems displayed when the user
			// right-clicks.  Obtain a reference to that collection, 
			// and add our new menuitem to the collection. 
			IList menuItems = 
				layerControl1.GetLayerTypeMenuItems(typeof(MapInfo.Mapping.Map)); 

			// Insert a separator and a new menuitem to the collection of menuitems.  
			menuItems.Add(new MenuItem("-")); 
			menuItems.Add(chooseCoordSysMenuItem); 
		}

		// The method called when the user chooses the Choose Coordinate System menuitem
		private void MenuItemChooseCoordSys(Object sender, System.EventArgs e) 
		{
			if (mapControl1.Map.IsDisplayCoordSysReadOnly) 
			{
				// We cannot allow the user to change the coordinate system if 
				// the coordinate system is locked due to a raster layer. 
				MessageBox.Show("Coordinate system is currently restricted, due to a raster layer."); 
			}
			else 
			{
				CoordSysPickerDlg csysDlg = new CoordSysPickerDlg();
				csysDlg.SelectedCoordSys = mapControl1.Map.GetDisplayCoordSys();
				if (csysDlg.ShowDialog(this) == DialogResult.OK) 
				{
					CoordSys csysNew = csysDlg.SelectedCoordSys;
					if (csysNew != mapControl1.Map.GetDisplayCoordSys() ) 
					{
						mapControl1.Map.SetDisplayCoordSys(csysNew); 
					}
				}
			}
		}

		/// <summary>
		/// Standard Windows forms method.  Clean up any resources being used.
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
			this.layerControl1 = new MapInfo.Windows.Controls.LayerControl();
			this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
			this.checkBoxShowAddButton = new System.Windows.Forms.CheckBox();
			this.checkBoxToolTips = new System.Windows.Forms.CheckBox();
			this.checkBoxShowMapNode = new System.Windows.Forms.CheckBox();
			this.checkBoxConfirmationPrompt = new System.Windows.Forms.CheckBox();
			this.buttonLabelLayers = new System.Windows.Forms.Button();
			this.buttonDisableRemovals = new System.Windows.Forms.Button();
			this.buttonDisableMapViewTab = new System.Windows.Forms.Button();
			this.buttonRemoveMapViewTab = new System.Windows.Forms.Button();
			this.buttonAddMenuItem = new System.Windows.Forms.Button();
			this.buttonCustomStyleTab = new System.Windows.Forms.Button();
			this.checkBoxShowContextMenu = new MapInfo.Windows.Controls.CheckBox();
			this.SuspendLayout();
			// 
			// layerControl1
			// 
			this.layerControl1.AllowRenaming = true;
			this.layerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.layerControl1.ConfirmLayerRemoval = true;
			this.layerControl1.EditNameAfterInsertingLayer = true;
			this.layerControl1.Location = new System.Drawing.Point(8, 8);
			this.layerControl1.Map = null;
			this.layerControl1.Name = "layerControl1";
			this.layerControl1.ShowContextMenu = true;
			this.layerControl1.ShowMapNode = true;
			//this.layerControl1.ShowPredominantGeometryType = true;
			this.layerControl1.Size = new System.Drawing.Size(285, 440);
			this.layerControl1.TabIndex = 0;
			this.layerControl1.UpdateWhenCollectionChanges = true;
			this.layerControl1.UpdateWhenMapViewChanges = true;
			// 
			// mapControl1
			// 
			this.mapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mapControl1.Location = new System.Drawing.Point(301, 8);
			this.mapControl1.Name = "mapControl1";
			this.mapControl1.Size = new System.Drawing.Size(347, 208);
			this.mapControl1.TabIndex = 1;
			this.mapControl1.Text = "mapControl1";
			this.mapControl1.Tools.LeftButtonTool = "Arrow";
			this.mapControl1.Tools.MiddleButtonTool = "Pan";
			// 
			// checkBoxShowAddButton
			// 
			this.checkBoxShowAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxShowAddButton.Checked = true;
			this.checkBoxShowAddButton.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxShowAddButton.Location = new System.Drawing.Point(304, 224);
			this.checkBoxShowAddButton.Name = "checkBoxShowAddButton";
			this.checkBoxShowAddButton.Size = new System.Drawing.Size(336, 24);
			this.checkBoxShowAddButton.TabIndex = 5;
			this.checkBoxShowAddButton.Text = "Show the Add button on the Layer Control toolbar";
			this.checkBoxShowAddButton.CheckedChanged += new System.EventHandler(this.checkBoxShowAddButton_CheckedChanged);
			// 
			// checkBoxToolTips
			// 
			this.checkBoxToolTips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxToolTips.Checked = true;
			this.checkBoxToolTips.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxToolTips.Location = new System.Drawing.Point(304, 248);
			this.checkBoxToolTips.Name = "checkBoxToolTips";
			this.checkBoxToolTips.Size = new System.Drawing.Size(336, 24);
			this.checkBoxToolTips.TabIndex = 10;
			this.checkBoxToolTips.Text = "Show ToolTips over the layers tree";
			this.checkBoxToolTips.CheckedChanged += new System.EventHandler(this.checkBoxToolTips_CheckedChanged);
			// 
			// checkBoxShowMapNode
			// 
			this.checkBoxShowMapNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxShowMapNode.Checked = true;
			this.checkBoxShowMapNode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxShowMapNode.Location = new System.Drawing.Point(304, 272);
			this.checkBoxShowMapNode.Name = "checkBoxShowMapNode";
			this.checkBoxShowMapNode.Size = new System.Drawing.Size(336, 24);
			this.checkBoxShowMapNode.TabIndex = 15;
			this.checkBoxShowMapNode.Text = "Show a Map node at the root of the layers tree";
			this.checkBoxShowMapNode.CheckedChanged += new System.EventHandler(this.checkBoxShowMapNode_CheckedChanged);
			// 
			// checkBoxConfirmationPrompt
			// 
			this.checkBoxConfirmationPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxConfirmationPrompt.Checked = true;
			this.checkBoxConfirmationPrompt.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxConfirmationPrompt.Location = new System.Drawing.Point(304, 296);
			this.checkBoxConfirmationPrompt.Name = "checkBoxConfirmationPrompt";
			this.checkBoxConfirmationPrompt.Size = new System.Drawing.Size(336, 24);
			this.checkBoxConfirmationPrompt.TabIndex = 20;
			this.checkBoxConfirmationPrompt.Text = "Display \"Do you want to remove...\" confirmation prompts";
			this.checkBoxConfirmationPrompt.CheckedChanged += new System.EventHandler(this.checkBoxConfirmationPrompt_CheckedChanged);
			// 
			// buttonLabelLayers
			// 
			this.buttonLabelLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonLabelLayers.Location = new System.Drawing.Point(299, 389);
			this.buttonLabelLayers.Name = "buttonLabelLayers";
			this.buttonLabelLayers.Size = new System.Drawing.Size(168, 23);
			this.buttonLabelLayers.TabIndex = 60;
			this.buttonLabelLayers.Text = "Simplify LabelLayer nodes...";
			this.buttonLabelLayers.Click += new System.EventHandler(this.buttonLabelLayers_Click);
			// 
			// buttonDisableRemovals
			// 
			this.buttonDisableRemovals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDisableRemovals.Location = new System.Drawing.Point(299, 359);
			this.buttonDisableRemovals.Name = "buttonDisableRemovals";
			this.buttonDisableRemovals.Size = new System.Drawing.Size(168, 23);
			this.buttonDisableRemovals.TabIndex = 40;
			this.buttonDisableRemovals.Text = "Disable layer removal...";
			this.buttonDisableRemovals.Click += new System.EventHandler(this.buttonDisableRemovals_Click);
			// 
			// buttonDisableMapViewTab
			// 
			this.buttonDisableMapViewTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDisableMapViewTab.Location = new System.Drawing.Point(472, 359);
			this.buttonDisableMapViewTab.Name = "buttonDisableMapViewTab";
			this.buttonDisableMapViewTab.Size = new System.Drawing.Size(176, 23);
			this.buttonDisableMapViewTab.TabIndex = 50;
			this.buttonDisableMapViewTab.Text = "Disable the Map\'s View tab...";
			this.buttonDisableMapViewTab.Click += new System.EventHandler(this.buttonDisableMapViewTab_Click);
			// 
			// buttonRemoveMapViewTab
			// 
			this.buttonRemoveMapViewTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonRemoveMapViewTab.Location = new System.Drawing.Point(472, 389);
			this.buttonRemoveMapViewTab.Name = "buttonRemoveMapViewTab";
			this.buttonRemoveMapViewTab.Size = new System.Drawing.Size(176, 23);
			this.buttonRemoveMapViewTab.TabIndex = 70;
			this.buttonRemoveMapViewTab.Text = "Remove the Map\'s View tab...";
			this.buttonRemoveMapViewTab.Click += new System.EventHandler(this.buttonRemoveMapViewTab_Click);
			// 
			// buttonAddMenuItem
			// 
			this.buttonAddMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonAddMenuItem.Location = new System.Drawing.Point(472, 419);
			this.buttonAddMenuItem.Name = "buttonAddMenuItem";
			this.buttonAddMenuItem.Size = new System.Drawing.Size(177, 23);
			this.buttonAddMenuItem.TabIndex = 90;
			this.buttonAddMenuItem.Text = "Add custom menu items...";
			this.buttonAddMenuItem.Click += new System.EventHandler(this.buttonAddMenuItem_Click);
			// 
			// buttonCustomStyleTab
			// 
			this.buttonCustomStyleTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonCustomStyleTab.Location = new System.Drawing.Point(299, 419);
			this.buttonCustomStyleTab.Name = "buttonCustomStyleTab";
			this.buttonCustomStyleTab.Size = new System.Drawing.Size(168, 23);
			this.buttonCustomStyleTab.TabIndex = 80;
			this.buttonCustomStyleTab.Text = "Add a custom Map Style tab...";
			this.buttonCustomStyleTab.Click += new System.EventHandler(this.buttonCustomStyleTab_Click);
			// 
			// checkBoxShowContextMenu
			// 
			this.checkBoxShowContextMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxShowContextMenu.Checked = true;
			this.checkBoxShowContextMenu.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxShowContextMenu.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxShowContextMenu.Location = new System.Drawing.Point(305, 319);
			this.checkBoxShowContextMenu.Name = "checkBoxShowContextMenu";
			this.checkBoxShowContextMenu.Size = new System.Drawing.Size(335, 24);
			this.checkBoxShowContextMenu.TabIndex = 25;
			this.checkBoxShowContextMenu.Text = "Show context menu when user right-clicks in layer tree";
			this.checkBoxShowContextMenu.CheckedChanged += new System.EventHandler(this.checkBoxContextMenuChanged);
			// 
			// LayerControlDemoForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(656, 454);
			this.Controls.Add(this.checkBoxShowContextMenu);
			this.Controls.Add(this.buttonCustomStyleTab);
			this.Controls.Add(this.buttonAddMenuItem);
			this.Controls.Add(this.buttonRemoveMapViewTab);
			this.Controls.Add(this.buttonDisableMapViewTab);
			this.Controls.Add(this.buttonDisableRemovals);
			this.Controls.Add(this.buttonLabelLayers);
			this.Controls.Add(this.checkBoxConfirmationPrompt);
			this.Controls.Add(this.checkBoxShowMapNode);
			this.Controls.Add(this.checkBoxToolTips);
			this.Controls.Add(this.checkBoxShowAddButton);
			this.Controls.Add(this.mapControl1);
			this.Controls.Add(this.layerControl1);
			this.MinimumSize = new System.Drawing.Size(660, 470);
			this.Name = "LayerControlDemoForm";
			this.Text = "Layer Control Demo";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new LayerControlDemoForm());
		}

		// The method called when the user clicks the Disable Layer Removal button
		private void buttonDisableRemovals_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button prevents the user from removing FeatureLayer, "
				+ "GroupLayer, or LabelLayer nodes.  It does not prevent the user " 
				+ "from removing theme nodes or LabelSource nodes."; 

			if (ConfirmCustomization(prompt)) 
			{
				RestrictLayerRemoval();
				RefreshLayerTree(); 
				buttonDisableRemovals.Enabled = false; 
			}
		}

		// The method called when the user checks or clears the 
		// "Show The Add Button" checkbox
		private void checkBoxShowAddButton_CheckedChanged(object sender, System.EventArgs e)
		{
			// Show or hide the Layer Control's Add button, 
			// based on whether the checkbox is currently checked. 
			ShowAddButton(checkBoxShowAddButton.Checked); 
		}

		// The method called when the user checks or clears the 
		// Show ToolTips checkbox. 
		private void checkBoxToolTips_CheckedChanged(object sender, System.EventArgs e)
		{
			layerControl1.ToolTip.Active = checkBoxToolTips.Checked; 
		}

		// The method called when the user checks or clears the 
		// Show Map Node checkbox. 
		private void checkBoxShowMapNode_CheckedChanged(object sender, System.EventArgs e)
		{
			layerControl1.ShowMapNode = checkBoxShowMapNode.Checked; 
			RefreshLayerTree(); 
		}

		// The method called when the user checks or clears 
		// the Show Context Menu checkbox
		private void checkBoxContextMenuChanged(object sender, System.EventArgs e)
		{
			layerControl1.ShowContextMenu = checkBoxShowContextMenu.Checked; 
		}

		// The method called when the user checks or clears the 
		// Suppress All Confirmation Prompts checkbox. 
		private void checkBoxConfirmationPrompt_CheckedChanged(object sender, System.EventArgs e)
		{
			// Set the LayerControl object's ConfirmLayerRemoval property, 
			// which can be used to globally suppress all confirmation 
			// prompts that are displayed when the user removes a node
			// from the layer tree.  
			layerControl1.ConfirmLayerRemoval = checkBoxConfirmationPrompt.Checked;

			// NOTE: You can configure LayerControl so that it displays a 
			// confirmation prompt for some types of layers (e.g. 
			// "Do you want to remove this layer?") but does not display 
			// a confirmation prompt for other types of layers (e.g. 
			// you might not want to display a confirmation prompt when the
			// user removes a Style Override node).  To control confirmation
			// prompts separately for different types of layers, obtain 
			// the appropriate ILayerNodeHelper object and set its properties.
		}

		// The method called when the user clicks Simplify LabelLayer Nodes.
		private void buttonLabelLayers_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button hides all LabelSource nodes, and " 
				+ "changes the icon that appears next to LabelLayer nodes."; 
			if (ConfirmCustomization(prompt)) 
			{
				CustomizeLabelLayerNodes();
				RefreshLayerTree(); 
				buttonLabelLayers.Enabled = false; 
			}
		}

		// The method called when the user clicks Disable the Map's View Tab. 
		private void buttonDisableMapViewTab_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button disables the options on the View tab, "
				+ "which appears when the user selects the Map node."; 
			if (ConfirmCustomization(prompt)) 
			{
				DisableMapViewTab(); 
				buttonDisableMapViewTab.Enabled = false; 
			}
		}

		// The method called when the user clicks Remove the Map's View Tab. 
		private void buttonRemoveMapViewTab_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button removes the View tab, so that the TabControl "
				+ "does not contain a View tab when the user chooses the Map node."; 
			if (ConfirmCustomization(prompt)) 
			{
				RemoveMapViewTab(); 
				RefreshLayerTree(); 
				buttonRemoveMapViewTab.Enabled = false; 
				// Since we have removed the View tab, we must now disable 
				// the button on the demo form that allows you to make 
				// other changes to the View tab
				buttonDisableMapViewTab.Enabled = false; 
			}
		}

		// The method called when the user clicks Add Custom Menuitems.  
		private void buttonAddMenuItem_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button adds custom menu items to right-click menus: " 
				+ "a 'Choose Coordinate System' menu item (on the Map node's menu); "
				+ "a 'Find Labels' menu item (on each FeatureLayer node's menu); and "
				+ "a 'Find Layers' menu item (on each LabelSource node's menu)."; 
			if (ConfirmCustomization(prompt)) 
			{
				// Add the custom item to the Map node's menu 
				AddChooseCoordSysMenuitem();

				// Add custom items to the FeatureLayer and LabelSource menus
				LayerControlEnhancer lce = new LayerControlEnhancer(); 
				lce.LayerControl = layerControl1; 
				lce.AddLayerToLabelEnhancement(); 

				buttonAddMenuItem.Enabled = false; 
			}
		}

		// The method called when the user clicks Add Custom Map Style Tab. 
		private void buttonCustomStyleTab_Click(object sender, System.EventArgs e)
		{
			string prompt = "This button adds a custom 'Style' tab that allows the " 
				+ "user to select the Map node, then choose a map background color."; 
			if (ConfirmCustomization(prompt)) 
			{
				AddCustomMapStyleTab(); 
				RefreshLayerTree(); 
				buttonCustomStyleTab.Enabled = false; 
			}
		}

		// A method that re-assigns the LayerControl's Map property, which
		// causes the LayerControl to regenerate the hierarchy of nodes 
		// displayed in the layer tree.  (Most applications do not need to
		// re-assign the Map property in this manner, because most 
		// applications set LayerControl properties once, at the beginning;
		// but this application lets you set LayerControl properties on the
		// fly, which can create situations where the layer tree needs to
		// be recreated.) 
		private void RefreshLayerTree() {
			layerControl1.Map = mapControl1.Map; 
		}

		// A method that displays a confirmation dialog and determines whether
		// the user chooses OK or Cancel. 
		private bool ConfirmCustomization(string prompt) 
		{
			DialogResult result = DialogResult.Yes; 
			string caption = "Confirm Layer Control Customization"; 
			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			result = MessageBox.Show(this, prompt + " Do you want to proceed?", 
				caption, buttons, MessageBoxIcon.Question, 
				MessageBoxDefaultButton.Button1);

			if(result == DialogResult.Yes)
			{
				return true;
			}
			return false; 
		}

	}
}
