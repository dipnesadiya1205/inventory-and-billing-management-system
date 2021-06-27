using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;



public partial class Report : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd,cmd1;
    OleDbDataAdapter da,da1;
    DataSet ds = new DataSet();    
    DateTime d;
    DateTime t;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null && Session["pass"] == null)
        {
            MessageBox.Show("FIRST DO LOGIN");
            Response.Redirect("login.aspx");
        }                       
    }

    public void reorder()
    {
        con.Open();
        cmd = new OleDbCommand("select a.Product_ID,a.Product_Name,a.Qty,a.Reorder_qty,a.Rate,b.Subcategory_Name,c.Dealer_Name from Subcategory b,Product a,Dealer_Master c Where a.Subcategory_ID=b.Subcategory_ID and a.Dealer_ID=c.Dealer_ID and a.Qty<=a.Reorder_qty", con);
        da = new OleDbDataAdapter(cmd);
        da.Fill(ds, "Product");
        gvReOrder.DataSource = ds.Tables["Product"];
        gvReOrder.DataBind();
        con.Close();
    }

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlReport.SelectedValue) == 0)
        {
            pnlDailySales.Visible = false;
            pnlReOrder.Visible = false;
            pnlCustomer.Visible = false;
            pnlBilldetail.Visible = false;
        }
        else if (Convert.ToInt32(ddlReport.SelectedValue) == 1)
        {
            pnlDailySales.Visible = true;
            pnlReOrder.Visible = false;
            pnlCustomer.Visible = false;
            pnlBilldetail.Visible = false;
        }
        else if (Convert.ToInt32(ddlReport.SelectedValue) == 2)
        {
            pnlDailySales.Visible = false;
            pnlReOrder.Visible = true;
            pnlCustomer.Visible = false;
            pnlBilldetail.Visible = false;
            reorder();
        }
        else if (Convert.ToInt32(ddlReport.SelectedValue) == 3)
        {
            pnlDailySales.Visible = false;
            pnlReOrder.Visible = false;
            pnlCustomer.Visible = true;
            pnlBilldetail.Visible = false;
        }
        else if (Convert.ToInt32(ddlReport.SelectedValue) == 4)
        {
            pnlDailySales.Visible = false;
            pnlReOrder.Visible = false;
            pnlCustomer.Visible = false;
            pnlBilldetail.Visible = true;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        d = Convert.ToDateTime(txtDateFrom.Text);
       /* String.Format("{0:dd/mm/yyyy}", d);
        */
        con.Open();
       
        if (txtDateFrom.Visible==true && txtDateTo.Visible==false)
        {            
            if (DateTime.Now.Date >= d)
            {
                cmd = new OleDbCommand("Select b.Bill_No,c.Cust_Name,b.Total_amount from Bill_Master b,Cust_Detail c Where c.Cust_ID=b.Cust_ID AND b.Date_of_Purchase = #" + d.ToShortDateString() + "# ", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Bill_Master");
                gvDailySales.DataSource = ds.Tables["Bill_Master"];
                gvDailySales.DataBind();
            }
            else
            {
                MessageBox.Show("Select Current Date or Before current date");
            }
        }
        else if (txtDateTo.Visible==true && txtDateTo.Visible==true)
        {
            t = Convert.ToDateTime(txtDateTo.Text);
          /*  String.Format("{0:dd/mm/yyyy}", t);*/
            txtDateTo.Text = DateTime.Now.Date.ToShortDateString();

            if (d < t)
            {
                if (t <= DateTime.Now.Date)
                {
                    cmd1 = new OleDbCommand("Select b.Bill_No,c.Cust_Name,b.Total_amount from Bill_Master b,Cust_Detail c Where c.Cust_ID=b.Cust_ID AND b.Date_of_Purchase between #" + d.ToShortDateString() + "# and #" + t.ToShortDateString() + "# ", con);
                    da1 = new OleDbDataAdapter(cmd1);
                    da1.Fill(ds, "Bill_Master");
                    gvDailySales.DataSource = ds.Tables["Bill_Master"];
                    gvDailySales.DataBind();
                }
                else
                {
                    MessageBox.Show("Ending date should be less than or equal to current date");
                }
            }
            else
            {
                MessageBox.Show("Starting date should be less than ending date");
            }
        }
        con.Close();

    }
    protected void ddlDatePeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlDatePeriod.SelectedValue) == 1)
        {
            lblDate.Visible = true;
            txtDateFrom.Visible = true;
            btnSearch.Visible = true;
            txtDateTo.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Visible = false;
            lblDate0.Visible = false;
        }
        else if (Convert.ToInt32(ddlDatePeriod.SelectedValue) == 2)
        {
            lblDate0.Visible = true;
            txtDateTo.Visible = true;
            lblDate.Visible = true;
            txtDateFrom.Visible = true;
            txtDateFrom.Text = "";
            btnSearch.Visible = true;
        }
        
    }

    protected void txtSearchCustomer_TextChanged1(object sender, EventArgs e)
    {
            con.Open();
            cmd = new OleDbCommand("SELECT a.Cust_ID,a.Cust_Name,b.Bill_No,b.Total_amount,b.Date_of_Purchase FROM Cust_Detail a,Bill_Master b WHERE a.Cust_ID=b.Cust_ID AND a.Cust_Name LIKE '" + txtSearchCustomer.Text.Trim() + "' + '%'", con);
            da = new OleDbDataAdapter(cmd);
            da.Fill(ds, "Bill_Master");
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == false)
            {
                lblCustDetail0.Text = "";
                lblCustName0.Text = "";
            }
            gvCustomer.DataSource = ds.Tables["Bill_Master"];
            gvCustomer.DataBind();
            gvCustomer.Enabled = true;                       
            con.Close();         
    }


    protected void gvCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvCustomer.SelectedRow;
        txtSearchCustomer.Text = row.Cells[2].Text;
        con.Open();
        lblCustName0.Text = txtSearchCustomer.Text.ToString().ToUpper();

        string query = "Select sum(Total_amount) from Bill_Master where Cust_ID = (select Cust_ID from Cust_Detail where Cust_Name = '" + txtSearchCustomer.Text.Trim() + "')";
        using (System.Data.IDbCommand command = new System.Data.OleDb.OleDbCommand(query, con))
        {
            object result = command.ExecuteScalar();
            lblCustDetail0.Text = Convert.ToString(result);
        }
        con.Close();
    }
    protected void txtBillno_TextChanged(object sender, EventArgs e)
    {
        con.Open();

        cmd1 = new OleDbCommand("select a.Cust_Name,a.MOB_NO,b.Total_amount,b.Date_of_Purchase from Cust_Detail a,Bill_Master b where a.Cust_ID=b.Cust_ID and b.Bill_No = " + txtBillno.Text + " ", con);
        OleDbDataReader dr1 = cmd1.ExecuteReader();

       
            if (dr1.Read())
            {
                txtCustname.Text = dr1["Cust_Name"].ToString();
                txtMno.Text = dr1["MOB_NO"].ToString();
                txtDateofpurchase.Text = dr1["Date_of_Purchase"].ToString();
                txtTamount.Text = dr1["Total_amount"].ToString();               
            }
                 cmd = new OleDbCommand("select b.Product_Name,a.CGST,a.SGST,a.RATE,a.QUANTITY,a.DISCOUNT,a.AMOUNT from Product b,Product_Detail a where a.Product_ID=b.Product_ID and a.Bill_No = " + txtBillno.Text + "", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Product_Detail");
                OleDbDataReader dr = cmd.ExecuteReader();
                gvBillDetail.DataSource = ds.Tables["Product_Detail"];
                gvBillDetail.DataBind();
     
        if (dr1.HasRows == false)
        {
            txtTamount.Text = "";
            txtCustname.Text = "";
            txtDateofpurchase.Text = "";
            txtMno.Text = "";
            
        }
        con.Close();
    }

    protected void gvReOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvReOrder.SelectedRow;
        txtProductname.Text = row.Cells[2].Text;
        txtQty.Text = row.Cells[3].Text;
        txtDealername.Text = row.Cells[7].Text;
    }
    protected void btnPlaceorder_Click(object sender, EventArgs e)
    {
        Session["productname"] = txtProductname.Text;
        Session["qty"] = txtQty.Text;
        Session["dealername"] = txtDealername.Text;
        Response.Redirect("Order.aspx");
    }
}