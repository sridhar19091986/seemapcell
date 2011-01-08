Imports System
Imports System.Data
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms

Imports MapInfo.Data
Imports MapInfo.Data.Find
Imports MapInfo.Engine
Namespace FindVB
    Public Class FindSampleForm
        Inherits System.Windows.Forms.Form
        '/ <summary>
        '/ The MICommand variable is used to generate cursors against open tables.
        '/ </summary>
        Private _miCommand As MICommand
        Private _miConnection As MIConnection

        ' search table
        Private _searchTable As Table
        Private _refiningTable As Table
        Private _result As FindResult
        Private _bSearchIntersection As Boolean = False

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            '
            ' TODO: Add any constructor code after InitializeComponent call
            '
            '
            ' Create a connection object and open it.
            '
            Me._miConnection = New MIConnection
            _miConnection.Open()

            '
            ' Create a command object to hold parameters and last command executed
            '
            Me._miCommand = _miConnection.CreateCommand()

            buttonFind.Enabled = False
            checkBoxUseCloseMatches.Checked = True
            listBoxSearchResult.Visible = False
            textBoxMaxCloseMatches.Text = "5"
        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            CleanUp()
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
        Friend WithEvents textBoxSearchTable As System.Windows.Forms.TextBox
        Friend WithEvents textBoxMaxCloseMatches As System.Windows.Forms.TextBox
        Friend WithEvents buttonOpenSearchTable As System.Windows.Forms.Button
        Friend WithEvents groupBoxPreferences As System.Windows.Forms.GroupBox
        Friend WithEvents checkBoxAddressNumAfterStreet As System.Windows.Forms.CheckBox
        Friend WithEvents groupBoxSearchParameters As System.Windows.Forms.GroupBox
        Friend WithEvents checkBoxIntersection As System.Windows.Forms.CheckBox
        Friend WithEvents textBoxIntersection As System.Windows.Forms.TextBox
        Private WithEvents labelSearchTable As System.Windows.Forms.Label
        Private WithEvents labelMaxCloseMatches As System.Windows.Forms.Label

        Friend WithEvents buttonFind As System.Windows.Forms.Button
        Private WithEvents labelSearchString As System.Windows.Forms.Label
        Private WithEvents labelSearchColumn As System.Windows.Forms.Label
        Friend WithEvents groupBoxRefiningParameters As System.Windows.Forms.GroupBox
        Friend WithEvents comboBoxRefiningColumn As System.Windows.Forms.ComboBox
        Friend WithEvents textBoxRefiningString As System.Windows.Forms.TextBox
        Friend WithEvents labelRefiningString As System.Windows.Forms.Label
        Friend WithEvents labelRefiningColumn As System.Windows.Forms.Label
        Friend WithEvents buttonOpenRefiningTable As System.Windows.Forms.Button
        Friend WithEvents textBoxRefiningTable As System.Windows.Forms.TextBox
        Friend WithEvents labelRefiningTable As System.Windows.Forms.Label
        Friend WithEvents groupBoxSearchResults As System.Windows.Forms.GroupBox
        Friend WithEvents labelMultipleMatchesFound As System.Windows.Forms.Label
        Friend WithEvents listBoxSearchResult As System.Windows.Forms.ListBox
        Friend WithEvents labelSearchResult As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents ComboBoxSearchColumn As System.Windows.Forms.ComboBox
        Friend WithEvents textBoxSearchString As System.Windows.Forms.TextBox
        Friend WithEvents checkBoxUseCloseMatches As System.Windows.Forms.CheckBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.textBoxSearchTable = New System.Windows.Forms.TextBox
            Me.textBoxMaxCloseMatches = New System.Windows.Forms.TextBox
            Me.buttonOpenSearchTable = New System.Windows.Forms.Button
            Me.groupBoxPreferences = New System.Windows.Forms.GroupBox
            Me.Label3 = New System.Windows.Forms.Label
            Me.checkBoxUseCloseMatches = New System.Windows.Forms.CheckBox
            Me.checkBoxAddressNumAfterStreet = New System.Windows.Forms.CheckBox
            Me.groupBoxSearchParameters = New System.Windows.Forms.GroupBox
            Me.Label4 = New System.Windows.Forms.Label
            Me.textBoxSearchString = New System.Windows.Forms.TextBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.ComboBoxSearchColumn = New System.Windows.Forms.ComboBox
            Me.checkBoxIntersection = New System.Windows.Forms.CheckBox
            Me.textBoxIntersection = New System.Windows.Forms.TextBox
            Me.labelSearchTable = New System.Windows.Forms.Label
            Me.labelMaxCloseMatches = New System.Windows.Forms.Label
            Me.buttonFind = New System.Windows.Forms.Button
            Me.labelSearchString = New System.Windows.Forms.Label
            Me.labelSearchColumn = New System.Windows.Forms.Label
            Me.groupBoxRefiningParameters = New System.Windows.Forms.GroupBox
            Me.comboBoxRefiningColumn = New System.Windows.Forms.ComboBox
            Me.textBoxRefiningString = New System.Windows.Forms.TextBox
            Me.labelRefiningString = New System.Windows.Forms.Label
            Me.labelRefiningColumn = New System.Windows.Forms.Label
            Me.buttonOpenRefiningTable = New System.Windows.Forms.Button
            Me.textBoxRefiningTable = New System.Windows.Forms.TextBox
            Me.labelRefiningTable = New System.Windows.Forms.Label
            Me.groupBoxSearchResults = New System.Windows.Forms.GroupBox
            Me.labelMultipleMatchesFound = New System.Windows.Forms.Label
            Me.listBoxSearchResult = New System.Windows.Forms.ListBox
            Me.labelSearchResult = New System.Windows.Forms.Label
            Me.groupBoxPreferences.SuspendLayout()
            Me.groupBoxSearchParameters.SuspendLayout()
            Me.groupBoxRefiningParameters.SuspendLayout()
            Me.groupBoxSearchResults.SuspendLayout()
            Me.SuspendLayout()
            '
            'textBoxSearchTable
            '
            Me.textBoxSearchTable.Location = New System.Drawing.Point(80, 59)
            Me.textBoxSearchTable.Name = "textBoxSearchTable"
            Me.textBoxSearchTable.Size = New System.Drawing.Size(120, 20)
            Me.textBoxSearchTable.TabIndex = 19
            Me.textBoxSearchTable.Text = ""
            '
            'textBoxMaxCloseMatches
            '
            Me.textBoxMaxCloseMatches.Location = New System.Drawing.Point(136, 267)
            Me.textBoxMaxCloseMatches.Name = "textBoxMaxCloseMatches"
            Me.textBoxMaxCloseMatches.Size = New System.Drawing.Size(24, 20)
            Me.textBoxMaxCloseMatches.TabIndex = 16
            Me.textBoxMaxCloseMatches.Text = ""
            '
            'buttonOpenSearchTable
            '
            Me.buttonOpenSearchTable.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.buttonOpenSearchTable.Location = New System.Drawing.Point(208, 59)
            Me.buttonOpenSearchTable.Name = "buttonOpenSearchTable"
            Me.buttonOpenSearchTable.TabIndex = 20
            Me.buttonOpenSearchTable.Text = "Open Table"
            '
            'groupBoxPreferences
            '
            Me.groupBoxPreferences.Controls.Add(Me.Label3)
            Me.groupBoxPreferences.Controls.Add(Me.checkBoxUseCloseMatches)
            Me.groupBoxPreferences.Controls.Add(Me.checkBoxAddressNumAfterStreet)
            Me.groupBoxPreferences.Location = New System.Drawing.Point(24, 227)
            Me.groupBoxPreferences.Name = "groupBoxPreferences"
            Me.groupBoxPreferences.Size = New System.Drawing.Size(168, 120)
            Me.groupBoxPreferences.TabIndex = 26
            Me.groupBoxPreferences.TabStop = False
            Me.groupBoxPreferences.Text = "Preferences"
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(8, 40)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(104, 23)
            Me.Label3.TabIndex = 12
            Me.Label3.Text = "Max close matches:"
            Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'checkBoxUseCloseMatches
            '
            Me.checkBoxUseCloseMatches.Location = New System.Drawing.Point(8, 16)
            Me.checkBoxUseCloseMatches.Name = "checkBoxUseCloseMatches"
            Me.checkBoxUseCloseMatches.Size = New System.Drawing.Size(128, 24)
            Me.checkBoxUseCloseMatches.TabIndex = 11
            Me.checkBoxUseCloseMatches.Text = "Use Close Matches"
            '
            'checkBoxAddressNumAfterStreet
            '
            Me.checkBoxAddressNumAfterStreet.Location = New System.Drawing.Point(8, 72)
            Me.checkBoxAddressNumAfterStreet.Name = "checkBoxAddressNumAfterStreet"
            Me.checkBoxAddressNumAfterStreet.Size = New System.Drawing.Size(144, 32)
            Me.checkBoxAddressNumAfterStreet.TabIndex = 0
            Me.checkBoxAddressNumAfterStreet.Text = "Address number after street"
            '
            'groupBoxSearchParameters
            '
            Me.groupBoxSearchParameters.Controls.Add(Me.Label4)
            Me.groupBoxSearchParameters.Controls.Add(Me.textBoxSearchString)
            Me.groupBoxSearchParameters.Controls.Add(Me.Label2)
            Me.groupBoxSearchParameters.Controls.Add(Me.Label1)
            Me.groupBoxSearchParameters.Controls.Add(Me.ComboBoxSearchColumn)
            Me.groupBoxSearchParameters.Controls.Add(Me.checkBoxIntersection)
            Me.groupBoxSearchParameters.Controls.Add(Me.textBoxIntersection)
            Me.groupBoxSearchParameters.Location = New System.Drawing.Point(24, 35)
            Me.groupBoxSearchParameters.Name = "groupBoxSearchParameters"
            Me.groupBoxSearchParameters.Size = New System.Drawing.Size(272, 184)
            Me.groupBoxSearchParameters.TabIndex = 15
            Me.groupBoxSearchParameters.TabStop = False
            Me.groupBoxSearchParameters.Text = "Search Parameters"
            '
            'Label4
            '
            Me.Label4.Location = New System.Drawing.Point(8, 88)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(40, 15)
            Me.Label4.TabIndex = 10
            Me.Label4.Text = "String:"
            '
            'textBoxSearchString
            '
            Me.textBoxSearchString.Location = New System.Drawing.Point(56, 88)
            Me.textBoxSearchString.Name = "textBoxSearchString"
            Me.textBoxSearchString.Size = New System.Drawing.Size(120, 20)
            Me.textBoxSearchString.TabIndex = 9
            Me.textBoxSearchString.Text = ""
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(8, 24)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(42, 23)
            Me.Label2.TabIndex = 8
            Me.Label2.Text = "Table:"
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 56)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(48, 23)
            Me.Label1.TabIndex = 7
            Me.Label1.Text = "Column:"
            '
            'ComboBoxSearchColumn
            '
            Me.ComboBoxSearchColumn.AllowDrop = True
            Me.ComboBoxSearchColumn.BackColor = System.Drawing.SystemColors.Window
            Me.ComboBoxSearchColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboBoxSearchColumn.ItemHeight = 13
            Me.ComboBoxSearchColumn.Location = New System.Drawing.Point(56, 56)
            Me.ComboBoxSearchColumn.Name = "ComboBoxSearchColumn"
            Me.ComboBoxSearchColumn.Size = New System.Drawing.Size(121, 21)
            Me.ComboBoxSearchColumn.TabIndex = 6
            '
            'checkBoxIntersection
            '
            Me.checkBoxIntersection.Location = New System.Drawing.Point(16, 120)
            Me.checkBoxIntersection.Name = "checkBoxIntersection"
            Me.checkBoxIntersection.Size = New System.Drawing.Size(160, 24)
            Me.checkBoxIntersection.TabIndex = 1
            Me.checkBoxIntersection.Text = "Use intersection:"
            '
            'textBoxIntersection
            '
            Me.textBoxIntersection.Enabled = False
            Me.textBoxIntersection.Location = New System.Drawing.Point(56, 152)
            Me.textBoxIntersection.Name = "textBoxIntersection"
            Me.textBoxIntersection.Size = New System.Drawing.Size(120, 20)
            Me.textBoxIntersection.TabIndex = 0
            Me.textBoxIntersection.Text = ""
            '
            'labelSearchTable
            '
            Me.labelSearchTable.Location = New System.Drawing.Point(32, 59)
            Me.labelSearchTable.Name = "labelSearchTable"
            Me.labelSearchTable.TabIndex = 17
            Me.labelSearchTable.Text = "Table:"
            '
            'labelMaxCloseMatches
            '
            Me.labelMaxCloseMatches.Location = New System.Drawing.Point(32, 267)
            Me.labelMaxCloseMatches.Name = "labelMaxCloseMatches"
            Me.labelMaxCloseMatches.Size = New System.Drawing.Size(104, 23)
            Me.labelMaxCloseMatches.TabIndex = 28
            Me.labelMaxCloseMatches.Text = "Max close matches:"
            Me.labelMaxCloseMatches.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'buttonFind
            '
            Me.buttonFind.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.buttonFind.Location = New System.Drawing.Point(24, 355)
            Me.buttonFind.Name = "buttonFind"
            Me.buttonFind.TabIndex = 18
            Me.buttonFind.Text = "Find"
            '
            'labelSearchString
            '
            Me.labelSearchString.Location = New System.Drawing.Point(32, 123)
            Me.labelSearchString.Name = "labelSearchString"
            Me.labelSearchString.TabIndex = 23
            Me.labelSearchString.Text = "String:"
            '
            'labelSearchColumn
            '
            Me.labelSearchColumn.Location = New System.Drawing.Point(32, 91)
            Me.labelSearchColumn.Name = "labelSearchColumn"
            Me.labelSearchColumn.TabIndex = 21
            Me.labelSearchColumn.Text = "Column:"
            '
            'groupBoxRefiningParameters
            '
            Me.groupBoxRefiningParameters.Controls.Add(Me.comboBoxRefiningColumn)
            Me.groupBoxRefiningParameters.Controls.Add(Me.textBoxRefiningString)
            Me.groupBoxRefiningParameters.Controls.Add(Me.labelRefiningString)
            Me.groupBoxRefiningParameters.Controls.Add(Me.labelRefiningColumn)
            Me.groupBoxRefiningParameters.Controls.Add(Me.buttonOpenRefiningTable)
            Me.groupBoxRefiningParameters.Controls.Add(Me.textBoxRefiningTable)
            Me.groupBoxRefiningParameters.Controls.Add(Me.labelRefiningTable)
            Me.groupBoxRefiningParameters.Location = New System.Drawing.Point(312, 35)
            Me.groupBoxRefiningParameters.Name = "groupBoxRefiningParameters"
            Me.groupBoxRefiningParameters.Size = New System.Drawing.Size(272, 184)
            Me.groupBoxRefiningParameters.TabIndex = 25
            Me.groupBoxRefiningParameters.TabStop = False
            Me.groupBoxRefiningParameters.Text = "Refining Parameters (Optional)"
            '
            'comboBoxRefiningColumn
            '
            Me.comboBoxRefiningColumn.ItemHeight = 13
            Me.comboBoxRefiningColumn.Location = New System.Drawing.Point(56, 56)
            Me.comboBoxRefiningColumn.Name = "comboBoxRefiningColumn"
            Me.comboBoxRefiningColumn.Size = New System.Drawing.Size(121, 21)
            Me.comboBoxRefiningColumn.TabIndex = 4
            '
            'textBoxRefiningString
            '
            Me.textBoxRefiningString.Location = New System.Drawing.Point(56, 88)
            Me.textBoxRefiningString.Name = "textBoxRefiningString"
            Me.textBoxRefiningString.Size = New System.Drawing.Size(120, 20)
            Me.textBoxRefiningString.TabIndex = 6
            Me.textBoxRefiningString.Text = ""
            '
            'labelRefiningString
            '
            Me.labelRefiningString.Location = New System.Drawing.Point(8, 88)
            Me.labelRefiningString.Name = "labelRefiningString"
            Me.labelRefiningString.TabIndex = 5
            Me.labelRefiningString.Text = "String:"
            '
            'labelRefiningColumn
            '
            Me.labelRefiningColumn.Location = New System.Drawing.Point(8, 56)
            Me.labelRefiningColumn.Name = "labelRefiningColumn"
            Me.labelRefiningColumn.TabIndex = 3
            Me.labelRefiningColumn.Text = "Column:"
            '
            'buttonOpenRefiningTable
            '
            Me.buttonOpenRefiningTable.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.buttonOpenRefiningTable.Location = New System.Drawing.Point(184, 24)
            Me.buttonOpenRefiningTable.Name = "buttonOpenRefiningTable"
            Me.buttonOpenRefiningTable.TabIndex = 2
            Me.buttonOpenRefiningTable.Text = "Open Table"
            '
            'textBoxRefiningTable
            '
            Me.textBoxRefiningTable.Location = New System.Drawing.Point(56, 24)
            Me.textBoxRefiningTable.Name = "textBoxRefiningTable"
            Me.textBoxRefiningTable.Size = New System.Drawing.Size(120, 20)
            Me.textBoxRefiningTable.TabIndex = 1
            Me.textBoxRefiningTable.Text = ""
            '
            'labelRefiningTable
            '
            Me.labelRefiningTable.Location = New System.Drawing.Point(8, 24)
            Me.labelRefiningTable.Name = "labelRefiningTable"
            Me.labelRefiningTable.TabIndex = 0
            Me.labelRefiningTable.Text = "Table:"
            '
            'groupBoxSearchResults
            '
            Me.groupBoxSearchResults.Controls.Add(Me.labelMultipleMatchesFound)
            Me.groupBoxSearchResults.Controls.Add(Me.listBoxSearchResult)
            Me.groupBoxSearchResults.Controls.Add(Me.labelSearchResult)
            Me.groupBoxSearchResults.Location = New System.Drawing.Point(200, 227)
            Me.groupBoxSearchResults.Name = "groupBoxSearchResults"
            Me.groupBoxSearchResults.Size = New System.Drawing.Size(384, 120)
            Me.groupBoxSearchResults.TabIndex = 29
            Me.groupBoxSearchResults.TabStop = False
            Me.groupBoxSearchResults.Text = "Search Results"
            '
            'labelMultipleMatchesFound
            '
            Me.labelMultipleMatchesFound.Location = New System.Drawing.Point(8, 56)
            Me.labelMultipleMatchesFound.Name = "labelMultipleMatchesFound"
            Me.labelMultipleMatchesFound.Size = New System.Drawing.Size(144, 23)
            Me.labelMultipleMatchesFound.TabIndex = 1
            Me.labelMultipleMatchesFound.Text = "Multiple matches found"
            Me.labelMultipleMatchesFound.Visible = False
            '
            'listBoxSearchResult
            '
            Me.listBoxSearchResult.Location = New System.Drawing.Point(160, 24)
            Me.listBoxSearchResult.Name = "listBoxSearchResult"
            Me.listBoxSearchResult.Size = New System.Drawing.Size(208, 82)
            Me.listBoxSearchResult.TabIndex = 0
            '
            'labelSearchResult
            '
            Me.labelSearchResult.Location = New System.Drawing.Point(16, 24)
            Me.labelSearchResult.Name = "labelSearchResult"
            Me.labelSearchResult.Size = New System.Drawing.Size(136, 24)
            Me.labelSearchResult.TabIndex = 0
            '
            'FindSampleForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 413)
            Me.Controls.Add(Me.textBoxSearchTable)
            Me.Controls.Add(Me.textBoxMaxCloseMatches)
            Me.Controls.Add(Me.buttonOpenSearchTable)
            Me.Controls.Add(Me.groupBoxPreferences)
            Me.Controls.Add(Me.groupBoxSearchParameters)
            Me.Controls.Add(Me.labelSearchTable)
            Me.Controls.Add(Me.labelMaxCloseMatches)
            Me.Controls.Add(Me.buttonFind)
            Me.Controls.Add(Me.labelSearchString)
            Me.Controls.Add(Me.labelSearchColumn)
            Me.Controls.Add(Me.groupBoxRefiningParameters)
            Me.Controls.Add(Me.groupBoxSearchResults)
            Me.Name = "FindSampleForm"
            Me.Text = "Find Example"
            Me.groupBoxPreferences.ResumeLayout(False)
            Me.groupBoxSearchParameters.ResumeLayout(False)
            Me.groupBoxRefiningParameters.ResumeLayout(False)
            Me.groupBoxSearchResults.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Sub CleanUp()
            If Not (_miCommand Is Nothing) Then
                _miCommand.Dispose()
                _miCommand = Nothing
            End If
            If Not (_miConnection Is Nothing) Then
                _miConnection.Close()
                _miConnection = Nothing
            End If
            Session.Current.Catalog.CloseAll()
            Session.Dispose()
        End Sub 'CleanUp

        Private Sub buttonOpenSearchTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOpenSearchTable.Click
            Dim searchTable As Table
            If OpenTable(searchTable) Then
                If searchTable Is Nothing Then
                    MessageBox.Show("Please specify a valid search table.")
                    Return
                End If
                If searchTable.IsMappable Then
                    _searchTable = searchTable
                    '				searchTable.Close()
                    textBoxSearchTable.Text = ""
                    comboBoxSearchColumn.Items.Clear()
                    comboBoxSearchColumn.Text = ""
                    textBoxSearchString.Text = ""
                    textBoxSearchTable.Text = _searchTable.TableInfo.Alias
                    SetColumnField(_searchTable, comboBoxSearchColumn)
                Else
                    MessageBox.Show([String].Format(("Table " + _searchTable.Alias + " is not mappable.")))
                End If
            End If
        End Sub 'buttonOpenSearchTable_Click


        Private Sub SetColumnField(ByVal table As Table, ByVal comboBox As System.Windows.Forms.ComboBox)
            Dim columns As Columns = table.TableInfo.Columns
            Dim column As Column
            For Each column In columns
                If column.Indexed Then
                    comboBox.Items.Add(column.Alias.ToString)
                End If
            Next column

            If comboBox.Items.Count > 0 Then
                comboBox.SelectedIndex = 0
            Else
                MessageBox.Show([String].Format(("No indexed columns in " + table.Alias + ".")))
            End If
        End Sub 'SetColumnField


        Private Sub textBoxSearchString_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles textBoxSearchString.TextChanged
            enableButtonFind()
        End Sub 'textBoxSearchString_TextChanged


        Private Sub comboBoxSearchColumn_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles comboBoxSearchColumn.TextChanged
            enableButtonFind()
        End Sub 'comboBoxSearchColumn_TextChanged


        Private Sub enableButtonFind()
            If textBoxSearchString.Text.Length > 0 And textBoxSearchTable.Text.Length > 0 And comboBoxSearchColumn.Items.Count > 0 Then
                buttonFind.Enabled = True
            Else
                buttonFind.Enabled = False
            End If
        End Sub 'enableButtonFind


        Private Sub buttonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFind.Click
            listBoxSearchResult.Items.Clear()
            listBoxSearchResult.Visible = False
            Dim find As MapInfo.Data.Find.Find = Nothing
            Dim searchColumn As Column = _searchTable.TableInfo.Columns(comboBoxSearchColumn.SelectedItem.ToString())
            If Not (_refiningTable Is Nothing) Then
                If _refiningTable.IsOpen Then
                    If comboBoxRefiningColumn.Items.Count > 0 Then
                        Dim refiningColumn As Column = _refiningTable.TableInfo.Columns(comboBoxRefiningColumn.SelectedItem.ToString())
                        find = New MapInfo.Data.Find.Find(_searchTable, searchColumn, _refiningTable, refiningColumn)
                    Else
                        MessageBox.Show([String].Format(("No indexed columns in " & _refiningTable.ToString & ".")))
                        Return
                    End If
                Else
                    find = New MapInfo.Data.Find.Find(_searchTable, searchColumn)
                End If
            Else
                find = New MapInfo.Data.Find.Find(_searchTable, searchColumn)
            End If

            If checkBoxUseCloseMatches.Checked Then
                find.UseCloseMatches = True
                find.CloseMatchesMax = Integer.Parse(textBoxMaxCloseMatches.Text)
            End If

            If checkBoxAddressNumAfterStreet.Checked Then
                find.AddressNumberAfterStreet = True
            End If

            ' Do the actual search.
            If Not (_refiningTable Is Nothing) Then
                If _refiningTable.IsOpen Then
                    If _bSearchIntersection Then
                        _result = find.SearchIntersection(textBoxSearchString.Text, textBoxIntersection.Text, textBoxRefiningString.Text)
                    Else
                        _result = find.Search(textBoxSearchString.Text, textBoxRefiningString.Text)
                    End If
                Else
                    If _bSearchIntersection Then
                        _result = find.SearchIntersection(textBoxSearchString.Text, textBoxIntersection.Text)
                    Else
                        _result = find.Search(textBoxSearchString.Text)
                    End If
                End If
            Else
                If _bSearchIntersection Then
                    _result = find.SearchIntersection(textBoxSearchString.Text, textBoxIntersection.Text)
                Else
                    _result = find.Search(textBoxSearchString.Text)
                End If
            End If

            ' display label that tells us when multiple matches were found
            labelMultipleMatchesFound.Visible = _result.MultipleMatches

            If _result.ExactMatch And _result.NameResultCode.Equals(FindNameCode.ExactMatch) And Not (_result.FoundPoint Is Nothing) Then
                labelSearchResult.Text = "Exact Match"
                showPointOnSearchTableMap(_result.FoundPoint.X, _result.FoundPoint.Y)
            Else
                If _result.NameResultCode.Equals(FindNameCode.ExactMatch) And _result.AddressResultCode.Equals(FindAddressCode.AddressNumNotSpecified) Then
                    labelSearchResult.Text = _result.AddressResultCode.ToString()
                    Dim _enum As FindAddressRangeEnumerator = _result.GetAddressRangeEnumerator()
                    Dim _findAddressRange As FindAddressRange

                    listBoxSearchResult.Visible = True
                    While _enum.MoveNext()
                        _findAddressRange = _enum.Current
                        listBoxSearchResult.Items.Add([String].Format("Address range: [{0} - {1}]", _findAddressRange.BeginRange, _findAddressRange.EndRange))
                    End While
                ElseIf _result.NameResultCode.Equals(FindNameCode.ExactMatch) And _result.MultipleMatches Then
                    labelSearchResult.Text = _result.NameResultCode.ToString()
                    listBoxSearchResult.Visible = True
                    Dim enumerator As FindCloseMatchEnumerator = _result.GetCloseMatchEnumerator()
                    While enumerator.MoveNext()
                        listBoxSearchResult.Items.Add(enumerator.Current.Name)
                    End While
                Else
                    labelSearchResult.Text = _result.NameResultCode.ToString()
                    If find.UseCloseMatches Then
                        listBoxSearchResult.Visible = True
                        Dim enumerator As FindCloseMatchEnumerator = _result.GetCloseMatchEnumerator()
                        While enumerator.MoveNext()
                            listBoxSearchResult.Items.Add(enumerator.Current.Name)
                        End While
                    End If
                End If
            End If
            find.Dispose()
        End Sub 'buttonFind_Click


        Private Sub showPointOnSearchTableMap(ByVal x As Double, ByVal y As Double)
            Dim map As MapInfo.Mapping.Map = Session.Current.MapFactory.CreateEmptyMap(System.IntPtr.Zero, New Size(10, 10))
            Dim searchLayer As New MapInfo.Mapping.FeatureLayer(_searchTable)
            map.Layers.Add(searchLayer)

            Dim mapForm As New MapForm1(map)
            Dim parentForm As Form = Me

            ' create a temp table and add a featurelayer for it (use map alias as table alias)
            ' make the table hidden (maybe)
            Dim coordSys As MapInfo.Geometry.CoordSys = map.GetDisplayCoordSys()
            Dim ti As New TableInfoMemTable("temp")
            ti.Temporary = True

            ' add object column
            Dim col As Column
            col = New GeometryColumn(coordSys) ' specify coordsys for object column
            col.Alias = "obj"
            col.DataType = MIDbType.FeatureGeometry
            ti.Columns.Add(col)

            ' add style column
            col = New Column
            col.Alias = "MI_Style"
            col.DataType = MIDbType.Style
            ti.Columns.Add(col)

            Dim pointTable As Table = Session.Current.Catalog.CreateTable(ti)

            ' I am using a Point example here. You can create a rectangle instead
            Dim g As New MapInfo.Geometry.Point(coordSys, x, y)
            Dim vs As New MapInfo.Styles.SimpleVectorPointStyle(37, System.Drawing.Color.Red, 14)
            Dim cs As New MapInfo.Styles.CompositeStyle(vs)

            Dim cmd As MICommand = _miConnection.CreateCommand()
            cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry)
            cmd.Parameters.Add("style", MIDbType.Style)
            cmd.CommandText = "Insert Into temp (obj,MI_Style) values (geometry,style)"
            cmd.Prepare()
            cmd.Parameters(0).Value = g
            cmd.Parameters(1).Value = cs
            Dim nchanged As Integer = cmd.ExecuteNonQuery()
            cmd.Dispose()

            map.Layers.Add(New MapInfo.Mapping.FeatureLayer(pointTable))
            ' another way: Map.Load(new MapTableLoader(table));
            ' make the map encompass the entire search layer
            map.SetView(searchLayer)

            ' size the map form
            mapForm.Size = New Size(500, 500)

            'Show the form like a dialog (modal)			
            mapForm.ShowDialog(parentForm)

            pointTable.Close()
        End Sub 'showPointOnSearchTableMap


        Private Sub checkBoxUseCloseMatches_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkBoxUseCloseMatches.CheckedChanged
            If checkBoxUseCloseMatches.Checked Then
                labelMaxCloseMatches.Enabled = True
                textBoxMaxCloseMatches.Enabled = True
            Else
                labelMaxCloseMatches.Enabled = False
                textBoxMaxCloseMatches.Enabled = False
            End If
        End Sub 'checkBoxUseCloseMatches_CheckedChanged


        Private Sub textBoxMaxCloseMatches_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles textBoxMaxCloseMatches.Leave
            Try
                Dim closeMatches As Integer = Integer.Parse(textBoxMaxCloseMatches.Text)
            Catch
            End Try
        End Sub 'textBoxMaxCloseMatches_Leave


        ' returns true when a table was attempted to be opened, false when OpenFileDialog cancelled.
        Private Function OpenTable(ByRef table As Table) As Boolean
            Dim filename As String = Nothing
            Dim s As String
            Dim openFile As New OpenFileDialog
            openFile.DefaultExt = "tab"
            ' The Filter property requires a search string after the pipe ( | )
            openFile.Filter = "MapInfo Tables (*.tab)|*.tab"
            openFile.Multiselect = False
            table = Nothing
            If openFile.ShowDialog() = DialogResult.OK Then
                If openFile.FileName.Length > 0 Then
                    filename = openFile.FileName
                    '				MessageBox.Show(filename);
                    If Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, s) Then
                        '					MessageBox.Show("file exists");
                        table = Session.Current.Catalog.OpenTable(s)
                    Else
                        ' try anyway, at least we will get an exception to report
                        table = Session.Current.Catalog.OpenTable(filename)
                    End If
                    Return True
                End If
            End If
            Return False
        End Function 'OpenTable

        Private Sub buttonOpenRefiningTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonOpenRefiningTable.Click
            Dim refiningTable As Table

            If OpenTable(refiningTable) Then
                If refiningTable Is Nothing Then
                    MessageBox.Show("Please specify a valid refining table.")
                    Return
                End If
                If refiningTable.IsMappable Then
                    _refiningTable = refiningTable
                    textBoxRefiningTable.Text = ""
                    comboBoxRefiningColumn.Items.Clear()
                    comboBoxRefiningColumn.Text = ""
                    textBoxRefiningString.Text = ""
                    textBoxRefiningTable.Text = _refiningTable.TableInfo.Alias
                    SetColumnField(_refiningTable, comboBoxRefiningColumn)
                Else
                    MessageBox.Show([String].Format(("Table " + _refiningTable.Alias + " is not mappable.")))
                End If
            End If
        End Sub 'buttonOpenRefiningTable_Click


        Private Sub listBoxSearchResult_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles listBoxSearchResult.DoubleClick
            ' get the selected object
            Dim selectedCloseMatch As String = CStr(Me.listBoxSearchResult.SelectedItem)
            ' get the selected object index in the list as well since multiple matches
            ' may have the same name - we can't just compare close match names.
            Dim selectedIndex As Integer = Me.listBoxSearchResult.SelectedIndex
            Dim currentIndex As Integer = 0

            ' create a close match object
            Dim closeMatch As FindCloseMatch = Nothing

            ' create an enumerator from the results
            Dim enumerator As FindCloseMatchEnumerator = _result.GetCloseMatchEnumerator()
            While enumerator.MoveNext()
                ' if the selected name equals the enumerated name...
                If currentIndex = selectedIndex And selectedCloseMatch.Equals(enumerator.Current.Name) Then
                    ' set the close match object
                    closeMatch = enumerator.Current

                    ' break out of the loop
                    Exit While
                End If
                ' else keep looking
                currentIndex += 1
            End While
            If Not (closeMatch Is Nothing) Then
                ' create the command string
                Dim command As String = "select obj from " & _searchTable.Alias & " where MI_Key = '" & closeMatch.Key.ToString & "'"

                ' create the command object
                Dim cmd As MICommand = _miConnection.CreateCommand()
                cmd.CommandText = command

                ' create the reader by executing the command
                Dim rdr As MIDataReader = cmd.ExecuteReader()

                ' read a row
                rdr.Read()

                ' get the point
                Dim point As MapInfo.Geometry.DPoint = rdr.GetFeatureGeometry(0).Centroid

                ' Close the reader and dispose of the command.
                rdr.Close()
                cmd.Cancel()
                cmd.Dispose()

                ' show point on a map
                showPointOnSearchTableMap(point.x, point.y)
            End If
        End Sub 'listBoxSearchResult_DoubleClick


        Private Sub checkBoxIntersection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkBoxIntersection.CheckedChanged
            textBoxIntersection.Enabled = checkBoxIntersection.Checked
            _bSearchIntersection = checkBoxIntersection.Checked
        End Sub 'checkBoxIntersection_CheckedChanged



    End Class
End Namespace 'FindVB