Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Interface IMapOperations
    '/// <summary>
    '/// This method exports a map with given mapalias, width and height into a steam and returns it.
    '/// </summary>
    '/// <remarks>The stream containing the map is written to the response and streamed back to the client.</remarks>
    '/// <param name="mapAlias">MapAlias of the requested map</param>
    '/// <param name="mapWidth">Width of the map</param>
    '/// <param name="mapHeight">Height of the map</param>
    '/// <returns>Stream containing the map</returns>
    Function GetMap(ByVal mapAlias As String, ByVal mapWidth As Integer, ByVal mapHeight As Integer, ByVal exportFormat As String) As MemoryStream

    '/ <summary>
    '/ Set maps size
    '/ </summary>
    '/ <param name="mapAlias">MapAlias of the map</param>
    '/ <param name="width">Width to be set</param>
    '/ <param name="height">Height to be set</param>
    Sub SetMapSize(ByVal mapAlias As String, ByVal width As Integer, ByVal height As Integer)

    '/// <summary>
    '/// Center the map to a given point in screen coodinates
    '/// </summary>
    '/// <remarks>This method takes screen point and centers the map to that point. This method is called when center tool is used</remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point">Point at which the map is to be centerd</param>
    Sub Center(ByVal mapAlias As String, ByVal point As System.Drawing.Point)

    '/// <summary>
    '/// Calculate distance in map's unit from given points in the screen coodinates.
    '/// </summary>
    '/// <remarks>This method calculates the total distance between given points.</remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="points">Array of points in pixels</param>
    '/// <returns></returns>
    Function Distance(ByVal mapAlias As String, ByVal points As System.Drawing.Point(), ByVal distanceType As String, ByVal distanceUnit As String) As Double

    '/// <summary>
    '/// Set a given layer's visibility for a given map.
    '/// </summary>
    '/// <remarks>This method sets layers's visibility with given layer alias to true or false. If the layer type is label layer then it 
    '/// sets the visibility of all label sources. This method is called when the visibility is controlled from Layer Control.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="layerAlias">LayerAlias of the layer</param>
    '/// <param name="layerType">Type of the layer</param>
    '/// <param name="visible">Visibility of the layer</param>
    Sub SetLayerVisibility(ByVal mapAlias As String, ByVal layerAlias As String, ByVal layerType As String, ByVal visible As Boolean)

    '/// <summary>
    '/// Pan the map given start and end point
    '/// </summary>
    '/// <remarks>This method takes two points, calculates the offset and then calls Map object's pan method to do the pan. This method is called
    '/// when Pan tool is used to drag the map from one location to another on the client.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point1">Starting point when drag started</param>
    '/// <param name="point2">End point when the drag was finished</param>
    Sub Pan(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point)

    '/// <summary>
    '/// Pan the map by units
    '/// </summary>
    '/// <remarks>This method takes north and east offsets in unit of the map and pans the map with it. This method is called when the navigation tools
    '/// are used.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="north">North offset. </param>
    '/// <param name="east">East offset.</param>
    Sub Pan(ByVal mapAlias As String, ByVal north As Double, ByVal east As Double)

    '/// <summary>
    '/// Pan the map by screen coordinates.
    '/// </summary>
    '/// <remarks>This method takes x and y offsets in screen coordinates and pans the map. This method is called by navigation tools and when
    '/// the map is to be panned by percentage of the screen instead of units.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="xOffset">X offset in pixels</param>
    '/// <param name="yOffset">Y offset in pixels</param>
    Sub Pan(ByVal mapAlias As String, ByVal xOffset As Int32, ByVal yOffset As Int32)

    '/// <summary>
    '/// Select all features in all visible and selectable layers near a given point.
    '/// </summary>
    '/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
    '/// default selection.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point">Point in pixels</param>
    Sub PointSelection(ByVal mapAlias As String, ByVal point As System.Drawing.Point)

    '/// <summary>
    '/// Select all features in all visible and selectable layers near a given point using given pixel tolerance
    '/// </summary>
    '/// <remarks>This method searches all features near a given point in all visible and selectable layers and then updates the
    '/// default selection.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point">Point in pixels</param>
    '/// <param name="pixelTolerance">Pixel tolerance</param>
    Sub PointSelection(ByVal mapAlias As String, ByVal point As System.Drawing.Point, ByVal pixelTolerance As Integer)

    '/// <summary>
    '/// Select all feature whose centroid lie within given polygon.
    '/// </summary>
    '/// <remarks>This method searches for all features whose centroids are within the given polygon and updates the 
    '/// default selection.</remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="points">Array of points forming polygonCenter of the circle</param>
    Sub PolygonSelection(ByVal mapAlias As String, ByVal points As System.Drawing.Point())

    '/// <summary>
    '/// Select all feature with given radius.
    '/// </summary>
    '/// <remarks>This method searches for all features whose centroids are within the given radius and updates the 
    '/// default selection.</remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="center">Center of the circle</param>
    '/// <param name="radius">Radius of the circle</param>
    Sub RadiusSelection(ByVal mapAlias As String, ByVal center As System.Drawing.Point, ByVal radius As Integer)

    '/// <summary>
    '/// Select all feature with given rectangle.
    '/// </summary>
    '/// <remarks>This method searches for all features whose centroids are within the given rectangle and updates the 
    '/// default selection.</remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point1">First corner of the rectangle</param>
    '/// <param name="point2">Second corner of the rectangle</param>
    Sub RectangleSelection(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point)

    '/// <summary>
    '/// Zoom the map based on two points.
    '/// </summary>
    '/// <remarks>This method zooms the map based on two screen points. This method is called when zoom tool is used and rectangle or single
    '/// point is chosen
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="point1">First corner of the rectangle</param>
    '/// <param name="point2">Second corner of the rectangle</param>
    '/// <param name="zoomIn">Whether to do zoom-in or zoom-out.</param>
    Sub Zoom(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point, ByVal zoomIn As Boolean)

    '/// <summary>
    '/// Zoom the map based on zoom factor or zoom level
    '/// </summary>
    '/// <remarks>This method zooms the map either by a factor or to a specific zoom level in map's units. This method is called when zoombar tools
    '/// are used to zoom to specific zoom level.
    '/// </remarks>
    '/// <param name="mapAlias">MapAlias of the map</param>
    '/// <param name="zoomFactor">Zoom factor</param>
    '/// <param name="zoomLevel">Zoom level</param>
    Sub Zoom(ByVal mapAlias As String, ByVal zoomFactor As Double, ByVal zoomLevel As Double)
End Interface
