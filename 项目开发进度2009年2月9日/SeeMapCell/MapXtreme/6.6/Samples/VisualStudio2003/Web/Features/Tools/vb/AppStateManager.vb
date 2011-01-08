Imports System
Imports System.Web
Imports MapInfo.WebControls
Imports MapInfo.Mapping
<Serializable()> _
        Public Class AppStateManager
    Inherits StateManager
    Private _session As ManualSerializer = Nothing

    Public Sub New()
        _session = New ManualSerializer
    End Sub

    Private Function GetMapObj(ByVal mapAlias As String) As Map
        Dim map As Map = Nothing
        If mapAlias = Nothing Or mapAlias.Length <= 0 Then
            map = MapInfo.Engine.Session.Current.MapFactory(0)
        Else
            map = MapInfo.Engine.Session.Current.MapFactory(mapAlias)
            If map Is Nothing Then
                map = MapInfo.Engine.Session.Current.MapFactory(0)
            End If
        End If
        Return map
    End Function

    ' Restore the state
    Public Overrides Sub RestoreState()
        Dim mapAlias As String = CType(ParamsDictionary(ActiveMapAliasKey), String)
        Dim myMap As Map = GetMapObj(mapAlias)

        ' If it was user's first time and the session was not dirty then save this default state to be applied later.
        ' If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
        If IsUsersFirstTime() Then
            If IsDirtyMapXtremeSession() Then
                RestoreDefaultState(myMap)
            Else
                SaveDefaultState(myMap)
            End If
        Else
            AppStateManager.RestoreZoomCenterState(myMap)
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession(GetKey("Selection"))
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("tempTable")
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("tempLayer")
        End If
    End Sub

    ' Save the state
    Public Overrides Sub SaveState()
        Dim mapAlias As String = CType(Me.ParamsDictionary(AppStateManager.ActiveMapAliasKey), String)
        Dim map As MapInfo.Mapping.Map = GetMapObj(mapAlias)

        If Not map Is Nothing Then
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Selections.DefaultSelection, StateManager.GetKey("Selection"))

            MapInfo.WebControls.StateManager.SaveZoomCenterState(map)

            ' Needs this because StateManger doens't have proper function to save them.
            If StateManager.IsManualState() Then
                ' this TempLayer should be always there, otherwise there is a chance to get other customer's TempLayer.
                Dim lyr As MapInfo.Mapping.IMapLayer = map.Layers(SampleConstants.TempLayerAlias)
                If Not lyr Is Nothing Then
                    Dim fLyr As MapInfo.Mapping.FeatureLayer = CType(lyr, MapInfo.Mapping.FeatureLayer)
                    ' Need to serialize the temp table first since the temp layer is based on it.
                    ManualSerializer.SaveMapXtremeObjectIntoHttpSession(fLyr.Table, "tempTable")
                    ManualSerializer.SaveMapXtremeObjectIntoHttpSession(fLyr, "tempLayer")
                End If
            End If
        End If
    End Sub


    ' This method checks if the mapinfo session got from the pool is dirty or clean
    Private Function IsDirtyMapXtremeSession() As Boolean
        ' Check if MapXtreme Session is dirty by looking for our flag
        Return Not MapInfo.Engine.Session.Current.CustomProperties("Dirty") Is Nothing
    End Function

    ' Check if this is user's first time accessing this page. IF there is a zoom value in the asp.net session then it is not user's first time.
    Private Function IsUsersFirstTime() As Boolean
        Return (HttpContext.Current.Session(StateManager.GetKey("Zoom")) Is Nothing)
    End Function

    ' When the session is not dirty these values are initial state of the session.
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
        ' Set the dirty flag in this MapXtreme Session Instance
        MapInfo.Engine.Session.Current.CustomProperties.Add("Dirty", True)
    End Sub

    ' When session is dirty but it is first time for user, this will be applied to give users it's initial state
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
