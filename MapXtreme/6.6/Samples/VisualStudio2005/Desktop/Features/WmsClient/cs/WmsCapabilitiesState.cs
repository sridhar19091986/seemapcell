using System;
using System.Collections;
using System.Windows.Forms;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for an ICapabilities object in a set of controls:
	///   A TextBox for the server capabilities URL.
	///   A TextBox for the sever version.
	///   A TreeView for the list of layers.
	///   A ComboBox for the list of image formats.
	///   A WmsLayerState object for maintaining state information for the layer selected in the TreeView.
	/// </summary>
	public class WmsCapabilitiesState
	{
		private ICapabilities _capabilities;
		private TextBox _textBoxURL;
		private TextBox _textBoxVersion;
		private TreeView _treeViewServerLayers;
		private ComboBox _comboBoxImageFormat;
		private WmsLayerState _layerState;
		static private string[] _validFormats = {"png", "gif", "jpg", "jpeg", "tiff", "geotiff", "wbmp", };
		static private string mime = "image/";

		public WmsCapabilitiesState(
			TextBox textBoxURL,
			TextBox textBoxVersion,
			TreeView treeViewServerLayers,
			ComboBox comboBoxImageFormat,
			WmsLayerState layerState)
		{
			_textBoxURL = textBoxURL;
			_textBoxVersion = textBoxVersion;
			_treeViewServerLayers = treeViewServerLayers;
			_comboBoxImageFormat = comboBoxImageFormat;
			_layerState = layerState;

			Clear();
		}

		public void Clear()
		{
			_textBoxURL.Text = null;
			_textBoxVersion.Text = null;
			_treeViewServerLayers.EnabledChanged -= new EventHandler(_treeViewServerLayers_EnabledChanged);
			_treeViewServerLayers.AfterSelect -= new TreeViewEventHandler(_treeViewServerLayers_AfterSelect);
			_treeViewServerLayers.Nodes.Clear();
			_treeViewServerLayers.Enabled = false;
			_comboBoxImageFormat.Items.Clear();
			_comboBoxImageFormat.Enabled = false;
			_layerState.Clear();
			_capabilities = null;
		}

		public void Set(ICapabilities capabilities)
		{
			Clear();
			_capabilities = capabilities;
			if (null != _capabilities)
			{
				_textBoxURL.Text = _capabilities.CapabilitiesRequestUrl;
				_textBoxVersion.Text = _capabilities.Version;
				_treeViewServerLayers.AfterSelect += new TreeViewEventHandler(_treeViewServerLayers_AfterSelect);
				_treeViewServerLayers.EnabledChanged += new EventHandler(_treeViewServerLayers_EnabledChanged);
				LoadLayerList();
				LoadImageFormats();
			}
		}
		public void LoadLayerList()
		{
			_treeViewServerLayers.Nodes.Clear();
			if (_capabilities != null)
			{
				AddLayer(_capabilities.RootLayer, _treeViewServerLayers.Nodes);
			}
			_treeViewServerLayers.ExpandAll();
			_treeViewServerLayers.Enabled = _treeViewServerLayers.Nodes.Count > 0;
			_treeViewServerLayers.SelectedNode = _treeViewServerLayers.Nodes.Count > 0 ? _treeViewServerLayers.Nodes[0] : null;
		}

		private void AddLayer(IWmsLayer mapLayer, TreeNodeCollection nodes)
		{
			// Add this layer to the control
			TreeNode newNode = new TreeNode(mapLayer.Title);
			newNode.Tag = mapLayer;
			nodes.Add(newNode);
			// Recurse to add child layers
			AddLayers(mapLayer.Layers, newNode.Nodes);
		}

		private void AddLayers(IWmsLayer[] mapLayers, TreeNodeCollection nodes)
		{
			foreach (IWmsLayer mapLayer in mapLayers)
			{
				AddLayer(mapLayer, nodes);
			}
		}

		private bool IsValidFormat(string mimeType)
		{
			int start = mimeType.StartsWith(mime) ? mime.Length : 0;
			string format = mimeType.Substring(start);
			foreach (string s in _validFormats)
			{
				if (0 == string.Compare(s, format, true))
				{
					return true;
				}
			}
			return false;
		}

		private void LoadImageFormats()
		{
			_comboBoxImageFormat.Items.Clear();
			foreach (string mimeType in _capabilities.MapFormats)
			{
				if (IsValidFormat(mimeType)) _comboBoxImageFormat.Items.Add(mimeType);
			}
			_comboBoxImageFormat.Enabled = _comboBoxImageFormat.Items.Count > 0;
		}

		private void _treeViewServerLayers_AfterSelect(object sender, TreeViewEventArgs e)
		{
			IWmsLayer layer = e.Node.Tag as IWmsLayer;
			_layerState.Set(layer, null);
		}

		private void _treeViewServerLayers_EnabledChanged(object sender, EventArgs e)
		{
			if (!(sender as TreeView).Enabled)
			{
				_layerState.Clear();
			}
		}
	}
}