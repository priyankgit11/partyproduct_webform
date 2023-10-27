using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartyProductWebForm.Products
{
    public partial class ProductAddEdit : System.Web.UI.Page
    {
        string method = "", id = "", name = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            lblProduct.ForeColor = System.Drawing.Color.Black;
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
            if (method != null && txtName != String.Empty && method == "EditProduct")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_update_product";
                    // Add the input parameter and set its properties.
                    SqlParameter editName = new SqlParameter("@productName", System.Data.SqlDbType.VarChar, 50);
                    editName.Direction = ParameterDirection.Input;
                    editName.Value = txtName;
                    SqlParameter editID = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                    editID.Direction = ParameterDirection.Input;
                    editID.Value = id;
                    cmd.Parameters.Add(editName);
                    cmd.Parameters.Add(editID);
                    Global.con.Open();
                    cmd.ExecuteNonQuery();
                    Global.con.Close();
                    lblProduct.Text = "Data Updated Successfully";
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
                    SqlParameter checkName = new SqlParameter("@productName", System.Data.SqlDbType.VarChar, 50);
                    checkName.Direction = ParameterDirection.Input;
                    checkName.Value = txtName;
                    tmpCmd.Connection = Global.con;
                    tmpCmd.CommandType = CommandType.StoredProcedure;
                    tmpCmd.CommandText = "sp_check_product_exists";
                    tmpCmd.Parameters.Add(checkName);
                    Global.con.Open();
                    var output = tmpCmd.ExecuteScalar();
                    Global.con.Close();

                    if ((string)output == "FALSE")
                    {
                        SqlParameter addName = new SqlParameter("@productName", System.Data.SqlDbType.VarChar, 50);
                        addName.Direction = ParameterDirection.Input;
                        addName.Value = txtName;
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_store_product";
                        cmd.Parameters.Add(addName);
                        Global.con.Open();
                        cmd.ExecuteNonQuery();
                        Global.con.Close();
                        lblProduct.Text = "Data Inserted Successfully";
                    }
                    else
                    {
                        lblProduct.Text = "Enter Unique Party Name!!";
                        lblProduct.ForeColor = System.Drawing.Color.Red;
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
                lblProduct.Text = "Insert Data Correctly!!";
                lblProduct.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}