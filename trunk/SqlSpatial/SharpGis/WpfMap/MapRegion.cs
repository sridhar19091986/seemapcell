namespace SharpGis.WpfMap
{
    using SharpGIS.Gis.Geometries;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public class MapRegion : DependencyObject
    {
        [CompilerGenerated]
        private double <LineThickness>k__BackingField;
        public static readonly DependencyProperty AttributesProperty = DependencyProperty.Register("Attributes", typeof(Dictionary<string, object>), typeof(MapRegion));
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(string), typeof(MapRegion));
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register("Geometry", typeof(SharpGIS.Gis.Geometries.Geometry), typeof(MapRegion));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(MapRegion));
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register("LineColor", typeof(string), typeof(MapRegion));
        private Rect m_BoundingBox;
        private Map m_map;
        private ObservableCollection<MapGeometry> m_polygons;

        public MapRegion(Map map)
        {
            this.m_map = map;
            this.LineThickness = 1.0;
            this.m_polygons = new ObservableCollection<MapGeometry>();
            this.Attributes = new Dictionary<string, object>();
            this.IsSelected = false;
            this.LineScale = 0.1;
            this.Geometries.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Geometries_CollectionChanged);
        }

        private void Geometries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object obj2 in e.NewItems)
                {
                    (obj2 as MapGeometry).SetRegion(this);
                }
            }
        }

        private void PolygonCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ((MapPolygon) e.NewItems).SetRegion(this);
                    return;

                case NotifyCollectionChangedAction.Remove:
                    ((MapPolygon) e.OldItems).SetRegion(null);
                    return;

                case NotifyCollectionChangedAction.Replace:
                    ((MapPolygon) e.NewItems).SetRegion(this);
                    ((MapPolygon) e.OldItems).SetRegion(null);
                    return;
            }
        }

        public Dictionary<string, object> Attributes
        {
            get
            {
                return (Dictionary<string, object>) base.GetValue(AttributesProperty);
            }
            set
            {
                base.SetValue(AttributesProperty, value);
            }
        }

        public Rect BoundingBox
        {
            get
            {
                return this.m_BoundingBox;
            }
            set
            {
                this.m_BoundingBox = value;
            }
        }

        public string FillColor
        {
            get
            {
                return (string) base.GetValue(FillColorProperty);
            }
            set
            {
                base.SetValue(FillColorProperty, value);
            }
        }

        public ObservableCollection<MapGeometry> Geometries
        {
            get
            {
                return this.m_polygons;
            }
        }

        public SharpGIS.Gis.Geometries.Geometry Geometry
        {
            get
            {
                return (SharpGIS.Gis.Geometries.Geometry) base.GetValue(GeometryProperty);
            }
            set
            {
                base.SetValue(GeometryProperty, value);
            }
        }

        public bool IsSelected
        {
            get
            {
                return (bool) base.GetValue(IsSelectedProperty);
            }
            set
            {
                base.SetValue(IsSelectedProperty, value);
            }
        }

        public string LineColor
        {
            get
            {
                return (string) base.GetValue(LineColorProperty);
            }
            set
            {
                base.SetValue(LineColorProperty, value);
            }
        }

        public double LineScale
        {
            get
            {
                if (this.m_map != null)
                {
                    return (this.LineThickness / this.m_map.ScaleX);
                }
                return this.LineThickness;
            }
            set
            {
            }
        }

        public double LineThickness
        {
            [CompilerGenerated]
            get
            {
                return this.<LineThickness>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LineThickness>k__BackingField = value;
            }
        }
    }
}

