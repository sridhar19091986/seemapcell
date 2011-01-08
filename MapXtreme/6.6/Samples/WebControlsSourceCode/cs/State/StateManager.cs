using System;
using System.Configuration;
using System.Web;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping;

namespace MapInfo.WebControls
{
	/// <summary>
	/// This is an abstract class to handle state.
	/// </summary>
	/// <remarks>Each application has its own way of saving and restoring state. Therefore, the implementation of this class contains code to save and restore state.
	/// Implement this class and its methods and then insert the instance of that class into an ASP.NET session. All MapXtreme tools and controls get the
	/// instance of a statemanager out of the ASP.NET session and then call save and restore methods on it before and after processing is done.
	/// </remarks>
	[Serializable]
	public abstract class  StateManager
	{
		/// <summary>
		/// Name used in the key to put StateManager in an ASP.NET session
		/// </summary>
		/// <remarks>None</remarks>
		public readonly static string StateManagerKey = "StateManager";
		/// <summary>
		/// Name used in the key to put zoom value in an ASP.NET session
		/// </summary>
		/// <remarks>None</remarks>
		public readonly static string ZoomKey= "Zoom";
		/// <summary>
		/// Name used in the key to put center value in an ASP.NET session
		/// </summary>
		/// <remarks>None</remarks>
		public readonly static string CenterKey= "Center";
		/// <summary>
		/// Name used in the key to put selections in an ASP.NET session
		/// </summary>
		/// <remarks>None</remarks>
		public readonly static string SelectionsKey= "Selections";
		/// <summary>
		/// Name used in the key to put layers in an ASP.NET session
		/// </summary>
		/// <remarks>None</remarks>
		public readonly static string LayersKey= "Layers";
		/// <summary>
		/// Key to get an error string from resources.
		/// </summary>
		/// <remarks>None</remarks>
		public static readonly string StateManagerResErr1 = "StateManagerInstanceError";

		// IDictionary to hold parameters used by this state manager.
		private System.Collections.SortedList parametersDictionary = new System.Collections.SortedList();

		/// <summary>
		/// Puts a user instance of a user-implemented state manager in an ASP.NET session.
		/// </summary>
		/// <remarks>Implement StateManager in your application if you are handling state manually. This method allows you to
		/// put an instance of your state manager into an ASP.NET session.</remarks>
		/// <param name="sm">Instance of class derived from StateManager.</param>
		public static void PutStateManagerInSession(StateManager sm)
		{
			HttpContext.Current.Session[GetKey(StateManager.StateManagerKey)] = sm;
		}

		/// <summary>
		/// Gets an instance of a user implemented state manager from an ASP.NET session.
		/// </summary>
		/// <remarks>Tools and controls use this method to get state manager from an ASP.NET session and call restore and save methods.</remarks>
		/// <returns>StateManager object</returns>
		public static StateManager GetStateManagerFromSession() {
			StateManager sm = null;
			if (HttpContext.Current.Session[GetKey(StateManager.StateManagerKey)]  !=  null) {
				sm = (StateManager)HttpContext.Current.Session[GetKey(StateManager.StateManagerKey)];
			}
			return sm;
		}

		/// <summary>
		/// Creates the key to be used when objects are put in the ASP.NET session.
		/// </summary>
		/// <param name="name">Name to be included in the key.</param>
		/// <returns>string containing key</returns>
		/// <remarks>The key is formed as a SessionID_given name.</remarks>
		public static string GetKey(string name)
		{
			return string.Format("{0}_{1}", HttpContext.Current.Session.SessionID, name) ;
		}

