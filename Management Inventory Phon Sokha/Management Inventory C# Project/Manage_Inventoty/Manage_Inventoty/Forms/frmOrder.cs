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
using TheArtOfDevHtmlRenderer.Adapters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static Guna.UI2.Native.WinApi;

namespace Manage_Inventoty.Forms
{
    public partial class frmOrder : Form
    {
        public frmOrder()
        {
            InitializeComponent();
            DisplayData();
            txtserch.Text = "Search Name OR ID here....!";
            txtserch.ForeColor = Color.Gray;

        }


        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");
        private void frmOrder_Load(object sender, EventArgs e)
        {

        }

        public void refresh()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)Refresh);
                return;
            }
            DisplayData();
            UpdateTotals();
            LoadOrderGrid();
            UpdateTotals();

        }
        public void DisplayData()
        {

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            Data_order data = new Data_order();
            List<Data_order> list = data.Data_showOrder();
            Dtg_Order.DataSource = list;
            Dtg_Order.ColumnHeadersDefaultCellStyle.Font = new Font("Cambria", 15, FontStyle.Bold);
            Dtg_Order.DefaultCellStyle.Font = new Font("Cambria", 13);
            con.Close();
        }
        

        bool check = false;
        

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    decimal getTotal = Convert.ToDecimal(lblAmount.Text.ToString().Replace("$", ""));
                    decimal getChang = Convert.ToDecimal(txtChange.Text);
                    if (getTotal > getChang)
                    {
                        MessageBox.Show("invariable Amount", "Messing", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        check = true;
                        lblTotal.Text = $"$" + (getChang - getTotal);
                    }
                    e.SuppressKeyPress = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex, "Messing", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }



        private void UpdateTotals()
        {
            decimal total = Orderlist.Sum(p => p.Price * p.QTY);
            lblAmount.Text = total.ToString("$0.00");

        }

        List<oder_item> Orderlist = new List<oder_item>();
        private void Dtg_Order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Change "id" to correct column name (case-sensitive)
                string id = (Dtg_Order.Rows[e.RowIndex].Cells["id"].Value.ToString());
                string name = Dtg_Order.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                decimal price = Convert.ToDecimal(Dtg_Order.Rows[e.RowIndex].Cells["Price"].Value);

                var existing = Orderlist.FirstOrDefault(p => p.ID == id);
                if (existing != null)
                {
                    existing.QTY++;
                }
                else
                {
                    Orderlist.Add(new oder_item
                    {
                        ID = id,
                        Name = name,
                        QTY = 1,
                        Price = price
                    });
                }

                LoadOrderGrid();
                UpdateTotals();
            }

        }


        private void LoadOrderGrid()
        {

            var displayList = Orderlist.Select(p => new
            {
                p.ID,
                p.Name,
                p.QTY,
                
                Price = p.Price * p.QTY
            }).ToList();

            Dtg_order_1.DataSource = null;
            Dtg_order_1.DataSource = displayList;
            AddActionButtons();

        }

        private object count;

        private void guna2Button1_Click(object sender, EventArgs e)
        {

                if (txtChange.Text == "")
                {
                    MessageBox.Show("Change Data Please..!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    string countDate = "select count(*) from tbl_order";

                    SqlCommand cmd1 = new SqlCommand(countDate, con);
                    count =Convert.ToInt32( cmd1.ExecuteScalar())+ 1;
                    DialogResult re = MessageBox.Show("Are you sure you want Order?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (re == DialogResult.Yes)
                    {
                      
                      
                        foreach (var item in Orderlist)
                        {
                            if(con.State==ConnectionState.Closed)
                            {
                                con.Open();
                            }  
                            SqlCommand cmd = new SqlCommand("INSERT INTO tbl_order (customer_id,ProductID, Name, QTY, Price, Total,date) VALUES (@cid,@id, @name, @qty, @price, @total ,@date)", con);
                            DateTime today = DateTime.Now;
                         
                            cmd.Parameters.AddWithValue("@cid", $"CID"+count);
                            cmd.Parameters.AddWithValue("@id", item.ID);
                            cmd.Parameters.AddWithValue("@name", item.Name);
                            cmd.Parameters.AddWithValue("@qty", item.QTY);
                            cmd.Parameters.AddWithValue("@price", item.Price);
                            cmd.Parameters.AddWithValue("@total", item.QTY * item.Price);
                            cmd.Parameters.AddWithValue("@date", today);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        MessageBox.Show("Order saved successfully!", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //Orderlist.Clear();
                        //lblAmount.Text = "";
                        //txtChange.Text = "";
                        //lblTotal.Text = "$0.00";
                        LoadOrderGrid();
                        UpdateTotals();
                    }

                }
                

        }


        private void AddActionButtons()
        {
            // បន្ថែមតែបើមិនទាន់មាន
            if (!Dtg_order_1.Columns.Contains("Remove"))
            {
                DataGridViewButtonColumn btnRemove = new DataGridViewButtonColumn();
                btnRemove.Name = "Remove";
                btnRemove.HeaderText = "Remove";
                btnRemove.Text = "X";
                btnRemove.UseColumnTextForButtonValue = true;
                btnRemove.DefaultCellStyle.BackColor = Color.Red;
                btnRemove.DefaultCellStyle.ForeColor = Color.White;
                Dtg_order_1.Columns.Add(btnRemove);
            }

            if (!Dtg_order_1.Columns.Contains("MinusQty"))
            {
                DataGridViewButtonColumn btnMinus = new DataGridViewButtonColumn();
                btnMinus.Name = "MinusQty";
                btnMinus.HeaderText = "Qty -";
                btnMinus.Text = "-";
                btnMinus.UseColumnTextForButtonValue = true;
                btnMinus.DefaultCellStyle.BackColor = Color.Orange;
                btnMinus.DefaultCellStyle.ForeColor = Color.Black;
                Dtg_order_1.Columns.Add(btnMinus);
            }
        }


        private void Dtg_order_1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = Dtg_order_1.Columns[e.ColumnIndex].Name;

                string id = Dtg_order_1.Rows[e.RowIndex].Cells["ID"].Value.ToString();

                var item = Orderlist.FirstOrDefault(p => p.ID == id);
                Color backColor = Color.White;
                Color textColor = Color.Black;

                if (item != null)
                {
                    if (columnName == "Remove")
                    {
                        // លុបចេញ
                        Orderlist.Remove(item);
                        backColor = Color.Red;
                        textColor = Color.White;

                    }
                    else if (columnName == "MinusQty")
                    {
                        if (item.QTY > 1)
                        {
                            item.QTY--; // បន្ថយ QTY
                            backColor = Color.Orange;
                            textColor = Color.Black;
                        }
                        else
                        {
                            Orderlist.Remove(item); // បើ QTY = 1 => លុបចេញ
                        }
                    }

                    LoadOrderGrid();
                    UpdateTotals();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtserch.Text))
                {
                    MessageBox.Show("Input product name or category ID to search!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT id,name, CategoryId FROM tbl_products WHERE UPPER(name)  like @n OR CategoryId = @id or id=@i", con);
                cmd.Parameters.AddWithValue("@n","%"+ txtserch.Text.Trim().ToUpper()+"%");
                cmd.Parameters.AddWithValue("@i", txtserch.Text.Trim());
                if (int.TryParse(txtserch.Text.Trim(), out int id))
                {
                    cmd.Parameters.AddWithValue("@id", id);
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
                    Dtg_Order.DataSource = dt;
                }
                else
                {
                    Dtg_Order.DataSource = null;
                    MessageBox.Show("No matching records found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private void btnreceitp_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(printDocument1_BeginPrint);
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
        private int rowIndex = 0;
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            rowIndex = 0;
        }

       
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float y = e.MarginBounds.Top;
            int[] colWidths = { 50, 180, 70, 100 }; // ID, Name, Qty, Price
            int tableWidth = 0;
            foreach (var w in colWidths) tableWidth += w;

            Font font = new Font("Cambria", 11);
            Font bold = new Font("Cambria", 11, FontStyle.Bold);
            Font headerFont = new Font("Cambria", 16, FontStyle.Bold);
            Font labelFont = new Font("Cambria", 13, FontStyle.Bold);
            StringFormat centerAlign = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            // 🏪 Shop Header (centered)
            string shopName = "Management Inventory In Stock";
            string admin = "Admin: Poun Sokha";
            string address = "Address: #123, Phnom Penh";
            string phone = "Phone: 012 345 678";

            float centerX = e.MarginBounds.Left + (e.MarginBounds.Width / 2);
            e.Graphics.DrawString(shopName, headerFont, Brushes.Black, new PointF(centerX, y), centerAlign);
            y += headerFont.GetHeight(e.Graphics) + 5;
            e.Graphics.DrawString(admin, font, Brushes.Black, new PointF(centerX, y), centerAlign);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString(address, font, Brushes.Black, new PointF(centerX, y), centerAlign);
            y += font.GetHeight(e.Graphics);
            e.Graphics.DrawString(phone, font, Brushes.Black, new PointF(centerX, y), centerAlign);
            y += font.GetHeight(e.Graphics) + 15;

            // 📋 Table Header (centered table)
            string[] headers = { "ID", "Name", "Qty", "Price ($)" };
            float startX = centerX - (tableWidth / 2); // Center table horizontally

            float x = startX;
            for (int i = 0; i < headers.Length; i++)
            {
                e.Graphics.DrawString(headers[i], bold, Brushes.Black, new RectangleF(x, y, colWidths[i], bold.GetHeight(e.Graphics) + 6), centerAlign);
                x += colWidths[i];
            }

            y += bold.GetHeight(e.Graphics) + 8;

            // 🧾 Table Content (centered)
            while (rowIndex < Dtg_order_1.Rows.Count)
            {
                DataGridViewRow row = Dtg_order_1.Rows[rowIndex];
                float rowX = startX;

                for (int i = 0; i < headers.Length; i++)
                {
                    string colName = headers[i].Replace(" ", "").Replace("($)", "");
                    if (Dtg_order_1.Columns.Contains(colName))
                    {
                        object val = row.Cells[colName].Value;
                        string text = val != null ? val.ToString() : "";

                        e.Graphics.DrawString(text, font, Brushes.Black, new RectangleF(rowX, y, colWidths[i], font.GetHeight(e.Graphics) + 5), centerAlign);
                    }
                    rowX += colWidths[i];
                }

                y += font.GetHeight(e.Graphics) + 8;
                rowIndex++;

                if (y + 100 > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            // 💵 Footer (right aligned on page)
            y += 10;
            float rightX = e.MarginBounds.Right - 200;
            e.Graphics.DrawString($"Total Amount:    {lblAmount.Text} $", labelFont, Brushes.Black, rightX, y);
            y += labelFont.GetHeight(e.Graphics) + 4;
            e.Graphics.DrawString($"Payment:     {txtChange.Text} $", labelFont, Brushes.Black, rightX, y);
            y += labelFont.GetHeight(e.Graphics) + 4;
            e.Graphics.DrawString($"Change:   {lblTotal.Text} $", labelFont, Brushes.Black, rightX, y);

            y += labelFont.GetHeight(e.Graphics) + 10;
            e.Graphics.DrawString("----------------------------------", font, Brushes.Black, rightX, y);
            y += font.GetHeight(e.Graphics);

            string dateTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
            e.Graphics.DrawString(dateTime, font, Brushes.Black, rightX, y);

            // Final page
            e.HasMorePages = false;
            rowIndex = 0; // Reset for next print

        }

        private void txtserch_Enter(object sender, EventArgs e)
        {
            if (txtserch.Text == "Search Name OR ID here....!") ;
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
                txtserch.ForeColor= Color.Gray;
            }
        }
    }
}

