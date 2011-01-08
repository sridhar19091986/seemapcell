using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using MapInfo.Mapping;
using MapInfo.Data;
using MapInfo.WebControls;


namespace CustomWebTools
{
	/// <summary>
	/// Info command for InfoWebTool.
	/// </summary>
	[Serializable]
	public class Info : MapInfo.WebControls.MapBaseCommand
	{		
		/// <summary>
		/// Key to be used to get the pixel tolerance parameter value from the URL.
		/// </summary>
		protected const string PixelToleranceKey = "PixelTolerance";
		protected const string InfoCommand = "Info";
		

		/// <summary>
		/// Constructor for Info class
		/// </summary>
		public Info()
		{
			Name = InfoCommand;
		}

		/// <summary>
		/// Override the Execute method in MapBasicCommand class to not save state, because
		/// for info tool, which does not change map state, so there is no need to save map state.
		/// </summary>
		public override void Execute()
		{
			
			StateManager sm = StateManager.GetStateManagerFromSession();
			if (sm == null) 
			{
				if(StateManager.IsManualState())
				{
					throw new NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.");
				}
			} 
			ParseContext();
			if(sm != null)
			{
				PrepareStateManagerParamsDictionary(sm);
				sm.RestoreState();
			}

			Process();
		}

		/// <summary>
		/// method to do the real server side process for info tool.
		/// </summary>
		public override void Process()
		{
			//get pixel tolerance from url of client side.
			int pixelTolerance = System.Convert.ToInt32(HttpContext.Current.Request[PixelToleranceKey]);
			
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			
			//extract points from url of client side.
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			
			//do searching and get results back
			MultiResultSetFeatureCollection mrfc = RetrieveInfo(points, pixelTolerance);
						
			IEnumerator resultEnum = mrfc.GetEnumerator();
			
			//retrieve the selected feature from collection
			while(resultEnum.MoveNext())
			{
				IResultSetFeatureCollection irfc = (IResultSetFeatureCollection)resultEnum.Current;
				IFeatureEnumerator ftrEnum = irfc.GetFeatureEnumerator();
				
				while(ftrEnum.MoveNext())
				{
					Feature ftr = (Feature)ftrEnum.Current;
					//create a html table to display feature info and stream back to client side.
					CreateInfoTable(ftr);		
					irfc.Close();
					mrfc.Clear();
					break;
				}	
				break;
			}	
		}

		/// <summary>
		/// Creates html table to hold passed in feature info, and stream back to client.
		/// </summary>
		/// <param name="ftr">feature object</param>
		private void CreateInfoTable(Feature ftr)
		{
			//create a table control and populat it with the column name/value(s) from the feature returned and
			// and the name of the layer where the feature belong
			System.Web.UI.WebControls.Table infoTable = new System.Web.UI.WebControls.Table();
			//set table attribute/styles
			infoTable.CellPadding = 4;			
			infoTable.Font.Name = "Arial";
			infoTable.Font.Size = new FontUnit(8);
			infoTable.BorderWidth = 1;
			//infoTable.BorderStyle = BorderStyle.Outset; 
			
			System.Drawing.Color backColor = Color.Bisque;

			//add the first row, the layer name/value where the selected feature belongs 
			TableRow r = new TableRow();
			r.BackColor = backColor;

			TableCell c = new TableCell();
			c.Font.Bold = true;			
			c.ForeColor = Color.Indigo;

			c.Text = "Layer Name";			
			r.Cells.Add(c);

			c = new TableCell();

			//the feature returned is from a resultset table whose name is got from appending _2
			//to the real table name, so below is to get the real table name.
			string alias = ftr.Table.Alias;
			c.Text = alias.Substring(0, alias.Length-2);
			c.Font.Bold = true;
			r.Cells.Add(c);
			
			infoTable.Rows.Add(r);

			foreach(Column col in ftr.Columns)
			{
				String upAlias = col.Alias.ToUpper();
				//don't display obj, MI_Key or MI_Style columns
				if(upAlias != "OBJ" && upAlias != "MI_STYLE" && upAlias != "MI_KEY")
				{
					r = new TableRow();
					r.BackColor = backColor;

					r.Cells.Clear();
					c = new TableCell();
					c.Text = col.Alias;
					c.Font.Bold = true;
					c.ForeColor = Color.RoyalBlue;

					r.Cells.Add(c);
					c = new TableCell();
					c.Text = ftr[col.Alias].ToString();
					r.Cells.Add(c);
					infoTable.Rows.Add(r);
				}
			}

			//stream the html table back to client
			StringWriter sw = new StringWriter();
			HtmlTextWriter hw = new HtmlTextWriter(sw);
			infoTable.RenderControl(hw);
			String strHTML = sw.ToString();
            HttpContext.Current.Response.Output.Write(strHTML);
		}

