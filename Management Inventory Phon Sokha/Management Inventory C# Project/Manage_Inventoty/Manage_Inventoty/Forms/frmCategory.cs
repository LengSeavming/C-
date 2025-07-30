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
using System.Xml.Linq;
using Manage_Inventoty.Classes;

namespace Manage_Inventoty.Forms
{
    public partial class frmCategory : Form
    {
        public frmCategory()
        {
            InitializeComponent();
            DisplayData();
            txtserch.Text = "Search Name OR ID here....!";
            txtserch.ForeColor = Color.Gray;
        }

        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");

        public void refresh()
        {
            if(InvokeRequired)
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
            Data_category data = new Data_category();
            List<Data_category> list = data.Data_showCategory();
            Dtg_category.DataSource = list;
            Dtg_category.ColumnHeadersDefaultCellStyle.Font=new Font("Cambria",15,FontStyle.Bold);
            Dtg_category.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            //FormAddCategroy frm=new FormAddCategroy();
            //frm.Show();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            DisplayData();
           
        }

        
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_category WHERE categoryId = @id OR categoryName like @c", con);
                cmd.Parameters.AddWithValue("@c","%"+txtserch.Text.Trim()+"%");

                if (int.TryParse(txtserch.Text.Trim(), out int id))
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
                    MessageBox.Show("No records found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dtg_category.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }
            



        public void ClearData()
        {
            txtid.Text = "";
            txtName.Text = "";
            cmstatus.SelectedIndex = -1;
        }
        public bool Empty()
        {
            if (txtid.Text == "" || txtName.Text == "" || cmstatus.SelectedIndex == -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Empty())
                {
                    MessageBox.Show("Input Data please....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into tbl_category values(@n,@id,@s,@d)", con);
                    DateTime today = DateTime.Now;

                    cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", txtid.Text.Trim());
                    cmd.Parameters.AddWithValue("@s", cmstatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@d", today);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Saved successfully....", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayData();
                    ClearData();

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

        //private int getid
        int getid=0;
        private void Dtg_category_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (Dtg_category.Rows.Count > 0)
            {
                int i = e.RowIndex;

                   DataGridViewRow row = Dtg_category.Rows[i];

                    // Null checks to avoid exceptions
                    getid =(int) row.Cells[0].Value;
                    txtid.Text = row.Cells[2]?.Value?.ToString() ?? string.Empty;
                    txtName.Text = row.Cells[1]?.Value?.ToString() ?? string.Empty;
                    cmstatus.Text = row.Cells[3]?.Value?.ToString() ?? string.Empty;
                
            }

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if(Empty())
            {
                MessageBox.Show("Input Data ....", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult re = MessageBox.Show("Are you sure you want to Delete " + txtName.Text + " this data? ", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (re == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM tbl_category WHERE id = @id", con);
                        cmd.Parameters.AddWithValue("@id",getid);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData(); ;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
     
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (Empty())
                {
                    MessageBox.Show("Input Data please....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult re = MessageBox.Show("Are you sure you want to Update " + txtName.Text + " this data? ", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (re == DialogResult.Yes)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Update  tbl_category set categoryName=@n,categoryId=@i,status=@s,date=@d where id=@id", con);
                        DateTime today = DateTime.Now;

                        cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@i", txtid.Text.Trim());
                        cmd.Parameters.AddWithValue("@s", cmstatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@d", today);
                        cmd.Parameters.AddWithValue("@id", getid);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Update successfully....", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData(); 
                    }
                    

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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void txtserch_Enter(object sender, EventArgs e)
        {
            if(txtserch.Text =="Search Name OR ID here....!")
            {
                txtserch.Text = "";
                txtserch.ForeColor = Color.Black;
            }
        }

        private void txtserch_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtserch.Text))
            {
                txtserch.Text = "Search Name OR ID here....!";
                txtserch.ForeColor = Color.Gray;

            }
        }
    }
}
