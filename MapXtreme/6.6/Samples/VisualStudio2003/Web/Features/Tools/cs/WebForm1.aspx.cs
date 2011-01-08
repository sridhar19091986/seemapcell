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
using MapInfo.WebControls;
using MapInfo.Mapping;

namespace ToolsSample
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected MapInfo.WebControls.RadiusSelectionTool RadiusSelectionTool2;
		protected MapInfo.WebControls.RectangleSelectionTool RectangleSelectionTool1;
		protected MapInfo.WebControls.RadiusSelectionTool RadiusSelectionTool1;
		protected MapInfo.WebControls.PolygonSelectionTool PolygonSelectionTool1;
		protected MapInfo.WebControls.PointSelectionTool PointSelectionTool1;
		protected System.Web.UI.WebControls.Label ChangedExistingToolsLabel;
		protected MapInfo.WebControls.PanTool PanTool1;
		protected System.Web.UI.WebControls.Label CustomizedToolLabel;
		protected CustomizedWebTools.PinPointWebTool PinPointWebTool2;
		protected CustomizedWebTools.PinPointWebTool PinPointWebTool1;
		protected MapInfo.WebControls.DistanceTool DistanceTool1;
		protected MapInfo.WebControls.CenterTool CenterTool1;
		protected MapInfo.WebControls.ZoomOutTool ZoomOutTool1;
		protected MapInfo.WebControls.ZoomInTool ZoomInTool1;
		protected MapInfo.WebControls.MapControl MapControl1;
		protected System.Web.UI.WebControls.Label NoteLabel;
		protected System.Web.UI.WebControls.Label BuiltinToolsLabel;
		protected System.Web.UI.WebControls.Label TitleLabel;

		private void Page_Load(object sender, System.EventArgs e)
		{
			MapInfo.Mapping.Map myMap = GetMapObj();
			if(Session.IsNewSession)
			{
				AppStateManager stateManager = new AppStateManager();
				stateManager.ParamsDictionary[AppStateManager.ActiveMapAliasKey] = this.MapControl1.MapAlias;
				MapInfo.WebControls.StateManager.PutStateManagerInSession(stateManager);

				// Add customized web tools
				// Below line will put controlModel into HttpSessionState.
				MapInfo.WebControls.MapControlModel controlModel = MapControlModel.SetDefaultModelInSession();
				controlModel.Commands.Add(new AddPinPointCommand());
				controlModel.Commands.Add(new ClearPinPointCommand());
				controlModel.Commands.Add(new ModifiedRadiusSelectionCommand());

				/****** Set the initial state of the map  *************/
				// Clear up the temp layer left by other customer requests.
				if(myMap != null)
				{
					if(myMap.Layers[SampleConstants.TempLayerAlias] != null)
					{
						myMap.Layers.Remove(SampleConstants.TempLayerAlias);
					}
				}
				// Need to clean up "dirty" temp table left by other customer requests.
				MapInfo.Engine.Session.Current.Catalog.CloseTable(SampleConstants.TempTableAlias);
				// Need to clear the DefautlSelection.
				MapInfo.Engine.Session.Current.Selections.DefaultSelection.Clear();

				// Creat a temp table and AddPintPointCommand will add features into it.
				MapInfo.Data.TableInfoMemTable ti = new MapInfo.Data.TableInfoMemTable(SampleConstants.TempTableAlias);
				// Make the table mappable
				ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateFeatureGeometryColumn(myMap.GetDisplayCoordSys()));
				ti.Columns.Add(MapInfo.Data.ColumnFactory.CreateStyleColumn());
            
				MapInfo.Data.Table table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);
				// Create a new FeatureLayer based on the temp table, so we can see the temp table on the map.
				myMap.Layers.Insert(0, new FeatureLayer(table, "templayer", SampleConstants.TempLayerAlias));

				// This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
				myMap.Zoom = new MapInfo.Geometry.Distance(25000, MapInfo.Geometry.DistanceUnit.Mile);
				myMap.Center = new MapInfo.Geometry.DPoint(27775.805792979896,-147481.33999999985);
			}

			MapInfo.WebControls.StateManager.GetStateManagerFromSession().RestoreState();
		}

		private void Page_Unload(object sender, System.EventArgs e)
		{
			MapInfo.WebControls.StateManager.GetStateManagerFromSession().SaveState();
		}

		private MapInfo.Mapping.Map GetMapObj()
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
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Unload += new System.EventHandler(this.Page_Unload);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
