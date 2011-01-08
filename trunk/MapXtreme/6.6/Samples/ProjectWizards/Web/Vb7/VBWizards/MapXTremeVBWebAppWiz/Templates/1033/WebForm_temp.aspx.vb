imports MapInfo
Imports MapInfo.WebControls
Imports MapInfo.Mapping
Imports MapInfo.Engine

Public Class [!output SAFE_CLASS_NAME]
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
		' If the StateManager doesn't exist in the session put it else get it.
		If StateManager.GetStateManagerFromSession() Is Nothing Then
			StateManager.PutStateManagerInSession(New AppStateManager())
		End If

		'Now Restore State
        'Put the mapalias used by mapcontrol
        StateManager.GetStateManagerFromSession().ParamsDictionary(StateManager.ActiveMapAliasKey) = MapControl1.MapAlias
        StateManager.GetStateManagerFromSession().RestoreState()
    End Sub

	'At the time of unloading the page, save the state
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        StateManager.GetStateManagerFromSession().SaveState()
    End Sub
End Class

'/ <summary>
'/ State management can be complex operation. It is efficient to save and restore what is needed.
'/ The method used here is described in the BEST PRACTISES documentation. This is a template application
'/ which changes zoom, center, default selection and layer visibility. Hence we save and restore only these objects.
'/ </summary>
<Serializable()> _
Public Class AppStateManager
    Inherits StateManager
    Private _session As ManualSerializer = Nothing

    Public Sub New()
        _session = New ManualSerializer
    End Sub

    Private Function GetMapObj(ByVal mapAlias As String) As Map
        Dim map1 As Map = Nothing
        If (mapAlias Is Nothing) Then
            map1 = MapInfo.Engine.Session.Current.MapFactory(0)
        ElseIf (mapAlias.Length <= 0) Then
            map1 = MapInfo.Engine.Session.Current.MapFactory(0)
        Else
            map1 = MapInfo.Engine.Session.Current.MapFactory(mapAlias)
            If (map1 Is Nothing) Then
                map1 = MapInfo.Engine.Session.Current.MapFactory(0)
            End If
        End If
        Return map1
    End Function

    ' Restore the state
    Public Overrides Sub RestoreState()
        Dim mapAlias As String = ParamsDictionary(ActiveMapAliasKey)
        Dim map As Map = GetMapObj(mapAlias)

        ' If it was user's first time and the session was not dirty then save this default state to be applied later.
        ' If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
        If IsUsersFirstTime() Then
            If IsDirtyMapXtremeSession() Then
                RestoreDefaultState(map)
            Else
                SaveDefaultState(map)
            End If
        Else
            ' If it is not user's first time then restore the last state they saved
            RestoreZoomCenterState(map)
            ' Just by setting it to temp variables the objects are serialized into session. There is no need to set them explicitly.
            Dim lyrsTemp As Layers = CType(_session("Layers"), Layers)
            Dim defTemp As Selection = CType(_session("Selection"), Selection)
        End If
    End Sub

    ' Save the state
    Public Overrides Sub SaveState()
        Dim mapAlias As String = ParamsDictionary(ActiveMapAliasKey)
        Dim map As Map = GetMapObj(mapAlias)
        If Not map Is Nothing Then
            SaveZoomCenterState(map)
            _session("Layers") = map.Layers
            _session("Selection") = MapInfo.Engine.Session.Current.Selections.DefaultSelection
        End If
    End Sub


    ' This method checks if the MapXtreme Session from the pool is dirty or clean
    Private Function IsDirtyMapXtremeSession() As Boolean
        ' Check if layers collection in application state if it is already there session is dirty
        Return (Not (MapInfo.Engine.Session.Current.CustomProperties.Item("Dirty") Is Nothing))
    End Function

    ' Check if this is user's first time accessing this page. IF there is a zoom value in the asp.net session then it is not user's first time.
    Private Function IsUsersFirstTime() As Boolean
        Return (HttpContext.Current.Session(StateManager.GetKey("Zoom")) Is Nothing)
    End Function

    ' When the MapXtreme Session is not dirty, save the initial state of the application
    Private Sub SaveDefaultState(ByVal map As Map)
        Dim application As HttpApplicationState = HttpContext.Current.Application
        If application("DefaultZoom") Is Nothing Then
            ' Store default selection
            application("DefaultSelection") = ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Selections.DefaultSelection)
            ' Store layers collection
            application("DefaultLayers") = ManualSerializer.BinaryStreamFromObject(map.Layers)
            ' Store the original zoom and center.
            application("DefaultCenter") = map.Center
            application("DefaultZoom") = map.Zoom
        End If
        ' Set the dirty bit for this MapXtreme Session
        MapInfo.Engine.Session.Current.CustomProperties.Add("Dirty", True)
    End Sub

    ' When th MapXtreme Session is dirty but it is first time for user, this will be applied to give users it's initial state
    Private Sub RestoreDefaultState(ByVal map As Map)
        Dim application As HttpApplicationState = HttpContext.Current.Application
        ' Get the default layers, center, and zoomfrom the Application. Clear Layers first, 
        'this resets the zoom and center which we will set later
        map.Layers.Clear()

        'Just by deserializing the binary stream we reset the MapFactory Deault layers collection
        Dim bytes() As Byte = CType(application("DefaultLayers"), Byte())
        Dim obj As Object = ManualSerializer.ObjectFromBinaryStream(bytes)

        ' For default selection
        bytes = CType(application("DefaultSelection"), Byte())
        obj = ManualSerializer.ObjectFromBinaryStream(bytes)

        ' For zoom and center
        map.Zoom = CType(application("DefaultZoom"), MapInfo.Geometry.Distance)
        map.Center = CType(application("DefaultCenter"), MapInfo.Geometry.DPoint)
    End Sub
End Class

