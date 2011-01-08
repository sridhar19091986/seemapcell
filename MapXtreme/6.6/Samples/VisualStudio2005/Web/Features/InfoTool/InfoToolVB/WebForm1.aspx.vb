Imports System.Web
Imports MapInfo
Imports MapInfo.Engine
Imports MapInfo.WebControls
Imports MapInfo.Mapping
Imports MapInfo.Geometry


Namespace InfoToolWeb



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
        'If it is new session then set the initial state of the map
        If Me.Session.IsNewSession Then
            'get default mapcontrol model from session
            Dim controlModel As MapInfo.WebControls.MapControlModel = MapControlModel.SetDefaultModelInSession()

            'add custom commands to map control model
            controlModel.Commands.Add(New Info)
            controlModel.Commands.Add(New ZoomValue)

            'instanciate AppStateManager class
            Dim myStateManager As New AppStateManager

            'put current map alias to state manager dictionary
            myStateManager.ParamsDictionary(StateManager.ActiveMapAliasKey) = Me.MapControl1.MapAlias

            'put state manager to session
            StateManager.PutStateManagerInSession(myStateManager)
        End If
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

End Namespace
