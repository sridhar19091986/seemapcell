Imports MapInfo.Engine
Imports MapInfo.Data

Public Class Form1
  Inherits System.Windows.Forms.Form

  ' The MICommand variable is used to generate cursors against open tables.
  Private miCommand As MapInfo.Data.MICommand
  Private miConnection As MapInfo.Data.MIConnection

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    UpdateNavigationButtons()
    Me.components = New System.ComponentModel.Container

    ' Create a command object to hold parameters and last command executed
    Me.miConnection = New MapInfo.Data.MIConnection
    Me.miConnection.Open()
    Me.miCommand = miConnection.CreateCommand()

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
      Me.miCommand.Cancel()
      Me.miCommand.Dispose()
      Me.miConnection.Close()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  Friend WithEvents OpenButton As System.Windows.Forms.Button
  Friend WithEvents showTableStructure As System.Windows.Forms.CheckBox
  Friend WithEvents ExitButton As System.Windows.Forms.Button
  Friend WithEvents dataGrid As System.Windows.Forms.DataGrid
  ' Allows looping over the set of open tables
  Private tableEnum As MapInfo.Data.ITableEnumerator
  ' Returns the number of open tables
  Private ReadOnly Property OpenTableCount() As String
    Get
      Return Session.Current.Catalog.Count
    End Get
  End Property
  ' Alias of the currently displayed table
  Private tableAlias As String
  ' Index of the currently displayed table
  Private tableIndex As Integer = 0
  Friend WithEvents buttonCloseTable As System.Windows.Forms.Button
  Friend WithEvents buttonNext As System.Windows.Forms.Button
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.OpenButton = New System.Windows.Forms.Button
    Me.showTableStructure = New System.Windows.Forms.CheckBox
    Me.ExitButton = New System.Windows.Forms.Button
    Me.dataGrid = New System.Windows.Forms.DataGrid
    Me.buttonCloseTable = New System.Windows.Forms.Button
    Me.buttonNext = New System.Windows.Forms.Button
    CType(Me.dataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'OpenButton
    '
    Me.OpenButton.Location = New System.Drawing.Point(8, 8)
    Me.OpenButton.Name = "OpenButton"
    Me.OpenButton.Size = New System.Drawing.Size(96, 32)
    Me.OpenButton.TabIndex = 0
    Me.OpenButton.Text = "Open Table"
    '
    'showTableStructure
    '
    Me.showTableStructure.Location = New System.Drawing.Point(392, 16)
    Me.showTableStructure.Name = "showTableStructure"
    Me.showTableStructure.Size = New System.Drawing.Size(144, 16)
    Me.showTableStructure.TabIndex = 1
    Me.showTableStructure.Text = "Show Table Structure"
    '
    'ExitButton
    '
    Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.ExitButton.Location = New System.Drawing.Point(624, 8)
    Me.ExitButton.Name = "ExitButton"
    Me.ExitButton.Size = New System.Drawing.Size(80, 32)
    Me.ExitButton.TabIndex = 2
    Me.ExitButton.Text = "Exit"
    '
    'dataGrid
    '
    Me.dataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.dataGrid.DataMember = ""
    Me.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
    Me.dataGrid.Location = New System.Drawing.Point(8, 56)
    Me.dataGrid.Name = "dataGrid"
    Me.dataGrid.Size = New System.Drawing.Size(704, 424)
    Me.dataGrid.TabIndex = 3
    '
    'buttonCloseTable
    '
    Me.buttonCloseTable.Location = New System.Drawing.Point(112, 8)
    Me.buttonCloseTable.Name = "buttonCloseTable"
    Me.buttonCloseTable.Size = New System.Drawing.Size(88, 32)
    Me.buttonCloseTable.TabIndex = 5
    Me.buttonCloseTable.Text = "Close Table"
    '
    'buttonNext
    '
    Me.buttonNext.Location = New System.Drawing.Point(232, 8)
    Me.buttonNext.Name = "buttonNext"
    Me.buttonNext.Size = New System.Drawing.Size(112, 32)
    Me.buttonNext.TabIndex = 7
    Me.buttonNext.Text = "Display Next Table"
    '
    'Form1
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(720, 494)
    Me.Controls.Add(Me.buttonNext)
    Me.Controls.Add(Me.buttonCloseTable)
    Me.Controls.Add(Me.dataGrid)
    Me.Controls.Add(Me.ExitButton)
    Me.Controls.Add(Me.showTableStructure)
    Me.Controls.Add(Me.OpenButton)
    Me.Name = "Form1"
    Me.Text = "Open Table Sample"
    CType(Me.dataGrid, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region


  ' Set the data from the specified table into the data grid.
  ' Show the table's scheme if showSchema is true.
  Private Sub SetGrid(ByVal miTable As MapInfo.Data.Table, ByVal showSchema As Boolean)
    dataGrid.CaptionText = miTable.Alias
    Me.miCommand.CommandText = "Select * from " + miTable.Alias
    Dim miReader As MapInfo.Data.MIDataReader
    Dim dt As System.Data.DataTable
    Dim i As Integer
    Dim dc As DataColumn
    Dim dr As DataRow

    miReader = Me.miCommand.ExecuteReader()
    dt = New DataTable("Data")
    For i = 0 To (miReader.FieldCount() - 1)
      dc = dt.Columns.Add(miReader.GetName(i))
    Next
    While (miReader.Read())
      dr = dt.NewRow()
      For i = 0 To (miReader.FieldCount - 1)
        dr.Item(i) = miReader.GetValue(i)
      Next
      dt.Rows.Add(dr)
    End While
    If (showSchema) Then
      dataGrid.DataSource = miReader.GetSchemaTable()
    Else
      dataGrid.DataSource = dt
    End If
    miReader.Close()
  End Sub

  ' Handle a click of the Open Table button
  Private Sub OpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenButton.Click
    Dim openFile As New System.Windows.Forms.OpenFileDialog
    Dim miTable As MapInfo.Data.Table
    Dim filename As String
    Dim s As String
    openFile.DefaultExt = "tab"
    ' The Filter property requires a search string after the pipe ( | )
    openFile.Filter = "MapInfo Tables (*.tab)|*.tab"
    openFile.Multiselect = False
    openFile.ShowDialog()
    If openFile.FileName.Length > 0 Then
      filename = openFile.FileName
      If Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, s) Then
        miTable = Session.Current.Catalog.OpenTable(s)
      Else
        ' try anyway, at least we will get an exception to report
        miTable = Session.Current.Catalog.OpenTable(filename, "OpenTableTable")
      End If
      tableAlias = miTable.Alias
      ResetTableEnum()
      SetGrid(tableEnum.Current, showTableStructure.Checked)
      UpdateNavigationButtons()
    End If

  End Sub
  ' Handle a click of the Exit button
  Private Sub ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitButton.Click
    Application.Exit()
  End Sub
  ' Reset the _tableEnum field to the table being displayed, if any
  Private Sub ResetTableEnum()
    Dim found As Boolean
    Dim index As Integer
    found = False
    index = 0
    tableEnum = Nothing
    tableEnum = Session.Current.Catalog.EnumerateTables(TableFilterFactory.FilterAllTables())
    While tableEnum.MoveNext()
      index = index + 1
      If String.Equals(tableEnum.Current.Alias, tableAlias) Then
        found = True
        Exit While
      End If
    End While
    If Not found Then
      tableEnum.Reset()
      tableEnum.MoveNext()
      tableIndex = 1
    Else
      tableIndex = index
    End If

  End Sub
  ' Enable the Display Next button iff there is more than one table open.
  ' Enable the Close Table button iff there is at least one table open.
  Private Sub UpdateNavigationButtons()
    buttonNext.Enabled = ((OpenTableCount > 1) And (tableIndex <= OpenTableCount))
    buttonCloseTable.Enabled = (OpenTableCount > 0)
  End Sub
  ' Handle a click of the Close Table button
  Private Sub buttonCloseTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonCloseTable.Click
    tableEnum.Current.Close()
    If OpenTableCount > 0 Then
      ResetTableEnum()
      SetGrid(tableEnum.Current, Me.showTableStructure.Checked)
    Else
      dataGrid.DataSource = Nothing
      dataGrid.CaptionText = ""
    End If
    UpdateNavigationButtons()
  End Sub
  ' Handle a click of the Display Next button
  Private Sub buttonNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonNext.Click
    If Not tableEnum.MoveNext() Then
      tableEnum.Reset()
      tableEnum.MoveNext()
    End If
    SetGrid(tableEnum.Current, Me.showTableStructure.Checked)
    tableIndex = (tableIndex + 1) Mod OpenTableCount
    tableAlias = tableEnum.Current.Alias
    UpdateNavigationButtons()
  End Sub
  ' Handle a click of the Show Table Structure button
  Private Sub showTableStructure_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles showTableStructure.CheckedChanged
    If OpenTableCount > 0 Then
      ' Make sure the currently displayed table is showing its data if the box is not
      ' checked or its structure if the box is checked
      SetGrid(tableEnum.Current, Me.showTableStructure.Checked)
    End If
  End Sub
End Class
