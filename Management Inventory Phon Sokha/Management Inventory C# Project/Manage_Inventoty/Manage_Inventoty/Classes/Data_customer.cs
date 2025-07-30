using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage_Inventoty.Classes
{
    internal class Data_customer
    {
        SqlConnection con = new SqlConnection(@"Server=.\SQLEXPRESS02;Database=Management_Inventory;UID=sa;PWD=sa123456");
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string Qty { get; set; }
        public string Prices { get; set; }
        public string Totalprice { get; set; }

        public string DateOrder { get; set; }

        public List<Data_customer> list_Customer()
        {
            con.Open();
            List<Data_customer> Data = new List<Data_customer>();
            SqlCommand cmd = new SqlCommand("select * from tbl_order ", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Data_customer data = new Data_customer();
                data.ID = Convert.ToInt32(dr["OrderID"]);
                data.CustomerID = dr["customer_id"].ToString();
                data.ProductID = dr["productID"].ToString();
                data.Qty = dr["QTY"].ToString();
                data.Prices = dr["Price"].ToString();
                data.Totalprice = dr["Total"].ToString();
                data.DateOrder = ((DateTime)dr["date"]).ToString("dd-MM-yyyy");
                Data.Add(data);
            }
            return Data;
        }

        public List<Data_customer> list_sale()
        {
            con.Open();
            List<Data_customer> Data = new List<Data_customer>();

            SqlCommand cmd = new SqlCommand("select * from tbl_order where CAST(date AS DATE) = @d", con);
            cmd.Parameters.AddWithValue("@d", DateTime.Today);  // use DateTime, not string

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Data_customer data = new Data_customer();
                data.ID = Convert.ToInt32(dr["OrderID"]);
                data.CustomerID = dr["customer_id"].ToString();
                data.ProductID = dr["productid"].ToString();
                data.Qty = dr["qty"].ToString();
                data.Prices = dr["price"].ToString();
                data.Totalprice = dr["total"].ToString();
                data.DateOrder = ((DateTime)dr["date"]).ToString("dd-MM-yyyy");
                Data.Add(data);
            }

            dr.Close();
            con.Close();

            return Data;
        }

    }
}
