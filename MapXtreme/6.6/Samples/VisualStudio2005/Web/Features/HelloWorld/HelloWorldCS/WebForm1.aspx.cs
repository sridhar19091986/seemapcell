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

namespace HelloWorldWeb
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class WebForm1 : System.Web.UI.Page
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			MapInfo.Mapping.Map myMap = GetMapObj();
			if(myMap == null) return;

			// The first time in
			if (Session.IsNewSession)
			{
				//******************************************************************************//
				//*   You need to follow below lines in your own application.                  *//
				//*   You don't need this state manager if the "MapInfo.Engine.Session.State" *//
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
				
				// Initial state.
				InitState();
			}
			// Restore state.
			if(AppStateManager.IsManualState())
			{
				AppStateManager.GetStateManagerFromSession().RestoreState();
			}
			// Add a north arrow IAdornment into the map's adornments collection.
			AddNorthArrowAdornment(myMap);
		}

		private void AddNorthArrowAdornment(Map myMap)
		{
			if(myMap == null) return;
			// don't create a new NorthArrowAdornment object if there is one in the map, because it is same for all requests,
			// and we don't want to create a new one when we could use the old one.
			if(myMap.Adornments["north_arrow"] == null)
			{
				string path = HttpContext.Current.Server.MapPath(string.Format("MapXtremeWebResources", MapInfo.Engine.ProductInfo.MajorVersion, MapInfo.Engine.ProductInfo.MinorVersion));
				string northFile = System.IO.Path.Combine(path, "northarrow.bmp");
				myMap.Adornments.Append(new NorthArrowAdornment(myMap.Alias, new Size(100, 100), "north_arrow", "aaaa", northFile));
			}
		}

		private void Page_UnLoad(object sender, System.EventArgs e)
		{
			if(AppStateManager.IsManualState())
			{
				AppStateManager.GetStateManagerFromSession().SaveState();
			}
		}

		private void InitState()
		{
			MapInfo.Mapping.Map myMap = this.GetMapObj();

			if(Application.Get("HelloWorldWeb") == null)
			{
				System.Collections.IEnumerator iEnum = MapInfo.Engine.Session.Current.MapFactory.GetEnumerator();
				// Put each map's Layers into a byte[] and keep them in HttpApplicationState for the original layers state.
				while(iEnum.MoveNext())
				{
					MapInfo.Mapping.Map tempMap = iEnum.Current as MapInfo.Mapping.Map;
					byte[] lyrsBits = MapInfo.WebControls.ManualSerializer.BinaryStreamFromObject(tempMap.Layers);
					Application.Add(tempMap.Alias + "_layers", lyrsBits);
				}
				// this is a marker key/value only.
				Application.Add("HelloWorldWeb", "Here");
			}
			else
			{
				// Set the initial Layers of the map because below reasons:
				// 1. There is a LayerControl in the web form.
				// 2. Pooled MapInfo Session objects.
				// 3. Settings of IMapLayer of the map which is from Pooled Session may not be the one you want.
				// 4. There is Layers collection with initial state stored in Application level with byte[] format.
				Object obj = Context.Application[myMap.Alias + "_layers"];

				// if we found out there is correct Layers collection stored in Application level.
				if(obj != null)
				{
					// deserialization applys "original layers setting" to the one in the current map.
					// "Object tempObj" is only for compiling purpose, otherwise it is useless, 
					// because MapXtreme object's deserialization process will put MapXtreme object back to the place it belongs to.
					Object tempObj = MapInfo.WebControls.ManualSerializer.ObjectFromBinaryStream(obj as byte[]);
				}
			}

			// Set the initial state of the map
			// This step is needed because you may get a dirty map from mapinfo Session.Current which is retrived from session pool.
			myMap.Zoom = new MapInfo.Geometry.Distance(25000, DistanceUnit.Mile);
			myMap.Center = new DPoint(27775.805792979896,-147481.33999999985);
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

		}
		#endregion
	}
}
