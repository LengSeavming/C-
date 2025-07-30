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

namespace Manage_Inventoty.Forms
{
    public partial class frmRegisterAccount : Form
    {
        public frmRegisterAccount()
        {
            InitializeComponent();
        }


        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");
        private void showPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = showPassword.Checked;
            txtComfirm.UseSystemPasswordChar= showPassword.Checked;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            frmLogin frm=new frmLogin();
            frm.Show();
            this.Hide();
        }

        public bool Empty()
        {
            if (txtusername.Text == "" || txtpassword.Text==""||txtComfirm.Text=="")
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
            txtusername.Text = "";
            txtpassword.Text = "";
            txtComfirm.Text = "";
        }
        private void btnsignUp_Click(object sender, EventArgs e)
        {
            try
            {
                if(Empty())
                {
                    MessageBox.Show("Input Data please...!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtpassword.Text!=txtComfirm.Text)
                {
                    MessageBox.Show("Incorrect password...!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtpassword.Text.Length<8)
                {
                    MessageBox.Show("Password use 8 Charator...!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into tbl_users(username,password,date)values(@n,@p,@d)", con);
                    DateTime today = DateTime.Now;
                    cmd.Parameters.AddWithValue("@n", txtusername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtpassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@d", today);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Create User successfully...", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearData();
                    frmLogin frm=new frmLogin();
                    frm.Show();
                    this.Hide();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error",ex.Message);
            }
            finally
            {
                con.Close();
            } 
        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
