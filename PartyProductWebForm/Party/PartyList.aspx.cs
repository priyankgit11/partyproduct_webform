using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Policy;

namespace PartyProductWebForm.Party
{
    public partial class PartyList : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowAllParty();
            }
        }
        protected void ShowAllParty()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                Global.con.Open();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_getall_party";
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
            Response.Redirect(ResolveUrl("~/Party/PartyAddEdit.aspx"));
        }

        protected void edtBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;
            var name = GridView1.Rows[row].Cells[1].Text;
            Server.Transfer(ResolveUrl("~/Party/PartyAddEdit.aspx?method=" + e.CommandName.ToString() + "&id=" + id + "&name=" + name));
        }

        protected void delBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            string id = GridView1.Rows[row].Cells[0].Text;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_delete_party";
                SqlParameter delID = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                delID.Direction = ParameterDirection.Input;
                delID.Value = id;
                cmd.Parameters.Add(delID);
                Global.con.Open();
                int read = cmd.ExecuteNonQuery();
                Response.Write(read);
                Global.con.Close();
                ShowAllParty();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                Global.con.Close();
            }
        }
    }
}