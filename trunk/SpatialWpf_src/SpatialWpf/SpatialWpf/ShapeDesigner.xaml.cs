using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Types;
using System.Collections.ObjectModel;

namespace SpatialWpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ShapeDesigner : Window
    {
        SqlConnection conn;
        GeometryCollection geoms;
        ObservableCollection<GeometryInfo> lines;

        public ShapeDesigner()
        {
            InitializeComponent();

            geoms = (GeometryCollection)this.FindResource("GeometryCollection");
            lines = geoms.Geometries;
        }

        private void btnAddSQL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = txtSQL.Text;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();

                            int geomCol = 0; int idCol = -1;
                            if (rdr.FieldCount > 2)
                            {
                                txtStatus.Text = "Error: the result cannot contain more than two columns.";
                                return;
                            }
                            if (rdr.FieldCount == 2)
                            {
                                geomCol = 1;
                                idCol = 0;
                            }

                            if (!(rdr[geomCol] is SqlGeometry))
                            {
                                txtStatus.Text = "Error: the result set is not a geometry collection.";
                                return;
                            }

                            int count = 0;
                            do
                            {
                                if (rdr[geomCol] != DBNull.Value)
                                {
                                    GeometryInfo gi = new GeometryInfo();
                                    gi.Id = (idCol == 0 ? rdr[idCol].ToString() : count.ToString());
                                    gi.Data = (SqlGeometry)rdr[geomCol];
                                    lines.Add(gi);
                                    count++;
                                }
                            } while (rdr.Read());
                            txtStatus.Text = string.Format("Fetched {0} records.", count);
                        }
                    }
                    catch (SqlException ex)
                    {
                        txtStatus.Text = string.Format("Could not execute SQL. The error returned was: {0}", ex.Message);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lines.Clear();
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            this.conn.Close();
            this.conn = null;
            btnConnect.IsEnabled = true;
            btnDisconnect.IsEnabled = false;
            btnAddSQL.IsEnabled = false;

            stConnectionStatus.Text = "Not connected";
        }

        private void txtOGC_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool bAddValid = true;
            btnAdd.IsEnabled = true;
            btnAddValid.IsEnabled = false;
            StringBuilder errors = new StringBuilder();

            if (txtOGC.Text.Length > 0)
            {
                foreach (string geomText in txtOGC.Text.Split('\n'))
                {
                    if (geomText.Length > 0)
                    {
                        try
                        {
                            SqlGeometry g = SqlGeometry.Parse(geomText);
                            if (g.STIsValid())
                            {

                                btnAdd.IsEnabled = btnAdd.IsEnabled & true;
                            }
                            else
                            {
                                errors.AppendLine(String.Format("Parsed {0} Invalid - Valid = {1}", geomText, g.MakeValid().ToString()));
                                bAddValid = bAddValid & (bool)g.MakeValid().STIsValid();

                                btnAdd.IsEnabled = false;
                            }

                        }
                        catch (Exception ex)
                        {
                            errors.AppendLine(String.Format("Parsed {0} Invalid - Error = {1}", geomText, ex.Message));
                            btnAdd.IsEnabled = false;
                        }
                    }
                }

                if (errors.Length > 0)
                {
                    txtValidation.Text = errors.ToString();
                    if (bAddValid) btnAddValid.IsEnabled = true;
                }
                else
                    txtValidation.Text = "Parsed - Valid";
            }
            else
                txtValidation.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (string geomText in txtOGC.Text.Split('\n'))
            {
                if (geomText.Length > 0)
                {
                    GeometryInfo gi = new GeometryInfo();
                    gi.Id = Guid.NewGuid().ToString();
                    gi.Data = SqlGeometry.Parse(txtOGC.Text);
                    lines.Add(gi);
                }
            }
        }

        private void btnAddValid_Click(object sender, RoutedEventArgs e)
        {
            if (txtOGC.Text.Length > 0)
            {
                foreach (string geomText in txtOGC.Text.Split('\n'))
                {
                    if (geomText.Length > 0)
                    {
                        try
                        {
                            SqlGeometry g = SqlGeometry.Parse(geomText);

                            if (!g.STIsValid())
                                g = g.MakeValid();

                            GeometryInfo gi = new GeometryInfo();
                            gi.Id = Guid.NewGuid().ToString();
                            gi.Data = g;
                            lines.Add(gi);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        private void dGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems)
                ((GeometryInfo)item).Fill = Brushes.Green;

            foreach (var item in e.AddedItems)
                ((GeometryInfo)item).Fill = Brushes.Blue;
        }

        Point scrollStartPoint;

        private void masterCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (masterCanvas.IsMouseCaptured)
            {
                Point currentPoint = e.GetPosition(masterCanvas);
                Point delta = new Point(scrollStartPoint.X - currentPoint.X, scrollStartPoint.Y - currentPoint.Y);

                TranslateTransform t = (TranslateTransform)drawingCanvas.RenderTransform;
                Point org = new Point(t.X, t.Y);
                drawingCanvas.RenderTransform = new TranslateTransform(org.X - delta.X, org.Y - delta.Y);

                scrollStartPoint = e.GetPosition(masterCanvas);
            }
        }

        private void masterCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            masterCanvas.CaptureMouse();
            Cursor = Cursors.ScrollAll;

            scrollStartPoint = e.GetPosition(masterCanvas);
        }

        private void masterCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (masterCanvas.IsMouseCaptured)
            {
                masterCanvas.ReleaseMouseCapture();
                Cursor = Cursors.Arrow;
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectionDialog dlg = new ConnectionDialog();
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.conn = dlg.Connection;
                btnConnect.IsEnabled = false;
                btnDisconnect.IsEnabled = true;
                btnAddSQL.IsEnabled = true;

                stConnectionStatus.Text = string.Format("Connected - {0}", this.conn.Database);
            }
        }
    }
}
