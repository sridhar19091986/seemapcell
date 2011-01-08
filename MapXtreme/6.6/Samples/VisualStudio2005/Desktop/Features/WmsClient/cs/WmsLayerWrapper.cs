using System;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Wrapper for WmsMapLayer which overrides ToString so that the layer title is seen in the list box.
	/// </summary>
	public class WmsMapLayerWrapper
	{
		private WmsMapLayer _layer;
		private ICapabilities _capabilities;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="capabilities"></param>
		public WmsMapLayerWrapper(WmsMapLayer layer, ICapabilities capabilities)
		{
			_layer = layer;
			_capabilities = capabilities;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _capabilities.GetLayer(_layer.Layer).ToString();
		}

		public IWmsLayer WmsLayer
		{
			get
			{
				return _capabilities.GetLayer(_layer.Layer);
			}
		}
		public IWmsStyle WmsStyle
		{
			get
			{
				return WmsLayer.GetStyle(_layer.Style);
			}
		}
	}
}