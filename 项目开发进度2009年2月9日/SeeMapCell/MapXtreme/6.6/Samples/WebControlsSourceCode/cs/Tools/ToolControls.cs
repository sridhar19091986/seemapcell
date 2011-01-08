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
using MapInfo.Geometry;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This tool allows users to draw a rectangle specifying the view to zoom into.
	/// </summary>/
	/// <remarks>
	/// Major functionality is executed by the base class.
	/// </remarks>
	[
	ToolboxData("<{0}:ZoomInTool runat=server></{0}:ZoomInTool>")
	]
	public class ZoomInTool : WebTool
	{
		/// <summary>
		/// The constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public ZoomInTool()
			:base() 
		{
			Command = CommandEnum.ZoomIn.ToString();
			ClientInteraction = ClientInteractionEnum.RectInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("ZoomInMapToolHelpText");
		}
	}

	/// <summary>
	/// This tool allows users to draw a rectangle specifying the view to zoom out.
	/// </summary>
	/// <remarks>
	/// Major functionality is executed by the base class.
	/// </remarks>
	[
	ToolboxData("<{0}:ZoomOutTool runat=server></{0}:ZoomOutTool>")
	]
	public class ZoomOutTool : WebTool
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public ZoomOutTool() 
			:base() 
		{
			Command = CommandEnum.ZoomOut.ToString();
			ClientInteraction = ClientInteractionEnum.RectInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("ZoomOutMapToolHelpText");
		}
	}

	/// <summary>
	/// This allows users to pan the map by dragging in any direction.
	/// </summary>
	/// <remarks>
	/// While dragging the map, if the cursor goes out of the bounds of the map image, the map is automatically updated. This is done
	/// to prevent strange behavior if users keep on dragging indefinitely.
	/// </remarks>
	[
	ToolboxData("<{0}:PanTool runat=server></{0}:PanTool>")
	]
	public class PanTool : WebTool
	{
		/// <summary>
		/// The constructor sets the command name , the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public PanTool() 
			:base() 
		{
			Command = CommandEnum.Pan.ToString();
			ClientInteraction = ClientInteractionEnum.DragInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("PanMapToolHelpText");
			ClientCommand = ClientCommandEnum.PanCommand.ToString();
		}

		/// <summary>
		/// The Pan tool uses a different command on the client side, therefore this method is overridden.
		/// </summary>
		/// <param name="writer">HtmlTextWriter</param>
		/// <remarks>The client side command for pan is different so it can position the map correctly (might have been dragged out of position).</remarks>
		protected override void RenderJS(HtmlTextWriter writer) {
			RenderJS(writer, ClientCommand, "ImgTool");
		}
	}

	///<summary>
	/// Allows users to draw a polyline and then compute total distance between
	/// end points of the lines.
	/// </summary>
	///	<remarks>
	///When this ToolControl is active, it allows users to draw a polygon with
	/// single clicks and finish with a double-click. The tool then calculates the total distance
	/// between all supplied points. 
	/// </remarks>
	[
	ToolboxData("<{0}:DistanceTool runat=server></{0}:DistanceTool>")
	]
	public class DistanceTool : WebTool {
		/// <summary>
		/// The constructor sets the command name , the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public DistanceTool() 
			:base() 
		{
			Command = CommandEnum.Distance.ToString();
			ClientInteraction = ClientInteractionEnum.PolylineInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("DistanceMapToolHelpText");
			ClientCommand = ClientCommandEnum.DistanceCommand.ToString();
		}

		///	<summary>Provides a method (Cartesian or Spherical) to calculate the distance
		/// between end points of lines.</summary>
		///	<value>Gets or sets the method to be used to compute distances.</value>
		///	<remarks>DistanceType is the method used (Cartesian or Spherical) to calculate
		/// the distance between end points of lines.</remarks>
		[
		Browsable(true),
		Category("Mapping"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute)
		] 
		public MapInfo.Geometry.DistanceType DistanceType {
			get {
				if (ViewState["DistanceType"] == null) return MapInfo.Geometry.DistanceType.Spherical;
				return (MapInfo.Geometry.DistanceType)ViewState["DistanceType"];
			}

			set {
				ViewState["DistanceType"] = value;
			}
		}

		///	<summary>Gets or sets the distance unit to compute distances.</summary>
		///	<value>Gets or sets the distance unit to compute distances.</value>
		///	<remarks>None.</remarks>
		[
		Browsable(true),
		Category("Mapping"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute)
		] 
		public MapInfo.Geometry.DistanceUnit DistanceUnit {
			get {
				if (ViewState["DistanceUnit"] == null) return MapInfo.Geometry.DistanceUnit.Mile;
				return (MapInfo.Geometry.DistanceUnit)ViewState["DistanceUnit"];
			}

			set {
				ViewState["DistanceUnit"] = value;
			}
		}

		/// <summary>
		/// Renders javascript to set distance type and unit.
		/// </summary>
		/// <remarks>The distance type and unit are then added to the URL to be used on the server.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer)
		{
			// Since this is runtime behavior do it only if there is context.
			RenderJS(writer, ClientCommand, "ImgTool");
			writer.AddAttribute("language", "javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(string.Format("{0}Cmd.distanceType = '{1}';", UniqueID, DistanceType.ToString()));
			writer.WriteLine(string.Format("{0}Cmd.distanceUnit = '{1}';", UniqueID, DistanceUnit.ToString()));
			writer.RenderEndTag();
		}
	}

	/// <summary>
	/// This tool allows users to change the center of the map by clicking where they would like the center on the map.
	/// </summary>
	/// <remarks>
	/// The major functionality is executed by the base class.
	/// </remarks>
	[
	ToolboxData("<{0}:CenterTool runat=server></{0}:CenterTool>")
	]
	public class CenterTool : WebTool
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public CenterTool() 
			:base() 
		{
			Command = CommandEnum.Center.ToString();
			ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("CenterMapToolHelpText");
		}
	}

	/// <summary>
	/// This tool allows users to select features by clicking on the map.
	/// </summary>
	/// <remarks>
	/// The pixel tolerance property is used to form a rectangle around the point click, all features are selected in all selectable layers 
	/// intersecting this rectangle. The one nearest is selected.
	/// </remarks>
	[
	ToolboxData("<{0}:PointSelectionTool runat=server></{0}:PointSelectionTool>")
	]
	public class PointSelectionTool : WebTool
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>
		/// The major functionality is executed by the base class.
		/// </remarks>
		public PointSelectionTool() 
			:base() 
		{
			Command = CommandEnum.PointSelection.ToString();
			ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString();
			ClientCommand = ClientCommandEnum.PointSelectionCommand.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("PointSelectionMapToolHelpText");
		}

		/// <summary>
		/// Pixel tolerance for performing a search.
		/// </summary>
		/// <remarks>The pixel tolerance is converted to a distance in map units, and a buffer is created around the given point used for the search.
		/// The first feature to intersect this buffered point is added to the default selection. A default value of 6 is used for the tolerance.
		/// </remarks>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		PersistenceMode(PersistenceMode.Attribute),
		]
		public int PixelTolerance{
			get {
				if (ViewState["PixelTolerance"] == null) return 6;
				else return (int)ViewState["PixelTolerance"];
			}
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException();
				ViewState["PixelTolerance"] = value;
			}
		}

		/// <summary>
		/// Renders javascript to set a distance type and unit.
		/// </summary>
		/// <remarks>The pixel tolerance is added to the URL to be used by the server.</remarks>
		/// <param name="writer">HtmlTextWriter</param>
		protected override void RenderJS(HtmlTextWriter writer) {
			// Since this is runtime behavior do it only if there is context
			RenderJS(writer, ClientCommand, "ImgTool");
			writer.AddAttribute("language", "javascript");
	        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(string.Format("{0}Cmd.pixelTolerance = {1};", UniqueID, PixelTolerance));
			writer.RenderEndTag();
		}
	}

	/// <summary>
	/// This tool allows users to select all features whose centroids fall within a drawn rectangle.
	/// </summary>
	/// <remarks>
	/// All features are selected whose centroid lie within the rectangle drawn by the user in all selectable layers.
	/// </remarks>
	[
	ToolboxData("<{0}:RectangleSelectionTool runat=server></{0}:RectangleSelectionTool>")
	]
	public class RectangleSelectionTool : WebTool
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public RectangleSelectionTool() 
			:base() 
		{
			Command = CommandEnum.RectangleSelection.ToString();
			ClientInteraction = ClientInteractionEnum.RectInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("RectangleSelectionMapToolHelpText");
		}
	}

	/// <summary>
	/// This tool allows users to select all features whose centroids lie within a given polygon.
	/// </summary>
	///	<remarks>The polygon is drawn by clicks (the nodes of the polygon) and a double click to finish.
	/// </remarks>
	[
	ToolboxData("<{0}:PolygonSelectionTool runat=server></{0}:PolygonSelectionTool>")
 	]
	public class PolygonSelectionTool : WebTool
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public PolygonSelectionTool() 
			:base() 
		{
			Command = CommandEnum.PolygonSelection.ToString();
			ClientInteraction = ClientInteractionEnum.PolygonInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("PolygonSelectionMapToolHelpText");
		}
	}

	/// <summary>
	/// This tool allows users to select all features whose centroids fall within a given radius.
	/// </summary>
	/// <remarks>
	/// All features are selected whose centroid lie within the circle drawn by users in all selectable layers.
	/// </remarks>
	[
	ToolboxData("<{0}:RadiusSelectionTool runat=server></{0}:RadiusSelectionTool>")
	]
	public class RadiusSelectionTool : WebTool 
	{
		/// <summary>
		/// This constructor sets the command name, the interaction it is going to perform, and the cursor it is going to use.
		/// </summary>
		/// <remarks>None</remarks>
		public RadiusSelectionTool() 
			:base() 
		{
			Command = CommandEnum.RadiusSelection.ToString();
			ClientInteraction = ClientInteractionEnum.RadInteraction.ToString();
			Active = false;
			CursorImageUrl = string.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command);
			ToolTip = L10NUtils.Resources.GetString("RadiusSelectionMapToolHelpText");
		}
	}
}
