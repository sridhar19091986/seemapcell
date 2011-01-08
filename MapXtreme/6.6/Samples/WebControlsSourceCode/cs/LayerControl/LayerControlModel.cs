using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using MapInfo.Data;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.Styles;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This class is the model for the LayerControl. The model's responsibility is to talk to the MapXtreme object model and return XML which can be used
	/// to generate HTML.
	/// </summary>
	/// <remarks>
	/// This model has methods to generate XML by going through the object model. This XML, combined with XSLT, generates the
	/// HTML.
	/// </remarks>
	[Serializable]
	public class LayerControlModel
	{
		private string _mapAlias = null;

		/// <summary>
		/// Gets the model for the LayerControl from the ASP.NET session.
		/// </summary>
		/// <remarks>This method will return null if the model is not found.</remarks>
		/// <returns>LayerControl model</returns>
		public static LayerControlModel GetModelFromSession() {
			HttpContext context = HttpContext.Current;
			string key = string.Format("{0}_LayerModel", context.Session.SessionID);
			LayerControlModel model = null;
			if (context.Session[key] != null) {
				model = context.Session[key] as LayerControlModel;
			}
			return model;
		}

		/// <summary>
		/// Creates the default model for the LayerControl provided by MapXtreme and sets the object in the ASP.NET session.
		/// </summary>
		/// <remarks>This method tries to extract the model for the LayerControl from the ASP.NET session. If the medel is not found it will create the default one and places it there.
		/// This is a safe method, and always returns the model. TThis method can be used when the model may not exist in the session.</remarks>
		/// <returns>LayerControlModel from session.</returns>
		public static LayerControlModel SetDefaultModelInSession() {
			HttpContext context = HttpContext.Current;
			LayerControlModel model = GetModelFromSession();
			if (model == null) {
				model = new LayerControlModel();
				SetModelInSession(model);
			}
			return model;
		}

		/// <summary>
		/// Sets the given model for the LayerControl in the ASP.NET session.
		/// </summary>
		/// <remarks>If a custom model is used, then this method can be used to set the model in the session that will be used instead.</remarks>
		/// <param name="model">The model to be set in the ASP.NET session.</param>
		public static void SetModelInSession(LayerControlModel model) {
			HttpContext context = HttpContext.Current;
			string key = string.Format("{0}_LayerModel", context.Session.SessionID);
			context.Session[key] = model;
		}

		private Map GetMapObj(string mapAlias) {
			Map map = null;
			if (mapAlias == null || mapAlias.Length <= 0) {
				map = MapInfo.Engine.Session.Current.MapFactory[0];
			} else {
				map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];
				if (map == null) map = MapInfo.Engine.Session.Current.MapFactory[0];
			}
			return map;
		}

		private XmlDocument InitDocument(XmlDocument doc) 
		{
			XmlProcessingInstruction pi = doc.CreateProcessingInstruction("xml",
				" version='1.0' encoding='UTF-8'");
			doc.AppendChild(pi);

			String PItext = "type='text/xsl' href='treeview.xslt'";
			pi = doc.CreateProcessingInstruction("xml-stylesheet", PItext);
			doc.AppendChild(pi);

			XmlElement element = doc.CreateElement("treeview");
			doc.AppendChild(element);

			return doc;
		}

		private XmlElement CreateLabelLayerElement(XmlDocument _doc, IMapLayer layer, string uniqueID) {
			Map map = GetMapObj(_mapAlias);

			XmlElement element = _doc.CreateElement("branch");
			element.SetAttribute("title", "", layer.Name);
			element.SetAttribute("uniqueid", "", uniqueID);
			element.SetAttribute("alias", "", layer.Alias);
			element.SetAttribute("type", "", layer.Type.ToString());
			element.SetAttribute("expanded", null, "true");
			element.SetAttribute("img", null,  "lclabel.bmp");
			element.SetAttribute("branchtype", "", "folder");
			if (layer.IsVisible) {
				element.SetAttribute("visible", "true");
			} else {
				element.SetAttribute("visible", "false");
				if (layer.VisibleRangeEnabled && !layer.VisibleRange.Within(map.Zoom)) {
					element.SetAttribute("rangevisible", "true");
				} else {
					element.SetAttribute("rangevisible", "false");
				}
			}

			foreach (LabelSource source in ((LabelLayer)layer).Sources) {
				XmlElement leafElement = _doc.CreateElement("branch");
				bool bHasModifiers = false;
				LabelModifiers mods = source.Modifiers;
				if ( mods.Count > 0 ) {
					bHasModifiers = true;	
				}
				leafElement.SetAttribute("uniqueid", "", uniqueID);
				leafElement.SetAttribute("title", null, source.Name);
				leafElement.SetAttribute("alias", "", source.Alias);
				leafElement.SetAttribute("type", "", layer.Type.ToString());
				if (source.Visible) {
					leafElement.SetAttribute("visible", "true");
				} else {
					leafElement.SetAttribute("visible", "false");
					if (source.VisibleRangeEnabled && !source.VisibleRange.Within(map.Zoom)) {
						leafElement.SetAttribute("rangevisible", "true");
					} else {
						leafElement.SetAttribute("rangevisible", "false");
					}
				}
				if ( bHasModifiers ) {
					leafElement.SetAttribute("branchtype", "", "folder");
					leafElement.SetAttribute("expanded", null, "true");
					leafElement.SetAttribute("code", "", "2");
				} else {
					leafElement.SetAttribute("branchtype", "", "leaf");
					leafElement.SetAttribute("code", "", "nonselectable");
				}
				leafElement.SetAttribute("img", null,  "lclabelsource.bmp");
				if ( bHasModifiers ) {
					CreateLabelModifierElement(_doc, source, leafElement, uniqueID);
				}
				element.AppendChild(leafElement);
			}
			return element;
		}

		private XmlElement CreateFeatureLayerElement(XmlDocument _doc, IMapLayer layer, string uniqueID) {
			Map map = GetMapObj(_mapAlias);
			FeatureLayer flayer = (FeatureLayer)layer;
			bool bRasterLayer = false;
			bool bHasModifiers = false;
			FeatureStyleModifiers mods = flayer.Modifiers;
			if ( mods.Count > 0 ) {
				bHasModifiers = true;	
			}
			if ( flayer.Type == LayerType.Raster || 
				flayer.Type == LayerType.Grid ||
				flayer.Type == LayerType.Wms ) {
				bRasterLayer = true;
			}
			XmlElement element = _doc.CreateElement("branch");
			if ( bHasModifiers ) {
				element.SetAttribute("branchtype", "", "folder");
				element.SetAttribute("expanded", null, "true");
			} else {
				element.SetAttribute("branchtype", "", "leaf");
			}
			element.SetAttribute("title", null, layer.Name);
			element.SetAttribute("alias", "", layer.Alias);
			element.SetAttribute("type", "", layer.Type.ToString());
			element.SetAttribute("uniqueid", "", uniqueID);
			if ( bRasterLayer ) {
				element.SetAttribute("code", "", "nonselectable");
			} else if ( bHasModifiers ) {
				element.SetAttribute("code", "", "4");
			} else {
				element.SetAttribute("code", "", "featurelayer");
			}

			if (layer.IsVisible) {
				element.SetAttribute("visible", "true");
			} else {
				element.SetAttribute("visible", "false");
				if (flayer.VisibleRangeEnabled && !flayer.VisibleRange.Within(map.Zoom)) {
					element.SetAttribute("rangevisible", "true");
				} else {
					element.SetAttribute("rangevisible", "false");
				}
			}
		
			// If layer is remote, image can be set from database style
			// null value returned if layer is not remote
			string image = DetermineRemoteGeomType(flayer);
			if ( image == null ) {
				image = "lclayerpoint.bmp";
				if ( bRasterLayer ) {
					image = "lclayerraster.bmp";
				} else {
					TableInfo tableInfo = flayer.Table.TableInfo;
					GeometryColumn geoCol = null; 
					Columns columns = tableInfo.Columns; 
					foreach (Column col in columns) {
						geoCol = col as GeometryColumn; 
						if (geoCol != null) {
							break; 
							// TODO: Check for the case where there are MULTIPLE columns.
						}
					}
					if (geoCol != null) {
						if (geoCol.PredominantGeometryType == GeometryType.MultiCurve) {
							image = "lclayerline.bmp"; 
						}
						else if ((geoCol.PredominantGeometryType == GeometryType.MultiPolygon) 
							|| (geoCol.PredominantGeometryType == GeometryType.Rectangle)) {
							image = "lclayerregion.bmp";
						}
						else if (geoCol.PredominantGeometryType == GeometryType.Point) {
							image = "lclayerpoint.bmp";
						}
					}
				}
			}
			element.SetAttribute("img", null, image);

			if ( bHasModifiers ) {
				CreateFeatureModifierElement(_doc, flayer, element, uniqueID);
			}
			return element;
		}

		private string DetermineRemoteGeomType(FeatureLayer layer) {
			MapInfo.Data.Table t = layer.Table;
			MapInfo.Styles.Style style = null; 
			
			MIConnection con = null; 
			MICommand cmd = null; 
			MIDataReader dr = null; 
			try {
				con = new MIConnection(); 
				con.Open(); 
				cmd = con.CreateCommand(); 
				cmd.CommandText = "select mi_style from \"" + t.Alias + "\"";
				cmd.CommandType = System.Data.CommandType.Text;
				dr = cmd.ExecuteReader();
				while (dr.Read()) {
					if (!dr.IsDBNull(0)) {
						style = dr.GetStyle(0);
						break;
					}
				}
			}
			catch (MIException) {
				// e.g. if there is no mi_style column
			}
			finally {
				if (cmd != null) {
					cmd.Dispose();
					cmd = null;
				}
				if (dr != null) {
					dr.Close();
				}
				if (con != null) {
					con.Close();
					con = null;
				}
			}

			if (style != null) {
				if (style is SimpleLineStyle) { 
					return "lclayerline.bmp"; 
				}
				else if (style is SimpleInterior || style is AreaStyle) {
					return "lclayerregion.bmp"; 
				}
				else if (style is BasePointStyle) {
					return "lclayerpoint.bmp";
				} else {
					return "lclayer.bmp";
				}
			} else {
				return null; 
			}
		}

		private void CreateLabelModifierElement(XmlDocument _doc, LabelSource source, XmlElement lyrElement, string uniqueID) {
			LabelModifiers mods = source.Modifiers;
			foreach (LabelModifier lm in mods) {
				XmlElement element = _doc.CreateElement("branch");
				element.SetAttribute("branchtype", "", "leaf");
				element.SetAttribute("title", null, lm.Name);
	            element.SetAttribute("alias", "", lm.Alias);
				element.SetAttribute("code", "", "nonselectable");
	            element.SetAttribute("type", "", "LabelMod");
				element.SetAttribute("img", null,  "lcmodifier.bmp");
	            element.SetAttribute("uniqueid", "", uniqueID);
				if (lm.Enabled) {
					element.SetAttribute("visible", "true");
				} else {
					element.SetAttribute("visible", "false");
				}
				lyrElement.AppendChild(element);
			}
		}

		private void CreateFeatureModifierElement(XmlDocument _doc, FeatureLayer layer, XmlElement lyrElement, string uniqueID) {
			FeatureStyleModifiers mods = layer.Modifiers;
			foreach (FeatureStyleModifier fsm in mods) {
				XmlElement element = _doc.CreateElement("branch");
				element.SetAttribute("branchtype", "", "leaf");
				element.SetAttribute("title", null, fsm.Name);
				if (fsm.Enabled) {
					element.SetAttribute("visible", "true");
				} else {
					element.SetAttribute("visible", "false");
				}
				element.SetAttribute("uniqueid", "", uniqueID);
				element.SetAttribute("code", "", "nonselectable");
				element.SetAttribute("img", null,  "lcmodifier.bmp");
				element.SetAttribute("alias", "", fsm.Alias);
				element.SetAttribute("type", "", "Mod");
				lyrElement.AppendChild(element);
			}
		}

		private XmlElement CreateObjectThemeElement(XmlDocument _doc, IMapLayer layer, string uniqueID) {
			Map map = GetMapObj(_mapAlias);
			XmlElement element = _doc.CreateElement("branch");
			element.SetAttribute("branchtype", "", "leaf");
			element.SetAttribute("alias", "", layer.Alias);
			if (layer.Enabled) {
				element.SetAttribute("visible", "true");
			} else {
				element.SetAttribute("visible", "false");
				if (layer.VisibleRangeEnabled && !layer.VisibleRange.Within(map.Zoom)) {
					element.SetAttribute("rangevisible", "true");
				} else {
					element.SetAttribute("rangevisible", "false");
				}
			}
			element.SetAttribute("type", "", layer.Type.ToString());
			element.SetAttribute("uniqueid", "", uniqueID);
			element.SetAttribute("title", null, layer.Name);
			element.SetAttribute("code", "", "nonselectable");
			element.SetAttribute("img", null,   "lcmodifier.bmp");
			return element;
		}

		private XmlElement CreateGroupLayerElement(XmlDocument _doc, IMapLayer layer, string uniqueID){
			Map map = GetMapObj(_mapAlias);

			XmlElement element = _doc.CreateElement("branch");
			element.SetAttribute("branchtype", "", "folder");
			element.SetAttribute("title", "", ((GroupLayer)layer).Name);
			element.SetAttribute("code", "", "3");
			element.SetAttribute("uniqueid", "", uniqueID);
			element.SetAttribute("alias", "", layer.Alias);
			element.SetAttribute("expanded", null, "true");
			element.SetAttribute("img", null,  "lcgroup.bmp");
			element.SetAttribute("type", "", layer.Type.ToString());
			if (layer.Enabled) {
				element.SetAttribute("visible", "true");
			} else {
				element.SetAttribute("visible", "false");
				if (layer.VisibleRangeEnabled && !layer.VisibleRange.Within(map.Zoom)) {
					element.SetAttribute("rangevisible", "true");
				} else {
					element.SetAttribute("rangevisible", "false");
				}
			}
			
			foreach (IMapLayer inlayer in (GroupLayer)layer) {
				if ( inlayer is LabelLayer ) {
					element.AppendChild(CreateLabelLayerElement(_doc, inlayer, uniqueID));	
				}
				else if ( inlayer is FeatureLayer ) {
					element.AppendChild(CreateFeatureLayerElement(_doc, inlayer, uniqueID));
				}
				else if ( inlayer is GroupLayer ) {
					element.AppendChild(CreateGroupLayerElement(_doc, inlayer, uniqueID));
				} 
				else if (inlayer is ObjectThemeLayer) {
					element.AppendChild(CreateObjectThemeElement(_doc, inlayer, uniqueID));
				}
			}
			return element;
		}

		/// <summary>
		/// Gets the XML containing the layer information to be used to transform to HTML.
		/// </summary>
		/// <remarks>This method goes through the map's layers and adds them to the XML document in the format which XSLT is expecting.</remarks>
		/// <param name="mapAlias">MapAlias of the map.</param>
		/// <param name="uniqueID">Unique ID of the LayerControl.</param>
		/// <param name="imgPath">Path to images and XSLT files.</param>
		/// <returns>Returns XML containing layer information.</returns>
		public XmlDocument GetLayerXML(string mapAlias, string uniqueID, string imgPath) 
		{
			_mapAlias = mapAlias;
			StateManager sm = StateManager.GetStateManagerFromSession();
			if(sm == null) {
				if (StateManager.IsManualState()) {
					throw new NullReferenceException(L10NUtils.Resources.GetString(StateManager.StateManagerResErr1));
				}
			}

			if(sm != null) {
				sm.ParamsDictionary[StateManager.ActiveMapAliasKey] = _mapAlias;
				sm.RestoreState();
			}

			Map map = GetMapObj(_mapAlias);

			 XmlDocument _doc = null;
			_doc = InitDocument(new XmlDocument());

			XmlElement root = _doc.DocumentElement;

			XmlElement custParam = _doc.CreateElement("custom-parameters");
			XmlElement param1 = _doc.CreateElement("param");
			param1.SetAttribute("name", "", "param-shift-width");
			param1.SetAttribute("value", "", "15");
			custParam.AppendChild(param1);

			XmlElement param2 = _doc.CreateElement("param");
			param2.SetAttribute("name", "", "img-directory");
			param2.SetAttribute("value", "", imgPath+"/");
			custParam.AppendChild(param2);

			XmlElement param3 = _doc.CreateElement("param");
			param3.SetAttribute("name", "", "mapzoom");
			param3.SetAttribute("value", "", map.Zoom.Value.ToString());
			custParam.AppendChild(param3);

			root.AppendChild(custParam);

			XmlElement topMost = _doc.CreateElement("branch");
			topMost.SetAttribute("title", "", map.Name);
			topMost.SetAttribute("img", null, "lcgroup.bmp");
			topMost.SetAttribute("branchtype", "", "folder");
			topMost.SetAttribute("expanded", null, "true");
	       topMost.SetAttribute("nocheckbox", null, "true");
			root.AppendChild(topMost);
			foreach (IMapLayer layer in map.Layers) {
				if ( layer is LabelLayer ) {
					topMost.AppendChild(CreateLabelLayerElement(_doc, layer, uniqueID));
				} else if ( layer is ObjectThemeLayer ) {
					topMost.AppendChild(CreateObjectThemeElement(_doc, layer, uniqueID));
				} else if ( layer is FeatureLayer ) {
					topMost.AppendChild(CreateFeatureLayerElement(_doc, layer, uniqueID));
				} else if ( layer is GroupLayer ) {
					topMost.AppendChild(CreateGroupLayerElement(_doc, layer, uniqueID));
				}
			}
			if (sm != null) {
				sm.SaveState();
			}
			return _doc;
		}
	}
}
