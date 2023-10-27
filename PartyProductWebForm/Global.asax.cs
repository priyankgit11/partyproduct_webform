using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PartyProductWebForm
{
    public class Global : System.Web.HttpApplication
    {
        public static SqlConnection con;
        public static readonly HttpClient client = new HttpClient();
        protected void Application_Start(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-38HJ281;Initial Catalog=PartyProductWebForm;Integrated Security=True";
            con = new SqlConnection(connectionString);
            HttpClient client = new HttpClient();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Response.Write(e.ToString());
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

}