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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace SpatialWpf
{
    /// <summary>
    /// Interaction logic for ConnectionDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : Window
    {
        public ConnectionDialog()
        {
            InitializeComponent();
        }

        private SqlConnection conn;

        private void txtServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtServer.Text.Trim().Length > 0)
            {
                gbLogOn.IsEnabled = true;
                gbConnectDb.IsEnabled = true;
            }
            else
            {
                gbLogOn.IsEnabled = false;
                gbConnectDb.IsEnabled = false;
                cmbDatabase.Text = "";

                btnOK.IsEnabled = false;
            }
        }

        private void cmbDatabase_DropDownOpened(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(string.Format("Server={0};{1};",
                txtServer.Text,
                rdSQLAuth.IsChecked.Value ? string.Format("Uid={0};Pwd={1}", 
                    txtUsername.Text, txtPassword.Password) : "Integrated Security=True"));
            cmbDatabase.Items.Clear();

            try
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "sp_databases";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            cmbDatabase.Items.Add(dr.GetString(0));
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(string.Format("SQL Server responded with an error:\n\n{0}", ex.Message),
                    "SQL Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rdSQLAuth_Checked(object sender, RoutedEventArgs e)
        {
            grdSQLAuth.IsEnabled = true;
        }

        private void rdSQLAuth_Unchecked(object sender, RoutedEventArgs e)
        {
            grdSQLAuth.IsEnabled = false;
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }

        private void rdAttach_Checked(object sender, RoutedEventArgs e)
        {
            grdAttachDb.IsEnabled = true;
        }

        private void rdAttach_Unchecked(object sender, RoutedEventArgs e)
        {
            grdAttachDb.IsEnabled = false;
            txtFilename.Text = string.Empty;
            txtLogicalName.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void cmbDatabase_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbDatabase.Text.Trim() != string.Empty)
                btnOK.IsEnabled = true;
            else
                btnOK.IsEnabled = false;
        }

        public SqlConnection Connection
        {
            get { return conn; }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(string.Format("Server={0};{1};Initial Catalog={2}",
                txtServer.Text,
                rdSQLAuth.IsChecked.Value ? string.Format("Uid={0};Pwd={1}",
                    txtUsername.Text, txtPassword.Password) : "Integrated Security=True",
                cmbDatabase.Text));

            try
            {
                conn.Open();
                conn.Close();

                this.conn = conn;
                this.DialogResult = true;
                this.Close();
            }
            catch (SqlException ex)
            {
                conn = null;
                MessageBox.Show(string.Format("SQL Server responded with an error:\n\n{0}", ex.Message),
                    "SQL Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
