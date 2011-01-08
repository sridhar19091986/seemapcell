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
	/// This tool allows you to zoom to a given fixed level.
	/// </summary>
	/// <remarks>
	/// This tool behaves differently from other tools like zoom-in. When the mouse hovers over the tool or goes outside the tool 
	///  it uses a different image to give users the impression of being active or not active. There is no interaction on the map. A click on the tool zooms the map 
	///  to level provided. The zoom level is in the map's current units.
	///  This control can be part of a zoom bar which allows users to zoom to various pre-set levels. You can drop multiple zoom bar tools and set
	///  different levels for each.
	/// </remarks>
	[
	ToolboxData("<{0}:ZoomBarTool runat=server></{0}:ZoomBarTool>")
	]
	public class ZoomBarTool : WebTool
	{
		/// <summary>
		/// Constructor for the tool.
		/// </summary>
		/// <remarks>
		/// Sets default values for parameters.
		/// </remarks>
		public ZoomBarTool()
		{
			ZoomLevel = 0.0;
			InactiveImageUrl =  string.Format("{0}/{1}Inactive.gif", Resources.ResourceFolder, GetType().Name);
			ActiveImageUrl = string.Format("{0}/{1}Active.gif", Resources.ResourceFolder, GetType().Name);
			Active = false;
			Command = CommandEnum.ZoomToLevel.ToString();
			ClientInteraction = ClientInteractionEnum.NullInteraction.ToString();
			ClientCommand = ClientCommandEnum.ZoomCommand.ToString();
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
		/// Zoom level which the map should be zoomed.
		/// </summary>
		/// <remarks>The value is provided in the map's exisiting units.</remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public double ZoomLevel {
			get {
				return (double)ViewState["ZoomLevel"];
			}
			set {
				ViewState["ZoomLevel"] = value;
			}
		}

		/// <summary>
		/// Renders the control.
		/// </summary>
		/// <remarks>This method renders the image and events for the tool. This method writes javascript code to create a client side command.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderContents(HtmlTextWriter writer) {
			if (Page !=null) {
				Page.VerifyRenderingInServerForm(this);
			}

			// Render HTML
			writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, InactiveImageUrl);
			writer.AddAttribute("OnMouseOver", string.Format("javascript:this.src='{0}';", ActiveImageUrl));
			writer.AddAttribute("OnMouseOut", string.Format("javascript:this.src='{0}';", InactiveImageUrl));
			ToolTip = L10NUtils.Resources.GetString("ZoomBarMapToolHelpText");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip);
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();

			writer.AddAttribute("language", "javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(string.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID));
			writer.WriteLine(string.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, ClientCommand, Command));
			writer.WriteLine(string.Format("var {0}Me = FindElement('{1}_Image');", UniqueID,  UniqueID));
			writer.WriteLine(string.Format("{0}Cmd.zoomLevel = {1};", UniqueID, ZoomLevel.ToString()));
			writer.WriteLine(string.Format("{0}Me.onclick = {0}Cmd.Exc;", UniqueID));
			writer.RenderEndTag();
		}
	}
}
