using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MapInfo;
using MapInfo.Data.Find;
using MapInfo.Data;
using MapInfo.Geometry;
using MapInfo.Styles;
using MapInfo.Mapping.Thematics;
using MapInfo.Mapping ;
using MapInfo.Mapping.Legends ;
using MapInfo.Engine;
using MapInfo.WebControls;

namespace DataAccessWeb
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected MapInfo.WebControls.MapControl MapControl1;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Image Image2;
		protected MapInfo.WebControls.ZoomInTool Zoomintool2;
		protected MapInfo.WebControls.ZoomOutTool Zoomouttool2;
		protected MapInfo.WebControls.SouthNavigationTool SouthNavigationTool2;
		protected MapInfo.WebControls.NorthNavigationTool NorthNavigationTool2;
		protected MapInfo.WebControls.EastNavigationTool EastNavigationTool2;
		protected MapInfo.WebControls.WestNavigationTool WestNavigationTool2;
		protected MapInfo.WebControls.NorthEastNavigationTool NorthEastNavigationTool1;
		protected MapInfo.WebControls.SouthWestNavigationTool SouthWestNavigationTool1;
		protected MapInfo.WebControls.SouthEastNavigationTool SouthEastNavigationTool1;
		protected MapInfo.WebControls.NorthWestNavigationTool NorthWestNavigationTool1;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool1;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool2;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool3;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool4;
		protected MapInfo.WebControls.ZoomBarTool ZoomBarTool5;
		protected MapInfo.WebControls.CenterTool Centertool2;
		protected MapInfo.WebControls.PanTool Pantool2;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label DataBaseLabel;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Button OpenTableButton;
		protected System.Web.UI.WebControls.TextBox OpenTableTextBox;
		protected System.Web.UI.WebControls.Repeater Repeater1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Button ApplyButton;
		protected System.Web.UI.WebControls.Label WarningLabel;
		protected System.Web.UI.WebControls.CheckBoxList CheckBoxList1;
		protected System.Web.UI.WebControls.TextBox SelectClauseTextBox;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// The first time in
			if (Session.IsNewSession)
			{
				//******************************************************************************//
				//*   You need to follow below lines in your own application in order to       *//  
				//*   save state manually.                                                     *//
				//*   You don't need this state manager if the "MapInfo.Engine.Session.State"  *//
				//*   in the web.config is not set to "Manual"                                 *//
				//******************************************************************************//
				if(AppStateManager.IsManualState())
				{
					AppStateManager stateManager = new AppStateManager();
					// tell the state manager which map alias you want to use.
					// You could also add your own key/value pairs, the value should be serializable.
					stateManager.ParamsDictionary[AppStateManager.ActiveMapAliasKey] = this.MapControl1.MapAlias;
					// Put state manager into HttpSession, so we could get it later on.
					AppStateManager.PutStateManagerInSession(stateManager);
				}

				// Initialize map setting.
				InitState();

				// WarningLabel is invisible in the init state
				WarningLabel.Visible = false;
			}

			// Restore state.
			if(MapInfo.WebControls.StateManager.IsManualState())
			{
				MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState();
			}

			if(!IsPostBack)
			{
				// DataBind named connection names to RadioButtonList.
				CheckBoxList1.DataSource = MapInfo.Engine.Session.Current.Catalog.NamedConnections.Keys;
				CheckBoxList1.DataBind();

				// Populate aliases of opened tables to Repeater web control.
				BindOpenedTablesAliasToRepeater();
			}
		}

		private void InitState()
		{
			MapInfo.Mapping.Map myMap = this.GetMapObj();

			// We need to put original state of applicatin into HttpApplicationState.
			if(Application.Get("DataAccessWeb") == null)
			{
				System.Collections.IEnumerator iEnum = MapInfo.Engine.Session.Current.MapFactory.GetEnumerator();
				// Put maps into byte[] objects and keep them in HttpApplicationState
				while(iEnum.MoveNext())
				{
					MapInfo.Mapping.Map tempMap = iEnum.Current as MapInfo.Mapping.Map;
					byte[] mapBits = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(tempMap);
					Application.Add(tempMap.Alias, mapBits);
				}

				// Load Named connections into catalog.
				if(MapInfo.Engine.Session.Current.Catalog.NamedConnections.Count == 0)
				{
					System.Web.HttpServerUtility util = HttpContext.Current.Server;
					string path = util.MapPath(string.Format("/DataAccessWebCS_{0}_{1}", MapInfo.Engine.ProductInfo.MajorVersion, MapInfo.Engine.ProductInfo.MinorVersion));
					string fileName = System.IO.Path.Combine(path, "namedconnection.xml");
					MapInfo.Engine.Session.Current.Catalog.NamedConnections.Load(fileName);
				}

				// Put Catalog into a byte[] and keep it in HttpApplicationState
				byte[] catalogBits = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Catalog);
				Application.Add("Catalog", catalogBits);

				// Put a marker key/value.
				Application.Add("DataAccessWeb", "Here");
			}
			else
			{
				// Apply original Catalog state.
				Object obj = Application.Get("Catalog");
				if(obj != null)
				{
					Object tempObj = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(obj as byte[]);
				}

				// Apply original Map object state.
				obj = Application.Get(MapControl1.MapAlias);
				if(obj != null)
				{
					Object tempObj = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(obj as byte[]);
				}
			}

			// Set the initial zoom, center and size of the map
			// This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
			myMap.Zoom = new MapInfo.Geometry.Distance(25000, DistanceUnit.Mile);
			myMap.Center = new DPoint(27775.805792979896,-147481.33999999985);
			myMap.Size = new System.Drawing.Size((int)this.MapControl1.Width.Value, (int)this.MapControl1.Height.Value);
		}

		private void Page_UnLoad(object sender, System.EventArgs e)
		{
			// Save state.
			if(MapInfo.WebControls.StateManager.IsManualState())
			{
				MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState();
			}
		}

		private Map GetMapObj()
		{
			// Get the map
			MapInfo.Mapping.Map myMap = MapInfo.Engine.Session.Current.MapFactory[MapControl1.MapAlias];
			if(myMap == null)
			{
				myMap = MapInfo.Engine.Session.Current.MapFactory[0];
			}
			return myMap;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			this.Unload += new System.EventHandler(this.Page_UnLoad);
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			this.OpenTableButton.Click += new System.EventHandler(this.OpenTableButton_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void BindOpenedTablesAliasToRepeater()
		{
			ArrayList aliasList = new ArrayList();
			ITableEnumerator iEnum = MapInfo.Engine.Session.Current.Catalog.EnumerateTables();
			while(iEnum.MoveNext())
			{
				aliasList.Add(iEnum.Current.Alias);
			}
			Repeater1.DataSource = aliasList;
			Repeater1.DataBind();
		}

		private void ApplyButton_Click(object sender, System.EventArgs e)
		{
			if(SelectClauseTextBox.Text != null)
			{
				SetDataGrid(DataGrid1, SelectClauseTextBox.Text.Trim(), false);
				DataGrid1.DataBind();
			}
		}

		private void SetDataGrid(DataGrid dataGrid, string commandText, bool bShowSchema)
		{
			MapInfo.Data.MIConnection miConnection = new MIConnection();
			miConnection.Open();
			MapInfo.Data.MICommand miCommand = miConnection.CreateCommand();
			miCommand.CommandText = commandText;
			MapInfo.Data.MIDataReader miReader = null;
			try
			{
				miReader = miCommand.ExecuteReader();
				if (bShowSchema) 
				{
					dataGrid.DataSource = miReader.GetSchemaTable();
				}
				else
				{
					DataTable dt = new DataTable("Data");
					for (int index = 0; index < miReader.FieldCount; index++)
					{
						DataColumn dc = dt.Columns.Add(miReader.GetName(index));
					}
					while (miReader.Read())
					{
						DataRow dr = dt.NewRow();
						for (int index = 0; index < miReader.FieldCount; index++)
						{
							dr[index] = miReader.GetValue(index);
						}
						dt.Rows.Add(dr);
					}
					dataGrid.DataSource = dt;
				}
			}
			catch(Exception)
			{
				SelectClauseTextBox.Text +=  " ***The select clause is either is not specified or is incorrect. Please enter it again.***";
			}
			finally
			{
				if(miReader != null)
				{
					miReader.Close();
				}
			}
		}
		
		#region Table Open related methods.
		// Open a new table or use existing one.
		private void OpenTableButton_Click(object sender, System.EventArgs e)
		{
			if(CheckBoxList1.SelectedValue == null || CheckBoxList1.SelectedValue.Length <= 0) return;
			WarningLabel.Visible = false;
			if(OpenTableTextBox.Text != null && OpenTableTextBox.Text.Trim().Length != 0)
			{
				string tableName = OpenTableTextBox.Text.Trim();
				MapInfo.Data.Table table = null;
				try
				{
					table = this.OpenTable(CheckBoxList1.SelectedValue, tableName);
					if(table != null)
					{
						string lyrAlias = "alias_flyr_" + table.Alias;
						MapInfo.Mapping.Map myMap = GetMapObj();
						if(myMap == null) return;
						if(myMap.Layers[lyrAlias] != null)
						{
							myMap.Layers.Remove(lyrAlias);
						}
						FeatureLayer fLyr = new FeatureLayer(table, "LayerName_" + tableName, lyrAlias);
						myMap.Layers.Insert(0, fLyr);

						// Need to rebind again since a new table got opened.
						BindOpenedTablesAliasToRepeater();
					}
					else
					{
						WarningLabel.Visible = true;
					}
				}
				catch(Exception)
				{
					WarningLabel.Visible = true;
					if(table != null)
					{
						table.Close();
					}
				}
			}
		}

		private MapInfo.Data.Table OpenTable(string connectionName, string tableName)
		{
			MapInfo.Data.NamedConnectionInfo nci = MapInfo.Engine.Session.Current.Catalog.NamedConnections.Get(connectionName);
			if(nci == null) return null;
			if(nci.DBType.ToLower().Equals("file"))
			{
				return OpenNativeTable(nci, tableName);
			}
			else
			{
				return OpenDataBaseTable(nci, tableName);
			}
		}

		private MapInfo.Data.Table OpenNativeTable(MapInfo.Data.NamedConnectionInfo nci, string tableName)
		{
			string fileName = tableName;
			string alias = tableName + "_" + nci.Name;
			MapInfo.Data.Table table = MapInfo.Engine.Session.Current.Catalog.GetTable(alias);
			if(table == null)
			{
				if(tableName.ToLower().IndexOf(".tab") <=0 )
				{
					fileName = tableName + ".tab";
				}
				table = MapInfo.Engine.Session.Current.Catalog.OpenTable(nci.Name, alias, fileName);
			}
			return table;
		}

		private MapInfo.Data.Table OpenDataBaseTable(MapInfo.Data.NamedConnectionInfo nci, string dbTableName)
		{
			String tableAlias = dbTableName + "_" + nci.Name.Replace(" ", "_").Replace(".", "_");
			MapInfo.Data.Table table = MapInfo.Engine.Session.Current.Catalog.GetTable(tableAlias);
			// we need to create a new table if there is no table in the catalog.
			if(table == null)
			{
				TableInfoServer tis = new TableInfoServer( "tableInfoServer_" + dbTableName);
				tis.ConnectString = nci.ConnectionString;
				tis.Query = "select * from " + dbTableName;
				tis.Toolkit = this.GetServerToolkit(nci);
				tis.CacheSettings = new CacheParameters(CacheOption.On);
				tis.Alias = tableAlias;
				table = MapInfo.Engine.Session.Current.Catalog.OpenTable(nci.Name, tis);
			}
			return table;
		}

		private ServerToolkit GetServerToolkit(MapInfo.Data.NamedConnectionInfo nci)
		{
			switch (nci.ConnectionMethod)
			{
				case ConnectionMethod.Odbc :
					return ServerToolkit.Odbc;
				case ConnectionMethod.OracleOci :
					return ServerToolkit.Oci;
				default:
					throw new ArgumentException("ConnectionMethod should only be Odbc or OracleOci!", "nci");
			}
		}
		#endregion
	}
}
