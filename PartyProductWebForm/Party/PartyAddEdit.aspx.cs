using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartyProductWebForm.Party
{
    public partial class PartyAddEdit : System.Web.UI.Page
    {
        string method = "", id = "", name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblParty.ForeColor = System.Drawing.Color.Black;
            method = Request.QueryString["method"];
            id = Request.QueryString["id"];
            name = Request.QueryString["name"];
            if (method == null)
            {
                Button1.Text = "Add";
            }
            else
            {
                Button1.Text = "Edit";
            }
            if (PreviousPage != null)
            {
                TextBox1.Text = name;
            }
            //Response.Write(id);
            //Response.Write(name);
        }

        protected void Button1_Command(object sender, CommandEventArgs e)
        {
            string txtName = TextBox1.Text;
            if (method != null && txtName != String.Empty && method == "EditParty")
            {
                try
                {
                    SqlCommand tmpCmd = new SqlCommand();
                    SqlParameter checkName = new SqlParameter("@partyName", System.Data.SqlDbType.VarChar, 50);
                    checkName.Direction = ParameterDirection.Input;
                    checkName.Value = txtName.Trim() ;
                    tmpCmd.Connection = Global.con;
                    tmpCmd.CommandType = CommandType.StoredProcedure;
                    tmpCmd.CommandText = "sp_check_party_exists";
                    tmpCmd.Parameters.Add(checkName);
                    Global.con.Open();
                    var output = tmpCmd.ExecuteScalar();
                    Global.con.Close();
                    if ((string)output == "FALSE")
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_update_party";
                        // Add the input parameter and set its properties.
                        SqlParameter editName = new SqlParameter("@partyName", System.Data.SqlDbType.VarChar, 50);
                        editName.Direction = ParameterDirection.Input;
                        editName.Value = txtName.Trim() ;
                        SqlParameter editID = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                        editID.Direction = ParameterDirection.Input;
                        editID.Value = id;
                        cmd.Parameters.Add(editName);
                        cmd.Parameters.Add(editID);
                        Global.con.Open();
                        cmd.ExecuteNonQuery();
                        Global.con.Close();
                        lblParty.Text = "Data Updated Successfully";
                    }
                    else
                    {
                        lblParty.Text = "Enter Unique Party Name!!";
                        lblParty.ForeColor = System.Drawing.Color.Red;
                    }
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
            else if (txtName != String.Empty)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlCommand tmpCmd = new SqlCommand();
                    SqlParameter checkName = new SqlParameter("@partyName", System.Data.SqlDbType.VarChar, 50);
                    checkName.Direction = ParameterDirection.Input;
                    checkName.Value = txtName.Trim() ;
                    tmpCmd.Connection = Global.con;
                    tmpCmd.CommandType = CommandType.StoredProcedure;
                    tmpCmd.CommandText = "sp_check_party_exists";
                    tmpCmd.Parameters.Add(checkName);
                    Global.con.Open();
                    var output = tmpCmd.ExecuteScalar();
                    Global.con.Close();

                    if ((string)output == "FALSE")
                    {
                        SqlParameter addName = new SqlParameter("@partyName", System.Data.SqlDbType.VarChar, 50);
                        addName.Direction = ParameterDirection.Input;
                        addName.Value = txtName.Trim() ;
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_store_party";
                        cmd.Parameters.Add(addName);
                        Global.con.Open();
                        cmd.ExecuteNonQuery();
                        Global.con.Close();
                        lblParty.Text = "Data Inserted Successfully";
                    }
                    else
                    {
                        lblParty.Text = "Enter Unique Party Name!!";
                        lblParty.ForeColor = System.Drawing.Color.Red;
                    }
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
            else
            {

                lblParty.Text = "Insert Data Correctly!!";
                lblParty.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}