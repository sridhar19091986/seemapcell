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
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("mdb_table")
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("theme_table")
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("theme_layer")
            ManualSerializer.RestoreMapXtremeObjectFromHttpSession("group_layer")

            ' Important Note:
            '   The saved theme layer, group layer and tables get restored automatically by ASP.NET serialization.
            '   By the time this method is called, this has already happened.

            ' We need to move the GroupLayer to the Top, so the contents within it could be displayed.
            ' This step is necessary since the GroupLayer is created on the fly and not loaded through pre-loaded workspace and 
            ' the position of the groupLayer inside the Layers collection is not guarantee when it get deserialized back 
            ' since we don't save the whole Layers collection for performance purpose in this sample.
            Dim indexOfGroupLayer As Integer = myMap.Layers.IndexOf(myMap.Layers(SampleConstants.GroupLayerAlias))
            If indexOfGroupLayer > 0 Then
                myMap.Layers.Move(indexOfGroupLayer, 0)
                ' Need to turn off other labellayer if we have a demo labellayer in the GroupLayer.
                ' This step is needed if "MapInfo.Engine.Session.Pooled" value="false" in the web.config,
                ' Otherwise you would see other LabelLayers show up on the map.
                WebForm1.HandleLabelLayerVisibleStatus(myMap)
            End If
        End If
    End Sub

    ' Save the state
    Public Overrides Sub SaveState()
        Dim mapAlias As String = CType(Me.ParamsDictionary(AppStateManager.ActiveMapAliasKey), String)
        Dim map As MapInfo.Mapping.Map = GetMapObj(mapAlias)

        If Not map Is Nothing Then
            MapInfo.WebControls.StateManager.SaveZoomCenterState(map)
            ' Note: Please be aware of the order of assigning below objects to HttpContext.Current.Session.
            ' The order matters in this case since theme_table contains some temp columns constructed from mdb_table.
            ' and some themes/modifiers in the theme_layer are based on the theme_table.
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Catalog(SampleConstants.EWorldAlias), "mdb_table")
            'HttpContext.Current.Session["mdb_table"] = MapInfo.Engine.Session.Current.Catalog[SampleConstants.EWorldAlias];
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Catalog(SampleConstants.ThemeTableAlias), "theme_table")
            'HttpContext.Current.Session["theme_table"] = MapInfo.Engine.Session.Current.Catalog[SampleConstants.ThemeTableAlias];
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers(SampleConstants.ThemeLayerAlias), "theme_layer")
            'HttpContext.Current.Session["theme_layer"] = map.Layers[SampleConstants.ThemeLayerAlias];
            ' Group Layer holds temp object theme or label layers which varies with requests.
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers(SampleConstants.GroupLayerAlias), "group_layer")
            'HttpContext.Current.Session["group_layer"] = map.Layers[SampleConstants.GroupLayerAlias];
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
