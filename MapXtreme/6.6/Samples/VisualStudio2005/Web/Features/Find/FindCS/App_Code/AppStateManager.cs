using System;
using System.Web;
using MapInfo.WebControls;
using MapInfo.Mapping;

namespace FindSampleWeb
{
	/// <summary>
	/// State management for Find web sample.
	/// </summary>
	[Serializable]

    public class AppStateManager : StateManager
    {
        private ManualSerializer _session = null;

        public AppStateManager()
        {
            _session = new ManualSerializer();
        }

        private Map GetMapObj(string mapAlias)
        {
            Map map = null;
            if (mapAlias == null || mapAlias.Length <= 0)
            {
                map = MapInfo.Engine.Session.Current.MapFactory[0];
            }
            else
            {
                map = MapInfo.Engine.Session.Current.MapFactory[mapAlias];
                if (map == null) map = MapInfo.Engine.Session.Current.MapFactory[0];
            }
            return map;
        }

        // Restore the state
        public override void RestoreState()
        {
            string mapAlias = ParamsDictionary[ActiveMapAliasKey] as string;
            Map myMap = GetMapObj(mapAlias);

            // If it was user's first time and the session was not dirty then save this default state to be applied later.
            // If it was a users's first time and the session was dirty then apply the default state  saved in above step to give users a initial state.
            if (IsUsersFirstTime())
            {
                if (IsDirtyMapXtremeSession())
                {
                    RestoreDefaultState(myMap);
                }
                else
                {
                    SaveDefaultState(myMap);
                }
            }
            else
            {
                if (myMap == null) return;
                // Restore everything we saved explictly.
                AppStateManager.RestoreZoomCenterState(myMap);
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("WorkingTable");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("WorkingLayer");

                // Get the workingLayerName saved in WebForm1.aspx page_load.
                string workingLayerName = this.ParamsDictionary["WorkingLayerName"] as String;
                int indexOfLayer = myMap.Layers.IndexOf(myMap.Layers[workingLayerName]);
                // Move the working layer to the Top, so it can be displayed.
                myMap.Layers.Move(indexOfLayer, 0);
            }
        }

        // Save the state
        public override void SaveState()
        {
            string mapAlias = this.ParamsDictionary[AppStateManager.ActiveMapAliasKey] as String;
            MapInfo.Mapping.Map myMap = GetMapObj(mapAlias);

            if (myMap == null) return;

            // Note: for performance reasons, only save the map's center and zoom.	
            AppStateManager.SaveZoomCenterState(myMap);

            // Get the workingLayerName saved in WebForm1.aspx page_load.
            string workingLayerName = this.ParamsDictionary["WorkingLayerName"] as String;
            // Save the map's Working table and layer
            MapInfo.Mapping.FeatureLayer workingLayer = (MapInfo.Mapping.FeatureLayer)myMap.Layers[workingLayerName];
            MapInfo.Data.Table workingTable = workingLayer != null ? workingLayer.Table : null;
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(workingTable, "WorkingTable");
            ManualSerializer.SaveMapXtremeObjectIntoHttpSession(workingLayer, "WorkingLayer");
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
            map.Zoom = (MapInfo.Geometry.Distance)application["DefaultZoom"];
            map.Center = (MapInfo.Geometry.DPoint)application["DefaultCenter"];
        }
	}
}
