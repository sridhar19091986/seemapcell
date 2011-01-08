Imports System
Imports System.Web
Imports MapInfo.WebControls
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Mapping.Legends



Public Class WebForm1
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents MapControl1 As MapInfo.WebControls.MapControl
    Protected WithEvents ZoomInTool1 As MapInfo.WebControls.ZoomInTool
    Protected WithEvents ZoomOutTool1 As MapInfo.WebControls.ZoomOutTool
    Protected WithEvents SouthNavigationTool2 As MapInfo.WebControls.SouthNavigationTool
    Protected WithEvents NorthNavigationTool2 As MapInfo.WebControls.NorthNavigationTool
    Protected WithEvents EastNavigationTool2 As MapInfo.WebControls.EastNavigationTool
    Protected WithEvents WestNavigationTool2 As MapInfo.WebControls.WestNavigationTool
    Protected WithEvents NorthEastNavigationTool1 As MapInfo.WebControls.NorthEastNavigationTool
    Protected WithEvents SouthWestNavigationTool1 As MapInfo.WebControls.SouthWestNavigationTool
    Protected WithEvents SouthEastNavigationTool1 As MapInfo.WebControls.SouthEastNavigationTool
    Protected WithEvents NorthWestNavigationTool1 As MapInfo.WebControls.NorthWestNavigationTool
    Protected WithEvents ZoomBarTool1 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents ZoomBarTool2 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents ZoomBarTool3 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents ZoomBarTool4 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents ZoomBarTool5 As MapInfo.WebControls.ZoomBarTool
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents Image2 As System.Web.UI.WebControls.Image
    Protected WithEvents CenterTool1 As MapInfo.WebControls.CenterTool
    Protected WithEvents PanTool1 As MapInfo.WebControls.PanTool
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox

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
        'If it is new session then set the initial state of the map
        If Me.Session.IsNewSession Then
            'get default mapcontrol model from session
            Dim controlModel As MapInfo.WebControls.MapControlModel = MapControlModel.SetDefaultModelInSession()

            'add custom commands to map control model
            controlModel.Commands.Add(New CreateTheme)
            controlModel.Commands.Add(New RemoveTheme)
            controlModel.Commands.Add(New GetLegend)

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


