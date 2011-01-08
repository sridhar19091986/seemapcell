using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapInfo.Mapping; 

namespace ToolsAppCS
{
	/// <summary>
	/// A specialized StatusBar subclass that displays information
	/// about which layers support selecting, editing, and drawing, 
	/// and allows the user to click to launch a dialog to change
	/// the layer settings. 
	/// </summary>
	public class LayerFilterStatusBar : System.Windows.Forms.StatusBar
	{
		private System.ComponentModel.IContainer components = null;
		private IMapLayerFilter _selectFilter = null; 
		private IMapLayerFilter _editFilter = null; 
		private IMapLayerFilter _insertFilter = null; 
		private LayersBase _layers = null; 
		private System.Windows.Forms.ContextMenu contextMenu = 
			new System.Windows.Forms.ContextMenu();

		private System.Windows.Forms.MenuItem _menuItemSelect = null;
		private System.Windows.Forms.MenuItem _menuItemEdit = null;
		private System.Windows.Forms.MenuItem _menuItemInsert = null;

		System.Windows.Forms.StatusBarPanel _selectPanel = 
			new System.Windows.Forms.StatusBarPanel(); 
		System.Windows.Forms.StatusBarPanel _editPanel = 
			new System.Windows.Forms.StatusBarPanel(); 
		System.Windows.Forms.StatusBarPanel _insertPanel = 
			new System.Windows.Forms.StatusBarPanel();

		public LayerFilterStatusBar()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.ShowPanels = true; 
			_selectPanel.Width = _editPanel.Width = _insertPanel.Width = 150; 
			_selectPanel.Text = "Select from: "; 
			_selectPanel.ToolTipText = _selectPanel.Text; 
			_editPanel.Text = "Edit: "; 
			_insertPanel.Text = "Draw into: "; 

			this.PanelClick += 
				new System.Windows.Forms.StatusBarPanelClickEventHandler(
						this.statusBarPanelClick);

			_menuItemSelect = new System.Windows.Forms.MenuItem(
				"Choose layers to &Select from...", 
				new System.EventHandler(this.MenuItemSelect_Click) ); 

			_menuItemEdit = new System.Windows.Forms.MenuItem(
				"Choose layers to &Edit...", 
				new System.EventHandler(this.MenuItemEdit_Click) ); 

			_menuItemInsert = new System.Windows.Forms.MenuItem(
				"Choose layers to &Draw new features into...", 
				new System.EventHandler(this.MenuItemInsert_Click) ); 
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


		#region Public Properties 

		/// <summary>
		/// Gets or sets the IMapLayerFilter that specifies which 
		/// layers are selectable. 
		/// </summary>
		/// <remarks>
		/// Set this property to a value such as  
		/// mapControl1.Tools.SelectMapToolProperties.SelectableLayerFilter.
		/// </remarks>
		/// <value>IMapLayerFilter object</value>
		public IMapLayerFilter SelectableLayerFilter 
		{
			get 
			{
				return _selectFilter; 
			}

			set 
			{
				_selectFilter = value; 
				if (value == null) 
				{
					// remove panel if it's there
					if ( this.Panels.Contains(_selectPanel) )
					{
						this.Panels.Remove(_selectPanel); 
					}
				}
				else 
				{
					// add panel if we haven't already 
					if (!(this.Panels.Contains(_selectPanel)) ) 
					{
						this.Panels.Add(_selectPanel); 
					}
					UpdateSelectText(); 
				}
			}
		}

		/// <summary>
		/// Gets or sets the IMapLayerFilter that specifies which 
		/// layers are editable. 
		/// </summary>
		/// <remarks>
		/// Set this property to a value such as  
		/// mapControl1.Tools.SelectMapToolProperties.EditableLayerFilter.
		/// </remarks>
		/// <value>IMapLayerFilter object</value>
		public IMapLayerFilter EditableLayerFilter 
		{
			get 
			{
				return _editFilter; 
			}

			set 
			{
				_editFilter = value; 

				if (value == null) 
				{
					// remove panel if it's there
					if ( this.Panels.Contains(_editPanel) )
					{
						this.Panels.Remove(_editPanel); 
					}
				}
				else 
				{
					// add panel if we haven't already 
					if (!(this.Panels.Contains(_editPanel)) ) 
					{
						this.Panels.Add(_editPanel); 
					}
					UpdateEditText(); 
				}

			}
		}

