
Public Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected MapControl1 As MapInfo.WebControls.MapControl
    Protected LayerControl1 As MapInfo.WebControls.LayerControl
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
    Protected Label1 As System.Web.UI.WebControls.Label

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
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj()
        If (myMap Is Nothing) Then Return
        If Me.Session.IsNewSession Then

            '//******************************************************************************//
            '//*   You need to follow below lines in your own application.                  *//
            '//*   You don't need this state manager if the "MapInfo.Engine.Session.State" *//
            '//*   in the web.config is not set to "Manual"                                 *//
            '//******************************************************************************//
            If AppStateManager.IsManualState() Then

                Dim stateManager As New AppStateManager
                '// tell the state manager which map alias you want to use.
                '// You could also add your own key/value pairs, the value should be serializable.
                stateManager.ParamsDictionary.Item(AppStateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias
                '// Put state manager into HttpSession, so we could get it later on.
                AppStateManager.PutStateManagerInSession(stateManager)
            End If
            '// Initial state.
            InitState()

        End If
        ' Restore state.
        If AppStateManager.IsManualState Then
            MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState()
        End If
        ' Add a north arrow IAdornment into the map's adornments collection.
        Me.AddNorthArrowAdornment(myMap)
    End Sub

    Private Sub Page_UnLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        If AppStateManager.IsManualState Then
            MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState()
        End If
    End Sub

    Private Sub InitState()
        Dim myMap As MapInfo.Mapping.Map = Me.GetMapObj
        If (MyBase.Application.Get("HelloWorldWeb") Is Nothing) Then
            Dim iEnum As IEnumerator = MapInfo.Engine.Session.Current.MapFactory.GetEnumerator
            '// Put each map's Layers into a byte[] and keep them in HttpApplicationState for the original layers state.
            Do While iEnum.MoveNext
                Dim tempMap As MapInfo.Mapping.Map = CType(iEnum.Current, MapInfo.Mapping.Map)
                Dim lyrsBits As Byte() = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(tempMap.Layers)
                MyBase.Application.Add((tempMap.Alias & "_layers"), lyrsBits)
            Loop
            '// this is a marker key/value only.
            MyBase.Application.Add("HelloWorldWeb", "Here")
        Else
            '// Set the initial Layers of the map because below reasons:
            '// 1. There is a LayerControl in the web form.
            '// 2. Pooled MapInfo Session objects.
            '// 3. Settings of IMapLayer of the map which is from Pooled Session may not be the one you want.
            '// 4. There is Layers collection with initial state stored in Application level with byte[] format.
            Dim obj As Object = Me.Context.Application.Item((myMap.Alias & "_layers"))
            If (Not obj Is Nothing) Then
                ' deserialization applys "original layers setting" to the one in the current map.
                ' "Object tempObj" is only for compiling purpose, otherwise it is useless, 
                '  because MapXtreme object's deserialization process will put MapXtreme object back to the place it belongs to.
                Dim tempObj As Object = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(CType(obj, Byte()))
            End If
        End If
        ' Set the initial state of the map
        ' This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
        myMap.Zoom = New MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile)
        myMap.Center = New MapInfo.Geometry.DPoint(27775.805792979896, -147481.33999999985)
    End Sub

    Private Function GetMapObj() As MapInfo.Mapping.Map
        ' Get the map
        Dim myMap As MapInfo.Mapping.Map = MapInfo.Engine.Session.Current.MapFactory.Item(Me.MapControl1.MapAlias)
        If (myMap Is Nothing) Then
            myMap = MapInfo.Engine.Session.Current.MapFactory.Item(0)
        End If
        Return myMap
    End Function

    Private Sub AddNorthArrowAdornment(ByVal myMap As MapInfo.Mapping.Map)
        If (myMap Is Nothing) Then Return
        If (myMap.Adornments.Item("north_arrow") Is Nothing) Then
            Dim path As String = HttpContext.Current.Server.MapPath(String.Format("/MapXtremeWebResources {0}_{1}", MapInfo.Engine.ProductInfo.MajorVersion, MapInfo.Engine.ProductInfo.MinorVersion))
            Dim northFile As String = System.IO.Path.Combine(path, "northarrow.bmp")
            myMap.Adornments.Append(New NorthArrowAdornment(myMap.Alias, New Size(100, 100), "north_arrow", "aaaa", northFile))
        End If
    End Sub
End Class
