using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CellLoadSharing
{
    public partial class FormCellLoadSharing : Form
    {
        public FormCellLoadSharing()
        {
            InitializeComponent();
        }

        private void btnExecuteCLS_Click(object sender, EventArgs e)
        {  
            ComputeCell cc = new ComputeCell();
            StaticTable.computecell = cc;
            dataGridView1.DataSource = StaticTable.computecell.dtVar;
            MessageBox.Show("ok");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private DataTable FilterDataTable(DataTable dataSource, string matchtypes)
        {
            DataView dv = dataSource.DefaultView;
            dv.RowFilter = "cell_name = '" + matchtypes + "'";
            DataTable newTable1 = dv.ToTable();
            return newTable1;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                MessageBox.Show(e.RowIndex.ToString());

                MessageBox.Show(e.ColumnIndex.ToString());
                //MessageBox.Show(cellname);
                string cellname = dataGridView1[ e.ColumnIndex,e.RowIndex].Value.ToString();
                MessageBox.Show(cellname);
                dataGridView2.DataSource = FilterDataTable(StaticTable.computecell.dtDetail, cellname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormCellLoadSharing_Load(object sender, EventArgs e)
        {
            //DataClasses1DataContext dc = new DataClasses1DataContext();
            //dataGridView1.DataSource = dc.CELLGPRS_0822_1.Take(10);
        }
    }
}
