Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.IO
Imports System.Web.UI.Design
Imports System.Web.UI
Imports System.Xml

Public Class ListConverter
    Inherits System.ComponentModel.StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        If (context Is Nothing) OrElse (context.Container Is Nothing) Then
            Return Nothing
        End If
        Dim serverControls() As Object = Me.GetList(context.Container)
        If Not serverControls Is Nothing Then
            Return New StandardValuesCollection(serverControls)
        End If
        Return Nothing
    End Function

    Protected Overridable Function GetList(ByVal container As IContainer) As Object()
        Return Nothing
    End Function
End Class

Public Class ServerControlConverter
    Inherits ListConverter

    Protected Overrides Function GetList(ByVal container As IContainer) As Object()
        Dim availableControls As New ArrayList
        Dim component1 As IComponent
        For Each component1 In container.Components
            Dim control1 As Control = CType(component1, Control)
            If ((((Not control1 Is Nothing) AndAlso Not TypeOf control1 Is Page) AndAlso ((Not control1.ID Is Nothing) AndAlso (control1.ID.Length <> 0))) AndAlso Me.IncludeControl(control1)) Then
                availableControls.Add(control1.ID)
            End If
        Next
        availableControls.Sort(Comparer.Default)
        Return availableControls.ToArray
    End Function

    Protected Overridable Function IncludeControl(ByVal serverControl As Control) As Boolean
        If TypeOf serverControl Is MapControl Then
            Return True
        End If
        Return False
    End Function
End Class

Public Class CommandListConverter
    Inherits ListConverter

    Protected Overrides Function GetList(ByVal container As IContainer) As Object()
        Dim availableCommands As New ArrayList
        Dim o As Object
        For Each o In WebTool.CommandEnum.GetValues(GetType(WebTool.CommandEnum))
            availableCommands.Add(o.ToString())
        Next
        Return availableCommands.ToArray
    End Function
End Class

Public Class InteractionListConverter
    Inherits ListConverter
    Protected Overrides Function GetList(ByVal container As IContainer) As Object()
        Dim availableCommands As New ArrayList
        Dim o As Object
        For Each o In WebTool.ClientInteractionEnum.GetValues(GetType(WebTool.ClientInteractionEnum))
            availableCommands.Add(o.ToString())
        Next
        Return availableCommands.ToArray
    End Function
End Class

Public Class ClientCommandListConverter
    Inherits ListConverter

    Protected Overrides Function GetList(ByVal container As IContainer) As Object()
        Dim availableCommands As New ArrayList
        Dim o As Object
        For Each o In WebTool.ClientCommandEnum.GetValues(GetType(WebTool.ClientCommandEnum))
            availableCommands.Add(o.ToString())
        Next
        Return availableCommands.ToArray
    End Function
End Class

Friend Class LayerControlDesigner
    Inherits ControlDesigner
    Private _layerControl As LayerControl

    '/	<summary>Updates the web.config file with information about MapXTreme assemblies and handlers.</summary>
    '/	<remarks>None.</remarks>
    Public Shared Sub UpdateForLayerControl(ByVal documentURL As String)
        ' Open the web.config file as xmldoc
        Dim webConfigPath As String
        Dim xmlDoc As XmlDocument = WebDesignerUtility.GetWebConfig(documentURL, webConfigPath)

        ' Add httphandler for mapcontrol
        Dim updated As Boolean = False
        updated = updated Or WebDesignerUtility.AddController(xmlDoc, "LayerController.ashx", "MapInfo.WebControls.LayerController")
        updated = updated Or WebDesignerUtility.AddAppSettings(xmlDoc)
        updated = updated Or WebDesignerUtility.AddHttpModules(xmlDoc)

        If updated Then xmlDoc.Save(webConfigPath)
    End Sub

    Public Overrides Sub Initialize(ByVal component As IComponent)
        _layerControl = CType(component, LayerControl)
        Dim host As IDesignerHost = CType(component.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim docService As IWebFormsDocumentService = CType(component.Site.GetService(GetType(IWebFormsDocumentService)), IWebFormsDocumentService)
        Dim documentUrl As String = docService.DocumentUrl
        UpdateForLayerControl(documentUrl)
        MyBase.Initialize(component)
    End Sub

    Public Overrides Function GetDesignTimeHtml() As String
        Dim text As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(text)
        writer.AddAttribute("style", String.Format("border-top:solid 1px black; border-left:solid 1px black; border-bottom:solid 1px black; border-right:solid 1px black; width: {0}px; height: {1}px;", _layerControl.Width.Value, _layerControl.Height.Value))
        writer.RenderBeginTag(HtmlTextWriterTag.Div)
        writer.WriteLine(L10NUtils.Resources.GetString("LCDesignString"))
        writer.RenderEndTag()
        Return text.ToString()
    End Function
End Class
