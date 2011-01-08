Imports System
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports MapInfo.Mapping

<Serializable()> _
 Public MustInherit Class MapBaseCommand
    '/ <summary>
    '/ Key to be used to get Command parameter value from the url
    '/ </summary>
    Public Shared CommandKey As String = "Command"
    '/ <summary>
    '/ Key to be used to get MapAlias parameter value from the url
    '/ </summary>
    Public Shared MapAliasKey As String = "MapAlias"
    '/ <summary>
    '/ Key to be used to get Width parameter value from the url
    '/ </summary>
    Public Shared WidthKey As String = "Width"
    '/ <summary>
    '/ Key to be used to get Height parameter value from the url
    '/ </summary>
    Public Shared HeightKey As String = "Height"
    '/ <summary>
    '/ Key to be used to get Border parameter value from the url
    '/ </summary>
    Public Shared BorderKey As String = "Border"
    '/ <summary>
    '/ Key to be used to get Points from the url
    '/ </summary>
    Public Shared PointsKey As String = "Points"
    '/ <summary>
    '/ Key to be used to get export format from the URL
    '/ </summary>
    Public Shared ExportFormatKey As String = "ExportFormat"

    '/ <summary>
    '/ Name of the GetMap command
    '/ </summary>
    Public Shared GetMapCommand As String = "GetMap"
    '/ <summary>
    '/ Name of the ZoomIn command
    '/ </summary>
    Public Shared ZoomInCommand As String = "ZoomIn"
    '/ <summary>
    '/ Name of the ZoomOut command
    '/ </summary>
    Public Shared ZoomOutCommand As String = "ZoomOut"
    '/ <summary>
    '/ Name of the ZoomWithFactor command
    '/ </summary>
    Public Shared ZoomWithFactorCommand As String = "ZoomWithFactor"
    '/ <summary>
    '/ Name of the ZoomWithFactor command
    '/ </summary>
    Public Shared ZoomToLevelCommand As String = "ZoomToLevel"
    '/ <summary>
    '/ Name of the Center command
    '/ </summary>
    Public Shared CenterCommand As String = "Center"
    '/ <summary>
    '/ Name of the Pan command
    '/ </summary>
    Public Shared PanCommand As String = "Pan"
    '/ <summary>
    '/ Name of the Navigate command
    '/ </summary>
    Public Shared NavigateCommand As String = "Navigate"
    '/ <summary>
    '/ Name of the Distance command
    '/ </summary>
    Public Shared DistanceCommand As String = "Distance"
    '/ <summary>
    '/ Name of the PointSelection command
    '/ </summary>
    Public Shared PointSelectionCommand As String = "PointSelection"
    '/ <summary>
    '/ Name of the RectangleSelection command
    '/ </summary>
    Public Shared RectangleSelectionCommand As String = "RectangleSelection"
    '/ <summary>
    '/ Name of the RadiusSelection command
    '/ </summary>
    Public Shared RadiusSelectionCommand As String = "RadiusSelection"
    '/ <summary>
    '/ Name of the PolygonSelection command
    '/ </summary>
    Public Shared PolygonSelectionCommand As String = "PolygonSelection"
    '/ <summary>
    '/ Name of the LayerVisibility command
    '/ </summary>
    Public Shared LayerVisibilityCommand As String = "LayerVisibility"

    Private _name As String
    '/ <summary>
    '/ Name of the Command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal Value As String)
            _name = value
        End Set
    End Property

    Private _mapAlias As String
    '/ <summary>
    '/ MapAlias of the map which needs operated upon
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property MapAlias() As String
        Get
            Return _mapAlias
        End Get
        Set(ByVal Value As String)
            _mapAlias = value
        End Set
    End Property

    Private _mapWidth As Integer, _mapHeight As Integer

    '/ <summary>
    '/ Width of the map to be used in the exported image
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property MapWidth() As Integer
        Get
            Return _mapWidth
        End Get
        Set(ByVal Value As Integer)
            _mapWidth = value
        End Set
    End Property

    '/ <summary>
    '/ Height of the map to be used in the exported image
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property MapHeight() As Integer
        Get
            Return _mapHeight
        End Get
        Set(ByVal Value As Integer)
            _mapHeight = value
        End Set
    End Property

    Private _exportFormat As String
    '/ <summary>
    '/ Format to be used in the exporting image.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property ExportFormat() As String
        Get
            Return _exportFormat
        End Get
        Set(ByVal Value As String)
            _exportFormat = Value
        End Set
    End Property

    Private _commandData As String = Nothing
    '/ <summary>
    '/ Data in string format embedded in url
    '/ </summary>
    '/ <remarks>The format used is "num-of-points, x1,y1,x2,y2...". Data is extracted from this string and used to do operations</remarks>
    Public Property DataString() As String
        Get
            Return _commandData
        End Get
        Set(ByVal Value As String)
            _commandData = Value
        End Set
    End Property

    Private _border As Boolean
    '/ <summary>
    '/ Boolean to indicate whether to draw border around exported image or not
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Property DrawBorderOnExport() As Boolean
        Get
            Return _border
        End Get
        Set(ByVal Value As Boolean)
            _border = Value
        End Set
    End Property

    '/ <summary>
    '/ This method takes the memory stream containing image and streams is back to the client by writing it to the response.
    '/ </summary>
    '/ <param name="ms">Memory stream containing exported image</param>
    Public Overridable Sub StreamImageToClient(ByVal ms As MemoryStream)
        If Not ms Is Nothing Then
            Dim contentType As String = String.Format("image/{0}", ExportFormat.ToString())
            Dim reader As BinaryReader = New BinaryReader(ms)
            Dim length As Integer = CType(ms.Length, Integer)
            If Not contentType Is Nothing Then
                HttpContext.Current.Response.ContentType = contentType
            End If
            HttpContext.Current.Response.OutputStream.Write(reader.ReadBytes(length), 0, length)
            reader.Close()
            ms.Close()
        End If
    End Sub

    '/ <summary>
    '/ This method extracts points from datastring embedded in url
    '/ </summary>
    '/ <param name="dataString">String in the format "num-of-points, x1,y1,x2,y2..."</param>
    '/ <returns>Array of points extracted from the string</returns>
    '/ <remarks>None</remarks>
    Public Overridable Function ExtractPoints(ByVal dataString As String) As Point()
        If (Not dataString Is Nothing) Then
            Dim text1 As String = ","
            Dim chArray1 As Char() = text1.ToCharArray
            Dim textArray1 As String() = dataString.Split(chArray1)
            If (Not textArray1 Is Nothing) Then
                Dim num1 As Integer = Convert.ToInt32(textArray1(0))
                Dim pointArray1 As Point() = New Point(num1 - 1) {}
                Dim num2 As Integer
                For num2 = 0 To num1 - 1
                    text1 = "_"
                    chArray1 = text1.ToCharArray
                    Dim textArray2 As String() = textArray1((num2 + 1)).Split(chArray1)
                    pointArray1(num2).X = Convert.ToInt32(textArray2(0))
                    pointArray1(num2).Y = Convert.ToInt32(textArray2(1))
                Next num2
                Return pointArray1
            End If
        End If
        Return Nothing
    End Function

    '/ <summary>
    '/ This method extracts commonly used values of parameters from the context
    '/ </summary>
    '/ <remarks>The url originating from the client contains the command and various parameters. This method extracts the most commonly used
    '/ parameter values
    '/ </remarks>
    Public Overridable Sub ParseContext()
        _name = HttpContext.Current.Request(CommandKey)
        _mapAlias = HttpContext.Current.Request(MapAliasKey)
        _mapWidth = System.Convert.ToInt32(HttpContext.Current.Request(WidthKey))
        _mapHeight = System.Convert.ToInt32(HttpContext.Current.Request(HeightKey))
        _border = (System.Convert.ToInt32(HttpContext.Current.Request(BorderKey)) = 1)
        _commandData = HttpContext.Current.Request(PointsKey)
        _exportFormat = HttpContext.Current.Request(ExportFormatKey)
    End Sub

    '/ <summary>
    '/ Method to execute the command
    '/ </summary>
    '/ <remarks>The default implementation calls user implemented Restorestate before and Savestate after Process method. Process method is where
    '/ actual operation is done.
    '/ </remarks>
    Public Overridable Sub Execute()
        Dim sm As StateManager = StateManager.GetStateManagerFromSession()
        If StateManager.IsManualState() Then
            If sm Is Nothing Then
                Throw New NullReferenceException(L10NUtils.Resources.GetString(StateManager.StateManagerResErr1))
            End If
        End If

        ParseContext()

        If Not sm Is Nothing Then
            PrepareStateManagerParamsDictionary(sm)
            sm.RestoreState()
        End If

        Process()

        If Not sm Is Nothing Then
            sm.SaveState()
        End If
    End Sub

    Public Overridable Sub PrepareStateManagerParamsDictionary(ByVal sm As StateManager)
        If Not sm Is Nothing Then
            sm.ParamsDictionary.Item(StateManager.ActiveMapAliasKey) = Me.MapAlias
        End If
    End Sub

    '/ <summary>
    '/ Abstract method which does the actual processing
    '/ </summary>
    '/ <remarks>This method should contain the actual business logic to do operations.</remarks>
    Public MustOverride Sub Process()
