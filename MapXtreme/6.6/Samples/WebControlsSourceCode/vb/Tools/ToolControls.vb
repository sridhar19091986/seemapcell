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
<ToolboxData("<{0}:ZoomInTool runat=server></{0}:ZoomInTool>")> _
Public Class ZoomInTool
    Inherits WebTool
    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.ZoomIn.ToString()
        ClientInteraction = ClientInteractionEnum.RectInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("ZoomInMapToolHelpText")
    End Sub
End Class

'/// <summary>
'/// Class for ZoomOutTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>
<ToolboxData("<{0}:ZoomOutTool runat=server></{0}:ZoomOutTool>")> _
Public Class ZoomOutTool
    Inherits WebTool

    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.ZoomOut.ToString()
        ClientInteraction = ClientInteractionEnum.RectInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("ZoomOutMapToolHelpText")
    End Sub
End Class

'/// <summary>
'/// Class for PanTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>
<ToolboxData("<{0}:PanTool runat=server></{0}:PanTool>")> _
    Public Class PanTool
    Inherits WebTool

    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.Pan.ToString()
        ClientInteraction = ClientInteractionEnum.DragInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("PanMapToolHelpText")
        ClientCommand = ClientCommandEnum.PanCommand.ToString()
    End Sub
    '/// <summary>
    '/// The Pan tool uses different command on the client side, therefore this method is overridden
    '/// </summary>
    '/// <param name="writer">HtmlTextWriter</param>
    '/// <remarks>None</remarks>

    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        ' Since this is runtime behavior do it only if there is context
        RenderJS(writer, ClientCommand, "ImgTool")
    End Sub
End Class
'/// <summary>
'/// Class for DistanceTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>

