using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MapInfo.Data;
using MapInfo.Data.Find;
using MapInfo.Engine;

namespace FindCS
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormFindSample : System.Windows.Forms.Form
	{
		/// <summary>
		/// The MICommand variable is used to generate cursors against open tables.
		/// </summary>
		private MICommand _miCommand;
		private MIConnection _miConnection;

		// search table
		private Table _searchTable;
		private Table _refiningTable;
		private FindResult _result;
		private bool _bSearchIntersection = false;

		private System.Windows.Forms.Label labelSearchTable;
		private System.Windows.Forms.TextBox textBoxSearchTable;
		private System.Windows.Forms.Label labelSearchColumn;
		private System.Windows.Forms.TextBox textBoxSearchString;
		private System.Windows.Forms.Label labelSearchString;
		private System.Windows.Forms.ListBox listBoxSearchResult;
		private System.Windows.Forms.ComboBox comboBoxSearchColumn;
		private System.Windows.Forms.Button buttonFind;
		private System.Windows.Forms.CheckBox checkBoxUseCloseMatches;
		private System.Windows.Forms.TextBox textBoxMaxCloseMatches;
		private System.Windows.Forms.Label labelMaxCloseMatches;
		private System.Windows.Forms.Label labelSearchResult;
		private System.Windows.Forms.Button buttonOpenSearchTable;
		private System.Windows.Forms.GroupBox groupBoxSearchParameters;
		private System.Windows.Forms.GroupBox groupBoxRefiningParameters;
		private System.Windows.Forms.ComboBox comboBoxRefiningColumn;
		private System.Windows.Forms.TextBox textBoxRefiningString;
		private System.Windows.Forms.Label labelRefiningString;
		private System.Windows.Forms.Label labelRefiningColumn;
		private System.Windows.Forms.Button buttonOpenRefiningTable;
		private System.Windows.Forms.TextBox textBoxRefiningTable;
		private System.Windows.Forms.Label labelRefiningTable;
		private System.Windows.Forms.GroupBox groupBoxPreferences;
		private System.Windows.Forms.GroupBox groupBoxSearchResults;
		private System.Windows.Forms.CheckBox checkBoxAddressNumAfterStreet;
		private System.Windows.Forms.TextBox textBoxIntersection;
		private System.Windows.Forms.CheckBox checkBoxIntersection;
		private System.Windows.Forms.Label labelMultipleMatchesFound;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormFindSample()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			//
			// Create a connection object and open it.
			//
			this._miConnection = new MIConnection();
			_miConnection.Open();

			//
			// Create a command object to hold parameters and last command executed
			//
			this._miCommand = _miConnection.CreateCommand();

			buttonFind.Enabled = false;
			checkBoxUseCloseMatches.Checked = true;
			listBoxSearchResult.Visible = false;
			textBoxMaxCloseMatches.Text = "5";
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
            this.labelSearchTable = new System.Windows.Forms.Label();
            this.textBoxSearchTable = new System.Windows.Forms.TextBox();
            this.buttonOpenSearchTable = new System.Windows.Forms.Button();
            this.labelSearchColumn = new System.Windows.Forms.Label();
            this.textBoxSearchString = new System.Windows.Forms.TextBox();
            this.labelSearchString = new System.Windows.Forms.Label();
            this.listBoxSearchResult = new System.Windows.Forms.ListBox();
            this.comboBoxSearchColumn = new System.Windows.Forms.ComboBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.labelSearchResult = new System.Windows.Forms.Label();
            this.checkBoxUseCloseMatches = new System.Windows.Forms.CheckBox();
            this.labelMaxCloseMatches = new System.Windows.Forms.Label();
            this.textBoxMaxCloseMatches = new System.Windows.Forms.TextBox();
            this.groupBoxSearchParameters = new System.Windows.Forms.GroupBox();
            this.checkBoxIntersection = new System.Windows.Forms.CheckBox();
            this.textBoxIntersection = new System.Windows.Forms.TextBox();
            this.groupBoxRefiningParameters = new System.Windows.Forms.GroupBox();
            this.comboBoxRefiningColumn = new System.Windows.Forms.ComboBox();
            this.textBoxRefiningString = new System.Windows.Forms.TextBox();
            this.labelRefiningString = new System.Windows.Forms.Label();
            this.labelRefiningColumn = new System.Windows.Forms.Label();
            this.buttonOpenRefiningTable = new System.Windows.Forms.Button();
            this.textBoxRefiningTable = new System.Windows.Forms.TextBox();
            this.labelRefiningTable = new System.Windows.Forms.Label();
            this.groupBoxPreferences = new System.Windows.Forms.GroupBox();
            this.checkBoxAddressNumAfterStreet = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchResults = new System.Windows.Forms.GroupBox();
            this.labelMultipleMatchesFound = new System.Windows.Forms.Label();
            this.groupBoxSearchParameters.SuspendLayout();
            this.groupBoxRefiningParameters.SuspendLayout();
            this.groupBoxPreferences.SuspendLayout();
            this.groupBoxSearchResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSearchTable
            // 
            this.labelSearchTable.Location = new System.Drawing.Point(19, 34);
            this.labelSearchTable.Name = "labelSearchTable";
            this.labelSearchTable.Size = new System.Drawing.Size(120, 25);
            this.labelSearchTable.TabIndex = 1;
            this.labelSearchTable.Text = "Table:";
            // 
            // textBoxSearchTable
            // 
            this.textBoxSearchTable.Location = new System.Drawing.Point(77, 34);
            this.textBoxSearchTable.Name = "textBoxSearchTable";
            this.textBoxSearchTable.Size = new System.Drawing.Size(144, 21);
            this.textBoxSearchTable.TabIndex = 2;
            // 
            // buttonOpenSearchTable
            // 
            this.buttonOpenSearchTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOpenSearchTable.Location = new System.Drawing.Point(230, 34);
            this.buttonOpenSearchTable.Name = "buttonOpenSearchTable";
            this.buttonOpenSearchTable.Size = new System.Drawing.Size(90, 25);
            this.buttonOpenSearchTable.TabIndex = 3;
            this.buttonOpenSearchTable.Text = "Open Table";
            this.buttonOpenSearchTable.Click += new System.EventHandler(this.buttonOpenSearchTable_Click);
            // 
            // labelSearchColumn
            // 
            this.labelSearchColumn.Location = new System.Drawing.Point(19, 69);
            this.labelSearchColumn.Name = "labelSearchColumn";
            this.labelSearchColumn.Size = new System.Drawing.Size(120, 25);
            this.labelSearchColumn.TabIndex = 4;
            this.labelSearchColumn.Text = "Column:";
            // 
            // textBoxSearchString
            // 
            this.textBoxSearchString.Location = new System.Drawing.Point(77, 103);
            this.textBoxSearchString.Name = "textBoxSearchString";
            this.textBoxSearchString.Size = new System.Drawing.Size(144, 21);
            this.textBoxSearchString.TabIndex = 7;
            this.textBoxSearchString.TextChanged += new System.EventHandler(this.textBoxSearchString_TextChanged);
            // 
            // labelSearchString
            // 
            this.labelSearchString.Location = new System.Drawing.Point(19, 103);
            this.labelSearchString.Name = "labelSearchString";
            this.labelSearchString.Size = new System.Drawing.Size(120, 25);
            this.labelSearchString.TabIndex = 6;
            this.labelSearchString.Text = "String:";
            // 
            // listBoxSearchResult
            // 
            this.listBoxSearchResult.ItemHeight = 12;
            this.listBoxSearchResult.Location = new System.Drawing.Point(192, 26);
            this.listBoxSearchResult.Name = "listBoxSearchResult";
            this.listBoxSearchResult.Size = new System.Drawing.Size(250, 76);
            this.listBoxSearchResult.TabIndex = 0;
            this.listBoxSearchResult.DoubleClick += new System.EventHandler(this.listBoxSearchResult_DoubleClick);
            // 
            // comboBoxSearchColumn
            // 
            this.comboBoxSearchColumn.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxSearchColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchColumn.ItemHeight = 12;
            this.comboBoxSearchColumn.Location = new System.Drawing.Point(77, 69);
            this.comboBoxSearchColumn.Name = "comboBoxSearchColumn";
            this.comboBoxSearchColumn.Size = new System.Drawing.Size(145, 20);
            this.comboBoxSearchColumn.TabIndex = 5;
            this.comboBoxSearchColumn.TextChanged += new System.EventHandler(this.comboBoxSearchColumn_TextChanged);
            // 
            // buttonFind
            // 
            this.buttonFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonFind.Location = new System.Drawing.Point(10, 353);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(90, 25);
            this.buttonFind.TabIndex = 1;
            this.buttonFind.Text = "Find";
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // labelSearchResult
            // 
            this.labelSearchResult.Location = new System.Drawing.Point(19, 26);
            this.labelSearchResult.Name = "labelSearchResult";
            this.labelSearchResult.Size = new System.Drawing.Size(163, 26);
            this.labelSearchResult.TabIndex = 0;
            // 
            // checkBoxUseCloseMatches
            // 
            this.checkBoxUseCloseMatches.Location = new System.Drawing.Point(19, 233);
            this.checkBoxUseCloseMatches.Name = "checkBoxUseCloseMatches";
            this.checkBoxUseCloseMatches.Size = new System.Drawing.Size(154, 25);
            this.checkBoxUseCloseMatches.TabIndex = 10;
            this.checkBoxUseCloseMatches.Text = "Use Close Matches";
            this.checkBoxUseCloseMatches.CheckedChanged += new System.EventHandler(this.checkBoxUseCloseMatches_CheckedChanged);
            // 
            // labelMaxCloseMatches
            // 
            this.labelMaxCloseMatches.Location = new System.Drawing.Point(19, 258);
            this.labelMaxCloseMatches.Name = "labelMaxCloseMatches";
            this.labelMaxCloseMatches.Size = new System.Drawing.Size(125, 25);
            this.labelMaxCloseMatches.TabIndex = 11;
            this.labelMaxCloseMatches.Text = "Max close matches:";
            this.labelMaxCloseMatches.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMaxCloseMatches
            // 
            this.textBoxMaxCloseMatches.Location = new System.Drawing.Point(144, 258);
            this.textBoxMaxCloseMatches.Name = "textBoxMaxCloseMatches";
            this.textBoxMaxCloseMatches.Size = new System.Drawing.Size(29, 21);
            this.textBoxMaxCloseMatches.TabIndex = 0;
            this.textBoxMaxCloseMatches.Leave += new System.EventHandler(this.textBoxMaxCloseMatches_Leave);
            // 
            // groupBoxSearchParameters
            // 
            this.groupBoxSearchParameters.Controls.Add(this.checkBoxIntersection);
            this.groupBoxSearchParameters.Controls.Add(this.textBoxIntersection);
            this.groupBoxSearchParameters.Location = new System.Drawing.Point(10, 9);
            this.groupBoxSearchParameters.Name = "groupBoxSearchParameters";
            this.groupBoxSearchParameters.Size = new System.Drawing.Size(326, 198);
            this.groupBoxSearchParameters.TabIndex = 0;
            this.groupBoxSearchParameters.TabStop = false;
            this.groupBoxSearchParameters.Text = "Search Parameters";
            // 
            // checkBoxIntersection
            // 
            this.checkBoxIntersection.Location = new System.Drawing.Point(19, 129);
            this.checkBoxIntersection.Name = "checkBoxIntersection";
            this.checkBoxIntersection.Size = new System.Drawing.Size(192, 26);
            this.checkBoxIntersection.TabIndex = 1;
            this.checkBoxIntersection.Text = "Use intersection:";
            this.checkBoxIntersection.CheckedChanged += new System.EventHandler(this.checkBoxIntersection_CheckedChanged);
            // 
            // textBoxIntersection
            // 
            this.textBoxIntersection.Enabled = false;
            this.textBoxIntersection.Location = new System.Drawing.Point(67, 164);
            this.textBoxIntersection.Name = "textBoxIntersection";
            this.textBoxIntersection.Size = new System.Drawing.Size(144, 21);
            this.textBoxIntersection.TabIndex = 0;
            // 
            // groupBoxRefiningParameters
            // 
            this.groupBoxRefiningParameters.Controls.Add(this.comboBoxRefiningColumn);
            this.groupBoxRefiningParameters.Controls.Add(this.textBoxRefiningString);
            this.groupBoxRefiningParameters.Controls.Add(this.labelRefiningString);
            this.groupBoxRefiningParameters.Controls.Add(this.labelRefiningColumn);
            this.groupBoxRefiningParameters.Controls.Add(this.buttonOpenRefiningTable);
            this.groupBoxRefiningParameters.Controls.Add(this.textBoxRefiningTable);
            this.groupBoxRefiningParameters.Controls.Add(this.labelRefiningTable);
            this.groupBoxRefiningParameters.Location = new System.Drawing.Point(355, 9);
            this.groupBoxRefiningParameters.Name = "groupBoxRefiningParameters";
            this.groupBoxRefiningParameters.Size = new System.Drawing.Size(327, 198);
            this.groupBoxRefiningParameters.TabIndex = 8;
            this.groupBoxRefiningParameters.TabStop = false;
            this.groupBoxRefiningParameters.Text = "Refining Parameters (Optional)";
            // 
            // comboBoxRefiningColumn
            // 
            this.comboBoxRefiningColumn.ItemHeight = 12;
            this.comboBoxRefiningColumn.Location = new System.Drawing.Point(67, 60);
            this.comboBoxRefiningColumn.Name = "comboBoxRefiningColumn";
            this.comboBoxRefiningColumn.Size = new System.Drawing.Size(145, 20);
            this.comboBoxRefiningColumn.TabIndex = 4;
            // 
            // textBoxRefiningString
            // 
            this.textBoxRefiningString.Location = new System.Drawing.Point(67, 95);
            this.textBoxRefiningString.Name = "textBoxRefiningString";
            this.textBoxRefiningString.Size = new System.Drawing.Size(144, 21);
            this.textBoxRefiningString.TabIndex = 6;
            // 
            // labelRefiningString
            // 
            this.labelRefiningString.Location = new System.Drawing.Point(10, 95);
            this.labelRefiningString.Name = "labelRefiningString";
            this.labelRefiningString.Size = new System.Drawing.Size(120, 25);
            this.labelRefiningString.TabIndex = 5;
            this.labelRefiningString.Text = "String:";
            // 
            // labelRefiningColumn
            // 
            this.labelRefiningColumn.Location = new System.Drawing.Point(10, 60);
            this.labelRefiningColumn.Name = "labelRefiningColumn";
            this.labelRefiningColumn.Size = new System.Drawing.Size(120, 25);
            this.labelRefiningColumn.TabIndex = 3;
            this.labelRefiningColumn.Text = "Column:";
            // 
            // buttonOpenRefiningTable
            // 
            this.buttonOpenRefiningTable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOpenRefiningTable.Location = new System.Drawing.Point(221, 26);
            this.buttonOpenRefiningTable.Name = "buttonOpenRefiningTable";
            this.buttonOpenRefiningTable.Size = new System.Drawing.Size(90, 25);
            this.buttonOpenRefiningTable.TabIndex = 2;
            this.buttonOpenRefiningTable.Text = "Open Table";
            this.buttonOpenRefiningTable.Click += new System.EventHandler(this.buttonOpenRefiningTable_Click);
            // 
            // textBoxRefiningTable
            // 
            this.textBoxRefiningTable.Location = new System.Drawing.Point(67, 26);
            this.textBoxRefiningTable.Name = "textBoxRefiningTable";
            this.textBoxRefiningTable.Size = new System.Drawing.Size(144, 21);
            this.textBoxRefiningTable.TabIndex = 1;
            // 
            // labelRefiningTable
            // 
            this.labelRefiningTable.Location = new System.Drawing.Point(10, 26);
            this.labelRefiningTable.Name = "labelRefiningTable";
            this.labelRefiningTable.Size = new System.Drawing.Size(120, 25);
            this.labelRefiningTable.TabIndex = 0;
            this.labelRefiningTable.Text = "Table:";
            // 
            // groupBoxPreferences
            // 
            this.groupBoxPreferences.Controls.Add(this.checkBoxAddressNumAfterStreet);
            this.groupBoxPreferences.Location = new System.Drawing.Point(10, 215);
            this.groupBoxPreferences.Name = "groupBoxPreferences";
            this.groupBoxPreferences.Size = new System.Drawing.Size(201, 130);
            this.groupBoxPreferences.TabIndex = 9;
            this.groupBoxPreferences.TabStop = false;
            this.groupBoxPreferences.Text = "Preferences";
            // 
            // checkBoxAddressNumAfterStreet
            // 
            this.checkBoxAddressNumAfterStreet.Location = new System.Drawing.Point(10, 78);
            this.checkBoxAddressNumAfterStreet.Name = "checkBoxAddressNumAfterStreet";
            this.checkBoxAddressNumAfterStreet.Size = new System.Drawing.Size(172, 34);
            this.checkBoxAddressNumAfterStreet.TabIndex = 0;
            this.checkBoxAddressNumAfterStreet.Text = "Address number after street";
            // 
            // groupBoxSearchResults
            // 
            this.groupBoxSearchResults.Controls.Add(this.labelMultipleMatchesFound);
            this.groupBoxSearchResults.Controls.Add(this.listBoxSearchResult);
            this.groupBoxSearchResults.Controls.Add(this.labelSearchResult);
            this.groupBoxSearchResults.Location = new System.Drawing.Point(221, 215);
            this.groupBoxSearchResults.Name = "groupBoxSearchResults";
            this.groupBoxSearchResults.Size = new System.Drawing.Size(461, 130);
            this.groupBoxSearchResults.TabIndex = 14;
            this.groupBoxSearchResults.TabStop = false;
            this.groupBoxSearchResults.Text = "Search Results";
            // 
            // labelMultipleMatchesFound
            // 
            this.labelMultipleMatchesFound.Location = new System.Drawing.Point(10, 60);
            this.labelMultipleMatchesFound.Name = "labelMultipleMatchesFound";
            this.labelMultipleMatchesFound.Size = new System.Drawing.Size(172, 25);
            this.labelMultipleMatchesFound.TabIndex = 1;
            this.labelMultipleMatchesFound.Text = "Multiple matches found";
            this.labelMultipleMatchesFound.Visible = false;
            // 
            // FormFindSample
            // 
            this.AcceptButton = this.buttonFind;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(576, 410);
            this.Controls.Add(this.textBoxMaxCloseMatches);
            this.Controls.Add(this.textBoxSearchString);
            this.Controls.Add(this.textBoxSearchTable);
            this.Controls.Add(this.labelMaxCloseMatches);
            this.Controls.Add(this.checkBoxUseCloseMatches);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.comboBoxSearchColumn);
            this.Controls.Add(this.labelSearchString);
            this.Controls.Add(this.labelSearchColumn);
            this.Controls.Add(this.buttonOpenSearchTable);
            this.Controls.Add(this.labelSearchTable);
            this.Controls.Add(this.groupBoxSearchParameters);
            this.Controls.Add(this.groupBoxRefiningParameters);
            this.Controls.Add(this.groupBoxPreferences);
            this.Controls.Add(this.groupBoxSearchResults);
            this.Name = "FormFindSample";
            this.Text = "Find Example";
            this.Enter += new System.EventHandler(this.buttonFind_Click);
            this.groupBoxSearchParameters.ResumeLayout(false);
            this.groupBoxSearchParameters.PerformLayout();
            this.groupBoxRefiningParameters.ResumeLayout(false);
            this.groupBoxRefiningParameters.PerformLayout();
            this.groupBoxPreferences.ResumeLayout(false);
            this.groupBoxSearchResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			FormFindSample findSample = new FormFindSample();
			Application.Run(findSample);
			// clean up
			findSample.CleanUp();
		}

		private void CleanUp()
		{
			if ( _miCommand != null )
			{
				_miCommand.Dispose();
				_miCommand = null;
			}
			if ( _miConnection != null )
			{
				_miConnection.Close();
				_miConnection = null;
			}
			Session.Current.Catalog.CloseAll();
			Session.Dispose();

		}
		private void buttonOpenSearchTable_Click(object sender, System.EventArgs e)
		{
			Table searchTable;
			if (OpenTable(out searchTable)) 
			{
				if ( searchTable == null )
				{
					MessageBox.Show("Please specify a valid search table.");
					return;
				}
				if ( searchTable.IsMappable )
				{
					_searchTable = searchTable;
					//				searchTable.Close()
					textBoxSearchTable.Text = "";
					comboBoxSearchColumn.Items.Clear();
					comboBoxSearchColumn.Text = "";
					textBoxSearchString.Text = "";
					textBoxSearchTable.Text = _searchTable.TableInfo.Alias;
					SetColumnField(_searchTable,comboBoxSearchColumn);
				}
				else
				{
					MessageBox.Show(String.Format("Table " + _searchTable.Alias + " is not mappable."));
				}
			}
		}

		private void SetColumnField(Table table, System.Windows.Forms.ComboBox comboBox)
		{
			Columns columns = table.TableInfo.Columns;
			foreach ( Column column in columns)
			{
				if ( column.Indexed )
				{
					comboBox.Items.Add(column.Alias);
				}
			}
			
			if ( comboBox.Items.Count > 0 )
			{
				comboBox.SelectedIndex = 0;
			}
			else
			{
				MessageBox.Show(String.Format("No indexed columns in " + table.Alias + "."));
			}
		}

		private void textBoxSearchString_TextChanged(object sender, System.EventArgs e)
		{
			enableButtonFind();
		}

		private void comboBoxSearchColumn_TextChanged(object sender, System.EventArgs e)
		{
			enableButtonFind();
		}

		private void enableButtonFind()
		{
			if (textBoxSearchString.Text.Length > 0 && 
				textBoxSearchTable.Text.Length > 0 &&
				comboBoxSearchColumn.Items.Count > 0)
			{
				buttonFind.Enabled = true;
			}
			else
			{
				buttonFind.Enabled = false;
			}
		}

		private void buttonFind_Click(object sender, System.EventArgs e)
		{
			listBoxSearchResult.Items.Clear();
			listBoxSearchResult.Visible = false;
			Find find = null;
			Column searchColumn = _searchTable.TableInfo.Columns[comboBoxSearchColumn.SelectedItem.ToString()];
			if ( _refiningTable != null && _refiningTable.IsOpen )
			{
				if (comboBoxRefiningColumn.Items.Count > 0 )
				{
					Column refiningColumn = _refiningTable.TableInfo.Columns[comboBoxRefiningColumn.SelectedItem.ToString()];
					find = new Find(_searchTable, searchColumn, _refiningTable, refiningColumn);
				}
				else
				{
					MessageBox.Show(String.Format("No indexed columns in " + _refiningTable + "."));
					return;
				}

			}
			else
			{
				find = new Find(_searchTable, searchColumn);
			}

			if ( checkBoxUseCloseMatches.Checked )
			{
				find.UseCloseMatches = true;
				find.CloseMatchesMax = int.Parse(textBoxMaxCloseMatches.Text);
			}

			if ( checkBoxAddressNumAfterStreet.Checked )
			{
				find.AddressNumberAfterStreet = true;
			}

			// Do the actual search.
			if ( _refiningTable != null && _refiningTable.IsOpen )
			{
				if (_bSearchIntersection) 
				{
					_result= find.SearchIntersection(textBoxSearchString.Text, textBoxIntersection.Text, textBoxRefiningString.Text);
				} 
				else 
				{
					_result = find.Search(textBoxSearchString.Text, textBoxRefiningString.Text);
				}
			}
			else
			{
				if (_bSearchIntersection) 
				{
					_result= find.SearchIntersection(textBoxSearchString.Text, textBoxIntersection.Text);
				} 
				else 
				{
					_result = find.Search(textBoxSearchString.Text);
				}
			}

			// display label that tells us when multiple matches were found
			labelMultipleMatchesFound.Visible = _result.MultipleMatches;

			if ( _result.ExactMatch 
				&& _result.NameResultCode.Equals(FindNameCode.ExactMatch) 
				&& _result.FoundPoint != null)
			{
				labelSearchResult.Text = "Exact Match";
				showPointOnSearchTableMap(_result.FoundPoint.X, _result.FoundPoint.Y);				
			}
			else if (_result.NameResultCode.Equals(FindNameCode.ExactMatch) 
				&& _result.AddressResultCode.Equals(FindAddressCode.AddressNumNotSpecified))
			{
				labelSearchResult.Text = _result.AddressResultCode.ToString();
				FindAddressRangeEnumerator _enum = _result.GetAddressRangeEnumerator();
				FindAddressRange _findAddressRange;

				listBoxSearchResult.Visible = true;
				while (_enum.MoveNext()) 
				{
					_findAddressRange = _enum.Current;
					listBoxSearchResult.Items.Add(
						String.Format("Address range: [{0} - {1}]", _findAddressRange.BeginRange, _findAddressRange.EndRange));
				}
			}
			else if (_result.NameResultCode.Equals(FindNameCode.ExactMatch) 
				&& _result.MultipleMatches)
			{
				labelSearchResult.Text = _result.NameResultCode.ToString();
				listBoxSearchResult.Visible = true;
				FindCloseMatchEnumerator enumerator = _result.GetCloseMatchEnumerator();
				while ( enumerator.MoveNext() )
				{
					listBoxSearchResult.Items.Add(enumerator.Current.Name);
				}
			}
			else
			{
				labelSearchResult.Text = _result.NameResultCode.ToString();
				if ( find.UseCloseMatches )
				{
					listBoxSearchResult.Visible = true;
					FindCloseMatchEnumerator enumerator = _result.GetCloseMatchEnumerator();
					while ( enumerator.MoveNext() )
					{
						listBoxSearchResult.Items.Add(enumerator.Current.Name);
					}
				}
			}
			find.Dispose();
		}

		private void showPointOnSearchTableMap(double x, double y)
		{
			MapInfo.Mapping.Map map = Session.Current.MapFactory.CreateEmptyMap(System.IntPtr.Zero,  new Size(10,10));
			MapInfo.Mapping.FeatureLayer searchLayer = new MapInfo.Mapping.FeatureLayer(_searchTable);
			map.Layers.Add(searchLayer);
				
			MapForm1 mapForm = new MapForm1(map);
			Form parentForm = this;

			// create a temp table and add a featurelayer for it (use map alias as table alias)
			// make the table hidden (maybe)
			MapInfo.Geometry.CoordSys coordSys = map.GetDisplayCoordSys();
			TableInfoMemTable ti = new TableInfoMemTable("temp");
			ti.Temporary = true;

			// add object column
			Column col;
			col = new GeometryColumn(coordSys); // specify coordsys for object column
			col.Alias = "obj";
			col.DataType = MIDbType.FeatureGeometry;
			ti.Columns.Add(col);
            
			// add style column
			col = new Column();
			col.Alias = "MI_Style";
			col.DataType = MIDbType.Style;
			ti.Columns.Add(col);
            
			Table pointTable = Session.Current.Catalog.CreateTable(ti);

			// I am using a Point example here. You can create a rectangle instead
			MapInfo.Geometry.FeatureGeometry g = new MapInfo.Geometry.Point(coordSys, x, y);
			MapInfo.Styles.SimpleVectorPointStyle vs = new MapInfo.Styles.SimpleVectorPointStyle(37, System.Drawing.Color.Red, 14);
			MapInfo.Styles.CompositeStyle cs = new MapInfo.Styles.CompositeStyle(vs);
			
			MICommand cmd =  _miConnection.CreateCommand();
			cmd.Parameters.Add("geometry",MIDbType.FeatureGeometry);
			cmd.Parameters.Add("style",MIDbType.Style);
			cmd.CommandText = "Insert Into temp (obj,MI_Style) values (geometry,style)";
			cmd.Prepare();
			cmd.Parameters[0].Value = g;
			cmd.Parameters[1].Value = cs;
			int nchanged = cmd.ExecuteNonQuery();
			cmd.Dispose();

			map.Layers.Add(new MapInfo.Mapping.FeatureLayer(pointTable));
			// another way: Map.Load(new MapTableLoader(table));

			// make the map encompass the entire search layer
			map.SetView(searchLayer);

			// size the map form
			mapForm.Size = new Size(500,500);

			//Show the form like a dialog (modal)			
			mapForm.ShowDialog(parentForm);		
		
			pointTable.Close();

		}

		private void checkBoxUseCloseMatches_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( checkBoxUseCloseMatches.Checked )
			{
				labelMaxCloseMatches.Enabled = true;
				textBoxMaxCloseMatches.Enabled = true;
			}
			else
			{
				labelMaxCloseMatches.Enabled = false;
				textBoxMaxCloseMatches.Enabled = false;
			}
		}

		private void textBoxMaxCloseMatches_Leave(object sender, System.EventArgs e)
		{
			try
			{
				int closeMatches = int.Parse(textBoxMaxCloseMatches.Text);
			}
			catch(FormatException)
			{
				MessageBox.Show("Invalid number entered.");
				textBoxMaxCloseMatches.Focus();
				textBoxMaxCloseMatches.SelectAll();
			}
		}

		// returns true when a table was attempted to be opened, false when OpenFileDialog cancelled.
		private bool OpenTable(out Table table)
		{
			string filename = null;
			string s;
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.DefaultExt = "tab";
			// The Filter property requires a search string after the pipe ( | )
			openFile.Filter = "MapInfo Tables (*.tab)|*.tab";
			openFile.Multiselect = false;
			table = null;
			if (openFile.ShowDialog() == DialogResult.OK) 
			{
				if( openFile.FileName.Length > 0 )
				{
					filename = openFile.FileName;
					//				MessageBox.Show(filename);
					if (Session.Current.TableSearchPath.FileExists(System.IO.Directory.GetCurrentDirectory(), filename, out s)) 
					{
						//					MessageBox.Show("file exists");
						table = Session.Current.Catalog.OpenTable(s);
					}
					else
					{
						// try anyway, at least we will get an exception to report
						table = Session.Current.Catalog.OpenTable(filename);
					}
					return true;
				}
			}
			return false;
		}
		private void buttonOpenRefiningTable_Click(object sender, System.EventArgs e)
		{
			Table refiningTable;

			if (OpenTable(out refiningTable)) 
			{
				if ( refiningTable == null )
				{
					MessageBox.Show("Please specify a valid refining table.");
					return;
				}
				if ( refiningTable.IsMappable )
				{
					_refiningTable = refiningTable;
					textBoxRefiningTable.Text = "";
					comboBoxRefiningColumn.Items.Clear();
					comboBoxRefiningColumn.Text = "";
					textBoxRefiningString.Text = "";
					textBoxRefiningTable.Text = _refiningTable.TableInfo.Alias;
					SetColumnField(_refiningTable, comboBoxRefiningColumn);
				}
				else
				{
					MessageBox.Show(String.Format("Table " + _refiningTable.Alias + " is not mappable."));
				}
			}
		}

		private void listBoxSearchResult_DoubleClick(object sender, System.EventArgs e)
		{
			// get the selected object
			string selectedCloseMatch = (string)this.listBoxSearchResult.SelectedItem;
			// get the selected object index in the list as well since multiple matches
			// may have the same name - we can't just compare close match names.
			int selectedIndex = this.listBoxSearchResult.SelectedIndex;
			int currentIndex = 0;

			// create a close match object
			FindCloseMatch closeMatch = null;

			// create an enumerator from the results
			FindCloseMatchEnumerator enumerator = _result.GetCloseMatchEnumerator();
			while ( enumerator.MoveNext() )
			{
				// if the selected name equals the enumerated name...
				if (currentIndex == selectedIndex && selectedCloseMatch.Equals(enumerator.Current.Name) )
				{
					// set the close match object
					closeMatch = enumerator.Current;

					// break out of the loop
					break;
				}
				// else keep looking
				currentIndex++;
			}
			if ( closeMatch != null )
			{
				// create the command string
				string command = "select obj from " + _searchTable.Alias + " where MI_Key = \'" + closeMatch.Key + "\'";

				// create the command object
				MICommand cmd = _miConnection.CreateCommand();
				cmd.CommandText = command;

				// create the reader by executing the command
				MIDataReader rdr = cmd.ExecuteReader();

				// read a row
				rdr.Read();

				// get the point
				MapInfo.Geometry.DPoint point = rdr.GetFeatureGeometry(0).Centroid;

				// Close the reader and dispose of the command.
				rdr.Close();
				cmd.Cancel();
				cmd.Dispose();

				// show point on a map
				showPointOnSearchTableMap(point.x, point.y);
			}
		}

		private void checkBoxIntersection_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxIntersection.Enabled = checkBoxIntersection.Checked;
			_bSearchIntersection = checkBoxIntersection.Checked;
		}

	}
}
