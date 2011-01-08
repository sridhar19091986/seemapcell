Public Class TableSearchPathDlg
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
  Friend WithEvents button2 As System.Windows.Forms.Button
  Friend WithEvents button1 As System.Windows.Forms.Button
  Friend WithEvents label1 As System.Windows.Forms.Label
  Friend WithEvents textBoxTableSearchPath As System.Windows.Forms.TextBox
  Friend WithEvents labelPath As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.button2 = New System.Windows.Forms.Button
    Me.button1 = New System.Windows.Forms.Button
    Me.label1 = New System.Windows.Forms.Label
    Me.textBoxTableSearchPath = New System.Windows.Forms.TextBox
    Me.labelPath = New System.Windows.Forms.Label
    Me.SuspendLayout()
    '
    'button2
    '
    Me.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.button2.Location = New System.Drawing.Point(256, 88)
    Me.button2.Name = "button2"
    Me.button2.TabIndex = 9
    Me.button2.Text = "Cancel"
    '
    'button1
    '
    Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.button1.Location = New System.Drawing.Point(136, 88)
    Me.button1.Name = "button1"
    Me.button1.TabIndex = 8
    Me.button1.Text = "OK"
    '
    'label1
    '
    Me.label1.Location = New System.Drawing.Point(184, 56)
    Me.label1.Name = "label1"
    Me.label1.Size = New System.Drawing.Size(168, 16)
    Me.label1.TabIndex = 7
    Me.label1.Text = "Separate Paths with ';'"
    '
    'textBoxTableSearchPath
    '
    Me.textBoxTableSearchPath.Location = New System.Drawing.Point(128, 24)
    Me.textBoxTableSearchPath.Name = "textBoxTableSearchPath"
    Me.textBoxTableSearchPath.Size = New System.Drawing.Size(312, 20)
    Me.textBoxTableSearchPath.TabIndex = 6
    Me.textBoxTableSearchPath.Text = ""
    '
    'labelPath
    '
    Me.labelPath.Location = New System.Drawing.Point(16, 24)
    Me.labelPath.Name = "labelPath"
    Me.labelPath.Size = New System.Drawing.Size(104, 16)
    Me.labelPath.TabIndex = 5
    Me.labelPath.Text = "Table Search Path:"
    '
    'TableSearchPathDlg
    '
    Me.AcceptButton = Me.button1
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.CancelButton = Me.button2
    Me.ClientSize = New System.Drawing.Size(456, 126)
    Me.Controls.Add(Me.button2)
    Me.Controls.Add(Me.button1)
    Me.Controls.Add(Me.label1)
    Me.Controls.Add(Me.textBoxTableSearchPath)
    Me.Controls.Add(Me.labelPath)
    Me.Name = "TableSearchPathDlg"
    Me.Text = "TableSearchPathDlg"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Property Path() As String
    Get
      Return textBoxTableSearchPath.Text
    End Get
    Set(ByVal Value As String)
      textBoxTableSearchPath.Text = value
    End Set
  End Property

  Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
    Me.Close()
  End Sub
End Class
