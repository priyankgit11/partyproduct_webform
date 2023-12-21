using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartyProductWebForm.Invoice
{
    public partial class InvoiceList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowAllInvoice();
            }
        }
        protected void ShowAllInvoice()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                Global.con.Open();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_getall_invoice";
                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
                Global.con.Close();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                Global.con.Close();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Invoice/InvoiceAdd.aspx"));
        }
        protected void viewBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;
            Server.Transfer(ResolveUrl("~/Invoice/InvoiceAdd.aspx?method=" + e.CommandName.ToString() + "&id=" +id));
        }

    }
}