		/// <summary>
		/// Gets or sets the IMapLayerFilter that specifies which 
		/// layers allow the adding (drawing) of new features.  
		/// </summary>
		/// <remarks>
		/// Set this property to a value such as  
		/// mapControl1.Tools.AddMapToolProperties.InsertionLayerFilter.
		/// </remarks>
		/// <value>IMapLayerFilter object</value>
		public IMapLayerFilter InsertionLayerFilter 
		{
			get 
			{
				return _insertFilter; 
			}

			set 
			{
				_insertFilter = value; 
				if (value == null) 
				{
					// remove panel if it's there
					if ( this.Panels.Contains(_insertPanel) )
					{
						this.Panels.Remove(_insertPanel); 
					}
				}
				else 
				{
					// add panel if we haven't already 
					if (!(this.Panels.Contains(_insertPanel)) ) 
					{
						this.Panels.Add(_insertPanel); 
					}
					UpdateInsertText(); 
				}

			}
		}

		/// <summary>
		///  Gets or sets the collection of layers to be displayed. 
		/// </summary>
		/// <remarks>Assign a value such as mapControl1.Map.Layers. 
		/// </remarks>
		/// <value>a LayersBase object</value>
		public LayersBase Layers 
		{
			get 
			{
				return _layers; 
			}

			set 
			{
				_layers = value; 
			}
		}

		/// <summary>
		/// Gets or sets the width of the Selectable StatusBarPanel. 
		/// </summary>
		public int SelectPanelWidth 
		{
			get 
			{
				return _selectPanel.Width; 
			}

			set 
			{
				_selectPanel.Width = value; 
			}
		}


		/// <summary>
		/// Gets or sets the width of the Editable StatusBarPanel. 
		/// </summary>
		public int EditPanelWidth 
		{
			get 
			{
				return _editPanel.Width; 
			}

			set 
			{
				_editPanel.Width = value; 
			}
		}

		/// <summary>
		/// Gets or sets the width of the Editable StatusBarPanel. 
		/// </summary>
		public int InsertionPanelWidth 
		{
			get 
			{
				return _insertPanel.Width; 
			}

			set 
			{
				_insertPanel.Width = value; 
			}
		}

		#endregion

