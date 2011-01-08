Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms


Namespace MIDataExplorer
    '/ <summary>
    '/ Summary description for TableSearchPathDialog.
    '/ </summary>
		Public Class TableSearchPathDialog
				Inherits System.Windows.Forms.Form
				Private labelPath As System.Windows.Forms.Label
				Private textBoxTableSearchPath As System.Windows.Forms.TextBox
				Private label1 As System.Windows.Forms.Label
        Private button2 As System.Windows.Forms.Button
				'/ <summary>
				'/ Required designer variable.
				'/ </summary>
				Private components As System.ComponentModel.Container = Nothing


				Public Sub New()
						'
						' Required for Windows Form Designer support
						'
						InitializeComponent()
				End Sub	 'New

				'
				' TODO: Add any constructor code after InitializeComponent call
				'

				'/ <summary>
				'/ Clean up any resources being used.
				'/ </summary>
				Protected Overloads Sub Dispose(ByVal disposing As Boolean)
						If disposing Then
								If Not (components Is Nothing) Then
										components.Dispose()
								End If
						End If
						MyBase.Dispose(disposing)
				End Sub	 'Dispose


				'/ Required method for Designer support - do not modify
				'/ the contents of this method with the code editor.
				'/ </summary>
        Private WithEvents button1 As System.Windows.Forms.Button
        Private Sub InitializeComponent()
            Me.labelPath = New System.Windows.Forms.Label
            Me.textBoxTableSearchPath = New System.Windows.Forms.TextBox
            Me.label1 = New System.Windows.Forms.Label
            Me.button1 = New System.Windows.Forms.Button
            Me.button2 = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'labelPath
            '
            Me.labelPath.Location = New System.Drawing.Point(16, 8)
            Me.labelPath.Name = "labelPath"
            Me.labelPath.Size = New System.Drawing.Size(104, 16)
            Me.labelPath.TabIndex = 0
            Me.labelPath.Text = "Table Search Path:"
            '
            'textBoxTableSearchPath
            '
            Me.textBoxTableSearchPath.Location = New System.Drawing.Point(136, 8)
            Me.textBoxTableSearchPath.Name = "textBoxTableSearchPath"
            Me.textBoxTableSearchPath.Size = New System.Drawing.Size(312, 20)
            Me.textBoxTableSearchPath.TabIndex = 1
            Me.textBoxTableSearchPath.Text = ""
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(192, 40)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(168, 16)
            Me.label1.TabIndex = 2
            Me.label1.Text = "Separate Paths with ';'"
            '
            'button1
            '
            Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.button1.Location = New System.Drawing.Point(143, 72)
            Me.button1.Name = "button1"
            Me.button1.TabIndex = 3
            Me.button1.Text = "OK"
            '
            'button2
            '
            Me.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.button2.Location = New System.Drawing.Point(263, 72)
            Me.button2.Name = "button2"
            Me.button2.TabIndex = 4
            Me.button2.Text = "Cancel"
            '
            'TableSearchPathDialog
            '
            Me.AcceptButton = Me.button1
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.CancelButton = Me.button2
            Me.ClientSize = New System.Drawing.Size(480, 110)
            Me.Controls.Add(Me.button2)
            Me.Controls.Add(Me.button1)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.textBoxTableSearchPath)
            Me.Controls.Add(Me.labelPath)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "TableSearchPathDialog"
            Me.ShowInTaskbar = False
            Me.Text = "Table Search Path"
            Me.ResumeLayout(False)

        End Sub  'InitializeComponent


        Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click  '
            Me.Close()
        End Sub  'button1_Click


        Public Property Path() As String
            Get
                Return textBoxTableSearchPath.Text
            End Get
            Set(ByVal Value As String)
                textBoxTableSearchPath.Text = Value
            End Set
        End Property


    End Class 'TableSearchPathDialog
End Namespace 'MIDataExplorer