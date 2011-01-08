using System;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Web;
using MapInfo.Engine;
using MapInfo.Mapping;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This is the base class for all Commands.
	/// </summary>
	/// <remarks>
	/// A command's responsibility is to parse the parameters out of the URL and perform the required operation. The model for the MapControl holds a collection 
	/// of all pre-defined commands. When a request comes from the client side, depending upon the command name, the command object is fetched from the collection
	/// and an Execute method is called. The Execute method takes care of the state and calls a Process method which does the actual processing and sends the 
	/// appropriate response to the client. 
	/// All derived tools implement the Process method to perform a specific operation.
	/// Since the model is held in the ASP.NET session, that class and all commands are declared with a Serializable attribute.
	/// </remarks>
	[Serializable]
	public abstract class MapBaseCommand {
		/// <summary>
		/// Key used to get the Command parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static string CommandKey = "Command";
		/// <summary>
		/// Key used to get the MapAlias parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string MapAliasKey = "MapAlias";
		/// <summary>
		/// Key used to get the Width parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string WidthKey = "Width";
		/// <summary>
		/// Key used to get the Height parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string HeightKey = "Height";
		/// <summary>
		/// Key used to get the Border parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string BorderKey = "Border";
		/// <summary>
		/// Key used to get Points from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string PointsKey = "Points";
		/// <summary>
		/// Key used to get the export format from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		public static  string ExportFormatKey = "ExportFormat";

		/// <summary>
		/// Name of the GetMap command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string GetMapCommand = "GetMap";
		/// <summary>
		/// Name of the ZoomIn command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string ZoomInCommand = "ZoomIn";
		/// <summary>
		/// Name of the ZoomOut command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string ZoomOutCommand = "ZoomOut";
		/// <summary>
		/// Name of the ZoomWithFactor command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string ZoomWithFactorCommand = "ZoomWithFactor";
		/// <summary>
		/// Name of the ZoomToLevel command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string ZoomToLevelCommand = "ZoomToLevel";
		/// <summary>
		/// Name of the Center command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string CenterCommand = "Center";
		/// <summary>
		/// Name of the Pan command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string PanCommand = "Pan";
		/// <summary>
		/// Name of the Navigate command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string NavigateCommand = "Navigate";
		/// <summary>
		/// Name of the Distance command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string DistanceCommand = "Distance";
		/// <summary>
		/// Name of the PointSelection command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string PointSelectionCommand = "PointSelection";
		/// <summary>
		/// Name of the RectangleSelection command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string RectangleSelectionCommand = "RectangleSelection";
		/// <summary>
		/// Name of the RadiusSelection command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string RadiusSelectionCommand = "RadiusSelection";
		/// <summary>
		/// Name of the PolygonSelection command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string PolygonSelectionCommand = "PolygonSelection";
		/// <summary>
		/// Name of the LayerVisibility command.
		/// </summary>
		/// <remarks>None</remarks>
		public static string LayerVisibilityCommand = "LayerVisibility";

		private string _name;
		/// <summary>
		/// Name of the Command.
		/// </summary>
		/// <remarks>This name is used to search the collection in the model.</remarks>
		public string Name {
			get{
				return _name;
			} 
			set{
				_name = value;
			}
		}

		private string _mapAlias;
		/// <summary>
		/// MapAlias of the map.
		/// </summary>
		/// <remarks>None</remarks>
		public string MapAlias {
			get{
				return _mapAlias;
			} 
			set{
				_mapAlias = value;
			}
		}

		private int _mapWidth,  _mapHeight;

		/// <summary>
		/// Width of the map used in the exported image.
		/// </summary>
		/// <remarks>The width and height are used to determine the size for the exported image.</remarks>
		public int MapWidth {
			get {return _mapWidth;}
			set {_mapWidth = value;}
		}

		/// <summary>
		/// Height of the map used in the exported image.
		/// </summary>
		/// <remarks>The width and height are used to determine size for exported image.</remarks>
		public int MapHeight {
			get {return _mapHeight;}
			set {_mapHeight= value;}
		}

		private string _exportFormat;
		/// <summary>
		/// Format used in the exporting image.
		/// </summary>
		/// <remarks>Supported formats are GIF, BMP, and JPEG.</remarks>
		public string ExportFormat {
			get {return _exportFormat;}
			set {_exportFormat= value;}
		}

		private string _commandData = null;
		/// <summary>
		/// Data in string format embedded in the URL.
		/// </summary>
		/// <remarks>The format used is "num-of-points, x1,y1,x2,y2...". Data is extracted from this string and used to perform operations.</remarks>
		public string DataString {get{return _commandData;} set{_commandData = value;}}

		private bool _border;
		/// <summary>
		/// Indicates whether or not to draw a border around the exported image.
		/// </summary>
		/// <remarks>None</remarks>
		public bool DrawBorderOnExport {get{return _border;} set{_border = value;}}

		/// <summary>
		/// Streams the memory stream back to the client.
		/// </summary>
		/// <remarks>The exported image is written to the output stream of the response object.</remarks>
		/// <param name="ms">Memory stream containing the exported image.</param>
		public virtual void StreamImageToClient(MemoryStream ms) {
			if (ms != null) {
				string contentType = string.Format("image/{0}", ExportFormat.ToString());
				BinaryReader reader = new BinaryReader(ms);
				int length = (int)ms.Length;
				if (contentType != null)    HttpContext.Current.Response.ContentType = contentType;
				HttpContext.Current.Response.OutputStream.Write(reader.ReadBytes(length), 0, length);
				reader.Close();
				ms.Close();
			}
		}

		/// <summary>
		/// Extracts points from the data string embedded in the URL.
		/// </summary>
		/// <param name="dataString">String in the format "num-of-points, x1,y1,x2,y2..."</param>
		/// <returns>Array of points extracted from the string.</returns>
		/// <remarks>The array of points is then used in operations.</remarks>
		public virtual System.Drawing.Point[] ExtractPoints(string dataString) {
			if (dataString != null) {
				string sep = ",";
				char [] cSep = sep.ToCharArray();
				string [] strPoints = dataString.Split(cSep);
				if (strPoints != null) {
					// First value is number of points
					int numPoints = Convert.ToInt32(strPoints[0]);
					System.Drawing.Point [] points = new Point[numPoints];
					for (int indx = 0; indx < numPoints; indx++) {
						sep ="_";
						cSep = sep.ToCharArray();
						string [] strpt = strPoints[indx+1].Split(cSep);
						points[indx].X = Convert.ToInt32(strpt[0]);
						points[indx].Y = Convert.ToInt32(strpt[1]);
					}
					return points;
				}
			}
			return null;
		}

		/// <summary>
		/// Extracts commonly used values of parameters from the context.
		/// </summary>
		/// <remarks>The URL originating from the client contains the command and various parameters. 
		/// This method extracts the most commonly used parameter values.
		/// </remarks>
		public virtual void ParseContext() {
			_name = HttpContext.Current.Request[CommandKey];
			_mapAlias = HttpContext.Current.Request[MapAliasKey];
			_mapWidth = System.Convert.ToInt32(HttpContext.Current.Request[WidthKey]);
			_mapHeight = System.Convert.ToInt32(HttpContext.Current.Request[HeightKey]);
			_border = (System.Convert.ToInt32(HttpContext.Current.Request[BorderKey]) == 1);
			_commandData = HttpContext.Current.Request[PointsKey];
			_exportFormat = HttpContext.Current.Request[ExportFormatKey];
		}

		/// <summary>
		/// Method to execute the command.
		/// </summary>
		/// <remarks>The default implementation calls the implemented Restorestate before and Savestate after the  Process method. 
		/// The Process method is where the operation is performed.
		/// </remarks>
		public virtual void Execute()
		{
			StateManager sm = StateManager.GetStateManagerFromSession();
			if (sm == null) 
			{
				if(StateManager.IsManualState())
				{
					throw new NullReferenceException(L10NUtils.Resources.GetString(StateManager.StateManagerResErr1));
				}
			} 
			ParseContext();
			if(sm != null)
			{
				PrepareStateManagerParamsDictionary(sm);
				sm.RestoreState();
			}

			Process();

			if(sm != null) sm.SaveState();
		}

		/// <summary>
		/// Method to prepare parameters used by the StateManager.  
		/// </summary>
		/// <param name="sm">The StateManager object which contains an IDictionary to hold parameters.</param>
		/// <remarks>You can override this method to change parameters values but you need to synchronize with your own web application.
		/// </remarks>
		public virtual void PrepareStateManagerParamsDictionary(StateManager sm)
		{
			if(sm != null)
			{
				sm.ParamsDictionary[StateManager.ActiveMapAliasKey] = MapAlias;
			}
		}

		/// <summary>
		/// Abstract method which performs the processing.
		/// </summary>
		/// <remarks>This method contains the business logic to perform operations. The derived classes must implement this method to perform the processing.</remarks>
		public abstract void Process();

	}

	/// <summary>
	/// Command to get the map.
	/// </summary>
	/// <remarks>The map with the given alias is exported as a memory stream and returned in the response.</remarks>
	[Serializable]
	public class GetMap: MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public GetMap() {
			Name = GetMapCommand;
		}

		/// <summary>
		/// This method gets the map object from the mapfactory with the given mapalias and exports it to the memory stream and streams it back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process()
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to perform a zoom in operation.
	/// </summary>
	/// <remarks>This command extracts two end points of the rectangle and performs a zoom-in operation and streams the map back.</remarks>
	[Serializable]
	public class ZoomIn : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public ZoomIn() {
			Name = ZoomInCommand;
		}

		/// <summary>
		/// Gets the map object out of the Mapfactory with a given MapAlias, zooms using two screen points, and streams the image
		/// back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.Zoom(MapAlias, points[0], points[1],  true);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to perform a zoom out operation.
	/// </summary>
	/// <remarks>This command extracts two end points of the rectangle and performs a zoom-out operation and streams the map back.</remarks>
	[Serializable]
	public class ZoomOut : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public ZoomOut() {
			Name = ZoomOutCommand;
		}

		/// <summary>
		/// Gets the map object from the MapFactory with a given MapAlias, zooms out using two screen points, and streams the image
		/// back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.Zoom(MapAlias, points[0], points[1],  false);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to perform a center operation.
	/// </summary>
	/// <remarks>This tool extracts a point and re-centers the map based on this point and streams the map back.</remarks>
	[Serializable]
	public class Center : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public Center() {
			Name = CenterCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, centers it using the screen point, and streams the image
		/// back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.Center(MapAlias, points[0]);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to performa a pan operation.
	/// </summary>
	/// <remarks>This tool extracts two points and pans the map by calculating the offsets and streams the map back.</remarks>
	[Serializable]
	public class Pan : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public Pan() {
			Name = PanCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, pans it using start and end screen points,
		/// and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.Pan(MapAlias, points[0], points[1]);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient( ms);
		}
	}

	/// <summary>
	/// Command to perform a Navigate operation.
	/// </summary>
	/// <remarks>This commands extracts the type of method to perform a pan and offsets and pans the map using these values and streams the
	/// map back.</remarks>
	[Serializable]
	public class Navigate : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public Navigate() {
			Name = NavigateCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, pans either by specified map units or percentage of
		/// the map on the screen, and streams the image back to client.
		/// </summary>
		/// <remarks>The map is panned using this method and North and East offsets.</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			double north = System.Convert.ToDouble(HttpContext.Current.Request["North"]);
			double east = System.Convert.ToDouble(HttpContext.Current.Request["East"]);
			string method = HttpContext.Current.Request["Method"];
			if (method.Equals("ByUnit")) {
				model.Pan(MapAlias, north, east);
			} else if (method.Equals("ByPercentage")) {
				int xoffset = (int)(east * 0.01 * MapWidth);
				int yoffset = (int)(north * 0.01 * MapHeight);
				model.Pan(MapAlias, xoffset, yoffset);
			}
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to find the distance.
	/// </summary>
	/// <remarks>This commands extracts the distance type and unit, calculates the distance, and returns the value back.</remarks>
	[Serializable]
	public class Distance : MapBaseCommand {
		/// <summary>
		/// Key used to get the distance type parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		protected static  string DistanceTypeKey = "DistanceType";

		/// <summary>
		/// Key used to get the distance unit parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		protected static  string DistanceUnitKey = "DistanceUnit";

		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public Distance() {
			Name = DistanceCommand;
		}
		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, calculates the total distance between the given points,
		/// and writes the double value to the response object.
		/// </summary>
		/// <remarks>This command is different, it does not update the map, but returns information. The user interface 
		/// used to display the value has to be decided by the javascript which receives it. The distance command from the client 
		/// side uses an XMLHttp object to make a call to the server to calculate the distance.
		/// </remarks>
		public override void Process() {
			string distanceType = System.Convert.ToString(HttpContext.Current.Request[DistanceTypeKey]);
			string distanceUnit  = System.Convert.ToString(HttpContext.Current.Request[DistanceUnitKey]);
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			double dist = 0.0;
			try {
				dist = model.Distance(MapAlias, points, distanceType, distanceUnit);
			} catch (Exception e) {
				HttpContext.Current.Response.Output.Write(e.Message);
				return;
			}
			HttpContext.Current.Response.Output.Write(dist);
		}
	}

	/// <summary>
	/// Command to perform a point selection.
	/// </summary>
	/// <remarks>This command extracts a point, selects the nearest feature, and streams the map back.</remarks>
	[Serializable]
	public class PointSelection : MapBaseCommand {
		/// <summary>
		/// Key used to get the pixel tolerance parameter value from the URL.
		/// </summary>
		/// <remarks>None</remarks>
		protected static  string PixelToleranceKey = "PixelTolerance";

		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public PointSelection() {
			Name = PointSelectionCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, searches all features near a 
		/// given point in all visible and selectable layers, updates the default selection, and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			int pixelTolerance = System.Convert.ToInt32(HttpContext.Current.Request[PixelToleranceKey]);
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.PointSelection(MapAlias, points[0], pixelTolerance);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to select all features whose centroids lie within a given rectangle.
	/// </summary>
	/// <remarks>This command extracts end points of the rectangle and selects the features whose centroid lie within the rectangle and streams
	/// the map back.</remarks>
	[Serializable]
	public class RectangleSelection : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public RectangleSelection() {
			Name = RectangleSelectionCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, searches for all features whose centroids are within 
		/// the given rectangle, updates the default selection, and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			if (points.Length == 1 || points[0] == points[1]) {
				model.PointSelection(MapAlias, points[0]);
			} else {
				model.RectangleSelection(MapAlias, points[0], points[1]);
			}
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to select all features whose centroids lie within a given circle.
	/// </summary>
	/// <remarks>This command extracts the center and radius values and selects features whose centroid lie within the circle and streams
	/// the map back.</remarks>
	[Serializable]
	public class RadiusSelection : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public RadiusSelection() {
			Name = RadiusSelectionCommand;
		}

		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, searches for all features whose centroids are within 
		/// the given radius, updates the default selection, and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			if (points.Length == 1) {
				model.PointSelection(MapAlias, points[0]);
			} else if (points.Length == 2 && ((points[0] == points[1]) || (points[1].X == 0))) {
				model.PointSelection(MapAlias, points[0]);
			} else {
				model.RadiusSelection(MapAlias, points[0], points[1].X);
			}
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to select all features whose centroids lie within a sgiven polygon.
	/// </summary>
	/// <remarks>This command extracts the center and radius values, selects features whose centroid lie within the polygon, and streams
	/// the map back.</remarks>
	[Serializable]
	public class PolygonSelection : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public PolygonSelection() {
			Name = PolygonSelectionCommand;
		}
		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, searches for all features whose centroids are within 
		/// the given polygon, updates the default selection, and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			model.PolygonSelection(MapAlias, points);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to set the layer visibility.
	/// </summary>
	/// <remarks>This commands extracs the layer alias, sets thea visibility to a given value (true or false), and streams the map back.</remarks>
	[Serializable]
	public class LayerVisibility : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public LayerVisibility() {
			Name = LayerVisibilityCommand;
		}
		/// <summary>
		/// Gets the map object out of the MapFactory with a given MapAlias, sets the visibility of the given layer,
		/// and streams the image back to the client.
		/// </summary>
		/// <remarks>
		/// This command is executed from the LayerControl. The LayerControl is set up when the visibility checkbox is checked, it
		/// uses an XMLHttp object to make a call to the server with a given layername and visibility. 
		/// There is no submit button for the LayerControl. Therefore, every trip to the server contains one layername.
		/// </remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			string layerAlias = HttpContext.Current.Request["LayerAlias"];
			string layerType = HttpContext.Current.Request["LayerType"];
			bool visible = (HttpContext.Current.Request["Visible"] == "true");
			model.SetLayerVisibility(MapAlias, layerAlias, layerType, visible);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}

	/// <summary>
	/// Command to perform a zoom with a given level.
	/// </summary>
	/// <remarks>
	/// This tool extracts the zoom level, zooms the map, and streams the map back.
	///  </remarks>
	[Serializable]
	public class ZoomToLevel : MapBaseCommand {
		/// <summary>
		/// Constructor for the command.
		/// </summary>
		/// <remarks>Sets the name of the command.</remarks>
		public ZoomToLevel() {
			Name = ZoomToLevelCommand;
		}

		private double _zoomLevel;
		/// <summary>
		/// Zoom level in Map units
		/// </summary>
		/// <remarks>This value is used to set the zoom level in map units.</remarks>
		public double ZoomLevel {
			get {return _zoomLevel;}
			set {_zoomLevel = value;}
		}

		/// <summary>
		/// Gets the map object out of the Mapfactory with a given MapAlias, zooms it using the given zoomfactor,
		/// and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() {
			MapControlModel model = MapControlModel.GetModelFromSession();
			ZoomLevel = System.Convert.ToDouble(HttpContext.Current.Request["ZoomLevel"]);
			model.Zoom(MapAlias, -1.0, ZoomLevel);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}
}
