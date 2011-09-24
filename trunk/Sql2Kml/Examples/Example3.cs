using System;
using Google.KML;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace Examples
{
    public static class Example3
    {
        public static geKML RunExample(string FileName)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = "My Root Document";

            gePlacemark pm = new gePlacemark();

            //Always complete the boundary by adding an end point that matches your beginning point

            List<geCoordinates> outerCoords = new List<geCoordinates>();
            outerCoords.Add(new geCoordinates(new geAngle90(37), new geAngle180(-114)));
            outerCoords.Add(new geCoordinates(new geAngle90(37), new geAngle180(-108)));
            outerCoords.Add(new geCoordinates(new geAngle90(31), new geAngle180(-108)));
            outerCoords.Add(new geCoordinates(new geAngle90(31), new geAngle180(-114)));
            outerCoords.Add(new geCoordinates(new geAngle90(37), new geAngle180(-114)));

            List<geCoordinates> innerCoords1 = new List<geCoordinates>();
            innerCoords1.Add(new geCoordinates(new geAngle90(36), new geAngle180(-113)));
            innerCoords1.Add(new geCoordinates(new geAngle90(36), new geAngle180(-112)));
            innerCoords1.Add(new geCoordinates(new geAngle90(35), new geAngle180(-112)));
            innerCoords1.Add(new geCoordinates(new geAngle90(35), new geAngle180(-113)));
            innerCoords1.Add(new geCoordinates(new geAngle90(36), new geAngle180(-113)));

            List<geCoordinates> innerCoords2 = new List<geCoordinates>();
            innerCoords2.Add(new geCoordinates(new geAngle90(34), new geAngle180(-113)));
            innerCoords2.Add(new geCoordinates(new geAngle90(34), new geAngle180(-112)));
            innerCoords2.Add(new geCoordinates(new geAngle90(33), new geAngle180(-112)));
            innerCoords2.Add(new geCoordinates(new geAngle90(33), new geAngle180(-113)));
            innerCoords2.Add(new geCoordinates(new geAngle90(34), new geAngle180(-113)));

            geOuterBoundaryIs outer = new geOuterBoundaryIs(new geLinearRing(outerCoords));
                        
            gePolygon poly = new gePolygon(outer);

            geInnerBoundaryIs inner1 = new geInnerBoundaryIs(new geLinearRing(innerCoords1));
            geInnerBoundaryIs inner2 = new geInnerBoundaryIs(new geLinearRing(innerCoords2));
            poly.InnerBoundaries.Add(inner1);
            poly.InnerBoundaries.Add(inner2);

            pm.Geometry = poly;

            doc.Features.Add(pm);

            //Lets add a Line somewhere too...
            geStyle myLineStyle = new geStyle("myLineStyle");
            myLineStyle.LineStyle = new geLineStyle();
            myLineStyle.LineStyle.Color.SysColor = Color.Yellow;
            myLineStyle.LineStyle.Width = 4;  //This may or may not work, depends on the end user's video card

            doc.StyleSelectors.Add(myLineStyle);

            gePlacemark pmLine = new gePlacemark();
            pmLine.StyleUrl = "#myLineStyle";
            pmLine.Name = "Example Line";
            pmLine.Description = "Some description";

            List<geCoordinates> lineCoords = new List<geCoordinates>();
            lineCoords.Add(new geCoordinates(new geAngle90(35),new geAngle180(-117)));
            lineCoords.Add(new geCoordinates(new geAngle90(35),new geAngle180(-106)));

            geLineString line = new geLineString(lineCoords);
            
            pmLine.Geometry = line;

            doc.Features.Add(pmLine);
            
            //Now that we have our document, lets create our KML
            geKML kml = new geKML(doc);

            return kml;

        }
    }
}