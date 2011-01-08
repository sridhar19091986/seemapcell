
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;

namespace MIDataExplorer
{

	/// <summary>
	/// This is the main SDI application form
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Member variables
		//used to format output for status window
		private System.IO.StringWriter _output = new System.IO.StringWriter();

		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuOpenTables;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.TreeView treeViewTables;
		private System.Windows.Forms.RichTextBox rtCommand;
		private System.Windows.Forms.RichTextBox rtResults;
		private System.Windows.Forms.DataGrid dataGrid;
		private System.Windows.Forms.ToolBarButton tbExecute;
		private System.Windows.Forms.ToolBarButton tbSep1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuExecute;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuFileExit;
		private System.Windows.Forms.ContextMenu contextMenuTables;
		private System.Windows.Forms.MenuItem menuSelectNode;
		private System.Windows.Forms.MenuItem menuCloseNode;
		private System.Windows.Forms.MenuItem menuCloseAll;
		private System.Windows.Forms.ContextMenu contextMenuCommand;
		private System.Windows.Forms.MenuItem menuCommandExecute;
		private System.Windows.Forms.MenuItem menuCommandClear;
		private System.Windows.Forms.MenuItem menuOpenCommandFile;
		private System.Windows.Forms.MenuItem menuSaveAs;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuCommand;
		private System.Windows.Forms.MenuItem menuView;
		private System.Windows.Forms.MenuItem menuViewClearCommandWindow;
		private System.Windows.Forms.MenuItem menuViewClearTextResults;
		private System.Windows.Forms.MenuItem menuViewClearGridResults;
		private System.Windows.Forms.ContextMenu contextMenuTextResults;
		private System.Windows.Forms.MenuItem menuClearTextResults;
		private System.Windows.Forms.MenuItem menuTableSchema;
		private System.Windows.Forms.MenuItem menuCommandExecuteAll;
		private System.Windows.Forms.ToolBarButton tbOpen;
		private System.Windows.Forms.ToolBarButton tbSave;
		private System.Windows.Forms.ToolBarButton tbSep2;
		private System.Windows.Forms.MenuItem menuTableCloseAll;
		private System.Windows.Forms.MenuItem menuListCommands;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItemTableSearchPath;
		private MICommand miCommand;
		private MIConnection miConnection;
		 		 #endregion

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ToolBarSetup();

			//
			// Create a connection object and open it.
			//
			miConnection = new MIConnection();
			miConnection.Open();

			//
			// Create a command object to hold parameters and last command executed
			//
			miCommand = miConnection.CreateCommand();

			// Add a few default parameters
			CoordSysFactory CSysFactory = Session.Current.CoordSysFactory;
			CoordSys Robinson = CSysFactory.CreateFromPrjString("12, 62, 7, 0");
			CoordSys LatLong = CSysFactory.CreateFromPrjString("1, 0");
			CoordSys NAD83 = CSysFactory.CreateFromPrjString("1, 74");
			miCommand.Parameters.Add("RobinsonCSys", Robinson);
			miCommand.Parameters.Add("LatLongCSys", LatLong);
			miCommand.Parameters.Add("NAD83CSys", NAD83);


			// Set table search path to value sampledatasearch registry key
			// if not found, then use the sampledatasearchpath
			// if that is not found, then just use the app's current directory
			Microsoft.Win32.RegistryKey keyApp = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MapInfo\MIDataExplorer");
			
			string s = (string)keyApp.GetValue("TableSearchPath");
			if (s == null || s.Length == 0) 
			{
				Microsoft.Win32.RegistryKey keySamp = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\MapInfo\MapXtreme\6.6");
				s = (string)keySamp.GetValue("SampleDataSearchPath");
				if (s != null && s.Length > 0) 
				{
					if (s.EndsWith("\\")==false) 
					{
						s += "\\";
					}
				}
				else 
				{
					s = Environment.CurrentDirectory;
				}
				keySamp.Close();
			}
			keyApp.Close();
	
			Session.Current.TableSearchPath.Path = s;

			ProcessCommand("help"); // show commands

