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

''/// <summary>
'/// Class for Navigation Control 
'/// </summary>
'/// <remarks>These set of controls allow users to pan the map to various directions. The pan can be either done by map units or by percentage
'/// of the size of the map on screen. It also uses north and east offsets.
'/// </remarks>
Public Class NavigationTool
    Inherits WebTool
    '/// <summary>
    '/// Offset method is either by units or by percentage of the screen to pan
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Enum OffsetMethodEnum
        '/// <summary>
        '/// Pan by unit.
        '/// </summary>
        '/// <remarks>None</remarks>
        ByUnit = 0
        '		/// <summary>
        '		/// Pan by percentage.
        '		/// </summary>
        '		/// <remarks>None</remarks>
        ByPercentage = 1
    End Enum
    '		/// <summary>
    '		/// Constructor for the Navigation tool.
    '		/// </summary>
    '		/// <remarks>None</remarks>
    Public Sub New()
        InactiveImageUrl = String.Format("{0}/{1}Inactive.gif", Resources.ResourceFolder, MyBase.GetType.Name)
        ActiveImageUrl = String.Format("{0}/{1}Active.gif", Resources.ResourceFolder, MyBase.GetType.Name)
        OffsetMethod = OffsetMethodEnum.ByPercentage
        Active = False
        Command = CommandEnum.Navigate.ToString()
        ClientInteraction = ClientInteractionEnum.NullInteraction.ToString()
        ClientCommand = ClientCommandEnum.NavigateCommand.ToString()
    End Sub

    '/ <summary>
    '/ This property is not used for this control
    '/ </summary>
    '/ <remarks>None</remarks>
    <Browsable(False)> _
    Public Shadows Property Active() As Boolean
        Get
            Return CType(ViewState("Active"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Active") = Value
        End Set
    End Property

    '		/// <summary>
    '		/// Offset to be used to pan to East
    '		/// </summary>
    '		/// <remarks>None</remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
  Public Property EastOffset() As Double
        Get
            Return CType(ViewState("EastOffset"), Double)
        End Get
        Set(ByVal Value As Double)
            ViewState("EastOffset") = Value
        End Set
    End Property

    '		/// <summary>
    '		/// Offset to be used to pan to North
    '		/// </summary>
    '		/// <remarks>None</remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property NorthOffset() As Double
        Get
            Return CType(ViewState("NorthOffset"), Double)
        End Get
        Set(ByVal Value As Double)
            ViewState("NorthOffset") = Value
        End Set
    End Property

    '		/// <summary>
    '		/// Percentage of the size of the map to do pan.
    '		/// </summary>
    '		/// <remarks>When 10.0 is used the map is panned by 10%</remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
  Public Property Percent() As Double
        Get
            Return CType(ViewState("Percent"), Double)
        End Get
        Set(ByVal Value As Double)
            ViewState("Percent") = Value
        End Set
    End Property

    '  		/// <summary>
    '		/// Method to do pan. Either by percentage or by map units
    '		/// </summary>
    '		/// <remarks>None</remarks>
    '
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property OffsetMethod() As OffsetMethodEnum
        Get
            Return CType(ViewState("OffsetMethod"), OffsetMethodEnum)
        End Get
        Set(ByVal Value As OffsetMethodEnum)
            ViewState("OffsetMethod") = Value
        End Set
    End Property

    '/ <summary>
    '/ Render this control to the output parameter specified.
    '/ </summary>
    '/ <param name="writer"> The HTML writer to write out to </param>
    '		/// <remarks>Writes the events for onmouseover and onmouseout in javascript to the page.</remarks>
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        If Not Page Is Nothing Then
            Page.VerifyRenderingInServerForm(Me)
        End If
        ' Render HTML
        writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "_Image")
        writer.AddAttribute(HtmlTextWriterAttribute.Src, InactiveImageUrl)
        writer.AddAttribute("OnMouseOver", String.Format("javascript:this.src='{0}';", ActiveImageUrl))
        writer.AddAttribute("OnMouseOut", String.Format("javascript:this.src='{0}';", InactiveImageUrl))
        ToolTip = L10NUtils.Resources.GetString("NavigationMapToolHelpText")
        writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip)
        writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip)
        writer.RenderBeginTag(HtmlTextWriterTag.Img)
        writer.RenderEndTag()

        RenderJS(writer)

    End Sub
    '/// <summary>
    '/// Render the javascript to create command and event handlers
    '/// </summary>
    '/// <param name="writer"></param>
    '/// <remarks>This control follows the same architecture as the tools.</remarks>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        writer.AddAttribute("language", "javascript")
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        writer.RenderBeginTag(HtmlTextWriterTag.Script)
        writer.WriteLine(String.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID))
        writer.WriteLine(String.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, ClientCommand, Command))
        writer.WriteLine(String.Format("var {0}Me = FindElement('{1}_Image');", UniqueID, UniqueID))
        writer.WriteLine(String.Format("{0}Cmd.method = '{1}';", UniqueID, OffsetMethod.ToString()))
        writer.WriteLine(String.Format("{0}Me.onclick = {0}Cmd.Exc;", UniqueID))
        writer.RenderEndTag()
    End Sub
    '/// <summary>
    '/// Render javascript to set parameters such as offsets
    '/// </summary>
    '/// <param name="writer">HtmlTextWriter</param>
    '/// <param name="percentEast">Percent of screen to be moved to East</param>
    '/// <param name="percentNorth">Percent of screen to be moved to North</param>
    '/// <param name="eastOffset">Offset in map's units to be moved to East</param>
    '/// <param name="northOffset">Offset in map's units to be moved to North</param>
    '/// <remarks>All the offsets are set and then depending upon the method chosen appropriate offsets are used.</remarks>

    Protected Overridable Sub RenderOffsetJS(ByVal writer As HtmlTextWriter, ByVal percentEast As Double, ByVal percentNorth As Double, ByVal eastOffset As Double, ByVal northOffset As Double)
        writer.AddAttribute("language", "javascript")
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        writer.RenderBeginTag(HtmlTextWriterTag.Script)
        If OffsetMethod = OffsetMethodEnum.ByPercentage Then
            writer.WriteLine(String.Format("{0}Cmd.east = {1};", UniqueID, percentEast))
            writer.WriteLine(String.Format("{0}Cmd.north = {1};", UniqueID, percentNorth))
        ElseIf OffsetMethod = OffsetMethodEnum.ByUnit Then
            writer.WriteLine(String.Format("{0}Cmd.east = {1};", UniqueID, eastOffset))
            writer.WriteLine(String.Format("{0}Cmd.north = {1};", UniqueID, northOffset))
        End If
        writer.RenderEndTag()
    End Sub
