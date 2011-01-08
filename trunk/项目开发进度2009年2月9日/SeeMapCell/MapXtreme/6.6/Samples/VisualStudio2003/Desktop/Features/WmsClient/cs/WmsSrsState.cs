using System;
using System.Windows.Forms;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for an Srs string in a set of controls:
	///   A TextBox for the coordinate system name for the Srs.
	/// </summary>
	public class WmsSrsState
	{
		private string _srs;
		private TextBox _textBoxCoordSysName;

		public WmsSrsState(TextBox textBoxCoordSysName)
		{
			_textBoxCoordSysName = textBoxCoordSysName;
			Clear();
		}

		public void Set(string srs)
		{
			Clear();
			_srs = srs;
			_textBoxCoordSysName.Text = null != srs ? MapInfo.Engine.Session.Current.CoordSysFactory.CoordSysName(MapInfo.Engine.Session.Current.CoordSysFactory.CreateCoordSys(_srs)) : null;
		}

		public void Clear()
		{
			_textBoxCoordSysName.Text = null;
			_srs = null;
		}
	}
}