End Class

'/ <summary>
'/ Class for the command GetMap
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class GetMap
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = GetMapCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and exports it to memory stream and streams it back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command ZoomIn
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class ZoomIn
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = ZoomInCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and zooms it using two screen points and streams the image
    '/ back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.Zoom(MapAlias, points(0), points(1), True)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command ZoomOut
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class ZoomOut
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = ZoomOutCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and zooms out using two screen points and streams the image
    '/ back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.Zoom(MapAlias, points(0), points(1), False)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command Center
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class Center
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = CenterCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and centers it using screen point and streams the image
    '/ back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.Center(MapAlias, points(0))
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command Pan
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class Pan
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = PanCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and pans it using start and end screen points 
    '/ and streams the image back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.Pan(MapAlias, points(0), points(1))
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command Pan
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class Navigate
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = NavigateCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and pans it either by specified map units or percentage of
    '/ map on the screen and streams the image back to client.
    '/ </summary>
    '/ <remarks>The map is panned by using the method and north and east offsets.</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim north As Double = System.Convert.ToDouble(HttpContext.Current.Request("North"))
        Dim east As Double = System.Convert.ToDouble(HttpContext.Current.Request("East"))
        Dim method As String = HttpContext.Current.Request("Method")
        If method.Equals("ByUnit") Then
            model.Pan(MapAlias, north, east)
        ElseIf method.Equals("ByPercentage") Then
            Dim xoffset As Integer = CType((east * 0.01 * MapWidth), Integer)
            Dim yoffset As Integer = CType((north * 0.01 * MapHeight), Integer)
            model.Pan(MapAlias, xoffset, yoffset)
        End If
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command Distance
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class Distance
    Inherits MapBaseCommand
    '/ <summary>
    '/ Key to be used to get the distance type parameter value from the URL.
    '/ </summary>
    Protected Shared DistanceTypeKey As String = "DistanceType"

    '/ <summary>
    '/ Key to be used to get the distance unit parameter value from the URL.
    '/ </summary>
    Protected Shared DistanceUnitKey As String = "DistanceUnit"

    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = DistanceCommand
    End Sub
    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and calculates the total distance between given points
    '/ and writes the double value to the response object.
    '/ </summary>
    '/ <remarks>This command is different than others as it doesn't update the map, but returns the information about it. The UI to be used
    '/ to display this value has to be decided by the javascript which receives it. Distance command from the client side uses XMLHttp object
    '/ to make call to server to calculate the distance.
    '/ </remarks>
    Public Overrides Sub Process()
        Dim distanceType As String = System.Convert.ToString(HttpContext.Current.Request(DistanceTypeKey))
        Dim distanceUnit As String = System.Convert.ToString(HttpContext.Current.Request(DistanceUnitKey))
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        Dim dist As Double = 0.0
        Try
            dist = model.Distance(MapAlias, points, distanceType, distanceUnit)
        Catch ex As Exception
            HttpContext.Current.Response.Output.Write(ex.Message)
            Return
        End Try
        HttpContext.Current.Response.Output.Write(dist)
    End Sub
