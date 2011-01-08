Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
'/	<summary>Provides export formats for the web environment.</summary>
'/	<remarks>This enumeration was created to support only those export formats in
'/ the MapExportFormat that work in the web environment.</remarks>
Public Enum WebExportFormat
    '/	<summary>Exports Windows Bitmap formats.</summary>
    Bmp
    '/	<summary>Exports Graphics Interchange Format (GIF) formats.</summary>
    Gif
    '/	<summary>Exports Joint Photographic Experts Group (JPEG) formats.</summary>
    Jpeg
End Enum

<Designer(GetType(MapControlDesigner)), ToolboxData("<{0}:MapControl runat=server></{0}:MapControl>")> _
Public Class MapControl
    Inherits WebControl
    <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Overrides Property Width() As Unit
        Get
            If MyBase.Width.IsEmpty Then
                MyBase.Width = New Unit(300)
            End If
            Return MyBase.Width
        End Get
        Set(ByVal value As Unit)
            MyBase.Width = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Overrides Property Height() As Unit
        Get
            If MyBase.Height.IsEmpty Then
                MyBase.Height = New Unit(300)
            End If
            Return MyBase.Height
        End Get
        Set(ByVal value As Unit)
            MyBase.Height = value
        End Set
    End Property
    Private _mapAlias As String
    <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Property MapAlias() As String
        Get
            Return Me._mapAlias
        End Get
        Set(ByVal value As String)
            Me._mapAlias = value
        End Set
    End Property
    Private _imgUseMap As String
    '/ <summary>
    '/ UseMap attribute for the IMG tag which is rendered with this mapcontrol
    '/ </summary>
    '/ <remarks>Sometimes users want to usemaps with the map image. The value of this property will be added as extra UseMap attribute to 
    '/ the IMG tag rendered with this mapcontrol.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property IMGUseMap() As String
        Get
            Return _imgUseMap
        End Get
        Set(ByVal Value As String)
            _imgUseMap = Value
        End Set
    End Property

    '/	<summary>Gets or sets the format used to export a map for rendering.</summary>
    '/	<value>Gets or sets the format used for exporting.</value>
    '/	<remarks>This property is used internally to export a map to be used for
    '/ rendering.</remarks>
    <Browsable(True), DefaultValue("GIF"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property ExportFormat() As WebExportFormat
        Get
            Dim format As WebExportFormat
            If ViewState("ExportFormat") Is Nothing Then
                format = WebExportFormat.Gif
            Else
                format = CType(ViewState("ExportFormat"), WebExportFormat)
            End If
            Return format
        End Get
        Set(ByVal Value As WebExportFormat)
            ViewState("ExportFormat") = value
        End Set
    End Property

    '/ <summary> 
    '/ Render the control by setting the source of the image to a url containing all information to get the right map
    '/ </summary>
    '/ <remarks>It renders the control html by writing html IMG tag and it's source is set to the url. Using javascript it also sets the mapalias property for the
    '/ IMG tag, so that other's can get to it after the image has been loaded, using OnLoad event of the IMG tag.
    '/ </remarks>
    '/ <param name="output"> The HTML writer object to write html to </param>
    Protected Overrides Sub RenderContents(ByVal output As HtmlTextWriter)
        ' Get the model out of ASP.NET and get the stream
        Dim model As MapControlModel = MapControlModel.GetModelFromSession()
        If model Is Nothing Then
            model = MapControlModel.SetDefaultModelInSession()
        End If
        Dim ms As System.IO.MemoryStream = Nothing
        Try
            ms = model.GetMap(MapAlias, CType(Width.Value, Integer), CType(Height.Value, Integer), ExportFormat.ToString())
        Catch ex As Exception
            output.WriteLine(ex.Message)
            output.WriteLine("<br>")
            output.WriteLine(L10NUtils.Resources.GetString("MapNotFoundErrorString"))
            HttpContext.Current.Server.ClearError()
            Return
        End Try

        ' Insert the image stream to Cache with key imageid and timeout in 2 mintues.
        Dim imageid As String = ImageHelper.GetUniqueID()
        ImageHelper.SetImageToCache(imageid, ms, 2)
        Dim url As String = ImageHelper.GetImageURL(imageid, ExportFormat.ToString())

        '//Write the IMG tag and set the mapalias.
        output.AddAttribute(HtmlTextWriterAttribute.Width, Me.Width.ToString)
        output.AddAttribute(HtmlTextWriterAttribute.Height, Me.Height.ToString)
        If (Not IMGUseMap = Nothing) Then
            If (IMGUseMap.Length > 0) Then
                output.AddAttribute("USEMAP", IMGUseMap)
            End If
        End If
        If (Not CssClass = Nothing) Then
            output.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
        End If

        output.AddAttribute(HtmlTextWriterAttribute.Alt, "")
        output.AddAttribute(HtmlTextWriterAttribute.Src, url)
        output.AddAttribute(HtmlTextWriterAttribute.Id, (Me.UniqueID & "_Image"))
        output.RenderBeginTag(HtmlTextWriterTag.Img)
        output.RenderEndTag()
        output.Flush()

        'Now set the alias and format manually by getting the image element.
        output.AddAttribute("language", "javascript")
        output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        output.RenderBeginTag(HtmlTextWriterTag.Script)
        output.WriteLine(String.Format("var {0}Me = document.getElementById('{0}_Image');", UniqueID))
        output.WriteLine(String.Format("{0}Me.mapAlias= '{1}';", UniqueID, MapAlias))
        output.WriteLine(String.Format("{0}Me.exportFormat= '{1}';", UniqueID, ExportFormat))
        output.RenderEndTag()

    End Sub
End Class
