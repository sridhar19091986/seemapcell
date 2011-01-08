using System;
using System.Collections;
using System.Collections.Specialized;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Mapping;
using MapInfo.Wms;

namespace WmsPreview
{
	/// <summary>
	/// Summary description for WmsServerInfo.
	/// </summary>
	public class WmsServerInfo : IComparable
	{
		string _description;
		string _url;
		NameValueCollection _userDefinedParameters;
		TableInfoWms _tableInfoWms = null;
		FeatureLayer _featureLayer = null;

		public WmsServerInfo(string description, string url)
			: this(description, url, null)
		{
		}

		public WmsServerInfo(string description, string url, NameValueCollection userDefinedParameters)
		{
			if (null == description) throw new ArgumentNullException("description");
			if (null == url) throw new ArgumentNullException("url");
			_description = description;
			_url = url;
			_userDefinedParameters = userDefinedParameters;
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string URL
		{
			get { return _url; }
			set { _url = value; }
		}

		public TableInfoWms TableInfoWms
		{
			get
			{
				if (null == _tableInfoWms)
				{
					WmsClient wmsClient = new WmsClient(WmsClientUtilities.GetCapabilities(_url, new string[] { "1.1.1", "1.1.0", "1.0.0", }, _userDefinedParameters));
					//WmsClient wmsClient = new WmsClient(WmsClientUtilities.GetCapabilities(_url, "1.1.1", _userDefinedParameters));
					wmsClient.UserDefinedParameters = _userDefinedParameters;
					_tableInfoWms = new TableInfoWms(Description, wmsClient);
				}
				return _tableInfoWms;
			}
		}

		public WmsClient Client
		{
			get { return TableInfoWms.WmsClient;  }
		}

		public ICapabilities Capabilities
		{
			get { return Client.Capabilities; }
		}

		public FeatureLayer FeatureLayer
		{
			get
			{
				if (null == _featureLayer)
				{
					try
					{
						Table t = Session.Current.Catalog.OpenTable(TableInfoWms);
						_featureLayer = new FeatureLayer(t);
					}
					catch (SystemException ex)
					{
						string x = ex.Message;
						string y = ex.GetType().ToString();
						string z = y;
					}
				}
				return _featureLayer;
			}
		}

		public Table Table
		{
			get { return FeatureLayer.Table; }
		}

		public TableInfo TableInfo
		{
			get { return TableInfoWms; }
		}

		public override string ToString()
		{
			return Description;
		}

		public int CompareTo(Object o)
		{
			if (o is WmsServerInfo)
			{
				return ToString().CompareTo(o.ToString());
			}
			throw new ArgumentException();
		}
	}
}