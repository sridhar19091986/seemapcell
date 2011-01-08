using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.UI.Design;
using System.Web.UI;
using System.Xml;
using EnvDTE;
using System.Runtime.InteropServices;


namespace MapInfo.WebControls
{
	/// <summary>
	/// This class provides various helper functions 
	/// </summary>
	internal class WebDesignerUtility {
		[DllImport("ole32.dll", EntryPoint = "GetRunningObjectTable")]
		private static extern int GetRunningObjectTable(int res, out UCOMIRunningObjectTable ROT);
		[DllImport("ole32.dll", EntryPoint = "CreateBindCtx")]
		private static extern int CreateBindCtx(int res, out UCOMIBindCtx ctx);

		private const uint S_OK = 0;

		private static readonly string _applicationDirRegkey = "SOFTWARE\\MapInfo\\MapXtreme\\6.6";
		private static readonly string _applicationDirSubkey = "ApplicationDir";

		[CLSCompliant(false)]
		internal static DTE GetCurrentDTEObject() {
			EnvDTE.DTE dte = null;

			// Get the ROT
			UCOMIRunningObjectTable rot;
			int uret = GetRunningObjectTable(0, out rot);
			if (uret == S_OK) {
				// Get an enumerator to access the registered objects
				UCOMIEnumMoniker EnumMon;
				rot.EnumRunning(out EnumMon);

				int fetched = 0;
				UCOMIMoniker [] aMons = new UCOMIMoniker[1];

				if (EnumMon != null) {
                    string dteNameStart = "VisualStudio.DTE.";
                    string dteNameEnd = ":" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
					while (EnumMon.Next(1, aMons, out fetched) == 0) {
						// Set up a bindind context so that we can access the monikers
						string name;
						UCOMIBindCtx ctx;
						uret = CreateBindCtx(0, out ctx);

						// Get the display string
						aMons[0].GetDisplayName(ctx, null, out name);

						// if this is the one we are interested in..
						if (name.IndexOf(dteNameStart) != -1 && name.EndsWith(dteNameEnd)) {
							object temp;
							rot.GetObject(aMons[0], out temp);
							dte = (EnvDTE.DTE) temp;
                            break;
						}
					}
				}
			}
			return dte;
		}

		///	<summary>Gets the current project.</summary>
		///	<param name="dte"/>
		///	<param name="documentUrl"></param>
		///	<returns>None.</returns>
		[CLSCompliantAttribute(false)]
		internal static Project GetCurrentWebProject(DTE dte, string documentUrl) {
			Project project = null;

			// Proceed if we have a solution open
			if (dte != null && dte.Solution.Count > 0) {
				// Proceed if solution has atleast one project
				Document activeDoc = dte.ActiveDocument;

				// If the URL of the project is same as the URL string returned by the control then
				// we have the current project
				Project tempProject = null;
				for (int i=1; i <= dte.Solution.Projects.Count; i++) {
					tempProject = dte.Solution.Projects.Item(i);
					if (tempProject.Properties != null) {
						string projUrl;
						CultureInfo us = new CultureInfo("en-US");
						bool is80 = false;
						if (Double.Parse(tempProject.DTE.Version.ToString(), us) >= 8.0) 
						{
							is80 = true;
							projUrl = tempProject.Properties.Item("OpenedURL").Value.ToString();
						}
						else 
						{
							projUrl = tempProject.Properties.Item("URL").Value.ToString();
						}
						if (documentUrl.IndexOf(projUrl) >= 0) 
						{
							project = tempProject;
							if (is80) {
								// Copy the web scripts and images local to the project. First create folder
								try {
									ProjectItem folder = project.ProjectItems.Item(Resources.ResourceFolder);
								} catch(Exception) {
									// Now copy files
									string appPath = null;
									Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_applicationDirRegkey);
									if (regKey!= null) {
										appPath =(string)regKey.GetValue(_applicationDirSubkey);
									}
									project.ProjectItems.AddFromFileCopy(appPath + @"\"+Resources.ResourceFolder);
									regKey.Close();
								}
							}
							break;
						}
					}
				}
			}
			return project;
		}

