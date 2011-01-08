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
    Private mapControl1 As MapInfo.Windows.Controls.MapControl
    Private panel1 As System.Windows.Forms.Panel
    Private statusBar1 As System.Windows.Forms.StatusBar

    'members used to create theme
    Private _table As Table
    Private _layer As FeatureLayer
    Private _thmLayer As ObjectThemeLayer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents mapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    Friend WithEvents mapToolBarButtonLayerControl As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents toolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents mapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton

    Friend WithEvents btnStacked As System.Windows.Forms.Button
    Friend WithEvents btnBarTheme As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.mapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.mapToolBarButtonLayerControl = New MapInfo.Windows.Controls.MapToolBarButton
        Me.toolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
        Me.mapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.btnStacked = New System.Windows.Forms.Button
        Me.btnBarTheme = New System.Windows.Forms.Button
        Me.panel1 = New System.Windows.Forms.Panel
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mapToolBarButtonOpenTable
        '
        Me.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
        Me.mapToolBarButtonOpenTable.ToolTipText = "Open Table"
        '
        'mapToolBar1
        '
        Me.mapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.mapToolBarButtonOpenTable, Me.mapToolBarButtonLayerControl, Me.toolBarButtonSeparator, Me.mapToolBarButtonSelect, Me.mapToolBarButtonZoomIn, Me.mapToolBarButtonZoomOut, Me.mapToolBarButtonPan})
        Me.mapToolBar1.Divider = False
        Me.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.mapToolBar1.DropDownArrows = True
        Me.mapToolBar1.Location = New System.Drawing.Point(10, 5)
        Me.mapToolBar1.MapControl = Me.mapControl1
        Me.mapToolBar1.Name = "mapToolBar1"
        Me.mapToolBar1.ShowToolTips = True
        Me.mapToolBar1.Size = New System.Drawing.Size(192, 26)
        Me.mapToolBar1.TabIndex = 12
        '
        'mapToolBarButtonLayerControl
        '
        Me.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl
        Me.mapToolBarButtonLayerControl.ToolTipText = "Layer Control"
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
        'mapToolBarButtonZoomIn
        '
        Me.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn
        Me.mapToolBarButtonZoomIn.ToolTipText = "Zoom-in"
        '
        'mapToolBarButtonZoomOut
        '
        Me.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut
        Me.mapToolBarButtonZoomOut.ToolTipText = "Zoom-out"
        '
        'mapToolBarButtonPan
        '
        Me.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan
        Me.mapToolBarButtonPan.ToolTipText = "Pan"
        '
        'mapControl1
        '
        Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.mapControl1.Location = New System.Drawing.Point(0, 0)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(754, 434)
        Me.mapControl1.TabIndex = 0
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = Nothing
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 482)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(792, 22)
        Me.statusBar1.TabIndex = 9
        '
        'btnStacked
        '
        Me.btnStacked.Location = New System.Drawing.Point(374, 5)
        Me.btnStacked.Name = "btnStacked"
        Me.btnStacked.Size = New System.Drawing.Size(106, 27)
        Me.btnStacked.TabIndex = 11
        Me.btnStacked.Text = "Stacked"
        '
        'btnBarTheme
        '
        Me.btnBarTheme.Location = New System.Drawing.Point(240, 5)
        Me.btnBarTheme.Name = "btnBarTheme"
        Me.btnBarTheme.Size = New System.Drawing.Size(106, 27)
        Me.btnBarTheme.TabIndex = 10
        Me.btnBarTheme.Text = "BarTheme"
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
        Me.panel1.Size = New System.Drawing.Size(758, 438)
        Me.panel1.TabIndex = 8
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(792, 504)
        Me.Controls.Add(Me.mapToolBar1)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.btnStacked)
        Me.Controls.Add(Me.btnBarTheme)
        Me.Controls.Add(Me.panel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadMap()
    End Sub


    Private Sub LoadMap()
        ' Set table search path to value of SampleDataSearchPath registry key
        Dim path As String = Environment.CurrentDirectory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        If (key.GetValue("SampleDataSearchPath") <> Nothing) Then
            path = CType(key.GetValue("SampleDataSearchPath"), String)
            If path.EndsWith("\\") = False Then
                path += "\\"
            End If
            key.Close()
        End If
        Session.Current.TableSearchPath.Path = path

        ' Open a table and put it on map
        _table = Session.Current.Catalog.OpenTable("Mexico.TAB")
        _layer = New FeatureLayer(_table)
        mapControl1.Map.Layers.Add(_layer)
        btnStacked.Enabled = False
    End Sub

    Private Sub btnBarTheme_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarTheme.Click

        Dim thmLayer As ObjectThemeLayer = CType(mapControl1.Map.Layers("VehicleTheme"), ObjectThemeLayer)

        ' remove any existing theme with the name we want to use:
        If Not thmLayer Is Nothing Then
            mapControl1.Map.Layers.Remove(thmLayer)
        End If
        Dim thm As MapInfo.Mapping.Thematics.BarTheme = New MapInfo.Mapping.Thematics.BarTheme(mapControl1.Map, _table, "Cars_91", "Buses_91", "Trucks_91")


        _thmLayer = New ObjectThemeLayer("Vehicles By Type, 1991", "VehicleTheme", thm)
        mapControl1.Map.Layers.Add(_thmLayer)
        btnStacked.Enabled = True
    End Sub

    Private Sub btnStacked_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStacked.Click
        Dim thm As MapInfo.Mapping.Thematics.BarTheme = CType(_thmLayer.Theme, MapInfo.Mapping.Thematics.BarTheme)
        thm.Stacked = True
        thm.GraduatedStacked = True

        _thmLayer.RebuildTheme()
        ' The ObjectThemeLayer will dispose itself when it got removed from Layers collection.
        ' so you can not re-add it into Layers collection.
        ' mapControl1.Map.Layers.Remove("VehicleTheme")
        ' mapControl1.Map.Layers.Add(_thmLayer)
        btnStacked.Enabled = False
    End Sub

    ' Handler function called when the active map's view changes
    Private Sub Map_ViewChanged(ByVal o As Object, ByVal e As ViewChangedEventArgs)
        ' Get the map
        Dim map As MapInfo.Mapping.Map = CType(o, MapInfo.Mapping.Map)
        ' Display the zoom level
        Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
        statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString()
    End Sub

    Private Sub statusBar1_PanelClick(ByVal sender As Object, ByVal e As System.Windows.Forms.StatusBarPanelClickEventArgs)

    End Sub


End Class
