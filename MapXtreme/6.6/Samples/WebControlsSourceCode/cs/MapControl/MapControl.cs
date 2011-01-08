using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MapInfo.WebControls
{
	///	<summary>Provides export formats for the web environment.</summary>
	///	<remarks>This enumeration was created to support only those export formats in
	/// the MapExportFormat that work in the web environment.</remarks>
	public enum WebExportFormat { 
		///	<summary>Exports Windows Bitmap format.</summary>
		Bmp,
		///	<summary>Exports Graphics Interchange Format (GIF) format.</summary>
		Gif,
		///	<summary>Exports Joint Photographic Experts Group (JPEG) format.</summary>
		Jpeg
	}

	///	<summary>Contains an instance of a map that is displayed.</summary>
	///	<remarks><para>The MapControl contains an instance of a Map object. This Map is obtained
	/// from a MapFactory by using the MapAlias property it is pointing to. The
	/// map is drawn by exporting the map into an image and this image is then used in the
	/// IMG HTML tag.  If the MapAlias property is not specified or is invalid, the first map from the
	/// MapFactory is chosen.</para>
	/// <para>The MapControl and other controls in this assembly follow the MVC (Model-View-Controller) pattern.
	/// The Control code draws the HTML and javascript, the trip to the server to perform the actual operation is done through the 
	/// httphandler. This way the map gets updated without re-drawing the entire page, allowing you to get visually appealing updates.
	/// The model talks to the MapXtreme core engine to perform the job.</para>
	/// <para>In order to draw a correct map, follow these steps:</para>
	/// <nl><item>Set up the workspace. It may contain single or multiple maps. When multiple maps are used, the recommended approach is to put all maps in a single workspace and draw a specific map through the MapAlias,
	/// instead of loading and unloading different workspaces.</item>
	/// <item>Choose the initial MapAlias for the map or leave it blank. If left blank, the first map in the 
	/// MapFactory will be chosen.</item></nl>
	/// <para>Once these steps are complete, the MapControl contacts the MapController with the appropriate information in the URL, 
	/// gets the exported image, and then the image is drawn.</para>
	///  </remarks>
	[
	ToolboxData("<{0}:MapControl runat=server></{0}:MapControl>"),
	Designer(typeof(MapControlDesigner))
	]
	public class MapControl : WebControl {
		/// <summary>
		/// Width of the map. 
		/// </summary>
		/// <remarks>The width is passed to the controller which tells the model to use the width for the exported map.</remarks>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public override Unit Width {
			get {
				if (base.Width.IsEmpty) {base.Width = new Unit(300);}
				return base.Width;
			}
			set {
				base.Width = value;
			}
		}

		/// <summary>
		/// Height of the map. 
		/// </summary>
		/// <remarks>The height is passed to the controller which tells the model to use the height for the exported map.</remarks>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public override Unit Height {
			get {
				if (base.Height.IsEmpty) {base.Height = new Unit(300);}
				return base.Height;
			}
			set {
				base.Height = value;
			}
		}

		private string _mapAlias;
		/// <summary>
		/// MapAlias to get a specific map. 
		/// </summary>
		/// <remarks>If this is not provided or is invalid, then the first map in the MapFactory is chosen.</remarks>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string MapAlias {
			get {
				return _mapAlias;
			}
			set {
				_mapAlias = value;
			}
		}

		private string _imgUseMap;
		/// <summary>
		/// UseMap attribute for the IMG tag which is rendered with the MapControl
		/// </summary>
		/// <remarks>Sometimes users want to usemaps with the map image. The value of this property will be added as an extra UseMap attribute to 
		/// the IMG tag rendered with this mapcontrol.</remarks>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute)
		]
		public string IMGUseMap {
			get {
				return  _imgUseMap;
		}
			set {
				_imgUseMap= value;
			}
		}

		///	<summary>Gets or sets the format used to export a map for rendering.</summary>
		///	<value>Gets or sets the format used for exporting.</value>
		///	<remarks>This property is used internally to export a map to be used for
		/// rendering.</remarks>
		[
		Browsable(true),
		DefaultValue("GIF"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		] 
		public WebExportFormat ExportFormat {
			get {
				WebExportFormat format; 
				if(ViewState["ExportFormat"] == null)
					format = WebExportFormat.Gif;
				else
					format = (WebExportFormat)ViewState["ExportFormat"];
				return format;
			}

			set {
				ViewState["ExportFormat"] = value;
			}
		}

		/// <summary> 
		/// Renders the control by setting the source of the image to a URL containing all information to get the right map.
		/// </summary>
		/// <remarks>This method renders the controls HTML by writing an HTML IMG tag setting its source to the URL. 
		/// </remarks>
		/// <param name="output">The HTML writer object to write HTML to.</param>
		protected override void RenderContents(HtmlTextWriter output) 
		{
			// Get the model out of ASP.NET and get the stream
			MapControlModel model = MapControlModel.GetModelFromSession();
			if (model == null) model = MapControlModel.SetDefaultModelInSession();
			MemoryStream ms = null;
			try{
				ms = model.GetMap(MapAlias, (int)Width.Value, (int)Height.Value, ExportFormat.ToString());
			} catch(Exception ex) {
				output.WriteLine(ex.Message);
				output.WriteLine("<br>");
				output.WriteLine(L10NUtils.Resources.GetString("MapNotFoundErrorString"));
				HttpContext.Current.Server.ClearError();
				return;
 				}

			// Insert the image stream to Cache with key imageid and timeout in 2 mintues.
			string imageid = ImageHelper.GetUniqueID();
			ImageHelper.SetImageToCache(imageid, ms, 2);
			string url = ImageHelper.GetImageURL(imageid, ExportFormat.ToString());

			//Write the IMG tag and set the mapalias.
			output.AddAttribute(HtmlTextWriterAttribute.Width, Width.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Height, Height.ToString());
//			output.AddAttribute("exportFormat", ExportFormat.ToString());
	//		output.AddAttribute("mapAlias", MapAlias);
			if (IMGUseMap != null) {
				if (IMGUseMap.Length > 0) output.AddAttribute("USEMAP", IMGUseMap);
			}
			if (CssClass != null) output.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
	        output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
			output.AddAttribute(HtmlTextWriterAttribute.Src, url);
			output.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image");
			output.RenderBeginTag(HtmlTextWriterTag.Img);
			output.RenderEndTag();
			output.Flush();

	        // Now set the alias and format manually by getting the image element.
			output.AddAttribute("language", "javascript");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			output.RenderBeginTag(HtmlTextWriterTag.Script);
			output.WriteLine(string.Format("var {0}Me = document.getElementById('{0}_Image');", UniqueID));
			output.WriteLine(string.Format("{0}Me.mapAlias= '{1}';", UniqueID, MapAlias));
			output.WriteLine(string.Format("{0}Me.exportFormat= '{1}';", UniqueID, ExportFormat));
			output.RenderEndTag();
		}
	}
}
