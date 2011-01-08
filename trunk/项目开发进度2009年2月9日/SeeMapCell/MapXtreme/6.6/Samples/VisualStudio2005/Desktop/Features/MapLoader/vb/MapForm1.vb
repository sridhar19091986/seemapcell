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
  Private dlgMapLoaderOptions As New MapLoaderOptions
#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Listen to some map events
    Map = MapControl1.Map
    Layers = MapControl1.Map.Layers
    AddHandler Map.ViewChangedEvent, AddressOf Map_ViewChanged

    AddHandler Session.Current.Catalog.TableOpenedEvent, AddressOf TableOpened
    AddHandler Session.Current.Catalog.TableIsClosingEvent, AddressOf TableIsClosing

        ' Assign the Pan tool to the middle mouse button
    MapControl1.Tools.MiddleButtonTool = "Pan"
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            MapInfo.Engine.Session.Dispose()
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents menuFile As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemFileCloseTables As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemFileOpen As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemFileClearMap As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemFileTableSearchPath As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemSep2 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemFileExit As System.Windows.Forms.MenuItem
    Friend WithEvents menuTableLoader As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderOptions As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemSep3 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderOneFile As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderMultipleFiles As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderFileArray As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderPickFiles As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemSep4 As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderMultipleTables As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderTableEnumerator As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemTableLoaderTableInfo As System.Windows.Forms.MenuItem
    Friend WithEvents menuGeosetLoader As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemGeosetLoaderOptions As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemGeosetLoaderLoadGeoset As System.Windows.Forms.MenuItem
    Friend WithEvents menuWorkspaceLoader As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemWorkspaceLoaderOptions As System.Windows.Forms.MenuItem
    Friend WithEvents menuItemWorkspaceLoaderLoadWorkspace As System.Windows.Forms.MenuItem
    Friend WithEvents MapToolBar1 As MapInfo.Windows.Controls.MapToolBar
    Friend WithEvents MapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents ToolBarButtonSeparator As System.Windows.Forms.ToolBarButton
    Friend WithEvents MapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
    Friend WithEvents MapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.MapControl1 = New MapInfo.Windows.Controls.MapControl
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.menuFile = New System.Windows.Forms.MenuItem
        Me.menuItemFileOpen = New System.Windows.Forms.MenuItem
        Me.menuItemFileCloseTables = New System.Windows.Forms.MenuItem
        Me.menuItemSep1 = New System.Windows.Forms.MenuItem
        Me.menuItemFileClearMap = New System.Windows.Forms.MenuItem
        Me.menuItemFileTableSearchPath = New System.Windows.Forms.MenuItem
        Me.menuItemSep2 = New System.Windows.Forms.MenuItem
        Me.menuItemFileExit = New System.Windows.Forms.MenuItem
        Me.menuTableLoader = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderOptions = New System.Windows.Forms.MenuItem
        Me.menuItemSep3 = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderOneFile = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderMultipleFiles = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderFileArray = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderPickFiles = New System.Windows.Forms.MenuItem
        Me.menuItemSep4 = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderMultipleTables = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderTableEnumerator = New System.Windows.Forms.MenuItem
        Me.menuItemTableLoaderTableInfo = New System.Windows.Forms.MenuItem
        Me.menuGeosetLoader = New System.Windows.Forms.MenuItem
        Me.menuItemGeosetLoaderOptions = New System.Windows.Forms.MenuItem
        Me.menuItemGeosetLoaderLoadGeoset = New System.Windows.Forms.MenuItem
        Me.menuWorkspaceLoader = New System.Windows.Forms.MenuItem
        Me.menuItemWorkspaceLoaderOptions = New System.Windows.Forms.MenuItem
        Me.menuItemWorkspaceLoaderLoadWorkspace = New System.Windows.Forms.MenuItem
        Me.MapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
        Me.MapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
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
        Me.Panel1.Location = New System.Drawing.Point(4, 32)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(384, 207)
        Me.Panel1.TabIndex = 0
        '
        'MapControl1
        '
        Me.MapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MapControl1.Location = New System.Drawing.Point(0, 0)
        Me.MapControl1.Name = "MapControl1"
        Me.MapControl1.Size = New System.Drawing.Size(380, 203)
        Me.MapControl1.TabIndex = 0
        Me.MapControl1.Text = "MapControl1"
        Me.MapControl1.Tools.LeftButtonTool = Nothing
        Me.MapControl1.Tools.MiddleButtonTool = Nothing
        Me.MapControl1.Tools.RightButtonTool = Nothing
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 243)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Size = New System.Drawing.Size(392, 22)
        Me.StatusBar1.TabIndex = 2
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFile, Me.menuTableLoader, Me.menuGeosetLoader, Me.menuWorkspaceLoader})
        '
        'menuFile
        '
        Me.menuFile.Index = 0
        Me.menuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemFileOpen, Me.menuItemFileCloseTables, Me.menuItemSep1, Me.menuItemFileClearMap, Me.menuItemFileTableSearchPath, Me.menuItemSep2, Me.menuItemFileExit})
        Me.menuFile.Text = "File"
        '
        'menuItemFileOpen
        '
        Me.menuItemFileOpen.Index = 0
        Me.menuItemFileOpen.Text = "&Open..."
        '
        'menuItemFileCloseTables
        '
        Me.menuItemFileCloseTables.Enabled = False
        Me.menuItemFileCloseTables.Index = 1
        Me.menuItemFileCloseTables.Text = "Close &Tables..."
        '
        'menuItemSep1
        '
        Me.menuItemSep1.Index = 2
        Me.menuItemSep1.Text = "-"
        '
        'menuItemFileClearMap
        '
        Me.menuItemFileClearMap.Index = 3
        Me.menuItemFileClearMap.Text = "&Clear Map"
        '
        'menuItemFileTableSearchPath
        '
        Me.menuItemFileTableSearchPath.Index = 4
        Me.menuItemFileTableSearchPath.Text = "Table Search Path..."
        '
        'menuItemSep2
        '
        Me.menuItemSep2.Index = 5
        Me.menuItemSep2.Text = "-"
        '
        'menuItemFileExit
        '
        Me.menuItemFileExit.Index = 6
        Me.menuItemFileExit.Text = "E&xit"
        '
        'menuTableLoader
        '
        Me.menuTableLoader.Index = 1
        Me.menuTableLoader.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemTableLoaderOptions, Me.menuItemSep3, Me.menuItemTableLoaderOneFile, Me.menuItemTableLoaderMultipleFiles, Me.menuItemTableLoaderFileArray, Me.menuItemTableLoaderPickFiles, Me.menuItemSep4, Me.menuItemTableLoaderMultipleTables, Me.menuItemTableLoaderTableEnumerator, Me.menuItemTableLoaderTableInfo})
        Me.menuTableLoader.Text = "&Table Loader"
        '
        'menuItemTableLoaderOptions
        '
        Me.menuItemTableLoaderOptions.Index = 0
        Me.menuItemTableLoaderOptions.Text = "&Options..."
        '
        'menuItemSep3
        '
        Me.menuItemSep3.Index = 1
        Me.menuItemSep3.Text = "-"
        '
        'menuItemTableLoaderOneFile
        '
        Me.menuItemTableLoaderOneFile.Index = 2
        Me.menuItemTableLoaderOneFile.Text = "One File"
        '
        'menuItemTableLoaderMultipleFiles
        '
        Me.menuItemTableLoaderMultipleFiles.Index = 3
        Me.menuItemTableLoaderMultipleFiles.Text = "Multiple Files"
        '
        'menuItemTableLoaderFileArray
        '
        Me.menuItemTableLoaderFileArray.Index = 4
        Me.menuItemTableLoaderFileArray.Text = "File Array"
        '
        'menuItemTableLoaderPickFiles
        '
        Me.menuItemTableLoaderPickFiles.Index = 5
        Me.menuItemTableLoaderPickFiles.Text = "Pick Files..."
        '
        'menuItemSep4
        '
        Me.menuItemSep4.Index = 6
        Me.menuItemSep4.Text = "-"
        '
        'menuItemTableLoaderMultipleTables
        '
        Me.menuItemTableLoaderMultipleTables.Index = 7
        Me.menuItemTableLoaderMultipleTables.Text = "Multiple Tables"
        '
        'menuItemTableLoaderTableEnumerator
        '
        Me.menuItemTableLoaderTableEnumerator.Index = 8
        Me.menuItemTableLoaderTableEnumerator.Text = "Table Enumerator"
        '
        'menuItemTableLoaderTableInfo
        '
        Me.menuItemTableLoaderTableInfo.Index = 9
        Me.menuItemTableLoaderTableInfo.Text = "TableInfo"
        '
        'menuGeosetLoader
        '
        Me.menuGeosetLoader.Index = 2
        Me.menuGeosetLoader.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemGeosetLoaderOptions, Me.menuItemGeosetLoaderLoadGeoset})
        Me.menuGeosetLoader.Text = "&Geoset Loader"
        '
        'menuItemGeosetLoaderOptions
        '
        Me.menuItemGeosetLoaderOptions.Index = 0
        Me.menuItemGeosetLoaderOptions.Text = "&Options..."
        '
        'menuItemGeosetLoaderLoadGeoset
        '
        Me.menuItemGeosetLoaderLoadGeoset.Index = 1
        Me.menuItemGeosetLoaderLoadGeoset.Text = "Load Geoset..."
        '
        'menuWorkspaceLoader
        '
        Me.menuWorkspaceLoader.Index = 3
        Me.menuWorkspaceLoader.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemWorkspaceLoaderOptions, Me.menuItemWorkspaceLoaderLoadWorkspace})
        Me.menuWorkspaceLoader.Text = "&Workspace Loader"
        '
        'menuItemWorkspaceLoaderOptions
        '
        Me.menuItemWorkspaceLoaderOptions.Index = 0
        Me.menuItemWorkspaceLoaderOptions.Text = "&Options..."
        '
        'menuItemWorkspaceLoaderLoadWorkspace
        '
        Me.menuItemWorkspaceLoaderLoadWorkspace.Index = 1
        Me.menuItemWorkspaceLoaderLoadWorkspace.Text = "Load Workspace..."
        '
        'MapToolBar1
        '
        Me.MapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.MapToolBarButtonOpenTable, Me.ToolBarButtonSeparator, Me.MapToolBarButtonSelect, Me.MapToolBarButtonZoomIn, Me.MapToolBarButtonZoomOut, Me.MapToolBarButtonPan})
        Me.MapToolBar1.ButtonSize = New System.Drawing.Size(25, 22)
        Me.MapToolBar1.Divider = False
        Me.MapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.MapToolBar1.DropDownArrows = True
        Me.MapToolBar1.Location = New System.Drawing.Point(8, 0)
        Me.MapToolBar1.MapControl = Me.MapControl1
        Me.MapToolBar1.Name = "MapToolBar1"
        Me.MapToolBar1.ShowToolTips = True
        Me.MapToolBar1.Size = New System.Drawing.Size(200, 26)
        Me.MapToolBar1.TabIndex = 4
        '
        'MapToolBarButtonOpenTable
        '
        Me.MapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
        Me.MapToolBarButtonOpenTable.ToolTipText = "Open Table"
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
        Me.ClientSize = New System.Drawing.Size(392, 265)
        Me.Controls.Add(Me.MapToolBar1)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.Panel1)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(250, 200)
        Me.Name = "MapForm1"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TableOpened(ByVal sender As System.Object, ByVal e As TableOpenedEventArgs)
        Me.menuItemFileCloseTables.Enabled = Session.Current.Catalog.Count > 0
    End Sub

    Private Sub TableIsClosing(ByVal sender As System.Object, ByVal e As TableIsClosingEventArgs)
        Me.menuItemFileCloseTables.Enabled = Session.Current.Catalog.Count > 1
    End Sub

    ' Handler function called when the active map's view changes
    Private Sub Map_ViewChanged(ByVal sender As System.Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
        Dim dblZoom As Double = Val(String.Format("{0:E2}", MapControl1.Map.Zoom.Value))
        StatusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + MapControl1.Map.Zoom.Unit.ToString()
    End Sub

    Private Sub menuFileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemFileOpen.Click
        Dim LoadMapWizard1 As New LoadMapWizard
        LoadMapWizard1.ShowDbms = True
        LoadMapWizard1.Run(Me, MapControl1.Map)
    End Sub

    Private Sub menuItemFileCloseTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemFileCloseTables.Click

        Dim dlg As SelectTablesDlg = New SelectTablesDlg(Session.Current.Catalog.EnumerateTables())
        dlg.Text = "Close Tables"
        If dlg.ShowDialog() = DialogResult.OK And Not dlg.SelectedTables Is Nothing Then
            Dim t As MapInfo.Data.Table
            For Each t In dlg.SelectedTables
                t.Close()
            Next
            MapControl1.Map.Invalidate()
        End If
        dlg.Dispose()
    End Sub

    Private Sub menuClearMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemFileClearMap.Click
        MapControl1.Map.Clear()
    End Sub

    Private Sub menuTableSearchPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemFileTableSearchPath.Click

        Dim dlg As TableSearchPathDlg = New TableSearchPathDlg
        dlg.Path = Session.Current.TableSearchPath.Path
        If dlg.ShowDialog(Me) = DialogResult.OK Then
            Session.Current.TableSearchPath.Path = dlg.Path
            Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
            key.SetValue("SampleDataSearchPath", Session.Current.TableSearchPath.Path)
            key.Close()
        End If
        dlg.Dispose()
    End Sub

    Private Sub menuFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemFileExit.Click
        Application.Exit()
    End Sub

    Private Sub MapForm1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set table search path to value sampledatasearch registry key
        ' if not found, then just use the app's current directory
        Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6")
        Dim s As String = CType(key.GetValue("SampleDataSearchPath"), String)
        If s <> Nothing AndAlso s.Length > 0 Then
            If s.EndsWith("\") = False Then
                s += "\"
            End If
        Else
            s = Environment.CurrentDirectory
        End If
        key.Close()


        Session.Current.TableSearchPath.Path = s
    End Sub

    Private Sub menuTableLoaderOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderOptions.Click
        dlgMapLoaderOptions.ShowDialog()
    End Sub

    Private Sub LoadTables(ByVal tl As MapInfo.Mapping.MapLoader)

        Try
            ' Set table loader options
            tl.AutoPosition = dlgMapLoaderOptions.AutoPosition
            tl.StartPosition = dlgMapLoaderOptions.StartPosition
            tl.EnableLayers = IIf(dlgMapLoaderOptions.EnableLayers, EnableLayers.Enable, EnableLayers.Disable)


            If dlgMapLoaderOptions.ClearMapFirst Then

                MapControl1.Map.Clear()
            End If
            MapControl1.Map.Load(tl)
        Catch ex As Exception

            Dim s As String = ex.Message + vbCrLf + "Make sure the TableSearchPath '" + Session.Current.TableSearchPath.Path + "' is set to point to the location where the sample data is installed."
            MessageBox.Show(s)
        End Try

    End Sub
    Private Sub menuTableLoaderOneFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderOneFile.Click
        Dim tl As MapTableLoader = New MapTableLoader("world.tab")
        LoadTables(tl)
    End Sub

    Private Sub menuTableLoaderMultipleFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderMultipleFiles.Click
        Dim tl As MapTableLoader = New MapTableLoader("ocean.tab", "usa.tab", "mexico.tab", "us_hiway.tab")
        LoadTables(tl)
    End Sub

    Private Sub menuTableLoaderFileArray_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderFileArray.Click
        Dim tables() As String = New String(3) {}
        tables(0) = "us_cnty.tab"
        tables(1) = "usa_caps.tab"
        tables(2) = "uscty_1k.tab"

        Dim tl As MapTableLoader = New MapTableLoader(tables)
        LoadTables(tl)
    End Sub

    Private Sub menuItemPickFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderPickFiles.Click
        Dim OpenFileDialog1 As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        OpenFileDialog1.Multiselect = True
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.DefaultExt = "TAB"
        OpenFileDialog1.Filter = "MapInfo Tables (*.tab)|*.tab||"


        Try ' set initial dir to first path on table searchpath
            OpenFileDialog1.InitialDirectory = Session.Current.TableSearchPath.Path.Split(";")(0)
        Catch
        End Try

        If OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim tl As MapTableLoader = New MapTableLoader
            Dim filename As String
            For Each filename In OpenFileDialog1.FileNames
                tl.Add(filename)
            Next
            LoadTables(tl)
        End If

    End Sub

    Private Sub menuTableLoaderMultipleTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderMultipleTables.Click

        Dim path As String
        Session.Current.TableSearchPath.FileExists("world.tab", path)
        Dim t1 As MapInfo.Data.Table = Session.Current.Catalog.OpenTable(path)
        Session.Current.TableSearchPath.FileExists("usa.tab", path)
        Dim t2 As MapInfo.Data.Table = Session.Current.Catalog.OpenTable(path)
        Session.Current.TableSearchPath.FileExists("mexico.tab", path)
        Dim t3 As MapInfo.Data.Table = Session.Current.Catalog.OpenTable(path)
        Dim tl As MapTableLoader = New MapTableLoader(t1, t2, t3)
        LoadTables(tl)
    End Sub

    Private Sub menuTableLoaderTableEnumerator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderTableEnumerator.Click

        Dim tl As MapTableLoader = New MapTableLoader(Session.Current.Catalog.EnumerateTables())
        LoadTables(tl)

    End Sub

    Private Sub menuTableLoaderTableInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableLoaderTableInfo.Click

        Dim path As String
        Session.Current.TableSearchPath.FileExists("world.tab", path)
        Dim ti As TableInfo = TableInfo.CreateFromFile(path)
        Dim tl As MapTableLoader = New MapTableLoader(ti)
        LoadTables(tl)
    End Sub

    Private Sub menuGeosetLoaderOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemGeosetLoaderOptions.Click
        menuTableLoaderOptions_Click(sender, e)
    End Sub

    Private Sub menuGeosetLoaderLoadGeoset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemGeosetLoaderLoadGeoset.Click
        Dim OpenFileDialog1 As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.DefaultExt = "GST"
        OpenFileDialog1.Filter = "Geosets (*.gst)|*.gst||"


        Try ' set initial dir to first path on table searchpath
            OpenFileDialog1.InitialDirectory = Session.Current.TableSearchPath.Path.Split(";")(0)
        Catch
        End Try

        If OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim gl As New MapGeosetLoader(OpenFileDialog1.FileName)
            ' set geoset specific options
            gl.LayersOnly = dlgMapLoaderOptions.LayersOnly
            gl.SetMapName = dlgMapLoaderOptions.SetMapName
            LoadTables(gl)
        End If

    End Sub

    Private Sub menuWorkspaceLoaderOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemWorkspaceLoaderOptions.Click
        menuTableLoaderOptions_Click(sender, e)
    End Sub

    Private Sub menuWorkspaceLoaderLoadWorkspace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemWorkspaceLoaderLoadWorkspace.Click
        Dim OpenFileDialog1 As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.DefaultExt = "MWS"
        OpenFileDialog1.Filter = "MapInfo Workspaces (*.mws)|*.mws||"


        Try ' set initial dir to first path on table searchpath
            OpenFileDialog1.InitialDirectory = Session.Current.TableSearchPath.Path.Split(";")(0)
        Catch
        End Try

        If OpenFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim wl As New MapWorkSpaceLoader(OpenFileDialog1.FileName)
            ' set geoset specific options
            wl.LayersOnly = dlgMapLoaderOptions.LayersOnly
            wl.SetMapName = dlgMapLoaderOptions.SetMapName
            LoadTables(wl)
        End If

    End Sub
End Class
