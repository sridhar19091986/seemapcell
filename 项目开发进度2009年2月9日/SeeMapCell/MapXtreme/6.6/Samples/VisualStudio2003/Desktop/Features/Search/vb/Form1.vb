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
Imports MapInfo.Geometry
Imports MapInfo.Mapping
Imports MapInfo.Styles

Namespace Search
    Public Class Form1
        Inherits System.Windows.Forms.Form
        
#Region " Windows Form Designer generated code "
        Private WithEvents mapControl1 As MapInfo.Windows.Controls.MapControl
        Private WithEvents panel1 As System.Windows.Forms.Panel
        Private WithEvents statusBar1 As System.Windows.Forms.StatusBar
        Private WithEvents mainMenu1 As System.Windows.Forms.MainMenu
        Private WithEvents menuItem1 As System.Windows.Forms.MenuItem
        Private WithEvents menuItem2 As System.Windows.Forms.MenuItem
        Private WithEvents menuItem11 As System.Windows.Forms.MenuItem
        Private WithEvents menuItem19 As System.Windows.Forms.MenuItem
        Private WithEvents menuFileExit As System.Windows.Forms.MenuItem
        Private WithEvents menuItemMapSearchWithinScreenRect As System.Windows.Forms.MenuItem
        Private WithEvents menuItemMapSearchWithinScreenRadius As System.Windows.Forms.MenuItem
        Private WithEvents menuItemMapSearchNearest As System.Windows.Forms.MenuItem
        Private components As System.ComponentModel.Container = Nothing

        Private WithEvents _map As Map = Nothing ' will be set to map from mapcontrol
        Private WithEvents _catalog As Catalog = Session.Current.Catalog
        Private WithEvents _selection As Selection = Session.Current.Selections.DefaultSelection
        Private WithEvents menuItemSearchWhere As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchWithinFeature As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchWithinRect As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchIntersectsFeature As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchWithinDistance As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchNearest As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSqlExpressionFilter As System.Windows.Forms.MenuItem
        Private WithEvents menuItemCustomProcessor As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSearchMultipleTables As System.Windows.Forms.MenuItem
        Private WithEvents menuItem4 As System.Windows.Forms.MenuItem
        Private WithEvents menuItem5 As System.Windows.Forms.MenuItem
        Private WithEvents menuItemLogicalFilter As System.Windows.Forms.MenuItem
        Private WithEvents menuItemCustomQueryFilter As System.Windows.Forms.MenuItem
        Private WithEvents menuItemIntersectFeature As System.Windows.Forms.MenuItem
        Private WithEvents menuItemContainsFilter As System.Windows.Forms.MenuItem
        Private WithEvents menuItemSetColumns As System.Windows.Forms.MenuItem
        Private WithEvents menuItem7 As System.Windows.Forms.MenuItem
        Private WithEvents mapToolBar1 As MapInfo.Windows.Controls.MapToolBar
        Private WithEvents mapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
        Private WithEvents toolBarButtonSeparator As System.Windows.Forms.ToolBarButton
        Private WithEvents mapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
        Private WithEvents mapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
        Private WithEvents mapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
        Private WithEvents mapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
        Private WithEvents _tempTable As Table = Nothing

        Public Sub New()
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            ' Listen to some map events
            AddHandler mapControl1.Map.ViewChangedEvent, AddressOf Map_ViewChanged

            _map = mapControl1.Map

            ' Assign the Pan tool to the middle mouse button
            mapControl1.Tools.MiddleButtonTool = "Pan"
        End Sub 'New

        

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
            Me.panel1 = New System.Windows.Forms.Panel
            Me.statusBar1 = New System.Windows.Forms.StatusBar
            Me.mainMenu1 = New System.Windows.Forms.MainMenu
            Me.menuItem19 = New System.Windows.Forms.MenuItem
            Me.menuFileExit = New System.Windows.Forms.MenuItem
            Me.menuItem1 = New System.Windows.Forms.MenuItem
            Me.menuItemSearchWhere = New System.Windows.Forms.MenuItem
            Me.menuItemSearchWithinFeature = New System.Windows.Forms.MenuItem
            Me.menuItemSearchWithinRect = New System.Windows.Forms.MenuItem
            Me.menuItemSearchIntersectsFeature = New System.Windows.Forms.MenuItem
            Me.menuItemSearchWithinDistance = New System.Windows.Forms.MenuItem
            Me.menuItemSearchNearest = New System.Windows.Forms.MenuItem
            Me.menuItem4 = New System.Windows.Forms.MenuItem
            Me.menuItemSearchMultipleTables = New System.Windows.Forms.MenuItem
            Me.menuItem11 = New System.Windows.Forms.MenuItem
            Me.menuItemCustomProcessor = New System.Windows.Forms.MenuItem
            Me.menuItem2 = New System.Windows.Forms.MenuItem
            Me.menuItemMapSearchWithinScreenRect = New System.Windows.Forms.MenuItem
            Me.menuItemMapSearchWithinScreenRadius = New System.Windows.Forms.MenuItem
            Me.menuItemMapSearchNearest = New System.Windows.Forms.MenuItem
            Me.menuItem5 = New System.Windows.Forms.MenuItem
            Me.menuItemSqlExpressionFilter = New System.Windows.Forms.MenuItem
            Me.menuItemContainsFilter = New System.Windows.Forms.MenuItem
            Me.menuItemIntersectFeature = New System.Windows.Forms.MenuItem
            Me.menuItemCustomQueryFilter = New System.Windows.Forms.MenuItem
            Me.menuItemLogicalFilter = New System.Windows.Forms.MenuItem
            Me.menuItem7 = New System.Windows.Forms.MenuItem
            Me.menuItemSetColumns = New System.Windows.Forms.MenuItem
            Me.mapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
            Me.mapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
            Me.toolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
            Me.mapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
            Me.mapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
            Me.mapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
            Me.mapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
            Me.panel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'mapControl1
            '
            Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mapControl1.Location = New System.Drawing.Point(0, 0)
            Me.mapControl1.Name = "mapControl1"
            Me.mapControl1.Size = New System.Drawing.Size(394, 244)
            Me.mapControl1.TabIndex = 0
            Me.mapControl1.Text = "mapControl1"
            Me.mapControl1.Tools.LeftButtonTool = Nothing
            Me.mapControl1.Tools.MiddleButtonTool = Nothing
            Me.mapControl1.Tools.RightButtonTool = Nothing
            '
            'panel1
            '
            Me.panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.panel1.Controls.Add(Me.mapControl1)
            Me.panel1.Location = New System.Drawing.Point(4, 32)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(398, 248)
            Me.panel1.TabIndex = 1
            '
            'statusBar1
            '
            Me.statusBar1.Location = New System.Drawing.Point(0, 286)
            Me.statusBar1.Name = "statusBar1"
            Me.statusBar1.Size = New System.Drawing.Size(406, 19)
            Me.statusBar1.TabIndex = 2
            '
            'mainMenu1
            '
            Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem19, Me.menuItem1, Me.menuItem2, Me.menuItem5})
            '
            'menuItem19
            '
            Me.menuItem19.Index = 0
            Me.menuItem19.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFileExit})
            Me.menuItem19.Text = "&File"
            '
            'menuFileExit
            '
            Me.menuFileExit.Index = 0
            Me.menuFileExit.Text = "E&xit"
            '
            'menuItem1
            '
            Me.menuItem1.Index = 1
            Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemSearchWhere, Me.menuItemSearchWithinFeature, Me.menuItemSearchWithinRect, Me.menuItemSearchIntersectsFeature, Me.menuItemSearchWithinDistance, Me.menuItemSearchNearest, Me.menuItem4, Me.menuItemSearchMultipleTables, Me.menuItem11, Me.menuItemCustomProcessor})
            Me.menuItem1.Text = "Search"
            '
            'menuItemSearchWhere
            '
            Me.menuItemSearchWhere.Index = 0
            Me.menuItemSearchWhere.Text = "SearchWhere"
            '
            'menuItemSearchWithinFeature
            '
            Me.menuItemSearchWithinFeature.Index = 1
            Me.menuItemSearchWithinFeature.Text = "SearchWithinGeometry"
            '
            'menuItemSearchWithinRect
            '
            Me.menuItemSearchWithinRect.Index = 2
            Me.menuItemSearchWithinRect.Text = "SearchWithinRect"
            '
            'menuItemSearchIntersectsFeature
            '
            Me.menuItemSearchIntersectsFeature.Index = 3
            Me.menuItemSearchIntersectsFeature.Text = "SearchIntersectsFeature"
            '
            'menuItemSearchWithinDistance
            '
            Me.menuItemSearchWithinDistance.Index = 4
            Me.menuItemSearchWithinDistance.Text = "SearchWithinDistance"
            '
            'menuItemSearchNearest
            '
            Me.menuItemSearchNearest.Index = 5
            Me.menuItemSearchNearest.Text = "SearchNearest"
            '
            'menuItem4
            '
            Me.menuItem4.Index = 6
            Me.menuItem4.Text = "-"
            '
            'menuItemSearchMultipleTables
            '
            Me.menuItemSearchMultipleTables.Index = 7
            Me.menuItemSearchMultipleTables.Text = "Search Multiple Tables"
            '
            'menuItem11
            '
            Me.menuItem11.Index = 8
            Me.menuItem11.Text = "-"
            '
            'menuItemCustomProcessor
            '
            Me.menuItemCustomProcessor.Index = 9
            Me.menuItemCustomProcessor.Text = "CustomSearchResultProcessor"
            '
            'menuItem2
            '
            Me.menuItem2.Index = 2
            Me.menuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemMapSearchWithinScreenRect, Me.menuItemMapSearchWithinScreenRadius, Me.menuItemMapSearchNearest})
            Me.menuItem2.Text = "MapSearch"
            '
            'menuItemMapSearchWithinScreenRect
            '
            Me.menuItemMapSearchWithinScreenRect.Index = 0
            Me.menuItemMapSearchWithinScreenRect.Text = "SearchWithinScreenRect"
            '
            'menuItemMapSearchWithinScreenRadius
            '
            Me.menuItemMapSearchWithinScreenRadius.Index = 1
            Me.menuItemMapSearchWithinScreenRadius.Text = "SearchWithinScreenRadius"
            '
            'menuItemMapSearchNearest
            '
            Me.menuItemMapSearchNearest.Index = 2
            Me.menuItemMapSearchNearest.Text = "SearchNearest"
            '
            'menuItem5
            '
            Me.menuItem5.Index = 3
            Me.menuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItemSqlExpressionFilter, Me.menuItemContainsFilter, Me.menuItemIntersectFeature, Me.menuItemCustomQueryFilter, Me.menuItemLogicalFilter, Me.menuItem7, Me.menuItemSetColumns})
            Me.menuItem5.Text = "QueryDefinition"
            '
            'menuItemSqlExpressionFilter
            '
            Me.menuItemSqlExpressionFilter.Index = 0
            Me.menuItemSqlExpressionFilter.Text = "SqlExpressionFilter"
            '
            'menuItemContainsFilter
            '
            Me.menuItemContainsFilter.Index = 1
            Me.menuItemContainsFilter.Text = "ContainsFilter"
            '
            'menuItemIntersectFeature
            '
            Me.menuItemIntersectFeature.Index = 2
            Me.menuItemIntersectFeature.Text = "IntersectFilter"
            '
            'menuItemCustomQueryFilter
            '
            Me.menuItemCustomQueryFilter.Index = 3
            Me.menuItemCustomQueryFilter.Text = "CustomQueryFilter"
            '
            'menuItemLogicalFilter
            '
            Me.menuItemLogicalFilter.Index = 4
            Me.menuItemLogicalFilter.Text = "LogicalFilter"
            '
            'menuItem7
            '
            Me.menuItem7.Index = 5
            Me.menuItem7.Text = "-"
            '
            'menuItemSetColumns
            '
            Me.menuItemSetColumns.Index = 6
            Me.menuItemSetColumns.Text = "Setting Columns"
            '
            'mapToolBar1
            '
            Me.mapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.mapToolBarButtonOpenTable, Me.toolBarButtonSeparator, Me.mapToolBarButtonSelect, Me.mapToolBarButtonZoomIn, Me.mapToolBarButtonZoomOut, Me.mapToolBarButtonPan})
            Me.mapToolBar1.ButtonSize = New System.Drawing.Size(25, 22)
            Me.mapToolBar1.Divider = False
            Me.mapToolBar1.Dock = System.Windows.Forms.DockStyle.None
            Me.mapToolBar1.DropDownArrows = True
            Me.mapToolBar1.Location = New System.Drawing.Point(8, 0)
            Me.mapToolBar1.MapControl = Me.mapControl1
            Me.mapToolBar1.Name = "mapToolBar1"
            Me.mapToolBar1.ShowToolTips = True
            Me.mapToolBar1.Size = New System.Drawing.Size(160, 26)
            Me.mapToolBar1.TabIndex = 3
            '
            'mapToolBarButtonOpenTable
            '
            Me.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
            Me.mapToolBarButtonOpenTable.ToolTipText = "Open Table"
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
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(406, 305)
            Me.Controls.Add(Me.mapToolBar1)
            Me.Controls.Add(Me.statusBar1)
            Me.Controls.Add(Me.panel1)
            Me.Menu = Me.mainMenu1
            Me.MinimumSize = New System.Drawing.Size(250, 200)
            Me.Name = "Form1"
            Me.Text = "Search Sample"
            Me.panel1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub 'InitializeComponent

        '

