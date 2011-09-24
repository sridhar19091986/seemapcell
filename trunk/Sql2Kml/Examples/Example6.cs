using System;
using Google.KML;
using SqlServerToKml;

namespace Examples
{
    public static class Example6
    {
        public static geKML RunExample(string FileName)
        {
 
            Utilities util = new Utilities();

            // Point
            
            geFolder fld = util.SqlGeogCommandToKmlFolder(
                "server=.;database=geonames;integrated security=sspi",
                "select top(10) geog, name from geonames",
                "Sample Folder");

            //Now that we have our document, lets create our KML
            geKML kml = new geKML(fld);

            return kml;

        }
    }   
}      