using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Manage_Inventoty.Classes
{
    internal class Data_products
    {
        SqlConnection con = new SqlConnection("Data Source=SAVANN;Initial Catalog=Management_Inventory;User ID=savann;Password=savann11$$;Pooling=False;Encrypt=False;");

        public string ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Stock { get; set; }
        public string Price{ get; set; }
        public string CategoryID { get; set; }
        public string Status { get; set; }
       
        public string Date { get; set; }

        public List<Data_products> Data_showProduct()
        {
            con.Open();
            List<Data_products> Data = new List<Data_products>();
            SqlCommand cmd = new SqlCommand("select * from tbl_products", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Data_products data = new Data_products();
                data.ID = dr["id"].ToString();
                data.Name = dr["name"].ToString();
                data.Category = dr["category"].ToString();
                data.Stock = dr["stock"].ToString();
                data.Price ="$ "+ dr["price"].ToString();
                data.CategoryID=dr["CategoryId"].ToString();
                data.Status = dr["status"].ToString();
                data.Date = ((DateTime)dr["date"]).ToString("dd/MM/yyyy");
                Data.Add(data);
            }

            return Data;
        }
    }
}
