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
    public partial class frmstock : Form
    {
        public frmstock()
        {
            InitializeComponent();
            DataProducts();
            DataCategory();
            Totalcategory();
            Totalproduct();
            txtSearch_category.Text = "Search Name OR ID here..!";
            txtSearch_category.ForeColor = Color.Gray;

            txtSearch_products.Text = "Search Name OR ID here..!";
            txtSearch_products.ForeColor = Color.Gray;
        }

        private void frmstock_Load(object sender, EventArgs e)
        {
            DataProducts();
            DataCategory();
            Totalcategory();
            Totalproduct();
        }

        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");

        public void refresh()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)Refresh);
                return;
            }
            DataProducts();
            DataCategory();
            Totalcategory();
            Totalproduct();


        }
        public void DataProducts()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            Data_products data = new Data_products();
            List<Data_products> list = data.Data_showProduct();
            Dtg_product.DataSource = list;
            Dtg_product.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_product.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }

        public void DataCategory()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            Data_category data = new Data_category();
            List<Data_category> list = data.Data_showCategory();
            Dtg_category.DataSource = list;
            Dtg_category.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_category.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Totalcategory()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from tbl_category", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            
            if (count > 0)
            {
                lblCategory.Text = Convert.ToString(count)+" Item";
            }
            else
            {
                lblCategory.Text = "0";
            }
            con.Close();
        }

        public void Totalproduct()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from tbl_products", con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count > 0)
            {
                lblProduct.Text = Convert.ToString(count) + " Item";
            }
            else
            {
                lblProduct.Text = "0";
            }
            con.Close();
        }

        private void Dtg_category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtSearch_products.Text == "")
                {
                    MessageBox.Show("Input Data or ID to Saerch...!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
              
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_products WHERE name LIKE @n OR CategoryId = @id OR id=@idpro  OR category like @c", con);

                    cmd.Parameters.AddWithValue("@idpro", txtSearch_products.Text.Trim());
                    cmd.Parameters.AddWithValue("@n", "%" + txtSearch_products.Text.Trim() + "%");
                    cmd.Parameters.AddWithValue("@c", "%" + txtSearch_products.Text.Trim() + "%");
                    if (int.TryParse(txtSearch_products.Text.Trim(), out int id_product))
                    {
                        cmd.Parameters.AddWithValue("@id", id_product);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@id", DBNull.Value);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Dtg_product.DataSource = dt;
                    }
                    else
                    {
                        Dtg_product.DataSource = null;
                        MessageBox.Show("No category found with the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.Close();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
            finally { con.Close(); }
        }

        private void btnSearch_category_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSearch_category.Text))
                {
                    MessageBox.Show("Please enter a Category ID to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_category WHERE categoryId = @id OR categoryName like @c", con);
               
                cmd.Parameters.AddWithValue("@c","%"+ txtSearch_category.Text.Trim()+"%");
                if (int.TryParse(txtSearch_category.Text.Trim(), out int id))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@id",DBNull.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Dtg_category.DataSource = dt;
                }
                else
                {
                    Dtg_category.DataSource = null;
                    MessageBox.Show("No category found with the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

        }

        private void txtSearch_category_Enter(object sender, EventArgs e)
        {
            if (txtSearch_category.Text == "Search Name OR ID here..!")
            {
                txtSearch_category.Text = "";
                txtSearch_category.ForeColor = Color.Black;
            }
        }

        private void txtSearch_category_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch_category.Text))
            {
                txtSearch_category.Text = "Search Name OR ID here..!";
                txtSearch_category.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_products_Enter(object sender, EventArgs e)
        {
            if (txtSearch_products.Text == "Search Name OR ID here..!")
            {
                txtSearch_products.Text = "";
                txtSearch_products.ForeColor = Color.Black;
            }
        }

        private void txtSearch_products_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch_products.Text))
            {
                txtSearch_products.Text = "Search Name OR ID here..!";
                txtSearch_products.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_category_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
