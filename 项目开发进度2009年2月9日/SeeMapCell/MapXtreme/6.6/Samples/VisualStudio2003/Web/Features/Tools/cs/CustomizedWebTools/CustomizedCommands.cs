using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Drawing;
using MapInfo.WebControls;
using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Styles;

namespace ToolsSample
{
	/// <summary>
	/// Summary description for PinPointCommand.
	/// </summary>
	[Serializable]
	public class AddPinPointCommand : MapInfo.WebControls.MapBaseCommand 
	{
		/// <summary>
		/// Constructor for this command, sets the name of the command
		/// </summary>
		/// <remarks>None</remarks>
		public AddPinPointCommand()
		{
			Name = "AddPinPointCommand";
		}

		/// <summary>
		/// This method gets the map object out of the mapfactory with given mapalias and 
		/// Adds a point feature into a temp layer, exports it to memory stream and streams it back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() 
		{
			// Extract points from the string
			System.Drawing.Point [] points = this.ExtractPoints(this.DataString);
			
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);

			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);
			if(map == null) return;

			// There will be only one point, convert it to spatial
			MapInfo.Geometry.DPoint point;
			map.DisplayTransform.FromDisplay(points[0], out point);

			IMapLayer lyr = map.Layers[SampleConstants.TempLayerAlias];
			if(lyr == null)
			{
				TableInfoMemTable ti = new TableInfoMemTable(SampleConstants.TempTableAlias);
				// Make the table mappable
				ti.Columns.Add(ColumnFactory.CreateFeatureGeometryColumn(map.GetDisplayCoordSys()));
				ti.Columns.Add(ColumnFactory.CreateStyleColumn());
            
				Table table = MapInfo.Engine.Session.Current.Catalog.CreateTable(ti);
				map.Layers.Insert(0, new FeatureLayer(table, "templayer", SampleConstants.TempLayerAlias));
			}
			lyr = map.Layers[SampleConstants.TempLayerAlias];
			if(lyr == null) return;
			FeatureLayer fLyr = lyr as FeatureLayer;

			MapInfo.Geometry.Point geoPoint = new MapInfo.Geometry.Point(map.GetDisplayCoordSys(), point);
			// Create a Point style which is a red pin point.
			SimpleVectorPointStyle vs = new SimpleVectorPointStyle();
			vs.Code = 67;
			vs.Color = Color.Red;
			vs.PointSize = Convert.ToInt16(24);
			vs.Attributes = StyleAttributes.PointAttributes.BaseAll;
			vs.SetApplyAll();

			// Create a Feature which contains a Point geometry and insert it into temp table.
			Feature pntFeature = new Feature(geoPoint, vs);
			MapInfo.Data.Key key = fLyr.Table.InsertFeature(pntFeature);

