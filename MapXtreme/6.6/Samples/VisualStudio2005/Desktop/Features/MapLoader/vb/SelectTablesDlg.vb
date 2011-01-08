Public Class SelectTablesDlg
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub
  '/ <summary>
  '/ Creates a new SelectTablesDlg object using a list of tables as input.
  '/ </summary>
  '/ <remarks>None.</remarks>
  '/ <param name="tables">List of open tables.</param>
    Public Sub New(ByVal tables As MapInfo.Data.ITableEnumerator)
        MyBase.New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()

        ' Populate the table list
        ' Add tables to the listbox
        Dim tab As MapInfo.Data.Table
        For Each tab In tables
            listBoxTables.Items.Add(tab)
        Next
        ' Set initial selection (first item in list)
        If listBoxTables.Items.Count > 0 Then
            listBoxTables.SelectedIndex = 0
        End If
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
    Friend WithEvents listBoxTables As System.Windows.Forms.ListBox
  Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonAccept As System.Windows.Forms.Button
  Private WithEvents labelSelectTables As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.listBoxTables = New System.Windows.Forms.ListBox
    Me.labelSelectTables = New System.Windows.Forms.Label
    Me.buttonCancel = New System.Windows.Forms.Button
    Me.buttonAccept = New System.Windows.Forms.Button
    Me.Label1 = New System.Windows.Forms.Label
    Me.SuspendLayout()
    '
    'listBoxTables
    '
    Me.listBoxTables.Location = New System.Drawing.Point(16, 32)
    Me.listBoxTables.Name = "listBoxTables"
    Me.listBoxTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
    Me.listBoxTables.Size = New System.Drawing.Size(256, 147)
    Me.listBoxTables.TabIndex = 5
    '
    'labelSelectTables
    '
    Me.labelSelectTables.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.labelSelectTables.Enabled = False
    Me.labelSelectTables.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.labelSelectTables.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.labelSelectTables.Location = New System.Drawing.Point(9, -15)
    Me.labelSelectTables.Name = "labelSelectTables"
    Me.labelSelectTables.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.labelSelectTables.Size = New System.Drawing.Size(144, 16)
    Me.labelSelectTables.TabIndex = 4
    Me.labelSelectTables.Text = "Select &Tables:"
    '
    'buttonCancel
    '
    Me.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.buttonCancel.Location = New System.Drawing.Point(160, 192)
    Me.buttonCancel.Name = "buttonCancel"
    Me.buttonCancel.TabIndex = 7
    Me.buttonCancel.Text = "Cancel"
    '
    'buttonAccept
    '
    Me.buttonAccept.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.buttonAccept.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.buttonAccept.Location = New System.Drawing.Point(48, 192)
    Me.buttonAccept.Name = "buttonAccept"
    Me.buttonAccept.TabIndex = 6
    Me.buttonAccept.Text = "OK"
    '
    'Label1
    '
    Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
    Me.Label1.Location = New System.Drawing.Point(16, 8)
    Me.Label1.Name = "Label1"
    Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label1.Size = New System.Drawing.Size(144, 16)
    Me.Label1.TabIndex = 8
    Me.Label1.Text = "Select &Tables:"
    '
    'SelectTablesDlg
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(288, 221)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.listBoxTables)
    Me.Controls.Add(Me.labelSelectTables)
    Me.Controls.Add(Me.buttonCancel)
    Me.Controls.Add(Me.buttonAccept)
    Me.Name = "SelectTablesDlg"
    Me.Text = "SelectTablesDlg"
    Me.ResumeLayout(False)

  End Sub

#End Region
  ' List of selected tables
  Private _selectedTables() As MapInfo.Data.Table = Nothing

  Private Sub buttonAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonAccept.Click
    ' Construct the selected table list
    If listBoxTables.SelectedItems.Count > 0 Then
      _selectedTables = New MapInfo.Data.Table(listBoxTables.SelectedItems.Count - 1) {}
      listBoxTables.SelectedItems.CopyTo(_selectedTables, 0)
    End If
    ' Set return value and close the dialog
    DialogResult = DialogResult.OK
    Me.Close()
  End Sub

  Private Sub buttonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCancel.Click
    DialogResult = DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub labelSelectTables_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles labelSelectTables.DoubleClick
    buttonAccept.PerformClick()
  End Sub


  '/ <summary>
  '/ Gets an array of UITables that were selected by the user.
  '/ </summary>
  '/ <remarks>
  '/ This value is only valid when the dialog returns with 
  '/ result of DialogResult.OK.  Otherwise the returned value is null.
  '/ </remarks>
  Public ReadOnly Property SelectedTables() As MapInfo.Data.Table()
    Get
      Return _selectedTables
    End Get
  End Property


  '/ <summary>
  '/ Gets the count of UITables that were selected by the user.
  '/ </summary>
  '/ <remarks>
  '/ This value is only valid when the dialog returns with 
  '/ result of DialogResult.OK.  Otherwise the returned value is zero.
  '/ </remarks>
  Public ReadOnly Property SelectionCount() As Integer
    Get
      If Not _selectedTables Is Nothing Then
        Return _selectedTables.Length
      Else
        Return 0
      End If
    End Get
  End Property
End Class
