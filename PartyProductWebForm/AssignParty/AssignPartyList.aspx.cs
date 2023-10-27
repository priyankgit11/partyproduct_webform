using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartyProductWebForm.AssignParty
{
    public partial class AssignPartyList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ShowAllAssignParty();
            }
        }
        protected void ShowAllAssignParty()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                Global.con.Open();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_getall_assign_party";
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
            Response.Redirect(ResolveUrl("~/AssignParty/AssignPartyAddEdit.aspx"));
        }

        protected void edtBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;
            var partyName = GridView1.Rows[row].Cells[1].Text;
            var productName = GridView1.Rows[row].Cells[2].Text;
            Server.Transfer(ResolveUrl("~/AssignParty/AssignPartyAddEdit.aspx?method=" + e.CommandName.ToString() + "&id=" + id + "&partyName=" + partyName + "&productName=" + productName));
        }

        protected void delBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            int id = Int32.Parse(GridView1.Rows[row].Cells[0].Text);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_delete_assign_party";

                SqlParameter delID = new SqlParameter("@id", System.Data.SqlDbType.Int);
                delID.Direction = ParameterDirection.Input;
                delID.Value = id;
                cmd.Parameters.Add(delID);
                Global.con.Open();
                cmd.ExecuteNonQuery();
                Global.con.Close();
                ShowAllAssignParty();
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