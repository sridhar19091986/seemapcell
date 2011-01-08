using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MapInfo.Data; 
using MapInfo.Mapping; 
using MapInfo.Tools; 

namespace ToolsAppCS
{

	/// <summary>
	/// Filter type enumeration, indicating what type of filter 
	/// will be displayed in the dialog. 
	/// </summary>
	public enum FilterType
	{
		/// <summary>
		/// Selectable - the dialog will let the user choose selectable layers
		/// </summary>
		Selectable = 1, 
		/// <summary>
		/// Editable - the dialog will let the user choose editable layers
		/// </summary>
		Editable = 2, 
		/// <summary>
		/// Selectable - the dialog will let the user choose which layers
		/// can have features inserted by using the drawing tools
		/// </summary>
		Insertable = 3
	}

	/// <summary>
	/// A dialog box that displays a list of layers in a ListBox.  
	/// The user can select which layers to apply; for example,
	/// you can use this dialog to let the user choose which layers
	/// should be Editable.  To populate the list, assign the 
	/// Layers and Filter properties. 
	/// </summary>
	/// <remarks>The list only shows currently visible layers, since
	/// it does not make sense to make a layer selectable or editable
	/// when the layer is not currently visible. 
	/// </remarks>
	public class LayerFilterDialog : MapInfo.Windows.Dialogs.ListDlg
	{
		private System.ComponentModel.IContainer components = null;
		private IMapLayerFilter _filter = null; 
		private IMapLayerFilter _selectFilter = null; 
		private IMapLayerFilter _editFilter = null; 
		private IMapLayerFilter _insertFilter = null; 
		private LayersBase _layers = null; 
		private FilterType _filterType = FilterType.Selectable; 

