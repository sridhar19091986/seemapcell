using System;
using System.Web;
using MapInfo.WebControls;
using MapInfo.Mapping;
namespace ThematicsWeb
{
    /// <summary>
    /// State management can be complex operation. It is efficient to save and restore what is needed.
    /// The method used here is described in the BEST PRACTISES documentation. This is a template application
    /// which changes zoom, center, default selection and layer visibility. Hence we save and restore only these objects.
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
                AppStateManager.RestoreZoomCenterState(myMap);
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("mdb_table");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("theme_table");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("theme_layer");
                ManualSerializer.RestoreMapXtremeObjectFromHttpSession("group_layer");

                // Important Note:
                //   The saved theme layer, group layer and tables get restored automatically by ASP.NET serialization.
                //   By the time this method is called, this has already happened.

                // We need to move the GroupLayer to the Top, so the contents within it could be displayed.
                // This step is necessary since the GroupLayer is created on the fly and not loaded through pre-loaded workspace and 
                // the position of the groupLayer inside the Layers collection is not guarantee when it get deserialized back 
                // since we don't save the whole Layers collection for performance purpose in this sample.
                int indexOfGroupLayer = myMap.Layers.IndexOf(myMap.Layers[SampleConstants.GroupLayerAlias]);
                if (indexOfGroupLayer > 0)
                {
                    myMap.Layers.Move(indexOfGroupLayer, 0);
                    // Need to turn off other labellayer if we have a demo labellayer in the GroupLayer.
                    // This step is needed if "MapInfo.Engine.Session.Pooled" value="false" in the web.config,
                    // Otherwise you would see other LabelLayers show up on the map.
                    WebForm1.HandleLabelLayerVisibleStatus(myMap);
                }
            }
        }

        // Save the state
        public override void SaveState()
        {
            string mapAlias = this.ParamsDictionary[AppStateManager.ActiveMapAliasKey] as String;
            MapInfo.Mapping.Map map = GetMapObj(mapAlias);

            if (map != null)
            {
                MapInfo.WebControls.StateManager.SaveZoomCenterState(map);
                // Note: Please be aware of the order of assigning below objects to HttpContext.Current.Session.
                // The order matters in this case since theme_table contains some temp columns constructed from mdb_table.
                // and some themes/modifiers in the theme_layer are based on the theme_table.
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Catalog[SampleConstants.EWorldAlias], "mdb_table");
                //HttpContext.Current.Session["mdb_table"] = MapInfo.Engine.Session.Current.Catalog[SampleConstants.EWorldAlias];
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(MapInfo.Engine.Session.Current.Catalog[SampleConstants.ThemeTableAlias], "theme_table");
                //HttpContext.Current.Session["theme_table"] = MapInfo.Engine.Session.Current.Catalog[SampleConstants.ThemeTableAlias];
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers[SampleConstants.ThemeLayerAlias], "theme_layer");
                //HttpContext.Current.Session["theme_layer"] = map.Layers[SampleConstants.ThemeLayerAlias];
                // Group Layer holds temp object theme or label layers which varies with requests.
                ManualSerializer.SaveMapXtremeObjectIntoHttpSession(map.Layers[SampleConstants.GroupLayerAlias], "group_layer");
                //HttpContext.Current.Session["group_layer"] = map.Layers[SampleConstants.GroupLayerAlias];
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
            map.Zoom = (MapInfo.Geometry.Distance)application["DefaultZoom"];
            map.Center = (MapInfo.Geometry.DPoint)application["DefaultCenter"];
        }
    }
}
