using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Web.UI.Design;
using System.Web.UI;
using System.Xml;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This type converter allows users at design time to display list of objects in a drop down list and allow them to pick one of them or enter a new one.
	/// </summary>
	/// <remarks>When this typeconverter is associated with a property, a list of objects can be displayed in dropdown list and users can either pick
	/// one of them or enter a new one. The example is MapControlID property lists ID's of all MapControl on the form in the property.
	/// </remarks>
	public class ListConverter : StringConverter 
	{
		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list.
		/// </summary>
		/// <remarks>See the base class TypeConverter for more information</remarks>
		/// <param name="context">context</param>
		/// <returns>true</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
			return true;
		}
		/// <summary>
		/// Returns whether the collection of standard values returned from GetStandardValues is an exclusive list.
		/// </summary>
		/// <remarks>See the base class TypeConverter for more information</remarks>
		/// <param name="context">context</param>
		/// <returns>false</returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
			return false;
		}
		/// <summary>
		/// Returns a collection of all strings
		/// </summary>
		/// <param name="context">Context</param>
		/// <returns>collection of all IDs</returns>
		/// <remarks>None</remarks>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
			if ((context == null) || (context.Container == null)) {
				return null; 
			}
			Object[] serverControls = this.GetList(context.Container);
			if (serverControls != null) {
				return new StandardValuesCollection(serverControls); 
			}
			return null; 
		}
		/// <summary>
		/// Virtual method for getting a list of objects.
		/// </summary>
		/// <remarks>This method is overridden and actual implementation is written.</remarks>
		/// <param name="container">container which is page.</param>
		/// <returns>Array of objects</returns>
		protected virtual  object[] GetList(IContainer container){
			return null;
		}
	}

	/// <summary>
	/// This class is used to browse through all MapControls on the same form at design time.
	/// </summary>
	/// <remarks>At design time when the property is clicked on, a dropdown box containing all of the MapControl IDs is displayed, 
	/// from which users can choose one.the MapControlID has to be set for the controls that depend upon it, 
	/// </remarks>
	public class ServerControlConverter : ListConverter 
	{
		/// <summary>
		/// Return list of MapControl ID's on the page.
		/// </summary>
		/// <remarks>This method goes through the Page and adds ID's of controls of type MapControl to the array.</remarks>
		/// <param name="container">Container which is Page</param>
		/// <returns>Array of MapControl ID's</returns>
		protected override object[] GetList(IContainer container) {
			ArrayList availableControls = new ArrayList();
			foreach( IComponent component in container.Components ) {
				Control serverControl = component as Control;
				MapControl m = serverControl as MapControl;
				if ( serverControl != null && !(serverControl is Page) && serverControl.ID != null && serverControl.ID.Length != 0  && 
					IncludeControl(serverControl)) {
					availableControls.Add(serverControl.ID);
				}
			}
			availableControls.Sort(Comparer.Default);
			return availableControls.ToArray(); 
		}

		/// <summary>
		/// Whether to include this control in the list
		/// </summary>
		/// <remarks>If the control is of type MapControl then it returns true or else false.</remarks>
		/// <param name="serverControl">server control from page</param>
		/// <returns>Boolean indicating whether to include this control to the list</returns>
		protected virtual Boolean IncludeControl( Control serverControl ) {
			if (serverControl is MapInfo.WebControls.MapControl) {
				return true;
			} else {
				return false;
			}
		}
	}

		/// <summary>
		/// This class is the type converter for the commands
		/// </summary>
		/// <remarks>At design time when the property is clicked on, a dropdown box containing all the commands is displayed, 
		/// from which users can choose one or enter a different command.
		/// </remarks>
		public class CommandListConverter : ListConverter
		{
			/// <summary>
			/// This method returns list of commands
			/// </summary>
			/// <remarks>This method goes through all commands and returns array of names.</remarks>
			/// <param name="container">Container which is the page but not used.</param>
			/// <returns>List of all commands in an array.</returns>
			protected override object[] GetList(IContainer container) {
				ArrayList availableCommands = new ArrayList();
				foreach(object o in WebTool.CommandEnum.GetValues(typeof(WebTool.CommandEnum))) {
					availableCommands.Add(o.ToString());
				}
				return availableCommands.ToArray(); 
			}
		}

	/// <summary>
	/// This class is the type converter for the interactions.
	/// </summary>
	/// <remarks>At design time when the property is clicked on, a dropdown box containing all the interactions  is displayed, 
	/// from which users can choose one or enter a different interaction.
	/// </remarks>
	public class InteractionListConverter : ListConverter {
		/// <summary>
		/// This method returns list of interactions
		/// </summary>
		/// <remarks>This method goes through all interactions and returns array of names.</remarks>
		/// <param name="container">Container which is the page but not used.</param>
		/// <returns>List of all interactions in an array.</returns>
		protected override object[] GetList(IContainer container) {
			ArrayList availableCommands = new ArrayList();
			foreach(object o in WebTool.ClientInteractionEnum.GetValues(typeof(WebTool.ClientInteractionEnum))) {
				availableCommands.Add(o.ToString());
			}
			return availableCommands.ToArray(); 
		}
	}

	/// <summary>
	/// This class is the type converter for the client side commands.
	/// </summary>
	/// <remarks>At design time when the property is clicked on, a dropdown box containing all the client side commands is displayed, 
	/// from which users can choose one or enter a different client side command.
	/// </remarks>
	public class ClientCommandListConverter : ListConverter {
		/// <summary>
		/// This method returns list of client side commands
		/// </summary>
		/// <remarks>This method goes through all client side commands and returns array of names.</remarks>
		/// <param name="container">Container which is the page but not used.</param>
		/// <returns>List of all client side commands in an array.</returns>
		protected override object[] GetList(IContainer container) {
			ArrayList availableCommands = new ArrayList();
			foreach(object o in WebTool.ClientCommandEnum.GetValues(typeof(WebTool.ClientCommandEnum))) {
				availableCommands.Add(o.ToString());
			}
			return availableCommands.ToArray(); 
		}
	}

	/// <summary>
	/// Designer for LayerControl
	/// </summary>
	/// <remarks>This designer doesn't have lot of functionality, it just displays message indicating that the MapControlID is to be set. It also
	/// updates web.config file with information about MapXtreme session activator and httphandler.</remarks>
	internal class LayerControlDesigner : ControlDesigner
	{
		private LayerControl _layerControl;

		///	<summary>Updates the web.config file with information about MapXtreme assemblies and handlers.</summary>
		///	<remarks>The information added to the web.config file is the httphandler, session activator settings and appsetting lines if they don't
		///	exist (these lines contain pooling, preloaded workspaces etc.).
		///	</remarks>
		/// <param name="documentURL">Url of the form.</param>
		public static void UpdateForLayerControl(string documentURL) {
			// Open the web.config file as xmldoc
			string webConfigPath;
			XmlDocument xmlDoc = WebDesignerUtility.GetWebConfig(documentURL, out webConfigPath);

			// Add httphandler for mapcontrol
			bool updated = false;
			updated |= WebDesignerUtility.AddController(xmlDoc, "LayerController.ashx", "MapInfo.WebControls.LayerController");
			updated |= WebDesignerUtility.AddAppSettings(xmlDoc);
			updated |= WebDesignerUtility.AddHttpModules(xmlDoc);

			if (updated) xmlDoc.Save(webConfigPath);
		}

		/// <summary>
		/// Initializes the designer
		/// </summary>
		/// <param name="component">Component for which this designer is for.</param>
		/// <remarks>It sets up all parameters for the designer and updates web.config file.</remarks>
		public override void Initialize(IComponent component)		
		{
			_layerControl  = component as LayerControl;
			IDesignerHost host = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			IWebFormsDocumentService docService = (IWebFormsDocumentService)component.Site.GetService(typeof(IWebFormsDocumentService));
			string documentUrl = docService.DocumentUrl;
			UpdateForLayerControl(documentUrl);
			base.Initialize(component);
		}

		/// <summary>
		/// Writes the design time HTML for MapControl. 
		/// </summary>
		/// <remarks>At design time no map is displayed, only a brief message is shoen on the control, indicating what it does at run time.
		/// </remarks>
		/// <returns>Returns a String containing HTML.</returns>
		public override string GetDesignTimeHtml() 
		{
			StringWriter text = new StringWriter();
			HtmlTextWriter writer = new HtmlTextWriter(text);
			writer.AddAttribute("style", string.Format("border-top:solid 1px black; border-left:solid 1px black; border-bottom:solid 1px black; border-right:solid 1px black; width: {0}px; height: {1}px;", _layerControl.Width.Value, _layerControl.Height.Value));
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine(L10NUtils.Resources.GetString("LCDesignString"));
			writer.RenderEndTag();
			return text.ToString();
		}
	}
}
