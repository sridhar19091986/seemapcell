using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace SpatialWpf
{
    #region GeometryInfo class

    public class GeometryInfo
        : INotifyPropertyChanged
    {
        #region Member variables

        private string _id = string.Empty;
        private SqlGeometry _data;
        private Brush fill = Brushes.Green;

        #endregion

        #region Properties

        internal SqlGeometry Data
        {
            get { return _data; }
            set
            {
                if (_data == null || !_data.Equals(value))
                {
                    _data = value;
                    NotifyPropertyChanged("Data");
                    NotifyPropertyChanged("Area");
                    NotifyPropertyChanged("Length");
                    NotifyPropertyChanged("Path");
                }
            }
        }

        public Brush Fill
        {
            get { return fill; }
            set 
            {
                if (fill != value)
                {
                    fill = value;
                    NotifyPropertyChanged("Fill");
                }
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        public string WKT
        {
            get
            {
                if (_data != null)
                    return new string(_data.STAsText().Value);
                else
                    return string.Empty;
            }
        }

        public string Area
        {
            get
            {
                if (_data != null)
                    return _data.STArea().Value.ToString("F3");
                return null;
            }
        }

        public string Length
        {
            get
            {
                if (_data != null)
                    return _data.STLength().Value.ToString("F3");
                return null;
            }
        }

        public bool IsValid
        {
            get
            {
                if (_data != null)
                    return _data.STIsValid().Value;
                return false;
            }
        }

        public Geometry Geometry
        {
            get
            {
                if (_data == null)
                    return Geometry.Empty;

                return Decode(_data);
            }
        }

        #endregion

        #region Helper methods

        private Geometry Decode(SqlGeometry g)
        {
            PathGeometry result = new PathGeometry();

            switch (g.STGeometryType().Value.ToLower())
            {
                case "point":
                    PathFigure pointFig = new PathFigure();
                    pointFig.StartPoint = new Point(g.STX.Value - 2, g.STY.Value - 2);
                    LineSegment pointLs = new LineSegment(new Point(g.STX.Value + 2, g.STY.Value + 2), true);
                    pointFig.Segments.Add(pointLs);
                    result.Figures.Add(pointFig);

                    pointFig = new PathFigure();
                    pointFig.StartPoint = new Point(g.STX.Value - 2, g.STY.Value + 2);
                    pointLs = new LineSegment(new Point(g.STX.Value + 2, g.STY.Value - 2), true);
                    pointFig.Segments.Add(pointLs);
                    result.Figures.Add(pointFig);

                    return result;
                case "polygon":
                    string cmd = new string(g.STAsText().Value).Trim().Substring(8);
                    string[] polyArray = (cmd.Substring(1, cmd.Length - 2) + ", ").Split('(');

                    var polys = from s in polyArray
                                where s.Length > 0
                                select s.Trim().Substring(0, s.Length - 3);

                    PathFigure fig;
                    foreach (var item in polys)
                    {
                        fig = new PathFigure();
                        var polyPoints = from p in item.Split(',')
                                         select p.Trim().Replace(" ", ",");
                        fig.StartPoint = Point.Parse(polyPoints.ElementAt(0));
                        for (int i = 1; i < polyPoints.Count(); i++)
                        {
                            LineSegment ls = new LineSegment(Point.Parse(polyPoints.ElementAt(i)), true);
                            fig.Segments.Add(ls);
                        }

                        result.Figures.Add(fig);
                    }

                    return result;
                case "linestring":
                    PathFigure lsfig = new PathFigure();
                    lsfig.StartPoint = new Point(g.STPointN(1).STX.Value, g.STPointN(1).STY.Value);

                    for (int i = 1; i <= g.STNumPoints(); i++)
                    {
                        LineSegment ls = new LineSegment();
                        ls.Point = new Point(g.STPointN(i).STX.Value, g.STPointN(i).STY.Value);
                        lsfig.Segments.Add(ls);
                    }
                    result.Figures.Add(lsfig);

                    return result;
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                    GeometryGroup mpG = new GeometryGroup();

                    for (int i = 1; i <= g.STNumGeometries().Value; i++)
                        mpG.Children.Add(Decode(g.STGeometryN(i)));

                    return mpG;
                default:
                    return Geometry.Empty;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    #region GeometryCollection class

    public class GeometryCollection
        : DependencyObject
    {
        #region DPs

        public static readonly DependencyPropertyKey BoundingBoxPropertyKey = DependencyProperty.RegisterReadOnly("BoundingBox", typeof(Rect), typeof(GeometryCollection), new PropertyMetadata());
        public static readonly DependencyProperty BoundingBoxProperty;

        public static readonly DependencyPropertyKey TranslateXPropertyKey = DependencyProperty.RegisterReadOnly("TranslateX", typeof(Double), typeof(GeometryCollection), new PropertyMetadata());
        public static readonly DependencyProperty TranslateXProperty;
        public static readonly DependencyPropertyKey TranslateYPropertyKey = DependencyProperty.RegisterReadOnly("TranslateY", typeof(Double), typeof(GeometryCollection), new PropertyMetadata());
        public static readonly DependencyProperty TranslateYProperty;

        public Rect BoundingBox
        {
            get { return (Rect)GetValue(BoundingBoxProperty); }
        }

        public Double TranslateX
        {
            get { return (Double)GetValue(TranslateXProperty); }
        }

        public Double TranslateY
        {
            get { return (Double)GetValue(TranslateYProperty); }
        }

        #endregion

        #region Constructors

        static GeometryCollection()
        {
            BoundingBoxProperty = BoundingBoxPropertyKey.DependencyProperty;
            TranslateXProperty = TranslateXPropertyKey.DependencyProperty;
            TranslateYProperty = TranslateYPropertyKey.DependencyProperty;
        }

        public GeometryCollection(ObservableCollection<GeometryInfo> geometries)
        {
            this.geometries = geometries;
            this.geometries.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(GeometriesCollectionChanged);
        }

        public GeometryCollection() : this(new ObservableCollection<GeometryInfo>()) { }

        #endregion

        #region Properties

        private ObservableCollection<GeometryInfo> geometries;
        public ObservableCollection<GeometryInfo> Geometries
        {
            get { return geometries; }
        }

        #endregion

        #region Events

        void GeometriesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Geometries.Count > 0)
            {
                var minX = (from p in Geometries
                            select p.Data.STEnvelope().STPointN(1).STX.Value).Min();
                var maxX = (from p in Geometries
                            select p.Data.STEnvelope().STPointN(2).STX.Value).Max();
                var minY = (from p in Geometries
                            select p.Data.STEnvelope().STPointN(1).STY.Value).Min();
                var maxY = (from p in Geometries
                            select p.Data.STEnvelope().STPointN(4).STY.Value).Max();

                SetValue(BoundingBoxPropertyKey, new Rect(minX, minY, maxX - minX, maxY - minY));
                SetValue(TranslateXPropertyKey, -BoundingBox.TopLeft.X);
                SetValue(TranslateYPropertyKey, -BoundingBox.TopLeft.Y);
            }
            else
            {
                SetValue(BoundingBoxPropertyKey, new Rect(0, 0, 0, 0));
                SetValue(TranslateXPropertyKey, 0.0);
                SetValue(TranslateYPropertyKey, 0.0);
            }
        }

        #endregion
    }

    #endregion
}
