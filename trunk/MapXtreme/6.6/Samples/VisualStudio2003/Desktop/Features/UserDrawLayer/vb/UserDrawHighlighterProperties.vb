Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms
Imports MapInfo.Windows.Controls

' this class is a property dialog for UserDrawHighlighter
' it lets you choose the highlight color and set the opacity
' it is intended to be used in the LayerControl.
Public Class UserDrawHighlighterProperties
  Inherits MapInfo.Windows.Controls.PropertiesUserControl

  ' Declare some private variables we will need later: 
  Private _bgColor As Color = Color.White
  Private _colorDlg As ColorDialog = Nothing
  Private _layer As UserDrawHighlighter
  Private labelBackground As System.Windows.Forms.Label
  Private panelSample As System.Windows.Forms.Panel
  Private WithEvents buttonChoose As System.Windows.Forms.Button

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    Category = PropertiesCategory.Style
    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'UserControl overrides dispose to clean up the component list.
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
  Friend WithEvents LabelOpacity As System.Windows.Forms.Label
  Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.labelBackground = New System.Windows.Forms.Label
    Me.panelSample = New System.Windows.Forms.Panel
    Me.buttonChoose = New System.Windows.Forms.Button
    Me.LabelOpacity = New System.Windows.Forms.Label
    Me.TrackBar1 = New System.Windows.Forms.TrackBar
    CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'labelBackground
    '
    Me.labelBackground.Location = New System.Drawing.Point(8, 8)
    Me.labelBackground.Name = "labelBackground"
    Me.labelBackground.Size = New System.Drawing.Size(160, 20)
    Me.labelBackground.TabIndex = 0
    Me.labelBackground.Text = "Highlight Color:"
    Me.labelBackground.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'panelSample
    '
    Me.panelSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.panelSample.Location = New System.Drawing.Point(8, 32)
    Me.panelSample.Name = "panelSample"
    Me.panelSample.Size = New System.Drawing.Size(112, 24)
    Me.panelSample.TabIndex = 1
    '
    'buttonChoose
    '
    Me.buttonChoose.Location = New System.Drawing.Point(128, 32)
    Me.buttonChoose.Name = "buttonChoose"
    Me.buttonChoose.Size = New System.Drawing.Size(112, 24)
    Me.buttonChoose.TabIndex = 2
    Me.buttonChoose.Text = "Choose C&olor..."
    '
    'LabelOpacity
    '
    Me.LabelOpacity.Location = New System.Drawing.Point(8, 72)
    Me.LabelOpacity.Name = "LabelOpacity"
    Me.LabelOpacity.Size = New System.Drawing.Size(56, 16)
    Me.LabelOpacity.TabIndex = 3
    Me.LabelOpacity.Text = "&Opacity: "
    '
    'TrackBar1
    '
    Me.TrackBar1.Location = New System.Drawing.Point(96, 64)
    Me.TrackBar1.Maximum = 255
    Me.TrackBar1.Name = "TrackBar1"
    Me.TrackBar1.Size = New System.Drawing.Size(136, 40)
    Me.TrackBar1.TabIndex = 4
    Me.TrackBar1.TickFrequency = 16
    Me.TrackBar1.Value = 128
    '
    'UserDrawHighlighterProperties
    '
    Me.Controls.Add(Me.TrackBar1)
    Me.Controls.Add(Me.LabelOpacity)
    Me.Controls.Add(Me.buttonChoose)
    Me.Controls.Add(Me.panelSample)
    Me.Controls.Add(Me.labelBackground)
    Me.Name = "UserDrawHighlighterProperties"
    Me.Size = New System.Drawing.Size(256, 112)
    CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

  '/ The Setup method initializes the control.  LayerControl will call this
  '/ method automatically, whenever the user selects a node of the specified
  '/ type in the layer tree.  Each node in the layer tree has a corresponding
  '/ object in the MapXtreme 2004 object model; when the user selects a node in
  '/ the layer tree, that node's object is passed to the Setup method.  
  '/ For this class, we assume obj is a UserDrawHighlighter object
  Public Overrides Sub Setup(ByVal obj As Object)
    ' the obj parameter should never be null, but just to be on the safe side:
    If (obj Is Nothing) Then

      Exit Sub
    End If

    _layer = obj
    _bgColor = _layer.HighlightColor
    panelSample.BackColor = Color.FromArgb(_layer.Opacity, _bgColor)
    TrackBar1.Value = _layer.Opacity
  End Sub

  Private Sub buttonChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonChoose.Click
    If _colorDlg Is Nothing Then
      _colorDlg = New ColorDialog
    End If
    _colorDlg.Color = _bgColor
    If _colorDlg.ShowDialog() = DialogResult.OK Then
      ' The user chose a color and clicked OK.  
      ' Use the color as the map's background color. 
      _bgColor = _colorDlg.Color
      panelSample.BackColor = Color.FromArgb(_layer.Opacity, _bgColor)
      _layer.HighlightColor = _bgColor
    End If

  End Sub

  Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
    _layer.Opacity = TrackBar1.Value
    panelSample.BackColor = Color.FromArgb(_layer.Opacity, _bgColor)

  End Sub
End Class
