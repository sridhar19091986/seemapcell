Imports MapInfo.Windows.Controls

Public Class MapBackgroundControl
    Inherits MapInfo.Windows.Controls.PropertiesUserControl

    ' Declare some private variables we will need later: 
    Private _bgColor As Color = Color.White
    Private _colorDlg As ColorDialog = Nothing

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the PropertiesUserControl.Category property,  which 
        ' dictates what text will appear on the Tab.  
        Me.Category = PropertiesCategory.Style

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

    ' The Setup method initializes the control.  LayerControl will call this
    ' method automatically, whenever the user selects a node of the specified
    ' type in the layer tree.  Each node in the layer tree has a corresponding
    ' object in the MapXtreme 2004 object model; when the user select a node in
    ' the layer tree, that node's object is passed to the Setup method.  
    '  
    ' For this class, we assume obj is a Map object 
    Public Overrides Sub Setup(ByVal obj As [Object])
        ' the obj parameter should never be null, but just to be on the safe side:
        If obj Is Nothing Then
            Return
        End If

        ' If Layer Control has been set up correctly, obj should be a Map object.
        ' Verify that obj is a Map, just to be on the safe side: 
        If TypeOf obj Is MapInfo.Mapping.Map Then
            ' At this point, you could cast obj to an appropriate type of object.
            ' For example, if you are writing a control to manage properties
            ' of a FeatureLayer, you would cast obj to a FeatureLayer. 
            ' You would then use the object to initialize the state of the control.
            '
            ' But this control is for setting properties of a Map object, 
            ' and Map object is a special case.  The PropertiesUserControl
            ' class has a Map property which represents the Map object in use.
            ' So we don't need to cast obj to a Map, since we already have a 
            ' Map property.  
            Dim bgBrush As Brush = Map.BackgroundBrush
            If TypeOf bgBrush Is SolidBrush Then
                _bgColor = CType(bgBrush, SolidBrush).Color
                panelSample.BackColor = _bgColor
            End If
        End If
    End Sub 'Setup

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents LabelBackground As System.Windows.Forms.Label
    Friend WithEvents PanelSample As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.LabelBackground = New System.Windows.Forms.Label
        Me.PanelSample = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'LabelBackground
        '
        Me.LabelBackground.Location = New System.Drawing.Point(8, 8)
        Me.LabelBackground.Name = "LabelBackground"
        Me.LabelBackground.Size = New System.Drawing.Size(208, 23)
        Me.LabelBackground.TabIndex = 0
        Me.LabelBackground.Text = "Map Background Color:"
        Me.LabelBackground.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PanelSample
        '
        Me.PanelSample.Location = New System.Drawing.Point(8, 32)
        Me.PanelSample.Name = "PanelSample"
        Me.PanelSample.Size = New System.Drawing.Size(112, 24)
        Me.PanelSample.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(128, 32)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Choose &Color..."
        '
        'MapBackgroundControl
        '
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PanelSample)
        Me.Controls.Add(Me.LabelBackground)
        Me.Name = "MapBackgroundControl"
        Me.Size = New System.Drawing.Size(248, 64)
        Me.ResumeLayout(False)

    End Sub

#End Region

    ' The method called when the user clicks the Choose Color button
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If _colorDlg Is Nothing Then
            _colorDlg = New ColorDialog
        End If
        _colorDlg.Color = _bgColor
        If _colorDlg.ShowDialog() = DialogResult.OK Then
            ' The user chose a color and clicked OK.  
            ' Use the color as the map's background color. 
            _bgColor = _colorDlg.Color
            PanelSample.BackColor = _bgColor
            Me.Map.BackgroundBrush = New SolidBrush(_bgColor)
        End If
    End Sub
End Class
