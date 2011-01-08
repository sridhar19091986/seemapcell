Imports System
Imports System.Drawing
Imports System.IO
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl

'/ <summary>
'/ This control depends upon mapControl, does rubberbanding drawing on client side. All it knows is the HTML image tag its doing
'/ drawing on.
'/ </summary>
<ToolboxData("<{0}:ZoomBarTool runat=server></{0}:ZoomBarTool>")> _
 Public Class ZoomBarTool
    Inherits WebTool
    Public Sub New()
        ZoomLevel = 0.0
        InactiveImageUrl = String.Format("{0}/{1}Inactive.gif", Resources.ResourceFolder, MyBase.GetType.Name)
        ActiveImageUrl = String.Format("{0}/{1}Active.gif", Resources.ResourceFolder, MyBase.GetType.Name)
        Active = False
        Command = CommandEnum.ZoomToLevel.ToString()
        ClientInteraction = ClientInteractionEnum.NullInteraction.ToString()
        ClientCommand = ClientCommandEnum.ZoomCommand.ToString()
    End Sub

    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property ZoomLevel() As Double
        Get
            Return CType(ViewState("ZoomLevel"), Double)
        End Get
        Set(ByVal Value As Double)
            ViewState("ZoomLevel") = Value
        End Set
    End Property
    <Browsable(False)> _
    Public Shadows Property Active() As Boolean
        Get
            Return CType(ViewState("Active"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Active") = Value
        End Set
    End Property

    '/ <summary>
    '/ Render this control to the output parameter specified.
    '/ </summary>
    '/ <param name="writer"> The HTML writer to write out to </param>
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)

        If Not Page Is Nothing Then
            Page.VerifyRenderingInServerForm(Me)
        End If

        ' Render HTML
        writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image")
        writer.AddAttribute(HtmlTextWriterAttribute.Src, InactiveImageUrl)
        writer.AddAttribute("OnMouseOver", String.Format("javascript:this.src='{0}';", ActiveImageUrl))
        writer.AddAttribute("OnMouseOut", String.Format("javascript:this.src='{0}';", InactiveImageUrl))
        ToolTip = L10NUtils.Resources.GetString("ZoomBarMapToolHelpText")
        writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip)
        writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip)
        writer.RenderBeginTag(HtmlTextWriterTag.Img)
        writer.RenderEndTag()

        writer.AddAttribute("language", "javascript")
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        writer.RenderBeginTag(HtmlTextWriterTag.Script)
        writer.WriteLine(String.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID))
        writer.WriteLine(String.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, ClientCommand, Command))
        writer.WriteLine(String.Format("var {0}Me = FindElement('{1}_Image');", UniqueID, UniqueID))
        writer.WriteLine(String.Format("{0}Cmd.zoomLevel = {1};", UniqueID, ZoomLevel.ToString()))
        writer.WriteLine(String.Format("{0}Me.onclick = {0}Cmd.Exc;", UniqueID))
        writer.RenderEndTag()
    End Sub
End Class

