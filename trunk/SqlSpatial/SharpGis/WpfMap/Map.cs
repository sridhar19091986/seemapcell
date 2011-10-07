namespace SharpGis.WpfMap
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    public class Map : DependencyObject
    {
        public static DependencyProperty BoundingBoxProperty = DependencyProperty.Register("BoundingBox", typeof(Rect), typeof(Map));
        public static DependencyProperty LineScaleProperty = DependencyProperty.Register("LineScale", typeof(double), typeof(Map));
        private ObservableCollection<MapRegion> m_regions;
        public static DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(double), typeof(Map));
        public static DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(double), typeof(Map));
        public static DependencyProperty TranslateXPRoperty = DependencyProperty.Register("TranslateX", typeof(double), typeof(Map));
        public static DependencyProperty TranslateYPRoperty = DependencyProperty.Register("TranslateY", typeof(double), typeof(Map));

        public Map()
        {
            this.ScaleX = 1.0;
            this.ScaleY = 1.0;
            this.TranslateX = 0.0;
            this.TranslateY = 0.0;
            this.m_regions = new ObservableCollection<MapRegion>();
            this.BoundingBox = new Rect();
        }

        public double ScaleTo(Size size)
        {
            if ((this.BoundingBox.Width != 0.0) && (this.BoundingBox.Height != 0.0))
            {
                double num = size.Width / this.BoundingBox.Width;
                double num2 = size.Height / this.BoundingBox.Height;
                this.ScaleX = (num < num2) ? num : num2;
                this.ScaleY = -1.0 * this.ScaleX;
            }
            else if (this.BoundingBox.Width != 0.0)
            {
                this.ScaleX = size.Width / this.BoundingBox.Width;
                this.ScaleY = -this.ScaleX;
            }
            else if (this.BoundingBox.Height != 0.0)
            {
                this.ScaleX = size.Height / this.BoundingBox.Height;
                this.ScaleY = -this.ScaleX;
            }
            this.LineScale = 1.0 / this.ScaleX;
            this.TranslateX = -this.BoundingBox.Left + (((size.Width / this.ScaleX) - this.BoundingBox.Width) / 2.0);
            this.TranslateY = -this.BoundingBox.Bottom - (((-size.Height / this.ScaleY) - this.BoundingBox.Height) / 2.0);
            return this.ScaleX;
        }

        public Rect BoundingBox
        {
            get
            {
                return (Rect) base.GetValue(BoundingBoxProperty);
            }
            set
            {
                base.SetValue(BoundingBoxProperty, value);
            }
        }

        public double LineScale
        {
            get
            {
                return (double) base.GetValue(LineScaleProperty);
            }
            set
            {
                base.SetValue(LineScaleProperty, value);
            }
        }

        public ObservableCollection<MapRegion> Regions
        {
            get
            {
                return this.m_regions;
            }
        }

        public double ScaleX
        {
            get
            {
                return (double) base.GetValue(ScaleXProperty);
            }
            set
            {
                base.SetValue(ScaleXProperty, value);
            }
        }

        public double ScaleY
        {
            get
            {
                return (double) base.GetValue(ScaleYProperty);
            }
            set
            {
                base.SetValue(ScaleYProperty, value);
            }
        }

        public double TranslateX
        {
            get
            {
                return (double) base.GetValue(TranslateXPRoperty);
            }
            set
            {
                base.SetValue(TranslateXPRoperty, value);
            }
        }

        public double TranslateY
        {
            get
            {
                return (double) base.GetValue(TranslateYPRoperty);
            }
            set
            {
                base.SetValue(TranslateYPRoperty, value);
            }
        }
    }
}

