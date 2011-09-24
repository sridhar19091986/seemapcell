using System;
using Google.KML;
using SqlServerToKml;

namespace Examples
{
    public static class Example5
    {
        public static geKML RunExample(string FileName)
        {
 
            Utilities util = new Utilities();

            // Point
            /*
            geDocument doc = util.SqlGeogCommandToKmlDoc("SQL Server Spatial Sample Doc",
                "server=.;database=geonames;integrated security=sspi",
                "select top(1) geog, name from geonames");

            // LineString
            geDocument doc = util.SqlGeogCommandToKmlDoc("SQL Server Spatial Sample Doc",
                "server=.;database=Sample_USA;integrated security=sspi",
                "select geog, signt + ' ' + signn from Highways where id = 1132");
            */

            // Polygon
            /*
            geDocument doc = util.SqlGeogCommandToKmlDoc("SQL Server Spatial Sample Doc",
                "server=.;database=Sample_USA;integrated security=sspi",
                "select geog, name_2 from counties where id = 847");
             */

            // Multi-ring polygon
            /*
            geDocument doc = util.SqlGeogCommandToKmlDoc("SQL Server Spatial Sample Doc",
                "server=.;database=SpatialSamples;integrated security=sspi",
                "select geog, cntry_name from cntry00 where fips_cntry = 'WE'");
             */

            // MultiLineString
            geDocument doc = util.SqlGeogCommandToKmlDoc("SQL Server Spatial Sample Doc",
                " Data Source=localhost;Initial Catalog=JiangMengGnInterface;Integrated Security=True",
                "select  geom,cell from MCOMSite_region where id=1");
           
            //Now that we have our document, lets create our KML
           
            geKML kml = new geKML(doc);

            return kml;

        }
    }   
}      