using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Persistence;
using MapInfo.Tools;

namespace MapInfo.WebControls
{
	/// <summary>
	/// Implements the model interface.</summary> 
	/// <remarks>This implementation talks to MapXtreme to do various operations.</remarks>
	[Serializable]
	public class MapControlModel  :  IMapOperations {
		/// <summary>
		/// Constructor for the model.
		/// </summary>
		/// <remarks>
		/// In the constructor, all default command objects are created and added to the collection. 
		/// The InvokeCommand method then goes through this collection to find a command and executes it.
		/// </remarks>
		public MapControlModel() {
			_commands.Add(new GetMap());
			_commands.Add(new ZoomIn());
			_commands.Add(new ZoomOut());
			_commands.Add(new ZoomToLevel());
			_commands.Add(new Center());
			_commands.Add(new Pan());
			_commands.Add(new Navigate());
			_commands.Add(new Distance());
			_commands.Add(new PointSelection());
			_commands.Add(new RectangleSelection());
			_commands.Add(new RadiusSelection());
			_commands.Add(new PolygonSelection());
			_commands.Add(new LayerVisibility());
		}

		private ArrayList _commands = new ArrayList();
		/// <summary>
		/// Gets or sets the collection containing commands.
		/// </summary>
		/// <remarks>If you write your own custom command or want to replace the existing command 
		/// you can manipulate this collection.</remarks>
		public ArrayList Commands {
			get {return _commands;}
			set {_commands = value;}
		}

		/// <summary>
		/// Gets the command object with the given name.
		/// </summary>
		/// <remarks>The collection is searched for the command with the given name and a command object is returned.</remarks>
		/// <param name="name">Name of the command.</param>
		/// <returns>Returns a command object.</returns>
		public MapBaseCommand GetNamedCommand(string name) {
			foreach(MapBaseCommand cmd in Commands) {
				if (cmd.Name.Equals(name)) return cmd;
			}
			return null;
		}

		/// <summary>
		/// Invokes the command contained in the URL when the client talks to the server.
		/// </summary>
		/// <remarks>The client creates a URL containing a command name and data, then calls the controller to perform the task. 
		/// The controller calls this method and passes the context that contains the parameters.
		/// </remarks>
		public virtual void InvokeCommand() {
			string command = HttpContext.Current.Request[MapBaseCommand.CommandKey];
			// Go through list and find the command and execute it
			foreach(MapBaseCommand cmd in _commands) {
				if (cmd.Name.Equals(command)) {
					cmd.Execute();
				}
			}
		}

		/// <summary>
		/// Creates the default model provided by MapXtreme and sets the object in the ASP.NET session.
		/// </summary>
		/// <remarks>This method tries to extract the model for the MapControl from the ASP.NET session. If a model is not found, a default will be created and places there.
		/// This is a safe method and always returns the model. This method can be used when the model does not exist in the session.</remarks>
		/// <returns>MapControlMode from session.</returns>
		public static MapControlModel SetDefaultModelInSession() {
			HttpContext context = HttpContext.Current;
			MapControlModel model = GetModelFromSession();
			if (model == null) {
				model = new MapControlModel();
				SetModelInSession(model);
			}
			return model;
		}

		/// <summary>
		/// Gets the model from the ASP.NET session.
		/// </summary>
		/// <remarks>This method will return null if the model is not found.</remarks>
		/// <returns>MapControlModel</returns>
		public static MapControlModel GetModelFromSession() {
			HttpContext context = HttpContext.Current;
			string key = string.Format("{0}_MapModel", context.Session.SessionID);
			MapControlModel model = null;
			if (context.Session[key] != null) {
				model = context.Session[key] as MapControlModel;
			}
			return model;
		}

		/// <summary>
		/// Sets the given model in the ASP.NET session
		/// </summary>
		/// <remarks>If a custom model is used, this method can be used to set the model in the session that will be used instead</remarks>
		/// <param name="model">The model to be set in the ASP.NET session.</param>
		public static void SetModelInSession(MapControlModel model) {
			HttpContext context = HttpContext.Current;
			string key = string.Format("{0}_MapModel", context.Session.SessionID);
			context.Session[key] = model;
		}