End Class
'/// <summary>
'/// This tool allows users to navigate to East.
'/// </summary>
'/// <remarks>When navigating to East, the map is panned to West so that more East is visible.</remarks>

<ToolboxData("<{0}:EastNavigationTool runat=server></{0}:EastNavigationTool>")> _
 Public Class EastNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for East Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10.0
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to East navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer"></param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, -Percent, 0.0, -EastOffset, 0.0)
    End Sub
End Class

'/// <summary>
'/// This tool allows users to navigate to West.
'/// </summary>
'/// <remarks>When navigating to West, the map is panned to East so that more West is visible.</remarks>

<ToolboxData("<{0}:WestNavigationTool runat=server></{0}:WestNavigationTool>")> _
 Public Class WestNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for West Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10.0
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to West navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer"></param>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, Percent, 0.0, EastOffset, 0.0)
    End Sub
End Class
'/// <summary>
'/// This tool allows users to navigate to North
'/// </summary>
'/// <remarks>When navigating to North, the map is panned to South so that more North is visible.</remarks>

<ToolboxData("<{0}:NorthNavigationTool runat=server></{0}:NorthNavigationTool>")> _
 Public Class NorthNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for North Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10.0
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to North navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer"></param>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, 0.0, Percent, 0.0, NorthOffset)
    End Sub
