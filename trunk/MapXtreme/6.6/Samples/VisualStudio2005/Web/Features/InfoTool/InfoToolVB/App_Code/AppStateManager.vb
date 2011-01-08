'====================================================================
' This file is generated as part of Web project conversion.
' The extra class 'AppStateManager' in the code behind file in 'WebForm1.aspx.vb' is moved to this file.
'====================================================================


Imports System.Web
Imports MapInfo
Imports MapInfo.Engine
Imports MapInfo.WebControls
Imports MapInfo.Mapping
Imports MapInfo.Geometry


Namespace InfoToolWeb
    <Serializable()> _
Public Class AppStateManager
        Inherits StateManager
        Private _session As ManualSerializer = Nothing

        Public Sub New()
            _session = New ManualSerializer
        End Sub

        ' Restore the state
        Public Overrides Sub RestoreState()
            Dim mapAlias As String = ParamsDictionary(ActiveMapAliasKey)
            Dim map As Map = MapInfo.Engine.Session.Current.MapFactory(mapAlias)

            ' If it was user's first time and the session was not dirty then save this default state to be applied later.
            ' If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
            If IsUsersFirstTime() Then
                If IsDirtySession(map) Then
                    RestoreDefaultState(map)
                Else
                    SaveDefaultState(map)
                End If
            Else
                ' If it is not user's first time then restore the last state they saved
                RestoreZoomCenterState(map)
                ' Just by setting it to temp variables the objects are serialized into session. There is no need to set them explicitly.
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("Layers")
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("Selection")
            End If
        End Sub

        ' Save the state
        Public Overrides Sub SaveState()
            Dim mapAlias As String = ParamsDictionary(ActiveMapAliasKey)
            Dim map As Map = MapInfo.Engine.Session.Current.MapFactory(mapAlias)
            If Not map Is Nothing Then
                SaveZoomCenterState(map)
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers, "Layers")
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Selections.DefaultSelection, "Selection")
            End If
        End Sub


        ' This method checks if the mapinfo session got from the pool is dirty or clean
        Private Function IsDirtySession(ByVal map As Map) As Boolean
            ' Check if layers collection in application state if it is already there session is dirty
            Return (Not HttpContext.Current.Application("Layers") Is Nothing)
        End Function

        ' Check if this is user's first time accessing this page. IF there is a zoom value in the asp.net session then it is not user's first time.
        Private Function IsUsersFirstTime() As Boolean
            Return (HttpContext.Current.Session(StateManager.GetKey("Zoom")) Is Nothing)
        End Function

        ' When the session is not dirty these values are initial state of the session.
        Private Sub SaveDefaultState(ByVal map As Map)
            Dim application As HttpApplicationState = HttpContext.Current.Application
            If application("Zoom") Is Nothing Then
                ' Store default selection
                application("Selection") = ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Selections.DefaultSelection)
                ' Store layers collection
                application("Layers") = ManualSerializer.BinaryStreamFromObject(map.Layers)
                ' Store the original zoom and center.
                application("Center") = map.Center
                application("Zoom") = map.Zoom
            End If
        End Sub

        ' When session is dirty but it is first time for user, this will be applied to give users it's initial state
        Private Sub RestoreDefaultState(ByVal map As Map)
            Dim application As HttpApplicationState = HttpContext.Current.Application
            ' Get the default layers, center, and zoomfrom the Application. Clear Layers first, 
            'this resets the zoom and center which we will set later
            map.Layers.Clear()

            'Just by deserializing the binary stream we reset the MapFactory Deault layers collection
            Dim bytes() As Byte = CType(application("Layers"), Byte())
            Dim obj As Object = ManualSerializer.ObjectFromBinaryStream(bytes)

            ' For default selection
            bytes = CType(application("Selection"), Byte())
            obj = ManualSerializer.ObjectFromBinaryStream(bytes)

            ' For zoom and center
            map.Zoom = CType(application("Zoom"), MapInfo.Geometry.Distance)
            map.Center = CType(application("Center"), MapInfo.Geometry.DPoint)
        End Sub
    End Class
End Namespace
