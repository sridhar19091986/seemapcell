Public Class MapLoaderOptions
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    cbEnableLayers.Checked = True
    cbAutoPosition.Checked = True

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
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents btnOK As System.Windows.Forms.Button
  Friend WithEvents cbClearMap As System.Windows.Forms.CheckBox
  Friend WithEvents textBoxStartPosition As System.Windows.Forms.TextBox
  Friend WithEvents labelStartPosition As System.Windows.Forms.Label
  Friend WithEvents cbAutoPosition As System.Windows.Forms.CheckBox
  Friend WithEvents cbEnableLayers As System.Windows.Forms.CheckBox
  Friend WithEvents cbLayersOnly As System.Windows.Forms.CheckBox
  Friend WithEvents cbSetMapName As System.Windows.Forms.CheckBox
  Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOK = New System.Windows.Forms.Button
    Me.cbClearMap = New System.Windows.Forms.CheckBox
    Me.textBoxStartPosition = New System.Windows.Forms.TextBox
    Me.labelStartPosition = New System.Windows.Forms.Label
    Me.cbAutoPosition = New System.Windows.Forms.CheckBox
    Me.cbEnableLayers = New System.Windows.Forms.CheckBox
    Me.cbLayersOnly = New System.Windows.Forms.CheckBox
    Me.cbSetMapName = New System.Windows.Forms.CheckBox
    Me.groupBox1 = New System.Windows.Forms.GroupBox
    Me.groupBox2 = New System.Windows.Forms.GroupBox
    Me.SuspendLayout()
    '
    'btnCancel
    '
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Location = New System.Drawing.Point(209, 48)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.TabIndex = 17
    Me.btnCancel.Text = "Cancel"
    '
    'btnOK
    '
    Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.btnOK.Location = New System.Drawing.Point(209, 16)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.TabIndex = 16
    Me.btnOK.Text = "OK"
    '
    'cbClearMap
    '
    Me.cbClearMap.Location = New System.Drawing.Point(17, 192)
    Me.cbClearMap.Name = "cbClearMap"
    Me.cbClearMap.TabIndex = 15
    Me.cbClearMap.Text = "Clear Map First"
    '
    'textBoxStartPosition
    '
    Me.textBoxStartPosition.Location = New System.Drawing.Point(137, 72)
    Me.textBoxStartPosition.Name = "textBoxStartPosition"
    Me.textBoxStartPosition.Size = New System.Drawing.Size(32, 20)
    Me.textBoxStartPosition.TabIndex = 12
    Me.textBoxStartPosition.Text = "1"
    Me.textBoxStartPosition.WordWrap = False
    '
    'labelStartPosition
    '
    Me.labelStartPosition.Location = New System.Drawing.Point(25, 80)
    Me.labelStartPosition.Name = "labelStartPosition"
    Me.labelStartPosition.Size = New System.Drawing.Size(104, 23)
    Me.labelStartPosition.TabIndex = 11
    Me.labelStartPosition.Text = "Start Position:"
    '
    'cbAutoPosition
    '
    Me.cbAutoPosition.Location = New System.Drawing.Point(25, 56)
    Me.cbAutoPosition.Name = "cbAutoPosition"
    Me.cbAutoPosition.TabIndex = 10
    Me.cbAutoPosition.Text = "Auto Position"
    '
    'cbEnableLayers
    '
    Me.cbEnableLayers.Location = New System.Drawing.Point(25, 32)
    Me.cbEnableLayers.Name = "cbEnableLayers"
    Me.cbEnableLayers.TabIndex = 9
    Me.cbEnableLayers.Text = "Enable Layers"
    '
    'cbLayersOnly
    '
    Me.cbLayersOnly.Location = New System.Drawing.Point(25, 128)
    Me.cbLayersOnly.Name = "cbLayersOnly"
    Me.cbLayersOnly.TabIndex = 14
    Me.cbLayersOnly.Text = "Layers Only"
    '
    'cbSetMapName
    '
    Me.cbSetMapName.Location = New System.Drawing.Point(25, 152)
    Me.cbSetMapName.Name = "cbSetMapName"
    Me.cbSetMapName.TabIndex = 13
    Me.cbSetMapName.Text = "Set Map Name"
    '
    'groupBox1
    '
    Me.groupBox1.Location = New System.Drawing.Point(9, 8)
    Me.groupBox1.Name = "groupBox1"
    Me.groupBox1.Size = New System.Drawing.Size(192, 96)
    Me.groupBox1.TabIndex = 18
    Me.groupBox1.TabStop = False
    Me.groupBox1.Text = "Map Loader Options"
    '
    'groupBox2
    '
    Me.groupBox2.Location = New System.Drawing.Point(9, 112)
    Me.groupBox2.Name = "groupBox2"
    Me.groupBox2.Size = New System.Drawing.Size(192, 72)
    Me.groupBox2.TabIndex = 19
    Me.groupBox2.TabStop = False
    Me.groupBox2.Text = "Geoset && Workspace Only"
    '
    'MapLoaderOptions
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(292, 222)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.cbClearMap)
    Me.Controls.Add(Me.textBoxStartPosition)
    Me.Controls.Add(Me.labelStartPosition)
    Me.Controls.Add(Me.cbAutoPosition)
    Me.Controls.Add(Me.cbEnableLayers)
    Me.Controls.Add(Me.cbLayersOnly)
    Me.Controls.Add(Me.cbSetMapName)
    Me.Controls.Add(Me.groupBox1)
    Me.Controls.Add(Me.groupBox2)
    Me.Name = "MapLoaderOptions"
    Me.Text = "MapLoaderOptions"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Property EnableLayers() As Boolean
    Get
      Return cbEnableLayers.Checked
    End Get
    Set(ByVal Value As Boolean)
      cbEnableLayers.Checked = value
    End Set
  End Property
  Public Property AutoPosition() As Boolean
    Get
      Return cbAutoPosition.Checked
    End Get
    Set(ByVal Value As Boolean)
      cbAutoPosition.Checked = value
    End Set
  End Property
  Public Property ClearMapFirst() As Boolean
    Get
      Return cbClearMap.Checked
    End Get
    Set(ByVal Value As Boolean)
      cbClearMap.Checked = value
    End Set
  End Property

  Public Shadows Property StartPosition() As Integer
    Get
      Return Integer.Parse(textBoxStartPosition.Text)
    End Get
    Set(ByVal Value As Integer)
      textBoxStartPosition.Text = value.ToString()
    End Set
  End Property

  Public Property LayersOnly() As Boolean
    Get
      Return cbLayersOnly.Checked
    End Get
    Set(ByVal Value As Boolean)
      cbLayersOnly.Checked = value
    End Set
  End Property

  Public Property SetMapName() As Boolean
    Get
      Return cbSetMapName.Checked
    End Get
    Set(ByVal Value As Boolean)
      cbSetMapName.Checked = value
    End Set
  End Property
End Class
