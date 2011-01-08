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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents mapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents toolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents mapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonLayerControl As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    Friend WithEvents mapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar
    Friend WithEvents panel1 As System.Windows.Forms.Panel
    Friend WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.mapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
        Me.toolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
        Me.mapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonLayerControl = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.mapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.panel1 = New System.Windows.Forms.Panel
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mapToolBarButtonSelect
        '
        Me.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select
        Me.mapToolBarButtonSelect.ToolTipText = "Select"
        '
        'toolBarButtonSeparator
        '
        Me.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
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
        'mapToolBarButtonZoomOut
        '
        Me.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut
        Me.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out"
        '
        'mapToolBarButtonLayerControl
        '
        Me.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl
        Me.mapToolBarButtonLayerControl.ToolTipText = "Layer Control"
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
        'mapToolBarButtonOpenTable
        '
        Me.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
        Me.mapToolBarButtonOpenTable.ToolTipText = "Open Table"
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 442)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(744, 22)
        Me.statusBar1.TabIndex = 10
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
        Me.panel1.Size = New System.Drawing.Size(720, 397)
        Me.panel1.TabIndex = 9
        '
        'mapControl1
        '
        Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapControl1.Location = New System.Drawing.Point(0, 0)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(716, 393)
        Me.mapControl1.TabIndex = 0
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = Nothing
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(744, 464)
        Me.Controls.Add(Me.mapToolBar1)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.panel1)
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
        If keySamp.GetValue("SampleDataSearchPath") <> Nothing Then
            s = CType(keySamp.GetValue("SampleDataSearchPath"), String)
            If s.EndsWith("\\") = False Then
                s += "\\"
            End If
            keySamp.Close()
        End If
        Session.Current.TableSearchPath.Path = s

        ' Add the USA table to the map
        mapControl1.Map.Load(New MapTableLoader("usa.tab"))


        ' Create a ranged theme on the USA layer. 
        Dim map As Map = mapControl1.Map
        Dim lyr As FeatureLayer = map.Layers("usa")
        Dim THM As MapInfo.Mapping.Thematics.RangedTheme = New MapInfo.Mapping.Thematics.RangedTheme(lyr, "Round(MI_Area(Obj, 'sq mi', 'Spherical'), 1)", "Area (square miles)", 5, MapInfo.Mapping.Thematics.DistributionMethod.EqualCountPerRange)
        lyr.Modifiers.Append(THM)

        ' Create a legend
        Dim legend As MapInfo.Mapping.Legends.Legend = map.Legends.CreateLegend(New Size(5, 5))
        ' Create a LegendFrame that contains the theme and add that frame to the Legend.
        Dim frame As ThemeLegendFrame = LegendFrameFactory.CreateThemeLegendFrame("Area", "Area", THM)
        legend.Frames.Append(frame)
        frame.Title = "Area (sq. mi.)"

        ' Create a LegendExport and export the legend to a bitmap file.
        Dim legendExport As MapInfo.Mapping.LegendExport = New MapInfo.Mapping.LegendExport(map, legend)
        legendExport.Format = ExportFormat.Bmp
        legendExport.ExportSize = New ExportSize(300, 200)
        legendExport.Export("legend.bmp")

        ' Display the legend in a window.
        Dim legendForm As System.Windows.Forms.Form = New LegendForm
        legendForm.BackgroundImage = System.Drawing.Image.FromFile("legend.bmp")
        legendForm.Size = New Size(300, 200)
        legendForm.Show()

        ' Alternatively, you can add the legend as a child window of the map
        '  by appending it to the Adornments collection.  In this case, most likely
        '  a smaller size should be used when the Legend object is created.
        '
        'legend.Border = true;
        'map.Adornments.Append(legend);

        ' Set the initial legend location to be the upper left corner of the map control.
        'legend.Location = new System.Drawing.Point(mapControl1.Left, mapControl1.Top);

    End Sub
End Class
