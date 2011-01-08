using System;
using System.Collections;
using System.Windows.Forms;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for an IWmsLayer object in a set of controls:
	///   A TextBox for the layer abstract.
	///   A ComboBox for the list of available styles for the layer.
	///   A WmsStyleState object for maintaining state information for the style selected in the ComboBox.
	/// </summary>
	public class WmsLayerState
	{
		private IWmsLayer _wmsLayer;
		private TextBox _textBoxAbstract;
		private ComboBox _comboBoxStyles;
		private WmsStyleState _wmsStyleState;

		public WmsLayerState(
			TextBox textBoxAbstract,
			ComboBox comboBoxStyles,
			WmsStyleState wmsStyleState)
		{
			_textBoxAbstract = textBoxAbstract;
			_comboBoxStyles = comboBoxStyles;
			_wmsStyleState = wmsStyleState;
			Clear();
		}

		public void Set(IWmsLayer wmsLayer, IWmsStyle wmsStyle)
		{
			Clear();
			_wmsLayer = wmsLayer;
			_comboBoxStyles.SelectedIndexChanged += new EventHandler(_comboBoxStyles_SelectedIndexChanged);
			if (null != _wmsLayer)
			{
				if (null != _textBoxAbstract)
				{
					_textBoxAbstract.Text = _wmsLayer.Abstract;
				}
				LoadStyleList(wmsStyle);
			}
			_wmsStyleState.Set(wmsStyle);
		}

		public void Clear()
		{
			_comboBoxStyles.Items.Clear();
			_comboBoxStyles.Enabled = false;
			_wmsStyleState.Clear();
			if (null != _textBoxAbstract)
			{
				_textBoxAbstract.Text = null;
			}
			_comboBoxStyles.SelectedIndexChanged -= new EventHandler(_comboBoxStyles_SelectedIndexChanged);
			_wmsLayer = null;
		}

		private void LoadStyleList(IWmsStyle wmsStyle)
		{
			_comboBoxStyles.Items.Clear();
			if (null != _wmsLayer)
			{
				_comboBoxStyles.Items.Add("<Default>");
				foreach (IWmsStyle style in _wmsLayer.Styles)
				{
					_comboBoxStyles.Items.Add(style);
				}
			}
			_comboBoxStyles.Enabled = _comboBoxStyles.Items.Count > 0;
			_comboBoxStyles.SelectedIndex = null != wmsStyle ? _comboBoxStyles.FindString(wmsStyle.ToString()) : 0;
		}

		private void _comboBoxStyles_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBoxStyles = sender as ComboBox;
			_wmsStyleState.Set(comboBoxStyles.SelectedItem as IWmsStyle);
		}
	}
}