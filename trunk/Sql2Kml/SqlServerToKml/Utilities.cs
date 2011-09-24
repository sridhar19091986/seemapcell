using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using Google.KML;
using System.Data.SqlClient;

namespace SqlServerToKml
{
    public static class MyExtensions
    {
        public static string AsKML(this SqlGeography geog) 
        {
            return AsKML(geog, "SQL Server KML Document", "Sample Name", "Sample Description");
        }

        public static string AsKML(this SqlGeography geog, string pname) 
        {
            return AsKML(geog, "SQL Server KML Document", pname, pname);
        }

        public static string AsKML(this SqlGeography geog, string pname, string description) 
        {
            return AsKML(geog, "SQL Server KML Document", pname, description);
        }

        public static string AsKML(this SqlGeography geog, string docname, string pname, string description)
        {
            Utilities u = new Utilities();

            if (u.IsNullOrEmpty(geog))
                return "";

            geDocument doc = u.SqlGeogInstanceToKmlDoc(geog, docname, pname, description);
            geKML kml = new geKML(doc);
            return System.Text.Encoding.ASCII.GetString(kml.ToKML());
        }
 
    }  

    public class Utilities
    {
        public geDocument SqlGeogCommandToKmlDoc(string docname, string connstr, string cmdstr)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = docname;

            //Finally, add the placemark to the document
            gePlacemark pm = null;
            pm = SqlGeogCommandToKmlPlacemark(connstr, cmdstr);
            if (pm != null)
                doc.Features.Add(pm);

            return doc;
        }

        public geDocument SqlGeogInstanceToKmlDoc(SqlGeography geog, string docname, string pname, string description)
        {
            // Use a Document as the root of the KML
            geDocument doc = new geDocument();
            doc.Name = docname;

            //Finally, add the placemark to the document
            if (!IsNullOrEmpty(geog))
                doc.Features.Add(SqlGeogToKmlPlacemark(geog, pname, description));

            return doc;
        }

