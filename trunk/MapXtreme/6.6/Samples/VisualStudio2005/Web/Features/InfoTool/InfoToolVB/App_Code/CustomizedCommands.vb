Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports MapInfo.Mapping
Imports MapInfo.Data
Imports MapInfo.WebControls


Namespace InfoToolWeb



''' <summary>
''' Info command for InfoWebTool.
''' </summary>
<Serializable()> _
Public Class Info
    Inherits MapInfo.WebControls.MapBaseCommand

    ' Key to be used to get the pixel tolerance parameter value from the URL.
    Protected PixelToleranceKey As String = "PixelTolerance"
    Protected InfoCommand As String = "Info"

    ''' <summary>
    ''' Constructor for Info class
    ''' </summary>
    Public Sub New()
        Name = InfoCommand
    End Sub


    ''' <summary>
    ''' Override the Execute method in MapBasicCommand class to not save state, because
    ''' for info tool, which does not change map state, so there is no need to save map state.
    ''' </summary>
    Public Overrides Sub Execute()

        Dim sm As StateManager = StateManager.GetStateManagerFromSession()
        If sm Is Nothing Then
            If StateManager.IsManualState() Then
                Throw New NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.")
            End If
        End If
        ParseContext()
        If Not (sm Is Nothing) Then
            PrepareStateManagerParamsDictionary(sm)
            sm.RestoreState()
        End If

        Process()
    End Sub


    ''' <summary>
    ''' method to do the real server side process for info tool.
    ''' </summary>
    Public Overrides Sub Process()
        'get pixel tolerance from url of client side.
        Dim pixelTolerance As Integer = System.Convert.ToInt32(HttpContext.Current.Request(PixelToleranceKey))

        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        model.SetMapSize(MapAlias, MapWidth, MapHeight)

        'extract points from url of client side.
        Dim points As System.Drawing.Point() = ExtractPoints(DataString)

        'do searching and get results back
        Dim mrfc As MultiResultSetFeatureCollection = RetrieveInfo(points, pixelTolerance)

        Dim resultEnum As IEnumerator = mrfc.GetEnumerator()

        'retrieve the selected feature from collection
        While resultEnum.MoveNext()
            Dim irfc As IResultSetFeatureCollection = CType(resultEnum.Current, IResultSetFeatureCollection)
            Dim ftrEnum As IFeatureEnumerator = irfc.GetFeatureEnumerator()

            While ftrEnum.MoveNext()
                Dim ftr As Feature = CType(ftrEnum.Current, Feature)
                'create a html table to display feature info and stream back to client side.
                CreateInfoTable(ftr)
                irfc.Close()
                mrfc.Clear()
                Exit While
            End While
            Exit While
        End While
    End Sub


    ''' <summary>
    ''' Creates html table to hold passed in feature info, and stream back to client.
    ''' </summary>
    ''' <param name="ftr">feature object</param>
    Private Sub CreateInfoTable(ByVal ftr As Feature)
        'create a table control and populat it with the column name/value(s) from the feature returned and the name of the layer where the feature belong
        Dim infoTable As New System.Web.UI.WebControls.Table
        'set table attribute/styles
        infoTable.CellPadding = 4
        infoTable.Font.Name = "Arial"
        infoTable.Font.Size = New FontUnit(8)
        infoTable.BorderWidth = New Unit(1)
        'infoTable.BorderStyle = BorderStyle.Outset; 
        Dim backColor As System.Drawing.Color = Color.Bisque

        'add the first row, the layer name/value where the selected feature belongs 
        Dim r As New TableRow
        r.BackColor = backColor

        Dim c As New TableCell
        c.Font.Bold = True
        c.ForeColor = Color.Indigo

        c.Text = "Layer Name"
        r.Cells.Add(c)

        c = New TableCell

        'the feature returned is from a resultset table whose name is got from appending _2 to the real table name, so below is to get the real table name.
        Dim [alias] As String = ftr.Table.Alias
        c.Text = [alias].Substring(0, [alias].Length - 2)
        c.Font.Bold = True
        r.Cells.Add(c)

        infoTable.Rows.Add(r)

        Dim col As Column
        For Each col In ftr.Columns
            Dim upAlias As [String] = col.Alias.ToUpper()
            'don't display obj, MI_Key or MI_Style columns
            If upAlias <> "OBJ" And upAlias <> "MI_STYLE" And upAlias <> "MI_KEY" Then
                r = New TableRow
                r.BackColor = backColor

                r.Cells.Clear()
                c = New TableCell
                c.Text = col.Alias
                c.Font.Bold = True
                c.ForeColor = Color.RoyalBlue

                r.Cells.Add(c)
                c = New TableCell
                c.Text = ftr(col.Alias).ToString()
                r.Cells.Add(c)
                infoTable.Rows.Add(r)
            End If
        Next col

        'stream the html table back to client
        Dim sw As New StringWriter
        Dim hw As New HtmlTextWriter(sw)
        infoTable.RenderControl(hw)
        Dim strHTML As [String] = sw.ToString()
        HttpContext.Current.Response.Output.Write(strHTML)
    End Sub 'CreateInfoTable


    ''' <summary>
    ''' Get a MultiFeatureCollection containing features in all layers falling into the tolerance of the point.
    ''' </summary>
    ''' <param name="points">points array</param>
    ''' <param name="pixelTolerance">pixel tolerance used when searching</param>
    ''' <returns>Returns a MultiResultSetFeatureCollection object</returns>
    Protected Function RetrieveInfo(ByVal points() As Point, ByVal pixelTolerance As Integer) As MultiResultSetFeatureCollection
        If points.Length <> 1 Then
            Return Nothing
        End If
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        'get map object from map model
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MapAlias)

        If map Is Nothing Then
            Return Nothing
        End If
        'creat a layer filter to include normal visible layers for searching
        Dim layerFilter As IMapLayerFilter = MapLayerFilterFactory.FilterForTools(map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(True), "MapInfo.Tools.MapToolsDefault.SelectLayers", Nothing)

        Dim tableEnum As ITableEnumerator = map.Layers.GetTableEnumerator(layerFilter)

        'return if there is no valid layer to search
        If tableEnum Is Nothing Then
            Return Nothing
        End If
        Dim center As System.Drawing.Point = points(0)

        'create a SearchInfo with a point and tolerance
        Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map, center, pixelTolerance)
        CType(si.SearchResultProcessor, ClosestSearchResultProcessor).Options = ClosestSearchOptions.StopAtFirstMatch
        si.QueryDefinition.Columns = Nothing

        Dim d As MapInfo.Geometry.Distance = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(map, pixelTolerance)
        CType(si.SearchResultProcessor, ClosestSearchResultProcessor).DistanceUnit = d.Unit
        CType(si.SearchResultProcessor, ClosestSearchResultProcessor).MaxDistance = d.Value

        'do search
        Dim mrfc As MultiResultSetFeatureCollection = MapInfo.Engine.Session.Current.Catalog.Search(tableEnum, si)
        Return mrfc
    End Function
End Class


''' <summary>
''' ZoomValue command to write current zoom value to client for display.
''' </summary>
<Serializable()> _
Public Class ZoomValue
    Inherits MapInfo.WebControls.MapBaseCommand

    ''' <summary>
    ''' Constructor for ZoomValue class
    ''' </summary>
    Public Sub New()
        Name = "ZoomValue"
    End Sub 'New


    ''' <summary>
    ''' Override the Execute method in MapBasicCommand class to NOT save state, because for this command, which does not change map state, so there is no need to save map state.
    ''' </summary>
    Public Overrides Sub Execute()

        Dim sm As StateManager = StateManager.GetStateManagerFromSession()
        If sm Is Nothing Then
            If StateManager.IsManualState() Then
                Throw New NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.")
            End If
        End If
        ParseContext()
        If Not (sm Is Nothing) Then
            PrepareStateManagerParamsDictionary(sm)
            sm.RestoreState()
        End If

        Process()
    End Sub


    Public Overrides Sub Process()
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        'get map object from map model
        Dim map As MapInfo.Mapping.Map = model.GetMapObj(MapAlias)
        Dim zoomStr As String = map.Zoom.ToString()
        HttpContext.Current.Response.Output.Write(zoomStr)
    End Sub
End Class

End Namespace


