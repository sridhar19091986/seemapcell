Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms


Namespace FindVB
    '/ <summary>
    '/ Summary description for MapForm1.
    '/ </summary>
    Public Class MapForm1
        Inherits System.Windows.Forms.Form
        Private mapControl1 As MapInfo.Windows.Controls.MapControl
        '/ <summary>
        '/ Required designer variable.
        '/ </summary>
        Private components As System.ComponentModel.Container = Nothing

        Public Sub New(ByVal map As MapInfo.Mapping.Map)
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
            mapControl1.Map = map
        End Sub 'New


        '/ <summary>
        '/ Clean up any resources being used.
        '/ </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub 'Dispose

        '/ Required method for Designer support - do not modify
        '/ the contents of this method with the code editor.
        '/ </summary>
        Private Sub InitializeComponent()
            Me.mapControl1 = New MapInfo.Windows.Controls.MapControl
            Me.SuspendLayout()
            '
            'mapControl1
            '
            Me.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.mapControl1.Location = New System.Drawing.Point(0, 0)
            Me.mapControl1.Name = "mapControl1"
            Me.mapControl1.Size = New System.Drawing.Size(292, 266)
            Me.mapControl1.TabIndex = 0
            Me.mapControl1.Text = "mapControl1"
            Me.mapControl1.Tools.LeftButtonTool = Nothing
            Me.mapControl1.Tools.MiddleButtonTool = Nothing
            Me.mapControl1.Tools.RightButtonTool = Nothing
            '
            'MapForm1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.mapControl1)
            Me.Name = "MapForm1"
            Me.Text = "MapForm1"
            Me.ResumeLayout(False)

        End Sub 'InitializeComponent

        Private Sub MapForm1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load '
            mapControl1.Tools.InfoTipsEnabled = True
        End Sub 'MapForm1_Load

        
    End Class 'MapForm1
End Namespace 'FindVB