using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage_Inventoty.Classes
{
    internal class Data_order
    {

        SqlConnection con = new SqlConnection(@"Server =.\SQLEXPRESS02; Database=Management_Inventory;UID=sa;PWD=sa123456");

        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }

        public int Total { get; set; }


        public List<Data_order> Data_showOrder()
        {
            List<Data_order> Data = new List<Data_order>();

            //using (SqlConnection con = new SqlConnection("your_connection_string"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT *,[stock]*[price] As [Total] FROM tbl_products", con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Data_order data = new Data_order();
                    data.ID = dr["id"].ToString();
                    data.Name = dr["name"].ToString();
                    data.Category=dr["category"].ToString();
                    data.Stock = Convert.ToInt32(dr["stock"]);
                    data.Price = Convert.ToInt32(dr[4]);
                    data.Total = data.Price * data.Stock;
                    Data.Add(data);
                }
            }

            return Data;
        }

    }
}
