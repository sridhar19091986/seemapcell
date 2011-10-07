namespace SharpGis.WpfMap
{
    using System.Windows.Media;

    public class MapPolygon : MapGeometry
    {
        private PointCollection m_Points = new PointCollection();

        public PointCollection Points
        {
            get
            {
                return this.m_Points;
            }
        }
    }
}

