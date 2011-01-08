using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MapInfo.Data;
using MapInfo.Data.Find;
using MapInfo.Engine;


namespace SeeMapCell
{
    public partial class SeeMapCell : Form
    {
        public SeeMapCell()
        {
            InitializeComponent();
        }

        private void buttonOpenSearchTable_Click(object sender, EventArgs e)
        {
            Table searchTable;
            if (OpenTable(out searchTable))
            {
                if (searchTable == null)
                {
                    MessageBox.Show("Please specify a valid search table.");
                    return;
                }
                if (searchTable.IsMappable)
                {
                    _searchTable = searchTable;
                    //				searchTable.Close()
                    textBoxSearchTable.Text = "";
                    comboBoxSearchColumn.Items.Clear();
                    comboBoxSearchColumn.Text = "";
                    textBoxSearchString.Text = "";
                    textBoxSearchTable.Text = _searchTable.TableInfo.Alias;
                    SetColumnField(_searchTable, comboBoxSearchColumn);
                }
                else
                {
                    MessageBox.Show(String.Format("Table " + _searchTable.Alias + " is not mappable."));
                }
            }
        }
        private Table _searchTable;
        private Table _refiningTable;
        private FindResult _result;
        private bool _bSearchIntersection = false;
        private MICommand _miCommand;
        private MIConnection _miConnection;
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
                if (openFile.FileName.Length > 0)
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
        private void SetColumnField(Table table, System.Windows.Forms.ComboBox comboBox)
        {
            Columns columns = table.TableInfo.Columns;
            foreach (Column column in columns)
            {
                if (column.Indexed)
                {
                    comboBox.Items.Add(column.Alias);
                }
            }

            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(String.Format("No indexed columns in " + table.Alias + "."));
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)        
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
            MapInfo.Mapping.Map map = Session.Current.MapFactory.CreateEmptyMap(System.IntPtr.Zero, new Size(10, 10));
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

            MICommand cmd = _miConnection.CreateCommand();
            cmd.Parameters.Add("geometry", MIDbType.FeatureGeometry);
            cmd.Parameters.Add("style", MIDbType.Style);
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
            mapForm.Size = new Size(500, 500);

            //Show the form like a dialog (modal)			
            mapForm.ShowDialog(parentForm);

            pointTable.Close();

        }
    }
}