		/// <summary>
		/// Get a MultiFeatureCollection containing features in all layers falling into the tolerance of the point.
		/// </summary>
		/// <param name="points">points array</param>
		/// <param name="pixelTolerance">pixel tolerance used when searching</param>
		/// <returns>Returns a MultiResultSetFeatureCollection object</returns>
		protected MultiResultSetFeatureCollection RetrieveInfo(Point[] points, int pixelTolerance) 
		{
			if(points.Length != 1)
				return null;

			MapControlModel model = MapControlModel.GetModelFromSession();
			//get map object from map model
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);

			if(map == null) return null;

			//creat a layer filter to include normal visible layers for searching
			IMapLayerFilter layerFilter = MapLayerFilterFactory.FilterForTools(
				map, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			ITableEnumerator tableEnum = map.Layers.GetTableEnumerator(layerFilter);
			
			//return if there is no valid layer to search
			if(tableEnum == null) return null;

			System.Drawing.Point center = points[0];
			
			//create a SearchInfo with a point and tolerance
			SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map, center, pixelTolerance);
			(si.SearchResultProcessor as ClosestSearchResultProcessor).Options = ClosestSearchOptions.StopAtFirstMatch;
			//retrieve all columns
			si.QueryDefinition.Columns = null;
			
			MapInfo.Geometry.Distance d = MapInfo.Mapping.SearchInfoFactory.ScreenToMapDistance(map, pixelTolerance);
			(si.SearchResultProcessor as ClosestSearchResultProcessor).DistanceUnit=d.Unit;
			(si.SearchResultProcessor as ClosestSearchResultProcessor).MaxDistance = d.Value;

			
			//do search
			MultiResultSetFeatureCollection mrfc = MapInfo.Engine.Session.Current.Catalog.Search(tableEnum, si);
			return mrfc;

		}
	}

	/// <summary>
	/// ZoomValue command to write current zoom value to client for display.
	/// </summary>
	[Serializable]
	public class ZoomValue : MapInfo.WebControls.MapBaseCommand
	{
		/// <summary>
		/// Constructor for ZoomValue class
		/// </summary>
		public ZoomValue()
		{
			Name = "ZoomValue";
		}

		/// <summary>
		/// Override the Execute method in MapBasicCommand class to NOT save state, because
		/// for this command, which does not change map state, so there is no need to save map state.
		/// </summary>
		public override void Execute()
		{
			
			StateManager sm = StateManager.GetStateManagerFromSession();
			if (sm == null) 
			{
				if(StateManager.IsManualState())
				{
					throw new NullReferenceException("Cannot find instance of StateManager in the ASP.NET session.");
				}
			} 
			ParseContext();
			if(sm != null)
			{
				PrepareStateManagerParamsDictionary(sm);
				sm.RestoreState();
			}

			Process();
		}

		public override void Process()
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
			//get map object from map model
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);
			string zoomStr = map.Zoom.ToString();
			HttpContext.Current.Response.Output.Write(zoomStr);

		}



	}
}