			// Send contents back to client.
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
	}
	/// <summary>
	/// Summary description for PinPointCommand.
	/// </summary>
	[Serializable]
	public class ClearPinPointCommand : MapInfo.WebControls.MapBaseCommand
	{
		/// <summary>
		/// Constructor for this command, sets the name of the command
		/// </summary>
		/// <remarks>None</remarks>
		public ClearPinPointCommand()
		{
			Name = "ClearPinPointCommand";
		}

		/// <summary>
		/// This method gets the map object out of the mapfactory with given mapalias 
		/// and This method delete the pin point features added by AddPinPointCommand in a given point 
		/// and then streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() 
		{
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			MapInfo.Mapping.Map map = model.GetMapObj(MapAlias);
			if(map == null) return;
			PointDeletion(map, points[0]);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}
		/// <summary>
		/// Delete a feature in the temporary layer.
		/// </summary>
		/// <param name="mapAlias">MapAlias of the map</param>
		/// <param name="point">Point in pixels</param>
		private void PointDeletion(Map map, System.Drawing.Point point)
		{
			// Do the search and show selections
			SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(map, point, 10);
			(si.SearchResultProcessor as ClosestSearchResultProcessor).Options = ClosestSearchOptions.StopAtFirstMatch;
					
			Table table = MapInfo.Engine.Session.Current.Catalog[SampleConstants.TempTableAlias];
			if(table != null)
			{
				IResultSetFeatureCollection ifc = Session.Current.Catalog.Search(table, si);
				foreach(Feature f in ifc)
				{
					table.DeleteFeature(f);
				}
				ifc.Close();
			}
		}
	}

	/// <summary>
	/// Summary description for ModifiedRadiusSelectionCommand.
	/// </summary>
	[Serializable]
	public class ModifiedRadiusSelectionCommand : MapInfo.WebControls.MapBaseCommand
	{
		/// <summary>
		/// Constructor for this command, sets the name of the command
		/// </summary>
		/// <remarks>None</remarks>
		public ModifiedRadiusSelectionCommand()
		{
			Name = "ModifiedRadiusSelectionCommand";
		}

		/// <summary>
		/// This method gets the map object out of the mapfactory with given mapalias and 
		/// This method searches all features within a given point and a radius in all visible and selectable layers 
		/// except the features picked up by the given point 
		/// and then updates the default selection and streams the image back to client.
		/// </summary>
		/// <remarks>None</remarks>
		public override void Process() 
		{
			MapControlModel model = MapControlModel.GetModelFromSession();
			model.SetMapSize(MapAlias, MapWidth, MapHeight);
			System.Drawing.Point[]  points = ExtractPoints(DataString);
			Map myMap = model.GetMapObj(MapAlias);
			RadiusSelection(myMap, points);
			MemoryStream ms = model.GetMap(MapAlias, MapWidth, MapHeight, ExportFormat);
			StreamImageToClient(ms);
		}

		/// <summary>
		/// Select all feature with given radius but ones selected by the center point.
		/// </summary>
		/// <remarks>This method searches for all features whose centroids are within the given radius but ones selected by the center point and updates the 
		/// default selection. This method will clear DefaultSelection if radius is 0 or only one click happened in client side.</remarks>
		/// <param name="mapAlias">MapAlias of the map</param>
		/// <param name="myMap">Map object</param>
		private void RadiusSelection(Map myMap, System.Drawing.Point[] points)
		{
			Session.Current.Selections.DefaultSelection.Clear();

			// just return if it is one point only or first and second points are same.
			if(points.Length == 1 || points[0] == points[1])
			{
				return;
			}
			
			IMapLayerFilter _selFilter = MapLayerFilterFactory.FilterForTools(
				myMap, MapLayerFilterFactory.FilterByLayerType(LayerType.Normal), MapLayerFilterFactory.FilterVisibleLayers(true), 
				"MapInfo.Tools.MapToolsDefault.SelectLayers", null);

			// alias for temp selection object.
			string tempAlias = "tempSelection";
			ITableEnumerator iTableEnum = myMap.Layers.GetTableEnumerator(_selFilter);
			if ( iTableEnum != null ) 
			{ 
				try
				{
					// Get center and radius
					System.Drawing.Point center = points[0];
					int radius = points[1].X;

					// search within screen radius.
					SearchInfo si = MapInfo.Mapping.SearchInfoFactory.SearchWithinScreenRadius(myMap, center, radius, 20, ContainsType.Centroid);
					Session.Current.Catalog.Search(iTableEnum, si, Session.Current.Selections.DefaultSelection, ResultSetCombineMode.AddTo);

					// Create the temp selection object.
					Session.Current.Selections.CreateSelection(tempAlias);

					// Search nearest the center point.
					si = MapInfo.Mapping.SearchInfoFactory.SearchNearest(myMap, center, 6);
					Session.Current.Catalog.Search(iTableEnum, si, Session.Current.Selections[tempAlias], ResultSetCombineMode.AddTo);

					// Subtract radius selected features from point selected features.
					IEnumerator iEnum = Session.Current.Selections[tempAlias].GetEnumerator();
					while(iEnum.MoveNext())
					{
						IResultSetFeatureCollection pntCollection = iEnum.Current as IResultSetFeatureCollection;

						IResultSetFeatureCollection radiusCollection = null;
						for(int index=0; index < Session.Current.Selections.DefaultSelection.Count; index++)
						{
							// Need to find out the IResultSetFeatureCollection based on the same BaseTable.
							if(Session.Current.Selections.DefaultSelection[index].BaseTable.Alias == pntCollection.BaseTable.Alias)
							{
								radiusCollection = Session.Current.Selections.DefaultSelection[index];
								break;
							}
						}
						if(radiusCollection != null)
						{
							// Remove features in pntCollection from radiusCollection.
							radiusCollection.Remove(pntCollection);
						}
					}
				}
				catch(Exception )
				{
					Session.Current.Selections.DefaultSelection.Clear();
				}
				finally
				{
					Session.Current.Selections.Remove(Session.Current.Selections[tempAlias]);
				}
			}
		}
	}
}