		private static bool RemoveEntry(XmlNode node, AssemblyName assembly, string attr) {
			bool bRemoved = false;
			if (node != null) {
				string assemblyAttr = node.Attributes[attr].Value;
				// Remove the node only if the version is different
				if (assembly != null && assemblyAttr.IndexOf(assembly.Version.ToString()) < 0) {
					node.ParentNode.RemoveChild(node);
					bRemoved = true;
				}
			} else {
				bRemoved = true;
			}
			return bRemoved;
		}
		
		private static bool AddHttpNode(XmlDocument xmlDoc, XmlNode httpHandlersNode, string path, string type) {
			string searchStr = string.Format("//add[contains(@path, '{0}')]", path);
			XmlNode node = xmlDoc.SelectSingleNode(searchStr);
			bool updated = false;

			AssemblyName _webAssembly = typeof(MapInfo.WebControls.MapControl).Assembly.GetName();

			if (RemoveEntry(node, _webAssembly, "type")) {
				XmlNode addNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "");
				System.Xml.XmlAttribute addAttrib1 = xmlDoc.CreateAttribute("verb");
				addAttrib1.Value = "*";
				addNode.Attributes.Append(addAttrib1);

				System.Xml.XmlAttribute addAttrib2 = xmlDoc.CreateAttribute("path");
				addAttrib2.Value = path;
				addNode.Attributes.Append(addAttrib2);

				System.Xml.XmlAttribute addAttrib3 = xmlDoc.CreateAttribute("type");
				addAttrib3.Value =string.Format("{0}, {1}", type, _webAssembly.FullName);
				addNode.Attributes.Append(addAttrib3);

				httpHandlersNode.AppendChild(addNode);
				updated = true;
			}
			return updated;
		}

		/// <summary>
		/// Adds the line containing the HttpHandler to the web.config file.
		/// </summary>
		/// <param name="xmlDoc">Document pointing to the web.config file.</param>
		/// <param name="handlerName">Name of the HttpHandler.</param>
		/// <param name="className">Namespace and the classname of the handler.</param>
		/// <remarks>This method is only used at design time to automatically update the web.config file with appropriate handlers.</remarks>
		internal static bool AddController(XmlDocument xmlDoc, string handlerName, string className) {
			bool updated = false;
			// Now create httphandler node
			XmlNode httpNode = xmlDoc.SelectSingleNode("//httpHandlers");
			if (httpNode == null) {
				httpNode = xmlDoc.CreateNode(XmlNodeType.Element, "httpHandlers", "");
			}

			updated = AddHttpNode(xmlDoc, httpNode, handlerName, className);

			// Now insert this information into system.web node in the file
			if (updated) {
				System.Xml.XmlNodeList sysNode = xmlDoc.GetElementsByTagName("system.web");
				sysNode.Item(0).AppendChild(httpNode);
			}
			return updated;
		}

		/// <summary>
		/// Adds commented appsettings lines to web.config files, if they don't exist.
		/// </summary>
		/// <param name="xmlDoc">Document pointing to the web.config file.</param>
		/// <param name="appSettingsNode">Node to be added.</param>
		/// <param name="comment">comment line.</param>
		/// <remarks>These lines are used to decide whether to use pooling, use preloaded workspaces, and way state if managed. These are
		/// added as comments because it is the user's decision. This happens at design time.</remarks>
		private static bool AddCommentNode(XmlDocument xmlDoc, XmlNode appSettingsNode, string comment) 
		{
			if(comment == null)
			{
				return false;
			}
			// Leave if already there:
			foreach(XmlNode child in appSettingsNode.ChildNodes) {
				XmlComment childAsComment = child as XmlComment;
				if(childAsComment != null) {
					if(childAsComment.Data.ToLower() == comment.ToLower()) {
						return false;
					}
				}
			}

			// Add it:
			XmlNode commentNode = xmlDoc.CreateComment(comment);
			appSettingsNode.AppendChild(commentNode);
			return true;
		}

