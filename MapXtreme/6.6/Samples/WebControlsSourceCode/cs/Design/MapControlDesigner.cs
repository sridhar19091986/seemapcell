using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
	/// Designer for MapControl.
	/// </summary>
	/// <remarks>This designer doesn't have lot of functionality, it just displays a brief  message about the usage. It also
	/// updates web.config file with information about MapXtreme session activator and httphandler.</remarks>
	public class MapControlDesigner : ControlDesigner
	{
		private MapControl _mapControl;

		///	<summary>Updates the web.config file with information about MapXtreme assemblies and handlers.</summary>
		///	<remarks>The information added to the web.config file is the httphandler, session activator settings and appsetting lines if they don't
		///	exist (these lines contain pooling, preloaded workspaces etc.).
		///	</remarks>
		/// <param name="documentURL">Url of the form</param>
		public static void UpdateForMapControl(string documentURL) {
			// Open the web.config file as xmldoc
			string webConfigPath;
			XmlDocument xmlDoc = WebDesignerUtility.GetWebConfig(documentURL, out webConfigPath);
	
			// Add httphandler for mapcontrol
			bool updated = false;
			updated |= WebDesignerUtility.AddController(xmlDoc, "MapController.ashx", "MapInfo.WebControls.MapController");
			updated |= WebDesignerUtility.AddAppSettings(xmlDoc);
			updated |= WebDesignerUtility.AddHttpModules(xmlDoc);
			updated |= WebDesignerUtility.AddAssemblies(xmlDoc);
			updated |= WebDesignerUtility.AddSessionState(xmlDoc);

			if (updated) xmlDoc.Save(webConfigPath);
		}

		/// <summary>
		/// Initializes the designer.
		/// </summary>
		/// <param name="component">Component for which this designer is for.</param>
		/// <remarks>It sets up all parameters for the designer and updates web.config file.</remarks>
		public override void Initialize(IComponent component) 
		{
			_mapControl = component as MapControl;
			IDesignerHost host = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			IWebFormsDocumentService docService = (IWebFormsDocumentService)component.Site.GetService(typeof(IWebFormsDocumentService));
			string documentUrl = docService.DocumentUrl;
			UpdateForMapControl(documentUrl);
			base.Initialize(component);
		}

		/// <summary>
		/// Serialize the width and height of the mapcontrol
		/// </summary>
		/// <remarks>Serialize the width and height of the mapcontrol into the web form aspx file.</remarks>
		protected override void OnBehaviorAttached() {
			base.OnBehaviorAttached ();
			Behavior.SetAttribute("Width", _mapControl.Width.Value, false);
			Behavior.SetAttribute("Height", _mapControl.Height.Value, false);
		}

		/// <summary>
		/// Writes the design time HTML for MapControl. 
		/// </summary>
		/// <remarks>At design time no map is displayed, only a brief message indicating what it does at run time is visible 
		/// on the control.</remarks>
		/// <returns>string containing HTML</returns>
		public override string GetDesignTimeHtml() 
		{
			StringWriter text = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(text);
			writer.AddAttribute("style", string.Format("border-top:solid 1px black; border-left:solid 1px black; border-bottom:solid 1px black; border-right:solid 1px black; width: {0}px; height: {1}px;", _mapControl.Width.Value, _mapControl.Height.Value));
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine(L10NUtils.Resources.GetString("MCDesignString"));
			writer.RenderEndTag();
			return text.ToString();
		}
	}
}
