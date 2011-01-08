Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports MapInfo.Data
Imports MapInfo.Engine
Imports MapInfo.Mapping
Imports MapInfo.Mapping.Thematics
Imports MapInfo.Tools
Imports MapInfo.Windows
Imports MapInfo.Windows.Dialogs
Imports MapInfo.Windows.Controls



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
    Friend WithEvents mapToolBarButtonLayerControl As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents toolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents mapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    Friend WithEvents mapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
    Friend WithEvents mainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuOpenTable As System.Windows.Forms.MenuItem
    Friend WithEvents mnuOpenGeoset As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCloseTable As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCloseAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuTheme As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAddTheme As System.Windows.Forms.MenuItem
    Friend WithEvents mnuRemoveTheme As System.Windows.Forms.MenuItem
    Friend WithEvents mnuModifyTheme As System.Windows.Forms.MenuItem
    Friend WithEvents statusBar1 As System.Windows.Forms.StatusBar



    ' the theme created
    Private _thm As ITheme

    'keep track of the layer themed for use when modifiying or removing
    Private _themedFeatureLayer As MapInfo.Mapping.FeatureLayer = Nothing
    Private _themedLabelLayer As MapInfo.Mapping.LabelLayer = Nothing
    Friend WithEvents Label1 As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.mapToolBarButtonLayerControl = New MapInfo.Windows.Controls.MapToolBarButton
        Me.toolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
        Me.mapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.mapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
        Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuOpenTable = New System.Windows.Forms.MenuItem
        Me.mnuOpenGeoset = New System.Windows.Forms.MenuItem
        Me.mnuCloseTable = New System.Windows.Forms.MenuItem
        Me.mnuCloseAll = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuTheme = New System.Windows.Forms.MenuItem
        Me.mnuAddTheme = New System.Windows.Forms.MenuItem
        Me.mnuRemoveTheme = New System.Windows.Forms.MenuItem
        Me.mnuModifyTheme = New System.Windows.Forms.MenuItem
        Me.statusBar1 = New System.Windows.Forms.StatusBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        'mapToolBar1
        '
        Me.mapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.mapToolBarButtonOpenTable, Me.mapToolBarButtonLayerControl, Me.toolBarButtonSeparator, Me.mapToolBarButtonSelect, Me.mapToolBarButtonZoomIn, Me.mapToolBarButtonZoomOut, Me.mapToolBarButtonPan})
        Me.mapToolBar1.Divider = False
        Me.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.mapToolBar1.DropDownArrows = True
        Me.mapToolBar1.Location = New System.Drawing.Point(10, 0)
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
        Me.mapControl1.Location = New System.Drawing.Point(10, 37)
        Me.mapControl1.Name = "mapControl1"
        Me.mapControl1.Size = New System.Drawing.Size(518, 286)
        Me.mapControl1.TabIndex = 10
        Me.mapControl1.Text = "mapControl1"
        Me.mapControl1.Tools.LeftButtonTool = Nothing
        Me.mapControl1.Tools.MiddleButtonTool = Nothing
        Me.mapControl1.Tools.RightButtonTool = Nothing
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuTheme})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpenTable, Me.mnuOpenGeoset, Me.mnuCloseTable, Me.mnuCloseAll, Me.mnuExit})
        Me.mnuFile.Text = "File"
        '
        'mnuOpenTable
        '
        Me.mnuOpenTable.Index = 0
        Me.mnuOpenTable.Text = "Open Table..."
        '
        'mnuOpenGeoset
        '
        Me.mnuOpenGeoset.Index = 1
        Me.mnuOpenGeoset.Text = "Open Geoset..."
        '
        'mnuCloseTable
        '
        Me.mnuCloseTable.Index = 2
        Me.mnuCloseTable.Text = "Close Table..."
        '
        'mnuCloseAll
        '
        Me.mnuCloseAll.Index = 3
        Me.mnuCloseAll.Text = "Close All"
        '
        'mnuExit
        '
        Me.mnuExit.Index = 4
        Me.mnuExit.Text = "Exit"
        '
        'mnuTheme
        '
        Me.mnuTheme.Index = 1
        Me.mnuTheme.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddTheme, Me.mnuRemoveTheme, Me.mnuModifyTheme})
        Me.mnuTheme.Text = "Theme"
        '
        'mnuAddTheme
        '
        Me.mnuAddTheme.Index = 0
        Me.mnuAddTheme.Text = "Add Theme..."
        '
        'mnuRemoveTheme
        '
        Me.mnuRemoveTheme.Enabled = False
        Me.mnuRemoveTheme.Index = 1
        Me.mnuRemoveTheme.Text = "Remove Theme"
        '
        'mnuModifyTheme
        '
        Me.mnuModifyTheme.Enabled = False
        Me.mnuModifyTheme.Index = 2
        Me.mnuModifyTheme.Text = "Modify Theme..."
        '
        'statusBar1
        '
        Me.statusBar1.Location = New System.Drawing.Point(0, 328)
        Me.statusBar1.Name = "statusBar1"
        Me.statusBar1.Size = New System.Drawing.Size(536, 22)
        Me.statusBar1.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(200, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(304, 16)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "ThemeDialog Sample - VB.net"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(536, 350)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.mapControl1)
        Me.Controls.Add(Me.statusBar1)
        Me.Controls.Add(Me.mapToolBar1)
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region




    Private Sub Map_ViewChanged(ByVal o As Object, ByVal e As ViewChangedEventArgs)
        ' Get the map
        Dim map As MapInfo.Mapping.Map = CType(o, MapInfo.Mapping.Map)
        ' Display the zoom level
        Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
        statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        SetupTablePath()
        Setup()
    End Sub

    Private Sub SetupTablePath()
        ' Set table search path to value sampledatasearch registry key
        ' if not found, then just use the app's current directory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        Dim s As String = CType(key.GetValue("SampleDataSearchPath"), String)
        If s <> Nothing And s.Length > 0 Then
            If s.EndsWith("\\") = False Then
                s += "\\"
            End If
        Else
            s = Environment.CurrentDirectory
        End If
        key.Close()

        Session.Current.TableSearchPath.Path = s
    End Sub
    Private Sub Setup()
        ' Load the layer
        Try
            mapControl1.Map.Load(New MapTableLoader("mexico.tab"))
            Dim layer As MapInfo.Mapping.FeatureLayer = mapControl1.Map.Layers("mexico")
            Dim LabelLayer As MapInfo.Mapping.LabelLayer = New MapInfo.Mapping.LabelLayer("Label Layer", "Label Layer")
            LabelLayer.Sources.Append(New MapInfo.Mapping.LabelSource(layer.Table))
            mapControl1.Map.Layers.Add(LabelLayer)
        Catch
            mnuCloseTable.Enabled = False
            mnuCloseAll.Enabled = False
            mnuTheme.Enabled = False
        End Try
    End Sub

 

    Private Sub mnuRemoveTheme_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveTheme.Click

        ' Remove the theme

        RemoveTheme()

        ' Update the controls
        'mnuAddTheme.Enabled = true;
        'mnuRemoveTheme.Enabled = false;
        'mnuModifyTheme.Enabled = false;					

        ' if me was the last map layer, reset the close menus
        If mapControl1.Map.Layers.Count = 0 Then
            mnuCloseTable.Enabled = False
            mnuCloseAll.Enabled = False
            mnuTheme.Enabled = False
        End If
    End Sub

    Private Sub RemoveTheme()
        Dim buttons As MessageBoxButtons = MessageBoxButtons.YesNo
        Dim result As DialogResult

        If Not _themedFeatureLayer Is Nothing Then
            Dim modifier As FeatureStyleModifier = _themedFeatureLayer.Modifiers("theme1")

            If Not modifier Is Nothing Then
                result = System.Windows.Forms.MessageBox.Show(Me, "Removing Theme : " + modifier.Name, "ThemeDialogSampleApp", buttons)
                If result = DialogResult.Yes Then

                    _themedFeatureLayer.Modifiers.Remove(modifier)
                    mapControl1.Map.Invalidate()
                End If
            End If
        End If
        Dim thmLayer As MapInfo.Mapping.ObjectThemeLayer = mapControl1.Map.Layers("theme1")
        If Not thmLayer Is Nothing Then
            result = System.Windows.Forms.MessageBox.Show(Me, "Removing Theme : " + thmLayer.Name, "ThemeDialogSampleApp", buttons)
            If result = DialogResult.Yes Then
                mapControl1.Map.Layers.Remove(thmLayer)
                mapControl1.Map.Invalidate()
            End If
        End If
        If Not _themedLabelLayer Is Nothing Then
            Dim labelModifier As MapInfo.Mapping.LabelModifier = _themedLabelLayer.Sources(0).Modifiers("theme1")
            If Not labelModifier Is Nothing Then
                result = System.Windows.Forms.MessageBox.Show(Me, "Removing Theme : " + labelModifier.Name, "ThemeDialogSampleApp", buttons)
                If result = DialogResult.Yes Then
                    _themedLabelLayer.Sources(0).Modifiers.Remove(labelModifier)
                    mapControl1.Map.Invalidate()
                End If
            End If
        End If

    End Sub

    Private Sub mnuModifyTheme_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuModifyTheme.Click
        ' Modify the theme
        ModifyTheme()
    End Sub

    Private Sub ModifyTheme()

        ' Bring up the modify theme dialog for modifier themes	
        If Not _themedFeatureLayer Is Nothing Then

            Dim modifier As FeatureStyleModifier = _themedFeatureLayer.Modifiers("theme1")
            If Not modifier Is Nothing Then
                If TypeOf modifier Is MapInfo.Mapping.Thematics.DotDensityTheme Then
                    Dim thm As MapInfo.Mapping.Thematics.DotDensityTheme = modifier
                    Dim dlg As ModifyDotDensityThemeDlg = New ModifyDotDensityThemeDlg(mapControl1.Map, thm)
                    dlg.ShowDialog()
                ElseIf TypeOf modifier Is MapInfo.Mapping.Thematics.RangedTheme Then
                    Dim thm As MapInfo.Mapping.Thematics.RangedTheme = modifier
                    Dim dlg As ModifyRangedThemeDlg = New ModifyRangedThemeDlg(mapControl1.Map, thm)
                    dlg.ShowDialog()
                ElseIf TypeOf modifier Is MapInfo.Mapping.Thematics.IndividualValueTheme Then
                    Dim thm As MapInfo.Mapping.Thematics.IndividualValueTheme = modifier
                    Dim dlg As ModifyIndValueThemeDlg = New ModifyIndValueThemeDlg(mapControl1.Map, thm)
                    dlg.ShowDialog()
                End If
                _themedFeatureLayer.Invalidate()
            End If
        End If
        ' Bring up the modify theme dialog for object themes
        Dim thmLayer As ObjectThemeLayer = mapControl1.Map.Layers("theme1")
        If Not thmLayer Is Nothing Then
            If TypeOf thmLayer.Theme Is GraduatedSymbolTheme Then
                Dim thm As GraduatedSymbolTheme = thmLayer.Theme
                Dim dlg As ModifyGradSymbolThemeDlg = New ModifyGradSymbolThemeDlg(mapControl1.Map, thm)
                dlg.ShowDialog()
            ElseIf TypeOf thmLayer.Theme Is MapInfo.Mapping.Thematics.BarTheme Then
                Dim thm As MapInfo.Mapping.Thematics.BarTheme = thmLayer.Theme
                Dim dlg As ModifyBarThemeDlg = New ModifyBarThemeDlg(mapControl1.Map, thm)
                dlg.ShowDialog()
            ElseIf TypeOf thmLayer.Theme Is MapInfo.Mapping.Thematics.PieTheme Then
                Dim thm As MapInfo.Mapping.Thematics.PieTheme = thmLayer.Theme
                Dim dlg As ModifyPieThemeDlg = New ModifyPieThemeDlg(mapControl1.Map, thm)
                dlg.ShowDialog()
            End If
            thmLayer.RebuildTheme()
        End If
        If Not _themedLabelLayer Is Nothing Then
            Dim labelModifier As MapInfo.Mapping.LabelModifier = _themedLabelLayer.Sources(0).Modifiers("theme1")
            If Not labelModifier Is Nothing Then
                If TypeOf labelModifier Is MapInfo.Mapping.Thematics.RangedLabelTheme Then
                    Dim thm As RangedLabelTheme = labelModifier
                    Dim dlg As ModifyRangedThemeDlg = New ModifyRangedThemeDlg(mapControl1.Map, thm)
                    dlg.ShowDialog()
                ElseIf TypeOf labelModifier Is IndividualValueLabelTheme Then
                    Dim thm As MapInfo.Mapping.Thematics.IndividualValueLabelTheme = labelModifier
                    Dim dlg As ModifyIndValueThemeDlg = New ModifyIndValueThemeDlg(mapControl1.Map, thm)
                    dlg.ShowDialog()
                End If
                _themedLabelLayer.Invalidate()
            End If
        End If

    End Sub

    Private Sub mnuOpenGeoset_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenGeoset.Click
        CloseAll()
        Dim openFileDlg As OpenFileDialog = New OpenFileDialog
        openFileDlg.InitialDirectory = Session.Current.TableSearchPath.Path
        openFileDlg.Filter = "Geoset (*.gst)|*.gst"
        openFileDlg.FilterIndex = 1

        If openFileDlg.ShowDialog() = DialogResult.OK Then
            mapControl1.Map.Load(New MapGeosetLoader(openFileDlg.FileName))

            mnuCloseTable.Enabled = True
            mnuCloseAll.Enabled = True
            mnuTheme.Enabled = True
            mnuAddTheme.Enabled = True
        End If
    End Sub
    Private Sub CloseAll()
        mapControl1.Map.Layers.Clear()

    End Sub
    Private Sub mnuOpenTable_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenTable.Click

        CloseAll()

        Dim openFileDlg As OpenFileDialog = New OpenFileDialog
        openFileDlg.InitialDirectory = Session.Current.TableSearchPath.Path
        openFileDlg.Filter = "MapInfo Table (*.tab)|*.tab"
        openFileDlg.FilterIndex = 1

        If openFileDlg.ShowDialog() = DialogResult.OK Then
            mapControl1.Map.Load(New MapTableLoader(openFileDlg.FileName))

            mnuCloseTable.Enabled = True
            mnuCloseAll.Enabled = True
            mnuTheme.Enabled = True
            mnuAddTheme.Enabled = True
        End If
    End Sub

    Private Sub mnuCloseTable_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseTable.Click

        Dim catalog As Catalog = Session.Current.Catalog
        Dim tables As ITableEnumerator = catalog.EnumerateTables()
        Dim tablesList As ArrayList = New ArrayList
        While tables.MoveNext()
            tablesList.Add(tables.Current)
        End While
        Dim closeTableDlg As CloseTableDlg = New CloseTableDlg
        closeTableDlg.Items.AddRange(tablesList)
        closeTableDlg.SelectedIndex = 0
        If closeTableDlg.ShowDialog() = DialogResult.OK Then
            Dim selectedTables As ArrayList = closeTableDlg.SelectedItems
            Dim tableToClose As IEnumerator = selectedTables.GetEnumerator()
            While tableToClose.MoveNext()
                catalog.CloseTable((CType(tableToClose.Current, MapInfo.Data.Table)).Alias)
            End While

            ' if all the tables were closed, reset the menus
            If tablesList.Count = selectedTables.Count Then

                ' remove the layers from the map so that theme layers get removed
                RemoveAllMapLayers()

                mnuCloseTable.Enabled = False
                mnuCloseAll.Enabled = False
                mnuTheme.Enabled = False
                mnuAddTheme.Enabled = False
                mnuRemoveTheme.Enabled = False
                mnuModifyTheme.Enabled = False
            End If
        End If
    End Sub

    ' Removes all layers from the map
    Private Sub RemoveAllMapLayers()
        Dim enumer As MapLayerEnumerator = mapControl1.Map.Layers.GetMapLayerEnumerator()
        Dim enumer1 As MapLayerEnumerator = mapControl1.Map.Layers.GetMapLayerEnumerator()
        While enumer1.MoveNext()
            While enumer1.MoveNext()
                mapControl1.Map.Layers.Remove(enumer1.Current)
            End While
            While enumer1.MoveNext()
                mapControl1.Map.Layers.Remove(enumer1.Current)
            End While
        End While

    End Sub

    Private Sub mnuCloseAll_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseAll.Click
        ' remove the layers from the map so that theme layers get removed
        RemoveAllMapLayers()

        Session.Current.Catalog.CloseAll()

        mnuCloseTable.Enabled = False
        mnuCloseAll.Enabled = False
        mnuTheme.Enabled = False
        mnuAddTheme.Enabled = False
        mnuRemoveTheme.Enabled = False
        mnuModifyTheme.Enabled = False
    End Sub

    Private Sub mnuExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        Application.Exit()
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        mapControl1.Height = Me.Height
        mapControl1.Width = Me.Width
    End Sub




    Private Sub mnuAddTheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddTheme.Click
        Try
            Dim createTheme As CreateThemeWizard = New CreateThemeWizard(mapControl1.Map, Me)
            _thm = createTheme.CreateTheme("theme1")
            Dim themedLayer As MapLayer = createTheme.SelectedLayer
            If TypeOf themedLayer Is FeatureLayer Then
                _themedFeatureLayer = CType(themedLayer, FeatureLayer)
            Else
                _themedLabelLayer = CType(themedLayer, MapInfo.Mapping.LabelLayer)
            End If
            If createTheme.WizardResult = WizardStepResult.Done Then
                ' Update the controls
                'mnuAddTheme.Enabled = false;
                mnuRemoveTheme.Enabled = True
                mnuModifyTheme.Enabled = True
            End If
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Error creating theme: " + ex.Message)
        End Try
    End Sub



    Private Sub Form1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
