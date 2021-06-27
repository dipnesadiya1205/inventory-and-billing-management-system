using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

public partial class Order : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd,cmd1,cmd2;
    OleDbDataAdapter da;
    int o_id;
    int pid;
    int did;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null && Session["pass"] == null)
        {
            MessageBox.Show("FIRST DO LOGIN");
            Response.Redirect("login.aspx");
        }
        else
        {
            con.Open();
            cmd = new OleDbCommand("Select MAX(Order_ID) as max_oid From Order_Master", con);
            da = new OleDbDataAdapter(cmd);
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.GetValue(0) != DBNull.Value)
                {
                    o_id = Convert.ToInt32(dr.GetValue(0));
                    o_id = o_id + 1;
                }
                else
                {
                    o_id = 1;
                }
            }
            txtOrderid.Text = o_id.ToString();

            txtTodaydate.Text = DateTime.Now.Date.ToShortDateString();
            txtDealername.Text = Session["dealername"].ToString();
            txtQty.Text = Session["qty"].ToString();
            txtProductname.Text = Session["productname"].ToString();
        }
    }
    protected void btnPlaceorder_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd1=new OleDbCommand("select Product_ID from Product where Product_Name = '" + txtProductname.Text.Trim() + "' ",con);
        pid=cmd1.ExecuteNonQuery();
        cmd1=new OleDbCommand("select Dealer_ID from Dealer_Master where Dealer_Name = '" + txtDealername.Text.Trim() + "' ",con);
        did=cmd1.ExecuteNonQuery();
        MessageBox.Show("" + did + " " + pid);
        con.Close();
    }
}