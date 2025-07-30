using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manage_Inventoty.Classes;

namespace Manage_Inventoty.Forms
{
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
            DisplayData();
        }

        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");

        public void refresh()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)Refresh);
                return;
            }
            DisplayData();
        }
        public void DisplayData()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            Data_customer data = new Data_customer();
            List<Data_customer> list = data.list_Customer();
            Dtg_customer.DataSource = list;
            Dtg_customer.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_customer.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }

        private void Dtg_customer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
