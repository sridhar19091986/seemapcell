namespace SharpGis.WpfMap
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows;

    public class MapGeometry : DependencyObject
    {
        [CompilerGenerated]
        private Dictionary<string, object> <Attributes>k__BackingField;
        public static readonly DependencyProperty RegionProperty = RegionPropertyKey.DependencyProperty;
        protected static readonly DependencyPropertyKey RegionPropertyKey = DependencyProperty.RegisterReadOnly("Region", typeof(MapRegion), typeof(MapPolygon), new PropertyMetadata());

        internal void SetRegion(MapRegion r)
        {
            base.SetValue(RegionPropertyKey, r);
        }

        public Dictionary<string, object> Attributes
        {
            [CompilerGenerated]
            get
            {
                return this.<Attributes>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Attributes>k__BackingField = value;
            }
        }

        public MapRegion Region
        {
            get
            {
                return (MapRegion) base.GetValue(RegionProperty);
            }
        }
    }
}

