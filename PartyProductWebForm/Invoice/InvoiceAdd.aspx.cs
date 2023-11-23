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
    public partial class InvoiceAdd : System.Web.UI.Page
    {
        static int invoiceDetailsID;
        static int invoiceID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowHelper();
                invoiceDetailsID = 0;
            }
            if (!String.IsNullOrEmpty(Request.QueryString["invDetID"]))
            {
                invoiceDetailsID = Int32.Parse(Request.QueryString["invDetID"]);
            }
            if (Request.QueryString["method"] == "ViewInvoice" || Request.QueryString["method"] == "EditInvoice")
                DisplayInvoice();
            ShowGrandTotal();
        }


        protected void ShowHelper()
        {
            ShowPartyName();
            ShowProductName();
            if (Request.QueryString["method"] == "EditInvoice")
            {
                ShowOnEdit();
            }
            SetRate();
        }
        protected void ShowOnEdit()
        {
            if (Request.QueryString["method"] == "EditInvoice")
            {
                int invID = Int32.Parse(Request.QueryString["id"]);
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_get_specific_invoice_detail";

                        cmd.Parameters.AddWithValue("@id", invID);
                        DataTable dt = new DataTable();
                        Global.con.Open();
                        dt.Load(cmd.ExecuteReader());
                        Global.con.Close();
                        invoiceID = Int32.Parse(dt.Rows[0]["ID"].ToString());
                        PartyDDl.SelectedValue = dt.Rows[0]["Party_ID"].ToString();
                        ProductDDl.SelectedValue = dt.Rows[0]["Product_ID"].ToString();
                        txtRate.Text = dt.Rows[0]["Rate"].ToString();
                        txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                        PartyDDl.Enabled = false;
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
        protected void ShowGrandTotal()
        {
            if (invoiceDetailsID != 0)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_get_grandtotal";
                        cmd.Parameters.AddWithValue("@id", invoiceDetailsID);
                        Global.con.Open();
                        string gtotal = cmd.ExecuteScalar().ToString();
                        Global.con.Close();
                        gtotalLbl.Text = gtotal;
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
        protected void ShowPartyName()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
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

        /// <summary>
        /// Bind data in Product's Dropdown list
        /// </summary>
        protected void ShowProductName()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_get_product_per_party_assign_party";
                    SqlParameter partyID = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                    partyID.Direction = ParameterDirection.Input;
                    partyID.Value = PartyDDl.SelectedValue;
                    cmd.Parameters.Add(partyID);
                    Global.con.Open();
                    ProductDDl.DataSource = cmd.ExecuteReader();
                    ProductDDl.DataTextField = "Product_Name";
                    ProductDDl.DataValueField = "ID";
                    ProductDDl.DataBind();
                    Global.con.Close();
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
        protected void DisplayInvoice()
        {
            if (Request.QueryString["method"] == "ViewInvoice")
            {
                invoiceDetailsID = Int32.Parse(Request.QueryString["id"]);
                Panel1.Visible = false;
                secTitle.Text = "View Invoice";
            }
            if (invoiceDetailsID != 0)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_getall_invoice_details";

                        cmd.Parameters.AddWithValue("@id", invoiceDetailsID);

                        Global.con.Open();
                        GridView1.DataSource = cmd.ExecuteReader();
                        GridView1.DataBind();
                        Global.con.Close();
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
        protected void SetRate()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_get_rate";
                    SqlParameter productID = new SqlParameter("@id", System.Data.SqlDbType.Int);
                    productID.Direction = ParameterDirection.Input;
                    productID.Value = ProductDDl.SelectedValue;
                    cmd.Parameters.Add(productID);
                    Global.con.Open();
                    int rate = (int)cmd.ExecuteScalar();
                    txtRate.Text = rate.ToString();
                    Global.con.Close();
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
        protected void PartyDDl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowProductName();
            SetRate();
        }
        protected void ProductDDl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRate();
        }
        protected void editBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;
            Server.Transfer(ResolveUrl("~/Invoice/InvoiceAdd.aspx?method=" + e.CommandName.ToString() + "&id=" + id + "&invDetID=" + invoiceDetailsID));
        }

        protected void deleteBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = Global.con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_delete_invoice";
                    cmd.Parameters.AddWithValue("@id",id);
                    Global.con.Open();
                    cmd.ExecuteNonQuery();
                    Global.con.Close();
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
            DisplayInvoice();
            ShowGrandTotal();
        }

        protected void closeBtn_Command(object sender, CommandEventArgs e)
        {
            invoiceDetailsID = 0;
            Response.Redirect(ResolveUrl("~/Invoice/InvoiceList.aspx"));
        }

        protected void addBtn_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (invoiceDetailsID == 0) // To check if data is inserted first time or not.
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_store_invoice_details";

                        SqlParameter partyID = new SqlParameter("@partyID", System.Data.SqlDbType.Int);
                        partyID.Direction = ParameterDirection.Input;
                        partyID.Value = Int32.Parse(PartyDDl.SelectedValue);

                        cmd.Parameters.Add(partyID);
                        cmd.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = DateTime.Now;
                        cmd.Parameters.Add(new SqlParameter("@grandTotal", System.Data.SqlDbType.Int)).Value = 0;
                        Global.con.Open();
                        invoiceDetailsID = Int32.Parse(cmd.ExecuteScalar().ToString());
                        Global.con.Close();
                    }
                }
                if (Request.QueryString["method"] == "EditInvoice")
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_update_invoice_details";

                        cmd.Parameters.AddWithValue("@id", invoiceID);
                        cmd.Parameters.AddWithValue("@productID", Int32.Parse(ProductDDl.SelectedValue));
                        cmd.Parameters.AddWithValue("@rate", Int32.Parse(txtRate.Text));
                        cmd.Parameters.AddWithValue("@qty", Int32.Parse(txtQuantity.Text));
                        Global.con.Open();
                        cmd.ExecuteNonQuery();
                        Global.con.Close();
                        ShowGrandTotal();
                    }
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = Global.con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_store_invoice";

                        cmd.Parameters.Add(new SqlParameter("@partyID", System.Data.SqlDbType.Int)).Value = Int32.Parse(PartyDDl.SelectedValue);
                        cmd.Parameters.Add(new SqlParameter("@productID", System.Data.SqlDbType.Int)).Value = Int32.Parse(ProductDDl.SelectedValue);
                        cmd.Parameters.Add(new SqlParameter("@rate", System.Data.SqlDbType.Int)).Value = Int32.Parse(txtRate.Text);
                        cmd.Parameters.Add(new SqlParameter("@qty", System.Data.SqlDbType.Int)).Value = Int32.Parse(txtQuantity.Text);
                        cmd.Parameters.Add(new SqlParameter("@invoiceID", System.Data.SqlDbType.Int)).Value = invoiceDetailsID;
                        Global.con.Open();
                        cmd.ExecuteNonQuery();
                        Global.con.Close();

                        PartyDDl.Enabled = false;
                    }
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
            DisplayInvoice();
            ShowGrandTotal();
        }


    }
}