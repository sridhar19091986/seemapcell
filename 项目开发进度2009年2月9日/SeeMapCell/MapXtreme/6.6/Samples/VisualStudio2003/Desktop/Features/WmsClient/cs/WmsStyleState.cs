using System;
using System.Windows.Forms;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for an IWmsStyle object in a set of controls:
	///   A TextBox for the style abstract.
	/// </summary>
	public class WmsStyleState
	{
		private IWmsStyle _wmsStyle;
		private TextBox _textBoxStyleAbstract;

		public WmsStyleState(TextBox textBoxStyleAbstract)
		{
			_textBoxStyleAbstract = textBoxStyleAbstract;
			Clear();
		}

		public void Set(IWmsStyle wmsStyle)
		{
			_wmsStyle = wmsStyle;
			_textBoxStyleAbstract.Text = _wmsStyle != null ? _wmsStyle.Abstract : null;
		}

		public void Clear()
		{
			_textBoxStyleAbstract.Text = null;
			_wmsStyle = null;
		}
	}
}