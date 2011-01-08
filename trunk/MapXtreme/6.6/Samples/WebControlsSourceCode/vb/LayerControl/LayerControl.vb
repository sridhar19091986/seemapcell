Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Design


<Designer(GetType(LayerControlDesigner)), ToolboxData("<{0}:LayerControl runat=server></{0}:LayerControl>")> _
Public Class LayerControl
    Inherits WebControl
    Public Sub New()
        ResourcesPath = Resources.ResourceFolder
    End Sub
    Private _mapControlID As String

    <TypeConverter(GetType(ServerControlConverter)), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Property MapControlID() As String
        Get
            Return Me._mapControlID
        End Get
        Set(ByVal value As String)
            Me._mapControlID = value
        End Set
    End Property

    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), _
              Editor(GetType(UrlEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    PersistenceMode(PersistenceMode.Attribute)> _
    Public Property ResourcesPath() As String
        Get
            Return CType(ViewState("ResourcesPath"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("ResourcesPath") = Value
        End Set
    End Property

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
        Dim codeName As String = "Interaction.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (Resources.ResourceFolder + "/" + codeName)))
        End If
        codeName = "Command.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript""  type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (Resources.ResourceFolder + "/" + codeName)))
        End If
        codeName = "LayerControl2.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript""  type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (Resources.ResourceFolder + "/" + codeName)))
        End If
    End Sub

    Protected Overrides Sub RenderContents(ByVal output As HtmlTextWriter)
        MyBase.RenderContents(output)
        If (Not Me.Context Is Nothing) Then
            Dim initCode As String = String.Format("var {0}LayerInfo = new LayerInfo('{1}', '{2}', '{3}', '{4}');", UniqueID, UniqueID, MapControlID, ResourcesPath, "treeview2.xslt")
            Dim activateScript As String = String.Format("<script type='text/javascript'>AppendLayerScriptToForm(""{0}"");</script>", initCode)
            Page.RegisterStartupScript(UniqueID, activateScript)
        End If
    End Sub
End Class