#End Region
        ' Handler function called when the active map's view changes
        Private Sub Map_ViewChanged(ByVal o As Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
            ' Get the map
            Dim map As MapInfo.Mapping.Map = o '(MapInfo.Mapping.Map)
            ' Display the zoom level
            Dim dblZoom As Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
            statusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + mapControl1.Map.Zoom.Unit.ToString()
        End Sub

        Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Set table search path to value sampledatasearch registry key
            ' if not found, then just use the app's current directory
            Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\MapXtreme\6.6\")
            Dim s As String = key.GetValue("SampleDataSearchPath").ToString
            If Not (s Is Nothing) And (s.Length > 0) Then
                If (s.EndsWith("\") = False) Then
                    s += "\"
                End If
            Else
                s = Environment.CurrentDirectory
            End If
            key.Close()

            MapInfo.Engine.Session.Current.TableSearchPath.Path = s

            ' load some layers, set csys and default view
            Try

                _map.Load(New MapInfo.Mapping.MapTableLoader("usa.tab", "mexico.tab", "usa_caps.tab", "uscty_1k.tab", "grid15.tab"))

            Catch ex As System.Exception

                MessageBox.Show(Me, "Unable to load tables. Sample Data may not be installed.\r\n" + ex.Message)
                Me.Close()
                Exit Sub
            End Try
            _map.SetDisplayCoordSys(DirectCast(_map.Layers("grid15"), MapInfo.Mapping.FeatureLayer).CoordSys)
            Dim lyr As MapInfo.Mapping.FeatureLayer = _map.Layers("uscty_1k")
            _map.SetView(lyr.DefaultBounds, lyr.CoordSys)

            ' create and add temp layer
            Dim ti As TableInfo = TableInfoFactory.CreateTemp("temp") ' create tableinfo with just obj and style cols
            _tempTable = _catalog.CreateTable(ti)
            _map.Layers.Insert(0, New MapInfo.Mapping.FeatureLayer(_tempTable))

        End Sub

      
        Private Sub menuFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuFileExit.Click

            Application.Exit()
        End Sub


        Private Overloads Sub ShowSearchGeometry(ByVal g As FeatureGeometry)
            ShowSearchGeometry(g, True)
        End Sub 'ShowSearchGeometry

        ' add geometry to temp layer with hollow style
        Private Overloads Sub ShowSearchGeometry(ByVal g As MapInfo.Geometry.FeatureGeometry, ByVal clear As Boolean)
            If (clear) Then
                ' first clear out any other geometries from table
                DirectCast(_tempTable, MapInfo.Data.IFeatureCollection).Clear()
            End If

            Dim s As MapInfo.Styles.Style = Nothing
            If TypeOf g Is MapInfo.Geometry.IGenericSurface Then
                s = New MapInfo.Styles.AreaStyle(New MapInfo.Styles.SimpleLineStyle(New LineWidth(2, LineWidthUnit.Pixel), 2, Color.Red, False), New SimpleInterior(0))
            ElseIf TypeOf g Is MapInfo.Geometry.Point Then
                s = New MapInfo.Styles.SimpleVectorPointStyle(34, Color.Red, 18)
            End If

            Dim f As MapInfo.Data.Feature = New Feature(g, s)

            ' Add feature to temp table
            _tempTable.InsertFeature(f)

        End Sub

        ' select features in feature collection
        Private Sub SelectFeatureCollection(ByVal fc As MapInfo.Data.IResultSetFeatureCollection)
            ' force map to update
            mapControl1.Update()

            _selection.Clear()
            _selection.Add(fc)
        End Sub

        ' this is similar to searchwithinrect, but the rect constructed is a screen rectangle
        ' as opposed to a map rectangle (try both and see the difference)
        Private Sub menuItemMapSearchWithinScreenRect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemMapSearchWithinScreenRect.Click
            Try

                Cursor.Current = Cursors.WaitCursor
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                rect.X += rect.Width / 3
                rect.Width = rect.Width / 3
                rect.Y += rect.Height / 3
                rect.Height = rect.Height / 3
                Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRect(_map, rect, ContainsType.Centroid)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)

                ' show search geometry on screen for visual confirmation
                Dim p As MapInfo.Geometry.MultiPolygon = MapInfo.Mapping.SearchInfoFactory.CreateScreenRect(DirectCast(_map.Layers("temp"), MapInfo.Mapping.FeatureLayer), rect)
                ShowSearchGeometry(p)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default

            End Try
        End Sub

        ' find cities with 1/3 radius of center
        Private Sub menuItemMapSearchWithinScreenRadius_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemMapSearchWithinScreenRadius.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                Dim pt As System.Drawing.Point = New System.Drawing.Point(rect.Left, rect.Top)
                Dim pixelRadius As Integer
                pt.X += rect.Width / 2
                pt.Y += rect.Height / 2
                pixelRadius = rect.Width / 6
                Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(_map, pt, pixelRadius, 20, ContainsType.Centroid)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)

                ' show search geometry on screen for visual confirmation
                Dim p As MapInfo.Geometry.MultiPolygon = MapInfo.Mapping.SearchInfoFactory.CreateScreenCircle(DirectCast(_map.Layers("temp"), MapInfo.Mapping.FeatureLayer), pt, pixelRadius, 20)
                ShowSearchGeometry(p)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' find cities nearest to center within 3 pixel radius
        Private Sub menuItemMapSearchNearest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemMapSearchNearest.Click
            Try

                Cursor.Current = Cursors.WaitCursor
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                Dim pt As System.Drawing.Point = New System.Drawing.Point(rect.Left, rect.Top)
                pt.X += rect.Width / 2
                pt.Y += rect.Height / 2


                Dim si As SearchInfo = MapInfo.Mapping.SearchInfoFactory.SearchNearest(_map, pt, 3)
                Dim fc As IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)

                rect.X = pt.X
                rect.Y = pt.Y
                rect.Width = 0
                rect.Height = 0
                rect.Inflate(3, 3)
                ' show search geometry on screen for visual confirmation
                Dim p As MapInfo.Geometry.MultiPolygon = MapInfo.Mapping.SearchInfoFactory.CreateScreenRect(_map, rect)
                ShowSearchGeometry(p)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' find states where pop 1990 < 4 million
        Private Sub menuItemSearchWhere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemSearchWhere.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                ' find and select all states with pop > 2 million
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchWhere("POP_90 > 2000000")
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("mexico", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' find and select cities with Georgia and Florida using geometry
        Private Sub menuItemSearchWithinGeometry_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Cursor.Current = Cursors.WaitCursor
                ' find and select cities with Georgia and Florida using geometry
                ' also uses search for feature
                Dim fFlorida As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"))
                Dim fGeorgia As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='GA'"))
                Dim g As MapInfo.Geometry.FeatureGeometry = fFlorida.Geometry.Combine(fGeorgia.Geometry)
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchWithinGeometry(g, ContainsType.Centroid)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ShowSearchGeometry(g)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' this is similar to searchwithinscreenrect, but the rect constructed is a map rectangle
        ' as opposed to a screen rectangle (try both and see the difference)
        Private Sub menuItemSearchWithinRect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSearchWithinRect.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                rect.X += rect.Width / 3
                rect.Width = rect.Width / 3
                rect.Y += rect.Height / 3
                rect.Height = rect.Height / 3
                Dim mapRect As MapInfo.Geometry.DRect = New MapInfo.Geometry.DRect

                ' use csys and transform of feature layer, because that is the 
                ' layer we are doing the search on
                Dim layer As MapInfo.Mapping.FeatureLayer = _map.Layers("uscty_1k")
                layer.DisplayTransform.FromDisplay(rect, mapRect)
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchWithinRect(mapRect, layer.CoordSys, ContainsType.Centroid)
                Dim fc As IResultSetFeatureCollection = _catalog.Search(layer.Table, si)

                ' show search geometry on screen for visual confirmation
                Dim pts(4) As MapInfo.Geometry.DPoint
                mapRect.GetCornersOfRect(pts)
                Dim g As MapInfo.Geometry.FeatureGeometry = New MapInfo.Geometry.MultiPolygon(layer.CoordSys, CurveSegmentType.Linear, pts)
                ShowSearchGeometry(g)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' find states that intersect KS
        Private Sub menuItemSearchIntersectsFeature_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSearchIntersectsFeature.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                ' find states that intersect KS
                ' also uses search for feature
                Dim fKS As MapInfo.Data.Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='KS'"))
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchIntersectsFeature(fKS, IntersectType.Geometry)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("usa", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ShowSearchGeometry(fKS.Geometry)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub


        ' find cities with distance of center of map
        Private Sub menuItemSearchWithinDistance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSearchWithinDistance.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                ' to compare to SearchWithinScreenRadius, we are calculating
                ' the search distance the same way it does
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                Dim pt As System.Drawing.Point = New System.Drawing.Point(rect.Left, rect.Top)
                pt.X += rect.Width / 2
                pt.Y += rect.Height / 2

                Dim dpt1 As MapInfo.Geometry.DPoint = New MapInfo.Geometry.DPoint
                ' convert center point to map coords (could use map.Center)
                _map.DisplayTransform.FromDisplay(pt, dpt1)

                Dim d As MapInfo.Geometry.Distance = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(_map, rect.Width / 6)
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchWithinDistance(dpt1, _map.GetDisplayCoordSys(), d, ContainsType.Centroid)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)

                ' show search geometry on screen for visual confirmation
                Dim p As MapInfo.Geometry.Point = New MapInfo.Geometry.Point(_map.GetDisplayCoordSys(), dpt1)
                Dim Buffer As MapInfo.Geometry.FeatureGeometry = p.Buffer(d.Value, d.Unit, 20, DistanceType.Spherical)
                ShowSearchGeometry(Buffer)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' find nearest city to center of map 
        Private Sub menuItemSearchNearest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSearchNearest.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                ' to compare to SearchWithinScreenRadius, we are calculating
                ' the search distance the same way it does
                Dim rect As System.Drawing.Rectangle = mapControl1.Bounds
                Dim pt As System.Drawing.Point = New System.Drawing.Point(rect.Left, rect.Top)
                pt.X += rect.Width / 2
                pt.Y += rect.Height / 2

                Dim dpt1 As MapInfo.Geometry.DPoint = New MapInfo.Geometry.DPoint
                ' convert center point to map coords (could use map.Center)
                _map.DisplayTransform.FromDisplay(pt, dpt1)
                Dim d As MapInfo.Geometry.Distance = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(_map, 3)

                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchNearest(dpt1, _map.GetDisplayCoordSys(), d)
                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)

                Dim p As MapInfo.Geometry.Point = New MapInfo.Geometry.Point(_map.GetDisplayCoordSys(), dpt1)
                Dim Buffer As MapInfo.Geometry.FeatureGeometry = p.Buffer(d.Value, d.Unit, 20, DistanceType.Spherical)
                ShowSearchGeometry(Buffer)

                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' search through all tables to find objects that intersect
        ' the states bordering KS
        Private Sub menuItemSearchMultipleTables_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSearchMultipleTables.Click

            Try

                Cursor.Current = Cursors.WaitCursor
                ' find states that intersect KS
                ' then combine them and search all layers within
                ' also uses search for feature
                Dim fKS As MapInfo.Data.Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='KS'"))
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchIntersectsFeature(fKS, IntersectType.Geometry)
                Dim fc As MapInfo.data.IResultSetFeatureCollection = _catalog.Search("usa", si)

                Dim fp As MapInfo.FeatureProcessing.FeatureProcessor = New MapInfo.FeatureProcessing.FeatureProcessor
                Dim f As MapInfo.Data.Feature = fp.Combine(fc)

                si = MapInfo.Data.SearchInfoFactory.SearchWithinFeature(f, ContainsType.Centroid)
                Dim mfc As MapInfo.Data.MultiResultSetFeatureCollection = _catalog.Search(_catalog.EnumerateTables(TableFilterFactory.FilterMappableTables()), si)

                ' set map view to show search results
                _map.SetView(f)

                ShowSearchGeometry(f.Geometry)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        Private Sub menuItemSqlExpressionFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSqlExpressionFilter.Click

            Try

                ' build up a search info by hand (not using the factory)
                Dim Filter As QueryFilter = New SqlExpressionFilter("Buses_91 * 3 < Trucks_91")
                Dim qd As QueryDefinition = New QueryDefinition(Filter, "*")
                Dim si As SearchInfo = New SearchInfo(Nothing, qd)

                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("mexico", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' uses contains filter to get cities in florida
        Private Sub menuItemContainsFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemContainsFilter.Click
            Try
                Dim fFlorida As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"))

                ' build up a search info by hand (not using the factory)
                Dim filter = New ContainsFilter(fFlorida.Geometry, ContainsType.Geometry)
                Dim qd As New QueryDefinition(filter, "MI_Geometry", "MI_Style", "MI_Key")
                Dim si As New SearchInfo(Nothing, qd)

                Dim fc As IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)
                ' set map view to show search results
                _map.SetView(fFlorida)

                ShowSearchGeometry(fFlorida.Geometry)

                ' show results as selection
                SelectFeatureCollection(fc)
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub 'menuItemContainsFilter_Click

        ' uses intersect filter to get states that intersect florida
        Private Sub menuItemIntersectFeature_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemIntersectFeature.Click

            Try

                Dim fFlorida As MapInfo.Data.Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"))

                ' build up a search info by hand (not using the factory)
                Dim Filter As QueryFilter = New IntersectFilter(fFlorida.Geometry, IntersectType.Bounds)
                Dim qd As QueryDefinition = New QueryDefinition(Filter, "*")
                Dim si As SearchInfo = New SearchInfo(Nothing, qd)

                Dim fc As MapInfo.Data.IResultSetFeatureCollection = _catalog.Search("usa", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ShowSearchGeometry(fFlorida.Geometry)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try

        End Sub
        ' a custom query filter that includes objects at least
        ' a certain distance from the query object and not greater than a second distance
        Private Class MyCustomFilter
            Inherits SpatialFilter
            Private _distanceInner As New Distance(100, DistanceUnit.Mile)
            Private _distanceOuter As New Distance(100, DistanceUnit.Mile)

            Public Sub New(ByVal geometry As FeatureGeometry, ByVal innerDistance As Distance, ByVal outerDistance As Distance)
                MyBase.New(geometry)
                _distanceInner = innerDistance
                _distanceOuter = outerDistance
            End Sub 'New


            Public Overrides Function GetSqlExpression() As String
                Return String.Format("MI_CentroidDistance(MI_Geometry, {0}, '{1}', 'Spherical') > {2} and " + "MI_CentroidDistance(MI_Geometry, {0}, '{3}', 'Spherical') < {4} ", paramName, CoordSys.DistanceUnitAbbreviation(_distanceInner.Unit), _distanceInner.Value, CoordSys.DistanceUnitAbbreviation(_distanceOuter.Unit), _distanceOuter.Value)
            End Function 'GetSqlExpression
        End Class 'MyCustomFilter

        ' uses custom filter to select objects within a distance range from
        ' chicago
        Private Sub menuItemCustomQueryFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemCustomQueryFilter.Click

            Try

                Dim fChicago As MapInfo.Data.Feature = _catalog.SearchForFeature("uscty_1k", MapInfo.Data.SearchInfoFactory.SearchWhere("City='Chicago'"))

                ' build up a search info by hand (not using the factory)
                Dim d1 As MapInfo.Geometry.Distance = New MapInfo.Geometry.Distance(35, MapInfo.Geometry.DistanceUnit.Mile)
                Dim d2 As MapInfo.Geometry.Distance = New MapInfo.Geometry.Distance(125, MapInfo.Geometry.DistanceUnit.Mile)
                Dim Filter As QueryFilter = New MyCustomFilter(fChicago.Geometry, d1, d2)
                Dim qd As QueryDefinition = New QueryDefinition(Filter, "*")
                Dim si As SearchInfo = New SearchInfo(Nothing, qd)

                Dim fc As IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ' make a search geometry to show what we are doing
                Dim buffer1 As MapInfo.Geometry.FeatureGeometry = fChicago.Geometry.Buffer(d1.Value, d1.Unit, 20, MapInfo.Geometry.DistanceType.Spherical)
                Dim buffer2 As MapInfo.Geometry.FeatureGeometry = fChicago.Geometry.Buffer(d2.Value, d2.Unit, 20, MapInfo.Geometry.DistanceType.Spherical)
                ShowSearchGeometry(buffer1)
                ShowSearchGeometry(buffer2, False)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ' shows how to combine filters using logical And
        Private Sub menuItemLogicalFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemLogicalFilter.Click
            Try
                Dim fChicago As Feature = _catalog.SearchForFeature("uscty_1k", MapInfo.Data.SearchInfoFactory.SearchWhere("City='Chicago'"))

                ' build up a search info by hand (not using the factory)
                Dim d1 As New Distance(35, DistanceUnit.Mile)
                Dim d2 As New Distance(125, DistanceUnit.Mile)
                Dim filterA = New MyCustomFilter(fChicago.Geometry, d1, d2)

                ' build up a search info by hand (not using the factory)
                Dim filterB = New SqlExpressionFilter("State='IL'")
                Dim filter = New LogicalFilter(LogicalOperation.And, filterA, filterB)
                Dim qd As New QueryDefinition(filter, "*")
                Dim si As New SearchInfo(Nothing, qd)

                Dim fc As IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ' make a search geometry to show what we are doing
                Dim buffer1 As FeatureGeometry = fChicago.Geometry.Buffer(d1.Value, d1.Unit, 20, DistanceType.Spherical)
                Dim buffer2 As FeatureGeometry = fChicago.Geometry.Buffer(d2.Value, d2.Unit, 20, DistanceType.Spherical)
                ShowSearchGeometry(buffer1)
                ShowSearchGeometry(buffer2, False)

                Dim fIL As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='IL'"))
                ShowSearchGeometry(fIL.Geometry, False)

                ' show results as selection
                SelectFeatureCollection(fc)
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub 'menuItemLogicalFilter_Click
       _

        ' create a custom processor that returns only the first n rows
        Public Class MySearchResultProcessor
            Inherits SearchResultProcessor
            Private _maxRows As Integer = 0
            Private _rowCount As Integer = 0


            Public Sub New(ByVal maxRows As Integer)
                _maxRows = maxRows
            End Sub 'New

            Public Overrides Sub BeginProcessingTable(ByVal features As IResultSetFeatureCollection)
                _rowCount = 0
            End Sub 'BeginProcessingTable

            Public Overrides Function ProcessRow(ByVal reader As MIDataReader, ByVal features As IResultSetFeatureCollection) As Boolean
                If _maxRows = 0 Or _rowCount < _maxRows Then
                    DirectCast(features, MapInfo.Data.IFeatureCollection).Add(reader.Current)
                End If
                _rowCount += 1
                If _maxRows > 0 And _rowCount >= _maxRows Then
                    Return False ' stop processing of this table
                End If
                Return True
            End Function 'ProcessRow
        End Class 'MySearchResultProcessor


        ' return the first 10 rows from cities sorted by state in reverse
        ' return the first 10 rows from cities sorted by state in reverse
        Private Sub menuItemCustomProcessor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemCustomProcessor.Click
            Try
                Dim filter = New SqlExpressionFilter(Nothing) ' all rows
                Dim qd As New QueryDefinition(filter, "*")
                Dim orderby(1) As String
                orderby(0) = "State Desc"
                qd.OrderBy = orderby
                Dim srp = New MySearchResultProcessor(10) ' stop after 10 rows
                Dim si As New SearchInfo(srp, qd)

                Dim fc As IResultSetFeatureCollection = _catalog.Search("usa", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ' show results as selection
                SelectFeatureCollection(fc)
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub 'menuItemCustomProcessor_Click

        Private Sub menuItemSetColumns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuItemSetColumns.Click

            Try
                ' build up a search info by hand (not using the factory)
                Dim filter = New SqlExpressionFilter("POP_90 < 1000000")
                Dim qd As New QueryDefinition(filter, "MI_Key")

                ' to Add Columns
                qd.AppendColumns("State", "MI_Geometry")

                ' to set all new set of columns
                ' not using MI_Geometry here
                Dim cols() As String = {"MI_Key", "MI_Style", "State_Name", "POP_90", "Households_90"}
                qd.Columns = cols

                ' Note: if you are doing a multi table search, the columns must apply to each table
                ' alternatively, you can derive a new class from QueryDefinition and
                ' override the GetColumns() method to return different columns for each table being searched
                Dim si As New SearchInfo(Nothing, qd)

                Dim fc As IResultSetFeatureCollection = _catalog.Search("mexico", si)
                ' set map view to show search results
                _map.SetView(DirectCast(_map.Layers("mexico"), MapInfo.Mapping.FeatureLayer)) '

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default

            End Try

        End Sub


        Private Sub menuItemSearchWithinFeature_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemSearchWithinFeature.Click
            Try

                Cursor.Current = Cursors.WaitCursor
                ' find and select cities with Georgia and Florida using geometry
                ' also uses search for feature
                Dim fFlorida As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='FL'"))
                Dim fGeorgia As Feature = _catalog.SearchForFeature("usa", MapInfo.Data.SearchInfoFactory.SearchWhere("State='GA'"))
                Dim g As FeatureGeometry = fFlorida.Geometry.Combine(fGeorgia.Geometry)
                Dim si As SearchInfo = MapInfo.Data.SearchInfoFactory.SearchWithinGeometry(g, ContainsType.Centroid)
                Dim fc As IResultSetFeatureCollection = _catalog.Search("uscty_1k", si)
                ' set map view to show search results
                _map.SetView(fc.Envelope)

                ShowSearchGeometry(g)

                ' show results as selection
                SelectFeatureCollection(fc)

            Finally

                Cursor.Current = Cursors.Default

            End Try
        End Sub

    End Class
End Namespace