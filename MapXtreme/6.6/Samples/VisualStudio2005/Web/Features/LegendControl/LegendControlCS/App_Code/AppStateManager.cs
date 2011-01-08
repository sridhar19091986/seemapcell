//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'AppStateManager' in the code behind file in 'WebForm1.aspx.cs' is moved to this file.
//====================================================================


using System;
using System.Web;
using MapInfo.Engine;
using MapInfo.Geometry;
using MapInfo.Mapping.Legends;
using MapInfo.WebControls;
using MapInfo.Mapping;


namespace LegendControlWeb
 {


	[Serializable]
	public class AppStateManager: StateManager 
	{
		private ManualSerializer _session = null;

		public AppStateManager() 
		{
			_session = new ManualSerializer();
		}

		// Restore the state
		public override void RestoreState() 
		{
			string mapAlias = ParamsDictionary[ActiveMapAliasKey] as string;
			Map map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];

			// If it was user's first time and the session was not dirty then save this default state to be applied later.
			// If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
			if (IsUsersFirstTime()) 
			{
				if (IsDirtySession(map)) 
				{
					RestoreDefaultState(map);
				} 
				else 
				{
					SaveDefaultState(map);
				}
			} 
			else 
			{
				// If it is not user's first time then restore the last state they saved
				RestoreZoomCenterState(map);
				// Just by setting it to temp variables the objects are serialized into session. There is no need to set them explicitly.
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("Layers");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("Selection");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("Legends");
            }
		}

		// Save the state
		public override void SaveState() 
		{
			string mapAlias = ParamsDictionary[ActiveMapAliasKey] as string;
			Map map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];
			if (map != null) 
			{
				SaveZoomCenterState(map);
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers, "Layers");
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Selections.DefaultSelection, "Selection");
				//need to save map's Legends here
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Legends, "Legends");
			}
		}


		// This method checks if the mapinfo session got from the pool is dirty or clean
		private bool IsDirtySession(Map map) 
		{
			// Check if layers collection in application state if it is already there session is dirty
			return (HttpContext.Current.Application["Layers"] != null);
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
			if (application["Zoom"] == null) 
			{
				// Store default selection
				application["Selection"] = ManualSerializer.BinaryStreamFromObject(MapInfo.Engine.Session.Current.Selections.DefaultSelection);
				// Store layers collection
				application["Layers"] = ManualSerializer.BinaryStreamFromObject(map.Layers);
				// Store the original zoom and center.
				application["Center"] = map.Center;
				application["Zoom"] = map.Zoom;
			}
		}

		// When session is dirty but it is first time for user, this will be applied to give users it's initial state
		private void RestoreDefaultState(Map map) 
		{
			HttpApplicationState application = HttpContext.Current.Application;
			// Get the default layers, center, and zoomfrom the Application. Clear Layers first, 
			//this resets the zoom and center which we will set later
			map.Layers.Clear();

			//Just by deserializing the binary stream we reset the MapFactory Deault layers collection
			byte[] bytes = application["Layers"] as byte[];
			Object obj = ManualSerializer.ObjectFromBinaryStream(bytes);

			// For default selection
			bytes = application["Selection"] as byte[];
			obj = ManualSerializer.ObjectFromBinaryStream(bytes);

			// For zoom and center
			map.Zoom  = (MapInfo.Geometry.Distance)application["Zoom"];
			map.Center = (DPoint)application["Center"];
		}
	}

}