		// The user clicked on the status bar.  
		// If the status bar has just one panel (e.g. Selectable), display
		// the dialog associated with that panel. 
		// If the status bar has multiple panels, display a context menu to let the
		// user choose which dialog to display (Selectable, Editable, Drawable). 
		private void statusBarPanelClick(
			object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
		{
			if (_selectFilter != null && _editFilter == null && _insertFilter == null) 
			{
				MenuItemSelect_Click(null, null); 
			}
			else if (_selectFilter == null && _editFilter != null && _insertFilter == null) 
			{
				MenuItemEdit_Click(null, null); 
			}
			else if (_selectFilter == null && _editFilter == null && _insertFilter != null) 
			{
				MenuItemInsert_Click(null, null); 
			}
			else 
			{
				contextMenu.MenuItems.Clear(); 
				if (_selectFilter != null) 
				{
					contextMenu.MenuItems.Add(_menuItemSelect); 
				}
				if (_editFilter != null) 
				{
					contextMenu.MenuItems.Add(_menuItemEdit);
				}
				if (_insertFilter != null) 
				{
					contextMenu.MenuItems.Add(_menuItemInsert);
				}
				contextMenu.Show(this, new Point(e.X, e.Y)); 
			}
		}

		// an event handler for menuItemSelect to use when connecting its event handler.
		private void MenuItemSelect_Click(Object sender, System.EventArgs e) 
		{
			if (_selectFilter != null) 
			{
				LayerFilterDialog dlg = new LayerFilterDialog(); 
				dlg.Text = "Choose Layers to be Selectable"; 
				dlg.FilterType = FilterType.Selectable; 
				dlg.ListLabel = "&Selectable Layers:"; 
				dlg.SelectableLayerFilter = _selectFilter; 
				dlg.EditableLayerFilter = _editFilter;
				dlg.InsertionLayerFilter = _insertFilter; 
				dlg.Layers = Layers; 
				dlg.Sizable = false; 
				if (dlg.ShowDialog() == DialogResult.OK) 
				{
					UpdateSelectText(); 
					UpdateEditText(); 
					UpdateInsertText(); 
				}
			}
		}

		// an event handler for menuItemSelect to use when connecting its event handler.
		private void MenuItemEdit_Click(Object sender, System.EventArgs e) 
		{
			if (_editFilter != null) 
			{
				LayerFilterDialog dlg = new LayerFilterDialog(); 
				dlg.Text = "Choose Layers to be Editable"; 
				dlg.FilterType = FilterType.Editable; 
				dlg.ListLabel = "&Editable Layers:"; 
				dlg.SelectableLayerFilter = _selectFilter; 
				dlg.EditableLayerFilter = _editFilter; 
				dlg.InsertionLayerFilter = _insertFilter; 
				dlg.Layers = Layers; 
				dlg.Sizable = false; 
				if (dlg.ShowDialog() == DialogResult.OK) 
				{
					UpdateSelectText(); 
					UpdateEditText(); 
					UpdateInsertText(); 
				}
			}
		}

		// an event handler for menuItemSelect to use when connecting its event handler.
		private void MenuItemInsert_Click(Object sender, System.EventArgs e) 
		{
			if (_insertFilter != null) 
			{
				LayerFilterDialog dlg = new LayerFilterDialog(); 
				dlg.Text = "Choose Layers to Draw Into"; 
				dlg.FilterType = FilterType.Insertable; 
				dlg.ListLabel = "&Draw Into Layers:"; 
				dlg.SelectableLayerFilter = _selectFilter; 
				dlg.EditableLayerFilter = _editFilter;
				dlg.InsertionLayerFilter = _insertFilter; 
				dlg.Layers = Layers; 
				dlg.Sizable = false; 
				if (dlg.ShowDialog() == DialogResult.OK) 
				{
					UpdateSelectText(); 
					UpdateEditText(); 
					UpdateInsertText(); 
				}
			}
		}

		public void UpdateSelectText() 
		{
			if (_selectFilter != null) 
			{
				string layerList = String.Empty; 
				layerList = BuildLayerList(layerList, Layers, _selectFilter); 
				
				_selectPanel.Text = "Select: " + layerList;
				_selectPanel.ToolTipText = "Selectable layers: " + layerList; 
			}
		}

		public void UpdateEditText() 
		{
			if (_editFilter != null) 
			{
				string layerList = String.Empty; 
				layerList = BuildLayerList(layerList, Layers, _editFilter); 

				_editPanel.Text = "Edit: " + layerList; 
				_editPanel.ToolTipText = "Editable layers: " + layerList; 
			}
		}

		public void UpdateInsertText() 
		{
			if (_insertFilter != null) 
			{
				string layerList = String.Empty; 
				layerList = BuildLayerList(layerList, Layers, _insertFilter); 
				
				_insertPanel.Text = "Draw: " + layerList; 
				_insertPanel.ToolTipText = "Drawing tools insert into: " + layerList; 
			}
		}

		// Return a comma-separated list of layer names, representing the list of 
		// layers that are active according to the specified filter. 
		// This method calls itself recursively to search within GroupLayers. 
		private string BuildLayerList(string str, LayersBase layers, IMapLayerFilter filter) 
		{
			bool bAllLayers=true;

			foreach (IMapLayer layer in layers) 
			{
				if (layer is GroupLayer) 
				{
					GroupLayer grpLyr = layer as GroupLayer; 
					str = BuildLayerList(str, grpLyr, filter);
					if (str.StartsWith("<All>: ")) str=str.Remove(0,7);
					else bAllLayers=false;
				}
				else if (layer is FeatureLayer) 
				{
					FeatureLayer featLyr = layer as FeatureLayer; 
					if (featLyr != null && featLyr.IsVisible && 
						featLyr.Type != LayerType.Raster &&
						featLyr.Type != LayerType.Wms &&
						featLyr.Type != LayerType.Grid) 
					{
						if (filter.IncludeLayer(featLyr) ) 
						{
							if (str.Length > 0) 
							{
								str += ", "; 
							}
							str += featLyr.Name;
						}
						else bAllLayers=false;
					}
				}
			}

			if (str.Length==0) str = bAllLayers ? "<No layers available>" : "<None>";

			else if (bAllLayers==true)
			{
				if (str.Length > 16) str="<All>";
				else str="<All>: " + str;
			}

			return str; 
		}


		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

