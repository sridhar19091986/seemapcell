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
using MapInfo.WebControls;

namespace FindSampleWeb
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.LinkButton LinkButton16;

		private static string _workingLayerName = "WorkingLayer";
		private static string _findLayerName = "worldcap";
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
		private static string _findColumnName = "Country";

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

					// Add workingLayerName into State manager's ParamsDictionary.
					stateManager.ParamsDictionary["WorkingLayerName"] = _workingLayerName;
					
					// Put state manager into HttpSession, so we could get it later on.
					AppStateManager.PutStateManagerInSession(stateManager);
				}

				InitWorkingLayer();

				MapInfo.Mapping.Map myMap = MapInfo.Engine.Session.Current.MapFactory[this.MapControl1.MapAlias];
				// Set the initial zoom, center and size of the map
				// This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
				myMap.Zoom = new MapInfo.Geometry.Distance(25000, DistanceUnit.Mile);
				myMap.Center = new DPoint(27775.805792979896,-147481.33999999985);
				myMap.Size = new System.Drawing.Size((int)this.MapControl1.Width.Value, (int)this.MapControl1.Height.Value);
			}

			// Restore state.
			if(MapInfo.WebControls.StateManager.IsManualState())
			{
				MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState();
			}
		}

		private bool InitWorkingLayer()
		{
			MapInfo.Mapping.Map map = null;

			// Get the map
			if (MapInfo.Engine.Session.Current.MapFactory.Count == 0 ||
				(map = MapInfo.Engine.Session.Current.MapFactory[MapControl1.MapAlias]) == null)
			{
				return false;
			}

			// Make sure the Find layer's MemTable exists
			MapInfo.Data.Table table = MapInfo.Engine.Session.Current.Catalog.GetTable(_workingLayerName);
			if (table == null)
			{
				TableInfoMemTable ti = new TableInfoMemTable(_workingLayerName);
				ti.Temporary = true;
				// Add the Geometry column
				Column col = new MapInfo.Data.GeometryColumn(map.GetDisplayCoordSys());
				col.Alias = "obj";
				col.DataType = MIDbType.FeatureGeometry;
				ti.Columns.Add(col);
				// Add the Style column
				col = new MapInfo.Data.Column();
				col.Alias = "MI_Style";
				col.DataType = MIDbType.Style;
				ti.Columns.Add(col);
				table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);
			}
			if (table == null) return false;

			// Make sure the Find layer exists
			MapInfo.Mapping.FeatureLayer layer = (MapInfo.Mapping.FeatureLayer)map.Layers[_workingLayerName];
			if (layer == null)
			{
				layer = new MapInfo.Mapping.FeatureLayer(table, _workingLayerName, _workingLayerName);
				map.Layers.Insert(0, layer);
			}
			if (layer == null) return false;

			// Delete the find object.  There should only be one object in this table.
			(layer.Table as ITableFeatureCollection).Clear();

			return true;
		}

		private void Page_UnLoad(object sender, System.EventArgs e)
		{
			// Save state.
			if(MapInfo.WebControls.StateManager.IsManualState())
			{
				MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState();
			}
		}

		private void FillDropDown(string tableName, string colName) 
		{
			MapInfo.Mapping.Map map = null;

			// Get the map
			if (MapInfo.Engine.Session.Current.MapFactory.Count == 0 ||
				(map = MapInfo.Engine.Session.Current.MapFactory[MapControl1.MapAlias]) == null)
			{
				return;
			}

			DropDownList1.Items.Clear();
			MapInfo.Mapping.FeatureLayer fl = (MapInfo.Mapping.FeatureLayer)map.Layers[tableName];
			MapInfo.Data.Table  t = fl.Table;
			MIDataReader tr;
			MIConnection con = new MIConnection();
			MICommand tc = con.CreateCommand();
			tc.CommandText = "select " + colName + " from " + t.Alias ;
			con.Open();
			tr = tc.ExecuteReader() ;
			while (tr.Read()) 
			{
				DropDownList1.Items.Add(tr.GetString(0));
			}
			tc.Cancel();
			tc.Dispose();
			tr.Close() ;
			con.Close();
			//t.Close();
		}
		
		protected override void OnPreRender(EventArgs e)
		{
			if (!IsPostBack)
			{
				FillDropDown(_findLayerName, _findColumnName);
			}
		}

		private void FindCity()
		{
			Find find = null;
			try
			{
				MapInfo.Mapping.Map map = null;

				// Get the map
				if (MapInfo.Engine.Session.Current.MapFactory.Count == 0 ||
					(map = MapInfo.Engine.Session.Current.MapFactory[MapControl1.MapAlias]) == null)
				{
					return;
				}

				// Do the find
				MapInfo.Mapping.FeatureLayer findLayer = (MapInfo.Mapping.FeatureLayer) map.Layers[_findLayerName];
				find = new Find(findLayer.Table, findLayer.Table.TableInfo.Columns[_findColumnName]);
				FindResult result = find.Search(DropDownList1.SelectedItem.Text);
				if (result.ExactMatch)
				{
					// Create a Feature (point) for the location we found
					CoordSys csys = findLayer.CoordSys;
					FeatureGeometry g = new MapInfo.Geometry.Point(csys, result.FoundPoint.X, result.FoundPoint.Y);
					Feature f = new Feature(g, new MapInfo.Styles.SimpleVectorPointStyle(52, System.Drawing.Color.DarkGreen, 32));

					// Delete the existing find object and add the new one
					MapInfo.Mapping.FeatureLayer workingLayer = (MapInfo.Mapping.FeatureLayer)map.Layers[_workingLayerName];
					if (workingLayer != null)
					{
						(workingLayer.Table as ITableFeatureCollection).Clear();
						workingLayer.Table.InsertFeature(f);
					}

					// Set the map's center and zooom
					map.Center = new DPoint(result.FoundPoint.X, result.FoundPoint.Y);
					MapInfo.Geometry.Distance d = new MapInfo.Geometry.Distance(1000, map.Zoom.Unit);
					map.Zoom = d;
				}
				else 
				{
					this.Label3.Text = ("Cannot find the country");
				}
				find.Dispose();
			}
			catch (Exception)
			{
				if (find != null) find.Dispose();
			}
		}

		private void LinkButton16_Click(object sender, System.EventArgs e)
		{
			FindCity();
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
			this.LinkButton16.Click += new System.EventHandler(this.LinkButton16_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
