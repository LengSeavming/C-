using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manage_Inventoty.Forms
{
    public partial class frmManagement : Form
    {
        public frmManagement()
        {
            InitializeComponent();
            //getuser();
            frmCategory frmCategory = new frmCategory();
            frmproducts frmproducts = new frmproducts();
            frmstock frmstock=new frmstock();
            frmOrder frm=new frmOrder();
            frmCustomer frmCustomer = new frmCustomer();
            frmDashboard frmDashboard = new frmDashboard();
        }
        //addfrom
        private Forms.frmCategory Formcategory=new frmCategory();
        private Forms.frmproducts Formproducts = new Forms.frmproducts();
        private Forms.frmstock Formstock=new frmstock();
        private Forms.frmOrder Formorder=new frmOrder();
        private Forms.frmCustomer Formcustomer=new frmCustomer();
        private Forms.frmDashboard Formdashboard=new frmDashboard();
        //Create listcontroles
        List<Form>FormControle=new List<Form>();
        List<Button>FormButton=new List<Button>();
        List<PictureBox>pictureconlection=new List<PictureBox>();

        private int index_pic = 0;

        public void ChangecolorButtonlist(Button buttonclicked)
        {
            foreach(Button btn in FormButton)
            {
                if(buttonclicked.Name == btn.Name)
                {
                    btn.BackColor = SystemColors.Control;
                    btn.ForeColor = Color.Black;
                    
                }
                else
                {
                    btn.BackColor = Color.DodgerBlue;
                    btn.ForeColor = Color.White;
                }
            }
        }

        private void ChangePictureboxcollection(int index_picturebox)
        {
            foreach(PictureBox pic in pictureconlection)
            {
                if(pic.Name==this.pictureconlection[index_picturebox].Name)
                {
                    pic.Show();
                }
                else
                {
                    pic.Hide();
                }
            }
        }

        public void getuser()
        {
            lbluser.Text ="Welcome "+ frmLogin.Formlogin.username.Substring(0, 1).ToUpper() + frmLogin.Formlogin.username.Substring(1);
        }
        private void frmManagement_Load(object sender, EventArgs e)
        {
            // Add Controles for Changecolor buttonlsit 
            this.FormButton.Add(btnDashboard);
            this.FormButton.Add(btnProducts);
            this.FormButton.Add(btnCategory);
            this.FormButton.Add(btnStock_in);
            this.FormButton.Add(btn_Order);
            this.FormButton.Add(btnsupplier);
            this.FormButton.Add(btnLogout);

            //Add Controles for Chang color picturebox for list
            this.pictureconlection.Add(pictureBox1);
            this.pictureconlection.Add(pictureBox2);
            this.pictureconlection.Add(pictureBox3);
            this.pictureconlection.Add(pictureBox4);
            this.pictureconlection.Add(pictureBox5);
            this.pictureconlection.Add(pictureBox6);
            this.pictureconlection.Add(pictureBox7);
            
            //Hide PictureBox
            this.pictureBox1.Hide();
            this.pictureBox2.Hide();
            this.pictureBox3.Hide();
            this.pictureBox4.Hide();
            this.pictureBox5.Hide();
            this.pictureBox6.Hide();
            this.pictureBox7.Hide();

            //add form into panael
            Formcategory.TopLevel=false;
            panel1.Controls.Add(Formcategory);
            Formcategory.Dock=DockStyle.Fill;
            FormControle.Add(Formcategory);

            Formproducts.TopLevel=false;
            panel1.Controls.Add(Formproducts);
            Formproducts.Dock=DockStyle.Fill;
            FormControle.Add(Formproducts);

            Formstock.TopLevel=false;
            Formstock.Dock=DockStyle.Fill;
            panel1.Controls.Add(Formstock);
            FormControle.Add(Formstock);

            Formorder.TopLevel=false;
            panel1.Controls.Add(Formorder);
            Formorder.Dock=DockStyle.Fill;
            FormControle.Add(Formorder);

            Formcustomer.TopLevel=false;
            panel1.Controls.Add(Formcustomer);
            Formcustomer.Dock=DockStyle.Fill;
            FormControle.Add(Formcustomer);

            Formdashboard.TopLevel=false;
            panel1.Controls.Add(Formdashboard);
            Formdashboard.Dock=DockStyle.Fill;
            FormControle.Add(Formdashboard);

            
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btnDashboard);
            this.ChangePictureboxcollection(1);
            foreach (Form frm in FormControle)
            {
                if (Formdashboard.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
                Formdashboard.refresh();
            }
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btnProducts);
            this.ChangePictureboxcollection(2);

            foreach (Form frm in FormControle)
            {
                if (Formproducts.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
               
            }
            Formproducts.refresh();

        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btnCategory);
            this.ChangePictureboxcollection(3);
            foreach(Form frm in FormControle)
            {
                if(Formcategory.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
                btnCategory.Refresh();
            }
            


        }

        private void btnStock_in_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btnStock_in);
            this.ChangePictureboxcollection(4);
            foreach (Form frm in FormControle)
            {
                if (Formstock.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
                Formstock.refresh();
            }
            

        }

        private void btnstock_out_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btn_Order);
            this.ChangePictureboxcollection(5);
            foreach (Form frm in FormControle)
            {
                if (Formorder.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
                Formorder.refresh();
            }
            

        }

        private void btnsupplier_Click(object sender, EventArgs e)
        {
            //calling Function
            ChangecolorButtonlist(btnsupplier);
            this.ChangePictureboxcollection(6);
            foreach (Form frm in FormControle)
            {
                if (Formcustomer.Name == frm.Name)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
                Formcustomer.refresh();
            }
            
        }

        private void guna2CirclePictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //DialogResult re=MessageBox.Show("Are you sure you want to logout System...?","Question",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            //if(re==DialogResult.Yes)
            //{
            //    frmLogin frm = new frmLogin();
            //    frm.Show();
            //    this.Hide();
            //}
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ChangecolorButtonlist(btnLogout);
            this.ChangePictureboxcollection(0);
            DialogResult re = MessageBox.Show("Are you sure you want to logout System...?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                frmLogin frm = new frmLogin();
                frm.Show();
                this.Hide();
            }
        }

        

        private void pic_profile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPEG FILE |*.jpg; *.jpeg; | PNG FILE |*.png";
            fd.Title = "OPEN FILE";
            string image = "";
            if (DialogResult.OK == fd.ShowDialog())
            {
                image = fd.FileName;
                pic_profile.ImageLocation = image;

            }
        }

       
    }
}