		/// <summary>
		/// Gets the Map object from the MapFactory with the given MapAlias.
		/// </summary>
		/// <remarks>If the MapAlias is invalid or null, then the first map is returned from the MapFactory.</remarks>
		/// <param name="mapAlias">MapAlias of the Map object to be retrieved.</param>
		/// <returns>Returns a Map object.</returns>
		public virtual Map GetMapObj(string mapAlias) {
			Map map = null;
			if (mapAlias == null || mapAlias.Length <= 0) {
				map = MapInfo.Engine.Session.Current.MapFactory[0];
			} else {
				map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];
				if (map == null) map = MapInfo.Engine.Session.Current.MapFactory[0];
			}
			return map;
		}

		/// <summary>
		/// Exports a map with a given MapAlias, width, and height into a stream and returns it.
		/// </summary>
		/// <remarks>The stream containing the map is written to the response and streamed back to the client.</remarks>
		/// <param name="mapAlias">MapAlias of the requested map.</param>
		/// <param name="mapWidth">Width of the map.</param>
		/// <param name="mapHeight">Height of the map.</param>
		/// <param name="exportFormat">Export format to be used to export the map.</param>
		/// <returns>Returns the stream containing the map.</returns>
		public virtual MemoryStream GetMap(string mapAlias,  int mapWidth, int mapHeight, string exportFormat) {
			Map map = GetMapObj(mapAlias);
			map.Size = new Size(mapWidth, mapHeight);
			ExportFormat ef = (ExportFormat)ExportFormat.Parse(typeof(ExportFormat), exportFormat);

			MemoryStream memStream = null;

			if (map != null) {
				MapExport mapExport = new MapExport(map);
				mapExport.ExportSize = new ExportSize(mapWidth, mapHeight);
				memStream = new MemoryStream();
				mapExport.Format = ef;
				mapExport.Export(memStream);
				memStream.Position = 0;
				mapExport.Dispose();
			}
			return memStream;
		}

		/// <summary>
		/// Set the maps size.
		/// </summary>
		/// <remarks>Gets the map object with the given map alias and sets the size.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="width">Width to be set.</param>
		/// <param name="height">Height to be set.</param>
		public void SetMapSize(string mapAlias, int width, int height) {
			Map map = GetMapObj(mapAlias);
			map.Size = new Size(width, height);
		}

		/// <summary>
		/// Pans the map given a start and an end point.
		/// </summary>
		/// <remarks>This method takes two points, calculates the offset, and calls the Map object's pan method to perform the pan. 
		/// This method is called when the Pan tool is used to drag the map from one location to another on the client.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">Starting point where the drag started.</param>
		/// <param name="point2">End point where the drag finished.</param>
		public virtual void Pan(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2) {
			Map map = GetMapObj(mapAlias);
			map.Pan(-1*(point1.X - point2.X), -1*(point1.Y - point2.Y));
		}

		/// <summary>
		/// Pans the map by units.
		/// </summary>
		/// <remarks>This method takes North and East offsets in units of the map and pans the map. 
		/// This method is called when the navigation tools are used.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="north">North offset. </param>
		/// <param name="east">East offset.</param>
		public virtual void Pan(string mapAlias, double north, double east) {
			Map map = GetMapObj(mapAlias);
			map.Pan(north, east, map.Zoom.Unit, true);
		}

		/// <summary>
		/// Pans the map by screen coordinates.
		/// </summary>
		/// <remarks>This method takes x and y offsets in screen coordinates and pans the map. 
		/// This method is called by the navigation tools and when the map is panned by percentage of the screen instead of units.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="xOffset">X offset in pixels.</param>
		/// <param name="yOffset">Y offset in pixels.</param>
		public virtual void Pan(string mapAlias, int xOffset, int yOffset) {
			Map map = GetMapObj(mapAlias);
			map.Pan(xOffset, yOffset);
		}

