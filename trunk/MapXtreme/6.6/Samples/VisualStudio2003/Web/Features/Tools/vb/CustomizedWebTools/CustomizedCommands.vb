<Serializable()> _
Public Class AddPinPointCommand
    Inherits MapInfo.WebControls.MapBaseCommand

    ' <summary>
    ' Constructor for this command, sets the name of the command
    ' </summary>
    ' <remarks>None</remarks>
    Public Sub New()
        MyBase.Name = "AddPinPointCommand"
    End Sub
    '/// <summary>
    '/// This method gets the map object out of the mapfactory with given mapalias and 
    '/// Adds a point feature into a temp layer, exports it to memory stream and streams it back to client.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Overrides Sub Process()
        ' Extract points from the string
        Dim points As System.Drawing.Point() = Me.ExtractPoints(MyBase.DataString)
        Dim model As MapInfo.WebControls.MapControlModel = MapInfo.WebControls.MapControlModel.GetModelFromSession()
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MyBase.MapAlias)
        If (map Is Nothing) Then Return
        model.SetMapSize(Me.MapAlias, Me.MapWidth, Me.MapHeight)
        ' There will be only one point, convert it to spatial
        Dim point As MapInfo.Geometry.DPoint
        map.DisplayTransform.FromDisplay(points(0), point)
        Dim lyr As MapInfo.Mapping.IMapLayer = map.Layers.Item(SampleConstants.TempLayerAlias)
        If (lyr Is Nothing) Then
            Dim ti As New MapInfo.Data.TableInfoMemTable(SampleConstants.TempTableAlias)
            ' Make the table mappable
            ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateFeatureGeometryColumn(map.GetDisplayCoordSys()))
            ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateStyleColumn())
            Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti)
            map.Layers.Insert(0, New MapInfo.Mapping.FeatureLayer(table, "templayer", SampleConstants.TempLayerAlias))
        End If
        lyr = map.Layers.Item(SampleConstants.TempLayerAlias)
        If (lyr Is Nothing) Then Return

        Dim fLyr As MapInfo.Mapping.FeatureLayer = CType(lyr, MapInfo.Mapping.FeatureLayer)
        Dim geoPoint As New MapInfo.Geometry.Point(map.GetDisplayCoordSys(), point)
        ' Create a Point style which is a red pin point.
        Dim vs As New MapInfo.Styles.SimpleVectorPointStyle
        vs.Code = 67
        vs.Color = Color.Red
        vs.PointSize = Convert.ToInt16(24)
        vs.Attributes = MapInfo.Styles.StyleAttributes.PointAttributes.BaseAll
        vs.SetApplyAll()

        ' Create a Feature which contains a Point geometry and insert it into temp table.
        Dim pntFeature As New MapInfo.Data.Feature(geoPoint, vs)
        Dim key As MapInfo.Data.Key = fLyr.Table.InsertFeature(pntFeature)

        ' Send contents back to client.
        Dim ms As System.IO.MemoryStream = model.GetMap(MyBase.MapAlias, MyBase.MapWidth, MyBase.MapHeight, MyBase.ExportFormat)
        Me.StreamImageToClient(ms)
    End Sub
End Class

<Serializable()> _
Public Class ClearPinPointCommand
    Inherits MapInfo.WebControls.MapBaseCommand

    ' <summary>
    ' Constructor for this command, sets the name of the command
    ' </summary>
    ' <remarks>None</remarks>
    Public Sub New()
        MyBase.Name = "ClearPinPointCommand"
    End Sub

    '/// <summary>
    '/// This method gets the map object out of the mapfactory with given mapalias 
    '/// and This method delete the pin point features added by AddPinPointCommand in a given point 
    '/// and then streams the image back to client.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim points As Point() = Me.ExtractPoints(MyBase.DataString)
        Dim model As MapInfo.WebControls.MapControlModel = MapInfo.WebControls.MapControlModel.GetModelFromSession()
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MyBase.MapAlias)
        If (map Is Nothing) Then Return
        model.SetMapSize(Me.MapAlias, Me.MapWidth, Me.MapHeight)
        Me.PointDeletion(map, points(0))
        Dim ms As System.IO.MemoryStream = model.GetMap(MyBase.MapAlias, MyBase.MapWidth, MyBase.MapHeight, MyBase.ExportFormat)
        Me.StreamImageToClient(ms)
    End Sub

    ' <summary>
    ' Delete a feature in the temporary layer.
    ' </summary>
    ' <param name="mapAlias">MapAlias of the map</param>
    ' <param name="point">Point in pixels</param>
    Private Sub PointDeletion(ByVal map As MapInfo.Mapping.Map, ByVal point As System.Drawing.Point)
        ' Do the search and show selections
        Dim si As MapInfo.Data.SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map, point, 10)
        CType(si.SearchResultProcessor, MapInfo.Data.ClosestSearchResultProcessor).Options = MapInfo.Data.ClosestSearchOptions.StopAtFirstMatch

        Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.Item(SampleConstants.TempTableAlias)
        If (Not table Is Nothing) Then
            Dim ifc As MapInfo.Data.IResultSetFeatureCollection = MapInfo.Engine.Session.Current.Catalog.Search(table, si)
            Dim f As MapInfo.Data.Feature
            For Each f In ifc
                table.DeleteFeature(f)
            Next
            ifc.Close()
        End If
    End Sub
