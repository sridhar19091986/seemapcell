Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.IO
Imports System.Web.UI.Design
Imports System.Web.UI
Imports System.Runtime.InteropServices
Imports EnvDTE
Imports System.Xml
Imports System.Reflection

Friend Class MapControlDesigner
    Inherits ControlDesigner
    Private _mapControl As MapControl

    'Updates the web.config file with information about MapXTreme assemblies and handlers.
    Public Shared Sub UpdateForMapControl(ByVal documentURL As String)
        ' Open the web.config file as xmldoc
        Dim webConfigPath As String
        Dim xmlDoc As XmlDocument = WebDesignerUtility.GetWebConfig(documentURL, webConfigPath)

        ' Add httphandler for mapcontrol
        Dim updated As Boolean = False
        updated = updated Or WebDesignerUtility.AddController(xmlDoc, "MapController.ashx", "MapInfo.WebControls.MapController")
        updated = updated Or WebDesignerUtility.AddAppSettings(xmlDoc)
        updated = updated Or WebDesignerUtility.AddHttpModules(xmlDoc)
        updated = updated Or WebDesignerUtility.AddAssemblies(xmlDoc)
        updated = updated Or WebDesignerUtility.AddSessionState(xmlDoc)

        If updated Then xmlDoc.Save(webConfigPath)
    End Sub

    Public Overrides Sub Initialize(ByVal component As IComponent)
        _mapControl = CType(component, MapControl)
        Dim host As IDesignerHost = CType(component.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim docService As IWebFormsDocumentService = CType(component.Site.GetService(GetType(IWebFormsDocumentService)), IWebFormsDocumentService)
        Dim documentUrl As String = docService.DocumentUrl
        UpdateForMapControl(documentUrl)
        MyBase.Initialize(component)
    End Sub

    Protected Overrides Sub OnBehaviorAttached()
        MyBase.OnBehaviorAttached()
        Behavior.SetAttribute("Width", _mapControl.Width.Value, False)
        Behavior.SetAttribute("Height", _mapControl.Height.Value, False)
    End Sub

    Public Overrides Function GetDesignTimeHtml() As String
        Dim text As StringWriter = New StringWriter
        Dim writer As HtmlTextWriter = New HtmlTextWriter(text)
        writer.AddAttribute("style", String.Format("border-top:solid 1px black; border-left:solid 1px black; border-bottom:solid 1px black; border-right:solid 1px black; width: {0}px; height: {1}px;", _mapControl.Width.Value, _mapControl.Height.Value))
        writer.RenderBeginTag(HtmlTextWriterTag.Div)
        writer.WriteLine(L10NUtils.Resources.GetString("MCDesignString"))
        writer.RenderEndTag()
        Return text.ToString()
    End Function
End Class




