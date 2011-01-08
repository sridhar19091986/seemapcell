Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports MapInfo.Data
Imports MapInfo.Engine
Imports MapInfo.Geometry
Imports MapInfo.Mapping


Namespace MIDataExplorer
    '/ <summary>
    '/ This is the main SDI application form
    '/ </summary>
    Public Class MainForm
        Inherits System.Windows.Forms.Form

        Private _output As New System.IO.StringWriter  '

        Private statusBar1 As System.Windows.Forms.StatusBar
        Private splitter1 As System.Windows.Forms.Splitter
        Private panel1 As System.Windows.Forms.Panel
        Private WithEvents toolBar1 As System.Windows.Forms.ToolBar
        Private splitter2 As System.Windows.Forms.Splitter
        Private mainMenu1 As System.Windows.Forms.MainMenu
        Private menuFile As System.Windows.Forms.MenuItem
        Private WithEvents menuOpenTables As System.Windows.Forms.MenuItem
        Private splitter3 As System.Windows.Forms.Splitter
        Private WithEvents treeViewTables As System.Windows.Forms.TreeView
        Private rtCommand As System.Windows.Forms.RichTextBox
        Private rtResults As System.Windows.Forms.RichTextBox
        Private dataGrid As System.Windows.Forms.DataGrid
        Private tbExecute As System.Windows.Forms.ToolBarButton
        Private tbSep1 As System.Windows.Forms.ToolBarButton
        Private components As System.ComponentModel.IContainer
        Private WithEvents menuExecute As System.Windows.Forms.MenuItem
        Private menuItem2 As System.Windows.Forms.MenuItem
        Private WithEvents menuFileExit As System.Windows.Forms.MenuItem
        Private WithEvents contextMenuTables As System.Windows.Forms.ContextMenu
        Private WithEvents menuSelectNode As System.Windows.Forms.MenuItem
        Private WithEvents menuCloseNode As System.Windows.Forms.MenuItem
        Private WithEvents menuCloseAll As System.Windows.Forms.MenuItem
        Private contextMenuCommand As System.Windows.Forms.ContextMenu
        Private WithEvents menuCommandExecute As System.Windows.Forms.MenuItem
        Private WithEvents menuCommandClear As System.Windows.Forms.MenuItem
        Private WithEvents menuOpenCommandFile As System.Windows.Forms.MenuItem
        Private WithEvents menuSaveAs As System.Windows.Forms.MenuItem
        Private WithEvents menuItem5 As System.Windows.Forms.MenuItem
        Private WithEvents menuCommand As System.Windows.Forms.MenuItem
        Private WithEvents menuView As System.Windows.Forms.MenuItem
        Private WithEvents menuViewClearCommandWindow As System.Windows.Forms.MenuItem
        Private WithEvents menuViewClearTextResults As System.Windows.Forms.MenuItem
        Private WithEvents menuViewClearGridResults As System.Windows.Forms.MenuItem
        Private WithEvents contextMenuTextResults As System.Windows.Forms.ContextMenu
        Private WithEvents menuClearTextResults As System.Windows.Forms.MenuItem
        Private WithEvents menuTableSchema As System.Windows.Forms.MenuItem
        Private WithEvents menuCommandExecuteAll As System.Windows.Forms.MenuItem
        Private WithEvents tbOpen As System.Windows.Forms.ToolBarButton
        Private WithEvents tbSave As System.Windows.Forms.ToolBarButton
        Private WithEvents tbSep2 As System.Windows.Forms.ToolBarButton
        Private WithEvents menuTableCloseAll As System.Windows.Forms.MenuItem
        Private WithEvents menuListCommands As System.Windows.Forms.MenuItem
        Private menuItem3 As System.Windows.Forms.MenuItem
        Private WithEvents menuItemTableSearchPath As System.Windows.Forms.MenuItem
        Private miCommand As miCommand
        Private miConnection As miConnection

        Public Sub New()  '
            '
            ' Required for Windows Form Designer support
            '
            InitializeComponent()

            ToolBarSetup()

            '
            ' Create a connection object and open it.
            '
            miConnection = New miConnection
            miConnection.Open()

            '
            ' Create a command object to hold parameters and last command executed
            '
            miCommand = miConnection.CreateCommand()

            ' Add a few default parameters
            Dim CSysFactory As CoordSysFactory = Session.Current.CoordSysFactory
            Dim Robinson As CoordSys = CSysFactory.CreateFromPrjString("12, 62, 7, 0")
            Dim LatLong As CoordSys = CSysFactory.CreateFromPrjString("1, 0")
            Dim NAD83 As CoordSys = CSysFactory.CreateFromPrjString("1, 74")
            miCommand.Parameters.Add("RobinsonCSys", Robinson)
            miCommand.Parameters.Add("LatLongCSys", LatLong)
            miCommand.Parameters.Add("NAD83CSys", NAD83)


            ' Set table search path to value sampledatasearch registry key
            ' if not found, then use the sampledatasearchpath
            ' if that is not found, then just use the app's current directory
            Dim keyApp As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\MapInfo\MIDataExplorer")

            Dim s As String = ""
            s = CType(keyApp.GetValue("TableSearchPath"), String)
            If s Is Nothing Then
                Dim keySamp As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\MapInfo\Data")
                s = CType(keySamp.GetValue("SampleDataSearchPath"), String)
                If s <> Nothing Then
                    If s.EndsWith("\\") = False Then
                        s += "\\"
                    End If
                Else
                    s = Environment.CurrentDirectory
                End If
                keySamp.Close()
            End If
            keyApp.Close()

            Session.Current.TableSearchPath.Path = s

            ProcessCommand("help")

            ' For sample app purposes, open states.tab
            If Session.Current.TableSearchPath.FileExists("usa.tab") Then
                ProcessCommand("open usa.tab")
                ' For sample app purposes, load states.sql if we can find it.
                Dim sFound As String
                If Session.Current.TableSearchPath.FileExists("sample.sql", sFound) Then
                    rtCommand.LoadFile(sFound, RichTextBoxStreamType.PlainText)
                Else
                    ' could not open a command file, but we want to show something
                    ProcessCommand("select * from usa")
                End If
            End If
        End Sub  'New

        Protected Overloads Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                If (Not components Is Nothing) Then
                    components.Dispose()
                End If
                miCommand.Cancel()
                miCommand.Dispose()
                miConnection.Close()
            End If
            MyBase.Dispose(disposing)
        End Sub
        
        '/ Required method for Designer support - do not modify
        '/ the contents of this method with the code editor.
        '/ </summary>
        Private Sub InitializeComponent()
            Me.statusBar1 = New System.Windows.Forms.StatusBar
            Me.treeViewTables = New System.Windows.Forms.TreeView
            Me.contextMenuTables = New System.Windows.Forms.ContextMenu
            Me.menuSelectNode = New System.Windows.Forms.MenuItem
            Me.menuTableSchema = New System.Windows.Forms.MenuItem
            Me.menuCloseNode = New System.Windows.Forms.MenuItem
            Me.menuTableCloseAll = New System.Windows.Forms.MenuItem
            Me.splitter1 = New System.Windows.Forms.Splitter
            Me.panel1 = New System.Windows.Forms.Panel
            Me.dataGrid = New System.Windows.Forms.DataGrid
            Me.splitter3 = New System.Windows.Forms.Splitter
            Me.rtResults = New System.Windows.Forms.RichTextBox
            Me.contextMenuTextResults = New System.Windows.Forms.ContextMenu
            Me.menuClearTextResults = New System.Windows.Forms.MenuItem
            Me.splitter2 = New System.Windows.Forms.Splitter
            Me.rtCommand = New System.Windows.Forms.RichTextBox
            Me.contextMenuCommand = New System.Windows.Forms.ContextMenu
            Me.menuCommandExecute = New System.Windows.Forms.MenuItem
            Me.menuCommandClear = New System.Windows.Forms.MenuItem
            Me.toolBar1 = New System.Windows.Forms.ToolBar
            Me.tbOpen = New System.Windows.Forms.ToolBarButton
            Me.tbSave = New System.Windows.Forms.ToolBarButton
            Me.tbSep1 = New System.Windows.Forms.ToolBarButton
            Me.tbSep2 = New System.Windows.Forms.ToolBarButton
            Me.tbExecute = New System.Windows.Forms.ToolBarButton
            Me.mainMenu1 = New System.Windows.Forms.MainMenu
            Me.menuFile = New System.Windows.Forms.MenuItem
            Me.menuOpenCommandFile = New System.Windows.Forms.MenuItem
            Me.menuOpenTables = New System.Windows.Forms.MenuItem
            Me.menuSaveAs = New System.Windows.Forms.MenuItem
            Me.menuItem5 = New System.Windows.Forms.MenuItem
            Me.menuItemTableSearchPath = New System.Windows.Forms.MenuItem
            Me.menuCloseAll = New System.Windows.Forms.MenuItem
            Me.menuItem2 = New System.Windows.Forms.MenuItem
            Me.menuFileExit = New System.Windows.Forms.MenuItem
            Me.menuView = New System.Windows.Forms.MenuItem
            Me.menuViewClearCommandWindow = New System.Windows.Forms.MenuItem
            Me.menuViewClearTextResults = New System.Windows.Forms.MenuItem
            Me.menuViewClearGridResults = New System.Windows.Forms.MenuItem
            Me.menuCommand = New System.Windows.Forms.MenuItem
            Me.menuListCommands = New System.Windows.Forms.MenuItem
            Me.menuItem3 = New System.Windows.Forms.MenuItem
            Me.menuExecute = New System.Windows.Forms.MenuItem
            Me.menuCommandExecuteAll = New System.Windows.Forms.MenuItem
            Me.panel1.SuspendLayout()
            CType(Me.dataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'statusBar1
            '
            Me.statusBar1.Location = New System.Drawing.Point(0, 448)
            Me.statusBar1.Name = "statusBar1"
            Me.statusBar1.Size = New System.Drawing.Size(712, 22)
            Me.statusBar1.TabIndex = 2
            Me.statusBar1.Text = "statusBar1"
            '
            'treeViewTables
            '
            Me.treeViewTables.ContextMenu = Me.contextMenuTables
            Me.treeViewTables.Dock = System.Windows.Forms.DockStyle.Left
            Me.treeViewTables.HideSelection = False
            Me.treeViewTables.ImageIndex = -1
            Me.treeViewTables.Location = New System.Drawing.Point(0, 28)
            Me.treeViewTables.Name = "treeViewTables"
            Me.treeViewTables.SelectedImageIndex = -1
            Me.treeViewTables.Size = New System.Drawing.Size(168, 420)
            Me.treeViewTables.TabIndex = 3
            '
            'contextMenuTables
            '
            Me.contextMenuTables.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuSelectNode, Me.menuTableSchema, Me.menuCloseNode, Me.menuTableCloseAll})
            '
            'menuSelectNode
            '
            Me.menuSelectNode.DefaultItem = True
            Me.menuSelectNode.Index = 0
            Me.menuSelectNode.Text = "&Select"
            '
            'menuTableSchema
            '
            Me.menuTableSchema.Index = 1
            Me.menuTableSchema.Text = "Sc&hema"
            '
            'menuCloseNode
            '
            Me.menuCloseNode.Index = 2
            Me.menuCloseNode.Text = "&Close"
            '
            'menuTableCloseAll
            '
            Me.menuTableCloseAll.Index = 3
            Me.menuTableCloseAll.Text = "Close &All"
            '
            'splitter1
            '
            Me.splitter1.Location = New System.Drawing.Point(168, 28)
            Me.splitter1.Name = "splitter1"
            Me.splitter1.Size = New System.Drawing.Size(3, 420)
            Me.splitter1.TabIndex = 4
            Me.splitter1.TabStop = False
            '
            'panel1
            '
            Me.panel1.Controls.Add(Me.dataGrid)
            Me.panel1.Controls.Add(Me.splitter3)
            Me.panel1.Controls.Add(Me.rtResults)
            Me.panel1.Controls.Add(Me.splitter2)
            Me.panel1.Controls.Add(Me.rtCommand)
            Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panel1.Location = New System.Drawing.Point(171, 28)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(541, 420)
            Me.panel1.TabIndex = 5
            '
            'dataGrid
            '
            Me.dataGrid.AlternatingBackColor = System.Drawing.SystemColors.InactiveCaptionText
            Me.dataGrid.DataMember = ""
            Me.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.dataGrid.Location = New System.Drawing.Point(0, 195)
            Me.dataGrid.Name = "dataGrid"
            Me.dataGrid.ReadOnly = True
            Me.dataGrid.Size = New System.Drawing.Size(541, 225)
            Me.dataGrid.TabIndex = 5
            '
            'splitter3
            '
            Me.splitter3.Dock = System.Windows.Forms.DockStyle.Top
            Me.splitter3.Location = New System.Drawing.Point(0, 192)
            Me.splitter3.Name = "splitter3"
            Me.splitter3.Size = New System.Drawing.Size(541, 3)
            Me.splitter3.TabIndex = 4
            Me.splitter3.TabStop = False
            '
            'rtResults
            '
            Me.rtResults.ContextMenu = Me.contextMenuTextResults
            Me.rtResults.DetectUrls = False
            Me.rtResults.Dock = System.Windows.Forms.DockStyle.Top
            Me.rtResults.HideSelection = False
            Me.rtResults.Location = New System.Drawing.Point(0, 88)
            Me.rtResults.Name = "rtResults"
            Me.rtResults.ReadOnly = True
            Me.rtResults.Size = New System.Drawing.Size(541, 104)
            Me.rtResults.TabIndex = 3
            Me.rtResults.Text = ""
            Me.rtResults.WordWrap = False
            '
            'contextMenuTextResults
            '
            Me.contextMenuTextResults.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuClearTextResults})
            '
            'menuClearTextResults
            '
            Me.menuClearTextResults.Index = 0
            Me.menuClearTextResults.Text = "&Clear"
            '
            'splitter2
            '
            Me.splitter2.Dock = System.Windows.Forms.DockStyle.Top
            Me.splitter2.Location = New System.Drawing.Point(0, 80)
            Me.splitter2.Name = "splitter2"
            Me.splitter2.Size = New System.Drawing.Size(541, 8)
            Me.splitter2.TabIndex = 2
            Me.splitter2.TabStop = False
            '
            'rtCommand
            '
            Me.rtCommand.AutoWordSelection = True
            Me.rtCommand.CausesValidation = False
            Me.rtCommand.ContextMenu = Me.contextMenuCommand
            Me.rtCommand.Dock = System.Windows.Forms.DockStyle.Top
            Me.rtCommand.HideSelection = False
            Me.rtCommand.Location = New System.Drawing.Point(0, 0)
            Me.rtCommand.Name = "rtCommand"
            Me.rtCommand.ShowSelectionMargin = True
            Me.rtCommand.Size = New System.Drawing.Size(541, 80)
            Me.rtCommand.TabIndex = 0
            Me.rtCommand.Text = "#Enter commands or open script file"
            Me.rtCommand.WordWrap = False
            '
            'contextMenuCommand
            '
            Me.contextMenuCommand.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuCommandExecute, Me.menuCommandClear})
            '
            'menuCommandExecute
            '
            Me.menuCommandExecute.Index = 0
            Me.menuCommandExecute.Text = ""
            '
            'menuCommandClear
            '
            Me.menuCommandClear.Index = 1
            Me.menuCommandClear.Text = "&Clear"
            '
            'toolBar1
            '
            Me.toolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbOpen, Me.tbSave, Me.tbSep1, Me.tbSep2, Me.tbExecute})
            Me.toolBar1.DropDownArrows = True
            Me.toolBar1.Location = New System.Drawing.Point(0, 0)
            Me.toolBar1.Name = "toolBar1"
            Me.toolBar1.ShowToolTips = True
            Me.toolBar1.Size = New System.Drawing.Size(712, 28)
            Me.toolBar1.TabIndex = 6
            '
            'tbOpen
            '
            Me.tbOpen.ImageIndex = 0
            Me.tbOpen.ToolTipText = "Open Command File"
            '
            'tbSave
            '
            Me.tbSave.ToolTipText = "Save Command Window to File"
            '
            'tbSep1
            '
            Me.tbSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
            '
            'tbSep2
            '
            Me.tbSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
            '
            'tbExecute
            '
            Me.tbExecute.ToolTipText = "Execute Current Command"
            '
            'mainMenu1
            '
            Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuFile, Me.menuView, Me.menuCommand})
            '
            'menuFile
            '
            Me.menuFile.Index = 0
            Me.menuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuOpenCommandFile, Me.menuOpenTables, Me.menuSaveAs, Me.menuItem5, Me.menuItemTableSearchPath, Me.menuCloseAll, Me.menuItem2, Me.menuFileExit})
            Me.menuFile.Text = "&File"
            '
            'menuOpenCommandFile
            '
            Me.menuOpenCommandFile.Index = 0
            Me.menuOpenCommandFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO
            Me.menuOpenCommandFile.Text = "&Open..."
            '
            'menuOpenTables
            '
            Me.menuOpenTables.Index = 1
            Me.menuOpenTables.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO
            Me.menuOpenTables.Text = "Open &Tables..."
            '
            'menuSaveAs
            '
            Me.menuSaveAs.Index = 2
            Me.menuSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlS
            Me.menuSaveAs.Text = "Save As..."
            '
            'menuItem5
            '
            Me.menuItem5.Index = 3
            Me.menuItem5.Text = "-"
            '
            'menuItemTableSearchPath
            '
            Me.menuItemTableSearchPath.Index = 4
            Me.menuItemTableSearchPath.Text = "Table Search Path..."
            '
            'menuCloseAll
            '
            Me.menuCloseAll.Index = 5
            Me.menuCloseAll.Text = "Close &All Tables"
            '
            'menuItem2
            '
            Me.menuItem2.Index = 6
            Me.menuItem2.Text = "-"
            '
            'menuFileExit
            '
            Me.menuFileExit.Index = 7
            Me.menuFileExit.Text = "E&xit"
            '
            'menuView
            '
            Me.menuView.Index = 1
            Me.menuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuViewClearCommandWindow, Me.menuViewClearTextResults, Me.menuViewClearGridResults})
            Me.menuView.Text = "&View"
            '
            'menuViewClearCommandWindow
            '
            Me.menuViewClearCommandWindow.Index = 0
            Me.menuViewClearCommandWindow.Text = "Clear &Commands"
            '
            'menuViewClearTextResults
            '
            Me.menuViewClearTextResults.Index = 1
            Me.menuViewClearTextResults.Text = "Clear Text Results"
            '
            'menuViewClearGridResults
            '
            Me.menuViewClearGridResults.Index = 2
            Me.menuViewClearGridResults.Text = "Clear &Grid Results"
            '
            'menuCommand
            '
            Me.menuCommand.Index = 2
            Me.menuCommand.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuListCommands, Me.menuItem3, Me.menuExecute, Me.menuCommandExecuteAll})
            Me.menuCommand.Text = "&Command"
            '
            'menuListCommands
            '
            Me.menuListCommands.Index = 0
            Me.menuListCommands.Text = "List Commands"
            '
            'menuItem3
            '
            Me.menuItem3.Index = 1
            Me.menuItem3.Text = "-"
            '
            'menuExecute
            '
            Me.menuExecute.Index = 2
            Me.menuExecute.Shortcut = System.Windows.Forms.Shortcut.CtrlE
            Me.menuExecute.Text = "&Execute"
            '
            'menuCommandExecuteAll
            '
            Me.menuCommandExecuteAll.Index = 3
            Me.menuCommandExecuteAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftE
            Me.menuCommandExecuteAll.Text = "Execute &All"
            '
            'MainForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(712, 470)
            Me.Controls.Add(Me.panel1)
            Me.Controls.Add(Me.splitter1)
            Me.Controls.Add(Me.treeViewTables)
            Me.Controls.Add(Me.statusBar1)
            Me.Controls.Add(Me.toolBar1)
            Me.Menu = Me.mainMenu1
            Me.Name = "MainForm"
            Me.Text = "MapXtreme 2004 Data Explorer"
            Me.panel1.ResumeLayout(False)
            CType(Me.dataGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub  'InitializeComponent

        '/ <summary>
        '/ The main entry point for the application.
        '/ </summary>
        <STAThread()> _
        Public Shared Sub Main()
            Application.Run(New MainForm)
        End Sub  'Main


        '/ <summary>
        '/ Open one or more tables
        '/ </summary>
        Private Sub menuOpenTables_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuOpenTables.Click
            Dim openFileDialog1 As New System.Windows.Forms.OpenFileDialog
            openFileDialog1.Multiselect = True
            openFileDialog1.CheckFileExists = True
            openFileDialog1.DefaultExt = "TAB"
            openFileDialog1.Filter = "MapInfo Tables (*.tab)|*.tab|All files (*.*)|*.*"
            openFileDialog1.FilterIndex = 1
            openFileDialog1.RestoreDirectory = False
            Dim s As String = Session.Current.TableSearchPath.Path
            If Not (s Is Nothing) And s.Length > 0 Then
                Dim p As String = s.Split(";"c)(0)
                If Not (p Is Nothing) Then
                    openFileDialog1.InitialDirectory = p
                End If
            End If
            If openFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Dim filename As String
                For Each filename In openFileDialog1.FileNames
                    DoOpen(("open " + filename))
                Next filename
            End If
            UpdateTreeView()
        End Sub  'menuOpenTables_Click


        '/ <summary>
        '/ Execute selected lines in command window (or single line if no selection)
        '/ </summary>
        Private Sub menuExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuExecute.Click
            If rtCommand.Lines.Length = 0 Then
                Return   ' nothing to execute
            End If
            Dim savedPos As Integer = rtCommand.SelectionStart
            Dim nStartLine As Integer = rtCommand.GetLineFromCharIndex(rtCommand.SelectionStart)
            Dim nEndLine As Integer = rtCommand.GetLineFromCharIndex((rtCommand.SelectionStart + rtCommand.SelectionLength))
            Dim pos As Integer = 0
            ' calc pos of first line we are interested in
            Dim j As Integer
            For j = 0 To nStartLine - 1
                pos += rtCommand.Lines(j).Length + 1
            Next j

            Dim i As Integer
            For i = nStartLine To nEndLine
                rtCommand.Select(pos, rtCommand.Lines(i).Length)
                pos += rtCommand.Lines(i).Length + 1
                ProcessCommand(rtCommand.Lines(i))
            Next i
            rtCommand.Select(savedPos, 0)
        End Sub  'menuExecute_Click


        '/ <summary>
        '/ Execute all lines in Command window
        '/ </summary>
        Private Sub menuListCommands_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuListCommands.Click
            ProcessCommand("help")
        End Sub  'menuListCommands_Click

        Private Sub menuCommandExecuteAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuCommandExecuteAll.Click
            Dim pos As Integer = 0
            Dim line As String
            For Each line In rtCommand.Lines
                ' higlight line as it is being processed.
                rtCommand.Select(pos, line.Length)
                pos += line.Length + 1

                ProcessCommand(line)
            Next line
            rtCommand.Select(pos, 0)
        End Sub  'menuCommandExecuteAll_Click


        Private Sub toolBar1_ButtonClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolBar1.ButtonClick
            If (e.Button Is tbOpen) Then
                menuOpenCommandFile_Click(sender, e)
            End If
            If e.Button Is tbSave Then
                menuSaveAs_Click(sender, e)
            End If
            If e.Button Is tbExecute Then
                menuExecute_Click(sender, e)
            End If
        End Sub  'toolBar1_ButtonClick


        Private Sub menuFileExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuFileExit.Click
            Application.Exit()
        End Sub  'menuFileExit_Click


        Private Sub menuSelectNode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuSelectNode.Click
            Dim n As TreeNode = treeViewTables.SelectedNode
            If Not (n Is Nothing) Then
                ProcessCommand(("select * from " + n.Text))
            End If
        End Sub  'menuSelectNode_Click


        Private Sub menuDescribeNode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim n As TreeNode = treeViewTables.SelectedNode
            If Not (n Is Nothing) Then
                ProcessCommand(("desc " + n.Text))
            End If
        End Sub  'menuDescribeNode_Click


        Private Sub menuCloseNode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuCloseNode.Click
            Dim n As TreeNode = treeViewTables.SelectedNode
            If Not (n Is Nothing) Then
                ProcessCommand(("close " + n.Text))
            End If
        End Sub  'menuCloseNode_Click


        Private Sub menuCloseAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuTableCloseAll.Click, menuCloseAll.Click
            Session.Current.Catalog.CloseAll()
            UpdateTreeView()
        End Sub  'menuCloseAll_Click

        Private Sub menuCommandClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuCommandClear.Click
            If rtCommand.Modified Then
                'TODO: PROMPT TO SAVE
            End If

            rtCommand.Text = ""
        End Sub  'menuCommandClear_Click


        Private Sub menuCommandExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuCommandExecute.Click
            menuExecute_Click(sender, e)
        End Sub  'menuCommandExecute_Click

        Private Sub menuOpenCommandFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuOpenCommandFile.Click
            If rtCommand.Modified Then
                'TODO: PROMPT TO SAVE
            End If

            Dim openFileDialog1 As New System.Windows.Forms.OpenFileDialog
            openFileDialog1.Multiselect = False
            openFileDialog1.CheckFileExists = True
            openFileDialog1.DefaultExt = "SQL"
            openFileDialog1.Filter = "Sql Text Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All files (*.*)|*.*"
            openFileDialog1.FilterIndex = 1
            openFileDialog1.RestoreDirectory = False

            If openFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Dim filename As String
                For Each filename In openFileDialog1.FileNames
                    rtCommand.LoadFile(filename, RichTextBoxStreamType.PlainText)
                Next filename
            End If
        End Sub  'menuOpenCommandFile_Click

        Private Sub menuViewClearCommandWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuViewClearCommandWindow.Click
            rtCommand.Text = ""
        End Sub  'menuViewClearCommandWindow_Click


        Private Sub menuViewClearTextResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuClearTextResults.Click, menuViewClearTextResults.Click
            rtResults.Text = ""
        End Sub  'menuViewClearTextResults_Click


        Private Sub menuViewClearGridResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuViewClearGridResults.Click
            dataGrid.DataSource = Nothing
        End Sub  'menuViewClearGridResults_Click


        Private Sub menuTableSchema_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuTableSchema.Click
            Dim n As TreeNode = treeViewTables.SelectedNode
            If Not (n Is Nothing) Then
                miCommand.CommandType = System.Data.CommandType.Text
                miCommand.CommandText = "select * from " + n.Text
                miCommand.CommandText = miCommand.CommandText.Trim()
                miCommand.Prepare()
                DoGetSchemaTable()
            End If
        End Sub  'menuTableSchema_Click

        Private Sub treeViewTables_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeViewTables.AfterSelect
            ' set context menu based on selection
            Dim n As TreeNode = treeViewTables.SelectedNode
            If n Is Nothing Then
                Return
            End If
            If TypeOf n.Tag Is TableNode Then
                treeViewTables.ContextMenu = contextMenuTables
            Else
                treeViewTables.ContextMenu = Nothing
            End If
        End Sub  'treeViewTables_AfterSelect

        Private Sub contextMenuTables_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles contextMenuTables.Popup
            Dim n As TreeNode = treeViewTables.SelectedNode
            Dim m As MenuItem
            For Each m In contextMenuTables.MenuItems
                m.Enabled = Not (n Is Nothing) And Not n.Text.StartsWith("Open")
            Next m
        End Sub  'contextMenuTables_Popup

        Private Sub menuSaveAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuSaveAs.Click
            Dim saveFileDialog1 As New System.Windows.Forms.SaveFileDialog
            saveFileDialog1.CheckPathExists = True
            saveFileDialog1.DefaultExt = "SQL"
            saveFileDialog1.Filter = "Sql Text Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All files (*.*)|*.*"
            saveFileDialog1.FilterIndex = 1
            saveFileDialog1.RestoreDirectory = False

            If saveFileDialog1.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                Dim filename As String
                For Each filename In saveFileDialog1.FileNames
                    rtCommand.SaveFile(filename, RichTextBoxStreamType.PlainText)
                Next filename
            End If
        End Sub  'menuSaveAs_Click


        Private Sub treeViewTables_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treeViewTables.DoubleClick
            Dim n As TreeNode = treeViewTables.SelectedNode
            If n Is Nothing Then
                Return
            End If
            If TypeOf n.Tag Is TableNode Then
                menuSelectNode_Click(sender, e)
            End If
        End Sub  'treeViewTables_DoubleClick

        Private Sub menuItemTableSearchPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuItemTableSearchPath.Click
            Dim dlg As TableSearchPathDialog = New TableSearchPathDialog
            dlg.Path = Session.Current.TableSearchPath.Path
            If dlg.ShowDialog(Me) = DialogResult.OK Then
                Session.Current.TableSearchPath.Path = dlg.Path
                Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\MapInfo\MIDataExplorer")
                key.SetValue("TableSearchPath", Session.Current.TableSearchPath.Path)
                key.Close()
            End If
            dlg.Dispose()
        End Sub


        ' This method reads creates a bitmap object from resources
        Private Function GetBitmapResource(ByVal bitmapName As String) As Bitmap
            Dim a As [Assembly] = [Assembly].GetExecutingAssembly
            Dim s As Stream = a.GetManifestResourceStream(bitmapName)
            Return New Bitmap(s)
        End Function  'GetBitmapResource


        ' This method creates an imagelist from bitmap resources
        Private Function CreateImageList() As ImageList
            ' Create the list
            Dim imageList As New imageList
            imageList.ImageSize = New Size(18, 16)
            imageList.ColorDepth = ColorDepth.Depth4Bit
            imageList.TransparentColor = Color.FromArgb(192, 192, 192)

            ' Add bitmaps
            imageList.Images.AddStrip(GetBitmapResource("buttons.bmp"))

            Return imageList
        End Function  'CreateImageList


        ' This methods associates the imagelist with the toolbar
        ' and assigns images to each tool button.
        Private Sub ToolBarSetup()
            toolBar1.ImageList = CreateImageList()
            tbOpen.ImageIndex = 1
            tbSave.ImageIndex = 2
            tbExecute.ImageIndex = 9
        End Sub  'ToolBarSetup


        Sub AddTableNodes(ByVal tn As TreeNode, ByVal [alias] As String)
            Dim miTable As Table = Session.Current.Catalog.GetTable([alias])
            If Nothing Is miTable Then
                Return
            End If
            Dim ti As TableInfo = miTable.TableInfo

            Dim s1, s2, s3, s4 As String
            s1 = ti.Alias
            If s1 Is Nothing Then
                s1 = "null"
            End If
            s2 = ti.DataSourceName
            If s2 Is Nothing Then
                s2 = "null"
            End If
            s3 = ti.Description
            If s3 Is Nothing Then
                s3 = "null"
            End If
            s4 = ti.TableType.ToString()

            Dim tnv As New TreeNode("Datasource: " + s2)
            tn.Nodes.Add(tnv)
            tnv = New TreeNode("Description: " + s3)
            tn.Nodes.Add(tnv)
            tnv = New TreeNode("Type: " + s4)
            tn.Nodes.Add(tnv)
            tnv = New TreeNode("ReadOnly: " + ti.ReadOnly.ToString())
            tn.Nodes.Add(tnv)
            tnv = New TreeNode("Path: " + ti.TablePath)
            tn.Nodes.Add(tnv)

            tnv = New TreeNode("Columns")
            tn.Nodes.Add(tnv)
            Dim n As Integer = ti.Columns.Count
            Dim i As Integer
            Dim cn As TreeNode = Nothing
            For i = 0 To n - 1
                Dim col As Column = ti.Columns(i)
                cn = New TreeNode(col.Alias)
                tnv.Nodes.Add(cn)
                Dim tn2 As New TreeNode("Ordinal: " + col.Ordinal.ToString())
                cn.Nodes.Add(tn2)
                tn2 = New TreeNode("Type: " + col.DataType.ToString())
                cn.Nodes.Add(tn2)
                ' geocol stuff
                If col.DataType = MIDbType.FeatureGeometry Then
                    Dim spcol As GeometryColumn = col
                    Dim csys As MapInfo.Geometry.CoordSys = spcol.CoordSys

                    Dim tncv As New TreeNode("CoordSys: " + csys.MapBasicString)
                    tn2.Nodes.Add(tncv)

                    Dim dr As MapInfo.Geometry.DRect
                    dr = spcol.Bounds

                    Dim s As String = String.Format("Entire Bounds: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2)
                    tncv = New TreeNode(s)
                    tn2.Nodes.Add(tncv)

                    dr = spcol.DefaultView
                    s = String.Format("Default View: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2)
                    tncv = New TreeNode(s)
                    tn2.Nodes.Add(tncv)

                    dr = spcol.AlternateView
                    s = String.Format("Alternate View: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2)
                    tncv = New TreeNode(s)
                    tn2.Nodes.Add(tncv)
                End If

                tn2 = New TreeNode("Width: " + col.Width.ToString())
                cn.Nodes.Add(tn2)

                tn2 = New TreeNode("Index: " + col.Indexed.ToString())
                cn.Nodes.Add(tn2)

                If Not (col.ColumnExpression Is Nothing) Then
                    tn2 = New TreeNode("Expression: " + col.ColumnExpression)
                    cn.Nodes.Add(tn2)
                End If
                tn2 = New TreeNode("ReadOnly: " + col.ReadOnly.ToString())
                cn.Nodes.Add(tn2)
            Next i

            Dim mdt As MapInfo.Data.Metadata = ti.ClientMetadata

            If mdt.Count > 0 Then
                Dim dict As StringDictionary = mdt.GetAsDictionary()
                tnv = New TreeNode("Metadata")
                tn.Nodes.Add(tnv)

                Dim e As DictionaryEntry
                For Each e In dict
                    Dim sa As String() = e.Key.ToString().Split("\"c)
                    AddNode(tnv, sa, e.Value.ToString(), 2)
                Next e
            End If
            mdt.Dispose()
        End Sub  'AddTableNodes

        Function AddNode(ByVal tn As TreeNode, ByVal sa() As String, ByVal svalue As String, ByVal index As Integer) As Boolean
            Dim bDone As Boolean = False
            Dim t As TreeNode
            For Each t In tn.Nodes
                If t.Text = sa(index) Then
                    If index = sa.Length - 1 Then
                        Dim t2 As New TreeNode(sa(index) + ": " + svalue)
                        t.Nodes.Add(t2)
                        Return True
                    Else
                        bDone = AddNode(t, sa, svalue, index + 1)
                    End If
                End If
            Next t
            If Not bDone Then
                Dim tncv As TreeNode
                If index = sa.Length - 1 Then
                    tncv = New TreeNode(sa(index) + ": " + svalue)
                    tn.Nodes.Add(tncv)
                    Return True
                Else
                    tncv = New TreeNode(sa(index))
                    tn.Nodes.Add(tncv)
                    bDone = AddNode(tncv, sa, svalue, index + 1)
                End If
            End If
            Return bDone
        End Function  'AddNode


        Private Sub UpdateTreeView()
            Dim n As TreeNode = treeViewTables.SelectedNode
            Dim s As String = Nothing
            Dim index As Integer = 0
            If Not (n Is Nothing) Then
                s = n.Text
                index = treeViewTables.Nodes(0).Nodes.IndexOf(n)
            End If

            treeViewTables.Nodes.Clear()
            n = Nothing

            Dim top As New TreeNode("Open Tables")
            treeViewTables.Nodes.Add(top)

            Dim t As Table
            For Each t In Session.Current.Catalog
                Dim tn As New TreeNode(t.Alias)
                tn.Tag = New TableNode
                If t.Alias = s Then
                    n = tn
                End If
                top.Nodes.Add(tn)
                AddTableNodes(tn, t.Alias)
            Next t
            If Not (n Is Nothing) Then
                treeViewTables.SelectedNode = n
            Else
                If index >= 0 And index < treeViewTables.Nodes(0).GetNodeCount(True) Then
                    treeViewTables.SelectedNode = treeViewTables.Nodes(0).Nodes(index)
                    If index = 0 Then
                        treeViewTables.SelectedNode.Expand()
                    End If
                Else
                    Try
                        treeViewTables.SelectedNode = treeViewTables.Nodes(0).Nodes((treeViewTables.Nodes(0).GetNodeCount(True) - 1))
                    Catch
                    End Try
                End If
            End If
            top = New TreeNode("Variables")
            treeViewTables.Nodes.Add(top)

            Dim p As MIParameter
            For Each p In miCommand.Parameters
                Dim tn As New TreeNode(p.ParameterName)
                tn.Tag = New VariableNode
                top.Nodes.Add(tn)
                Dim tnv As New TreeNode("Type: " + p.MIDbType.ToString())
                tn.Nodes.Add(tnv)
                If p.MIDbType = MIDbType.CoordSys Then
                    tnv = New TreeNode("Value: " + CType(p.Value, MapInfo.Geometry.CoordSys).MapBasicString)
                Else
                    tnv = New TreeNode("Value: " + p.Value.ToString())
                End If
                tn.Nodes.Add(tnv)
            Next p
        End Sub  'UpdateTreeView


        Private Sub Flush()
            Dim s As String = _output.ToString()
            rtResults.AppendText(s)
            SetStatusMessage(s)
            _output.GetStringBuilder().Length = 0
            rtResults.Update()
        End Sub  'Flush

        Private Sub SetStatusMessage(ByVal msg As String)
            If msg.Length = 0 Then
                Return
            End If  ' strip off trailing crlf, and then find last line
            Dim nEnd As Integer = msg.Length
            If msg.EndsWith(ControlChars.Cr + ControlChars.Lf) Then
                nEnd -= 2
            End If
            Dim n As Integer = msg.LastIndexOf(ControlChars.Cr + ControlChars.Lf, nEnd - 1)
            If n <> -1 Then
                statusBar1.Text = msg.Substring(n + 2, nEnd - (n + 2))
            Else
                statusBar1.Text = msg.Substring(0, nEnd)
            End If
        End Sub  'SetStatusMessage

        Private Overloads Sub SetGrid(ByVal miReader As MIDataReader)
            SetGrid(miReader, False)
        End Sub  'SetGrid

        Private Overloads Sub SetGrid(ByVal miReader As MIDataReader, ByVal showSchema As Boolean)
            Dim ds As New DataSet("Results")
            ds.Tables.Add(miReader.GetSchemaTable())
            Dim dt As New DataTable("Data")
            Dim i As Integer
            For i = 0 To miReader.FieldCount - 1
                Dim dc As DataColumn = dt.Columns.Add(miReader.GetName(i))
            Next i
            While miReader.Read()
                Dim dr As DataRow = dt.NewRow()
                Dim j As Integer
                For j = 0 To miReader.FieldCount - 1
                    dr(j) = miReader.GetValue(j)
                Next j
                dt.Rows.Add(dr)
            End While
            ds.Tables.Add(dt)
            If showSchema Then
                dataGrid.DataSource = miReader.GetSchemaTable()
            Else
                dataGrid.DataSource = dt
            End If
            _output.WriteLine("{0} rows diplayed", dt.Rows.Count)
        End Sub  'SetGrid

        Private Sub DoHelp()
            _output.WriteLine("")
            _output.WriteLine("PATH <<table search path>>")
            _output.WriteLine("OPEN <<table file pathname>> [as <<alias>>]")
            _output.WriteLine("PACK <<table alias>>")
            _output.WriteLine("CLOSE <<table alias>> | ALL")
            _output.WriteLine("SET <<variable>> = <<expression>>")
            _output.WriteLine("")
            _output.WriteLine("PREPARE <<Statement>>")
            _output.WriteLine("ExecuteNonQuery")
            _output.WriteLine("ExecuteReader")
            _output.WriteLine("SCHEMA")
            _output.WriteLine("SELECT")
            _output.WriteLine("INSERT")
            _output.WriteLine("UPDATE")
            _output.WriteLine("DELETE")
            _output.WriteLine("ExecuteScalar <<expression>>")
            _output.WriteLine("")
            Flush()
        End Sub  'DoHelp


        Private Sub DoOpen(ByVal cmd As String)
            If System.String.Compare(cmd, 0, "open ", 0, 5, True) <> 0 Then
                Return
            End If
            cmd = cmd.Substring(5).Trim().ToLower()
            Dim miTable As Table
            Dim loc As Integer = cmd.IndexOf(" as ")
            Try
                If loc > 0 Then
                    Dim filename As String = cmd.Substring(0, loc).Trim()
                    Dim [alias] As String = cmd.Substring((loc + 4)).Trim()
                    _output.WriteLine("FileName={0}, Alias={1}", filename, [alias])
                    Dim s As String
                    If Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, s) Then
                        miTable = Session.Current.Catalog.OpenTable(s, [alias])
                    Else
                        ' try anyway, at least we will get an exception to report
                        miTable = Session.Current.Catalog.OpenTable(filename, [alias])
                    End If
                Else
                    Dim s As String
                    If Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), cmd, s) Then
                        miTable = Session.Current.Catalog.OpenTable(s)
                    Else
                        ' try anyway, at least we will get an exception to report
                        miTable = Session.Current.Catalog.OpenTable(cmd)
                    End If
                End If
                UpdateTreeView()
            Catch mix As MapInfo.Engine.CoreEngineException
                _output.WriteLine("Unable to open table {0}. Message: {1}", cmd, mix.Message)
                Flush()
                Return
            End Try
            _output.WriteLine("opened table {0}.", cmd)
            Flush()
        End Sub  'DoOpen

        Private Sub DoClose(ByVal cmd As String)
            If System.String.Compare(cmd, 0, "close ", 0, 6, True) <> 0 Then
                Return
            End If
            cmd = cmd.Substring(6).Trim()
            If System.String.Compare(cmd, 0, "all", 0, 3, True) = 0 Then
                Session.Current.Catalog.CloseAll()
                _output.WriteLine("Closed all tables.")
            Else
                Dim miTable As Table = Session.Current.Catalog.GetTable(cmd)
                If Nothing Is miTable Then
                    _output.WriteLine("Table {0} is not open!", cmd)
                    DoHelp()
                Else
                    miTable.Close()
                    _output.WriteLine("Closed table {0}", cmd)
                End If
            End If
            Flush()
            UpdateTreeView()
        End Sub  'DoClose

        Private Sub DoSet(ByVal cmd As String)
            If System.String.Compare(cmd, 0, "set ", 0, 4, True) <> 0 Then
                Return
            End If
            cmd = cmd.Substring(4).Trim()
            _output.WriteLine("processing : {0}", cmd)
            Flush()
            Dim loc As Integer = cmd.IndexOf("="c)
            If loc < 0 Then
                _output.WriteLine("Invalid format for SET command [{0}] : SET <<name>> = <<value>>", cmd)
                Flush()
                Return
            End If
            Dim pname As String = cmd.Substring(0, loc).Trim()  'vals[0].Trim();
            Dim pvalstr As String = cmd.Substring((loc + 1)).Trim()  'vals[1].Trim();
            Dim p As MIParameter = Nothing
            If miCommand.Parameters.Contains(pname) Then
                p = CType(miCommand.Parameters(pname), MIParameter)
            Else
                p = miCommand.CreateParameter()
                p.ParameterName = pname
                miCommand.Parameters.Add(p)
            End If
            miCommand.CommandText = pvalstr
            Dim oval As Object = miCommand.ExecuteScalar()
            If TypeOf oval Is Boolean Then
                p.Value = CBool(oval)
                p.DbType = System.Data.DbType.Boolean
            Else
                If TypeOf oval Is String Then
                    p.Value = oval
                    p.DbType = System.Data.DbType.String
                Else
                    If TypeOf oval Is System.Int16 Then
                        p.Value = CType(oval, System.Int16)
                        p.DbType = System.Data.DbType.Int16
                    Else
                        If TypeOf oval Is System.Int32 Then
                            p.Value = CType(oval, System.Int32)
                            p.DbType = System.Data.DbType.Int32
                        Else
                            If TypeOf oval Is System.UInt16 Then
                                p.Value = CType(oval, System.UInt16)
                                p.DbType = System.Data.DbType.UInt16
                            Else
                                If TypeOf oval Is System.UInt32 Then
                                    p.Value = CType(oval, System.UInt32)
                                    p.DbType = System.Data.DbType.UInt32
                                Else
                                    If TypeOf oval Is Double Then
                                        p.Value = CDbl(oval)
                                        p.DbType = System.Data.DbType.Double
                                    Else
                                        If TypeOf oval Is System.Decimal Then
                                            p.Value = CType(oval, System.Decimal)
                                            p.DbType = System.Data.DbType.Decimal
                                        Else
                                            If TypeOf oval Is System.DateTime Then
                                                p.Value = CType(oval, System.DateTime)
                                                p.DbType = System.Data.DbType.Date
                                            Else
                                                If TypeOf oval Is MapInfo.Geometry.FeatureGeometry Then
                                                    p.Value = oval
                                                    p.MIDbType = MIDbType.FeatureGeometry
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            _output.WriteLine("set parameter {0} to [{1}]", pname, p.Value)
            Flush()
            UpdateTreeView()
        End Sub  'DoSet

        Private Sub DoExecuteScalar(ByVal stmt As String)
            stmt = stmt.Substring(14).Trim()
            If Not (stmt Is Nothing) Then
                miCommand.CommandType = System.Data.CommandType.Text
                _output.WriteLine("processing : {0}", stmt)
                Flush()
                miCommand.CommandText = stmt
                miCommand.CommandText = miCommand.CommandText.Trim()
                miCommand.Prepare()
            End If
            Dim ret As Object = miCommand.ExecuteScalar()
            _output.WriteLine("Result: {0}", ret.ToString())
            Flush()
        End Sub  'DoExecuteScalar

        Private Sub DoNonQuery(ByVal stmt As String)
            If Not (stmt Is Nothing) Then
                miCommand.CommandType = System.Data.CommandType.Text
                _output.WriteLine("processing : {0}", stmt)
                Flush()
                miCommand.CommandText = stmt
                miCommand.CommandText = miCommand.CommandText.Trim()
                miCommand.Prepare()
            End If
            Dim nrecs As Integer = miCommand.ExecuteNonQuery()
            _output.WriteLine("{0} records effected.", nrecs)
            Flush()
        End Sub  'DoNonQuery

        Private Sub DoPrepare(ByVal stmt As String)
            If System.String.Compare(stmt, 0, "prepare ", 0, 8, True) <> 0 Then
                Return
            End If
            stmt = stmt.Substring(8).Trim()
            miCommand.CommandType = System.Data.CommandType.Text
            _output.WriteLine("Prepared : {0}", stmt)
            Flush()
            miCommand.CommandText = stmt
            miCommand.CommandText = miCommand.CommandText.Trim()
            miCommand.Prepare()
            Flush()
        End Sub  'DoPrepare

        Private Sub DoExecuteReader()
            Dim miReader As MIDataReader = miCommand.ExecuteReader()
            If miReader Is Nothing Then
                _output.WriteLine("Unable to construct reader for this statement")
                Flush()
                Return
            End If

            SetGrid(miReader)

            miCommand.Cancel()
            miReader.Close()
            Flush()
        End Sub  'DoExecuteReader

        Private Sub DoGetSchemaTable()
            _output.WriteLine("processing : {0}", "schema for " + miCommand.CommandText)
            Flush()
            Dim miReader As MIDataReader = miCommand.ExecuteReader()
            If miReader Is Nothing Then
                _output.WriteLine("Unable to construct reader for this statement")
                Flush()
                Return
            End If
            SetGrid(miReader, True)
            miCommand.Cancel()
            miReader.Close()
            Flush()
        End Sub  'DoGetSchemaTable

        Private Sub DoSelect(ByVal stmt As String)
            miCommand.CommandType = System.Data.CommandType.Text
            _output.WriteLine("processing : {0}", stmt)
            Flush()
            miCommand.CommandText = stmt
            miCommand.CommandText = miCommand.CommandText.Trim()
            miCommand.Prepare()
            DoExecuteReader()
            Flush()
        End Sub  'DoSelect

        Private Sub DoSetPath(ByVal cmd As String)
            If System.String.Compare(cmd, 0, "path ", 0, 5, True) <> 0 Then
                Return
            End If
            cmd = cmd.Substring(5).Trim().ToLower()
            Session.Current.TableSearchPath.Path = cmd
            _output.WriteLine("set table search path to {0}.", cmd)
            Flush()
        End Sub  'DoSetPath

        Private Sub DoPack(ByVal cmd As String)
            If System.String.Compare(cmd, 0, "pack ", 0, 5, True) <> 0 Then
                Return
            End If
            cmd = cmd.Substring(5).Trim()
            Dim miTable As Table = Session.Current.Catalog.GetTable(cmd)
            If Nothing Is miTable Then
                _output.WriteLine("table {0} is not open!", cmd)
            Else
                _output.WriteLine("packing table {0}", cmd)
                Flush()
                miTable.Pack(PackType.All)
                _output.WriteLine("table packed.")
            End If
            Flush()
        End Sub  'DoPack

        Private Sub ProcessCommand(ByVal s As String)
            Try
                Dim cmd As String = s
                If cmd Is Nothing Then
                    Return
                End If
                cmd = cmd.Trim()
                If cmd = "" Then
                    Return
                Else
                    If System.String.Compare(cmd, 0, "#", 0, 1, True) = 0 Then
                        Return
                    Else
                        If System.String.Compare(cmd, 0, "select ", 0, 7, True) = 0 Then
                            DoSelect(cmd)
                        Else
                            If System.String.Compare(cmd, 0, "insert ", 0, 7, True) = 0 Then
                                DoNonQuery(cmd)
                            Else
                                If System.String.Compare(cmd, 0, "update ", 0, 7, True) = 0 Then
                                    DoNonQuery(cmd)
                                Else
                                    If System.String.Compare(cmd, 0, "delete ", 0, 7, True) = 0 Then
                                        DoNonQuery(cmd)
                                    Else
                                        If System.String.Compare(cmd, 0, "ExecuteScalar ", 0, 14, True) = 0 Then
                                            DoExecuteScalar(cmd)
                                        Else
                                            If System.String.Compare(cmd, 0, "open ", 0, 5, True) = 0 Then
                                                DoOpen(cmd)
                                            Else
                                                If System.String.Compare(cmd, 0, "close ", 0, 6, True) = 0 Then
                                                    DoClose(cmd)
                                                Else
                                                    If System.String.Compare(cmd, 0, "help", 0, 4, True) = 0 Then
                                                        DoHelp()
                                                    Else
                                                        If System.String.Compare(cmd, 0, "set ", 0, 4, True) = 0 Then
                                                            DoSet(cmd)
                                                        Else
                                                            If System.String.Compare(cmd, 0, "prepare ", 0, 8, True) = 0 Then
                                                                DoPrepare(cmd)
                                                            Else
                                                                If System.String.Compare(cmd, 0, "schema", 0, 6, True) = 0 Then
                                                                    DoGetSchemaTable()
                                                                Else
                                                                    If System.String.Compare(cmd, 0, "path ", 0, 5, True) = 0 Then
                                                                        DoSetPath(cmd)
                                                                    Else
                                                                        If System.String.Compare(cmd, 0, "pack ", 0, 5, True) = 0 Then
                                                                            DoPack(cmd)
                                                                        Else
                                                                            _output.WriteLine(("Unknown command: " + cmd))
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Catch mix As MapInfo.Engine.CoreEngineException
                _output.WriteLine(mix.Message)
            Catch sx As System.Exception
                _output.WriteLine(sx.Message)
            End Try
            Flush()
        End Sub  'ProcessCommand




    End Class 'MainForm
    '
    Class TableNode
    End Class
 _
    Class VariableNode
    End Class 'VariableNode
 _
    Class ColumnsNode
    End Class 'ColumnsNode
 _
    Class ColumnNode
    End Class 'ColumnNode
 _
    Class MetadataNode
    End Class 'MetadataNode
End Namespace 'MIDataExplorer

