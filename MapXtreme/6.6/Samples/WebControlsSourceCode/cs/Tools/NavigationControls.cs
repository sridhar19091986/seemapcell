using System;
using System.Drawing;
using System.IO;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This tool pans the map by fixed values provided by the user. This class is the base class for navigation. The navigation tool performs
	/// specific pans such as north or south and provide appropriate offsets.
	/// </summary>
	/// <remarks>
	/// This tool behaves differently from tools like zoom-in. When the mouse hovers over the tool or goes outside the tool 
	///  it uses a different image to give the user the impression of being active or not active. There is no interaction on the map. A click on the tool pans the map 
	///  by the provided values. The pan can be either done by map units or by percentage of the size of the map on the screen.
	/// </remarks>
	public class NavigationTool : WebTool
	{
		/// <summary>
		/// Offset method to pan the map.
		/// </summary>
		/// <remarks>Either by units or by percentage of the screen.</remarks>
		public enum OffsetMethodEnum
		{
			/// <summary>
			/// Pan by unit.
			/// </summary>
			/// <remarks>The map's current unit is used.</remarks>
			ByUnit = 0,
			/// <summary>
			/// Pan by percentage.
			/// </summary>
			/// <remarks>Percentage of the size of the map image.</remarks>
			ByPercentage = 1
		}

		/// <summary>
		/// Constructor for the Navigation tool.
		/// </summary>
		/// <remarks>Various default values are set.</remarks>
		public NavigationTool()
		{
			InactiveImageUrl =  string.Format("{0}/{1}Inactive.gif", Resources.ResourceFolder, GetType().Name);
			ActiveImageUrl = string.Format("{0}/{1}Active.gif", Resources.ResourceFolder, GetType().Name);
			OffsetMethod = OffsetMethodEnum.ByPercentage;
			Active = false;
			Command = CommandEnum.Navigate.ToString();
			ClientInteraction = ClientInteractionEnum.NullInteraction.ToString();
			ClientCommand = ClientCommandEnum.NavigateCommand.ToString();
		}

		/// <summary>
		/// This property is not used for this control.
		/// </summary>
		/// <remarks>Since there is no activation or deactivation of the tool, this property is not to be used.</remarks>
		[
		Browsable(false)
		]
		public new bool Active {
			get {
				return (bool)ViewState["Active"];
			}
			set {
				ViewState["Active"] = value;
			}
		}

		/// <summary>
		/// Offset used to pan East.
		/// </summary>
		/// <remarks>The map's current units are used.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public double EastOffset {
			get {
				return (double)ViewState["EastOffset"];
			}
			set {
				ViewState["EastOffset"] = value;
			}
		}

		/// <summary>
		/// Offset used to pan North.
		/// </summary>
		/// <remarks>The map's current units are used.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public double NorthOffset
		{
			get {
				return (double)ViewState["NorthOffset"];
			}
			set {
				ViewState["NorthOffset"] = value;
			}
		}

		/// <summary>
		/// Percentage of the map to perform a pan.
		/// </summary>
		/// <remarks>When 10.0 is used, the map is panned by 10%.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public double Percent {
			get {
				return (double)ViewState["Percent"];
			}
			set {
				ViewState["Percent"] = value;
			}
		}

		/// <summary>
		/// Method used to pan the map.
		/// </summary>
		/// <remarks>Either by the map's current units or by a percentage of the size of the map.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public OffsetMethodEnum OffsetMethod{
			get {
				return (OffsetMethodEnum)ViewState["OffsetMethod"];
			}
			set {
				ViewState["OffsetMethod"] = value;
			}
		}

		/// <summary>
		/// Renders the control. 
		/// </summary>
		/// <param name="writer">The HTML writer to write out to.</param>
		/// <remarks>This method renders the image tag with onmouseover and onmouseout events in javascript to the page.</remarks>
		protected override void RenderContents(HtmlTextWriter writer) {
			if (Page !=null) {
				Page.VerifyRenderingInServerForm(this);
			}
			// Render HTML
			writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, InactiveImageUrl);
			writer.AddAttribute("OnMouseOver", string.Format("javascript:this.src='{0}';", ActiveImageUrl));
			writer.AddAttribute("OnMouseOut", string.Format("javascript:this.src='{0}';", InactiveImageUrl));
			ToolTip = L10NUtils.Resources.GetString("NavigationMapToolHelpText");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			RenderJS(writer);
		}

		/// <summary>
		/// Renders the javascript to create command and event handlers.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <remarks>This control follows the same architecture as the tools. There is no interaction on the map, so it uses different command.</remarks>
		protected override void RenderJS(HtmlTextWriter writer)
		{
			writer.AddAttribute("language", "javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);

			writer.WriteLine(string.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID));
			writer.WriteLine(string.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, ClientCommand, Command));
			writer.WriteLine(string.Format("var {0}Me = FindElement('{1}_Image');", UniqueID,  UniqueID));
			writer.WriteLine(string.Format("{0}Cmd.method = '{1}';", UniqueID, OffsetMethod.ToString()));
			writer.WriteLine(string.Format("{0}Me.onclick = {0}Cmd.Exc;", UniqueID));
			writer.RenderEndTag();
		}

		/// <summary>
		/// Render javascript to set offset parameters.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <param name="percentEast">Percent of the screen to be moved East.</param>
		/// <param name="percentNorth">Percent of the screen to be moved North.</param>
		/// <param name="eastOffset">Offset in the map's units to be moved East.</param>
		/// <param name="northOffset">Offset in the map's units to be moved North.</param>
		/// <remarks>All offsets are set, then depending upon the method chosen, the appropriate offsets are used from the parameter list. For example, when the 
		/// method is ByPercentage, percent offsets are used.</remarks>
		protected virtual void RenderOffsetJS(HtmlTextWriter writer, double percentEast, double percentNorth, double eastOffset, double northOffset)
		{
			writer.AddAttribute("language", "javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			if (OffsetMethod == OffsetMethodEnum.ByPercentage) {
				writer.WriteLine(string.Format("{0}Cmd.east = {1};", UniqueID, percentEast));
				writer.WriteLine(string.Format("{0}Cmd.north = {1};", UniqueID, percentNorth));
			} else if (OffsetMethod == OffsetMethodEnum.ByUnit) {
				writer.WriteLine(string.Format("{0}Cmd.east = {1};", UniqueID, eastOffset));
				writer.WriteLine(string.Format("{0}Cmd.north = {1};", UniqueID, northOffset));
			}
			writer.RenderEndTag();
		}
	}

	/// <summary>
	/// This tool allows users to navigate East.
	/// </summary>
	/// <remarks>When navigating East, the map is panned West so that more map information to the East is visible.</remarks>
	[
	ToolboxData("<{0}:EastNavigationTool runat=server></{0}:EastNavigationTool>")
	]
	public class EastNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the East Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public EastNavigationTool()
		{
			Percent = 10.0;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to the East navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, -Percent, 0.0, -EastOffset, 0.0);
		}
	}

	/// <summary>
	/// This tool allows users to navigate West.
	/// </summary>
	/// <remarks>When navigating West, the map is panned to East so that more map infomation to the West is visible.</remarks>
	[
	ToolboxData("<{0}:WestNavigationTool runat=server></{0}:WestNavigationTool>")
	]
	public class WestNavigationTool: NavigationTool{
		/// <summary>
		/// Constructor for the West Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public WestNavigationTool()
		{
			Percent = 10.0;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to West navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, Percent, 0.0, EastOffset, 0.0);
		}
	}

	/// <summary>
	/// This tool allows users to navigate North
	/// </summary>
	/// <remarks>When navigating North, the map is panned South so that more map information to the North is visible.</remarks>
	[
	ToolboxData("<{0}:NorthNavigationTool runat=server></{0}:NorthNavigationTool>")
	]
	public class NorthNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the North Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public NorthNavigationTool() {
			Percent = 10.0;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to North navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, 0.0, Percent, 0.0, NorthOffset);
		}
	}

	/// <summary>
	/// This tool allows users to navigate South.
	/// </summary>
	/// <remarks>When navigating South, the map is panned North so that more map information to the South is visible.</remarks>
	[
	ToolboxData("<{0}:SouthNavigationTool runat=server></{0}:SouthNavigationTool>")
	]
	public class SouthNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the South Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public SouthNavigationTool() {
			Percent = 10.0;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to South navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, 0.0, -Percent, 0.0, -NorthOffset);
		}
	}

	/// <summary>
	/// This tool allows users to navigate SouthEast.
	/// </summary>
	/// <remarks>When navigating SouthEast, the map is panned NorthWest so that more map information to the SouthEast is visible.</remarks>
	[
	ToolboxData("<{0}:SouthEastNavigationTool runat=server></{0}:SouthEastNavigationTool>")
	]
	public class SouthEastNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the SouthEast Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public SouthEastNavigationTool()
		{
			Percent = 10;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to SouthEast navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, -Percent, -Percent, -EastOffset, -NorthOffset);
		}
	}

	/// <summary>
	/// This tool allows users to navigate SouthWest.
	/// </summary>
	/// <remarks>When navigating SouthWest, the map is panned NorthEast so that more map information to the SouthWest is visible.</remarks>
	[
	ToolboxData("<{0}:SouthWestNavigationTool runat=server></{0}:SouthWestNavigationTool>")
	]
	public class SouthWestNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the SouthWest Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public SouthWestNavigationTool()
		{
			Percent = 10;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to the SouthWest navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, Percent, -Percent, EastOffset, -NorthOffset);
		}
	}

	/// <summary>
	/// This tool allows users to navigate NorthEast.
	/// </summary>
	/// <remarks>When navigating NorthEast, the map is panned SouthWest so that more map information to the NorthEast is visible.</remarks>
	[
	ToolboxData("<{0}:NorthEastNavigationTool runat=server></{0}:NorthEastNavigationTool>")
	]
	public class NorthEastNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the NorthEast Navigation tool.
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public NorthEastNavigationTool() {
			Percent = 10;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to the NorthEast navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, -Percent, Percent, -EastOffset, NorthOffset);
		}
	}

	/// <summary>
	/// This tool allows users to navigate NorthWest
	/// </summary>
	/// <remarks>When navigating NorthWest, the map is panned SouthEast so that more map information to the NorthWest is visible.</remarks>
	[
	ToolboxData("<{0}:NorthWestNavigationTool runat=server></{0}:NorthWestNavigationTool>")
	]
	public class NorthWestNavigationTool: NavigationTool{
		/// <summary>
		/// Constuctor for the NorthWest Navigation tool
		/// </summary>
		/// <remarks>Default values for the offsets are set.</remarks>
		public NorthWestNavigationTool() {
			Percent = 10;
			EastOffset = 0.0;
			NorthOffset = 0.0;
		}

		/// <summary>
		/// Render javascript specific to the NorthWest navigation tool.
		/// </summary>
		/// <remarks>Calls the base methods to render javascript with appropriate offsets.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer){
			base.RenderJS(writer);
			base.RenderOffsetJS(writer, Percent, Percent, EastOffset, NorthOffset);
		}
	}
}
