using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace MapInfo.WebControls
{
	/// <summary>
	/// Base class for all tools.
	/// </summary>
	/// <remarks><para>A tool is defined as an entity which does some interaction on the map. After it is finished, 
	/// a command is executed on the server side and the resulting new map is returned to the client. 
	/// Instead of following the regular architecture of web controls by doing a postback on each operation,
	/// which forces all controls on the page to redraw, these tools update the map by setting the source of 
	/// the map image to a URL which contains the command and data needed to perform the operation. The URL also contains the name 
	/// of the controller which handles the command. This controller parses the URL, gets the command and data, and performs the operation. This architecture has both server and client components.</para>
	/// <para><b>Client</b>: The client contains the javascript to perform the interaction on the map.
	/// The command to execute follows object oriented design. When tools are rendered, they use javascript to create the interaction, 
	/// tool, and command objects they need. When the tool is activated, event handlers are set to perform the interaction 
	/// on the map such as clicking or drawing a rectangle. When the interaction has finished,
	/// the Execute method of the command is invoked.  This sets the source of the map image to a URL containing the command and data.
	/// Then the request is then sent to server.</para>
	/// <para><b>Server</b>: The httphandler is called from the client with a URL. The httphandler on the server side is called by the 
	/// Controller. The Controller gets all parameters, commands, and data from the context and delegates the execution of the 
	/// command to the Model. The Model contains the actual code to perform the operation.  After the operation has completed, the map 
	/// is exported to an image and the image is streamed back to the client.</para>
	/// </remarks>
	[
	ToolboxData("<{0}:WebTool runat=server></{0}:WebTool>"),
	]
	public class WebTool : WebControl, IPostBackDataHandler
	{
		/// <summary>
		/// Enum for Client side interaction
		/// </summary>
		/// <remarks>This is the list of interactions that can be performed on the client side on the map.</remarks>
		public enum ClientInteractionEnum {
			/// <summary>
			/// No interaction is performed on the client side.
			/// </summary>
			/// <remarks>When there is no interaction done by tool, this may be used.</remarks>
			NullInteraction,
			/// <summary>
			/// Click interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>
			ClickInteraction,
			/// <summary>
			/// Rectangle interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>
			RectInteraction,
			/// <summary>
			/// Radius interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>
			RadInteraction,
			/// <summary>
			/// Drag interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>
			DragInteraction,
			/// <summary>
			/// Polyline interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>
			PolylineInteraction,
			/// <summary>
			/// Polygon interaction on the client side.
			/// </summary>
			/// <remarks>None</remarks>	
			PolygonInteraction
		}

		/// <summary>
		/// Enum for commands to be executed on the server side.
		/// </summary>
		/// <remarks>This name is used in the URL which is sent back.</remarks>
		public enum CommandEnum {
			/// <summary>
			/// Null command.
			/// </summary>
			/// <remarks>None</remarks>
			NullCommand,
			/// <summary>
			/// ZoomIn command
			/// </summary>
			/// <remarks>None</remarks>
			ZoomIn,
			/// <summary>
			/// ZoomOut command.
			/// </summary>
			/// <remarks>None</remarks>
			ZoomOut,
			/// <summary>
			/// ZoomWithFactor command.
			/// </summary>
			/// <remarks>None</remarks>
			ZoomWithFactor,
			/// <summary>
			/// ZoomToLevel command.
			/// </summary>
			/// <remarks>None</remarks>
			ZoomToLevel,
			/// <summary>
			/// Center command.
			/// </summary>
			/// <remarks>None</remarks>
			Center,
			/// <summary>
			/// Pan command.
			/// </summary>
			/// <remarks>None</remarks>
			Pan,
			/// <summary>
			/// Distance command.
			/// </summary>
			/// <remarks>None</remarks>
			Distance,
			/// <summary>
			/// Info command.
			/// </summary>
			/// <remarks>None</remarks>
			Info,
			/// <summary>
			/// PointSelection command.
			/// </summary>
			/// <remarks>None</remarks>
			PointSelection,
			/// <summary>
			/// RectangleSelection command.
			/// </summary>
			/// <remarks>None</remarks>
			RectangleSelection,
			/// <summary>
			/// RadiusSelection command.
			/// </summary>
			/// <remarks>None</remarks>
			RadiusSelection,
			/// <summary>
			/// PolygonSelection command.
			/// </summary>
			/// <remarks>None</remarks>
			PolygonSelection,
			/// <summary>
			/// Navigate command.
			/// </summary>
			/// <remarks>None</remarks>
			Navigate,
			/// <summary>
			/// LayerVisibility command.
			/// </summary>
			/// <remarks>None</remarks>
			LayerVisibility
		}

		/// <summary>
		/// Enum for names of the command objects that exist on the client side.
		/// </summary>
		/// <remarks>These command objects create a URL based on the name and interaction, create URLs with commands and data, sends to the server,
		///  gets a response back, and applies it appropriately.
		/// </remarks>
		public enum ClientCommandEnum 
		{
			/// <summary>
			/// MapComand
			/// </summary>
			/// <remarks>This command is responsible to update the map. It forms the URL and sets the map image's source to the URL which updates the map.</remarks>
			MapCommand,
			/// <summary>
			/// PanCommand
			/// </summary>
			/// <remarks>Then Pan command is almost same as the MapCommand, except since the user has dragged the image, it resets the image to the proper location.</remarks>
			PanCommand,
			/// <summary>
			/// DistanceCommand
			/// </summary>
			/// <remarks>The Distance command uses XmlHttp object to send a request to the server and get a response back to display the distance.</remarks>
			DistanceCommand,
			/// <summary>
			/// NavigateCommand
			/// </summary>
			/// <remarks>The Navigate command is same as the MapComand, except there are additional parameters added to the URL. This command is used by the Navigation tools.</remarks>
			NavigateCommand,
			/// <summary>
			/// PointSelectionCommand
			/// </summary>
			/// <remarks>The PointSelection command is same as the MapComand, except a pixel tolerance is added to the URL.</remarks>
			PointSelectionCommand,
			/// <summary>
			/// ZoomCommand
			/// </summary>
			/// <remarks>The ZoomCommand is same as the MapCommand, except a zoom factor is added to the URL. This is used by the ZoomBar tools.</remarks>
			ZoomCommand
		}

		/// <summary>
		/// Constructor for a tool.
		/// </summary>
		/// <remarks>Tools set appropriate properties to suit their needs using this constuctor.</remarks>
		public WebTool() {
			Active = false;
			ClientInteraction = ClientInteractionEnum.NullInteraction.ToString();
			ClientCommand = ClientCommandEnum.MapCommand.ToString();
			Command = CommandEnum.NullCommand.ToString();
			ResourcesPath = Resources.ResourceFolder;
			InactiveImageUrl =  string.Format("{0}/{1}ControlInactive.gif", ResourcesPath, GetType().Name);
			ActiveImageUrl = string.Format("{0}/{1}ControlActive.gif", ResourcesPath, GetType().Name);
		}

		/// <summary>
		/// Name of the command to be executed.
		/// </summary>
		/// <remarks>This is sent by the client to the server in the URL. On the server side, it is parsed from the URL and an appropriate command object is used to execute and send back a response.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		TypeConverter(typeof(CommandListConverter))
		]
		public virtual string Command {
			get {
				return (string)ViewState["Command"];
			}
			set {ViewState["Command"] = value;}
		}

		/// <summary>
		/// Name of the command used on the client side.
		/// </summary>
		/// <remarks>The javascript client side command is made up of the interaction and the name of the command to be sent to the server.
		/// This command creates the appropriate URL and sends it to the server to get back a response.
		/// This allows the use of custom commands which you may have written.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		TypeConverter(typeof(ClientCommandListConverter))
		]
		public virtual string ClientCommand {
			get {
				return (string)ViewState["ClientCommand"];
			}
			set {ViewState["ClientCommand"] = value;}
		}

		/// <summary>
		/// Name of the interaction to be performed on the client side.
		/// </summary>
		/// <remarks>The javascript code for performing various interactions on the map. 
		/// This allows users to pick the interaction for a custom tool. For known tools like ZoomIn or PointSelection, 
		/// this property is read only. </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		TypeConverter(typeof(InteractionListConverter))
		]
		public virtual string ClientInteraction {
			get {
				return (string)ViewState["ClientInteraction"];
			}
			set {ViewState["ClientInteraction"] = value;}
		}

		/// <summary>
		/// URL pointing to the root of all resources used by the MapXtreme web controls.
		/// </summary>
		/// <remarks>This root can be changed to point to your own root instead of the default MapXtremeWebResources xx_xx root.
		/// <para>
		/// There are three files Interaction.js, Command.js, and Tool.js that are used by the MapXtreme web controls. These script statements are rendered at 
		/// runtime. This root is used in the path. To customize the scripts, you can either add them to these files, or if there are additional files
		/// to render you can manually put script statements in the webform.
		/// </para>
		///  </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		Editor(typeof(UrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public string ResourcesPath{
			get {
				return (string)ViewState["ResourcesPath"];
			}
			set {ViewState["ResourcesPath"] = value;}
		}

		/// <summary>
		/// ID of the mapcontrol used by the tool.
		/// </summary>
		/// <remarks>This parameter tells the tool which map to use on the client side. 
		/// The MapControl renders the IMG tag which is used by the tool to perform the interaction.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		TypeConverter(typeof(ServerControlConverter))
		]
		public string MapControlID {
			get {
				return (string)ViewState["MapControlID"];
			}
			set{ViewState["MapControlID"] = value;}
		}

		/// <summary>Gets or sets the URL for an image when the tool control is not selected.</summary>
		/// <value>Gets or sets the URL for the image when ToolControl is not selected.</value>
		/// <remarks>This property gets or sets the URL for an image (including the path) when the ToolControl is not selected.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		Editor(typeof(UrlEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public string InactiveImageUrl {
			get {
					return (string)ViewState["InactiveImageUrl"];
				}
			set {
				ViewState["InactiveImageUrl"] = value;
			}
		}

		/// <summary>Gets or sets the URL for an image when the tool control is selected.</summary>
		/// <value>Gets or sets the URL for image when ToolControl is selected.</value>
		/// <remarks>This property gets or sets the URL for an image when the tool is selected. 
		/// This URL address includes the full path.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		Editor(typeof(UrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public string ActiveImageUrl {
			get {
					return (string)ViewState["ActiveImageUrl"];
			}
			set {
				ViewState["ActiveImageUrl"] = value;
			}
		}

		/// <summary>Gets or sets the URL for the cursor which is set when the ToolControl is selected.</summary>
		/// <value>Gets or sets the URL for the cursor, which is set when the ToolControl
		/// is selected (the cursor hovers over the MapControl).</value>
		/// <remarks>The file type should be a cursor file. The cursor file will not work when using Netscape or FireFox browsers.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		Editor(typeof(UrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public string CursorImageUrl {
			get {
				return (string)ViewState["CursorImageUrl"];
			}
			set {
				ViewState["CursorImageUrl"] = value;
			}
		}

		/// <summary>Gets or sets the boolean that determines whether or not the tool is active.</summary>
		/// <value>Gets or sets the boolean to indicate whether the tool is active or not.</value>
		/// <remarks>In the render method, if this flag is <c>true</c> then the tool is rendered as active.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public bool Active {
			get {
				return (bool)ViewState["Active"];
			}
			set {
				ViewState["Active"] = value;
			}
		}

		/// <summary>
		/// Renders all javascript needed.
		/// </summary>
		/// <param name="e">EventArgs</param>
		/// <remarks>None</remarks>
		protected override void OnLoad(EventArgs e) {
			string codeName = "Interaction.js";
			if(!Page.IsClientScriptBlockRegistered(codeName)) {
				// Register the client code:
				string format = "\n<script language=\"javascript\"   type=\"text/javascript\" src=\"{0}\"></script>";
				Page.RegisterClientScriptBlock(codeName, string.Format(format, ResourcesPath +"/"+codeName));
			}
			codeName = "Command.js";
			if(!Page.IsClientScriptBlockRegistered(codeName)) {
				// Register the client code:
				string format = "\n<script language=\"javascript\"   type=\"text/javascript\" src=\"{0}\"></script>";
				Page.RegisterClientScriptBlock(codeName, string.Format(format, ResourcesPath +"/"+codeName));
			}
			codeName = "Tool.js";
			if(!Page.IsClientScriptBlockRegistered(codeName)) {
				// Register the client code:
				string format = "\n<script language=\"javascript\"   type=\"text/javascript\" src=\"{0}\"></script>";
				Page.RegisterClientScriptBlock(codeName, string.Format(format, ResourcesPath +"/"+codeName));
			}
		}

		/// <summary>
		/// Enables the postback mechanism.
		/// </summary>
		/// <param name="e">EventArgs</param>
		/// <remarks>The tools do not perform postback after operations. However, applications may perform postback. In these cases, if the tool was
		/// active before postback, it must be active after the page is rendered.</remarks>
		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender (e);
			Page.RegisterRequiresPostBack(this);
		}

		/// <summary>
		/// Renders javascript to create interaction, command, and tool objects to be used for operations on the client side.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <remarks>None</remarks>
		protected virtual void RenderJS(HtmlTextWriter writer) {
			// Since this is a runtime behavior, you only need to use this if there is context.
			RenderJS(writer, ClientCommand, "ImgTool");
		}

		/// <summary>
		/// Renders javascript to create interaction, command, and tool objects to be used for operations on the client side.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <param name="clientCommandName">Command name to be used on the client side.</param>
		/// <param name="clientToolName">Tool name to be used on the client side.</param>
		/// <remarks>None</remarks>
		protected void RenderJS(HtmlTextWriter writer, string clientCommandName, string clientToolName)
		{
			if (Context != null) {
				writer.AddAttribute("language", "javascript");
				writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
				writer.RenderBeginTag(HtmlTextWriterTag.Script);
				writer.WriteLine(string.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID));
				writer.WriteLine(string.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, clientCommandName, Command));
				if (clientToolName != null) writer.WriteLine(string.Format("var {0}Tool = new {1}('{0}', {0}Int, {0}Cmd);", UniqueID, clientToolName));
				writer.WriteLine(string.Format("{0}Tool.activeImg = \"{1}\";", UniqueID, ActiveImageUrl));
				writer.WriteLine(string.Format("{0}Tool.inactiveImg = \"{1}\";", UniqueID, InactiveImageUrl));
				writer.WriteLine(string.Format("{0}Tool.cursorImg = \"{1}\";", UniqueID, CursorImageUrl));
				writer.RenderEndTag();
			}
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <remarks>Renders the tool as an image tag appropriate javascript.</remarks>
		protected override void RenderContents(HtmlTextWriter writer) {
			if (Page !=null) {
				Page.VerifyRenderingInServerForm(this);
			}

			// Render HTML
			writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, InactiveImageUrl);
			writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			// Write a hidden variable to store tool's state whether it is active or not
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID+"Active");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID+"Active");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();

			// Render javascript
			RenderJS(writer);

			// Activate the tool if active
			if (Context != null) {
				bool active = false;
				if ((Context.Session[UniqueID+"Active"]  != null) && ((int)Context.Session[UniqueID+"Active"] == 1)) {
					active = true;	
				}
				if (Active || active) {
					string activateCode = String.Format("{0}Tool.Activate();", UniqueID);
					string activateScript = String.Format("<script type='text/javascript'>AppendScriptToForm('{0}');</script>", activateCode);
					Page.RegisterStartupScript("Key1", activateScript);
				}
			}
		}
		#region IPostBackDataHandler Members

		/// <summary>
		/// NoOp
		/// </summary>
		/// <remarks>None</remarks>
		public void RaisePostDataChangedEvent() {
			// TODO:  Add WebTool.RaisePostDataChangedEvent implementation
		}

		/// <summary>
		/// Sets the state of the tool after postback
		/// </summary>
		/// <remarks>The tools themselves don't do postback but if tool is activated and the code in the application does postback.
		/// when the page gets rendered we need to render the tool as active because it was.
		/// </remarks>
		/// <param name="postDataKey">Key for identifying whether it is command we need to process</param>
		/// <param name="postCollection">Collection of all form values</param>
		/// <returns>Returns <c>false</c>.</returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			// Set the flag if this tool is active
			string value = postCollection[UniqueID+"Active"];
			if ( (value != null) && (value.IndexOf(UniqueID) >= 0 ) && (value.IndexOf(MapControlID) >= 0)) {
				Context.Session[UniqueID+"Active"] = 1;
			} else {
				Context.Session[UniqueID+"Active"] = 0;
			}
			return false;
		}

		#endregion
	}
}
