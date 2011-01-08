Imports System
Imports System.Web
Imports System.Configuration
Imports MapInfo.Engine
Imports MapInfo.Geometry
Imports MapInfo.Mapping

'/ <summary>
'/ Class managing state.
'/ </summary>
'/ <remarks>This class contains method to save and restore if necessary various objects. The default set of webcontrols provided with MapXTreme
'/ save and restore whatever they need to using these methods.
'/ </remarks>
<Serializable()> _
Public MustInherit Class StateManager
    '/ <summary>
    '/ Name used in key to put StateManager in ASP.NET session
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared ReadOnly StateManagerKey As String = "StateManager"
    '/ <summary>
    '/ Name used in key to put zoom value in ASP.NET session
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared ReadOnly ZoomKey As String = "Zoom"
    '/ <summary>
    '/ Name used in key to put center value in ASP.NET session
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared ReadOnly CenterKey As String = "Center"
    '/ <summary>
    '/ Name used in key to put selections in ASP.NET session
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared ReadOnly SelectionsKey As String = "Selections"
    '/ <summary>
    '/ Name used in key to put layers in ASP.NET session
    '/ </summary>
    '/ <remarks>None</remarks>
    Public Shared ReadOnly LayersKey As String = "Layers"

    '/// <summary>
    '/// Key to get error string from resources
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Shared ReadOnly StateManagerResErr1 As String = "StateManagerInstanceError"

    ' IDictionary to hold parameters used by this state manager.
    Private parametersDictionary As New System.Collections.SortedList


    '/ <summary>
    '/ Put user instance of user implemented state manager in ASP.NET session
    '/ </summary>
    '/ <param name="sm">Instance of class derived from StateManager</param>
    Public Shared Sub PutStateManagerInSession(ByVal sm As StateManager)
        HttpContext.Current.Session(GetKey(StateManager.StateManagerKey)) = sm
    End Sub

    '/ <summary>
    '/ Get instance of user implemented state manager from ASP.NET session
    '/ </summary>
    Public Shared Function GetStateManagerFromSession() As StateManager
        Dim sm As StateManager = Nothing
        If Not HttpContext.Current.Session(GetKey(StateManager.StateManagerKey)) Is Nothing Then
            sm = CType(HttpContext.Current.Session(GetKey(StateManager.StateManagerKey)), StateManager)
        End If
        Return sm
    End Function

    Public Shared Function GetKey(ByVal name As String) As String
        Return String.Format("{0}_{1}", HttpContext.Current.Session.SessionID, name)
    End Function

    '/ <summary>
    '/ Method returns true if the state management in the web.config file is Manual
    '/ </summary>
    '/ <returns>True if the state management in the web.config file is Manual</returns>
    '/ <remarks>None</remarks>
    Public Shared Function IsManualState() As Boolean
        Dim state As String = ConfigurationSettings.AppSettings("MapInfo.Engine.Session.State")
        If Not state Is Nothing Then
            If String.Compare(state, "Manual", True) = 0 Then
                Return True
            End If
        End If
        Return False
    End Function

    '/ <summary>
    '/ Save state of the map in ASP.NET Session
    '/ </summary>
    '/ <param name="map">Map object</param>
    '/ <remarks>None</remarks>
    Public Shared Sub SaveMapState(ByVal map As Map)
        If IsManualState() Then
            HttpContext.Current.Session(GetKey("Map")) = map
        End If
    End Sub

    '/ <summary>
    '/ Restore state of the zoom and center from ASP.NET session
    '/ </summary>
    '/ <param name="map">Map object</param>
    '/ <remarks>None</remarks>
    Public Shared Sub RestoreZoomCenterState(ByVal map As Map)
		If IsManualState() Then
			Dim manualSerializer As New ManualSerializer()
			If Not manualSerializer(GetKey("Zoom")) Is Nothing Then
				map.Zoom = CType(manualSerializer(GetKey("Zoom")), MapInfo.GeomeTry.Distance)
			End If
			If Not manualSerializer(GetKey("Center")) Is Nothing Then
				map.Center = CType(manualSerializer(GetKey("Center")), DPoint)
			End If
		End If
    End Sub

    '/ <summary>
    '/ Save state of the zoom and center in ASP.NET session
    '/ </summary>
    '/ <param name="map">Map object</param>
    '/ <remarks>None</remarks>
    Public Shared Sub SaveZoomCenterState(ByVal map As Map)
		If IsManualState() Then
			Dim manualSerializer As New ManualSerializer()
			manualSerializer(GetKey("Zoom")) = map.Zoom
			manualSerializer(GetKey("Center")) = map.Center
		End If
    End Sub

    '/ <summary>
    '/ Save state of the selection in ASP.NET session
    '/ </summary>
    '/ <param name="selection">Selection object</param>
    '/ <remarks>Restore method is not required because ASP.NET restores ISerializable objects automatically.</remarks>
    Public Shared Sub SaveSelectionState(ByVal selection As Selection)
        If IsManualState() Then
            HttpContext.Current.Session(GetKey("Selection")) = selection
        End If
    End Sub

    '/ <summary>
    '/ Save state of the layers  in ASP.NET session
    '/ </summary>
    '/ <param name="layers">Layers collection</param>
    '/ <remarks>Restore method is not required because ASP.NET restores ISerializable objects automatically.</remarks>
    Public Shared Sub SaveLayersState(ByVal layers As Layers)
        If IsManualState() Then
            HttpContext.Current.Session(GetKey("Layers")) = layers
        End If
    End Sub

    '/ <summary>
    '/ This method must be implemented by users to restore the state
    '/ </summary>
    '/ <remarks>The instance of this class is fetched from ASP.NET session and restore and save methods are called from tools.
    '/ The implementation is application specific. These methods can contain code to do any other processing.</remarks>
    Public MustOverride Sub RestoreState()

    '/ <summary>
    '/ This method must be implemented by users to save the state
    '/ </summary>
    '/ <remarks>The instance of this class is fetched from ASP.NET session and restore and save methods are called from tools.
    '/ The implementation is application specific. These methods can contain code to do any other processing.</remarks>
    Public MustOverride Sub SaveState()

    '/ <summary>
    '/ Return a IDictionary which could hold key/value pairs used across requests.
    '/ </summary>
    '/ <remarks>This property only supports get. The value which is going to be added must be serializable.
    '/ <para>You must add <see cref="P:ActiveMapAliasKey"/> with a map alias as the value into this IDictionary in the Page_Load()
    '/  if MapInfo.Engine.Session.State is set to "Manual" in the web.config.</para>
    '/ <para>This IDictionary passes along parameters among web page, state manager and web tools. 
    '/ key/pair values will be saved and restored by asp.net because the state manager will be put into HttpContext.Current.Session.</para>
    '/ </remarks>
    Public ReadOnly Property ParamsDictionary() As IDictionary
        Get
            Return parametersDictionary
        End Get
    End Property

    '/ <summary>
    '/ A key will be used to store Active map alias. 
    '/ </summary>
    '/ <remarks>You must add this key into <see cref="P:ParamsDictionary"/> with a map alias value if you want to use this state manager.</remarks>
    Public Shared ReadOnly Property ActiveMapAliasKey() As String
        Get
            Return StateManager.GetKey("ActiveMapAlias")
        End Get
    End Property


End Class
'----------------------------------------------------------------
