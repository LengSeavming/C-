using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Manage_Inventoty.Classes
{
    internal class Data_category
    {

        SqlConnection con = new SqlConnection("Data Source=SAVANN;Initial Catalog=Management_Inventory;User ID=savann;Password=savann11$$;Pooling=False;Encrypt=False;");

        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }

        public List<Data_category>Data_showCategory()
        {
            con.Open();
            List<Data_category>Data=new List<Data_category>();
            SqlCommand cmd = new SqlCommand("select * from tbl_category", con);
            SqlDataReader dr=cmd.ExecuteReader();
            while(dr.Read())
            {
                Data_category data= new Data_category();
                data.ID = (int)dr["id"];
                data.Name = dr["categoryName"].ToString();
                data.CategoryID = (int)dr["categoryId"];
                data.Status = dr["status"].ToString();
                data.Date = ((DateTime)dr["date"]).ToString("dd/MM/yyyy");
                Data.Add(data);
            }
            return Data;
        }
    }
}
