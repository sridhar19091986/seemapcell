using System;
using System.Windows.Forms;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Windows.Controls;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Maintains the state information for a Wms FeatureLayer in MapControl.
	/// </summary>
	public class FeatureLayerState
	{
		private MapControl _mapControlWms;
		private FeatureLayer _featureLayer = null;

		public FeatureLayerState(MapControl mapControlWms)
		{
			_mapControlWms = mapControlWms;

			Clear();
		}

		public FeatureLayer FeatureLayer
		{
			get
			{
				return _featureLayer;
			}
		}

		public void Clear()
		{
			_mapControlWms.Map.Layers.Clear();
			_featureLayer = null;
		}

		private void Clear(FeatureLayer featureLayer)
		{
			Clear();
			WmsClient wmsClient = GetClient(featureLayer);
			if (null != wmsClient)
			{
				wmsClient.ClientImagePropertiesChanged -= new MapInfo.Wms.WmsClient.ClientImagePropertiesChangedHandler(wmsClient_ClientImagePropertiesChanged);
			}
		}

		public void Set(FeatureLayer featureLayer)
		{
			Clear(featureLayer);
			_featureLayer = featureLayer;
			if (null != featureLayer)
			{
				WmsClient wmsClient = GetClient(featureLayer);
				if (null != wmsClient)
				{
					wmsClient.ClientImagePropertiesChanged += new MapInfo.Wms.WmsClient.ClientImagePropertiesChangedHandler(wmsClient_ClientImagePropertiesChanged);
					SwitchFeatureLayer(wmsClient);
				}
			}
		}

		private void SwitchFeatureLayer(WmsClient wmsClient)
		{
			_mapControlWms.Map.Layers.Clear();
			if (wmsClient.HasLayers && null != _featureLayer.CoordSys)
			{
				_mapControlWms.Map.Layers.Add(_featureLayer);
				SetMapBounds(wmsClient);
			}
		}

		private WmsClient GetClient(FeatureLayer featureLayer)
		{
			TableInfoWms tableInfoWms = featureLayer.Table.TableInfo as TableInfoWms;
			return null != tableInfoWms ? tableInfoWms.WmsClient : null;
		}

		private void SetMapBounds(WmsClient wmsClient)
		{
			_mapControlWms.Map.Bounds = _featureLayer.Bounds;
		}

		private void wmsClient_ClientImagePropertiesChanged(WmsClient client)
		{
			if (0 == _mapControlWms.Map.Layers.Count)
			{
				SwitchFeatureLayer(client);
			}
		}
	}
}