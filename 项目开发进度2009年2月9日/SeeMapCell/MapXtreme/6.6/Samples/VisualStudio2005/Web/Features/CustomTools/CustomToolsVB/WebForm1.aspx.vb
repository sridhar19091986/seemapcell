
Namespace ToolsSample

Partial Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj()
        If Me.Session.IsNewSession Then
                Dim stateManager As New AppStateManager
                stateManager.ParamsDictionary.Item(AppStateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias
                MapInfo.WebControls.StateManager.PutStateManagerInSession(stateManager)

            ' Add customized web tools
            ' Below line will put controlModel into HttpSessionState.
            Dim model As MapInfo.WebControls.MapControlModel = MapInfo.WebControls.MapControlModel.SetDefaultModelInSession()
            model.Commands.Add(New AddPinPointCommand)
            model.Commands.Add(New ClearPinPointCommand)
            model.Commands.Add(New ModifiedRadiusSelectionCommand)

            ' ****** Set the initial state of the map  *************
            ' Clear up the temp layer left by other customer requests.
            If ((Not myMap Is Nothing)) Then
                If ((Not myMap.Layers.Item(SampleConstants.TempLayerAlias) Is Nothing)) Then
                    myMap.Layers.Remove(SampleConstants.TempLayerAlias)
                End If
            End If
            ' Need to clean up "dirty" temp table left by other customer requests.
            MapInfo.Engine.Session.Current.Catalog.CloseTable(SampleConstants.TempTableAlias)
            ' Need to clear the DefautlSelection.
            MapInfo.Engine.Session.Current.Selections.DefaultSelection.Clear()

            ' Creat a temp table and AddPintPointCommand will add features into it.
            Dim ti As New MapInfo.Data.TableInfoMemTable(SampleConstants.TempTableAlias)
            ' Make the table mappable
            ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateFeatureGeometryColumn(myMap.GetDisplayCoordSys))
            ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateStyleColumn)

            Dim table As MapInfo.Data.Table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti)
            ' Create a new FeatureLayer based on the temp table, so we can see the temp table on the map.
            myMap.Layers.Insert(0, New MapInfo.Mapping.FeatureLayer(table, "templayer", SampleConstants.TempLayerAlias))

            ' This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
            myMap.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
            myMap.Center = New MapInfo.Geometry.DPoint(27775.805792979896, -147481.33999999985)
        End If
        MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState()
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState()
    End Sub

    Private Function GetMapObj() As MapInfo.Mapping.Map
        Dim myMap As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias)
        If (myMap Is Nothing) Then
            myMap = MapInfo.Engine.Session.Current.MapFactory.Item(0)
        End If
        Return myMap
    End Function
End Class

End Namespace

