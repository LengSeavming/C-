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
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
            DataSaleToday();
            Totalproduct();
            Data_SaleToday();
            DataTotal_sale();
            DataUser(); 
        }

        SqlConnection con = new SqlConnection("Data Source=techcode;Initial Catalog=Management_Inventory;User ID=heang;Password=HeangPro168;Pooling=False;Encrypt=False;");

        private void frmDashboard_Load(object sender, EventArgs e)
        {

        }
        public void refresh()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)Refresh);
                return;
            }
            DataSaleToday();
            Totalproduct();
            Data_SaleToday();
            DataTotal_sale();
            DataUser();

        }
        public void DataSaleToday()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            Data_customer data = new Data_customer();
            List<Data_customer> list = data.list_sale();
            Dtg_dashboard.DataSource = list;
            Dtg_dashboard.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_dashboard.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }

        public void DataUser()
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select count(id) from tbl_users", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int count = Convert.ToInt32(dr[0]);
                    lblUser.Text = count.ToString()+" User";

                }
                dr.Dispose();
            }
        }
        public void Totalproduct()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from tbl_products", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0)
            {
               lblProducts.Text = Convert.ToString(count) + " Item";
            }
            else
            {
                lblProducts.Text = "0";
            }
            con.Close();
        }

        public void Data_SaleToday()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT SUM(CAST(total AS decimal(10, 2))) FROM tbl_order WHERE CAST(date AS DATE) = @d";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@d", DateTime.Today); // use DateTime, not string

                    object result = cmd.ExecuteScalar();
                    decimal revenue = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                   lblsalToday.Text = "$" + revenue.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total revenue: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void DataTotal_sale()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();

                string query = "SELECT SUM(CAST(total AS decimal(10, 2))) FROM tbl_order ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@d", DateTime.Today); // use DateTime, not string

                    object result = cmd.ExecuteScalar();
                    decimal revenue = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                    lblTotalSale.Text = "$" + revenue.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total revenue: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void Dtg_dashboard_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
