using System;
using Google.KML;
using System.IO;

namespace Examples
{
    public static class Example2
    {
        public static geKML RunExample(string FileName)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = "My Root Document";

            // Create a new style to be added the doc
            geStyle myStyle = new geStyle("myStyle");
            
            //add a new IconStyle to the style
            myStyle.IconStyle = new geIconStyle();
            myStyle.IconStyle.Icon = new geIcon("Example2.png");
            myStyle.IconStyle.Scale = 2F; //or (float)2
            
            //Add the style
            doc.StyleSelectors.Add(myStyle);

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

            //Set the placemark's style to the style we created above
            pm.StyleUrl = "#myStyle";

            //Now lets add some other properties to our placemark
            pm.Name = "My Placemark";
            pm.Snippet = "This is where I put my Placemark";
            pm.Description = 
                "I wonder where this is...GOOGLE!";

            //Finally, add the placemark to the document
            doc.Features.Add(pm);

            //Now that we have our document, lets create our KML
            geKML kml = new geKML(doc);

            //Add supporting files to the KMZ (assuming it's going to be rendered as KMZ
            byte[] myFile = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\images\\Example2.png");
            kml.Files.Add("Example2.png", myFile);

            return kml;

        }
    }   
}      