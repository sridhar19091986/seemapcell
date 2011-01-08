Imports MapInfo.Mapping

Public Class [!output SAFE_ITEM_NAME]
    Inherits System.Windows.Forms.Form

	Friend WithEvents Map As MapInfo.Mapping.Map

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Listen to some map events
				Map = MapControl1.Map
				AddHandler Map.ViewChangedEvent, Addressof Map_ViewChanged

				' TODO: Set the MessageBox.Caption property to specify the text 
				' that appears on the title bar of various system message boxes: 
				'   MapInfo.Windows.MessageBox.Caption = "My Application Name"; 

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
	Friend WithEvents MapToolBar1 As MapInfo.Windows.Controls.MapToolBar
	Friend WithEvents MapToolBarButtonOpenTable As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents MapToolBarButtonLayerControl As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents ToolBarButtonSeparator As System.Windows.Forms.ToolBarButton
	Friend WithEvents MapToolBarButtonSelect As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents MapToolBarButtonZoomIn As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents MapToolBarButtonZoomOut As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents MapToolBarButtonPan As MapInfo.Windows.Controls.MapToolBarButton
	Friend WithEvents MapToolBarButtonCenter As MapInfo.Windows.Controls.MapToolBarButton
		  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Me.Panel1 = New System.Windows.Forms.Panel
		Me.MapControl1 = New MapInfo.Windows.Controls.MapControl
		Me.StatusBar1 = New System.Windows.Forms.StatusBar
		Me.MapToolBar1 = New MapInfo.Windows.Controls.MapToolBar
		Me.MapToolBarButtonOpenTable = New MapInfo.Windows.Controls.MapToolBarButton
		Me.MapToolBarButtonLayerControl = New MapInfo.Windows.Controls.MapToolBarButton
		Me.ToolBarButtonSeparator = New System.Windows.Forms.ToolBarButton
		Me.MapToolBarButtonSelect = New MapInfo.Windows.Controls.MapToolBarButton
		Me.MapToolBarButtonZoomIn = New MapInfo.Windows.Controls.MapToolBarButton
		Me.MapToolBarButtonZoomOut = New MapInfo.Windows.Controls.MapToolBarButton
		Me.MapToolBarButtonPan = New MapInfo.Windows.Controls.MapToolBarButton
		Me.MapToolBarButtonCenter = New MapInfo.Windows.Controls.MapToolBarButton
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
		Me.MapControl1.Text = "地图控件1"
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
		Me.MapToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.MapToolBarButtonOpenTable, Me.MapToolBarButtonLayerControl, Me.ToolBarButtonSeparator, Me.MapToolBarButtonSelect, Me.MapToolBarButtonZoomIn, Me.MapToolBarButtonZoomOut, Me.MapToolBarButtonPan, Me.MapToolBarButtonCenter})
		Me.MapToolBar1.Divider = False
		Me.MapToolBar1.Dock = System.Windows.Forms.DockStyle.None
		Me.MapToolBar1.DropDownArrows = True
		Me.MapToolBar1.Location = New System.Drawing.Point(4, 8)
		Me.MapToolBar1.MapControl = Me.MapControl1
		Me.MapToolBar1.Name = "MapToolBar1"
		Me.MapToolBar1.ShowToolTips = True
		Me.MapToolBar1.Size = New System.Drawing.Size(407, 26)
		Me.MapToolBar1.TabIndex = 3
		'
		'MapToolBarButtonOpenTable
		'
		Me.MapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable
		Me.MapToolBarButtonOpenTable.ToolTipText = "打开表"
		'
		'MapToolBarButtonLayerControl
		'
		Me.MapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl
		Me.MapToolBarButtonLayerControl.ToolTipText = "图层控制"
		'
		'ToolBarButtonSeparator
		'
		Me.ToolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
		'
		'MapToolBarButtonSelect
		'
		Me.MapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select
		Me.MapToolBarButtonSelect.ToolTipText = "选择"
		'
		'MapToolBarButtonZoomIn
		'
		Me.MapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn
		Me.MapToolBarButtonZoomIn.ToolTipText = "放大"
		'
		'MapToolBarButtonZoomOut
		'
		Me.MapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut
		Me.MapToolBarButtonZoomOut.ToolTipText = "缩小"
		'
		'MapToolBarButtonPan
		'
		Me.MapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan
		Me.MapToolBarButtonPan.ToolTipText = "平移"
		'
		'MapToolBarButtonCenter
		'
		Me.MapToolBarButtonCenter.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Center
		Me.MapToolBarButtonCenter.ToolTipText = "中心"
		'
		'[!output SAFE_ITEM_NAME]
		'
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(406, 306)
		Me.Controls.Add(Me.MapToolBar1)
		Me.Controls.Add(Me.StatusBar1)
		Me.Controls.Add(Me.Panel1)
		Me.MinimumSize = New System.Drawing.Size(250, 200)
		Me.Name = "[!output SAFE_ITEM_NAME]"
		Me.Text = "[!output SAFE_ITEM_NAME]"
		Me.Panel1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

#End Region


	' Handler function called when the active map's view changes
	Private Sub Map_ViewChanged(ByVal sender As System.Object, ByVal e As MapInfo.Mapping.ViewChangedEventArgs)
		' Display the zoom level
		Dim dblZoom as Double = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value))
		StatusBar1.Text = "缩放:  " + dblZoom.ToString() + " " + MapInfo.Geometry.CoordSys.DistanceUnitAbbreviation(mapControl1.Map.Zoom.Unit)
	End Sub

End Class
