using System;
using System.IO;
using System.Xml;
using System.Web.SessionState;

namespace MapInfo.WebControls
{
	/// <summary>
	/// Interface for performing different operations with a map object.
	/// <remarks>This interface is implemented in the model. The model talks to MapXtreme to get the job done. This way the model can decide
	/// which product to talk to when performing operations.
	/// </remarks>
	/// </summary>
	public interface IMapOperations
	{
		/// <summary>
		/// This method exports a map with a given mapalias, width, and height into a stream and returns it.
		/// </summary>
		/// <remarks>The stream containing the map is written to the response and streamed back to the client.</remarks>
		/// <param name="mapAlias">MapAlias of the requested map.</param>
		/// <param name="mapWidth">Width of the map.</param>
		/// <param name="mapHeight">Height of the map.</param>
		/// <param name="exportFormat">Export format to be used to export the map.</param>
		/// <returns>Stream containing the map.</returns>
		MemoryStream GetMap(string mapAlias, int mapWidth, int mapHeight, string exportFormat);
		/// <summary>
		/// Set the maps size.
		/// </summary>
		/// <remarks>This method is used before exporting the image so the properly sized image gets exported.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="width">Width to be set.</param>
		/// <param name="height">Height to be set.</param>
		void SetMapSize(string mapAlias, int width, int height);
		/// <summary>
		/// Sets a given layer's visibility for a given map.
		/// </summary>
		/// <remarks>This method sets the layers's visibility given the layer alias set to true or false. If the layer type is label layer then it 
		/// sets the visibility of all label sources. This method is called when the visibility is controlled from the Layer Control.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="layerAlias">LayerAlias of the layer.</param>
		/// <param name="layerType">Type of the layer.</param>
		/// <param name="visible">Visibility of the layer.</param>
		void SetLayerVisibility(string mapAlias, string layerAlias, string layerType, bool visible);
		/// <summary>
		/// Zooms the map based on two points.
		/// </summary>
		/// <remarks>This method zooms the map based on two screen points. This method is called when the zoom tool is used and a rectangle or single
		/// point used.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">First corner of the rectangle.</param>
		/// <param name="point2">Second corner of the rectangle.</param>
		/// <param name="zoomIn">Whether to perform a zoom-in or a zoom-out.</param>
		void Zoom(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2, bool zoomIn);
		/// <summary>
		/// Zooms the map based on the zoom factor or the zoom level.
		/// </summary>
		/// <remarks>This method zooms the map either by a factor or to a specific zoom level in map units. This method is called when the zoombar tools
		/// are used to zoom to a specific zoom level.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="zoomFactor">Zoom factor.</param>
		/// <param name="zoomLevel">Zoom level.</param>
		void Zoom(string mapAlias, double zoomFactor, double zoomLevel);
		/// <summary>
		/// Pans the map given start and end points
		/// </summary>
		/// <remarks>This method takes two points, calculates the offset, and calls the Map object's pan method to perform the pan. This method is called
		/// when the Pan tool is used to drag the map from one location to another on the client.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">Start point where the drag started.</param>
		/// <param name="point2">End point where the drag finished.</param>
		void Pan(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2);
		/// <summary>
		/// Pans the map using units.
		/// </summary>
		/// <remarks>This method takes north and east offsets in map units and pans the map. This method is called when navigation tools
		/// are used.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="north">North offset.</param>
		/// <param name="east">East offset.</param>
		void Pan(string mapAlias, double north, double east);
		/// <summary>
		/// Pans the map using screen coordinates.
		/// </summary>
		/// <remarks>This method takes x and y offsets in screen coordinates and pans the map. This method is called by navigation tools and when
		/// the map is to be panned by percentage of the screen instead of units.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="xOffset">X offset in pixels.</param>
		/// <param name="yOffset">Y offset in pixels.</param>
		void Pan(string mapAlias, int xOffset, int yOffset);
		/// <summary>
		/// Centers the map to a given point in screen coodinates.
		/// </summary>
		/// <remarks>This method takes a screen point and centers the map on that point. This method is called when the center tool is used.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point where the map is centerd.</param>
		void Center(string mapAlias, System.Drawing.Point point);
		/// <summary>
		/// Calculates the distance in map units from given points in screen coodinates.
		/// </summary>
		/// <remarks>This method calculates the total distance between given points.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="points">Array of points in pixels.</param>
		/// <param name="distanceType">Type of calculation to be used in distance.</param>
		/// <param name="distanceUnit">Units used in the distance calculation.</param>
		/// <returns>distance as double</returns>
		double Distance(string mapAlias, System.Drawing.Point[] points, string distanceType, string distanceUnit);
		/// <summary>
		/// Selects all features in all visible and selectable layers near a given point.
		/// </summary>
		/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
		/// default selection. The default pixel tolerance is 6 pixels.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point in pixels.</param>
		void PointSelection(string mapAlias, System.Drawing.Point point);
		/// <summary>
		/// Selects all features in all visible and selectable layers near a given point using a given pixel tolerance.
		/// </summary>
		/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
		/// default selection.
		/// </remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point">Point in pixels.</param>
		/// <param name="pixelTolerance">Pixel tolerance.</param>
		void PointSelection(string mapAlias, System.Drawing.Point point, int pixelTolerance);
		/// <summary>
		/// Selects all features within a given rectangle.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given rectangle and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="point1">First corner of the rectangle.</param>
		/// <param name="point2">Second corner of the rectangle.</param>
		void RectangleSelection(string mapAlias, System.Drawing.Point point1, System.Drawing.Point point2);
		/// <summary>
		/// Selects all features with a given radius.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given radius and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="center">Center of the circle.</param>
		/// <param name="radius">Radius of the circle.</param>
		void RadiusSelection(string mapAlias, System.Drawing.Point center, int radius);
		/// <summary>
		/// Selects all features whose centroids lie within a given polygon.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given polygon and updates the 
		/// default selection.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="points">Array of points forming the polygonCenter of the circle</param>
		void PolygonSelection(string mapAlias, System.Drawing.Point[] points);
	}
}
