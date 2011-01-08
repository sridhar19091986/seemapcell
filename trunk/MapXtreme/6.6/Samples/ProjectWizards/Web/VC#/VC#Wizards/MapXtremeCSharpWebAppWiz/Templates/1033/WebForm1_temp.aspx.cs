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
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;
using MapInfo.WebControls;

namespace [!output SAFE_NAMESPACE_NAME] {
	/// <summary>
	/// Summary description for  [!output SAFE_NAMESPACE_NAME].
	/// </summary>
	public class [!output SAFE_CLASS_NAME] : System.Web.UI.Page {
		private void Page_Load(object sender, System.EventArgs e) {
			// If the StateManager doesn't exist in the session put it else get it.
			if (StateManager.GetStateManagerFromSession() == null)
				StateManager.PutStateManagerInSession(new AppStateManager());

			// Now Restore State
			StateManager.GetStateManagerFromSession().ParamsDictionary[StateManager.ActiveMapAliasKey] = MapControl1.MapAlias;
			StateManager.GetStateManagerFromSession().RestoreState();
		}

		// At the time of unloading the page, save the state
		private void Page_UnLoad(object sender, System.EventArgs e) {
			StateManager.GetStateManagerFromSession().SaveState();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {    
			this.Load += new System.EventHandler(this.Page_Load);
			this.Unload += new System.EventHandler(this.Page_UnLoad);
		}
		#endregion
	}

	/// <summary>
	/// State management can be complex operation. It is efficient to save and restore what is needed.
	/// The method used here is described in the BEST PRACTISES documentation. This is a template application
	/// which changes zoom, center, default selection and layer visibility. Hence we save and restore only these objects.
	/// </summary>
	[Serializable]
	public class AppStateManager: StateManager {
		private ManualSerializer _session = null;

		public AppStateManager() {
			_session = new ManualSerializer();
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

		// Restore the state
		public override void RestoreState() {
			string mapAlias = ParamsDictionary[ActiveMapAliasKey] as string;
			Map map = GetMapObj(mapAlias);

			// If it was user's first time and the session was not dirty then save this default state to be applied later.
			// If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
			if (IsUsersFirstTime()) {
				if (IsDirtyMapXtremeSession()) {
					RestoreDefaultState(map);
				} else {
					SaveDefaultState(map);
				}
			} else {
				// If it is not user's first time then restore the last state they saved
				RestoreZoomCenterState(map);
				// Just by setting it to temp variables the objects are serialized into session. There is no need to set them explicitly.
				Layers lyrsTemp = _session["Layers"] as Layers;
				Selection defTemp = _session["Selection"] as Selection;
			}
		}

		// Save the state
		public override void SaveState() {
			string mapAlias = ParamsDictionary[ActiveMapAliasKey] as string;
			Map map = GetMapObj(mapAlias);

			if (map != null) {
				SaveZoomCenterState(map);
				_session["Layers"]= map.Layers;
				_session["Selection"] = MapInfo.Engine.Session.Current.Selections.DefaultSelection;
			}
		}


		// This method checks if the mapinfo session got from the pool is dirty or clean
		private bool IsDirtyMapXtremeSession() 
		{
			// Check if MapXtreme Session is dirty by looking for our flag
			return MapInfo.Engine.Session.Current.CustomProperties["Dirty"] != null;
		}

		// Check if this is user's first time accessing this page. IF there is a zoom value in the asp.net session then it is not user's first time.
		private bool IsUsersFirstTime() 
		{
			return (HttpContext.Current.Session[StateManager.GetKey("Zoom")] == null);
		}

		// When the session is not dirty these values are initial state of the session.
		private void SaveDefaultState(Map map) 
		{
			HttpApplicationState application = HttpContext.Current.Application;
			if (application["DefaultZoom"] == null) 
			{
				// Store default selection
				application["DefaultSelection"] = ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Selections.DefaultSelection);
				// Store layers collection
				application["DefaultLayers"] = ManualSerializer.BinaryStreamFromObject(map.Layers);
				// Store the original zoom and center.
				application["DefaultCenter"] = map.Center;
				application["DefaultZoom"] = map.Zoom;
			}
			// Set the dirty flag in this MapXtreme Session Instance
			MapInfo.Engine.Session.Current.CustomProperties.Add("Dirty", true);
		}

		// When session is dirty but it is first time for user, this will be applied to give users it's initial state
		private void RestoreDefaultState(Map map) 
		{
			HttpApplicationState application = HttpContext.Current.Application;
			// Get the default layers, center, and zoomfrom the Application. Clear Layers first, 
			//this resets the zoom and center which we will set later
			map.Layers.Clear();

			//Just by deserializing the binary stream we reset the MapFactory Deault layers collection
			byte[] bytes = application["DefaultLayers"] as byte[];
			Object obj = ManualSerializer.ObjectFromBinaryStream(bytes);

			// For default selection
			bytes = application["DefaultSelection"] as byte[];
			obj = ManualSerializer.ObjectFromBinaryStream(bytes);

			// For zoom and center
			map.Zoom  = (MapInfo.Geometry.Distance)application["DefaultZoom"];
			map.Center = (DPoint)application["DefaultCenter"];
		}
	}
}
