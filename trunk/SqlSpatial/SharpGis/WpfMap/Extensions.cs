namespace SharpGis.WpfMap
{
    using SharpGIS.Gis.Geometries;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;

    internal static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> sequence)
        {
            foreach (T local in sequence)
            {
                col.Add(local);
            }
        }

        public static Rect ToRect(this Envelope env)
        {
            return new Rect(env.MinX, env.MaxY, env.MaxX - env.MinX, env.MaxY - env.MinY);
        }

        public static string ToWpfPathString(this LineString line)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("M ");
            foreach (Coordinate coordinate in line.Vertices)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", new object[] { coordinate.X, coordinate.Y });
            }
            line.Vertices.Reverse();
            foreach (Coordinate coordinate2 in line.Vertices)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", new object[] { coordinate2.X, coordinate2.Y });
            }
            return builder.ToString();
        }

        public static string ToWpfPathString(this Polygon poly)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("M ");
            foreach (Coordinate coordinate in poly.ExteriorRing)
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", new object[] { coordinate.X, coordinate.Y });
            }
            foreach (CoordinateCollection coordinates in poly.InteriorRings)
            {
                builder.Append("M ");
                foreach (Coordinate coordinate2 in coordinates)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", new object[] { coordinate2.X, coordinate2.Y });
                }
            }
            builder.Append("Z");
            return builder.ToString();
        }
    }
}

