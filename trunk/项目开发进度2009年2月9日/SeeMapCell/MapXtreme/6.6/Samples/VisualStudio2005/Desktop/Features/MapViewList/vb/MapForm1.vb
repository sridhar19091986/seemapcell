Imports System.Reflection
Imports System.IO
Imports MapInfo.Mapping
Imports MapInfo.Engine
Imports MapInfo.Windows.Dialogs

Public Class MapForm1
	Inherits System.Windows.Forms.Form

	Friend WithEvents Map As MapInfo.Mapping.Map
  Friend WithEvents Layers As MapInfo.Mapping.Layers
	' MapViewList maintains information about the zoom level and center of the map for each view change
	Friend WithEvents MapViewList As MapInfo.Mapping.MapViewList

#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		' Listen to some map events
		Map = MapControl1.Map
		Layers = MapControl1.Map.Layers
		AddHandler Map.ViewChangedEvent, AddressOf Map_ViewChanged
        ' Create the view list for our map.  The default maximum number of views is 10; we'll go with 5
		MapViewList = New MapInfo.Mapping.MapViewList(Map, 5)
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuMap As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemPreviousView As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemNextView As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemView As System.Windows.Forms.MenuItem
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
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuMap = New System.Windows.Forms.MenuItem
        Me.MenuItemPreviousView = New System.Windows.Forms.MenuItem
        Me.MenuItemNextView = New System.Windows.Forms.MenuItem
        Me.MenuItemView = New System.Windows.Forms.MenuItem
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
        Me.Panel1.Size = New System.Drawing.Size(398, 248)
        Me.Panel1.TabIndex = 0
        '
        'MapControl1
        '
        Me.MapControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MapControl1.Location = New System.Drawing.Point(0, 0)
        Me.MapControl1.Name = "MapControl1"
        Me.MapControl1.Size = New System.Drawing.Size(394, 244)
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
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuMap})
        '
        'MenuMap
        '
        Me.MenuMap.Index = 0
        Me.MenuMap.MdiList = True
        Me.MenuMap.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemPreviousView, Me.MenuItemNextView, Me.MenuItemView})
        Me.MenuMap.Text = "&Map"
        '
        'MenuItemPreviousView
        '
        Me.MenuItemPreviousView.Enabled = False
        Me.MenuItemPreviousView.Index = 0
        Me.MenuItemPreviousView.Text = "&Previous View"
        '
        'MenuItemNextView
        '
        Me.MenuItemNextView.Enabled = False
        Me.MenuItemNextView.Index = 1
        Me.MenuItemNextView.Text = "&Next View"
        '
        'MenuItemView
        '
        Me.MenuItemView.Enabled = False
        Me.MenuItemView.Index = 2
        Me.MenuItemView.Text = "&View"
        '
        'MapToolBar1
        '
        Me.MapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.MapToolBarButtonOpenTable, Me.ToolBarButtonSeparator, Me.MapToolBarButtonSelect, Me.MapToolBarButtonZoomIn, Me.MapToolBarButtonZoomOut, Me.MapToolBarButtonPan})
        Me.MapToolBar1.Divider = False
        Me.MapToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.MapToolBar1.DropDownArrows = True
        Me.MapToolBar1.Location = New System.Drawing.Point(6, 0)
        Me.MapToolBar1.MapControl = Me.MapControl1
        Me.MapToolBar1.Name = "MapToolBar1"
        Me.MapToolBar1.ShowToolTips = True
        Me.MapToolBar1.Size = New System.Drawing.Size(160, 26)
        Me.MapToolBar1.TabIndex = 6
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
        Me.ClientSize = New System.Drawing.Size(406, 306)
        Me.Controls.Add(Me.MapToolBar1)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.Panel1)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(250, 200)
        Me.Name = "MapForm1"
        Me.Text = "MapViewList"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' Handler function called when the active map's view changes
	Private Sub Map_ViewChanged(ByVal sender As System.Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
        Dim dblZoom As Double = Val(String.Format("{0:E2}", MapControl1.Map.Zoom.Value))
        StatusBar1.Text = "Zoom: " + dblZoom.ToString() + " " + MapControl1.Map.Zoom.Unit.ToString()
    End Sub

    ' This method is invoked when the user selects the Next View menu item.
    Private Sub MenuItemNextView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuItemNextView.Click
        ' If our view list has a next view, switch to that view
        If MapViewList.HasNextView() Then
            MapViewList.NextView()
        End If
    End Sub

    ' This method is invoked when the user selects the Previous View menu item.
    Private Sub MenuItemPreviousView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuItemPreviousView.Click
        ' If our view list has a previous view, switch to that view
        If MapViewList.HasBackView() Then
            MapViewList.BackView()
        End If
    End Sub

    ' This method is invoked when the user selects the one of the sub-menu options of the View menu item.
    Private Sub MenuItemViewSubItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mi As MenuItem
        mi = sender
        ' We've stored the view number with the menu text, so we can retrieve it.
        ' Since the MapViewList is zero-based, we need to subract one as our menu text is one-based.
        Dim nSelectedView As Integer
        nSelectedView = Val(mi.Text) - 1
        MapViewList.SelectView(nSelectedView)
    End Sub

    ' This method handles the CurrentIndexChanged event for the MapViewList.
    ' We can use this method to maintain the view-related items in our Map menu.
    Private Sub MapViewList_CurrentIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MapViewList.CurrentIndexChanged
        ' Next View menu item is enabled if the MapViewList's HasNextView property is true
        MenuItemNextView.Enabled = MapViewList.HasNextView()

        ' Previous View menu item is enabled if the MapViewList's HasBackView property is true
        MenuItemPreviousView.Enabled = MapViewList.HasBackView()

        ' Determine the state of the View menu item
        MenuItemView.MenuItems.Clear()
        Dim i As Integer
        Dim nViews As Integer
        nViews = MapViewList.Count
        ' View menu item is enabled if the MapViewList has 1 or more MapViews
        MenuItemView.Enabled = nViews > 0
        ' Rebuild the list of views in the View menu item
        If MenuItemView.Enabled Then
            For i = 1 To nViews
                ' Include a view number in the menu text so we can determine which view is selected.
                ' Append the friendly name of the MapView item.
                MenuItemView.MenuItems.Add(i & ": " & MapViewList.Item(i - 1).Name)
                ' Associate our handler function with the menu item for the view.
                AddHandler MenuItemView.MenuItems.Item(i - 1).Click, AddressOf Me.MenuItemViewSubItem_Click
            Next
            ' Check the current view in the list of views
            MenuItemView.MenuItems.Item(MapViewList.Current).Checked = True
        End If
    End Sub
End Class