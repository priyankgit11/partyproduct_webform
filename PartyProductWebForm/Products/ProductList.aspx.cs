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
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowAllProduct();
            }
        }
        protected void ShowAllProduct()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                Global.con.Open();
                cmd.Connection = Global.con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_getall_products";
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
            Response.Redirect(ResolveUrl("~/Products/ProductAddEdit.aspx"));
        }

        protected void edtBtn_Command(object sender, CommandEventArgs e)
        {
            int row = Int32.Parse(e.CommandArgument.ToString());
            var id = GridView1.Rows[row].Cells[0].Text;
            var name = GridView1.Rows[row].Cells[1].Text;
            Server.Transfer(ResolveUrl("~/Products/ProductAddEdit.aspx?method=" + e.CommandName.ToString() + "&id=" + id + "&name=" + name));
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
                cmd.CommandText = "sp_delete_product";

                SqlParameter delID = new SqlParameter("@productID", System.Data.SqlDbType.Int);
                delID.Direction = ParameterDirection.Input;
                delID.Value = id;
                cmd.Parameters.Add(delID);
                Global.con.Open();
                int read = cmd.ExecuteNonQuery();
                Response.Write(read);
                Global.con.Close();
                ShowAllProduct();
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