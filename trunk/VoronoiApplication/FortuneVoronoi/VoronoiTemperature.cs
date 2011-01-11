using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TemperatureVoronoi;
using Voronoi.Data;
using Voronoi.Mathematics;

namespace TemperatureVoronoi
{
    public struct TemperatureLocation
    {
        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        private double t;

        public double T
        {
            get { return t; }
            set { t = value; }
        }

        public TemperatureLocation(double x, double y, double t)
        {
            this.x = x;
            this.y = y;
            this.t = t;
        }

        public double GetDistance(TemperatureLocation tl)
        {
            return Math.Sqrt((this.x - tl.x) * (this.x - tl.x) + (this.y - tl.y) * (this.y - tl.y));
        }
    }

    public class VoronoiTemparature
    {
        private List<TemperatureLocation> temperature = new List<TemperatureLocation>();
        private VoronoiGraph graph;
        private Color hotColor = Color.Red;

        public Color HotColor
        {
            get { return hotColor; }
            set { hotColor = value; }
        }
        private Color coldColor = Color.Blue;

        public Color ColdColor
        {
            get { return coldColor; }
            set { coldColor = value; }
        }

        private double maxTemperature;
        private double minTemperature;

        public VoronoiTemparature(List<TemperatureLocation> temp)
        {
            temperature = temp;
            List<Vector> vectors = new List<Vector>();
            foreach (TemperatureLocation t in temperature)
            {
                vectors.Add(new Vector(t.X, t.Y));
            }
            maxTemperature = GetMaxTemperature();
            minTemperature = GetMinTemperature();
            graph = Fortune.ComputeVoronoiGraph(vectors);
        }

        private double GetMinTemperature()
        {
            double minTemp = temperature[0].T;
            foreach (TemperatureLocation t in temperature)
            {
                if (t.T < minTemp)
                {
                    minTemp = t.T;
                }
            }
            return minTemp;
        }

        private double GetMaxTemperature()
        {
            double maxTemp = temperature[0].T;
            foreach (TemperatureLocation t in temperature)
            {
                if (t.T > maxTemp)
                {
                    maxTemp = t.T;
                }
            }
            return maxTemp;
        }

        private Color GetColorOfPoint(double x, double y)
        {
            Color result = coldColor;
            double temp = 0;
            foreach (TemperatureLocation t in temperature)
            {
                if ((t.X == x) & (t.Y == y))
                {
                    temp = t.T;
                    break;
                }
            }
            double k = (temp - minTemperature) / (maxTemperature - minTemperature);
            double r = coldColor.R + k * (hotColor.R - coldColor.R);
            double g = coldColor.G + k * (hotColor.G - coldColor.G);
            double b = coldColor.B + k * (hotColor.B - coldColor.B);
            return Color.FromArgb((int)r, (int)g, (int)b);
        }

        private Bitmap Smooth(Bitmap source)
        {
            Bitmap bmp = new Bitmap(source);
            for (int i = 1; i < source.Width-1; i++)
            {
                for (int j = 1; j < source.Height-1; j++)
                {
                    Color c10 = bmp.GetPixel(i - 1, j + 1);
                    Color c01 = bmp.GetPixel(i + 1, j - 1);
                    Color c11 = bmp.GetPixel(i - 1, j - 1);
                    Color c00 = bmp.GetPixel(i + 1, j + 1);
                    int r = (int)((c01.R + c10.R + c11.R + c00.R) / 4);
                    int g = (int)((c01.G + c10.G + c11.G + c00.G) / 4);
                    int b = (int)((c01.B + c10.B + c11.B + c00.B) / 4);
                    bmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return bmp;
        }

        public Bitmap GetMapTemperature(int weight, int height)
        {
            Bitmap bmp = new Bitmap(weight, height);
            Graphics g = Graphics.FromImage(bmp);

            foreach (TemperatureLocation t in temperature)
            {
                Vector v = new Vector(t.X,t.Y);
                foreach (object obj in graph.Edges)
                {
                    VoronoiEdge e = (VoronoiEdge)obj;
                    if (((e.LeftData[0] == v[0]) & (e.LeftData[1] == v[1]))|((e.RightData[0] == v[0]) & (e.RightData[1] == v[1])))
                    { 
                        SolidBrush brush = new SolidBrush(GetColorOfPoint(v[0],v[1]));
                        try
                        {
                            g.FillPolygon(brush, new Point[3] {new Point((int)v[0],(int)v[1]),
                                                          new Point((int)e.VVertexA[0],(int)e.VVertexA[1]),  
                                                          new Point((int)e.VVertexB[0],(int)e.VVertexB[1])});
                        }
                        catch
                        {
                        }
                    }
                    //g.DrawLine(Pens.Black, (int)e.VVertexA[0], (int)e.VVertexA[1], (int)e.VVertexB[0], (int)e.VVertexB[1]);
                }
            }

            return Smooth(bmp);
        }
    }
}
