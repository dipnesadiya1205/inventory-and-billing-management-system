using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

public partial class Home : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
    protected void lbtnProduct_Click(object sender, EventArgs e)
    {
        Response.Redirect("product.aspx");
    }
    protected void lbtnCategory_Click(object sender, EventArgs e)
    {
        Response.Redirect("category.aspx");
    }
    protected void lbtnSubcategory_Click(object sender, EventArgs e)
    {
        Response.Redirect("subcategory.aspx");
    }    

    protected void lbtnReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("Report.aspx");
    }
    protected void lbtnCalculateBill_Click(object sender, EventArgs e)
    {
        Response.Redirect("bill.aspx");
    }
    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void lbtnDealer_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dealer.aspx");
    }
}