End Class

<Serializable()> _
Public Class ModifiedRadiusSelectionCommand
    Inherits MapInfo.WebControls.MapBaseCommand
    ' <summary>
    ' Constructor for this command, sets the name of the command
    ' </summary>
    ' <remarks>None</remarks>
    Public Sub New()
        MyBase.Name = "ModifiedRadiusSelectionCommand"
    End Sub

    '/// <summary>
    '/// This method gets the map object out of the mapfactory with given mapalias and 
    '/// This method searches all features within a given point and a radius in all visible and selectable layers 
    '/// except the features picked up by the given point 
    '/// and then updates the default selection and streams the image back to client.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Overrides Sub Process()
        Dim points As Point() = Me.ExtractPoints(MyBase.DataString)
        Dim model As MapInfo.WebControls.MapControlModel = MapInfo.WebControls.MapControlModel.GetModelFromSession()
        Dim myMap As MapInfo.Mapping.Map = model.GetMapObj(MyBase.MapAlias)
        model.SetMapSize(Me.MapAlias, Me.MapWidth, Me.MapHeight)
        Me.RadiusSelection(myMap, points)
        Dim ms As System.IO.MemoryStream = model.GetMap(MyBase.MapAlias, MyBase.MapWidth, MyBase.MapHeight, MyBase.ExportFormat)
        Me.StreamImageToClient(ms)
    End Sub

    ' <summary>
    ' Select all feature with given radius but ones selected by the center point.
    ' </summary>
    ' <remarks>This method searches for all features whose centroids are within the given radius but ones selected by the center point and updates the 
    ' default selection. This method will clear DefaultSelection if radius is 0 or only one click happened in client side.</remarks>
    ' <param name="mapAlias">MapAlias of the map</param>
    ' <param name="myMap">Map object</param>
    Private Sub RadiusSelection(ByVal myMap As MapInfo.Mapping.Map, ByVal points As System.Drawing.Point())
        MapInfo.Engine.Session.Current.Selections.DefaultSelection.Clear()
        ' just return if it is one point only or first and second points are same.
        If (points.Length = 1 Or (points(0).X = points(1).X AndAlso points(0).Y = points(1).Y)) Then Return

        Dim _selFilter As MapInfo.Mapping.IMapLayerFilter = MapInfo.Mapping.MapLayerFilterFactory.FilterForTools( _
                myMap, MapInfo.Mapping.MapLayerFilterFactory.FilterByLayerType(MapInfo.Mapping.LayerType.Normal), _
                MapInfo.Mapping.MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)
        Dim tempAlias As String = "tempSelection"
        Dim iTableEnum As MapInfo.Data.ITableEnumerator = myMap.Layers.GetTableEnumerator(_selFilter)
        If (Not iTableEnum Is Nothing) Then
            Try
                ' Get center and radius
                Dim center As System.Drawing.Point = points(0)
                Dim radius As Integer = points(1).X

                ' search within screen radius.
                Dim si As MapInfo.Data.SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(myMap, center, radius, 20, MapInfo.Data.ContainsType.Centroid)
                MapInfo.Engine.Session.Current.Catalog.Search(iTableEnum, si, MapInfo.Engine.Session.Current.Selections.DefaultSelection, MapInfo.Data.ResultSetCombineMode.AddTo)

                ' Create the temp selection object.
                MapInfo.Engine.Session.Current.Selections.CreateSelection(tempAlias)
                si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(myMap, center, 6)
                MapInfo.Engine.Session.Current.Catalog.Search(iTableEnum, si, MapInfo.Engine.Session.Current.Selections.Item(tempAlias), MapInfo.Data.ResultSetCombineMode.AddTo)
                Dim iEnum As IEnumerator = MapInfo.Engine.Session.Current.Selections.Item(tempAlias).GetEnumerator
                Do While iEnum.MoveNext
                    Dim pntCollection As MapInfo.Data.IResultSetFeatureCollection = CType(iEnum.Current, MapInfo.Data.IResultSetFeatureCollection)
                    Dim radiusCollection As MapInfo.Data.IResultSetFeatureCollection = Nothing
                    Dim index As Integer
                    For index = 0 To MapInfo.Engine.Session.Current.Selections.DefaultSelection.Count - 1
                        ' Need to find out the IResultSetFeatureCollection based on the same BaseTable.
                        If (MapInfo.Engine.Session.Current.Selections.DefaultSelection.Item(index).BaseTable.Alias.Equals(pntCollection.BaseTable.Alias)) Then
                            radiusCollection = MapInfo.Engine.Session.Current.Selections.DefaultSelection.Item(index)
                            Exit For
                        End If
                    Next index
                    If (Not radiusCollection Is Nothing) Then
                        CType(radiusCollection, MapInfo.Data.IFeatureCollection).Remove(pntCollection)
                    End If
                Loop
            Catch exception1 As Exception
                MapInfo.Engine.Session.Current.Selections.DefaultSelection.Clear()
            Finally
                MapInfo.Engine.Session.Current.Selections.Remove(MapInfo.Engine.Session.Current.Selections.Item(tempAlias))
            End Try
        End If
    End Sub
End Class
