namespace SharpGis.WpfMap
{
    using System;

    public class MapPath : MapGeometry
    {
        private string m_PathData = "";

        public string PathData
        {
            get
            {
                return this.m_PathData;
            }
            set
            {
                this.m_PathData = value;
            }
        }
    }
}

