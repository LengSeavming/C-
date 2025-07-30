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
    public partial class frmproducts : Form
    {
        public frmproducts()
        {
            InitializeComponent();
            DisplayData();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtserch.Text = "Search ID here...!";
            txtserch.ForeColor = Color.Gray;

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
            Data_products data = new Data_products();
            List<Data_products> list = data.Data_showProduct();
            Dtg_product.DataSource = list;
            Dtg_product.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_product.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }

        
        public void getNameCategory()
        {

            SqlCommand cmd = new SqlCommand("select * from tbl_category", con);
            SqlDataReader dr;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("categoryName");
                    dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    CmCategory.ValueMember = "categoryName";
                    CmCategory.DataSource = dt;
                    con.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        string pname;
        public void getIDcategory()
        {
            try
            {
                if (CmCategory.SelectedValue == null)
                {
                    MessageBox.Show("Please select a Category Name.");
                    return;
                }

               
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT categoryId FROM tbl_category WHERE categoryName = @n", con);
                    cmd.Parameters.AddWithValue("@n",CmCategory.SelectedValue.ToString());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        string pname = dt.Rows[0]["categoryId"].ToString();
                        txtCategoryID.Text = pname;
                    }
                    else
                    {
                        txtCategoryID.Text = "";
                        MessageBox.Show("No CategoryID found for the selected Name.");
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally { con.Close(); }
        }
        public bool Empty()
        {
            if(txtID.Text==""||txtName.Text==""||txtprice.Text==""||CmCategory.SelectedIndex==-1||Cmstatus.SelectedIndex==-1||txtStock.Text=="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearData()
        {
            txtID.Text = "";
            txtName.Text = "";
            CmCategory.SelectedIndex = -1;
            txtprice.Text = "";
            txtStock.Text = "";
            Cmstatus.SelectedIndex = -1;
            txtCategoryID.Text = "";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cd = new SqlCommand("Select * from tbl_products where id=@id", con);
                cd.Parameters.AddWithValue("@id",txtID.Text.Trim());
                SqlDataAdapter da=new SqlDataAdapter(cd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    MessageBox.Show("ID " + txtID.Text.ToUpper()+ "  Exitting already..!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if(Empty())
                    {
                        MessageBox.Show("Input Data Please!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Insert into tbl_products(id,name,category,stock,price,status,date,CategoryId)values(@id,@n,@c,@s,@p,@st,@date,@cid)", con);
                        DateTime today = DateTime.Now;
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@c", CmCategory.Text);
                        cmd.Parameters.AddWithValue("@s", txtStock.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", txtprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@st", Cmstatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@cid", txtCategoryID.Text.Trim());
                        cmd.Parameters.AddWithValue("@date", today);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Add successfully...!", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData();
                    }
                    
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Dtg_product_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(Dtg_product.Rows.Count>0)
            {
                int i = e.RowIndex;
                DataGridViewRow row = Dtg_product.Rows[i];

                // Set values to form controls
                txtID.Text = row.Cells[0].Value?.ToString() ?? "";
                txtName.Text = row.Cells[1].Value?.ToString() ?? "";
                CmCategory.Text = row.Cells["category"].Value?.ToString() ?? "";

                txtStock.Text = row.Cells[3].Value?.ToString() ?? "";
                txtprice.Text = row.Cells[4].Value?.ToString() ?? "";
                txtCategoryID.Text = row.Cells[5].Value?.ToString() ?? "";
                Cmstatus.Text = row.Cells[6].Value?.ToString() ?? "";

                // Set date to DateTimePicker
                if (row.Cells[7].Value != null && DateTime.TryParse(row.Cells[7].Value.ToString(), out DateTime selectedDate))
                {
                    date_product.Value = selectedDate;
                }
                else
                {
                    date_product.Value = DateTime.Now;
                }


                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                btnAdd.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(Empty())
                {
                    MessageBox.Show("Select data for Update..!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult re = MessageBox.Show("Are you sure you want to Update Data..?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult.Yes == re)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE tbl_products SET  name=@n,category=@c, stock=@stock, price=@p, status=@s, date=@date, CategoryId=@cid WHERE id=@id", con);
                        DateTime today = DateTime.Now;

                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@c", CmCategory.Text.ToString());
                        cmd.Parameters.AddWithValue("@stock", txtStock.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", txtprice.Text.Trim());
                        cmd.Parameters.AddWithValue("@s", Cmstatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@cid",txtCategoryID.Text.Trim());
                        cmd.Parameters.AddWithValue("@date", today);

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Updated successfully...!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData();
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = false;
                        btnUpdate.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error",ex.Message);
            }
            finally { con.Close(); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Empty())
                {
                    MessageBox.Show("Select data for Delete..!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult re = MessageBox.Show("Are you sure you want to delete Data..?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DialogResult.Yes == re)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Delete tbl_products where id=@id", con);
    
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim().ToUpper());
                      
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Delete  successfully...!", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData();
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = false;
                        btnUpdate.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
            finally { con.Close(); }
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            ClearData();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = true;
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtserch.Text))
                {
                    MessageBox.Show("Input Category ID to search!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //if (!int.TryParse(txtserch.Text.Trim(), out int categoryId))
                //{
                //    MessageBox.Show("Category ID must be a numeric value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_products WHERE name like @n OR categoryId = @id OR id = @idpro or category like @c", con);
               
                cmd.Parameters.AddWithValue("@idpro", txtserch.Text.Trim());
                cmd.Parameters.AddWithValue("@n","%"+txtserch.Text.Trim()+"%");
                cmd.Parameters.AddWithValue("@c", "%" + txtserch.Text.Trim() + "%");
                if (int.TryParse(txtserch.Text.Trim(), out int id))
                {
                    cmd.Parameters.AddWithValue("@id",id);
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
                    Dtg_product.DataSource = dt;
                }
                else
                {
                    Dtg_product.DataSource = null;
                    MessageBox.Show("No products found for the given Category ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }

        private void CmCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            getIDcategory();
        }

        private void frmproducts_Load(object sender, EventArgs e)
        {
           getNameCategory();
        }

        private void txtserch_Enter(object sender, EventArgs e)
        {
            if(txtserch.Text=="Search ID here...!")
            {
                txtserch.Text = "";
                txtserch.ForeColor=Color.Black;
            }
        }

        private void txtserch_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtserch.Text))
            {
                txtserch.Text = "Search ID here...!";
                txtserch.ForeColor = Color.Gray;
                DisplayData();
            }
        }

        private void Dtg_product_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("$"+"0.00") ;
                    e.FormattingApplied = true;
                }
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtserch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
