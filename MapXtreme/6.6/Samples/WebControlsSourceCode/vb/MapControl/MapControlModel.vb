Imports System.IO
Imports System.Drawing
Imports System.Web.SessionState
Imports System.Xml
Imports System.Web
Imports MapInfo.Data
Imports MapInfo.Engine
Imports MapInfo.GeomeTry
Imports MapInfo.Mapping
Imports MapInfo.Persistence
Imports MapInfo.Tools
<Serializable()> _
    Public Class MapControlModel
    Implements IMapOperations, IRequiresSessionState

    Public Sub New()
        _commands.Add(New GetMap)
        _commands.Add(New ZoomIn)
        _commands.Add(New ZoomOut)
        _commands.Add(New ZoomToLevel)
        _commands.Add(New Center)
        _commands.Add(New Pan)
        _commands.Add(New Navigate)
        _commands.Add(New Distance)
        _commands.Add(New PointSelection)
        _commands.Add(New RectangleSelection)
        _commands.Add(New RadiusSelection)
        _commands.Add(New PolygonSelection)
        _commands.Add(New LayerVisibility)
    End Sub

    Private _commands As ArrayList = New ArrayList

    Public Property Commands() As ArrayList
        Get
            Return _commands
        End Get
        Set(ByVal Value As ArrayList)
            _commands = Value
        End Set
    End Property

    '/ <summary>
    '/ Get command object with given name
    '/ </summary>
    '/ <remarks>None</remarks>
    '/ <param name="name">Name of the command</param>
    '/ <returns>Command object</returns>
    Public Overridable Function GetNamedCommand(ByVal name As String) As MapBaseCommand
        Dim cmd As MapBaseCommand
        For Each cmd In Commands
            If cmd.Name.Equals(name) Then
                Return cmd
            End If
        Next
        Return Nothing
    End Function

    '/ <summary>
    '/ Invoke the command contained in the url when client talks to server.
    '/ </summary>
    '/ <remarks>The client creates a url containing command name and data and calls the controller to do the work. 
    '/ The controller calls this method and passes the context which contains all parameters.
    '/ </remarks>
    '/ <param name="context">HttpContext containing parameters for the command</param>
    Public Overridable Sub InvokeCommand()
        Dim command As String = HttpContext.Current.Request(MapBaseCommand.CommandKey)
        ' Go through list and find the command and execute it
        Dim cmd As MapBaseCommand
        For Each cmd In _commands
            If cmd.Name.Equals(command) Then
                cmd.Execute()
            End If
        Next
    End Sub

    '/ <summary>
    '/ This method creates the default model provided by MapXTreme and sets the object in the ASP.NET session.
    '/ </summary>
    '/ <remarks>This model is extracted from the session and used to do business logic</remarks>
    '/ <param name="context">HttpContext</param>
    '/ <returns>MapControlMode from session</returns>
    Public Shared Function SetDefaultModelInSession() As MapControlModel
        Dim context As HttpContext = HttpContext.Current
        Dim model As MapControlModel = GetModelFromSession()
        If model Is Nothing Then
            model = New MapControlModel
            SetModelInSession(model)
        End If
        Return model
    End Function

    '/ <summary>
    '/ Gets model from ASP.NET session
    '/ </summary>
    '/ <returns></returns>
    Public Shared Function GetModelFromSession() As MapControlModel
        Dim context As HttpContext = HttpContext.Current
        Dim key As String = String.Format("{0}_MapModel", context.Session.SessionID)
        Dim model As MapControlModel = Nothing
        If Not context.Session(key) Is Nothing Then
            model = CType(context.Session(key), MapControlModel)
        End If
        Return model
    End Function

    '/ <summary>
    '/ Set the given model in ASP.NET session
    '/ </summary>
    '/ <remarks>If custom model is used, then this method can be used to set it in session and that will be used instead</remarks?>
    '/ <param name="model">model to be set in the ASP.NET session</param>
    Public Shared Sub SetModelInSession(ByVal model As MapControlModel)
        Dim context As HttpContext = HttpContext.Current
        Dim key As String = String.Format("{0}_MapModel", context.Session.SessionID)
        context.Session(key) = model
    End Sub

    Public Overridable Function GetMap(ByVal mapAlias As String, ByVal mapWidth As Integer, ByVal mapHeight As Integer, ByVal exportFormat As String) As MemoryStream Implements IMapOperations.GetMap
        Dim map As map = Me.GetMapObj(mapAlias)
        map.Size = New Size(mapWidth, mapHeight)
        Dim ef As MapInfo.Mapping.ExportFormat = CType(MapInfo.Mapping.ExportFormat.Parse(GetType(MapInfo.Mapping.ExportFormat), exportFormat), exportFormat)
        Dim memStream As MemoryStream = Nothing
        If (Not map Is Nothing) Then
            Dim mapExport As New mapExport(map)
            mapExport.ExportSize = New ExportSize(mapWidth, mapHeight)
            memStream = New MemoryStream
            mapExport.Format = ef
            mapExport.Export(memStream)
            memStream.Position = 0
            mapExport.Dispose()
        End If
        Return memStream
    End Function
    '/ <summary>
    '/ Set maps size
    '/ </summary>
    '/ <param name="mapAlias">MapAlias of the map</param>
    '/ <param name="width">Width to be set</param>
    '/ <param name="height">Height to be set</param>
    Public Overridable Sub SetMapSize(ByVal mapAlias As String, ByVal width As Integer, ByVal height As Integer) Implements IMapOperations.SetMapSize
        Dim map As map = GetMapObj(mapAlias)
        map.Size = New Size(width, height)
    End Sub

    Public Overridable Function GetMapObj(ByVal mapAlias As String) As Map
        Dim map1 As Map = Nothing
        If (mapAlias Is Nothing) Then
            map1 = Session.Current.MapFactory(0)
        ElseIf (mapAlias.Length <= 0) Then
            map1 = Session.Current.MapFactory(0)
        Else
            map1 = Session.Current.MapFactory(mapAlias)
            If (map1 Is Nothing) Then
                map1 = Session.Current.MapFactory(0)
            End If
        End If
        Return map1
    End Function

    Public Overridable Sub Center(ByVal mapAlias As String, ByVal point As System.Drawing.Point) Implements IMapOperations.Center
        Dim dPnt As DPoint
        Dim map As map = Me.GetMapObj(mapAlias)
        dPnt = New DPoint
        map.DisplayTransform.FromDisplay(point, dPnt)
        map.SetView(dPnt, map.GetDisplayCoordSys, map.Zoom)
    End Sub

    Public Overridable Function Distance(ByVal mapAlias As String, ByVal points As System.Drawing.Point(), ByVal distanceType As String, ByVal distanceUnit As String) As Double Implements IMapOperations.Distance
        Dim distType As MapInfo.Geometry.DistanceType = MapInfo.Geometry.DistanceType.Parse(GetType(MapInfo.Geometry.DistanceType), distanceType)
        Dim distunit As MapInfo.Geometry.DistanceUnit = MapInfo.Geometry.DistanceUnit.Parse(GetType(MapInfo.Geometry.DistanceUnit), distanceUnit)

        Dim map As map = Me.GetMapObj(mapAlias)
        Distance = 0
        Dim i As Integer
        For i = 0 To (points.Length - 1) - 1
            Dim point1 As DPoint
            Dim point2 As DPoint
            map.DisplayTransform.FromDisplay(points(i), point1)
            map.DisplayTransform.FromDisplay(points((i + 1)), point2)
            Distance = (Distance + map.GetDisplayCoordSys.Distance(distType, distunit, point1, point2))
        Next i
        Return Distance
    End Function

    Public Overridable Sub Pan(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point) Implements IMapOperations.Pan
        Dim map As map = Me.GetMapObj(mapAlias)
        map.Pan((-1 * (point1.X - point2.X)), (-1 * (point1.Y - point2.Y)))
    End Sub

    Public Overridable Sub Pan(ByVal mapAlias As String, ByVal north As Double, ByVal east As Double) Implements IMapOperations.Pan
        Dim map As map = GetMapObj(mapAlias)
        map.Pan(north, east, map.Zoom.Unit, True)
    End Sub

    Public Overridable Sub Pan(ByVal mapAlias As String, ByVal xOffset As Integer, ByVal yOffset As Integer) Implements IMapOperations.Pan
        Dim map As map = GetMapObj(mapAlias)
        map.Pan(xOffset, yOffset)
    End Sub

    Public Overridable Sub PointSelection(ByVal mapAlias As String, ByVal point As System.Drawing.Point) Implements IMapOperations.PointSelection
        PointSelection(mapAlias, point, 6)
    End Sub
    Public Overridable Sub PointSelection(ByVal mapAlias As String, ByVal point As System.Drawing.Point, ByVal pixelTolerance As Integer) Implements IMapOperations.PointSelection
        Dim map As map = Me.GetMapObj(mapAlias)

        Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map, point, pixelTolerance)
        DirectCast(si.SearchResultProcessor, ClosestSearchResultProcessor).Options = ClosestSearchOptions.StopAtFirstMatch

        Dim d As MapInfo.Geometry.Distance = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(map, pixelTolerance)
        CType(si.SearchResultProcessor, ClosestSearchResultProcessor).DistanceUnit = d.Unit
        CType(si.SearchResultProcessor, ClosestSearchResultProcessor).MaxDistance = d.Value

        Session.Current.Selections.DefaultSelection.Clear()
        Dim typeArray1 As LayerType() = New LayerType(1 - 1) {}
        Dim filter1 As IMapLayerFilter = MapLayerFilterFactory.FilterForTools(map, MapLayerFilterFactory.FilterByLayerType(typeArray1), MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)
        Dim enumerator1 As ITableEnumerator = map.Layers.GetTableEnumerator(filter1)
        If (Not enumerator1 Is Nothing) Then
            Session.Current.Catalog.Search(enumerator1, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo)
        End If
    End Sub

    Public Overridable Sub PolygonSelection(ByVal mapAlias As String, ByVal points As System.Drawing.Point()) Implements IMapOperations.PolygonSelection
        Dim map As map = Me.GetMapObj(mapAlias)

        Dim dpnts As DPoint() = New DPoint(points.Length - 1) {}
        Dim indx As Integer
        For indx = 0 To points.Length - 1
            map.DisplayTransform.FromDisplay(points(indx), dpnts(indx))
        Next indx
        Dim dispCSys As CoordSys = map.GetDisplayCoordSys
        Dim geomCSys As CoordSys = Session.Current.CoordSysFactory.CreateCoordSys(dispCSys.Type, dispCSys.Datum, dispCSys.Units, dispCSys.OriginLongitude, dispCSys.OriginLatitude, dispCSys.StandardParallelOne, dispCSys.StandardParallelTwo, dispCSys.Azimuth, dispCSys.ScaleFactor, dispCSys.FalseEasting, dispCSys.FalseNorthing, dispCSys.Range, map.Layers.Bounds, dispCSys.AffineTransform)
        Dim mp As New MultiPolygon(geomCSys, 0, dpnts)
        Dim si As SearchInfo = Data.SearchInfoFactory.SearchWithinGeometry(mp, ContainsType.Centroid)
        Session.Current.Selections.DefaultSelection.Clear()
        Dim filter1 As IMapLayerFilter = MapLayerFilterFactory.FilterForTools(map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)
        Dim table As ITableEnumerator = map.Layers.GetTableEnumerator(filter1)
        If (Not table Is Nothing) Then
            Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo)
        End If
    End Sub

    Public Overridable Sub RadiusSelection(ByVal mapAlias As String, ByVal center As System.Drawing.Point, ByVal radius As Integer) Implements IMapOperations.RadiusSelection
        Dim map As map = Me.GetMapObj(mapAlias)
        Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(map, center, radius, 20, ContainsType.Centroid)
        Session.Current.Selections.DefaultSelection.Clear()
        Dim _selFilter As IMapLayerFilter = MapLayerFilterFactory.FilterForTools(map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)
        Dim table As ITableEnumerator = map.Layers.GetTableEnumerator(_selFilter)
        If (Not table Is Nothing) Then
            Session.Current.Catalog.Search(table, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo)
        End If
    End Sub

    Public Overridable Sub RectangleSelection(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point) Implements IMapOperations.RectangleSelection
        Dim rect As System.Drawing.Rectangle
        Dim map As map = Me.GetMapObj(mapAlias)

        rect = New System.Drawing.Rectangle(point1.X, point1.Y, 0, 0)
        rect.Width = Math.Abs(CType((point2.X - point1.X), Integer))
        rect.Height = Math.Abs(CType((point2.Y - point1.Y), Integer))
        Dim info1 As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRect(map, rect, ContainsType.Centroid)
        Session.Current.Selections.DefaultSelection.Clear()
        Dim _selFilter As IMapLayerFilter = MapLayerFilterFactory.FilterForTools(map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)
        Dim table As ITableEnumerator = map.Layers.GetTableEnumerator(_selFilter)
        If (Not table Is Nothing) Then
            Session.Current.Catalog.Search(table, info1, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo)
        End If
    End Sub

    Public Overridable Sub Zoom(ByVal mapAlias As String, ByVal point1 As System.Drawing.Point, ByVal point2 As System.Drawing.Point, ByVal zoomIn As Boolean) Implements IMapOperations.Zoom
        Dim map As map = Me.GetMapObj(mapAlias)
        If zoomIn Then
            map.SetView(point1, point2, MapInfo.Mapping.ZoomType.ZoomIn)
        Else
            map.SetView(point1, point2, MapInfo.Mapping.ZoomType.ZoomOut)
        End If
    End Sub

    Public Overridable Sub Zoom(ByVal mapAlias As String, ByVal zoomFactor As Double, ByVal zoomLevel As Double) Implements IMapOperations.Zoom
        Dim map As map = GetMapObj(mapAlias)
        ' Decide what type of zoom to use depending upon whether the value is negative
        If zoomFactor > 0.0 Then
            map.Zoom = New MapInfo.Geometry.Distance(map.Zoom.Value * zoomFactor, map.Zoom.Unit)
        Else
            map.Zoom = New MapInfo.Geometry.Distance(zoomLevel, map.Zoom.Unit)
        End If
    End Sub

    '<summary>
    ' Sets a given layer's visibility for a given map.
    ' </summary>
    ' <remarks>This method sets the layers visibility with a given layer alias to <c>true</c> or <c>false</c>. 
    'If the layer type is a feature layer, then it also sets the visibility for all modifers. If it is a group layer then it sets the visibility for all internal layers. 
    ' If it is a label layer then it sets the visibility of all label sources. If the layer type passed is either a modifier or label source then it finds the modifier or label and sets the visibility.
    ' </remarks>
    ' <param name="mapAlias">MapAlias of the map.</param>
    ' <param name="layerAlias">LayerAlias of the layer.</param>
    ' <param name="lyrType">Type of the layer.</param>
    ' <param name="visible">Visibility to be set.</param>

    Public Overridable Sub SetLayerVisibility(ByVal mapAlias As String, ByVal layerAlias As String, ByVal lyrType As String, ByVal visible As Boolean) Implements IMapOperations.SetLayerVisibility
        Dim map As map = Me.GetMapObj(mapAlias)
        Dim lenum As MapLayerEnumerator = map.Layers.GetMapLayerEnumerator(MapLayerEnumeratorOptions.Recurse)
        Dim lyr As IMapLayer
        For Each lyr In lenum
            If (lyr.Alias.Equals(layerAlias) AndAlso lyrType.Equals(lyr.Type.ToString)) Then
                lyr.Enabled = visible
                If (lyr.Type = LayerType.Normal) Then
                    'Go through modifiers to set visibility						
                    Dim mods As FeatureStyleModifiers = CType(lyr, FeatureLayer).Modifiers
                    Dim fsm As FeatureStyleModifier
                    For Each fsm In mods
                        fsm.Enabled = visible
                    Next
                    Exit For
                End If

                If (lyr.Type = LayerType.Group) Then
                    'Go through collection to set visibility
                    Dim inlayer As IMapLayer
                    For Each inlayer In CType(lyr, GroupLayer)
                        inlayer.Enabled = visible
                    Next
                    Exit For
                End If

                If (lyr.Type = LayerType.Label) Then
                    'go through label sources to set visibility
                    Dim source As MapInfo.Mapping.LabelSource
                    For Each source In CType(lyr, MapInfo.Mapping.LabelLayer).Sources
                        source.Enabled = visible
                        Dim lm As MapInfo.Mapping.LabelModifier
                        For Each lm In source.Modifiers
                            lm.Enabled = visible
                        Next
                    Next
                    Exit For
                End If
            Else
                'The layeralias did not match, hence the alias may be modifier or label source
                'If it is modifier find it 
                If (lyrType.Equals("Mod")) Then
                    If (lyr.Type = LayerType.Normal) Then
                        Dim mods As FeatureStyleModifiers = CType(lyr, FeatureLayer).Modifiers
                        Dim fsm As FeatureStyleModifier
                        For Each fsm In mods
                            If (fsm.Alias.Equals(layerAlias)) Then
                                fsm.Enabled = visible
                            End If
                        Next
                    End If
                End If

                'If it is label source find it and set it's visibility
                If (lyrType.Equals("Label")) Then
                    If (lyr.Type = LayerType.Label) Then
                        Dim source As MapInfo.Mapping.LabelSource
                        For Each source In CType(lyr, MapInfo.Mapping.LabelLayer).Sources
                            If (source.Alias.Equals(layerAlias)) Then
                                source.Enabled = visible
                                Dim lm As MapInfo.Mapping.LabelModifier
                                For Each lm In source.Modifiers
                                    lm.Enabled = visible
                                Next
                            End If
                        Next
                    End If
                End If

                'If it is label source modifier then set it's visibility
                If (lyrType.Equals("LabelMod")) Then
                    If (lyr.Type = LayerType.Label) Then
                        Dim source As MapInfo.Mapping.LabelSource
                        Dim lm As MapInfo.Mapping.LabelModifier
                        For Each source In CType(lyr, MapInfo.Mapping.LabelLayer).Sources
                            For Each lm In source.Modifiers
                                If (lm.Alias.Equals(layerAlias)) Then
                                    lm.Enabled = visible
                                End If
                            Next
                        Next
                    End If
                End If
            End If
        Next
    End Sub
End Class

