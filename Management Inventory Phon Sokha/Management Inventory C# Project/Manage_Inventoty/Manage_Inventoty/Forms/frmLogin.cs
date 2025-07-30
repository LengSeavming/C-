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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            Formlogin=this;
        }

        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");

        public static frmLogin Formlogin=new frmLogin();
        public string username { get;set; }

        private void showPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = showPassword.Checked;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            frmRegisterAccount frm=new frmRegisterAccount();
            frm.Show();
            this.Hide();
        }

        public bool Empty()
        {
            if(txtusername.Text==""||txtpassword.Text=="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnLgin_Click(object sender, EventArgs e)
        {
            try
            {
                if(Empty())
                {
                    MessageBox.Show("Input Data please...!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    username = txtusername.Text;
                    SqlCommand cmd = new SqlCommand("select * from tbl_users where username=@n and password=@p", con);
                    cmd.Parameters.AddWithValue("@n", txtusername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtpassword.Text.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        frmManagement frm = new frmManagement();
                        frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtpassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
