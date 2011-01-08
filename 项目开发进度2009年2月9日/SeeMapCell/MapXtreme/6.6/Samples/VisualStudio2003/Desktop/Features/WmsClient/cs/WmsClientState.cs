using System;
using System.Collections;
using System.Windows.Forms;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for a WmsClient object in a set of controls:
	///   A ComboBox for the list of image formats.
	///   A CheckBox for the transparent property.
	///   A PictureBox for the background color property.
	///   A ComboBox for the list of Srs strings.
	///   A WmsSrsState object for maintaining state information for the Srs selected in the ComboBox.
	///   A ListBox for the list of layers.
	///   A WmsLayerState object for maintaining state information for the layer selected in the ListBox.
	/// </summary>
	public class WmsClientState
	{
		private WmsClient _wmsClient = null;
		private ComboBox _comboBoxImageFormat;
		private CheckBox _checkBoxTransparent;
		private PictureBox _pictureBoxBGColor;
		private ComboBox _comboBoxProjection;
		private WmsSrsState _srsState;
		private ListBox _listBoxLayers;
		private WmsLayerState _layerState;

		public WmsClientState(
			ComboBox comboBoxImageFormat,
			CheckBox checkBoxTransparent,
			PictureBox pictureBoxBGColor,
			ComboBox comboBoxProjection,
			WmsSrsState srsState,
			ListBox listBoxClientLayers,
			WmsLayerState layerState)
		{
			_comboBoxImageFormat = comboBoxImageFormat;
			_checkBoxTransparent = checkBoxTransparent;
			_pictureBoxBGColor = pictureBoxBGColor;
			_comboBoxProjection = comboBoxProjection;
			_srsState = srsState;
			_listBoxLayers = listBoxClientLayers;
			_layerState = layerState;

			Clear();
		}

		public void Clear()
		{
			_comboBoxImageFormat.Enabled = false;
			_checkBoxTransparent.Enabled = false;
			_pictureBoxBGColor.Enabled = false;

			_comboBoxProjection.Enabled = false;
			_comboBoxProjection.Items.Clear();
			_comboBoxProjection.SelectedIndexChanged -= new EventHandler(_comboBoxProjection_SelectedIndexChanged);

			_srsState.Clear();

			_listBoxLayers.SelectedIndexChanged -= new EventHandler(_listBoxLayers_SelectedIndexChanged);
			_listBoxLayers.Enabled = false;
			_listBoxLayers.Items.Clear();

			_layerState.Clear();

			if (null != _wmsClient)
			{
				_wmsClient.LayerAdded -= new MapInfo.Wms.WmsClient.LayerAddedHandler(wmsClient_LayerAdded);
				_wmsClient.LayerRemoved -= new MapInfo.Wms.WmsClient.LayerRemovedHandler(wmsClient_LayerRemoved);
				_wmsClient.LayerMovedDown -= new MapInfo.Wms.WmsClient.LayerMovedDownHandler(wmsClient_LayerMovedDown);
				_wmsClient.LayerMovedUp -= new MapInfo.Wms.WmsClient.LayerMovedUpHandler(wmsClient_LayerMovedUp);
				_wmsClient.ClientSrsChanged -= new MapInfo.Wms.WmsClient.ClientSrsChangedHandler(wmsClient_ClientSrsChanged);
			}
			_wmsClient = null;
		}

		public void Set(WmsClient wmsClient)
		{
			Clear();
			_wmsClient = wmsClient;
			if (null != _wmsClient)
			{
				_comboBoxImageFormat.Enabled = _comboBoxImageFormat.Items.Count > 0;
				_comboBoxImageFormat.SelectedItem = _wmsClient.MimeType;

				_comboBoxProjection.SelectedIndexChanged += new EventHandler(_comboBoxProjection_SelectedIndexChanged);

				_checkBoxTransparent.Enabled = true;
				_checkBoxTransparent.Checked = wmsClient.Transparent;

				_pictureBoxBGColor.Enabled = true;
				_pictureBoxBGColor.BackColor = _wmsClient.BGColor;


				LoadLayerList();
				LoadSrsList();

				_wmsClient.LayerAdded += new MapInfo.Wms.WmsClient.LayerAddedHandler(wmsClient_LayerAdded);
				_wmsClient.LayerRemoved += new MapInfo.Wms.WmsClient.LayerRemovedHandler(wmsClient_LayerRemoved);
				_wmsClient.LayerMovedDown += new MapInfo.Wms.WmsClient.LayerMovedDownHandler(wmsClient_LayerMovedDown);
				_wmsClient.LayerMovedUp += new MapInfo.Wms.WmsClient.LayerMovedUpHandler(wmsClient_LayerMovedUp);
				_wmsClient.ClientSrsChanged += new MapInfo.Wms.WmsClient.ClientSrsChangedHandler(wmsClient_ClientSrsChanged);
			}
		}

		private void LoadLayerList()
		{
			foreach (WmsMapLayer layer in _wmsClient.Layers)
			{
				_listBoxLayers.Items.Add(new WmsMapLayerWrapper(layer, _wmsClient.Capabilities));
			}
			_listBoxLayers.SelectedIndexChanged += new EventHandler(_listBoxLayers_SelectedIndexChanged);
			_listBoxLayers.SelectedIndex = _listBoxLayers.Items.Count > 0 ? 0 : -1;
			_listBoxLayers.Enabled = _listBoxLayers.Items.Count > 0;
		}

		private void LoadSrsList()
		{
			_comboBoxProjection.Items.Clear();
			bool isAvailableSrs = false;
			foreach (string srs in _wmsClient.SrsList)
			{
				if (null != MapInfo.Engine.Session.Current.CoordSysFactory.CreateCoordSys(srs))
				{
					_comboBoxProjection.Items.Add(srs);
					isAvailableSrs = true;
				}
			}
			SetCurrentSrs(isAvailableSrs ? _wmsClient.Srs : null);
			_comboBoxProjection.Enabled = _comboBoxProjection.Items.Count > 0;
		}

		private void SetCurrentSrs(string srs)
		{
			int selectedIndex;
			if (null != srs && (selectedIndex = _comboBoxProjection.FindString(srs)) >= 0)
			{
				_comboBoxProjection.SelectedIndex = selectedIndex;
			}
			else
			{
				_comboBoxProjection.SelectedIndex = _comboBoxProjection.Items.Count - 1;
			}
			_srsState.Set(srs);
		}

		private void wmsClient_LayerAdded(WmsClient client, WmsMapLayer layer)
		{
			CheckSrs();
			_listBoxLayers.Items.Add(new WmsMapLayerWrapper(layer, _wmsClient.Capabilities));
			_listBoxLayers.SelectedIndex = client.GetLayerIndex(layer);
			_listBoxLayers.Enabled = _listBoxLayers.Items.Count > 0;
		}

		private void wmsClient_LayerRemoved(WmsClient client, int layer)
		{
			CheckSrs();
			_listBoxLayers.Items.RemoveAt(layer);
			_listBoxLayers.SelectedIndex = client.HasLayers ? Math.Min(layer, _listBoxLayers.Items.Count - 1) : -1;
			_listBoxLayers.Enabled = _listBoxLayers.Items.Count > 0;
		}

		private void wmsClient_LayerMovedDown(WmsClient client, int layer)
		{
			if (layer >= 0)
			{
				Object o = _listBoxLayers.Items[layer];
				_listBoxLayers.Items.RemoveAt(layer);
				layer += 1;
				if (layer > _listBoxLayers.Items.Count)
					_listBoxLayers.Items.Add(o);
				else
					_listBoxLayers.Items.Insert(layer, o);
				_listBoxLayers.SelectedIndex = layer;
			}
		}

		private void wmsClient_LayerMovedUp(WmsClient client, int layer)
		{
			if (layer >= 0)
			{
				Object o = _listBoxLayers.Items[layer];
				_listBoxLayers.Items.RemoveAt(layer);
				layer -= 1;
				_listBoxLayers.Items.Insert(layer, o);
				_listBoxLayers.SelectedIndex = layer;
			}
		}

		private void wmsClient_ClientSrsChanged(WmsClient client, string srs)
		{
			SetCurrentSrs(srs);
		}

		private void _listBoxLayers_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox listBox = sender as ListBox;
			WmsMapLayerWrapper wmsMapLayerWrapper = listBox.SelectedIndex >= 0 ? listBox.SelectedItem as WmsMapLayerWrapper : null;
			if (null != wmsMapLayerWrapper)
			{
				_layerState.Set(wmsMapLayerWrapper.WmsLayer, wmsMapLayerWrapper.WmsStyle);
			}
			else
			{
				_layerState.Clear();
			}
		}

		private void _comboBoxProjection_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			_srsState.Set(comboBox.SelectedItem as string);
		}

		private void CheckSrs()
		{
			LoadSrsList();
			string srs = _wmsClient.Srs;
			if (null == srs)
			{
				srs = "EPSG:4326";
			}
			if (!_wmsClient.IsValidSrs(srs))
			{
				IEnumerator e = _wmsClient.SrsList.GetEnumerator();
				srs = e.MoveNext() ? e.Current as string : null;
			}
			if (null != srs)
			{
				_wmsClient.Srs = srs;
			}
		}
	}
}