End Class

'/ <summary>
'/ Class for the command PointSelection
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class PointSelection
    Inherits MapBaseCommand
    Public Sub New()
        Name = PointSelectionCommand
    End Sub
    '/// <summary>
    '/// Key to be used to get the pixel tolerance parameter value from the URL.
    '/// </summary>
    Protected Shared PixelToleranceKey As String = "PixelTolerance"

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and This method searches all features near a given point in all visible and selectable layers 
    '/ and then updates the default selection and streams the image back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim pixelTolerance As Integer = System.Convert.ToInt32(HttpContext.Current.Request(PixelToleranceKey))
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.PointSelection(MapAlias, points(0), pixelTolerance)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command Rectangle selection
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class RectangleSelection
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = RectangleSelectionCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and searches for all features whose centroids are within 
    '/ the given rectangle and updates the default selection and streams the image back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        If (points.Length = 1) Then
            model.PointSelection(MapAlias, points(0))
        ElseIf (points.Length = 2 And (points(0).Equals(points(1)))) Then
            model.PointSelection(MapAlias, points(0))
        Else
            model.RectangleSelection(MapAlias, points(0), points(1))
        End If
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command RadiusSelection
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class RadiusSelection
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = RadiusSelectionCommand
    End Sub

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and searches for all features whose centroids are within 
    '/ the given radius and updates the default selection and streams the image back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        If (points.Length = 1) Then
            model.PointSelection(MapAlias, points(0))
        ElseIf (points.Length = 2) And ((points(0).Equals(points(1)) Or (points(1).X = 0))) Then
            model.PointSelection(MapAlias, points(0))
        Else
            model.RadiusSelection(MapAlias, points(0), points(1).X)
        End If
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command PolygonSelection
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class PolygonSelection
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = PolygonSelectionCommand
    End Sub
    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and searches for all features whose centroids are within 
    '/ the given polygon and updates the default selection and streams the image back to client.
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)
        Dim points() As System.Drawing.Point = ExtractPoints(DataString)
        model.PolygonSelection(MapAlias, points)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command LayerVisibility
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class LayerVisibility
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = LayerVisibilityCommand
    End Sub
    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and sets the visibility of given layer
    '/ and streams the image back to client.
    '/ </summary>
    '/ <remarks>
    '/ This command is execute from LayerControl. The LayerControl is set up where when visibility checkbox is checked, it
    '/ uses XMLHttp object to make call to server with given layername and visibility. There is no submit button for LayerControl therefore
    '/ every trip to server contains just one layername
    '/ </remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        Dim layerAlias As String = HttpContext.Current.Request("LayerAlias")
        Dim layerType As String = HttpContext.Current.Request("LayerType")
        Dim visible As Boolean = (HttpContext.Current.Request("Visible") = "true")
        model.SetLayerVisibility(MapAlias, layerAlias, layerType, visible)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

'/ <summary>
'/ Class for the command ZoomWithFactor
'/ </summary>
'/ <remarks>None</remarks>
<Serializable()> _
Public Class ZoomToLevel
    Inherits MapBaseCommand
    '/ <summary>
    '/ Constructor for this command, sets the name of the command
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Sub New()
        Name = ZoomToLevelCommand
    End Sub
    Private _zoomlevel As Double
    Public Property ZoomLevel() As Double
        Get
            Return _zoomlevel
        End Get
        Set(ByVal Value As Double)
            _zoomlevel = Value
        End Set
    End Property

    '/ <summary>
    '/ This method gets the map object out of the mapfactory with given mapalias and zooms it using given zoomfactor
    '/ and streams the image back to client.
    '/ </summary>
    '/ <param name="model">Model to be used to get map in memory stream</param>
    '/ <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        ZoomLevel = System.Convert.ToDouble(HttpContext.Current.Request("ZoomLevel"))
        model.Zoom(MapAlias, -1.0, ZoomLevel)
        Dim ms As MemoryStream = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat)
        StreamImageToClient(ms)
    End Sub
End Class

