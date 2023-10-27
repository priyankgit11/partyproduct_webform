using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PartyProductWebForm
{
    public static class Connection
    {
        public static SqlConnection connection { get; set; }
        public static void Init()
        {
            string connectionString = "Data Source=DESKTOP-38HJ281;Initial Catalog=PartyProductWebForm;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionString);
        }
    }
}