namespace SqlSpatial
{
    using SharpGIS.Gis;
    using SharpGIS.Gis.Converters;
    using SharpGIS.Gis.Geometries;
    using SharpGis.WpfMap;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Xml;

    public class GeometryVisualizer : UserControl, IComponentConnector, IStyleConnector
    {
        private bool _contentLoaded;
        internal Canvas AttributeCanvas;
        internal TextBlock AttributeTextBox;
        internal Image BackgroundWorldMap;
        private static ColorInterpolater colIntPol = ColorInterpolater.Rainbow5;
        internal Grid LayoutGrid;
        private Map map;
        internal ScaleTransform mapCanvasScale;
        internal TranslateTransform mapCanvasTranslate;
        private bool mapIsDragging;
        internal ItemsControl MapViewer;
        internal Canvas MasterCanvas;
        private Point mouseStartPos;
        internal Storyboard panMap;
        internal SplineDoubleKeyFrame translateMapX;
        internal SplineDoubleKeyFrame translateMapY;
        internal ScaleTransform worldmapScale;
        internal TranslateTransform worldmapTranslate;

        public GeometryVisualizer()
        {
            this.InitializeComponent();
            base.SizeChanged += new SizeChangedEventHandler(this.GeometryVisualizer_SizeChanged);
        }

        public void Clear()
        {
            this.MasterCanvas.DataContext = null;
            this.BackgroundWorldMap.Visibility = Visibility.Hidden;
            this.MasterCanvas.UpdateLayout();
        }

        private string ConvertToColor(object o, string defaultColor)
        {
            if (o is string)
            {
                return (o as string);
            }
            if (((o is long) || (o is int)) || (o is short))
            {
                long num = (long) o;
                float num2 = ((float) num) / 1.677722E+07f;
                return this.ConvertToColor(num2, defaultColor);
            }
            if (o is float)
            {
                float pos = (float) o;
                if (pos < 0f)
                {
                    return "Transparent";
                }
                Color color = colIntPol.GetColor(pos);
                return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { color.A, color.R, color.G, color.B });
            }
            if (o is double)
            {
                return this.ConvertToColor((float) ((double) o), defaultColor);
            }
            if (o is decimal)
            {
                return this.ConvertToColor((float) ((decimal) o), defaultColor);
            }
            return defaultColor;
        }

