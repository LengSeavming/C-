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

        SqlConnection con = new SqlConnection("Data Source=SAVANN;Initial Catalog=Management_Inventory;User ID=savann;Password=savann11$$;Pooling=False;Encrypt=False;");

        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Stock { get; set; }
        public string Price { get; set; }


        public List<Data_order> Data_showOrder()
        {
            List<Data_order> Data = new List<Data_order>();

            //using (SqlConnection con = new SqlConnection("your_connection_string"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_products", con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Data_order data = new Data_order();
                    data.ID = dr["id"].ToString();
                    data.Name = dr["name"].ToString();
                    data.Category=dr["category"].ToString();
                    data.Stock = dr["stock"].ToString();
                    data.Price = dr["price"].ToString();

                    Data.Add(data);
                }
            }

            return Data;
        }

    }
}