        public gePlacemark SqlGeogCommandToKmlPlacemark(string connstr, string cmdstr)
        {
            gePlacemark pm = null;

            using (SqlConnection conn = new SqlConnection(connstr))
            using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();

                    SqlGeography geog = (SqlGeography)rdr.GetValue(0);
                    if (!IsNullOrEmpty(geog))
                    {

                        if (rdr.FieldCount == 3)
                            pm = SqlGeogToKmlPlacemark(geog,
                                                    rdr.GetString(1),
                                                    rdr.GetString(2));
                        else
                            pm = SqlGeogToKmlPlacemark(geog,
                                                    rdr.GetString(1),
                                                    rdr.GetString(1));
                    }
                }
                catch 
                {
                    return null;
                }
            }

            return pm;
        }

        public geFolder SqlGeogCommandToKmlFolder(string connstr, string cmdstr, string fldname)
        {
            geFolder fld = new geFolder();
            fld.Name = fldname;

            using (SqlConnection conn = new SqlConnection(connstr))
            using (SqlCommand cmd = new SqlCommand(cmdstr, conn))
            {
                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SqlGeography geog = (SqlGeography)rdr.GetValue(0);
                        if (!IsNullOrEmpty(geog))
                        {
                            gePlacemark pm = new gePlacemark();

                            if (rdr.FieldCount == 3)
                                pm = SqlGeogToKmlPlacemark(geog,
                                                        rdr.GetString(1),
                                                        rdr.GetString(2));
                            else
                                pm = SqlGeogToKmlPlacemark(geog,
                                                        rdr.GetString(1),
                                                        rdr.GetString(1));
                            fld.Features.Add(pm);
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            return fld;
        }

        public gePlacemark SqlGeogToKmlPlacemark(SqlGeography geog, string pname, string description )
        {
            if (IsNullOrEmpty(geog))
                return null;

            gePlacemark pm = new gePlacemark();
            pm.Description = description;
            pm.Name = pname;

            switch (geog.STGeometryType().Value)
            {
                case "Point":
                    pm.Geometry = DoPoint(geog);
                    break;
                case "LineString":
                    pm.Geometry = DoLineString(geog);
                    break;
                case "Polygon":
                    pm.Geometry = DoPolygon(geog);
                    break;
                case "MultiPoint":
                case "MultiLineString":
                case "MultiPolygon":
                case "GeometryCollection":
                    pm.Geometry = DoMulti(geog);
                    break;
                default:
                    break;                 
            }

            return pm;
        }

        public bool IsNullOrEmpty(SqlGeography geog)
        {
            if (geog.IsNull)
                return true;

            if (geog.STIsEmpty() == SqlBoolean.Null)
                return true;
            else
                return geog.STIsEmpty().Value;
        }

        geGeometry DoPoint(SqlGeography g)
        {
            geCoordinates coor = new geCoordinates(new geAngle90(g.Lat.Value), new geAngle180(g.Long.Value));
            gePoint p = new gePoint(coor);
            return p;
        }

        geGeometry DoLineString(SqlGeography g)
        {
            List<geCoordinates> coor = new List<geCoordinates>(); 
            int points = 0;
            while (points < g.STNumPoints().Value)
            {
                coor.Add(new geCoordinates(new geAngle90(g.STPointN(points + 1).Lat.Value), new geAngle180(g.STPointN(points + 1).Long.Value)));
                points++;
            }
            geLineString ls = new geLineString(coor);
            return ls;
        }

        geGeometry DoPolygon(SqlGeography g)
        {
            List<geCoordinates> outcoor = new List<geCoordinates>();
            List<geCoordinates>[] innercoor = null;
            geInnerBoundaryIs[] inner = null;
            int points = 0; 
            int rings = 0;
            
            while (points < g.RingN(1).STNumPoints().Value)
            {
                outcoor.Add(new geCoordinates(new geAngle90(g.STPointN(points + 1).Lat.Value), new geAngle180(g.STPointN(points + 1).Long.Value)));
                points++;
            }

            geOuterBoundaryIs outer = new geOuterBoundaryIs(new geLinearRing(outcoor));
            gePolygon poly = new gePolygon(outer);

            if (g.NumRings().Value > 1)
            {
                innercoor = new List<geCoordinates>[g.NumRings().Value - 1];
                inner = new geInnerBoundaryIs[g.NumRings().Value - 1];
                rings = 1;
                while (rings < g.NumRings().Value)
                {
                    innercoor[rings-1] = new List<geCoordinates>();
                    points = 0;
                    while (points < g.RingN(rings+1).STNumPoints().Value)
                    {
                        geCoordinates c = new geCoordinates(new geAngle90(g.RingN(rings+1).STPointN(points + 1).Lat.Value), new geAngle180(g.RingN(rings+1).STPointN(points + 1).Long.Value));
                        innercoor[rings-1].Add(c);
                        points++;
                    }
                    inner[rings-1] = new geInnerBoundaryIs(new geLinearRing(innercoor[rings-1]));
                    poly.InnerBoundaries.Add(inner[rings-1]);
                    rings++;
                }
            }

            return poly;
        }

        geGeometry DoMulti(SqlGeography g)
        {
            geMultiGeometry multi = new geMultiGeometry();
            List<geGeometry> geos = new List<geGeometry>();

            int numgeos = 0;

            while (numgeos < g.STNumGeometries().Value)
            {
                SqlGeography currentgeo = g.STGeometryN(numgeos + 1);
                if (!IsNullOrEmpty(currentgeo))
                {
                    switch (currentgeo.STGeometryType().Value)
                    {
                        case "Point":
                            multi.Geometries.Add(DoPoint(currentgeo));
                            break;
                        case "LineString":
                            multi.Geometries.Add(DoLineString(currentgeo));
                            break;
                        case "Polygon":
                            multi.Geometries.Add(DoPolygon(currentgeo));
                            break;
                        case "MultiPoint":
                        case "MultiLineString":
                        case "MultiPolygon":
                        case "GeometryCollection":
                            multi.Geometries.Add(DoMulti(currentgeo));
                            break;
                        default:
                            break;
                    }
                }
                numgeos++;
            }

            return multi;
        }
    }
}
