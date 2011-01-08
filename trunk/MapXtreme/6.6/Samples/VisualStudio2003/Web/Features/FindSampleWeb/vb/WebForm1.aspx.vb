Imports MapInfo
Imports MapInfo.Data.Find
Imports MapInfo.Data
Imports MapInfo.Geometry
Imports MapInfo.Styles
Imports MapInfo.Mapping.Thematics
Imports MapInfo.Mapping
Imports MapInfo.Mapping.Legends
Imports MapInfo.WebControls

Public Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkButton16 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Private Shared _findColumnName As String = "Country"
    Private Shared _findLayerName As String = "worldcap"
    Private Shared _workingLayerName As String = "WorkingLayer"
    Protected MapControl1 As MapInfo.WebControls.MapControl
    Protected Zoomintool2 As MapInfo.WebControls.ZoomInTool
    Protected Zoomouttool2 As MapInfo.WebControls.ZoomOutTool
    Protected SouthNavigationTool2 As MapInfo.WebControls.SouthNavigationTool
    Protected NorthNavigationTool2 As MapInfo.WebControls.NorthNavigationTool
    Protected EastNavigationTool2 As MapInfo.WebControls.EastNavigationTool
    Protected WestNavigationTool2 As MapInfo.WebControls.WestNavigationTool
    Protected NorthEastNavigationTool1 As MapInfo.WebControls.NorthEastNavigationTool
    Protected SouthWestNavigationTool1 As MapInfo.WebControls.SouthWestNavigationTool
    Protected SouthEastNavigationTool1 As MapInfo.WebControls.SouthEastNavigationTool
    Protected NorthWestNavigationTool1 As MapInfo.WebControls.NorthWestNavigationTool
    Protected ZoomBarTool1 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool2 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool3 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool4 As MapInfo.WebControls.ZoomBarTool
    Protected ZoomBarTool5 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected Centertool2 As MapInfo.WebControls.CenterTool
    Protected Pantool2 As MapInfo.WebControls.PanTool

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Me.Session.IsNewSession Then
            '//******************************************************************************//
            '//*   You need to follow below lines in your own application in order to       *//  
            '//*   save state manually.                                                     *//
            '//*   You don't need this state manager if the "MapInfo.Engine.Session.State"  *//
            '//*   in the web.config is not set to "Manual"                                 *//
            '//******************************************************************************//
            If AppStateManager.IsManualState Then
                Dim stateManager As New AppStateManager
                '// tell the state manager which map alias you want to use.
                '// You could also add your own key/value pairs, the value should be serializable.
                stateManager.ParamsDictionary.Item(stateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias
                '// Add workingLayerName into State manager's ParamsDictionary.
                stateManager.ParamsDictionary.Item("WorkingLayerName") = _workingLayerName
                '// Put state manager into HttpSession, so we could get it later on.
                stateManager.PutStateManagerInSession(stateManager)
            End If
            Me.InitWorkingLayer()
            Dim myMap As Map = MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias)
            '// Set the initial zoom, center and size of the map
            '// This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
            myMap.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
            myMap.Center = New DPoint(27775.805792979896, -147481.33999999985)
            myMap.Size = New System.Drawing.Size(CType(Me.MapControl1.Width.Value, Integer), CType(Me.MapControl1.Height.Value, Integer))
        End If
        '// Restore state.
        If AppStateManager.IsManualState Then
            stateManager.GetStateManagerFromSession.RestoreState()
        End If
    End Sub

    Private Function InitWorkingLayer() As Boolean
        Dim map1 As Map = MapInfo.Engine.Session.Current.MapFactory(MapControl1.MapAlias)
        If ((MapInfo.Engine.Session.Current.MapFactory.Count = 0) OrElse (map1 Is Nothing)) Then
            Return False
        End If

        'Make sure the Find layer's MemTable exists
        Dim table As Table = MapInfo.Engine.Session.Current.Catalog.GetTable(WebForm1._workingLayerName)
        If (table Is Nothing) Then
            Dim ti As New TableInfoMemTable(WebForm1._workingLayerName)
            ti.Temporary = True
            'Add the Geometry column
            Dim column1 As New GeometryColumn(map1.GetDisplayCoordSys)
            column1.Alias = "obj"
            column1.DataType = 7
            ti.Columns.Add(column1)

            'Add the Style column
            Dim column2 As New Column
            column2.Alias = "MI_Style"
            column2.DataType = 11
            ti.Columns.Add(column2)
            table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti)
        End If

        If (table Is Nothing) Then
            Return False
        End If

        'Make sure the Find layer exists
        Dim layer1 As FeatureLayer = map1.Layers(WebForm1._workingLayerName)
        If (layer1 Is Nothing) Then
            layer1 = New FeatureLayer(table, WebForm1._workingLayerName, WebForm1._workingLayerName)
            map1.Layers.Insert(0, layer1)
        End If
        If (layer1 Is Nothing) Then
            Return False
        End If

        'Delete the find object.  There should only be one object in this table.
        CType(layer1.Table, ITableFeatureCollection).Clear()

        Return True
    End Function

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        '// Save state.
        If AppStateManager.IsManualState Then
            MapInfo.WebControls.StateManager.GetStateManagerFromSession.SaveState()
        End If
    End Sub

    Private Sub FindCity()
        Dim find As MapInfo.Data.Find.Find = Nothing

        Try
            Dim map1 As Map = MapInfo.Engine.Session.Current.MapFactory(MapControl1.MapAlias)

            If (MapInfo.Engine.Session.Current.MapFactory.Count = 0) Then
                Return
            End If

            If (map1 Is Nothing) Then
                Return
            End If

            'Do the find
            Dim findLayer As FeatureLayer = map1.Layers(WebForm1._findLayerName)
            find = New MapInfo.Data.Find.Find(findLayer.Table, findLayer.Table.TableInfo.Columns(WebForm1._findColumnName))
            Dim result As FindResult = find.Search(Me.DropDownList1.SelectedItem.Text)
            If result.ExactMatch Then
                'Create a Feature (point) for the location we found
                Dim csys As CoordSys = findLayer.CoordSys
                Dim geometry1 As New Point(csys, result.FoundPoint.X, result.FoundPoint.Y)
                Dim feature1 As New Feature(geometry1, New SimpleVectorPointStyle(52, Color.DarkGreen, 32))

                'Delete the existing find object and add the new one
                Dim workingLayer As FeatureLayer = map1.Layers(WebForm1._workingLayerName)
                If (Not workingLayer Is Nothing) Then
                    CType(workingLayer.Table, ITableFeatureCollection).Clear()
                    workingLayer.Table.InsertFeature(feature1)
                End If

                'Set the map's center and zooom
                map1.Center = New DPoint(result.FoundPoint.X, result.FoundPoint.Y)
                Dim distance2 As MapInfo.Geometry.Distance = New MapInfo.Geometry.Distance(1000, map1.Zoom.Unit)
                map1.Zoom = distance2
            Else
                Me.Label3.Text = "Cannot find the country"
            End If
            find.Dispose()
        Catch exception1 As Exception
            If (Not find Is Nothing) Then
                find.Dispose()
            End If
        End Try
    End Sub

    Private Sub FillDropDown(ByVal tableName As String, ByVal colName As String)
        Dim map1 As Map = MapInfo.Engine.Session.Current.MapFactory(MapControl1.MapAlias)

        If ((MapInfo.Engine.Session.Current.MapFactory.Count = 0) OrElse (map1 Is Nothing)) Then
            Return
        End If

        Me.DropDownList1.Items.Clear()
        Dim fl As FeatureLayer = map1.Layers(tableName)
        Dim t As Table = fl.Table
        Dim con As New MIConnection
        Dim tc As MICommand = con.CreateCommand
        tc.CommandText = "select " & colName & " from " & t.Alias
        con.Open()
        Dim tr As MIDataReader = tc.ExecuteReader
        Do While tr.Read
            Me.DropDownList1.Items.Add(tr.GetString(0))
        Loop
        tc.Cancel()
        tc.Dispose()
        tr.Close()
        con.Close()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not MyBase.IsPostBack Then
            Me.FillDropDown(WebForm1._findLayerName, WebForm1._findColumnName)
        End If
    End Sub

    Private Sub LinkButton16_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton16.Click
        Me.FindCity()
    End Sub
End Class
