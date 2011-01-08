using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Xml;
using MapInfo.Engine;

namespace WmsPreview
{
	/// <summary>
	/// Loads a set of WmsServerInfo objects into a ComboBox control.
	/// The list of servers is determined by the contents of MIWMSServers.xml.
	/// </summary>
	public class ServerList
	{
		private ComboBox _comboBoxServers;
		private static string _defaultServer;

		public ServerList(ComboBox comboBoxServers)
		{
			_comboBoxServers = comboBoxServers;
		}

		public void Set()
		{
			LoadServerList(InitServerList());
		}

		private static void SetSearchPath()
		{
			// Set table search path to value sampledatasearch registry key
			// if not found, then just use the app's current directory
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
			string keyValue = (string)key.GetValue("SampleDataSearchPath");
			if (keyValue != null && keyValue.Length > 0)
			{
				if (keyValue.EndsWith("\\") == false)
				{
					keyValue += "\\";
				}
			}
			else
			{
				keyValue = Environment.CurrentDirectory;
			}
			key.Close();
			Session.Current.TableSearchPath.Path = keyValue;
		}

		private static XmlDocument ServerDoc()
		{
			XmlDocument serverDoc = null;
			string path;
			if (!Session.Current.TableSearchPath.FileExists("MIWMSServers.xml", out path)) throw new ApplicationException("MIWMSServers.xml not found");
			serverDoc = new XmlDocument();
			serverDoc.Load(path);
			return serverDoc;
		}

		private static WmsServerInfo[] InitServerList()
		{
			SetSearchPath();
			XmlDocument serverDoc = ServerDoc();
			XmlNodeList serverNodes = serverDoc.DocumentElement.SelectNodes("Server");
			ArrayList serverList = new ArrayList();
			NameValueCollection userDefinedParameters = new NameValueCollection();
			foreach (XmlNode serverNode in serverNodes)
			{
				XmlNode userDefinedParametersNode = serverNode.SelectSingleNode("UserDefinedParameters");
				if (null != userDefinedParametersNode)
				{
					foreach (XmlAttribute xmlAttribute in userDefinedParametersNode.Attributes)
					{
						userDefinedParameters.Add(xmlAttribute.Name, xmlAttribute.Value);
					}
				}
				serverList.Add(new WmsServerInfo(serverNode.SelectSingleNode("Description").InnerText, serverNode.SelectSingleNode("HTTP").InnerText, userDefinedParameters.Count > 0 ? new NameValueCollection(userDefinedParameters) : null));
				userDefinedParameters.Clear();
			}
			XmlNode defaultServerNode = serverDoc.DocumentElement.SelectSingleNode("Server/Default");
			_defaultServer = defaultServerNode.ParentNode.SelectSingleNode("Description").InnerText;
			serverList.Sort();
			return serverList.ToArray(typeof(WmsServerInfo)) as WmsServerInfo[];
		}

		private void LoadServerList(WmsServerInfo[] serverList)
		{
			_comboBoxServers.Items.Clear();
			foreach (WmsServerInfo si in serverList)
			{
				_comboBoxServers.Items.Add(si);
			}
			_comboBoxServers.SelectedIndex = _comboBoxServers.FindString(_defaultServer);
		}
	}
}