		/// <summary>
		/// Centers the map on a given point in screen coodinates.
		/// </summary>
		/// <remarks>This method takes a screen point and centers the map to that point. This method is called when the 
		/// center tool is used.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point at which the map is to be centered.</param>
		public virtual void Center(string mapAlias, System.Drawing.Point point) {
			Map map = GetMapObj(mapAlias);
			// Gets the full client rectangle in screen coordinates
			DPoint dPnt = new DPoint();
			map.DisplayTransform.FromDisplay(point, out dPnt);

			// Now center
			map.SetView(dPnt, map.GetDisplayCoordSys(), map.Zoom);
		}

		/// <summary>
		/// Zooms the map based on two points.
		/// </summary>
		/// <remarks>This method zooms the map based on two screen points. This method is called when the zoom tool is used 
		/// and a rectangle or a single point is chosen.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">First corner of the rectangle.</param>
		/// <param name="point2">Second corner of the rectangle.</param>
		/// <param name="zoomIn">Whether to do zoom-in or zoom-out.</param>
		public virtual void Zoom(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2, bool zoomIn) {
			Map map = GetMapObj(mapAlias);
			if (zoomIn) {
				map.SetView(point1, point2, MapInfo.Mapping.ZoomType.ZoomIn);
			} else {
				map.SetView(point1, point2, MapInfo.Mapping.ZoomType.ZoomOut);
			}
		}

		/// <summary>
		/// Zooms the map based on zoom factor or zoom level.
		/// </summary>
		/// <remarks>This method zooms the map either by a factor or to a specific zoom level in maps units. 
		/// This method is called when the zoombar tools are used to zoom to a specific zoom level.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="zoomFactor">Zoom factor.</param>
		/// <param name="zoomLevel">Zoom level.</param>
		public virtual void Zoom(string mapAlias, double zoomFactor, double zoomLevel) {
			Map map = GetMapObj(mapAlias);
			// Decide what type of zoom to use depending upon whether the value is negative
			if (zoomFactor > 0.0){
				map.Zoom = new MapInfo.Geometry.Distance(map.Zoom.Value*zoomFactor, map.Zoom.Unit);
			} else {
				map.Zoom = new MapInfo.Geometry.Distance(zoomLevel, map.Zoom.Unit);
			}
		}

		/// <summary>
		/// Calculates the distance in map units from points given in screen coodinates.
		/// </summary>
		/// <remarks>This method calculates the total distance between the given points.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="points">Array of points in pixels.</param>
		/// <param name="distanceType">Type of calculation to be used in distance.</param>
		/// <param name="distanceUnit">Units to be used in the distance calculation.</param>
		/// <returns>distance as double</returns>
		public virtual double Distance(string mapAlias, System.Drawing.Point[] points, string distanceType, string distanceUnit) {
			DistanceType distType = (DistanceType)DistanceType.Parse(typeof(DistanceType), distanceType);
			DistanceUnit distunit = (DistanceUnit)DistanceUnit.Parse(typeof(DistanceUnit), distanceUnit);
			Map map = GetMapObj(mapAlias);
			double distance = 0.0;
			for (int i = 0; i < points.Length-1; i++) {
				MapInfo.Geometry.DPoint from;
				map.DisplayTransform.FromDisplay(points[i], out from);
				MapInfo.Geometry.DPoint to;
				map.DisplayTransform.FromDisplay(points[i+1], out to);

				// Compute distance
				distance += map.GetDisplayCoordSys().Distance(distType, distunit, from, to);
			}
			return distance;
		}

		/// <summary>
		/// Selects all features in all visible and selectable layers near a given point.
		/// </summary>
		/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
		/// default selection. The pixel tolerance used has a value of 6.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point in pixels.</param>
		public virtual void PointSelection(string mapAlias, System.Drawing.Point point) {
			PointSelection(mapAlias, point, 6);
		}