<ToolboxData("<{0}:DistanceTool runat=server></{0}:DistanceTool>")> _
    Public Class DistanceTool
    Inherits WebTool
    '/	<summary>Provides a method (Cartesian or Spherical) to calculate the distance
    '/ between end points of lines.</summary>
    '/	<value>Gets or sets the method to be used to compute distances.</value>
    '/	<remarks>DistanceType is the method used (Cartesian or Spherical) to calculate
    '/ the distance between end points of lines.</remarks>
		<Browsable(True),Category("Mapping"),DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),PersistenceMode(PersistenceMode.Attribute)> _  
		Public Property DistanceType() As MapInfo.GeomeTry.DistanceType
			Get 
				If ViewState("DistanceType") Is Nothing Then
					 Return MapInfo.GeomeTry.DistanceType.Spherical
				End If
				Return CType(ViewState("DistanceType"), MapInfo.GeomeTry.DistanceType)
			End Get
			Set (ByVal Value As MapInfo.GeomeTry.DistanceType) 
				ViewState("DistanceType") = value
			End Set
		End Property

    '/	<summary>Gets or sets the distance unit to compute distances.</summary>
    '/	<value>Gets or sets the distance unit to compute distances.</value>
    '/	<remarks>None.</remarks>
    <Browsable(True), Category("Mapping"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
  Public Property DistanceUnit() As MapInfo.GeomeTry.DistanceUnit
        Get
            If ViewState("DistanceUnit") Is Nothing Then
                Return MapInfo.GeomeTry.DistanceUnit.Mile
            End If
            Return CType(ViewState("DistanceUnit"), MapInfo.GeomeTry.DistanceUnit)
        End Get
        Set(ByVal Value As MapInfo.GeomeTry.DistanceUnit)
            ViewState("DistanceUnit") = value
        End Set
    End Property

    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.Distance.ToString()
        ClientInteraction = ClientInteractionEnum.PolylineInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("DistanceMapToolHelpText")
        ClientCommand = ClientCommandEnum.DistanceCommand.ToString()
    End Sub
    '/// <summary>
    '/// Renders javascript to create command and interaction objects on the client side
    '/// </summary>
    '/// <param name="writer">HtmlTextWriter</param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        RenderJS(writer, ClientCommand, "ImgTool")
        writer.AddAttribute("language", "javascript")
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        writer.RenderBeginTag(HtmlTextWriterTag.Script)
        writer.WriteLine(String.Format("{0}Cmd.distanceType = '{1}';", UniqueID, DistanceType.ToString()))
        writer.WriteLine(String.Format("{0}Cmd.distanceUnit = '{1}';", UniqueID, DistanceUnit.ToString()))
        writer.RenderEndTag()
    End Sub
End Class
'/// <summary>
'/// Class for CenterTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>

<ToolboxData("<{0}:CenterTool runat=server></{0}:CenterTool>")> _
    Public Class CenterTool
    Inherits WebTool
    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>

    Public Sub New()
        Command = CommandEnum.Center.ToString()
        ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("CenterMapToolHelpText")
    End Sub
End Class

'/// <summary>
'/// Class for PointSelectionTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>

<ToolboxData("<{0}:PointSelectionTool runat=server></{0}:PointSelectionTool>")> _
Public Class PointSelectionTool
    Inherits WebTool
    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.PointSelection.ToString()
        ClientInteraction = ClientInteractionEnum.ClickInteraction.ToString()
        ClientCommand = ClientCommandEnum.PointSelectionCommand.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("PointSelectionMapToolHelpText")
    End Sub
    '/ <summary>
    '/ Pixel tolerance for doing the search
    '/ </summary>
    '/ <remarks>The pixel tolerance is converted to distance in map units and a buffer is created around the given point 
    '/ then first feature to intersect this buffered point is added to default selection
    '/ </remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
  Public Property PixelTolerance() As Integer
        Get
            If ViewState("PixelTolerance") Is Nothing Then
                Return 6
            Else
                Return CType(ViewState("PixelTolerance"), Integer)
            End If
        End Get
        Set(ByVal Value As Integer)
            If Value <= 0 Then
                Throw New ArgumentOutOfRangeException
            End If
            ViewState("PixelTolerance") = Value
        End Set
    End Property
    '/ <summary>
    '/ Renders javascript to create command and interaction objects on the client side
    '/ </summary>
    '/ <param name="writer">HtmlTextWriter</param>
    Protected Overloads Overrides Sub RenderJS(ByVal writer As HtmlTextWriter)
        ' Since this is runtime behavior do it only if there is context
        RenderJS(writer, ClientCommand, "ImgTool")
        writer.AddAttribute("language", "javascript")
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
        writer.RenderBeginTag(HtmlTextWriterTag.Script)
        writer.WriteLine(String.Format("{0}Cmd.pixelTolerance = {1};", UniqueID, PixelTolerance))
        writer.RenderEndTag()
    End Sub
End Class
'/// <summary>
'/// Class for RectangleSelectionTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>

<ToolboxData("<{0}:RectangleSelectionTool runat=server></{0}:RectangleSelectionTool>")> _
Public Class RectangleSelectionTool
    Inherits WebTool
    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.RectangleSelection.ToString()
        ClientInteraction = ClientInteractionEnum.RectInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("RectangleSelectionMapToolHelpText")
    End Sub
End Class
'/// <summary>
'/// Class for PolygonSelectionTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>

<ToolboxData("<{0}:PolygonSelectionTool runat=server></{0}:PolygonSelectionTool>")> _
Public Class PolygonSelectionTool
    Inherits WebTool

    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.PolygonSelection.ToString()
        ClientInteraction = ClientInteractionEnum.PolygonInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("PolygonSelectionMapToolHelpText")
    End Sub
End Class

'/// <summary>
'/// Class for RadiusSelectionTool
'/// </summary>
'/// <remarks>
'/// None
'/// </remarks>
<ToolboxData("<{0}:RadiusSelectionTool runat=server></{0}:RadiusSelectionTool>")> _
Public Class RadiusSelectionTool
    Inherits WebTool
    '/// <summary>
    '/// The constructor sets the command name , the interaction it is going to perform and the cursor it is going to use.
    '/// </summary>
    '/// <remarks>None</remarks>
    Public Sub New()
        Command = CommandEnum.RadiusSelection.ToString()
        ClientInteraction = ClientInteractionEnum.RadInteraction.ToString()
        Active = False
        CursorImageUrl = String.Format("{0}/MapInfoWeb{1}.cur", Resources.ResourceFolder, Command)
        ToolTip = L10NUtils.Resources.GetString("RadiusSelectionMapToolHelpText")
    End Sub
End Class


