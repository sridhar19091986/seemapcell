//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\Stub_WebForm1_aspx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_WebForm1' in file 'WebForm1.aspx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================


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


namespace ThematicsWeb
 {


abstract public class WebForm1 :  System.Web.UI.Page
{
		public static void HandleLabelLayerVisibleStatus(Map map)
		{
			if(map == null) return;
			for(int index=0; index < map.Layers.Count; index++)
			{
				IMapLayer lyr = map.Layers[index];
				if(lyr is LabelLayer)
				{
					LabelLayer ll = lyr as LabelLayer;
					if(map.Layers[SampleConstants.NewLabelLayerAlias] != null 
						&& ll.Alias != SampleConstants.NewLabelLayerAlias)
					{
						ll.Enabled = false;
					}
					else
					{
						ll.Enabled = true;
					}
				}
			}
		}


}



}