		/// <summary>
		/// Indicates that the state management in the web.config is set to Manual. 
		/// </summary>
		/// <returns>Returns <c>true</c> if the state management in the web.config file is Manual.</returns>
		/// <remarks>None</remarks>
		public static bool IsManualState()
		{
			string state = ConfigurationSettings.AppSettings["MapInfo.Engine.Session.State"];
			if(state != null) {
				if(string.Compare(state, "Manual", true) == 0) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Saves the state of the map in the ASP.NET Session.
		/// </summary>
		/// <param name="map">Map object</param>
		/// <remarks>None</remarks>
		public static void SaveMapState(Map map)
		{
			if (IsManualState()) {
				HttpContext.Current.Session[GetKey("Map")] = map;
			}
		}
		
		/// <summary>
		/// Restores the state of the zoom and center from the ASP.NET session.
		/// </summary>
		/// <param name="map">Map object</param>
		/// <remarks>None</remarks>
		public static void RestoreZoomCenterState(Map map) 
		{
			if (IsManualState()) {
				ManualSerializer manualSerializer = new ManualSerializer();

				if (manualSerializer[GetKey("Zoom")] != null)
				{
					map.Zoom = (MapInfo.Geometry.Distance)manualSerializer[GetKey("Zoom")];
				}
				if (manualSerializer[GetKey("Center")] != null)
				{
					map.Center = (DPoint)manualSerializer[GetKey("Center")];
				}
			}
		}

		/// <summary>
		/// Saves the state of the zoom and center in the ASP.NET session.
		/// </summary>
		/// <param name="map">Map object</param>
		/// <remarks>None</remarks>
		public static void SaveZoomCenterState(Map map)
		{
			if (IsManualState()) {
				ManualSerializer manualSerializer = new ManualSerializer();
				manualSerializer[GetKey("Zoom")] = map.Zoom;
				manualSerializer[GetKey("Center")] = map.Center;
			}
		}

		/// <summary>
		/// Saves the state of the selection in the ASP.NET session.
		/// </summary>
		/// <param name="selection">Selection object</param>
		/// <remarks>Restore method is not required because ASP.NET restores ISerializable objects automatically.</remarks>
		public static void SaveSelectionState(Selection selection) {
			if (IsManualState()) {
				HttpContext.Current.Session[GetKey("Selection")] = selection;
			}
		}

		/// <summary>
		/// Saves the state of the layers in the ASP.NET session.
		/// </summary>
		/// <param name="layers">Layers collection</param>
		/// <remarks>Restore method is not required because ASP.NET restores ISerializable objects automatically.</remarks>
		public static void SaveLayersState(Layers layers) {
			if (IsManualState()) {
				HttpContext.Current.Session[GetKey("Layers")] = layers;
			}
		}

		/// <summary>
		/// Method to call to restore state. 
		/// </summary>
        /// <remarks>This method must be implemented to restore the state. The instance of this class is fetched from the ASP.NET session and the restore and save methods are called from the tools.
		/// The implementation is application specific. These methods can contain code to perform other processing.</remarks>
		public abstract void RestoreState();

		/// <summary>
		/// Method to call to save state.
		/// </summary>
        /// <remarks>This method must be implemented to save the state. The instance of this class is fetched from the ASP.NET session and the restore and save methods are called from the tools.
		/// The implementation is application specific. These methods can contain code to perform other processing.</remarks>
		public abstract void SaveState();

		/// <summary>
		/// Returns an IDictionary which could hold key/value pairs used across requests.
		/// </summary>
		/// <remarks>This property only supports get. The value added must be serializable.
		/// <para>You must add <see cref="P:ActiveMapAliasKey"/> with a map alias as the value into this IDictionary in the Page_Load()
		///  if MapInfo.Engine.Session.State is set to "Manual" in the web.config.</para>
		/// <para>This IDictionary passes along parameters among web pages, state manager, and web tools. 
		/// Key/pair values will be saved and restored by asp.net because the state manager will be put into HttpContext.Current.Session.</para>
		/// </remarks>
		public System.Collections.IDictionary ParamsDictionary
		{
			get{ return parametersDictionary;}
		}

		/// <summary>
		/// A key will be used to store the Active map alias. 
		/// </summary>
		/// <remarks>You must add this key into <see cref="P:ParamsDictionary"/> with a map alias value if you want to use this state manager.</remarks>
		public static string ActiveMapAliasKey
		{
			get{ return StateManager.GetKey("ActiveMapAlias");}
		}
	}
}
