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

            If myMap Is Nothing Then
                Return
            End If
            ' Restore everything we saved explictly.
            AppStateManager.RestoreZoomCenterState(myMap)
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("WorkingTable")
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("WorkingLayer")

            ' Get the workingLayerName saved in WebForm1.aspx page_load.
            Dim workingLayerName As String = CType(Me.ParamsDictionary("WorkingLayerName"), String)
            Dim indexOfLayer As Integer = myMap.Layers.IndexOf(myMap.Layers(workingLayerName))
            ' Move the working layer to the Top, so it can be displayed.
            myMap.Layers.Move(indexOfLayer, 0)
        End If
    End Sub

    ' Save the state
    Public Overrides Sub SaveState()
        Dim mapAlias As String = CType(Me.ParamsDictionary(AppStateManager.ActiveMapAliasKey), String)
        Dim myMap As MapInfo.Mapping.Map = GetMapObj(mapAlias)

        If myMap Is Nothing Then
            Return
        End If

        ' Note: for performance reasons, only save the map's center and zoom.	
        AppStateManager.SaveZoomCenterState(myMap)

        ' Get the workingLayerName saved in WebForm1.aspx page_load.
        Dim workingLayerName As String = CType(Me.ParamsDictionary("WorkingLayerName"), String)
        ' Save the map's Working table and layer
        Dim workingLayer As MapInfo.Mapping.FeatureLayer = CType(myMap.Layers(workingLayerName), MapInfo.Mapping.FeatureLayer)
        Dim workingTable As MapInfo.Data.Table = Nothing
        If (Not workingLayer Is Nothing) Then workingTable = workingLayer.Table

        ManualSerializer.SaveMapXtremeObjectIntoHttpSession(workingTable, "WorkingTable")
        ManualSerializer.SaveMapXtremeObjectIntoHttpSession(workingLayer, "WorkingLayer")
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
