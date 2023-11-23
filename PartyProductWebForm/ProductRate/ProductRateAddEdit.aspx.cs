using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace PartyProductWebForm.ProductRate
{
    public partial class ProductRateAddEdit : System.Web.UI.Page
    {
        string method = String.Empty;
        static int productRateID;
        string productName = String.Empty;
        int rate;
        string date=String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowProductDropdown();
            if (Request.QueryString["method"] != null && PreviousPage != null) // CHECK IF ITS REDIRECTED OR TRANSFERED, THUS AND OPERATOR
            {
                method = Request.QueryString["method"];
                productRateID = Int32.Parse(Request.QueryString["id"]);
                productName = Request.QueryString["productName"];
                rate = Int32.Parse(Request.QueryString["rate"]);
                date = Request.QueryString["date"];
                ProductDDl.Items.FindByText(productName).Selected = true;
                ProductRate.Text = rate.ToString();
                //ratedate.Value = "2012/10/01";
                ratedate.Value = DateTime.Parse(date).ToString("yyyy-MM-dd");

            }
            lblAssignParty.ForeColor = System.Drawing.Color.Black;
            if (method == "EditProductRate" || Button1.Text == "Edit")
            {
                secTitle.Text = "Party Rate Edit";
                Button1.Text = "Edit";
            }
            else
            {
                secTitle.Text = "Party Rate Add";
                Button1.Text = "Add";
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
            int productID = Int32.Parse(ProductDDl.SelectedValue);
            int rate = Int32.Parse(ProductRate.Text);
            string date = ratedate.Value;
            //DateTime dt = 
            Response.Write(productID);
            try
            {
                SqlCommand tmpCmd = new SqlCommand();
                tmpCmd.Connection = Global.con;
                tmpCmd.CommandType = CommandType.StoredProcedure;
                tmpCmd.CommandText = "sp_check_product_rate_exists";

                SqlParameter product = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                SqlParameter productRate = new SqlParameter("@rate", System.Data.SqlDbType.Int);
                SqlParameter rateDate = new SqlParameter("@date", System.Data.SqlDbType.VarChar,50);
                product.Direction = ParameterDirection.Input;
                productRate.Direction = ParameterDirection.Input;
                rateDate.Direction = ParameterDirection.Input;
                product.Value = productID;
                productRate.Value = rate;
                rateDate.Value = date;
                tmpCmd.Parameters.Add(product);
                tmpCmd.Parameters.Add(productRate);
                tmpCmd.Parameters.Add(rateDate);
                Global.con.Open();
                var output = tmpCmd.ExecuteScalar();
                Global.con.Close();

                if ((string)output != "TRUE") // CHECK IF CURRENT DATA EXISTS OR NOT, IF IT DOES THEN NO INSERT.
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Button1.Text == "Add")
                        cmd.CommandText = "sp_store_product_rate";
                    else
                    {
                        SqlParameter rateID = new SqlParameter("@rateID", System.Data.SqlDbType.Int);
                        rateID.Direction = ParameterDirection.Input;
                        rateID.Value = productRateID;
                        cmd.Parameters.Add(rateID);
                        cmd.CommandText = "sp_update_product_rate";
                    }
                    SqlParameter product2 = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                    SqlParameter rate2 = new SqlParameter("@rate", System.Data.SqlDbType.Int);
                    SqlParameter date2 = new SqlParameter("@date", System.Data.SqlDbType.VarChar,50);
                    product2.Direction = ParameterDirection.Input;
                    rate2.Direction = ParameterDirection.Input;
                    date2.Direction = ParameterDirection.Input;
                    product2.Value = productID;
                    rate2.Value = rate;
                    date2.Value = date;
                    cmd.Parameters.Add(product2);
                    cmd.Parameters.Add(rate2);
                    cmd.Parameters.Add(date2);

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