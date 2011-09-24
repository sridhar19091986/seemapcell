using System;
using Google.KML;
using SqlServerToKml;

namespace Examples
{
    public static class Example4
    {
        public static geKML RunExample(string FileName)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = "My Root Document";
           
            /*
             
            //Create a Placemark to put in the document
            //This placemark is going to be a point
            //but it could be anything in the Geometry class
            gePlacemark pm = new gePlacemark();

            //Create some coordinates for the point at which
            //this placemark will sit. (Lat / Lon)
            geCoordinates coords = new geCoordinates(
                new geAngle90(37.422067),
            new geAngle180(-122.084437));

            //Create a point with these new coordinates
            gePoint point = new gePoint(coords);

            //Assign the point to the Geometry property of the
            //placemark.
            pm.Geometry = point;
              
            //Now lets add some other properties to our placemark
            pm.Name = "My Placemark";
            pm.Snippet = "This is where I put my Placemark";
            pm.Description = 
                "I wonder where this is...";
            */

            Utilities util = new Utilities();

            // Point
            /*
            gePlacemark pm = util.SqlGeogCommandToKmlPlacemark(
                "server=.;database=geonames;integrated security=sspi",
                "select top(1) geog, name from geonames");

            // LineString
            gePlacemark pm = util.SqlGeogCommandToKmlPlacemark(
                "server=.;database=Sample_USA;integrated security=sspi",
                "select geog, signt + ' ' + signn from Highways where id = 1132");
            */

            // Polygon
            /*
            gePlacemark pm = util.SqlGeogCommandToKmlPlacemark(
                "server=.;database=Sample_USA;integrated security=sspi",
                "select geog, name_2 from counties where id = 847");
             */

            // Multi-ring polygon
            /*
            gePlacemark pm = util.SqlGeogCommandToKmlPlacemark(
                "server=.;database=SpatialSamples;integrated security=sspi",
                "select geog, cntry_name from cntry00 where fips_cntry = 'WE'");
             */

            // MultiLineString
            gePlacemark pm = util.SqlGeogCommandToKmlPlacemark(
                "server=.;database=Sample_USA;integrated security=sspi",
                "select geog, signt + ' ' + signn from Highways where id = 6315");

            //Finally, add the placemark to the document
            doc.Features.Add(pm);

            //Now that we have our document, lets create our KML
            geKML kml = new geKML(doc);

            return kml;

        }
    }   
}      