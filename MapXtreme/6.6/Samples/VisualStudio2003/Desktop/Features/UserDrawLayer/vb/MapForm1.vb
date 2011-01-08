Imports System.Reflection
Imports System.IO
Imports MapInfo.Data
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Windows.Dialogs

Public Class MapForm1
    Inherits System.Windows.Forms.Form

	Friend WithEvents Map As MapInfo.Mapping.Map
	Friend WithEvents Layers As MapInfo.Mapping.Layers

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Listen to some map events
				Map = MapControl1.Map
				Layers = MapControl1.Map.Layers
				AddHandler Map.ViewChangedEvent, Addressof Map_ViewChanged
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
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents ToolBarButtonLayerControl As System.Windows.Forms.ToolBarButton
    Friend WithEvents MapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents ToolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents MapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.MapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.MapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.MapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
        Me.ToolBarButtonLayerControl = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
        Me.MapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
        Me.MapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
        Me.MapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
        Me.MapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.MapControl1)
        Me.Panel1.Location = New System.Drawing.Point(4, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(398, 249)
        Me.Panel1.TabIndex = 0
        '
        'MapControl1
        '
        Me.MapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MapControl1.Location = New System.Drawing.Point(0, 0)
        Me.MapControl1.Name = "MapControl1"
        Me.MapControl1.Size = New System.Drawing.Size(394, 245)
        Me.MapControl1.TabIndex = 0
        Me.MapControl1.Text = "MapControl1"
        Me.MapControl1.Tools.MiddleButtonTool = Nothing
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 284)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(406, 22)
        Me.StatusBar1.TabIndex = 2
        '
        'MapToolBar1
        '
        Me.MapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.MapToolBarButtonOpenTable, Me.ToolBarButtonLayerControl, Me.ToolBarButtonSeparator, Me.MapToolBarButtonSelect, Me.MapToolBarButtonZoomIn, Me.MapToolBarButtonZoomOut, Me.MapToolBarButtonPan})
        Me.MapToolBar1.Divider = False
        Me.MapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.MapToolBar1.DropDownArrows = True
        Me.MapToolBar1.Location = New System.Drawing.Point(6, 8)
        Me.MapToolBar1.MapControl = Me.MapControl1
        Me.MapToolBar1.Name = "MapToolBar1"
        Me.MapToolBar1.ShowToolTips = True
        Me.MapToolBar1.Size = New System.Drawing.Size(200, 26)
        Me.MapToolBar1.TabIndex = 7
        '
        'MapToolBarButtonOpenTable
        '
        Me.MapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
        Me.MapToolBarButtonOpenTable.ToolTipText = "Open Table"
        '
        'ToolBarButtonLayerControl
        '
        Me.ToolBarButtonLayerControl.ImageIndex = 10
        Me.ToolBarButtonLayerControl.Tag = ""
        Me.ToolBarButtonLayerControl.ToolTipText = "Layer Control"
        '
        'ToolBarButtonSeparator
        '
        Me.ToolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'MapToolBarButtonSelect
        '
        Me.MapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select
        Me.MapToolBarButtonSelect.ToolTipText = "Select"
        '
        'MapToolBarButtonZoomIn
        '
        Me.MapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn
        Me.MapToolBarButtonZoomIn.ToolTipText = "Zoom-in"
        '
        'MapToolBarButtonZoomOut
        '
        Me.MapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut
        Me.MapToolBarButtonZoomOut.ToolTipText = "Zoom-out"
        '
        'MapToolBarButtonPan
        '
        Me.MapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan
        Me.MapToolBarButtonPan.ToolTipText = "Pan"
        '
        'MapForm1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(406, 306)
        Me.Controls.Add(Me.MapToolBar1)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.Panel1)
        Me.MinimumSize = New System.Drawing.Size(250, 200)
        Me.Name = "MapForm1"
        Me.Text = "MapForm1"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub MapForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set table search path to value sampledatasearch registry key
        ' if not found, then just use the app's current directory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        Dim s As String = CType(key.GetValue("SampleDataSearchPath"), String)
        If s <> Nothing Then
            If s.EndsWith("\") = False Then
                s += "\"
            End If
        Else
            s = Environment.CurrentDirectory
        End If
        key.Close()

        Session.Current.TableSearchPath.Path = s

        ' open up world.gst using a maploader
        Try
            Dim gl As New MapGeosetLoader(s + "world.gst")
            MapControl1.Map.Load(gl)
        Catch fnf As FileNotFoundException
            MessageBox.Show("File not found " + fnf.Message)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        ' now create and add our user draw layer to the map
        Dim ud = New UserDrawHighlighter("Highlighted Cities", "userdraw")
        MapControl1.Map.Layers.Insert(0, ud)
    End Sub


    ' Display layer control dialog
    ' We first need to add our custom properties tab into the Layer Control
    ' first the any tabs for standard user draw layers
    ' then add them into a new list along with our custom page
    Private Sub DisplayLayerControl()
        Dim dlg As LayerControlDlg = New LayerControlDlg
        dlg.Map = MapControl1.Map
        Dim stockUserDrawList As IList = dlg.LayerControl.GetLayerTypeControls(GetType(UserDrawLayer))
        Dim controlList As New ArrayList
        For Each c As Object In stockUserDrawList
            controlList.Add(c)
        Next
        Dim control As New UserDrawHighlighterProperties
        controlList.Add(control)
        dlg.LayerControl.SetLayerTypeControls(GetType(UserDrawHighlighter), controlList)
        dlg.ShowDialog(Me)
    End Sub


    ' Handler function called when the active map's view changes
    Private Sub Map_ViewChanged(ByVal sender As System.Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
        Dim dblZoom As Double = Val(String.Format("{0:E2}", MapControl1.Map.Zoom.Value))
        StatusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + MapControl1.Map.Zoom.Unit.ToString()
    End Sub


    ' This method handles the toolbar's button click event
    '  for any buttons that need custom functionality
    Private Sub MapToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles MapToolBar1.ButtonClick
        If Object.ReferenceEquals(e.Button, ToolBarButtonLayerControl) Then
            DisplayLayerControl()
        End If
    End Sub
End Class