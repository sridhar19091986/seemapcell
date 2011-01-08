using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using MapInfo.Engine;
using MapInfo.Data;

namespace OpenTable
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// The MICommand variable is used to generate cursors against open tables.
		/// </summary>
		private MapInfo.Data.MICommand miCommand;
		private MapInfo.Data.MIConnection miConnection;

		private System.Windows.Forms.Button OpenTable;
		private System.Windows.Forms.Button ExitButton;
		private System.Windows.Forms.DataGrid dataGrid;
		private System.Windows.Forms.CheckBox showTableStructure;
		private System.Windows.Forms.Button buttonCloseTable;
		private System.Windows.Forms.Button buttonNext;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Allows looping over the set of open tables
		/// </summary>
		private MapInfo.Data.ITableEnumerator _tableEnum = null;
		/// <summary>
		/// Returns the number of open tables
		/// </summary>
		private int OpenTableCount { get { return Session.Current.Catalog.Count; } }
		/// <summary>
		/// Alias of the currently displayed table
		/// </summary>
		private string _tableAlias;
		/// <summary>
		///  Index of the currently displayed table
		/// </summary>
		private int _tableIndex = 0;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			UpdateNavigationButtons();
			this.components = new System.ComponentModel.Container();

			//
			// Create a command object to hold parameters and last command executed
			//
			this.miConnection = new MIConnection();
			miConnection.Open();
			this.miCommand = miConnection.CreateCommand();
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
			this.OpenTable = new System.Windows.Forms.Button();
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.ExitButton = new System.Windows.Forms.Button();
			this.showTableStructure = new System.Windows.Forms.CheckBox();
			this.buttonCloseTable = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// OpenTable
			// 
			this.OpenTable.Location = new System.Drawing.Point(8, 8);
			this.OpenTable.Name = "OpenTable";
			this.OpenTable.Size = new System.Drawing.Size(88, 24);
			this.OpenTable.TabIndex = 0;
			this.OpenTable.Text = "Open Table";
			this.OpenTable.Click += new System.EventHandler(this.OpenTable_Click);
			// 
			// dataGrid
			// 
			this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGrid.DataMember = "";
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(8, 48);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.Size = new System.Drawing.Size(736, 432);
			this.dataGrid.TabIndex = 1;
			// 
			// ExitButton
			// 
			this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ExitButton.Location = new System.Drawing.Point(656, 8);
			this.ExitButton.Name = "ExitButton";
			this.ExitButton.Size = new System.Drawing.Size(88, 24);
			this.ExitButton.TabIndex = 2;
			this.ExitButton.Text = "Exit";
			this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
			// 
			// showTableStructure
			// 
			this.showTableStructure.Location = new System.Drawing.Point(400, 8);
			this.showTableStructure.Name = "showTableStructure";
			this.showTableStructure.Size = new System.Drawing.Size(136, 24);
			this.showTableStructure.TabIndex = 3;
			this.showTableStructure.Text = "Show Table Structure";
			this.showTableStructure.CheckedChanged += new System.EventHandler(this.ShowTableStructure_Click);
			// 
			// buttonCloseTable
			// 
			this.buttonCloseTable.Location = new System.Drawing.Point(104, 8);
			this.buttonCloseTable.Name = "buttonCloseTable";
			this.buttonCloseTable.Size = new System.Drawing.Size(88, 24);
			this.buttonCloseTable.TabIndex = 4;
			this.buttonCloseTable.Text = "Close Table";
			this.buttonCloseTable.Click += new System.EventHandler(this.buttonCloseTable_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(232, 8);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(112, 24);
			this.buttonNext.TabIndex = 6;
			this.buttonNext.Text = "Display Next Table";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(752, 494);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonCloseTable);
			this.Controls.Add(this.showTableStructure);
			this.Controls.Add(this.ExitButton);
			this.Controls.Add(this.dataGrid);
			this.Controls.Add(this.OpenTable);
			this.Name = "Form1";
			this.Text = "Open Table Sample";
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
			Application.Run(new Form1());
		}
		/// <summary>
		/// Set the data from the specified table into the data grid.
		/// Show the table's scheme if showSchema is true.
		/// </summary>
		/// <param name="miTable"></param>
		/// <param name="showSchema"></param>
		private void SetGrid(MapInfo.Data.Table miTable, bool showSchema)
		{
			dataGrid.CaptionText = miTable.Alias;
			this.miCommand.CommandText = "Select * from " + miTable.Alias;
			MapInfo.Data.MIDataReader miReader = this.miCommand.ExecuteReader();
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
			if (showSchema) 
			{
				dataGrid.DataSource = miReader.GetSchemaTable();
			}
			else 
			{
				dataGrid.DataSource = dt;
			}
			miReader.Close();
		}
		/// <summary>
		/// Handle a click of the Open Table button
		/// </summary>
		private void OpenTable_Click(object sender, System.EventArgs e)
		{
			string filename = null;
			string s;
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.DefaultExt = "tab";
			// The Filter property requires a search string after the pipe ( | )
			openFile.Filter = "MapInfo Tables (*.tab)|*.tab";
			openFile.Multiselect = false;
			openFile.ShowDialog();
			if( openFile.FileName.Length > 0 )
			{
				MapInfo.Data.Table miTable = null;
				filename = openFile.FileName;
				if (Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, out s)) 
				{
					miTable = Session.Current.Catalog.OpenTable(s);
				}
				else
				{
					// try anyway, at least we will get an exception to report
					miTable = Session.Current.Catalog.OpenTable(filename, "OpenTableTable");
				}

				_tableAlias = miTable.Alias;
				ResetTableEnum();
				SetGrid(_tableEnum.Current, this.showTableStructure.Checked);
				UpdateNavigationButtons();
			}
		}
		/// <summary>
		/// Handle a click of the Exit button
		/// </summary>
		private void ExitButton_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		/// <summary>
		/// Handle a click of the Close Table button
		/// </summary>
		private void buttonCloseTable_Click(object sender, System.EventArgs e)
		{
			_tableEnum.Current.Close();
			if (OpenTableCount > 0) 
			{
				ResetTableEnum();
				SetGrid(_tableEnum.Current, this.showTableStructure.Checked);
			}
			else 
			{
				dataGrid.DataSource = null;
				dataGrid.CaptionText = "";
			}
			UpdateNavigationButtons();
		}
		/// <summary>
		/// Reset the _tableEnum field to the table being displayed, if any
		/// </summary>
		private void ResetTableEnum()
		{
			bool bFound = false;
			int index = 0;
			
			_tableEnum = null;
			_tableEnum = Session.Current.Catalog.EnumerateTables(
				TableFilterFactory.FilterAllTables());

			while (_tableEnum.MoveNext()) 
			{
				index++;
				if (String.Equals(_tableEnum.Current.Alias, _tableAlias))
				{
					bFound = true;
					break;
				}
			}
			if (!bFound) 
			{
				_tableEnum.Reset();
				_tableEnum.MoveNext();
				_tableIndex = 1;
			}
			else 
			{
				_tableIndex = index;
			}
		}
		/// <summary>
		/// Enable the Display Next button iff there is more than one table open.
		/// Enable the Close Table button iff there is at least one table open.
		/// </summary>
		private void UpdateNavigationButtons()
		{
			buttonNext.Enabled = ((OpenTableCount > 1) && (_tableIndex <= OpenTableCount));
			buttonCloseTable.Enabled = (OpenTableCount > 0);
		}
		/// <summary>
		/// Handle a click of the Display Next button
		/// </summary>
		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			if ( ! _tableEnum.MoveNext())
			{
				_tableEnum.Reset();
				_tableEnum.MoveNext();
			}
			SetGrid(_tableEnum.Current, this.showTableStructure.Checked);
			_tableIndex = (_tableIndex + 1) % OpenTableCount;
			_tableAlias = _tableEnum.Current.Alias;
			UpdateNavigationButtons();
		}
		/// <summary>
		///  Handle a click of the Show Table Structure button.
		/// </summary>
		private void ShowTableStructure_Click(object sender, System.EventArgs e)
		{
			if (OpenTableCount > 0) 
			{
      // Make sure the currently displayed table is showing its data if the box is not
      // checked or its structure if the box is checked
				SetGrid(_tableEnum.Current, this.showTableStructure.Checked);	
			}
		}
	}
}
