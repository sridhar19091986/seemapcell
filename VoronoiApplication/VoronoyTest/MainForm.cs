using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Voronoi.Data;
using Voronoi.Mathematics;
using TemperatureVoronoi;

namespace VoronoyTest
{
    public partial class MainForm : Form
    {
        private HashSet<Vector> points = new HashSet<Vector>();
        private Random r = new Random();
        private List<TemperatureLocation> temps = new List<TemperatureLocation>();

        private void RegenerateDiagram()
        {
            points.Clear();
            for (int i = 0; i < 250; i++)
            {
                points.Add(new Vector(10.0 + r.NextDouble() * 600, 10.0 + r.NextDouble() * 450));
            }

        }

        private void RegenerateTemperatures()
        {
            temps.Clear();
            int iterations = Int32.Parse(txtPoints.Text);
            for (int i = 0; i < iterations; i++)
            {
                double x = r.NextDouble() * 640;
                double y = r.NextDouble() * 480;
                double t = 20 + Math.Sqrt((320 - x) * (320 - x) + (240 - y) * (240 - y)) / 20;
                temps.Add(new TemperatureLocation(x, y, t));
            }
        }


        public MainForm()
        {
            InitializeComponent();
            RegenerateDiagram();
            RegenerateTemperatures();
            VoronoiTemparature vt = new VoronoiTemparature(temps);
            vt.ColdColor = pnlCold.BackColor;
            vt.HotColor = pnlHot.BackColor;
            Bitmap bmp = vt.GetMapTemperature(640, 480);
            pbxVisialization.Image = bmp;
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {
            RegenerateTemperatures();
            VoronoiTemparature vt = new VoronoiTemparature(temps);
            vt.ColdColor = pnlCold.BackColor;
            vt.HotColor = pnlHot.BackColor;
            Bitmap bmp = vt.GetMapTemperature(640, 480);
            pbxVisialization.Image = bmp;
        }

        private void rbtVoronoi_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = Fortune.GetVoronoyMap(640, 480, points);
            pbxVisialization.Image = bmp;
        }

        private void rbtDelaunay_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = Fortune.GetDelaunayTriangulation(640, 480, points);
            pbxVisialization.Image = bmp;
        }


        private void pnlHot_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pnlHot.BackColor = colorDialog.Color;
            }
        }

        private void pnlCold_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pnlCold.BackColor = colorDialog.Color;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