        private void GeometryVisualizer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.map != null)
            {
                this.map.ScaleTo(new Size(this.MasterCanvas.ActualWidth, this.MasterCanvas.ActualHeight));
            }
        }

        private ContextMenu GetContextMenu(Geometry geom, Dictionary<string, object> attributes)
        {
            ContextMenu menu = new ContextMenu();
            menu.Opacity = 0.85;
            MenuItem newItem = new MenuItem();
            newItem.Header = "View Well-Known Text";
            newItem.Click += delegate (object sender, RoutedEventArgs e) {
                this.ShowWKT(geom);
            };
            MenuItem item2 = new MenuItem();
            item2.Header = "View GML";
            item2.Click += delegate (object sender, RoutedEventArgs e) {
                this.ShowGML(geom);
            };
            MenuItem item3 = new MenuItem();
            item3.Header = "View Attributes";
            item3.Click += delegate (object sender, RoutedEventArgs e) {
                this.ShowAttributes(attributes);
            };
            menu.Items.Add(newItem);
            menu.Items.Add(item2);
            menu.Items.Add(item3);
            return menu;
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/SqlSpatial;component/geometryvisualizer.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void LoadBackgroundMap(string backgroundImageFile)
        {
            Stream manifestResourceStream;
            if ((this.BackgroundWorldMap.Source is BitmapImage) && ((this.BackgroundWorldMap.Source as BitmapImage).StreamSource != null))
            {
                (this.BackgroundWorldMap.Source as BitmapImage).StreamSource.Close();
            }
            this.BackgroundWorldMap.Source = null;
            if (string.IsNullOrEmpty(backgroundImageFile) || !File.Exists(backgroundImageFile))
            {
                manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SqlSpatial.worldmap.png");
            }
            else
            {
                manifestResourceStream = File.OpenRead(backgroundImageFile);
            }
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = manifestResourceStream;
            image.EndInit();
            this.BackgroundWorldMap.Source = image;
            this.BackgroundWorldMap.Height = this.MasterCanvas.ActualHeight;
            this.worldmapTranslate.Y = (this.MasterCanvas.ActualHeight - 360.0) / 2.0;
            this.BackgroundWorldMap.Visibility = Visibility.Visible;
        }

        private void MasterCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.mapIsDragging = false;
        }

        private void MasterCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mouseStartPos = e.GetPosition(this.LayoutGrid);
            this.mapIsDragging = true;
        }

        private void MasterCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.mapIsDragging = false;
        }

        private void MasterCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.mapIsDragging)
            {
                Point position = e.GetPosition(this.LayoutGrid);
                double num = position.X - this.mouseStartPos.X;
                double num2 = position.Y - this.mouseStartPos.Y;
                this.mapCanvasTranslate.X += num / this.mapCanvasScale.ScaleX;
                this.mapCanvasTranslate.Y += num2 / this.mapCanvasScale.ScaleY;
                this.mouseStartPos = position;
            }
        }

        private void Polygon_MouseEnter(object sender, MouseEventArgs e)
        {
            Shape shape = (Shape) sender;
            MapRegion region = ((MapGeometry) shape.DataContext).Region;
            region.IsSelected = true;
            StringBuilder builder = new StringBuilder();
            if (region.Attributes != null)
            {
                foreach (string str in region.Attributes.Keys)
                {
                    builder.AppendFormat("{0}: {1}\n", str, region.Attributes[str]);
                }
            }
            this.AttributeTextBox.Text = builder.ToString();
            this.AttributeTextBox.Visibility = string.IsNullOrEmpty(this.AttributeTextBox.Text) ? Visibility.Hidden : Visibility.Visible;
            if (shape.ContextMenu == null)
            {
                shape.ContextMenu = this.GetContextMenu(region.Geometry, region.Attributes);
            }
        }

        private void Polygon_MouseLeave(object sender, MouseEventArgs e)
        {
            ((MapGeometry) ((Shape) sender).DataContext).Region.IsSelected = false;
            this.AttributeTextBox.Text = "";
            this.AttributeTextBox.Visibility = Visibility.Hidden;
        }

        private void ShowAttributes(Dictionary<string, object> attributes)
        {
            StringBuilder builder = new StringBuilder();
            if (attributes != null)
            {
                foreach (string str in attributes.Keys)
                {
                    builder.AppendFormat("{0}: \t{1}{2}", str, attributes[str], Environment.NewLine);
                }
            }
            new frmDisplayText(builder.ToString(), false).Show();
        }

        public void ShowBlankMap(string backgroundImageFile)
        {
            this.map = new Map();
            this.map.BoundingBox = new Rect(-180.0, -90.0, 360.0, 180.0);
            this.map.ScaleTo(new Size(this.MasterCanvas.ActualWidth, this.MasterCanvas.ActualHeight));
            this.LoadBackgroundMap(backgroundImageFile);
            this.MasterCanvas.DataContext = this.map;
        }

        private void ShowGML(Geometry geom)
        {
            new StringBuilder();
            StringWriter w = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(w);
            writer.Formatting = Formatting.Indented;
            new Gml().Write(geom, writer);
            writer.Close();
            new frmDisplayText(w.ToString(), false).Show();
        }

        public void ShowMap(IEnumerable<Feature> features, bool isGeographic, bool showmap, string backgroundImageFile)
        {
            this.map = new Map();
            Envelope envelope = null;
            foreach (Feature feature in features)
            {
                Geometry g = feature.Geometry;
                g.STAsGML();
                MapRegion item = new MapRegion(this.map);
                item.Geometry = g;
                item.Attributes = feature.Attributes;
                double result = 1.0;
                if (feature.Attributes.Keys.Contains<string>("FillColor"))
                {
                    item.FillColor = this.ConvertToColor(feature.Attributes["FillColor"], "White");
                }
                else
                {
                    item.FillColor = "White";
                }
                if (feature.Attributes.Keys.Contains<string>("LineColor"))
                {
                    item.LineColor = this.ConvertToColor(feature.Attributes["LineColor"], "Black");
                }
                else
                {
                    item.LineColor = "Black";
                }
                if (feature.Attributes.Keys.Contains<string>("LineThickness"))
                {
                    double num2 = 1.0;
                    double.TryParse(feature.Attributes["LineThickness"].ToString(), out num2);
                    item.LineThickness = num2;
                }
                if (feature.Attributes.Keys.Contains<string>("PointSize") && double.TryParse(feature.Attributes["PointSize"].ToString(), out result))
                {
                    result *= 0.5;
                }
                if (g is Point)
                {
                    Point point = g as Point;
                    MapPath path = new MapPath();
                    path.PathData = string.Format("M {2},{1} L {3},{1} M {0},{4} L {0} {5}", new object[] { point.X, point.Y, point.X - result, point.X + result, point.Y - result, point.Y + result });
                    item.Geometries.Add(path);
                }
                if (g is LineString)
                {
                    LineString line = g as LineString;
                    if (isGeographic)
                    {
                        MultiLineString str2 = line.SphericDensify(1.0);
                        foreach (LineString str3 in str2.Geometries)
                        {
                            MapPath path2 = new MapPath();
                            path2.PathData = str3.ToWpfPathString();
                            item.Geometries.Add(path2);
                        }
                        g = str2;
                    }
                    else
                    {
                        MapPath path3 = new MapPath();
                        path3.PathData = line.ToWpfPathString();
                        item.Geometries.Add(path3);
                    }
                }
                else if (g is MultiLineString)
                {
                    MultiLineString str4 = (g as MultiLineString).SphericDensify(1.0);
                    foreach (LineString str5 in str4.Geometries)
                    {
                        MapPath path4 = new MapPath();
                        path4.PathData = str5.ToWpfPathString();
                        item.Geometries.Add(path4);
                    }
                    g = str4;
                }
                else if (g is Polygon)
                {
                    Polygon poly = g as Polygon;
                    MapPath path5 = new MapPath();
                    path5.PathData = poly.ToWpfPathString();
                    item.Geometries.Add(path5);
                }
                else if (g is MultiPolygon)
                {
                    MultiPolygon polygon2 = g as MultiPolygon;
                    foreach (Polygon polygon3 in polygon2.Geometries)
                    {
                        MapPath path6 = new MapPath();
                        path6.PathData = polygon3.ToWpfPathString();
                        item.Geometries.Add(path6);
                    }
                }
                else if (g is GeometryCollection)
                {
                    GeometryCollection geometrys = g as GeometryCollection;
                    foreach (Geometry geometry2 in geometrys.Geometries)
                    {
                        MapPath path7 = new MapPath();
                        if (geometry2 is Polygon)
                        {
                            path7.PathData = (geometry2 as Polygon).ToWpfPathString();
                        }
                        else if (geometry2 is LineString)
                        {
                            path7.PathData = (geometry2 as LineString).ToWpfPathString();
                        }
                        if (!string.IsNullOrEmpty(path7.PathData))
                        {
                            item.Geometries.Add(path7);
                        }
                    }
                }
                Envelope envelope2 = g.GetEnvelope();
                envelope = envelope2.Join(envelope);
                item.BoundingBox = envelope2.ToRect();
                this.map.Regions.Add(item);
            }
            if ((envelope != null) && (this.map.Regions.Count != 0))
            {
                this.map.BoundingBox = new Rect(envelope.MinX, envelope.MinY, envelope.Width, envelope.Height);
                this.map.ScaleTo(new Size(this.MasterCanvas.ActualWidth, this.MasterCanvas.ActualHeight));
                if (((isGeographic && showmap) && ((envelope.MinX > -200.0) && (envelope.MinY > -100.0))) && ((envelope.MaxX < 200.0) && (envelope.MaxY < 100.0)))
                {
                    if (this.BackgroundWorldMap.Visibility == Visibility.Hidden)
                    {
                        this.LoadBackgroundMap(backgroundImageFile);
                    }
                }
                else
                {
                    this.BackgroundWorldMap.Visibility = Visibility.Hidden;
                }
                object obj1 = base.Resources["PolygonTemplate"];
                this.MasterCanvas.DataContext = this.map;
            }
        }

        private void ShowWKT(Geometry geom)
        {
            new frmDisplayText(geom.STAsText(), true).Show();
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 2:
                    this.LayoutGrid = (Grid) target;
                    return;

                case 3:
                    this.MasterCanvas = (Canvas) target;
                    this.MasterCanvas.MouseMove += new MouseEventHandler(this.MasterCanvas_MouseMove);
                    this.MasterCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(this.MasterCanvas_MouseLeftButtonDown);
                    this.MasterCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(this.MasterCanvas_MouseLeftButtonUp);
                    this.MasterCanvas.MouseLeave += new MouseEventHandler(this.MasterCanvas_MouseLeave);
                    return;

                case 4:
                    this.panMap = (Storyboard) target;
                    return;

                case 5:
                    this.translateMapX = (SplineDoubleKeyFrame) target;
                    return;

                case 6:
                    this.translateMapY = (SplineDoubleKeyFrame) target;
                    return;

                case 7:
                    this.mapCanvasTranslate = (TranslateTransform) target;
                    return;

                case 8:
                    this.mapCanvasScale = (ScaleTransform) target;
                    return;

                case 9:
                    this.BackgroundWorldMap = (Image) target;
                    return;

                case 10:
                    this.worldmapTranslate = (TranslateTransform) target;
                    return;

                case 11:
                    this.worldmapScale = (ScaleTransform) target;
                    return;

                case 12:
                    this.MapViewer = (ItemsControl) target;
                    return;

                case 13:
                    this.AttributeCanvas = (Canvas) target;
                    return;

                case 14:
                    this.AttributeTextBox = (TextBlock) target;
                    return;
            }
            this._contentLoaded = true;
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IStyleConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                ((Path) target).MouseEnter += new MouseEventHandler(this.Polygon_MouseEnter);
                ((Path) target).MouseLeave += new MouseEventHandler(this.Polygon_MouseLeave);
            }
        }

        public void ToggleBackgroundMap(bool show, string backgroundImageFile)
        {
            if ((this.BackgroundWorldMap.Visibility == Visibility.Hidden) && show)
            {
                this.LoadBackgroundMap(backgroundImageFile);
            }
            else if ((this.BackgroundWorldMap.Visibility == Visibility.Visible) && !show)
            {
                this.BackgroundWorldMap.Visibility = Visibility.Hidden;
            }
        }
    }
}