		/// <summary>
		/// Adds appsettings lines to web.config file to control pooling, statemanagement, and workspace loading.
		/// </summary>
		/// <param name="xmlDoc">Document pointing to the web.config file.</param>
		internal static bool AddAppSettings(XmlDocument xmlDoc) 
		{
			bool updated = false;
			XmlNode appSettingsNode = xmlDoc.SelectSingleNode("//appSettings");
			if(appSettingsNode == null) {
				appSettingsNode = xmlDoc.CreateNode(XmlNodeType.Element, "appSettings", "");
				System.Xml.XmlNodeList compNode = xmlDoc.GetElementsByTagName("configuration");
				compNode.Item(0).InsertBefore(appSettingsNode, compNode.Item(0).FirstChild);
				updated = true;
			}
			if(appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.Pooled']") == null) {
				updated |= AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("PooledSetting"));
				updated |= AddCommentNode(xmlDoc, appSettingsNode, "<add key=\"MapInfo.Engine.Session.Pooled\" value=\"false\" />");
			}
			if(appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.State']") == null) {
				updated |= AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("StateSetting"));
				updated |= AddCommentNode(xmlDoc, appSettingsNode, "<add key=\"MapInfo.Engine.Session.State\" value=\"HttpSessionState\" />");
			}
			if(appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.Workspace']") == null) {
				updated |= AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("WorkspaceSetting"));
				updated |= AddCommentNode(xmlDoc, appSettingsNode, "<add key=\"MapInfo.Engine.Session.Workspace\" value=\"c:\\my workspace.mws\" />");
			}
			if(appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.UseCallContext']") == null) 
			{
				updated |= AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("UseCallContextSetting"));
				updated |= AddCommentNode(xmlDoc, appSettingsNode, "<add key=\"MapInfo.Engine.Session.UseCallContext\" value=\"true\" />");
			}
			if(appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.RestoreWithinCallContext']") == null) {
				updated |= AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("RestoreWithinCallContextSetting"));
				updated |= AddCommentNode(xmlDoc, appSettingsNode, "<add key=\"MapInfo.Engine.Session.RestoreWithinCallContext\" value=\"true\" />");
			}
			return updated;
		}

		/// <summary>
		/// Adds lines containing httpmodules to web.config file. 
		/// </summary>
		/// <param name="xmlDoc">Document pointing to web.config file.</param>
		/// <remarks>This module handles the MapInfo session creation.</remarks>
		internal static bool AddHttpModules(XmlDocument xmlDoc) 
		{
			bool updated = false;
			// Now create httphandler node
			XmlNode httpNode = xmlDoc.SelectSingleNode("//httpModules");
			if (httpNode == null) {
				httpNode = xmlDoc.CreateNode(XmlNodeType.Element, "httpModules", "");
			}

			string searchStr = string.Format("//add[contains(@type, '{0}')]", "MapInfo.Engine.WebSessionActivator");
			XmlNode node = xmlDoc.SelectSingleNode(searchStr);
			AssemblyName coreEngineAssembly = typeof(MapInfo.Engine.WebSessionActivator).Assembly.GetName();
			if (RemoveEntry(node, coreEngineAssembly, "type")) {
				XmlNode addNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "");
				
				System.Xml.XmlAttribute addAttrib1 = xmlDoc.CreateAttribute("type");
				addAttrib1.Value = string.Format("{0}, {1}", "MapInfo.Engine.WebSessionActivator", coreEngineAssembly.FullName);
				addNode.Attributes.Append(addAttrib1);

				System.Xml.XmlAttribute addAttrib2 = xmlDoc.CreateAttribute("name");
				addAttrib2.Value = "WebSessionActivator";
				addNode.Attributes.Append(addAttrib2);

				httpNode.AppendChild(addNode);
				updated = true;
			}

			// Now insert this information into system.web node in the file
			if (updated) {
				System.Xml.XmlNodeList sysNode = xmlDoc.GetElementsByTagName("system.web");
				sysNode.Item(0).AppendChild(httpNode);
			}
			return updated;
		}

		private static bool AddAssemblyNode(XmlDocument xmlDoc, XmlNode assembliesNode, AssemblyName assembly) 
		{
			bool updated = false;
			string searchStr = string.Format("//add[contains(@assembly, '{0}')]", assembly.Name);
			XmlNode node = xmlDoc.SelectSingleNode(searchStr);
			if (RemoveEntry(node, assembly, "assembly")) {
				// Create add node
				XmlNode addNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "");

				// Set the attribute
				System.Xml.XmlAttribute addAttrib = xmlDoc.CreateAttribute("assembly");
				addAttrib.Value = assembly.FullName;

				// Add attribute node to add node
				addNode.Attributes.Append(addAttrib);

				// Add add node to assembly node
				assembliesNode.AppendChild(addNode);

				updated = true;
			}
			return updated;
		}

		// Handle all assemblies
		internal static bool AddAssemblies(XmlDocument xmlDoc) 
		{
			bool updated = false;
			XmlNode assembliesNode = xmlDoc.SelectSingleNode("//compilation/assemblies");
			if (assembliesNode == null) {
				assembliesNode = xmlDoc.CreateNode(XmlNodeType.Element, "assemblies", "");
			}

			AssemblyName coreEngineAssembly = (typeof(MapInfo.Mapping.Map)).Assembly.GetName();
			updated |= AddAssemblyNode(xmlDoc, assembliesNode, coreEngineAssembly);

			AssemblyName coreTypesAssembly = typeof(MapInfo.Geometry.DPoint).Assembly.GetName();
			updated |= AddAssemblyNode(xmlDoc, assembliesNode, coreTypesAssembly);

			// Now insert this information into compilation node in the file
			if (updated){
				System.Xml.XmlNodeList compNode = xmlDoc.GetElementsByTagName("compilation");
				compNode.Item(0).AppendChild(assembliesNode);
			}
			return updated;
		}

		internal static bool AddSessionState(XmlDocument xmlDoc)
		{
			bool updated = false;
			XmlNode sessStateNode = xmlDoc.SelectSingleNode("//sessionState");
			if (sessStateNode == null) {
				sessStateNode = xmlDoc.CreateNode(XmlNodeType.Element, "sessionState", "");

				System.Xml.XmlAttribute addAttrib1 = xmlDoc.CreateAttribute("mode");
				addAttrib1.Value = "StateServer";
				sessStateNode.Attributes.Append(addAttrib1);

				System.Xml.XmlAttribute addAttrib2 = xmlDoc.CreateAttribute("stateConnectionString");
				addAttrib2.Value = "tcpip=127.0.0.1:42424";
				sessStateNode.Attributes.Append(addAttrib2);

				System.Xml.XmlAttribute addAttrib3 = xmlDoc.CreateAttribute("sqlConnectionString");
				addAttrib3.Value = "data source=127.0.0.1;user	id=sa;password=";
				sessStateNode.Attributes.Append(addAttrib3);

				System.Xml.XmlAttribute addAttrib4 = xmlDoc.CreateAttribute("cookieless");
				addAttrib4.Value = "false";
				sessStateNode.Attributes.Append(addAttrib4);

				System.Xml.XmlAttribute addAttrib5 = xmlDoc.CreateAttribute("timeout");
				addAttrib5.Value = "20";
				sessStateNode.Attributes.Append(addAttrib5);
				
				System.Xml.XmlNodeList sysNode = xmlDoc.GetElementsByTagName("system.web");
				sysNode.Item(0).AppendChild(sessStateNode);

				updated = true;
			}

			return updated;
		}

		/// <summary>
		/// Loads the web.config file as XML document and returns it for updating.
		/// </summary>
		/// <param name="documentURL">URL of the web application.</param>
		/// <param name="webConfigPath">Return the path to the web.config file.</param>
		/// <returns>XML document pointing to web.config file.</returns>
		internal static XmlDocument GetWebConfig(string documentURL, out string webConfigPath)
		{
			DTE _dte = GetCurrentDTEObject();
			webConfigPath = "";
			XmlDocument xmlDoc = new XmlDocument();
			if (_dte != null) {
				Project currentProject = GetCurrentWebProject(_dte, documentURL);

				if (currentProject != null) {
					// Get pathname of the Web.Config file
           webConfigPath = currentProject.ProjectItems.Item("Web.Config").Properties.Item("FullPath").Value.ToString();

					XmlTextReader xmlReader = new XmlTextReader(webConfigPath);

					// Load into XML Dom
					xmlDoc.Load(xmlReader);
					xmlReader.Close();
				}
			}
			return xmlDoc;
		}
	}
}
