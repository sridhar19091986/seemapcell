using System;
using Google.KML;

namespace Examples
{
    public static class Example1
    {
        public static geKML RunExample(string FileName)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = "My Root Document";
           
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

            //Finally, add the placemark to the document
            doc.Features.Add(pm);

            //Now that we have our document, lets create our KML
            geKML kml = new geKML(doc);

            return kml;

        }
    }   
}      