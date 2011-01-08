Imports System.Reflection
Imports System.IO
Imports MapInfo.Data
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Windows.Dialogs
Imports MapInfo.Geocoding
Imports MapInfo.Routing
Imports MapInfo.Geometry


Public Class MapForm1
    Inherits System.Windows.Forms.Form

    Friend WithEvents Map As MapInfo.Mapping.Map
    Friend WithEvents Layers As MapInfo.Mapping.Layers

    ' default URLs for routing and geocoding:
    ' miAware demo server geocoding URL = "http://demetrius.mapinfo.com:8080/Route/services/Route"
    ' Standalone MapMarker J Server default URL = "http://{servername}:8095/mapmarker20/servlet/mapmarker"
    Dim strRjUrl As String

    ' miAware demo server routing URL = "http://demetrius.mapinfo.com:8080/LocationUtility/services/LocationUtility"
    ' Standalone Routing J Server default URL ="http://{servername}:8090/routing30/servlet/routing"
    Dim strMmjUrl As String

    Dim _layerDlg As LayerControlDlg

    Dim vbCrLf As String = Chr(13) & Chr(10)


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'theSession = Session.Current
        'theSession = Nothing

        ' Listen to some map events
        Map = MapControl1.Map
        Layers = MapControl1.Map.Layers
        AddHandler Map.ViewChangedEvent, AddressOf Map_ViewChanged
        AddHandler Layers.Added, AddressOf Me.Layers_CountChanged
        AddHandler Layers.Removed, AddressOf Me.Layers_CountChanged

        ' Extra toolbar setup
        ToolBarSetup()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents MapControl1 As MapInfo.Windows.Controls.MapControl
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBarButtonFileOpen As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonSelect As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonZoomIn As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonZoomOut As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonPan As System.Windows.Forms.ToolBarButton
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents ToolBarButtonLayerControl As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButtonSep As System.Windows.Forms.ToolBarButton
    Friend WithEvents panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents txtState1 As System.Windows.Forms.TextBox
    Friend WithEvents txtCity1 As System.Windows.Forms.TextBox
    Friend WithEvents txtCity2 As System.Windows.Forms.TextBox
    Friend WithEvents txtState2 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtZip2 As System.Windows.Forms.TextBox
    Friend WithEvents txtZip1 As System.Windows.Forms.TextBox
    Friend WithEvents btnRouting As System.Windows.Forms.Button
    Friend WithEvents txtMmjUrl As System.Windows.Forms.TextBox
    Friend WithEvents lblMmjUrl As System.Windows.Forms.Label
    Friend WithEvents lblRjUrl As System.Windows.Forms.Label
    Friend WithEvents radioUseMiAware As System.Windows.Forms.RadioButton
    Friend WithEvents radioUseJServers As System.Windows.Forms.RadioButton
    Friend WithEvents txtMiAwareUrl As System.Windows.Forms.TextBox
    Friend WithEvents lblMiAwareUrl As System.Windows.Forms.Label
    Friend WithEvents txtRjUrl As System.Windows.Forms.TextBox
    Friend WithEvents btnClearAddresses As System.Windows.Forms.Button
    Friend WithEvents btnClearMap As System.Windows.Forms.Button
    Friend WithEvents txtResults As System.Windows.Forms.TextBox

    Private Sub InitializeComponent()
        Me.btnRouting = New System.Windows.Forms.Button
        Me.txtMmjUrl = New System.Windows.Forms.TextBox
        Me.lblMmjUrl = New System.Windows.Forms.Label
        Me.lblRjUrl = New System.Windows.Forms.Label
        Me.txtRjUrl = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.MapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.ToolBarButtonFileOpen = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonLayerControl = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonSep = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonSelect = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonZoomIn = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonZoomOut = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonPan = New System.Windows.Forms.ToolBarButton
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.panel2 = New System.Windows.Forms.Panel
        Me.btnClearMap = New System.Windows.Forms.Button
        Me.btnClearAddresses = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtState1 = New System.Windows.Forms.TextBox
        Me.txtCity1 = New System.Windows.Forms.TextBox
        Me.txtCity2 = New System.Windows.Forms.TextBox
        Me.txtState2 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtZip2 = New System.Windows.Forms.TextBox
        Me.txtZip1 = New System.Windows.Forms.TextBox
        Me.radioUseMiAware = New System.Windows.Forms.RadioButton
        Me.radioUseJServers = New System.Windows.Forms.RadioButton
        Me.txtMiAwareUrl = New System.Windows.Forms.TextBox
        Me.lblMiAwareUrl = New System.Windows.Forms.Label
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRouting
        '
        Me.btnRouting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRouting.Location = New System.Drawing.Point(440, 8)
        Me.btnRouting.Name = "btnRouting"
        Me.btnRouting.Size = New System.Drawing.Size(96, 32)
        Me.btnRouting.TabIndex = 4
        Me.btnRouting.Text = "Find Route"
        '
        'txtMmjUrl
        '
        Me.txtMmjUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMmjUrl.Enabled = False
        Me.txtMmjUrl.Location = New System.Drawing.Point(296, 48)
        Me.txtMmjUrl.Name = "txtMmjUrl"
        Me.txtMmjUrl.Size = New System.Drawing.Size(248, 20)
        Me.txtMmjUrl.TabIndex = 19
        Me.txtMmjUrl.Text = "<MMJservername>"
        '
        'lblMmjUrl
        '
        Me.lblMmjUrl.Enabled = False
        Me.lblMmjUrl.Location = New System.Drawing.Point(208, 48)
        Me.lblMmjUrl.Name = "lblMmjUrl"
        Me.lblMmjUrl.Size = New System.Drawing.Size(96, 20)
        Me.lblMmjUrl.TabIndex = 20
        Me.lblMmjUrl.Text = "MMJ Server URL: "
        Me.lblMmjUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRjUrl
        '
        Me.lblRjUrl.Enabled = False
        Me.lblRjUrl.Location = New System.Drawing.Point(208, 72)
        Me.lblRjUrl.Name = "lblRjUrl"
        Me.lblRjUrl.Size = New System.Drawing.Size(88, 20)
        Me.lblRjUrl.TabIndex = 22
        Me.lblRjUrl.Text = "RJ Server URL:"
        Me.lblRjUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRjUrl
        '
        Me.txtRjUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRjUrl.Enabled = False
        Me.txtRjUrl.Location = New System.Drawing.Point(296, 72)
        Me.txtRjUrl.Name = "txtRjUrl"
        Me.txtRjUrl.Size = New System.Drawing.Size(248, 20)
        Me.txtRjUrl.TabIndex = 21
        Me.txtRjUrl.Text = "<RJservername>"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.MapControl1)
        Me.Panel1.Location = New System.Drawing.Point(28, 304)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(516, 240)
        Me.Panel1.TabIndex = 0
        '
        'MapControl1
        '
        Me.MapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MapControl1.Location = New System.Drawing.Point(0, 0)
        Me.MapControl1.Name = "MapControl1"
        Me.MapControl1.Size = New System.Drawing.Size(512, 236)
        Me.MapControl1.TabIndex = 0
        Me.MapControl1.Text = "MapControl1"
        Me.MapControl1.Tools.MiddleButtonTool = Nothing
        '
        'ToolBar1
        '
        Me.ToolBar1.AutoSize = False
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.ToolBarButtonFileOpen, Me.ToolBarButtonLayerControl, Me.ToolBarButtonSep, Me.ToolBarButtonSelect, Me.ToolBarButtonZoomIn, Me.ToolBarButtonZoomOut, Me.ToolBarButtonPan})
        Me.ToolBar1.ButtonSize = New System.Drawing.Size(22, 23)
        Me.ToolBar1.Divider = False
        Me.ToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.Location = New System.Drawing.Point(0, 304)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(27, 148)
        Me.ToolBar1.TabIndex = 1
        '
        'ToolBarButtonFileOpen
        '
        Me.ToolBarButtonFileOpen.ToolTipText = "Open Table"
        '
        'ToolBarButtonLayerControl
        '
        Me.ToolBarButtonLayerControl.ToolTipText = "Layer Control"
        '
        'ToolBarButtonSep
        '
        Me.ToolBarButtonSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'ToolBarButtonSelect
        '
        Me.ToolBarButtonSelect.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.ToolBarButtonSelect.Tag = "Select"
        Me.ToolBarButtonSelect.ToolTipText = "Select"
        '
        'ToolBarButtonZoomIn
        '
        Me.ToolBarButtonZoomIn.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.ToolBarButtonZoomIn.Tag = "ZoomIn"
        Me.ToolBarButtonZoomIn.ToolTipText = "Zoom-in"
        '
        'ToolBarButtonZoomOut
        '
        Me.ToolBarButtonZoomOut.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.ToolBarButtonZoomOut.Tag = "ZoomOut"
        Me.ToolBarButtonZoomOut.ToolTipText = "Zoom-out"
        '
        'ToolBarButtonPan
        '
        Me.ToolBarButtonPan.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.ToolBarButtonPan.Tag = "Pan"
        Me.ToolBarButtonPan.ToolTipText = "Pan"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 543)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(552, 22)
        Me.StatusBar1.TabIndex = 2
        '
        'panel2
        '
        Me.panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel2.BackColor = System.Drawing.SystemColors.Control
        Me.panel2.Controls.Add(Me.txtResults)
        Me.panel2.Controls.Add(Me.btnClearMap)
        Me.panel2.Controls.Add(Me.btnClearAddresses)
        Me.panel2.Controls.Add(Me.Label8)
        Me.panel2.Controls.Add(Me.Label7)
        Me.panel2.Controls.Add(Me.txtAddress2)
        Me.panel2.Controls.Add(Me.txtAddress1)
        Me.panel2.Controls.Add(Me.txtState1)
        Me.panel2.Controls.Add(Me.txtCity1)
        Me.panel2.Controls.Add(Me.txtCity2)
        Me.panel2.Controls.Add(Me.txtState2)
        Me.panel2.Controls.Add(Me.Label6)
        Me.panel2.Controls.Add(Me.Label5)
        Me.panel2.Controls.Add(Me.Label4)
        Me.panel2.Controls.Add(Me.Label3)
        Me.panel2.Controls.Add(Me.Label2)
        Me.panel2.Controls.Add(Me.Label1)
        Me.panel2.Controls.Add(Me.txtZip2)
        Me.panel2.Controls.Add(Me.txtZip1)
        Me.panel2.Controls.Add(Me.btnRouting)
        Me.panel2.Location = New System.Drawing.Point(0, 96)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(544, 208)
        Me.panel2.TabIndex = 18
        '
        'btnClearMap
        '
        Me.btnClearMap.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearMap.Location = New System.Drawing.Point(440, 88)
        Me.btnClearMap.Name = "btnClearMap"
        Me.btnClearMap.Size = New System.Drawing.Size(96, 32)
        Me.btnClearMap.TabIndex = 24
        Me.btnClearMap.Text = "Clear Map"
        '
        'btnClearAddresses
        '
        Me.btnClearAddresses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearAddresses.Location = New System.Drawing.Point(440, 48)
        Me.btnClearAddresses.Name = "btnClearAddresses"
        Me.btnClearAddresses.Size = New System.Drawing.Size(96, 32)
        Me.btnClearAddresses.TabIndex = 23
        Me.btnClearAddresses.Text = "Clear Addresses"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.Location = New System.Drawing.Point(320, 96)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 20)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "ZIP:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.Location = New System.Drawing.Point(320, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(32, 20)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "ZIP:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress2
        '
        Me.txtAddress2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress2.Location = New System.Drawing.Point(112, 72)
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.Size = New System.Drawing.Size(320, 20)
        Me.txtAddress2.TabIndex = 11
        Me.txtAddress2.Text = ""
        '
        'txtAddress1
        '
        Me.txtAddress1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddress1.Location = New System.Drawing.Point(112, 8)
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.Size = New System.Drawing.Size(320, 20)
        Me.txtAddress1.TabIndex = 6
        Me.txtAddress1.Text = ""
        '
        'txtState1
        '
        Me.txtState1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtState1.Location = New System.Drawing.Point(272, 32)
        Me.txtState1.Name = "txtState1"
        Me.txtState1.Size = New System.Drawing.Size(32, 20)
        Me.txtState1.TabIndex = 8
        Me.txtState1.Text = ""
        '
        'txtCity1
        '
        Me.txtCity1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCity1.Location = New System.Drawing.Point(32, 32)
        Me.txtCity1.Name = "txtCity1"
        Me.txtCity1.Size = New System.Drawing.Size(192, 20)
        Me.txtCity1.TabIndex = 7
        Me.txtCity1.Text = ""
        '
        'txtCity2
        '
        Me.txtCity2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCity2.Location = New System.Drawing.Point(32, 96)
        Me.txtCity2.Name = "txtCity2"
        Me.txtCity2.Size = New System.Drawing.Size(192, 20)
        Me.txtCity2.TabIndex = 12
        Me.txtCity2.Text = ""
        '
        'txtState2
        '
        Me.txtState2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtState2.Location = New System.Drawing.Point(272, 96)
        Me.txtState2.Name = "txtState2"
        Me.txtState2.Size = New System.Drawing.Size(32, 20)
        Me.txtState2.TabIndex = 13
        Me.txtState2.Text = ""
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.Location = New System.Drawing.Point(240, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 20)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "State:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.Location = New System.Drawing.Point(240, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 20)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "State:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 20)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "City:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 20)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "City:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 20)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Destination Address:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 20)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Starting Address:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtZip2
        '
        Me.txtZip2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtZip2.Location = New System.Drawing.Point(352, 96)
        Me.txtZip2.Name = "txtZip2"
        Me.txtZip2.Size = New System.Drawing.Size(80, 20)
        Me.txtZip2.TabIndex = 14
        Me.txtZip2.Text = ""
        '
        'txtZip1
        '
        Me.txtZip1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtZip1.Location = New System.Drawing.Point(352, 32)
        Me.txtZip1.Name = "txtZip1"
        Me.txtZip1.Size = New System.Drawing.Size(80, 20)
        Me.txtZip1.TabIndex = 9
        Me.txtZip1.Text = ""
        '
        'radioUseMiAware
        '
        Me.radioUseMiAware.Checked = True
        Me.radioUseMiAware.Location = New System.Drawing.Point(8, 5)
        Me.radioUseMiAware.Name = "radioUseMiAware"
        Me.radioUseMiAware.Size = New System.Drawing.Size(160, 16)
        Me.radioUseMiAware.TabIndex = 23
        Me.radioUseMiAware.TabStop = True
        Me.radioUseMiAware.Text = "Use miAware Demo Server"
        '
        'radioUseJServers
        '
        Me.radioUseJServers.Location = New System.Drawing.Point(8, 51)
        Me.radioUseJServers.Name = "radioUseJServers"
        Me.radioUseJServers.Size = New System.Drawing.Size(200, 16)
        Me.radioUseJServers.TabIndex = 24
        Me.radioUseJServers.Text = "Use MapMarker/Routing J Servers:"
        '
        'txtMiAwareUrl
        '
        Me.txtMiAwareUrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMiAwareUrl.Location = New System.Drawing.Point(104, 24)
        Me.txtMiAwareUrl.Name = "txtMiAwareUrl"
        Me.txtMiAwareUrl.Size = New System.Drawing.Size(440, 20)
        Me.txtMiAwareUrl.TabIndex = 25
        Me.txtMiAwareUrl.Text = "demetrius.mapinfo.com"
        '
        'lblMiAwareUrl
        '
        Me.lblMiAwareUrl.Location = New System.Drawing.Point(8, 24)
        Me.lblMiAwareUrl.Name = "lblMiAwareUrl"
        Me.lblMiAwareUrl.Size = New System.Drawing.Size(96, 20)
        Me.lblMiAwareUrl.TabIndex = 26
        Me.lblMiAwareUrl.Text = "miAware URL: "
        Me.lblMiAwareUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtResults
        '
        Me.txtResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtResults.Location = New System.Drawing.Point(8, 128)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtResults.Size = New System.Drawing.Size(528, 72)
        Me.txtResults.TabIndex = 25
        Me.txtResults.Text = "[Geocoding and routing status and/or error messages will be displayed here]"
        '
        'MapForm1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(552, 565)
        Me.Controls.Add(Me.txtMiAwareUrl)
        Me.Controls.Add(Me.lblMiAwareUrl)
        Me.Controls.Add(Me.txtRjUrl)
        Me.Controls.Add(Me.txtMmjUrl)
        Me.Controls.Add(Me.radioUseJServers)
        Me.Controls.Add(Me.radioUseMiAware)
        Me.Controls.Add(Me.lblRjUrl)
        Me.Controls.Add(Me.lblMmjUrl)
        Me.Controls.Add(Me.panel2)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.ToolBar1)
        Me.Controls.Add(Me.Panel1)
        Me.MinimumSize = New System.Drawing.Size(440, 504)
        Me.Name = "MapForm1"
        Me.Text = "Geocoding and Routing VB sample"
        Me.Panel1.ResumeLayout(False)
        Me.panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Map- and toolbar-handling code"
    ' MapToolBar cannot currently be used with this layout, since it
    '  enforces a left-to-right toolbar orientation.  A standard toolbar
    '  can be used instead, with the following setup and event code:
    '
    ' *******************************************
    ' *  standard map-handling code begins here *
    ' *******************************************

    ' This method creates an image list from a bitmap resource,
    ' associates the imagelist with the toolbar,
    ' and assigns images to each tool button.
    Private Sub ToolBarSetup()

        ' Create the ImageList
        Dim imList As New ImageList
        imList.ImageSize = New Size(18, 16)
        imList.ColorDepth = ColorDepth.Depth4Bit
        imList.TransparentColor = Color.FromArgb(192, 192, 192)

        ' adds the bitmap
        imList.Images.AddStrip(New Bitmap( _
         [Assembly].GetExecutingAssembly().GetManifestResourceStream( _
         Me.GetType(), "buttons.bmp")))

        ToolBar1.ImageList = imList
        ToolBarButtonFileOpen.ImageIndex = 1
        ToolBarButtonLayerControl.ImageIndex = 30

        ' Select tools
        ToolBarButtonSelect.ImageIndex = 10

        ' Map tools
        ToolBarButtonZoomIn.ImageIndex = 15
        ToolBarButtonZoomOut.ImageIndex = 16
        ToolBarButtonPan.ImageIndex = 18

        ' disable all tool buttons if there are no layers to work with
        If MapControl1.Map.Empty Then EnableToolButtons(False)

        ' Make sure the initial menu choice matches the initial tool
        CheckToolButton(MapControl1.Tools.LeftButtonTool)
    End Sub

    ' This method handles the toolbar's button click event (for all buttons on the bar).
    Private Sub toolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick
        If Object.ReferenceEquals(e.Button, ToolBarButtonFileOpen) Then
            DisplayLoadMapWizard()
        ElseIf Object.ReferenceEquals(e.Button, ToolBarButtonLayerControl) Then
            DisplayLayerControl()
        Else
            ' MapTool name stored in button tag.
            MapControl1.Tools.LeftButtonTool = e.Button.Tag
        End If

        ' Choice of tool may theoretically be cancelled by event code,
        '  so now set the toolbar to match the current LeftButton tool
        CheckToolButton(MapControl1.Tools.LeftButtonTool)

    End Sub

    ' Display the MapInfo.Windows.Dialogs.LoadMapWizard to
    ' load map files selected by the user.
    Private Sub DisplayLoadMapWizard()
        Dim loadMapWizard As New LoadMapWizard
        loadMapWizard.ShowDbms = True
        loadMapWizard.Run(Me, MapControl1.Map)
    End Sub

    ' Display the layer control dialog
    Private Sub DisplayLayerControl()
        ' only create a new LayerControlDialog _once_:
        '  in this case the dialog will be initialized on first use (if any)
        If _layerDlg Is Nothing Then _layerDlg = New LayerControlDlg
        _layerDlg.Map = MapControl1.Map
        _layerDlg.LayerControl.Tools = MapControl1.Tools
        _layerDlg.ShowDialog(Me)
    End Sub

    ' Helper function to enable/disable all the tool buttons on the tool bar
    ' Note that buttons that are always enabled should have blank Tag properties
    Private Sub EnableToolButtons(ByVal enable As Boolean)
        Dim tbb As ToolBarButton
        For Each tbb In ToolBar1.Buttons
            If tbb.Tag <> "" Then tbb.Enabled = enable
        Next
    End Sub

    ' Helper function to check one tool button and uncheck all the rest
    ' Note that buttons that are always enabled should have blank Tag properties
    Private Sub CheckToolButton(ByVal toolName As String)
        Dim tbb As ToolBarButton
        For Each tbb In ToolBar1.Buttons
            If tbb.Tag <> "" Then tbb.Pushed = (tbb.Tag = toolName)
        Next
    End Sub

    ' Event code to make sure that the tool buttons are enabled
    '  only if the map contains at least one layer.
    Private Sub Layers_CountChanged(ByVal sender As System.Object, ByVal e As MapInfo.Engine.CollectionEventArgs)
        EnableToolButtons(MapControl1.Map.Layers.Count > 0)
        'TODO:  any reason to worry about tool clicks on blank maps?
        'mapControl1.Enabled = (mapControl1.Map.Layers.Count > 0)
    End Sub

    ' Event code to update the status bar when the map's view changes
    Private Sub Map_ViewChanged(ByVal sender As System.Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
        Dim dblZoom As Double = Val(String.Format("{0:E2}", MapControl1.Map.Zoom.Value))
        StatusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + MapControl1.Map.Zoom.Unit.ToString()
    End Sub

    ' *****************************************
    ' *  standard map-handling code ends here *
    ' *****************************************
#End Region

#Region "Load, click, and change events for form controls"
    Private Sub MapForm1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtAddress1.Text = "1 Global View"
        txtCity1.Text = "Troy"
        txtState1.Text = "NY"
        txtZip1.Text = ""
        txtAddress2.Text = "100 Delaware Ave"
        txtCity2.Text = "Albany"
        txtState2.Text = "NY"
        txtZip2.Text = ""
    End Sub

    Private Sub btnClearAddresses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAddresses.Click
        txtAddress1.Text = ""
        txtCity1.Text = ""
        txtState1.Text = ""
        txtZip1.Text = ""
        txtAddress2.Text = ""
        txtCity2.Text = ""
        txtState2.Text = ""
        txtZip2.Text = ""
    End Sub

    Private Sub btnClearMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearMap.Click
        Dim tbl As MapInfo.Data.Table
        tbl = Session.Current.Catalog.GetTable("RouteLayer")
        tbl.Close()
    End Sub

    Private Sub radioUseMiAware_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioUseMiAware.CheckedChanged
        txtMiAwareUrl.Enabled = radioUseMiAware.Checked
        lblMiAwareUrl.Enabled = radioUseMiAware.Checked
        txtRjUrl.Enabled = radioUseJServers.Checked
        lblRjUrl.Enabled = radioUseJServers.Checked
        txtMmjUrl.Enabled = radioUseJServers.Checked
        lblMmjUrl.Enabled = radioUseJServers.Checked
        UpdateMmjUrl()
        UpdateRjUrl()
        'Note: don't need to listen for radioUseJServers changing
        ' [because a change in one radio button always implies a change in the other]
    End Sub

    Private Sub txtMiAwareUrl_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMiAwareUrl.TextChanged
        UpdateMmjUrl()
        UpdateRjUrl()
    End Sub

    Private Sub txtMmjUrl_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMmjUrl.TextChanged
        UpdateMmjUrl()
    End Sub

    Private Sub txtRjUrl_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRjUrl.TextChanged
        UpdateRjUrl()
    End Sub

    Private Sub UpdateMmjUrl()
        If InStr(txtMmjUrl.Text, "/") Then
            strMmjUrl = txtMmjUrl.Text
        ElseIf radioUseJServers.Checked Then
            ' if there's no detail in the URL, try with standard port settings
            strMmjUrl = "http://" & txtMmjUrl.Text & ":8095/mapmarker20/servlet/mapmarker"
        Else
            ' use miAware-style URL with default port and path settings
            strMmjUrl = "http://" & txtMiAwareUrl.Text & ":8080/LocationUtility/services/LocationUtility"
        End If
    End Sub

    Sub UpdateRjUrl()
        If InStr(txtRjUrl.Text, "/") Then
            strRjUrl = txtRjUrl.Text
        ElseIf radioUseJServers.Checked Then
            ' if there's no detail in the URL, try with standard port and path settings
            strRjUrl = "http://" & txtRjUrl.Text & ":8090/routing30/servlet/routing"
        Else
            ' use mi-Aware-style URL with default port and path settings
            strRjUrl = "http://" & txtMiAwareUrl.Text & ":8080/Route/services/Route"
        End If
    End Sub
#End Region

    Private Sub btnRouting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRouting.Click

        ' clear the Results textbox
        txtResults.Text = ""

        Dim addrList(1) As Address

        ' set up the address
        addrList = BuildAddresses()

        ' set up and send the geocode request
        Dim GeocodeResp As GeocodeResponse
        GeocodeResp = SendGeocodeRequest(addrList)
        If GeocodeResp Is Nothing Then Exit Sub

        ' process the geocode result
        If ProcessGeocodeResponse(GeocodeResp) = False Then Exit Sub

        ' set up and send the routing request
        Dim RouteResp As RouteResponse

        ' send route request
        RouteResp = SendRouteRequest(GeocodeResp)
        If RouteResp Is Nothing Then Exit Sub

        ' process the route result
        ProcessRouteResponse(RouteResp)
    End Sub

    Private Function BuildAddresses() As Address()
        Dim addrList() As Address = { _
            addrlistBuild(txtAddress1.Text, txtCity1.Text, txtState1.Text, txtZip1.Text), _
            addrlistBuild(txtAddress2.Text, txtCity2.Text, txtState2.Text, txtZip2.Text)}
        BuildAddresses = addrList
    End Function

    ' create the multi-level Address object based on an address, city, state and ZIP
    Private Function addrlistBuild(ByVal strAddr As String, ByVal strCity As String, _
            ByVal strState As String, ByVal strZIP As String) As Address
        Dim addr As New Address( _
            New StreetAddress( _
                New Street( _
                    "", _
                    "", _
                    strAddr, _
                    "", _
                    "")), _
            "USA")
        addr.PrimaryPostalCode = strZIP
        addr.PlaceList = New Place() { _
            New Place( _
                strCity, _
                NamedPlaceClassification.Municipality), _
            New Place( _
            strState, _
            NamedPlaceClassification.CountrySubdivision)}
        addrlistBuild = addr
    End Function

    ' *******************************************
    ' **               Geocoding               **
    ' *******************************************
    Private Function SendGeocodeRequest(ByVal addrList As Address()) As GeocodeResponse
        LogProgress( _
            "'Find Route' button clicked.", _
            "Setting up call to geocoding client...")

        Dim GeocodeReq As New GeocodeRequest(addrList)
        Dim GeocodeClient As Object ' may be IGeocodeClient or miAwareGeocodeClient
        If radioUseJServers.Checked Then
            GeocodeClient = GeocodeClientFactory.GetMmjHttpClient(strMmjUrl)
        Else
            GeocodeClient = GeocodeClientFactory.GetMiAwareGeocodeClient(strMmjUrl, "demo", "demo")
        End If
        Call LogProgress( _
            "GeocodeRequest object created.", _
            "Geocoding addresses with " & strMmjUrl & "...")
        Application.DoEvents()

        Dim GeocodeResp As GeocodeResponse
        Try
            GeocodeResp = GeocodeClient.Geocode(GeocodeReq)
        Catch ex As Exception
            Call LogProgress( _
                "Geocoding call failed with error:", _
                ex.ToString())
            Exit Function
        End Try
        SendGeocodeRequest = GeocodeResp
    End Function

    Private Function ProcessGeocodeResponse(ByVal GeocodeResp As GeocodeResponse) As Boolean

        If GeocodeResp.Length < 2 Then
            ' list of lists did not have the required 2 elements
            Call LogProgress( _
                "An address failed to geocode successfully.", _
                "No candidates were returned.")
            ProcessGeocodeResponse = False
            Exit Function
        End If
        If GeocodeResp.Item(0).Length = 0 Then
            ' candidate list for first address had 0 elements
            Call LogProgress( _
                "The starting address failed to geocode successfully.", _
                "No candidates were returned.")
            ProcessGeocodeResponse = False
            Exit Function
        End If
        If GeocodeResp.Item(1).Length = 0 Then
            ' candidate list for second address had 0 elements
            Call LogProgress( _
                "The destination address failed to geocode successfully.", _
                "No candidates were returned.")
            ProcessGeocodeResponse = False
            Exit Function
        End If
        ' iterate through candidates
        Dim ac As AddressCandidates, ca As CandidateAddress
        Dim candidate As String, i, j As Integer
        i = 1 : j = 1
        For Each ac In GeocodeResp
            For Each ca In ac
                Try
                    candidate = "Address#" & i.ToString() _
                     & " Candidate#" & j.ToString() _
                     & ": " & ca.Address.StreetAddress.Building.Number _
                     & " " & ca.Address.StreetAddress.Street.DirectionalPrefix _
                     & " " & ca.Address.StreetAddress.Street.TypePrefix _
                     & " " & ca.Address.StreetAddress.Street.OfficialName _
                     & " " & ca.Address.StreetAddress.Street.TypeSuffix _
                     & " " & ca.Address.StreetAddress.Street.DirectionalSuffix _
                     & " " & ca.Address.PlaceList(1).Name _
                     & ", " & ca.Address.PlaceList(0).Name _
                     & " " & ca.Address.PrimaryPostalCode _
                     & "-" & ca.Address.SecondaryPostalCode
                    LogProgress(candidate.Replace("  ", " ").Replace(" ,", ","), "")
                Catch ex As Exception
                    LogProgress("Exception returned while processing candidate list:", ex.ToString())
                End Try
                j = j + 1
            Next
            LogProgress("----------------------------------------------------------------", "")
            i = i + 1
            j = 1
        Next

        ' *******************************************
        ' **      Draw geocoded points on map      **
        ' *******************************************

        Call LogProgress( _
            "Geocoding call returned a valid set of points.", _
            "Creating map objects...")

        Dim tbl As MapInfo.Data.Table, ti As TableInfoMemTable
        Dim key1, key2 As MapInfo.Data.Key

        tbl = Session.Current.Catalog.GetTable("RouteLayer")

        If tbl Is Nothing Then
            ti = New TableInfoMemTable("RouteLayer")
            ti.Temporary = True
            'add object column
            Dim col As MapInfo.Data.Column
            col = New MapInfo.Data.GeometryColumn(MapControl1.Map.GetDisplayCoordSys())
            col.Alias = "obj"
            col.DataType = MIDbType.FeatureGeometry
            ti.Columns.Add(col)
            'add style column
            col = New MapInfo.Data.Column
            col.Alias = "MI_Style"
            col.DataType = MIDbType.Style
            ti.Columns.Add(col)

            tbl = Session.Current.Catalog.CreateTable(ti)
        End If

        Dim fl As FeatureLayer
        fl = New FeatureLayer(tbl)
        MapControl1.Map.Layers.Add(fl)

        Dim ftr As Feature
        ftr = New Feature( _
            GeocodeResp.Item(0).Item(0).Point, _
            New MapInfo.Styles.SimpleVectorPointStyle)
        key1 = tbl.InsertFeature(ftr)
        ftr = New Feature( _
                GeocodeResp.Item(1).Item(0).Point, _
                New MapInfo.Styles.SimpleVectorPointStyle)
        key2 = tbl.InsertFeature(ftr)

        MapControl1.Map.SetView(CType(MapControl1.Map.Layers.Item(0), FeatureLayer))
        MapControl1.Map.SetView( _
            MapControl1.Map.Center, _
            MapControl1.Map.GetDisplayCoordSys(), _
            New Distance( _
                MapControl1.Map.Zoom.Value * 1.05, _
                MapControl1.Map.Zoom.Unit))
        ProcessGeocodeResponse = True
    End Function

    ' *******************************************
    ' **                Routing                **
    ' *******************************************
    Private Function SendRouteRequest(ByVal GeocodeResp As GeocodeResponse) As RouteResponse
        LogProgress( _
             "Creation of point objects completed.", _
             "Setting up call to routing client...")

        Dim wpl As WayPointList = New WayPointList( _
            GeocodeResp.Item(0).Item(0).Point.Centroid, _
            GeocodeResp.Item(1).Item(0).Point.Centroid) ' first candidate for each address
        Dim rp As RoutePlan = New RoutePlan(wpl)
        Dim RouteReq As RouteRequest = New RouteRequest( _
            rp, _
            DistanceUnit.Mile)
        Dim RoutingClient As Object ' may be IRouteClient or miAwareRouteClient
        If radioUseJServers.Checked Then
            RoutingClient = RouteClientFactory.GetRjsHttpClient(strRjUrl)
        Else
            RoutingClient = RouteClientFactory.GetMiAwareRouteClient(strRjUrl, "demo", "demo")
        End If
        Dim rgr As RouteGeometryRequest = New RouteGeometryRequest
        rgr.ReturnGeometry = True
        Dim rir As RouteInstructionsRequest = New RouteInstructionsRequest
        rir.ReturnDirections = True
        RouteReq.RouteGeometryRequest = rgr
        RouteReq.RouteInstructionsRequest = rir

        Call LogProgress( _
            "RouteRequest object created.", _
            "Getting route from " & strRjUrl & "...")
        Application.DoEvents()

        Dim RouteResp As RouteResponse
        Try
            RouteResp = RoutingClient.Route(RouteReq)
        Catch ex As Exception
            Call LogProgress( _
                "Routing call failed with error:", _
                ex.ToString())
            Exit Function
        End Try
        SendRouteRequest = RouteResp
    End Function

    ' *******************************************
    ' **       Draw route object on map        **
    ' *******************************************
    Private Sub ProcessRouteResponse(ByVal RouteResp As RouteResponse)
        Call LogProgress( _
                "Call to routing client returned successfully.", _
                "Creating map objects...")

        Dim RouteCSys As CoordSys = RouteResp.RouteGeometry.CoordSys
        Dim RouteCurve As Curve = New Curve( _
            RouteCSys, _
            RouteResp.RouteGeometry)
        Dim RouteMultiCurve As MultiCurve = New MultiCurve(RouteCSys, RouteCurve)
        Dim ftr As New Feature( _
            RouteMultiCurve, _
            New MapInfo.Styles.SimpleLineStyle( _
                New MapInfo.Styles.LineWidth( _
                    2, _
                    MapInfo.Styles.LineWidthUnit.Pixel), _
                    65, _
                    Color.Yellow, _
                    True))
        Dim tbl As MapInfo.Data.Table
        tbl = Session.Current.Catalog.GetTable("RouteLayer")

        Dim keyRoute As MapInfo.Data.Key
        keyRoute = tbl.InsertFeature(ftr)
        MapControl1.Map.SetView(ftr)
        Dim dist As New Distance( _
            MapControl1.Map.Zoom.Value * 1.05, _
            MapControl1.Map.Zoom.Unit)
        MapControl1.Map.SetView( _
            MapControl1.Map.Center, _
            MapControl1.Map.GetDisplayCoordSys(), _
            dist)

        Call LogProgress( _
            " ", _
            "Route Directions:  [scroll down if necessary]")

        Dim s, sRoute As String, iTextPos As Integer
        iTextPos = txtResults.SelectionStart
        sRoute = ""

        For Each s In RouteResp.RouteInstructionList
            If Trim(s) <> "" Then sRoute = sRoute & vbCrLf & s
        Next
        txtResults.AppendText(sRoute)
        txtResults.SelectionStart = iTextPos
        txtResults.ScrollToCaret()
    End Sub

    ' the LogProgress subroutine is used by the various Routing and Geocoding
    '  functions as a standard way to update the Results textbox
    Private Sub LogProgress(ByVal strMsg1 As String, ByVal strMsg2 As String)
        txtResults.Text += vbCrLf & DateTime.Now & ":  " & strMsg1
        If strMsg2 <> "" Then
            txtResults.AppendText(vbCrLf & "  " & strMsg2)
        End If
    End Sub
End Class