		public LayerFilterDialog()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// When the user clicks OK, call method that updates filter. 
			this.buttonAccept.Click += new System.EventHandler(this.buttonOK_Click);
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
			}
		}

		/// <summary>
		/// Gets or sets the type of filter displayed in this dialog. 
		/// </summary>
		/// <remarks>
		/// Assign a value such as FilterType.Selectable
		/// </remarks>
		/// <value>
		/// a FilterType value
		/// </value>
		public FilterType FilterType
		{
			get
			{
				return _filterType; 
			}
			set
			{
				_filterType = value; 
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

		#endregion

		// Update the filter when the user clicks OK. 
		// Note that we are not setting DialogResult here; 
		// that's done in the base class. 
		private void buttonOK_Click(object sender, System.EventArgs e) 
		{
			if (_filter != null && _filter is ToolFilter && 
				listBoxList.Items.Count > 0) 
			{
				// The list is not empty, and the filter is a type of filter
				// that we can modify; proceed.
				ToolFilter toolFilter = 
					_filter as ToolFilter; 

				// Try to cast the various filters to ToolFilter objects,
				// so that we can call the ToolFilter.SetExplicitInclude method
				ToolFilter selectToolFilter = null; 
				if (_selectFilter != null) 
				{
					selectToolFilter = _selectFilter as ToolFilter; 
				}

				ToolFilter editToolFilter = null; 
				if (_editFilter != null) 
				{
					editToolFilter = _editFilter as ToolFilter; 
				}

				ToolFilter insertToolFilter = null; 
				if (_insertFilter != null) 
				{
					insertToolFilter = _insertFilter as ToolFilter; 
				}

				// Check each list item to see if it has been selected or de-selected
				for (int i = 0;  i < listBoxList.Items.Count; i++) 
				{
					FeatureLayer layer = (FeatureLayer)(listBoxList.Items[i]); 
					bool bInclude = listBoxList.GetSelected(i); 
					if (bInclude && !(_filter.IncludeLayer(layer)) 
						||
						!bInclude && _filter.IncludeLayer(layer)
						) 
					{
						// Either the layer was on and the user turned it off, 
						// or vice versa.  Update the filter's status to note
						// that the layer has been explicitly turned on or off. 
						toolFilter.SetExplicitInclude(layer,  bInclude); 

						// Now, we might want to adjust the other filters. 
						// For example, if a layer is changed so that it is no
						// longer selectable, we should turn off Editable and Insertable. 
						// Or if a layer is made Insertable (Drawable), 
						// we should make sure both Selectable and Editable are on.
						if (FilterType == FilterType.Selectable) 
						{
							// We're letting the user choose the selectable layers...
							if (!bInclude) 
							{
								// The user changed this layer to NOT be selectable,
								// so make sure the layer isn't Editable or Insertable
								if (editToolFilter != null && 
									editToolFilter.IncludeLayer(layer) ) 
								{
									// This layer IS currently editable; turn it off. 
									editToolFilter.SetExplicitInclude(layer, false); 
								}

								if (insertToolFilter != null && 
									insertToolFilter.IncludeLayer(layer) ) 
								{
									// This layer IS currently insertable; turn it off. 
									insertToolFilter.SetExplicitInclude(layer, false); 
								}
							}
						}
						else if (FilterType == FilterType.Editable) 
						{
							// We're letting the user choose the editable layers...
							if (bInclude) 
							{
								// The user changed this layer to be editable,
								// so make sure the layer IS Selectable
								if (selectToolFilter != null && 
									!selectToolFilter.IncludeLayer(layer) ) 
								{
									// This layer is NOT currently selectable; turn it on. 
									selectToolFilter.SetExplicitInclude(layer, true); 
								}
							}
							else 
							{
								// The user changed this layer to be NOT editable, 
								// so make sure it isn't Insertable either. 
								if (insertToolFilter != null && 
									insertToolFilter.IncludeLayer(layer) ) 
								{
									// This layer IS currently insertable; turn it off. 
									insertToolFilter.SetExplicitInclude(layer, false); 
								}
							}
						}
						else  
						{
							// We're letting the user choose the insertable layers...
							if (bInclude) 
							{
								// The user changed this layer to be insertable,
								// so make sure the layer IS Editable and Selectable
								if (selectToolFilter != null && 
									!selectToolFilter.IncludeLayer(layer) ) 
								{
									// This layer is NOT currently selectable; turn it on. 
									selectToolFilter.SetExplicitInclude(layer, true); 
								}
								if (editToolFilter != null && 
									!editToolFilter.IncludeLayer(layer) ) 
								{
									// This layer is NOT currently editable; turn it on. 
									editToolFilter.SetExplicitInclude(layer, true); 
								}
							}
						}
					}
				}
			}
			// Let the base class set DialogResult
		}

		// Populate the list with appropriate layers. 
		private void PopulateList() 
		{
			listBoxList.Items.Clear(); 
			if (_layers != null) 
			{
				AddLayersToList(_layers, listBoxList); 
			}
		}

		// Adds all appropriate Layer objects to the list. 
		// Calls itself recursively to search through GroupLayers. 
		private void AddLayersToList(LayersBase layers, ListBox listbox) 
		{
			foreach (IMapLayer layer in layers) 
			{
				if (layer is GroupLayer) 
				{
					GroupLayer grpLyr = layer as GroupLayer; 
					AddLayersToList(grpLyr, listbox); 
				}
				else if (layer is FeatureLayer) 
				{
					FeatureLayer featLyr = layer as FeatureLayer; 
					if (featLyr != null && featLyr.IsVisible && 
						featLyr.Type != LayerType.Raster &&
						featLyr.Type != LayerType.Grid) 
					{
						// The specified layer should be select-able.  
						// But if we've been asked to build a list of EDIT-able layers,
						// take some other criteria into account... Seamless layers are 
						// not editable, and some tables are simply read-only.  
						if ( (FilterType == FilterType.Selectable) || 
							  LayerCanBeMadeEditable(featLyr) ) 
						{
							listbox.Items.Add(featLyr); 
						}
					}
				}
			}
		}

		// Return true if the specified layer may be set to Editable. 
		// In the case of remote data, we may find that the Geocolumn is readonly, 
		// there is a spatial schema which provides for the ability to create a point 
		// geometry by referencing a geometry in another table. In this case, the table
		// may be editable but the geometry column will be marked as readonly.
		// Also Seamless tables can't be edited. 
		private bool LayerCanBeMadeEditable(FeatureLayer layer) 
		{
			bool b = false; 
			if (layer.Table.TableInfo.TableType == TableType.Seamless) 
			{
				b = false; // can't edit seamless tables
			}
			else if (_editFilter != null && _editFilter is ToolFilter) 
			{
				// The filter allows us to change Editable status; 
				// now see if the table also allows us. 
				if (layer.Table.SessionInfo.ReadOnly == false) 
				{
					// Table isn't read only, so we can proceed.  
					// If geocolumn isn't read only, the layer can be made editable.
					GeometryColumn geoCol = null; 
					Columns columns = layer.Table.TableInfo.Columns; 
					foreach (Column col in columns) 
					{
						geoCol = col as GeometryColumn; 
						if (geoCol != null)
						{
							break; 
						}
					}
					if (geoCol != null && geoCol.ReadOnly == false) 
					{
						b = true;
					}
				}
			}
			return b;
		}

		// Set which list items are selected, based on filter. 
		private void SetSelectedItems() 
		{
			if (_filter != null && listBoxList.Items.Count > 0) 
			{
				for (int i = 0;  i < listBoxList.Items.Count; i++) 
				{
					FeatureLayer layer = listBoxList.Items[i] as FeatureLayer; 
					if (layer != null  &&  _filter.IncludeLayer(layer) ) 
					{
						// Layer satisfies filter; show it as selected. 
						listBoxList.SetSelected(i, true); 
					}
				}
			}
		}

		/// <summary>
		/// Overriden to provide custom initialization.
		/// </summary>
		/// <param name="e">Standard event arg.</param>
		/// <remarks>None.</remarks>
		protected override void OnLoad(EventArgs e) 
		{
			// Do not call base.OnLoad (e)  because that would set the 
			// selected index based on the SelectedIndex, which we are not using
			if (FilterType == FilterType.Selectable) 
			{
				_filter = _selectFilter; 
			}
			else if (FilterType == FilterType.Editable) 
			{
				_filter = _editFilter; 
			}
			else 
			{
				_filter = _insertFilter;
			}
			if (_filter != null) 
			{
				listBoxList.Enabled = _filter is ToolFilter;
			}
			else 
			{
				listBoxList.Enabled = false; 
			}
			PopulateList(); 
			SetSelectedItems(); 
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