			// For sample app purposes, open states.tab
			if (Session.Current.TableSearchPath.FileExists("usa.tab")) 
			{
				ProcessCommand("open usa.tab");
				// For sample app purposes, load states.sql if we can find it.
				string sFound;
				if (Session.Current.TableSearchPath.FileExists("sample.sql", out sFound)) 
				{
					rtCommand.LoadFile(sFound, RichTextBoxStreamType.PlainText);
				}
				else 
				{
					// could not open a command file, but we want to show something
					ProcessCommand("select * from usa");
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				miCommand.Cancel();
				miCommand.Dispose();
				miConnection.Close();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.treeViewTables = new System.Windows.Forms.TreeView();
			this.contextMenuTables = new System.Windows.Forms.ContextMenu();
			this.menuSelectNode = new System.Windows.Forms.MenuItem();
			this.menuTableSchema = new System.Windows.Forms.MenuItem();
			this.menuCloseNode = new System.Windows.Forms.MenuItem();
			this.menuTableCloseAll = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.rtResults = new System.Windows.Forms.RichTextBox();
			this.contextMenuTextResults = new System.Windows.Forms.ContextMenu();
			this.menuClearTextResults = new System.Windows.Forms.MenuItem();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.rtCommand = new System.Windows.Forms.RichTextBox();
			this.contextMenuCommand = new System.Windows.Forms.ContextMenu();
			this.menuCommandExecute = new System.Windows.Forms.MenuItem();
			this.menuCommandClear = new System.Windows.Forms.MenuItem();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.tbOpen = new System.Windows.Forms.ToolBarButton();
			this.tbSave = new System.Windows.Forms.ToolBarButton();
			this.tbSep1 = new System.Windows.Forms.ToolBarButton();
			this.tbSep2 = new System.Windows.Forms.ToolBarButton();
			this.tbExecute = new System.Windows.Forms.ToolBarButton();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuOpenCommandFile = new System.Windows.Forms.MenuItem();
			this.menuOpenTables = new System.Windows.Forms.MenuItem();
			this.menuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuCloseAll = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuFileExit = new System.Windows.Forms.MenuItem();
			this.menuView = new System.Windows.Forms.MenuItem();
			this.menuViewClearCommandWindow = new System.Windows.Forms.MenuItem();
			this.menuViewClearTextResults = new System.Windows.Forms.MenuItem();
			this.menuViewClearGridResults = new System.Windows.Forms.MenuItem();
			this.menuCommand = new System.Windows.Forms.MenuItem();
			this.menuListCommands = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuExecute = new System.Windows.Forms.MenuItem();
			this.menuCommandExecuteAll = new System.Windows.Forms.MenuItem();
			this.menuItemTableSearchPath = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 448);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(712, 22);
			this.statusBar1.TabIndex = 2;
			this.statusBar1.Text = "statusBar1";
			// 
			// treeViewTables
			// 
			this.treeViewTables.ContextMenu = this.contextMenuTables;
			this.treeViewTables.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewTables.HideSelection = false;
			this.treeViewTables.ImageIndex = -1;
			this.treeViewTables.Location = new System.Drawing.Point(0, 28);
			this.treeViewTables.Name = "treeViewTables";
			this.treeViewTables.SelectedImageIndex = -1;
			this.treeViewTables.Size = new System.Drawing.Size(168, 420);
			this.treeViewTables.TabIndex = 3;
			this.treeViewTables.DoubleClick += new System.EventHandler(this.treeViewTables_DoubleClick);
			this.treeViewTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTables_AfterSelect);
			// 
			// contextMenuTables
			// 
			this.contextMenuTables.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																											this.menuSelectNode,
																																											this.menuTableSchema,
																																											this.menuCloseNode,
																																											this.menuTableCloseAll});
			this.contextMenuTables.Popup += new System.EventHandler(this.contextMenuTables_Popup);
			// 
			// menuSelectNode
			// 
			this.menuSelectNode.DefaultItem = true;
			this.menuSelectNode.Index = 0;
			this.menuSelectNode.Text = "&Select";
			this.menuSelectNode.Click += new System.EventHandler(this.menuSelectNode_Click);
			// 
			// menuTableSchema
			// 
			this.menuTableSchema.Index = 1;
			this.menuTableSchema.Text = "Sc&hema";
			this.menuTableSchema.Click += new System.EventHandler(this.menuTableSchema_Click);
			// 
			// menuCloseNode
			// 
			this.menuCloseNode.Index = 2;
			this.menuCloseNode.Text = "&Close";
			this.menuCloseNode.Click += new System.EventHandler(this.menuCloseNode_Click);
			// 
			// menuTableCloseAll
			// 
			this.menuTableCloseAll.Index = 3;
			this.menuTableCloseAll.Text = "Close &All";
			this.menuTableCloseAll.Click += new System.EventHandler(this.menuCloseAll_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(168, 28);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 420);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.dataGrid);
			this.panel1.Controls.Add(this.splitter3);
			this.panel1.Controls.Add(this.rtResults);
			this.panel1.Controls.Add(this.splitter2);
			this.panel1.Controls.Add(this.rtCommand);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(171, 28);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(541, 420);
			this.panel1.TabIndex = 5;
			// 
			// dataGrid
			// 
			this.dataGrid.AlternatingBackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.dataGrid.DataMember = "";
			this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(0, 195);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.ReadOnly = true;
			this.dataGrid.Size = new System.Drawing.Size(541, 225);
			this.dataGrid.TabIndex = 5;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 192);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(541, 3);
			this.splitter3.TabIndex = 4;
			this.splitter3.TabStop = false;
			// 
			// rtResults
			// 
			this.rtResults.ContextMenu = this.contextMenuTextResults;
			this.rtResults.DetectUrls = false;
			this.rtResults.Dock = System.Windows.Forms.DockStyle.Top;
			this.rtResults.HideSelection = false;
			this.rtResults.Location = new System.Drawing.Point(0, 88);
			this.rtResults.Name = "rtResults";
			this.rtResults.ReadOnly = true;
			this.rtResults.Size = new System.Drawing.Size(541, 104);
			this.rtResults.TabIndex = 3;
			this.rtResults.Text = "";
			this.rtResults.WordWrap = false;
			// 
			// contextMenuTextResults
			// 
			this.contextMenuTextResults.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																													 this.menuClearTextResults});
			// 
			// menuClearTextResults
			// 
			this.menuClearTextResults.Index = 0;
			this.menuClearTextResults.Text = "&Clear";
			this.menuClearTextResults.Click += new System.EventHandler(this.menuViewClearTextResults_Click);
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 80);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(541, 8);
			this.splitter2.TabIndex = 2;
			this.splitter2.TabStop = false;
			// 
			// rtCommand
			// 
			this.rtCommand.AutoWordSelection = true;
			this.rtCommand.CausesValidation = false;
			this.rtCommand.ContextMenu = this.contextMenuCommand;
			this.rtCommand.Dock = System.Windows.Forms.DockStyle.Top;
			this.rtCommand.HideSelection = false;
			this.rtCommand.Location = new System.Drawing.Point(0, 0);
			this.rtCommand.Name = "rtCommand";
			this.rtCommand.ShowSelectionMargin = true;
			this.rtCommand.Size = new System.Drawing.Size(541, 80);
			this.rtCommand.TabIndex = 0;
			this.rtCommand.Text = "#Enter commands or open script file";
			this.rtCommand.WordWrap = false;
			// 
			// contextMenuCommand
			// 
			this.contextMenuCommand.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																											 this.menuCommandExecute,
																																											 this.menuCommandClear});
			// 
			// menuCommandExecute
			// 
			this.menuCommandExecute.Index = 0;
			this.menuCommandExecute.Text = "E&xecute\tCtrl+E";
			this.menuCommandExecute.Click += new System.EventHandler(this.menuCommandExecute_Click);
			// 
			// menuCommandClear
			// 
			this.menuCommandClear.Index = 1;
			this.menuCommandClear.Text = "&Clear";
			this.menuCommandClear.Click += new System.EventHandler(this.menuCommandClear_Click);
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																																								this.tbOpen,
																																								this.tbSave,
																																								this.tbSep1,
																																								this.tbSep2,
																																								this.tbExecute});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(712, 28);
			this.toolBar1.TabIndex = 6;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// tbOpen
			// 
			this.tbOpen.ImageIndex = 0;
			this.tbOpen.ToolTipText = "Open Command File";
			// 
			// tbSave
			// 
			this.tbSave.ToolTipText = "Save Command Window to File";
			// 
			// tbSep1
			// 
			this.tbSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbSep2
			// 
			this.tbSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbExecute
			// 
			this.tbExecute.ToolTipText = "Execute Current Command";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																							this.menuFile,
																																							this.menuView,
																																							this.menuCommand});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.menuOpenCommandFile,
																																						 this.menuOpenTables,
																																						 this.menuSaveAs,
																																						 this.menuItem5,
																																						 this.menuItemTableSearchPath,
																																						 this.menuCloseAll,
																																						 this.menuItem2,
																																						 this.menuFileExit});
			this.menuFile.Text = "&File";
			// 
			// menuOpenCommandFile
			// 
			this.menuOpenCommandFile.Index = 0;
			this.menuOpenCommandFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuOpenCommandFile.Text = "&Open...";
			this.menuOpenCommandFile.Click += new System.EventHandler(this.menuOpenCommandFile_Click);
			// 
			// menuOpenTables
			// 
			this.menuOpenTables.Index = 1;
			this.menuOpenTables.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.menuOpenTables.Text = "Open &Tables...";
			this.menuOpenTables.Click += new System.EventHandler(this.menuOpenTables_Click);
			// 
			// menuSaveAs
			// 
			this.menuSaveAs.Index = 2;
			this.menuSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuSaveAs.Text = "Save As...";
			this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "-";
			// 
			// menuCloseAll
			// 
			this.menuCloseAll.Index = 5;
			this.menuCloseAll.Text = "Close &All Tables";
			this.menuCloseAll.Click += new System.EventHandler(this.menuCloseAll_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 6;
			this.menuItem2.Text = "-";
			// 
			// menuFileExit
			// 
			this.menuFileExit.Index = 7;
			this.menuFileExit.Text = "E&xit";
			this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// menuView
			// 
			this.menuView.Index = 1;
			this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.menuViewClearCommandWindow,
																																						 this.menuViewClearTextResults,
																																						 this.menuViewClearGridResults});
			this.menuView.Text = "&View";
			// 
			// menuViewClearCommandWindow
			// 
			this.menuViewClearCommandWindow.Index = 0;
			this.menuViewClearCommandWindow.Text = "Clear &Commands";
			this.menuViewClearCommandWindow.Click += new System.EventHandler(this.menuViewClearCommandWindow_Click);
			// 
			// menuViewClearTextResults
			// 
			this.menuViewClearTextResults.Index = 1;
			this.menuViewClearTextResults.Text = "Clear Text Results";
			this.menuViewClearTextResults.Click += new System.EventHandler(this.menuViewClearTextResults_Click);
			// 
			// menuViewClearGridResults
			// 
			this.menuViewClearGridResults.Index = 2;
			this.menuViewClearGridResults.Text = "Clear &Grid Results";
			this.menuViewClearGridResults.Click += new System.EventHandler(this.menuViewClearGridResults_Click);
			// 
			// menuCommand
			// 
			this.menuCommand.Index = 2;
			this.menuCommand.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																								this.menuListCommands,
																																								this.menuItem3,
																																								this.menuExecute,
																																								this.menuCommandExecuteAll});
			this.menuCommand.Text = "&Command";
			// 
			// menuListCommands
			// 
			this.menuListCommands.Index = 0;
			this.menuListCommands.Text = "List Commands";
			this.menuListCommands.Click += new System.EventHandler(this.menuListCommands_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuExecute
			// 
			this.menuExecute.Index = 2;
			this.menuExecute.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
			this.menuExecute.Text = "&Execute";
			this.menuExecute.Click += new System.EventHandler(this.menuExecute_Click);
			// 
			// menuCommandExecuteAll
			// 
			this.menuCommandExecuteAll.Index = 3;
			this.menuCommandExecuteAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftE;
			this.menuCommandExecuteAll.Text = "Execute &All";
			this.menuCommandExecuteAll.Click += new System.EventHandler(this.menuCommandExecuteAll_Click);
			// 
			// menuItemTableSearchPath
			// 
			this.menuItemTableSearchPath.Index = 4;
			this.menuItemTableSearchPath.Text = "Table Search Path...";
			this.menuItemTableSearchPath.Click += new System.EventHandler(this.menuItemTableSearchPath_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 470);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.treeViewTables);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.toolBar1);
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "MapXtreme 2004 Data Explorer";
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			this.ResumeLayout(false);

		}
		 		 #endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}


		#region Menu and toolbar handlers

		/// <summary>
		/// Open one or more tables
		/// </summary>
		private void menuOpenTables_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = true;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "TAB";
			openFileDialog1.Filter = "MapInfo Tables (*.tab)|*.tab|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 1 ;
			openFileDialog1.RestoreDirectory = false;
			string s = Session.Current.TableSearchPath.Path;
			if (s != null && s.Length > 0) 
			{
				string p = s.Split(';')[0];
				if (p!=null) openFileDialog1.InitialDirectory = p;
			}
		 		 		 
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				foreach(string filename in openFileDialog1.FileNames)		 
				{
					DoOpen("open " + filename);
				}
			}
			UpdateTreeView();
		}

		/// <summary>
		/// Execute selected lines in command window (or single line if no selection)
		/// </summary>
		private void menuExecute_Click(object sender, System.EventArgs e)
		{
			if (rtCommand.Lines.Length == 0) return; // nothing to execute

			int savedPos = rtCommand.SelectionStart;
			int nStartLine = rtCommand.GetLineFromCharIndex(rtCommand.SelectionStart);
			int nEndLine = rtCommand.GetLineFromCharIndex(rtCommand.SelectionStart + rtCommand.SelectionLength);
			int pos = 0;
			// calc pos of first line we are interested in
			for (int j=0; j< nStartLine; j++) 
			{
				pos += rtCommand.Lines[j].Length + 1;
			}

			for (int i=nStartLine; i<= nEndLine; i++)
			{
				rtCommand.Select(pos, rtCommand.Lines[i].Length);
				pos += rtCommand.Lines[i].Length+1;
				ProcessCommand(rtCommand.Lines[i]);
			}
			rtCommand.Select(savedPos, 0);
		}

		/// <summary>
		/// Execute all lines in Command window
		/// </summary>
		private void menuListCommands_Click(object sender, System.EventArgs e)
		{
			ProcessCommand("help");
		}
		private void menuCommandExecuteAll_Click(object sender, System.EventArgs e)
		{
			int pos=0;
			foreach (string line in rtCommand.Lines)
			{
				// higlight line as it is being processed.
				rtCommand.Select(pos, line.Length);
				pos += line.Length+1;
		 		 		 		 
				ProcessCommand(line);
			}		 		 
			rtCommand.Select(pos, 0);
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbOpen) 
			{
				menuOpenCommandFile_Click(sender, e);
			}
			if (e.Button == tbSave) 
			{
				menuSaveAs_Click(sender, e);
			}
			if (e.Button == tbExecute) 
			{
				menuExecute_Click(sender, e);
			}
		}

		private void menuFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();		 		 
		}

		private void menuSelectNode_Click(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;
			if (n != null) 
			{
				ProcessCommand("select * from " + n.Text);
			}
		}

		private void menuDescribeNode_Click(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;
			if (n != null) 
			{
				ProcessCommand("desc " + n.Text);
			}
		}

		private void menuCloseNode_Click(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;
			if (n != null) 
			{
				ProcessCommand("close " + n.Text);		 		 
			}
		}

		private void menuCloseAll_Click(object sender, System.EventArgs e)
		{
			Session.Current.Catalog.CloseAll();
			UpdateTreeView();		 		 
		}
		private void menuCommandClear_Click(object sender, System.EventArgs e)
		{
			if (rtCommand.Modified) 
			{
				//TODO: PROMPT TO SAVE
			}
			rtCommand.Text = "";
		}

		private void menuCommandExecute_Click(object sender, System.EventArgs e)
		{
			menuExecute_Click(sender, e);
		}
		private void menuOpenCommandFile_Click(object sender, System.EventArgs e)
		{
			if (rtCommand.Modified) 
			{
				//TODO: PROMPT TO SAVE
			}
			System.Windows.Forms.OpenFileDialog openFileDialog1=new System.Windows.Forms.OpenFileDialog();
			openFileDialog1.Multiselect = false;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.DefaultExt = "SQL";
			openFileDialog1.Filter = "Sql Text Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex = 1 ;
			openFileDialog1.RestoreDirectory = false;
		 		 		 
			if(openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				foreach(string filename in openFileDialog1.FileNames)		 
				{
					rtCommand.LoadFile(filename, RichTextBoxStreamType.PlainText);
				}
			}
		}
		private void menuViewClearCommandWindow_Click(object sender, System.EventArgs e)
		{
			rtCommand.Text = "";
		}

		private void menuViewClearTextResults_Click(object sender, System.EventArgs e)
		{
			rtResults.Text = "";
		}

		private void menuViewClearGridResults_Click(object sender, System.EventArgs e)
		{
			dataGrid.DataSource = null;		 		 
		}

		private void menuTableSchema_Click(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;
			if (n != null) 
			{
				miCommand.CommandType = System.Data.CommandType.Text;
				miCommand.CommandText = "select * from " + n.Text;
				miCommand.CommandText = miCommand.CommandText.Trim();
				miCommand.Prepare();
				DoGetSchemaTable();
			}
		 		 		 
		}
		private void treeViewTables_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			// set context menu based on selection
			TreeNode n = treeViewTables.SelectedNode;
			if (n == null) return;
			if (n.Tag is TableNode) 
			{
				treeViewTables.ContextMenu = contextMenuTables;
			}
			else 
			{
				treeViewTables.ContextMenu = null;
			}
		}
		private void contextMenuTables_Popup(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;
			foreach (MenuItem m in contextMenuTables.MenuItems)
			{
				m.Enabled = n != null && !n.Text.StartsWith("Open");
			}
		}
		private void menuSaveAs_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.SaveFileDialog saveFileDialog1=new System.Windows.Forms.SaveFileDialog();
			saveFileDialog1.CheckPathExists = true;
			saveFileDialog1.DefaultExt = "SQL";
			saveFileDialog1.Filter = "Sql Text Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All files (*.*)|*.*" ;
			saveFileDialog1.FilterIndex = 1 ;
			saveFileDialog1.RestoreDirectory = false;
		 		 		 
			if(saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
			{
				foreach(string filename in saveFileDialog1.FileNames)		 
				{
					rtCommand.SaveFile(filename, RichTextBoxStreamType.PlainText);
				}
			}
		}

		private void treeViewTables_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode n = treeViewTables.SelectedNode;		 		 
			if (n == null) return;
			if (n.Tag is TableNode) 
			{
				menuSelectNode_Click(sender, e);
			}
		}
		private void menuItemTableSearchPath_Click(object sender, System.EventArgs e)
		{
			TableSearchPathDialog dlg = new TableSearchPathDialog();
			dlg.Path = Session.Current.TableSearchPath.Path;
			if (dlg.ShowDialog(this) == DialogResult.OK) 
			{
				Session.Current.TableSearchPath.Path = dlg.Path;
				Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\MapInfo\MIDataExplorer");
				key.SetValue("TableSearchPath", Session.Current.TableSearchPath.Path);
				key.Close();
			}
			dlg.Dispose();
		}

		#endregion
		 		 
		// This method reads creates a bitmap object from resources
		private Bitmap GetBitmapResource(string bitmapName) 
		{
			Assembly a = Assembly.GetExecutingAssembly();
			Stream s = a.GetManifestResourceStream(this.GetType(), bitmapName);
			return new Bitmap(s);
		}

		// This method creates an imagelist from bitmap resources
		private ImageList CreateImageList() 
		{
			// Create the list
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(18, 16);
			imageList.ColorDepth = ColorDepth.Depth4Bit;
			imageList.TransparentColor = Color.FromArgb(192, 192, 192);

			// Add bitmaps
			imageList.Images.AddStrip(GetBitmapResource("buttons.bmp"));

			return imageList;
		}

		// This methods associates the imagelist with the toolbar
		// and assigns images to each tool button.
		private void ToolBarSetup() 
		{
			toolBar1.ImageList = CreateImageList();
			tbOpen.ImageIndex = 1;
			tbSave.ImageIndex = 2;
			tbExecute.ImageIndex = 9;
		}

		void AddTableNodes(TreeNode tn, string alias)
		{
			Table miTable = Session.Current.Catalog.GetTable(alias);
			if (null == miTable) 
			{
				return;
			}
			TableInfo ti = miTable.TableInfo;

			string s1,s2,s3,s4;
			s1 = ti.Alias; if (s1 == null) s1 = "null";
			s2 = ti.DataSourceName; if (s2 == null) s2 = "null";
			s3 = ti.Description; if (s3 == null) s3 = "null";
			s4 = ti.TableType.ToString();

			TreeNode tnv = new TreeNode("Datasource: " + s2);
			tn.Nodes.Add(tnv);
			tnv = new TreeNode("Description: " + s3);
			tn.Nodes.Add(tnv);
			tnv = new TreeNode("Type: " + s4);
			tn.Nodes.Add(tnv);
			tnv = new TreeNode("ReadOnly: " + ti.ReadOnly);
			tn.Nodes.Add(tnv);
			tnv = new TreeNode("Path: " + ti.TablePath);
			tn.Nodes.Add(tnv);
		 		 		 
			tnv = new TreeNode("Columns");
			tn.Nodes.Add(tnv);
			int i, n = ti.Columns.Count;
			TreeNode cn=null;
			for(i=0; i<n; i++)
			{
				Column col = ti.Columns[i];
				cn = new TreeNode(col.Alias);
				tnv.Nodes.Add(cn);
				TreeNode tn2 = new TreeNode("Ordinal: " + col.Ordinal);
				cn.Nodes.Add(tn2);
				tn2 = new TreeNode("Type: " +col.DataType.ToString());
				cn.Nodes.Add(tn2);
				// geocol stuff
				if (col.DataType == MIDbType.FeatureGeometry)
				{
					GeometryColumn spcol = col as GeometryColumn;
					MapInfo.Geometry.CoordSys csys = spcol.CoordSys;
		 		 		 		 		 
					TreeNode tncv = new TreeNode("CoordSys: " + csys.MapBasicString);
					tn2.Nodes.Add(tncv);

					MapInfo.Geometry.DRect dr;
					dr = spcol.Bounds;
		 		 		 		 		 
					string s = string.Format("Entire Bounds: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2);
					tncv = new TreeNode(s);
					tn2.Nodes.Add(tncv);

					dr = spcol.DefaultView;
					s = string.Format("Default View: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2);
					tncv = new TreeNode(s);
					tn2.Nodes.Add(tncv);

					dr = spcol.AlternateView;
					s = string.Format("Alternate View: ({0},{1}),({2},{3})", dr.x1, dr.y1, dr.x2, dr.y2);
					tncv = new TreeNode(s);
					tn2.Nodes.Add(tncv);
				}
		 		 		 		 
				tn2 = new TreeNode("Width: " + col.Width.ToString());
				cn.Nodes.Add(tn2);

				tn2 = new TreeNode("Index: " + col.Indexed.ToString());
				cn.Nodes.Add(tn2);

				if (col.ColumnExpression != null && col.ColumnExpression.Length > 0) 
				{
					tn2 = new TreeNode("Expression: " + col.ColumnExpression);
					cn.Nodes.Add(tn2);
				}
				tn2 = new TreeNode("ReadOnly: " + col.ReadOnly.ToString());
				cn.Nodes.Add(tn2);

			}

			Metadata mdt = ti.ClientMetadata;

			if (mdt.Count > 0) 
			{
				StringDictionary dict = mdt.GetAsDictionary();
				tnv = new TreeNode("Metadata");
				tn.Nodes.Add(tnv);

				foreach(DictionaryEntry e in dict)
				{
					string[] sa = e.Key.ToString().Split('\\');
					AddNode(tnv, sa, e.Value.ToString(), 2);
				}
			}
			mdt.Dispose();

		}
		bool AddNode(TreeNode tn, string []sa, string svalue, int index)
		{
			bool bDone=false;
			foreach(TreeNode t in tn.Nodes) 
			{
				if (t.Text == sa[index]) 
				{
					if (index == sa.Length-1) 
					{
						TreeNode t2 = new TreeNode(sa[index] + ": " + svalue);
						t.Nodes.Add(t2);
						return true;
					}
					else 
					{
						bDone=AddNode(t, sa, svalue, index+1);
					}
				}
			}
			if (!bDone) 
			{
				TreeNode tncv;
				if (index == sa.Length-1) 
				{
					tncv = new TreeNode(sa[index] + ": " + svalue);
					tn.Nodes.Add(tncv);
					return true;
				}
				else 
				{
					tncv = new TreeNode(sa[index]);
					tn.Nodes.Add(tncv);
					bDone = AddNode(tncv, sa, svalue, index+1);
				}
			}
			return bDone;
		}

		private void UpdateTreeView()
		{
			TreeNode n = treeViewTables.SelectedNode;
			string s=null;
			int index = 0;
			if (n != null) 
			{
				s = n.Text;
				index = treeViewTables.Nodes[0].Nodes.IndexOf(n);
			}

			treeViewTables.Nodes.Clear();
			n=null;

			TreeNode top = new TreeNode("Open Tables");
			treeViewTables.Nodes.Add(top);

			foreach (Table t in Session.Current.Catalog)
			{
				TreeNode tn = new TreeNode(t.Alias);
				tn.Tag = new TableNode();
				if (t.Alias == s) 
				{
					n = tn;
				}
				top.Nodes.Add(tn);
				AddTableNodes(tn, t.Alias);
			}
			if (n != null)
			{
				treeViewTables.SelectedNode = n;
			}
			else 
			{
				if (index >= 0 && index < treeViewTables.Nodes[0].GetNodeCount(true))
				{
					treeViewTables.SelectedNode = treeViewTables.Nodes[0].Nodes[index];
					if (index == 0) 
					{
						treeViewTables.SelectedNode.Expand();// .ExpandAll();
					}
				}
				else
				{
					try 
					{
						treeViewTables.SelectedNode = treeViewTables.Nodes[0].Nodes[treeViewTables.Nodes[0].GetNodeCount(true)-1];
					}
					catch {}
				}
			}

			top = new TreeNode("Variables");
			treeViewTables.Nodes.Add(top);

			foreach (MIParameter p in miCommand.Parameters)
			{
				TreeNode tn = new TreeNode(p.ParameterName);
				tn.Tag = new VariableNode();
				top.Nodes.Add(tn);
				TreeNode tnv = new TreeNode("Type: " + p.MIDbType.ToString());
				tn.Nodes.Add(tnv);
				if (p.MIDbType == MIDbType.CoordSys)
				{
					tnv = new TreeNode("Value: " + (p.Value as MapInfo.Geometry.CoordSys).MapBasicString);
				}
				else 
				{
					tnv = new TreeNode("Value: " + p.Value.ToString());
				}
				tn.Nodes.Add(tnv);
			}
		}

		private void Flush()
		{
			string s = _output.ToString();
			rtResults.AppendText(s);
			SetStatusMessage(s);
			_output.GetStringBuilder().Length = 0;
			rtResults.Update();
		}
		private void SetStatusMessage(string msg)
		{
			if (msg.Length == 0) return;
			// strip off trailing crlf, and then find last line
			int nEnd=msg.Length;
			if (msg.EndsWith("\r\n")) 
			{
				nEnd -= 2;
			}
			int n = msg.LastIndexOf("\r\n", nEnd-1);
			if (n != -1) 
			{
				statusBar1.Text = msg.Substring(n+2, nEnd - (n+2));
			}
			else 
			{
				statusBar1.Text = msg.Substring(0, nEnd);
			}
		}
		private void SetGrid(MIDataReader miReader)
		{
			SetGrid(miReader, false);
		}
		private void SetGrid(MIDataReader miReader, bool showSchema)
		{
			DataSet ds = new DataSet("Results");
			ds.Tables.Add(miReader.GetSchemaTable());
			DataTable dt = new DataTable("Data");
			for (int i = 0; i < miReader.FieldCount; i++)
			{
				DataColumn dc = dt.Columns.Add(miReader.GetName(i));
			}
			while (miReader.Read())
			{
				DataRow dr = dt.NewRow();
				for (int i = 0; i < miReader.FieldCount; i++)
				{
					dr[i] = miReader.GetValue(i);
				}
				dt.Rows.Add(dr);
			}
			ds.Tables.Add(dt);
			if (showSchema) 
			{
				dataGrid.DataSource = miReader.GetSchemaTable();
			}
			else 
			{
				dataGrid.DataSource = dt;
			}
			_output.WriteLine("{0} rows diplayed", dt.Rows.Count);
		}


		#region Our Command handlers
		private void DoHelp()
		{
			_output.WriteLine("");
			_output.WriteLine("PATH <<table search path>>");
			_output.WriteLine("OPEN <<table file pathname>> [as <<alias>>]");
			_output.WriteLine("PACK <<table alias>>");
			_output.WriteLine("CLOSE <<table alias>> | ALL");
			_output.WriteLine("SET <<variable>> = <<expression>>");
			_output.WriteLine("");
			_output.WriteLine("PREPARE <<Statement>>");
			_output.WriteLine("ExecuteNonQuery");
			_output.WriteLine("ExecuteReader");
			_output.WriteLine("SCHEMA");
			_output.WriteLine("SELECT");
			_output.WriteLine("INSERT");
			_output.WriteLine("UPDATE");
			_output.WriteLine("DELETE");
			_output.WriteLine("ExecuteScalar <<expression>>");
			_output.WriteLine("");
			Flush();
		}
		 		 
		private void DoOpen(string cmd)
		{
			if (System.String.Compare(cmd,0,"open ",0,5,true) != 0) return;
			cmd = cmd.Substring(5).Trim().ToLower();
			Table miTable;
			int loc = cmd.IndexOf(" as ");
			try
			{
				if (loc > 0)
				{
					string filename = cmd.Substring(0,loc).Trim();
					string alias = cmd.Substring(loc + 4).Trim();
					_output.WriteLine("FileName={0}, Alias={1}", filename, alias);
					string s;
					if (Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, out s)) 
					{
						miTable = Session.Current.Catalog.OpenTable(s, alias);
					}
					else
					{
						// try anyway, at least we will get an exception to report
						miTable = Session.Current.Catalog.OpenTable(filename, alias);
					}
				}
				else
				{
					string s;
					if (Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), cmd, out s))
					{
						miTable = Session.Current.Catalog.OpenTable(s);
					}
					else
					{
						// try anyway, at least we will get an exception to report
						miTable = Session.Current.Catalog.OpenTable(cmd);
					}
				}
				UpdateTreeView();
			}
			catch (MapInfo.Engine.CoreEngineException mix)
			{
				_output.WriteLine("Unable to open table {0}. Message: {1}", cmd, mix.Message);
				Flush();
				return;
			}
			_output.WriteLine("opened table {0}.", cmd);
			Flush();
		}
		private void DoClose(string cmd)
		{
			if (System.String.Compare(cmd,0,"close ",0,6,true) != 0) return;
			cmd = cmd.Substring(6).Trim();
			if (System.String.Compare(cmd,0,"all",0,3,true) == 0)
			{
				Session.Current.Catalog.CloseAll();
				_output.WriteLine("Closed all tables.");
			}
			else
			{
				Table miTable = Session.Current.Catalog.GetTable(cmd);
				if (null == miTable) 
				{
					_output.WriteLine("Table {0} is not open!", cmd);
					DoHelp();
				}
				else 
				{
					miTable.Close();
					_output.WriteLine("Closed table {0}", cmd);
				}
			}
			Flush();
			UpdateTreeView();
		}
		private void DoSet(string cmd)
		{
			if (System.String.Compare(cmd,0,"set ",0,4,true) != 0) return;
			cmd = cmd.Substring(4).Trim();
			_output.WriteLine("processing : {0}", cmd);
			Flush();
			int loc = cmd.IndexOf('=');
			if (loc < 0)
			{
				_output.WriteLine("Invalid format for SET command [{0}] : SET <<name>> = <<value>>", cmd);
				Flush();
				return;
			}
			string pname = cmd.Substring(0,loc).Trim();//vals[0].Trim();
			string pvalstr = cmd.Substring(loc+1).Trim();//vals[1].Trim();

			MIParameter p = null;
			if (miCommand.Parameters.Contains(pname))
			{
				p = (MIParameter) miCommand.Parameters[pname];
			}
			else
			{
				p = miCommand.CreateParameter();
				p.ParameterName = pname;
				miCommand.Parameters.Add(p);
			}
			miCommand.CommandText = pvalstr;
			object oval = miCommand.ExecuteScalar();
			if (oval is bool)
			{
				p.Value = (bool) oval;
				p.DbType = System.Data.DbType.Boolean;
			}
			else if (oval is string)
			{
				p.Value = oval as string;
				p.DbType = System.Data.DbType.String;
			}
			else if (oval is System.Int16)
			{
				p.Value = (System.Int16) oval;
				p.DbType = System.Data.DbType.Int16;
			}
			else if (oval is System.Int32)
			{
				p.Value = (System.Int32) oval;
				p.DbType = System.Data.DbType.Int32;
			}
			else if (oval is System.UInt16)
			{
				p.Value = (System.UInt16) oval;
				p.DbType = System.Data.DbType.UInt16;
			}
			else if (oval is System.UInt32)
			{
				p.Value = (System.UInt32) oval;
				p.DbType = System.Data.DbType.UInt32;
			}
			else if (oval is double)
			{
				p.Value = (double) oval;
				p.DbType = System.Data.DbType.Double;
			}
			else if (oval is System.Decimal)
			{
				p.Value = (System.Decimal) oval;
				p.DbType = System.Data.DbType.Decimal;
			}
			else if (oval is System.DateTime)
			{
				p.Value = (System.DateTime) oval;
				p.DbType = System.Data.DbType.Date;
			}
			else if (oval is MapInfo.Geometry.FeatureGeometry)
			{
				p.Value = oval as MapInfo.Geometry.FeatureGeometry;
				p.MIDbType = MIDbType.FeatureGeometry;
			}
			_output.WriteLine("set parameter {0} to [{1}]", pname, p.Value);
			Flush();
			UpdateTreeView();
		}
		private void DoExecuteScalar(string stmt)
		{
			stmt = stmt.Substring(14).Trim();
			if (stmt != null)
			{
				miCommand.CommandType = System.Data.CommandType.Text;
				_output.WriteLine("processing : {0}", stmt);
				Flush();
				miCommand.CommandText = stmt;
				miCommand.CommandText = miCommand.CommandText.Trim();
				miCommand.Prepare();
			}
			object ret = miCommand.ExecuteScalar();
			_output.WriteLine("Result: {0}", ret.ToString());
			Flush();
		}
		private void DoNonQuery(string stmt)
		{
			if (stmt != null)
			{
				miCommand.CommandType = System.Data.CommandType.Text;
				_output.WriteLine("processing : {0}", stmt);
				Flush();
				miCommand.CommandText = stmt;
				miCommand.CommandText = miCommand.CommandText.Trim();
				miCommand.Prepare();
			}
			int nrecs = miCommand.ExecuteNonQuery();
			_output.WriteLine("{0} records effected.", nrecs);
			Flush();
		}
		private void DoPrepare(string stmt)
		{
			if (System.String.Compare(stmt,0,"prepare ",0,8,true) != 0) return;
			stmt = stmt.Substring(8).Trim();
			miCommand.CommandType = System.Data.CommandType.Text;
			_output.WriteLine("Prepared : {0}", stmt);
			Flush();
			miCommand.CommandText = stmt;
			miCommand.CommandText = miCommand.CommandText.Trim();
			miCommand.Prepare();
			Flush();
		}
		private void DoExecuteReader()
		{
			MIDataReader miReader = miCommand.ExecuteReader();
			if (miReader == null)
			{
				_output.WriteLine("Unable to construct reader for this statement");
				Flush();
				return;
			}
		 		 		 
			SetGrid(miReader);

			miCommand.Cancel();
			miReader.Close();
			Flush();
		}
		private void DoGetSchemaTable()
		{
			_output.WriteLine("processing : {0}", "schema for " + miCommand.CommandText);
			Flush();
			MIDataReader miReader = miCommand.ExecuteReader();
			if (miReader == null)
			{
				_output.WriteLine("Unable to construct reader for this statement");
				Flush();
				return;
			}
			SetGrid(miReader, true);
			miCommand.Cancel();
			miReader.Close();
			Flush();
		}
		private void DoSelect(string stmt)
		{
			miCommand.CommandType = System.Data.CommandType.Text;
			_output.WriteLine("processing : {0}", stmt);
			Flush();
			miCommand.CommandText = stmt;
			miCommand.CommandText = miCommand.CommandText.Trim();
			miCommand.Prepare();
			DoExecuteReader();
			Flush();
		}
		private void DoSetPath(string cmd)
		{
			if (System.String.Compare(cmd,0,"path ",0,5,true) != 0) return;
			cmd = cmd.Substring(5).Trim().ToLower();
			Session.Current.TableSearchPath.Path = cmd;
			_output.WriteLine("set table search path to {0}.", cmd);
			Flush();
		}
		private void DoPack(string cmd)
		{
			if (System.String.Compare(cmd,0,"pack ",0,5,true) != 0) return;
			cmd = cmd.Substring(5).Trim();
			Table miTable = Session.Current.Catalog.GetTable(cmd);
			if (null == miTable) 
			{
				_output.WriteLine("table {0} is not open!", cmd);
			}
			else 
			{
				_output.WriteLine("packing table {0}", cmd);
				Flush();
				miTable.Pack(PackType.All);
				_output.WriteLine("table packed.");
			}
			Flush();
		}
		private void ProcessCommand(string s)
		{
			try
			{
				string cmd = s;
				if (cmd == null)
				{
					return;
				}
				cmd = cmd.Trim();
				if (cmd == "")
				{
					return;
				}
				else if (System.String.Compare(cmd,0,"#",0,1,true) == 0)
				{
					return;
				}
				else if (System.String.Compare(cmd,0,"select ",0,7,true) == 0) DoSelect(cmd);
				else if (System.String.Compare(cmd,0,"insert ",0,7,true) == 0) DoNonQuery(cmd);
				else if (System.String.Compare(cmd,0,"update ",0,7,true) == 0) DoNonQuery(cmd);
				else if (System.String.Compare(cmd,0,"delete ",0,7,true) == 0) DoNonQuery(cmd);
				else if (System.String.Compare(cmd,0,"ExecuteScalar ",0,14,true) == 0) DoExecuteScalar(cmd);
				else if (System.String.Compare(cmd,0,"open ",0,5,true) == 0) DoOpen(cmd);
				else if (System.String.Compare(cmd,0,"close ",0,6,true) == 0) DoClose(cmd);
				else if (System.String.Compare(cmd,0,"help",0,4,true) == 0) DoHelp();
				else if (System.String.Compare(cmd,0,"set ",0,4,true) == 0) DoSet(cmd);
				else if (System.String.Compare(cmd,0,"prepare ",0,8,true) == 0) DoPrepare(cmd);
				else if (System.String.Compare(cmd,0,"schema",0,6,true) == 0) DoGetSchemaTable();
				else if (System.String.Compare(cmd,0,"path ",0,5,true) == 0) DoSetPath(cmd);
				else if (System.String.Compare(cmd,0,"pack ",0,5,true) == 0) DoPack(cmd);
				else 
				{
					_output.WriteLine("Unknown command: " + cmd);
				}
			}
			catch (MapInfo.Engine.CoreEngineException mix)
			{
				_output.WriteLine(mix.Message);
			}
			catch (System.Exception sx)
			{
				_output.WriteLine(sx.Message);
			}
			Flush();
		 		 		 
		}

#endregion

	}

	#region Currently not used for much, but will be later
	class TableNode
	{
	}
	class VariableNode
	{
	}
	class ColumnsNode
	{
	}
	class ColumnNode
	{
	}
	class MetadataNode
	{
	}
		 #endregion

}