End Class
'/// <summary>
'/// This tool allows users to navigate to South
'/// </summary>
'/// <remarks>When navigating to South, the map is panned to North so that more South is visible.</remarks>

<ToolboxData("<{0}:SouthNavigationTool runat=server></{0}:SouthNavigationTool>")> _
 Public Class SouthNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for South Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10.0
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to South navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer"></param>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, 0.0, -Percent, 0.0, -NorthOffset)
    End Sub
End Class
'/// <summary>
'/// This tool allows users to navigate to SouthEast.
'/// </summary>
'/// <remarks>When navigating to SouthEast, the map is panned to NorthWest so that more SouthEast is visible.</remarks>

<ToolboxData("<{0}:SouthEastNavigationTool runat=server></{0}:SouthEastNavigationTool>")> _
 Public Class SouthEastNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for SouthEast Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to SouthEast navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer">HtmlTextWriter</param>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, -Percent, -Percent, -EastOffset, -NorthOffset)
    End Sub
End Class

'/// <summary>
'/// This tool allows users to navigate to SouthWest.
'/// </summary>
'/// <remarks>When navigating to SouthWest, the map is panned to NorthEast so that more SouthWest is visible.</remarks>
<ToolboxData("<{0}:SouthWestNavigationTool runat=server></{0}:SouthWestNavigationTool>")> _
 Public Class SouthWestNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for SouthWest Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '/// <summary>
    '/// Render javascript specific to SouthWest navigation tool.
    '/// </summary>
    '/// <remarks>None</remarks>
    '/// <param name="writer">HtmlTextWriter</param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, Percent, -Percent, EastOffset, -NorthOffset)
    End Sub
End Class
'/// <summary>
'/// This tool allows users to navigate to NorthEast.
'/// </summary>
'/// <remarks>When navigating to NorthEast, the map is panned to SouthWest so that more NorthEast is visible.</remarks>
<ToolboxData("<{0}:NorthEastNavigationTool runat=server></{0}:NorthEastNavigationTool>")> _
 Public Class NorthEastNavigationTool
    Inherits NavigationTool
    '/// <summary>
    '/// Constuctor for NorthEast Navigation tool
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '		'/// <summary>
    '		'/// Render javascript specific to NorthEast navigation tool.
    '		'/// </summary>
    '		'/// <remarks>None</remarks>
    '		'/// <param name="writer">HtmlTextWriter</param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, -Percent, Percent, -EastOffset, NorthOffset)
    End Sub
End Class
'	'/// <summary>
'	'/// This tool allows users to navigate to NorthWest
'	'/// </summary>
'	'/// <remarks>When navigating to NorthWest, the map is panned to SouthEast so that more NorthWest is visible.</remarks>

<ToolboxData("<{0}:NorthWestNavigationTool runat=server></{0}:NorthWestNavigationTool>")> _
 Public Class NorthWestNavigationTool
    Inherits NavigationTool
    '		'/// <summary>
    '		'/// Constuctor for NorthWest Navigation tool
    '		'/// </summary>
    '		'/// <remarks>None</remarks>
    Public Sub New()
        Percent = 10
        EastOffset = 0.0
        NorthOffset = 0.0
    End Sub
    '		'/// <summary>
    '		'/// Render javascript specific to NorthWest navigation tool.
    '		'/// </summary>
    '		'/// <remarks>None</remarks>
    '		'/// <param name="writer">HtmlTextWriter</param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        MyBase.RenderJS(writer)
        MyBase.RenderOffsetJS(writer, Percent, Percent, EastOffset, NorthOffset)
    End Sub
End Class
