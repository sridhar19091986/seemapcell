Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.Web.UI.WebControls

'/// <summary>
'/// Base class for all the tools
'/// </summary>
'/// <remarks>A tool is defined as a entity which does some interaction on the map and after it is finished a command is executed on the
'/// server side and resulting new map is sent back to the client. Instead of following the regular architecture of the webcontrols by doing
'/// a postback on each operation which forces all controls on the page to redraw, these tools just update the map by setting the source of 
'/// map image to a url which contains the command and data needed to do the operation. The url contains the name of the controller which is 
'/// going to handle the command. This controller parses the url gets command and data out of it and then does the operation.
'/// This architecture basically has server and client components
'/// Client: The javascript to do interaction on the map, command to execute follows object oriented design using inheritance. It is easy to extend 
'/// and also multi-browser friendly. When tools are rendered, they render javascript to create interaction, tool and command objects they need.
'/// when the tool is activated, event handlers are set to do interaction on the map like clicking or drawing a rectangle etc. when interaction is finished
'/// Execute method of the command is invoked which sets the source of the map image to url containing the command and data, as soon as this is done
'/// the request is sent to server.
'/// Server: The httphandler is called from client with url. The httphandler on the server side is called the controller. Controller gets all parameters
'/// commands and data from the context and delegates execution of the command to the Model. Model contains the actual code to do the operation
'/// After operation is done, the map is exported into an image and the image is streamed back to the client.
'/// </remarks>
Public Class WebTool
    Inherits WebControl
    Implements IPostBackDataHandler

    '/ <summary>
    '/ Enum for Client side interaction
    '/ </summary>
    '/ <remarks>This is the list of interactions that can be performed on the client side on the map.</remarks>
    Public Enum ClientInteractionEnum
        '/ <summary>
        '/ Noop interaction on the client side.
        '/ </summary>
        '/ <remarks>When there is no interaction done by tool, this may be used.</remarks>
        NullInteraction
        '/ <summary>
        '/ Click interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>
        ClickInteraction
        '/ <summary>
        '/ Rectangle interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>
        RectInteraction
        '/ <summary>
        '/ Radius interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>
        RadInteraction
        '/ <summary>
        '/ Drag interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>
        DragInteraction
        '/ <summary>
        '/ Polyline interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>
        PolylineInteraction
        '/ <summary>
        '/ Polygon interaction on the client side.
        '/ </summary>
        '/ <remarks>None</remarks>	
        PolygonInteraction
    End Enum

    '/ <summary>
    '/ Enum for commands to be executed on the server side.
    '/ </summary>
    '/ <remarks>This name is used in the url which is sent back.</remarks>
    Public Enum CommandEnum
        '/ <summary>
        '/ This is just null command.
        '/ </summary>
        '/ <remarks>None</remarks>
        NullCommand
        '/ <summary>
        '/ ZoomIn command
        '/ </summary>
        '/ <remarks>None</remarks>
        ZoomIn
        '/ <summary>
        '/ ZoomOut command.
        '/ </summary>
        '/ <remarks>None</remarks>
        ZoomOut
        '/ <summary>
        '/ ZoomWithFactor command.
        '/ </summary>
        '/ <remarks>None</remarks>
        ZoomWithFactor
        '/ <summary>
        '/ ZoomToLevel command.
        '/ </summary>
        '/ <remarks>None</remarks>
        ZoomToLevel
        '/ <summary>
        '/ Center command.
        '/ </summary>
        '/ <remarks>None</remarks>
        Center
        '/ <summary>
        '/ Pan command.
        '/ </summary>
        '/ <remarks>None</remarks>
        Pan
        '/ <summary>
        '/ Distance command.
        '/ </summary>
        '/ <remarks>None</remarks>
        Distance
        '/ <summary>
        '/ Info command.
        '/ </summary>
        '/ <remarks>None</remarks>
        Info
        '/ <summary>
        '/ PointSelection command.
        '/ </summary>
        '/ <remarks>None</remarks>
        PointSelection
        '/ <summary>
        '/ RectangleSelection command.
        '/ </summary>
        '/ <remarks>None</remarks>
        RectangleSelection
        '/ <summary>
        '/ RadiusSelection command.
        '/ </summary>
        '/ <remarks>None</remarks>
        RadiusSelection
        '/ <summary>
        '/ PolygonSelection command.
        '/ </summary>
        '/ <remarks>None</remarks>
        PolygonSelection
        '/ <summary>
        '/ Navigate command.
        '/ </summary>
        '/ <remarks>None</remarks>
        Navigate
        '/ <summary>
        '/ LayerVisibility command.
        '/ </summary>
        '/ <remarks>None</remarks>
        LayerVisibility
    End Enum

    '/ <summary>
    '/ Enum for names of the command objects that exist on the client side.
    '/ </summary>
    '/ <remarks>These command objects create url based on name and interaction, create urls with commands and data and send to server
    '/ and get response back and apply it appropriately.
    '/ </remarks>
    Public Enum ClientCommandEnum
        '/ <summary>
        '/ MapComand
        '/ </summary>
        '/ <remarks>This command's responsibility is to update map. It forms the url and sets the map image's source to this url which updates 
        '/ just the map.</remarks>
        MapCommand
        '/ <summary>
        '/ PanCommand
        '/ </summary>
        '/ <remarks>Pan command is almost same as MapCommand except that since user has dragged the image, it resets the image to proper location.</remarks>
        PanCommand
        '/ <summary>
        '/ DistanceCommand
        '/ </summary>
        '/ <remarks>Distance command uses XmlHttp object to send request to server and get response back to display the distance.</remarks>
        DistanceCommand
        '/ <summary>
        '/ NavigateCommand
        '/ </summary>
        '/ <remarks>Navigate command is same as MapComand except that there are additional parameters added to the url. This command is used by Navigation tools.</remarks>
        NavigateCommand
        '/ <summary>
        '/ PointSelectionCommand
        '/ </summary>
        '/ <remarks>PointSelection command is same as MapComand except pixeltolerance is added to the url.</remarks>
        PointSelectionCommand
        '/ <summary>
        '/ ZoomCommand
        '/ </summary>
        '/ <remarks>ZoomCommand is same as MapCommand except zoomfactor is added to the url. This is used by ZoomBar tools.</remarks>
        ZoomCommand
    End Enum

    '/// <summary>
    '/// Constructor for tool
    '/// </summary>
    '/// <remarks>Tools set appropriate properties to suit their needs in this constuctor</remarks>

    Public Sub New()
        Me.Active = False
        Me.ClientInteraction = ClientInteractionEnum.NullInteraction.ToString()
        Me.ClientCommand = ClientCommandEnum.MapCommand.ToString()
        Me.Command = CommandEnum.NullCommand.ToString()
        ResourcesPath = Resources.ResourceFolder
        Me.InactiveImageUrl = String.Format("{0}/{1}ControlInactive.gif", Resources.ResourceFolder, MyBase.GetType.Name)
        Me.ActiveImageUrl = String.Format("{0}/{1}ControlActive.gif", Resources.ResourceFolder, MyBase.GetType.Name)
    End Sub
    '///<summary>Gets or sets the boolean whether the tool is active or not</summary>
    '///<value>Gets or sets the boolean to indicate whether the tool is active or not.</value>
    '///<remarks>In the render method, if this flag is true then the tool is rendered as active.</remarks>

    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property Active() As Boolean
        Get
            Return CType(Me.ViewState("Active"), Boolean)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState.Item("Active") = value
        End Set
    End Property
    '/ <summary>
    '/ Name of the command to be executed
    '/ </summary>
    '/ <remarks>This is sent by client to server in the URL. On server side it is parsed from url and a appropriate command object is used to execute and send response back</remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute), _
    TypeConverter(GetType(CommandListConverter))> _
    Public Overridable Property Command() As String
        Get
            Return CType(ViewState("Command"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("Command") = Value
        End Set
    End Property

    '/ <summary>
    '/ Name of the command used on the client side.
    '/ </summary>
    '/ <remarks>The javascript client side command is made up of interaction and name of the command to be sent to server.
    '/ This command basically creates appropriate url and sends it to server to get response back.
    '/ This allows users to pick custom command which you may have written.</remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute), _
    TypeConverter(GetType(ClientCommandListConverter))> _
    Public Overridable Property ClientCommand() As String
        Get
            Return CType(ViewState("ClientCommand"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("ClientCommand") = Value
        End Set
    End Property

    '/ <summary>
    '/ Name of the interaction to be performed on the client side
    '/ </summary>
    '/ <remarks>The javascript code for performing various interactions on the map exists. 
    '/ This allows users to pick which interaction to choose for a custom tool they are going to write. For known tools like ZoomIn or PointSelection, 
    '/ this property is read only. </remarks>
    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute), _
    TypeConverter(GetType(InteractionListConverter))> _
    Public Overridable Property ClientInteraction() As String
        Get
            Return CType(ViewState("ClientInteraction"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("ClientInteraction") = Value
        End Set
    End Property

    '///<summary>Gets or sets the URL for an image when the tool control is selected.</summary>
    '///<value>Gets or sets the URL for image when ToolControl is selected.</value>
    '///<remarks>This property gets or sets the URL for an image when the tool is selected. This URL address includes the path.</remarks>

    <Browsable(True), Editor(GetType(UrlEditor), GetType(System.Drawing.Design.UITypeEditor)), PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)> _
    Public Property ActiveImageUrl() As String
        Get
            Return CType(Me.ViewState.Item("ActiveImageUrl"), String)
        End Get
        Set(ByVal value As String)
            Me.ViewState.Item("ActiveImageUrl") = value
        End Set
    End Property

    '		/// <summary>
    '		/// URL pointing to root of all resources used by MapXtreme web controls.
    '		/// </summary>
    '		/// <remarks>The users can change this root to point to their own root instead of default MapXtremeWebResources xx_xx root.
    '		/// <para>
    '		/// There are three files Interaction.js, Command.js and Tool.js that are used by MapXtreme webcontrols. These script statements are rendered at 
    '		/// runtime. This root is used in the path. If you want to customize the scripts, you can either add them to these files. Or if there are additional files
    '		/// you want to render, you can manually put script statements in the webform.
    '		/// </para>
    '		///  </remarks>

    <PersistenceMode(PersistenceMode.Attribute), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Editor(GetType(UrlEditor), GetType(System.Drawing.Design.UITypeEditor)), Browsable(True)> _
    Public Property ResourcesPath() As String
        Get
            Return CType(Me.ViewState.Item("ResourcesPath"), String)
        End Get
        Set(ByVal value As String)
            Me.ViewState.Item("ResourcesPath") = value
        End Set
    End Property
    '///<summary>Gets or sets the URL for the cursor which is set when this toolcontrol is selected.</summary>
    '///<value>Gets or sets the URL for the cursor, which is set when this ToolControl
    '/// is selected, when the cursor hovers over MapControl.</value>
    '///<remarks>The file should be a cursor file. On Nescape and FireFox browsers the cursor files don't work</remarks>

    <Editor(GetType(UrlEditor), GetType(System.Drawing.Design.UITypeEditor)), Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property CursorImageUrl() As String
        Get
            Return CType(Me.ViewState.Item("CursorImageUrl"), String)
        End Get
        Set(ByVal value As String)
            Me.ViewState.Item("CursorImageUrl") = value
        End Set
    End Property
    '///<summary>Gets or sets the URL for an image when the tool control is not selected.</summary>
    '///<value>Gets or sets the URL for the image when ToolControl is not selected.</value>
    '///<remarks>This property gets or sets the URL for an image (including the path) when the ToolControl is not selected.</remarks>

    <Editor(GetType(UrlEditor), GetType(System.Drawing.Design.UITypeEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute), Browsable(True)> _
    Public Property InactiveImageUrl() As String
        Get
            Return CType(Me.ViewState.Item("InactiveImageUrl"), String)
        End Get
        Set(ByVal value As String)
            Me.ViewState.Item("InactiveImageUrl") = value
        End Set
    End Property

    '/// <summary>
    '/// ID of the mapcontrol which this tool points to
    '/// </summary>
    '/// <remarks>This parameter basically tells tool which map to use on the client side. The MapControl renders the IMG tag which is used
    '/// by the tool to do interaction</remarks>

    <TypeConverter(GetType(ServerControlConverter)), Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), PersistenceMode(PersistenceMode.Attribute)> _
    Public Property MapControlID() As String
        Get
            Return CType(Me.ViewState.Item("MapControlID"), String)
        End Get
        Set(ByVal value As String)
            Me.ViewState.Item("MapControlID") = value
        End Set
    End Property

    '/// <summary>
    '/// Render all javascript needed.
    '/// </summary>
    '/// <param name="e">EventArgs</param>
    '/// <remarks>None</remarks>
    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        Dim codeName As String = "Interaction.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (ResourcesPath + "/" + codeName)))
        End If
        codeName = "Command.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (ResourcesPath + "/" + codeName)))
        End If
        codeName = "Tool.js"
        If Not Me.Page.IsClientScriptBlockRegistered(codeName) Then
            Dim format As String = ChrW(10) & "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"
            Me.Page.RegisterClientScriptBlock(codeName, String.Format(format, (ResourcesPath + "/" + codeName)))
        End If
    End Sub

    '/// <summary>
    '/// Set the state of the tool after postback
    '/// </summary>
    '/// <param name="postDataKey">Key for identifying whether it is command we need to process</param>
    '/// <param name="postCollection">Collection of all form values</param>
    '/// <returns>false</returns>

    Public Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean Implements System.Web.UI.IPostBackDataHandler.LoadPostData
        'Set the flag if this tool is active
        Dim value As String = postCollection.Item(Me.UniqueID & "Active")
        If ((value <> Nothing) AndAlso (value.IndexOf(Me.UniqueID) >= 0) AndAlso (value.IndexOf(Me.MapControlID) >= 0)) Then
            Me.Context.Session.Item((Me.UniqueID & "Active")) = 1
        Else
            Me.Context.Session.Item((Me.UniqueID & "Active")) = 0
        End If
        Return False
    End Function
    '/// <summary>
    '/// NoOp
    '/// </summary>
    '/// <remarks>None</remarks>

    Public Sub RaisePostDataChangedEvent() Implements System.Web.UI.IPostBackDataHandler.RaisePostDataChangedEvent
    End Sub
    '/// <summary>
    '/// Render javascript to create interaction, command and tool objects to be used for operation on the client side
    '/// </summary>
    '/// <param name="writer">HtmlTextWriter</param>
    '/// <remarks>None</remarks>

    Protected Overridable Sub RenderJS(ByVal writer As HtmlTextWriter, ByVal clientCommandName As String, ByVal clientToolName As String)
        If (Not Me.Context Is Nothing) Then
            writer.AddAttribute("language", "javascript")
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript")
            writer.RenderBeginTag(HtmlTextWriterTag.Script)
            writer.WriteLine(String.Format("var {0}Int = new {1}('{2}_Image', null);", UniqueID, ClientInteraction, MapControlID))
            writer.WriteLine(String.Format("var {0}Cmd = new {1}('{2}', {0}Int);", UniqueID, clientCommandName, Command))
            If Not clientToolName Is Nothing Then
                writer.WriteLine(String.Format("var {0}Tool = new {1}('{0}', {0}Int, {0}Cmd);", UniqueID, clientToolName))
            End If
            writer.WriteLine(String.Format("{0}Tool.activeImg = ""{1}"";", Me.UniqueID, Me.ActiveImageUrl))
            writer.WriteLine(String.Format("{0}Tool.inactiveImg = ""{1}"";", Me.UniqueID, Me.InactiveImageUrl))
            writer.WriteLine(String.Format("{0}Tool.cursorImg = ""{1}"";", Me.UniqueID, Me.CursorImageUrl))
            writer.RenderEndTag()
        End If
    End Sub

    Protected Overridable Sub RenderJS(ByVal writer As HtmlTextWriter)
        ' Since this is runtime behavior do it only if there is context
        RenderJS(writer, ClientCommand, "ImgTool")
    End Sub

    '/ <summary>
    '/ Enable postback mechanism
    '/ </summary>
    '/ <param name="e">EventArgs</param>
    '/ <remarks>The tools themselves don't do postback after operations, but applications may do that. In that case if the tool was
    '/ active before postback, it must be active after after page is rendered.</remarks>
    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)
        Page.RegisterRequiresPostBack(Me)
    End Sub

    '/// <summary>
    '/// Render the control.
    '/// </summary>
    '/// <param name="writer">HtmlTextWriter</param>
    '/// <remarks>Render the tool as image tag appropriate javascript</remarks>

    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        If (Not Me.Page Is Nothing) Then
            Me.Page.VerifyRenderingInServerForm(Me)
        End If

        'Render HTML
        writer.AddAttribute(HtmlTextWriterAttribute.Id, (Me.UniqueID & "_Image"))
        writer.AddAttribute(HtmlTextWriterAttribute.Src, Me.InactiveImageUrl)
        writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip)
        writer.AddAttribute(HtmlTextWriterAttribute.Alt, ToolTip)
        writer.RenderBeginTag(HtmlTextWriterTag.Img)
        writer.RenderEndTag()

        ' Write a hidden variable to store tool's state whether it is active or not
        writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden")
        writer.AddAttribute(HtmlTextWriterAttribute.Id, UniqueID + "Active")
        writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "Active")
        writer.RenderBeginTag(HtmlTextWriterTag.Input)
        writer.RenderEndTag()

        'Render Javscript
        Me.RenderJS(writer)

        'Activate the tool if active
        If (Not Me.Context Is Nothing) Then
            Dim active As Boolean = False
            If ((Not Me.Context.Session.Item((Me.UniqueID & "Active")) Is Nothing) AndAlso (CType(Me.Context.Session.Item((Me.UniqueID & "Active")), Integer) = 1)) Then
                active = True
            End If
            If (Me.Active OrElse active) Then
                Dim activateCode As String = String.Format("{0}Tool.Activate();", UniqueID)
                Dim activateScript As String = String.Format("<script type='text/javascript'>AppendScriptToForm('{0}');</script>", activateCode)
                Page.RegisterStartupScript("Key1", activateScript)
            End If
        End If
    End Sub
End Class