		/// <summary>
		/// Select all features in all visible and selectable layers near a given point using a given pixel tolerance.
		/// </summary>
		/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
		/// default selection.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point in pixels.</param>
		/// <param name="pixelTolerance">Pixel tolerance.</param>
		public void PointSelection(string mapAlias, System.Drawing.Point point, int pixelTolerance) {
			Map map = GetMapObj(mapAlias);

			// Do the search and show selections
			SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map,point, pixelTolerance);
			(si.SearchResultProcessor as ClosestSearchResultProcessor).Options = ClosestSearchOptions.StopAtFirstMatch;

			MapInfo.Geometry.Distance d = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(map, pixelTolerance);
			(si.SearchResultProcessor as ClosestSearchResultProcessor).DistanceUnit=d.Unit;
			(si.SearchResultProcessor as ClosestSearchResultProcessor).MaxDistance = d.Value;
			
			Session.Current.Selections.DefaultSelection.Clear();
			IMapLayerFilter _selFilter = MapLayerFilterFactory.FilterForTools(
				map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			ITableEnumerator table = map.Layers.GetTableEnumerator(_selFilter);
			if ( table != null ) { // null will be returned is select enabled layer is not visible, thus non-selectable
				Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo);
			}
		}

		/// <summary>
		/// Selects all features within a given rectangle.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given rectangle and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">First corner of the rectangle.</param>
		/// <param name="point2">Second corner of the rectangle.</param>
		public virtual void RectangleSelection(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2) {
			Map map = GetMapObj(mapAlias);

			System.Drawing.Rectangle  rect = new System.Drawing.Rectangle(point1.X, point1.Y, 0, 0);
			rect.Width = Math.Abs(point2.X - point1.X);
			rect.Height = Math.Abs(point2.Y - point1.Y);

			// Do the search and show selections
			SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRect(map, rect, ContainsType.Centroid);
			Session.Current.Selections.DefaultSelection.Clear();
			IMapLayerFilter _selFilter = MapLayerFilterFactory.FilterForTools(
				map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			ITableEnumerator table = map.Layers.GetTableEnumerator(_selFilter);
			if ( table != null ) { // null will be returned is select enabled layer is not visible, thus non-selectable
				Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo);
			}
		}

		/// <summary>
		/// Selects all feature within a given radius.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given radius and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="center">Center of the circle.</param>
		/// <param name="radius">Radius of the circle.</param>
		public virtual void RadiusSelection(string mapAlias, System.Drawing.Point center, int radius) {
			Map map = GetMapObj(mapAlias);

			SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(map, center, radius, 20, ContainsType.Centroid);
			Session.Current.Selections.DefaultSelection.Clear();
			IMapLayerFilter _selFilter = MapLayerFilterFactory.FilterForTools(
				map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			ITableEnumerator table = map.Layers.GetTableEnumerator(_selFilter);
			if ( table != null ) { // null will be returned is select enabled layer is not visible, thus non-selectable
				Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo);
			}
		}

		/// <summary>
		/// Selects all of the features whose centroids lie within a given polygon.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given polygon and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="points">Array of points forming the polygon.</param>
		public virtual void PolygonSelection(string mapAlias, System.Drawing.Point[] points) {
			Map map = GetMapObj(mapAlias);

			// Convert them to map coordinates
			MapInfo.Geometry.DPoint [] dpnts = new MapInfo.Geometry.DPoint[points.Length];
			for (int indx=0; indx < points.Length; indx++) {
				map.DisplayTransform.FromDisplay(points[indx], out dpnts[indx]);
			}

			// Create a polygon from these points
			CoordSys dispCSys = map.GetDisplayCoordSys();
			CoordSys geomCSys = 
				Session.Current.CoordSysFactory.CreateCoordSys(dispCSys.Type, dispCSys.Datum, dispCSys.Units, dispCSys.OriginLongitude, dispCSys.OriginLatitude, dispCSys.StandardParallelOne, dispCSys.StandardParallelTwo, dispCSys.Azimuth, dispCSys.ScaleFactor, dispCSys.FalseEasting, dispCSys.FalseNorthing, dispCSys.Range, map.Layers.Bounds, dispCSys.AffineTransform);
			MapInfo.Geometry.MultiPolygon mp = new MapInfo.Geometry.MultiPolygon(geomCSys, MapInfo.Geometry.CurveSegmentType.Linear, dpnts);

			// Search and select
			SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWithinGeometry(mp, ContainsType.Centroid);
			Session.Current.Selections.DefaultSelection.Clear();
			IMapLayerFilter _selFilter = MapLayerFilterFactory.FilterForTools(
				map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			ITableEnumerator table = map.Layers.GetTableEnumerator(_selFilter);
			if ( table != null ) { // null will be returned is select enabled layer is not visible, thus non-selectable
				Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo);
			}
		}

		/// <summary>
		/// Sets a given layer's visibility for a given map.
		/// </summary>
		/// <remarks>This method sets the layers visibility with a given layer alias to <c>true</c> or <c>false</c>. 
		/// If the layer type is a feature layer, then it also sets the visibility for all modifers. If it is a group layer then it sets the visibility for all internal layers. 
		/// If it is a label layer then it sets the visibility of all label sources. If the layer type passed is either a modifier or label source then it finds the modifier or label and sets the visibility.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="layerAlias">LayerAlias of the layer.</param>
		/// <param name="lyrType">Type of the layer.</param>
		/// <param name="visible">Visibility to be set.</param>
		public virtual void SetLayerVisibility(string mapAlias, string layerAlias, string lyrType, bool visible) {
			Map map = GetMapObj(mapAlias);

			MapLayerEnumerator lenum = map.Layers.GetMapLayerEnumerator(MapLayerEnumeratorOptions.Recurse);
			foreach (IMapLayer lyr in lenum) {
				if (lyr.Alias.Equals(layerAlias) && lyr.Type.ToString().Equals(lyrType)) {
					lyr.Enabled = visible;
					if (lyr.Type == LayerType.Normal) {
						FeatureStyleModifiers mods = ((FeatureLayer)lyr).Modifiers;
						// Go through modifiers to set visibility						
						foreach (FeatureStyleModifier fsm in mods) {
							fsm.Enabled = visible;
						}
						break;
					}
					if (lyr.Type == LayerType.Group) {
						// Go through collection to set visibility
						foreach (IMapLayer inlayer in (GroupLayer)lyr) {
							inlayer.Enabled = visible;
						}
						break;
					}
					if (lyr.Type == LayerType.Label) {
						// go through label sources to set visibility
						foreach (LabelSource source in ((LabelLayer)lyr).Sources) {
							source.Enabled = visible;
							foreach (LabelModifier lm in source.Modifiers) {
								lm.Enabled = visible;
							}
						}
						break;
					}
				} else {
					// The layeralias did not match, hence the alias may be modifier or label source
					// If it is modifier find it 
					if (lyrType.Equals("Mod")) {
						// Find it and set visibility
						if (lyr.Type == LayerType.Normal) {
							FeatureStyleModifiers mods = ((FeatureLayer)lyr).Modifiers;
							foreach (FeatureStyleModifier fsm in mods) {
								if (fsm.Alias.Equals(layerAlias)) fsm.Enabled = visible;
							}
						}
					}
					// If it is label source find it and set it's visibility
					if (lyrType.Equals("Label")) {
						if (lyr.Type == LayerType.Label) {
							foreach (LabelSource source in ((LabelLayer)lyr).Sources) {
								if (source.Alias.Equals(layerAlias)) {
									source.Enabled = visible;
									foreach (LabelModifier lm in source.Modifiers) {
										lm.Enabled = visible;
									}
								}
							}
						}
					}

					// If it is label source modifier then set it's visibility
					if (lyrType.Equals("LabelMod")) {
						if (lyr.Type == LayerType.Label) {
							foreach (LabelSource source in ((LabelLayer)lyr).Sources) {
								foreach (LabelModifier lm in source.Modifiers) {
									if (lm.Alias.Equals(layerAlias)) lm.Enabled = visible;
								}
							}
						}
					}
				}
			}
		}
	}
}
