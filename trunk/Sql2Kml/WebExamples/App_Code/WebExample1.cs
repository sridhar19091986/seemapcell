using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Google.KML;

/// <summary>
/// Summary description for Example1
/// </summary>
public static class WebExample1
{
    /// <summary>
    /// Gets the initial kml that contains a NetworkLink.  The network link defined says to go and get another URL
    /// every x number of seconds.  This other URL will contain the data that has the dynamic data we want to see.
    /// </summary>
    /// <param name="linkURL">The other URL that we will get every x number of seconds</param>
    /// <returns></returns>
    public static geKML GetInit(string linkURL)
    {
        geDocument doc = new geDocument();
        doc.Name = "WebExample1Init";
        
        geLink link = new geLink(linkURL);
        link.RefreshMode = geRefreshModeEnum.onInterval;
        link.RefreshInterval = 10;

        geNetworkLink networkLink = new geNetworkLink(link);
        networkLink.Name = "WebExample1Data";
        networkLink.Snippet = "Refreshes every " + networkLink.Link.RefreshInterval.ToString() + " seconds.";
        
        doc.Features.Add(networkLink);
        
        return new geKML(doc);
        
        
    }

    /// <summary>
    /// This kml will contain the data that will be refreshed according to what is defined in GetINIT()
    /// </summary>
    /// <returns></returns>
    public static geKML GetWebExample1Data()
    {
        geDocument doc = new geDocument();

        //you probably won't see this since the document is a child of a network link
        doc.Name = "WebExample1Data"; 
        doc.Snippet = "This data should auto refresh based on the RefreshInterval of our parent NetworkLink";

        gePlacemark pm = new gePlacemark();
        geCoordinates coord = new geCoordinates(new geAngle90(37.422067), new geAngle180(-122.084437));
        gePoint point = new gePoint(coord);

        //Just show the server dateTime so that we know it's updating as advertised.
        pm.Name = DateTime.Now.ToString();
        pm.Description = "This data will be automatically refreshed based on the parent NetworkLink";
        pm.Geometry = point;

        doc.Features.Add(pm);

        return new geKML(doc);

    }



}
