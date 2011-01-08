Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports MapInfo.Data
Imports MapInfo.Mapping
Imports MapInfo.Mapping.Thematics
Imports MapInfo.Mapping.Legends
Imports MapInfo.Engine
Imports MapInfo.Styles
Imports MapInfo.Windows.Dialogs


Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Private legend As MapInfo.Mapping.Legends.Legend = Nothing

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents mapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
    Friend WithEvents toolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents mapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonLayerControl As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents mapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.mapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
        Me.panel1 = New System.Windows.Forms.Panel
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.toolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
        Me.mapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonLayerControl = New MapInfo.Windows.Controls.MapToolBarButton
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.mapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mapToolBarButtonZoomIn
        '
        Me.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn
        Me.mapToolBarButtonZoomIn.ToolTipText = "Zoom-in"
        '
        'mapToolBarButtonPan
        '
        Me.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan
        Me.mapToolBarButtonPan.ToolTipText = "Pan"
        '
        'panel1
        '
        Me.panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panel1.Controls.Add(Me.mapControl1)
        Me.panel1.Location = New System.Drawing.Point(5, 42)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(704, 410)
        Me.panel1.TabIndex = 9
        '
        'mapControl1
        '
        Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapControl1.Location = New System.Drawing.Point(0, 0)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(700, 406)
        Me.mapControl1.TabIndex = 0
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = Nothing
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'toolBarButtonSeparator
        '
        Me.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'mapToolBarButtonSelect
        '
        Me.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select
        Me.mapToolBarButtonSelect.ToolTipText = "Select"
        '
        'mapToolBarButtonLayerControl
        '
        Me.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl
        Me.mapToolBarButtonLayerControl.ToolTipText = "Layer Control"
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 482)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(792, 22)
        Me.statusBar1.TabIndex = 10
        '
        'mapToolBarButtonOpenTable
        '
        Me.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
        Me.mapToolBarButtonOpenTable.ToolTipText = "Open Table"
        '
        'mapToolBarButtonZoomOut
        '
        Me.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut
        Me.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out"
        '
        'mapToolBar1
        '
        Me.mapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.mapToolBarButtonOpenTable, Me.mapToolBarButtonLayerControl, Me.toolBarButtonSeparator, Me.mapToolBarButtonSelect, Me.mapToolBarButtonZoomIn, Me.mapToolBarButtonZoomOut, Me.mapToolBarButtonPan})
        Me.mapToolBar1.Divider = False
        Me.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.mapToolBar1.DropDownArrows = True
        Me.mapToolBar1.Location = New System.Drawing.Point(7, 5)
        Me.mapToolBar1.MapControl = Me.mapControl1
        Me.mapToolBar1.Name = "mapToolBar1"
        Me.mapToolBar1.ShowToolTips = True
        Me.mapToolBar1.Size = New System.Drawing.Size(192, 26)
        Me.mapToolBar1.TabIndex = 11
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(792, 504)
        Me.Controls.Add(Me.panel1)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.mapToolBar1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set table search path to value sampledatasearch registry key
        Dim s As String = Environment.CurrentDirectory
        Dim keySamp As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        If (keySamp.GetValue("SampleDataSearchPath") <> Nothing) Then
            s = CType(keySamp.GetValue("SampleDataSearchPath"), String)
            If s.EndsWith("\\") = False Then
                s += "\\"
            End If
            keySamp.Close()
        End If
        Session.Current.TableSearchPath.Path = s

        ' Add the USA table to the map
        mapControl1.Map.Load(New MapTableLoader("usa.tab"))

        ' Listen to some map events
        'mapControl1.Map.ViewChangedEvent += New ViewChangedEventHandler(Map_ViewChanged)
        'mapControl1.Resize += New EventHandler(mapControl1_Resize)

        ' Create a ranged theme on the USA layer.
        Dim map As Map = mapControl1.Map
        Dim lyr As FeatureLayer = map.Layers("usa")
        Dim thm As MapInfo.Mapping.Thematics.RangedTheme = New MapInfo.Mapping.Thematics.RangedTheme(lyr, _
        "Round(MI_Area(Obj, 'sq mi', 'Spherical'), 1)", "Area (square miles)", _
        5, MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange)
        lyr.Modifiers.Append(thm)

        ' Change the default fill colors from Red->Gray to White->Blue
        Dim ars As AreaStyle
        ' Get the style from our first bin
        Dim cs As CompositeStyle = thm.Bins(0).Style
        ' Get the region -- Area -- style
        ars = cs.AreaStyle
        ' Change the fill color
        ars.Interior = StockStyles.WhiteFillStyle()
        ' Update the CompositeStyle with the new region color
        cs.AreaStyle = ars
        ' Update the bin with the new CompositeStyle settings
        thm.Bins(0).Style = cs

        ' Change the style settings on the last bin
        Dim nLastBin As Integer = thm.Bins.Count - 1
        cs = thm.Bins(nLastBin).Style
        ars = cs.AreaStyle
        ars.Interior = StockStyles.BlueFillStyle()
        thm.Bins(nLastBin).Style = cs

        ' Tell the theme how to fill in the other bins
        thm.SpreadBy = SpreadByPart.Color
        thm.ColorSpreadBy = ColorSpreadMethod.Rgb
        thm.RecomputeStyles()

        ' Create a legend
        legend = map.Legends.CreateLegend(New Size(5, 5))
        legend.Border = True
        Dim frame As ThemeLegendFrame = LegendFrameFactory.CreateThemeLegendFrame("Area", "Area", thm)
        legend.Frames.Append(frame)
        frame.Title = "Area (sq. mi.)"
        map.Adornments.Append(legend)
        ' Set the initial legend location to be the lower right corner of the map control.
        Dim pt As System.Drawing.Point = New System.Drawing.Point(0, 0)
        pt.X = mapControl1.Size.Width - legend.Size.Width
        pt.Y = mapControl1.Size.Height - legend.Size.Height
        legend.Location = pt

    End Sub

    Private Sub mapControl1_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim control As Control = CType(sender, Control)

        ' Move the Legend to the lower right corner...
        Dim pt As System.Drawing.Point = New System.Drawing.Point(0, 0)
        pt.X = control.Size.Width - legend.Size.Width
        pt.Y = control.Size.Height - legend.Size.Height
        legend.Location = pt
    End Sub

    ' Handler function called when the active map's view changes
    Private Sub Map_ViewChanged(ByVal o As Object, ByVal e As ViewChangedEventArgs)
        ' Get the map
        Dim map As MapInfo.Mapping.Map = CType(o, MapInfo.Mapping.Map)
        ' Display the zoom level
        Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
        statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString()
    End Sub

End Class
