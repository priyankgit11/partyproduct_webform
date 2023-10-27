using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Lifetime;

namespace PartyProductWebForm.AssignParty
{
    public partial class AssignPartyAddEdit : System.Web.UI.Page
    {
        string method = String.Empty;
        static int assignPartyID;
        string partyName = String.Empty;
        string productName = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            ShowHelper();
            if (Request.QueryString["method"] != null && PreviousPage!=null) // CHECK IF ITS REDIRECTED OR TRANSFERED, THUS AND OPERATOR
            {
                method = Request.QueryString["method"];
                partyName = Request.QueryString["partyName"];
                productName = Request.QueryString["productName"];
                assignPartyID = Int32.Parse(Request.QueryString["id"].ToString());
                PartyDDl.Items.FindByText(partyName).Selected = true;
                ProductDDl.Items.FindByText(productName).Selected = true;
            }
            lblAssignParty.ForeColor = System.Drawing.Color.Black;
            if (method == "EditAssignParty" || Button1.Text == "Edit")
            {
                secTitle.Text = "Assign Party Edit";
                Button1.Text = "Edit";
            }
            else
            {
                secTitle.Text = "Assign Party Add";
                Button1.Text = "Add";
            }
        }
        protected void ShowHelper()
        {
            ShowPartyInDropdown();
            ShowProductDropdown();
        }
        protected void ShowPartyInDropdown()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_get_party_assign_party";
                Global.con.Open();
                PartyDDl.DataSource = cmd.ExecuteReader();
                PartyDDl.DataTextField = "Party_Name";
                PartyDDl.DataValueField = "ID";
                PartyDDl.DataBind();
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
        protected void ShowProductDropdown()
        {

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_getall_products";

                Global.con.Open();
                ProductDDl.DataSource = cmd.ExecuteReader();
                ProductDDl.DataTextField = "Product_Name";
                ProductDDl.DataValueField = "ID";
                ProductDDl.DataBind();
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

        protected void Button1_Command1(object sender, CommandEventArgs e)
        {
            int partyID = Int32.Parse(PartyDDl.SelectedValue);
            int productID = Int32.Parse(ProductDDl.SelectedValue);
            Response.Write(partyID);
            Response.Write(productID);
            try
            {
                SqlCommand tmpCmd = new SqlCommand();
                tmpCmd.Connection = Global.con;
                tmpCmd.CommandType = CommandType.StoredProcedure;
                tmpCmd.CommandText = "sp_check_assign_party_exists";

                SqlParameter party = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                SqlParameter product = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                party.Direction = ParameterDirection.Input;
                product.Direction = ParameterDirection.Input;
                party.Value = partyID;
                product.Value = productID;
                tmpCmd.Parameters.Add(party);
                tmpCmd.Parameters.Add(product);
                Global.con.Open();
                var output = tmpCmd.ExecuteScalar();
                Global.con.Close();

                if ((string)output != "TRUE") // CHECK IF CURRENT DATA EXISTS OR NOT, IF IT DOES THEN NO INSERT.
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Button1.Text == "Add")
                        cmd.CommandText = "sp_store_assign_party";
                    else
                    {
                        SqlParameter assignID = new SqlParameter("@assignPartyID", System.Data.SqlDbType.Int);
                        assignID.Direction = ParameterDirection.Input;
                        assignID.Value = assignPartyID;
                        cmd.Parameters.Add(assignID);
                        cmd.CommandText = "sp_update_assign_party";
                    }
                    SqlParameter party2 = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                    SqlParameter product2 = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                    party2.Direction = ParameterDirection.Input;
                    product2.Direction = ParameterDirection.Input;
                    party2.Value = partyID;
                    product2.Value = productID;
                    cmd.Parameters.Add(party2);
                    cmd.Parameters.Add(product2);

                    Global.con.Open();
                    int read = cmd.ExecuteNonQuery();
                    Response.Write(read);
                    Global.con.Close();
                    lblAssignParty.Text = "Data Inserted Successfully";
                }
                else
                {
                    lblAssignParty.Text = "Data Already Exists";
                    lblAssignParty.ForeColor = System.Drawing.Color.Red;
                }

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
    }
}