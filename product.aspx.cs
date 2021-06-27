using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class images : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd , cmd1;
    OleDbDataAdapter da , da1;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    int reorder;
    int qty;
    double rate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null && Session["pass"] == null)
        {
            MessageBox.Show("First login");
            Response.Redirect("login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                con.Open();

                cmd1 = new OleDbCommand("SELECT * FROM Dealer_Master", con);
                da1 = new OleDbDataAdapter(cmd1);
                da1.Fill(ds1, "Dealer_Master");
                ddlDealer.DataSource = ds1.Tables["Dealer_Master"];
                ddlDealer.DataTextField = "Dealer_Name";
                ddlDealer.DataValueField = "Dealer_ID";
                ddlDealer.DataBind();
                ddlDealer.Items.Insert(0, new ListItem("--SELECT--", "-1"));

                cmd = new OleDbCommand("SELECT * FROM Subcategory", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Subcategory");
                ddlSubno.DataSource = ds.Tables["Subcategory"];
                ddlSubno.DataTextField = "Subcategory_Name";
                ddlSubno.DataValueField = "Subcategory_ID";
                ddlSubno.DataBind();
                ddlSubno.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
        }
    }

    protected void btnPcancel_Click(object sender, EventArgs e)
    {
        txtPname.Text = "";
        txtPqty.Text = "";
        txtPrate.Text = "";
        txtReOrder.Text = "";
        ddlSubno.SelectedIndex = 0;
        ddlDealer.SelectedIndex = 0;
    }
    protected void btnInsert_Click1(object sender, EventArgs e)
    {
        int i;
        con.Open();
        cmd1 = new OleDbCommand("SELECT COUNT(*) FROM Product Where Product_Name = '" + txtPname.Text.Trim().ToUpper() + "' ", con);
        da1 = new OleDbDataAdapter(cmd1);
        i = Convert.ToInt32(cmd1.ExecuteScalar());
        if (i == 0)
        {
            qty = Convert.ToInt32(txtPqty.Text);
            reorder = Convert.ToInt32(txtReOrder.Text);
            rate=Convert.ToDouble(txtPrate.Text);

            if (qty > 0 && reorder < qty && rate > 0 && reorder > 0)
            {

                cmd = new OleDbCommand("INSERT INTO Product(Product_Name,Qty,Reorder_qty,Rate,Subcategory_ID,Dealer_ID) VALUES ('" + txtPname.Text.ToUpper() + "'," + qty + "," + reorder + "," + rate + "," + Convert.ToInt32(ddlSubno.SelectedValue) + "," + Convert.ToInt32(ddlDealer.SelectedValue) + ") ", con);
                da = new OleDbDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                MessageBox.Show("INSERTED SUCCESSFULLY !!!!!");
                con.Close();
            }
            else 
            {
                MessageBox.Show("Check Details Again");
            }
        }
        else
        {
            Response.Write("<script>alert('Product is already exist !!!!!!!!')</script>");
        }
        txtPname.Text = "";
        txtPqty.Text = "";
        txtPrate.Text = "";
        txtReOrder.Text = "";
        ddlSubno.SelectedIndex = 0;
        ddlDealer.SelectedIndex = 0;
    }
    

    protected void ddlCateno_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnOption_Click(object sender, EventArgs e)
    {
     
   
        
        txtPqty.Text = "";
        txtPrate.Text = "";
        txtReOrder.Text = "";
        ddlSubno.SelectedIndex = 0;
        ddlDealer.SelectedIndex = 0;
    }
    protected void btnPdelete_Click(object sender, EventArgs e)
    {
        if(IsPostBack)
        {
            con.Open();
            cmd = new OleDbCommand("DELETE FROM Product WHERE Product_Name = '" + txtPname.Text + "' ", con);
            da = new OleDbDataAdapter(cmd);           
            cmd.ExecuteNonQuery();
            MessageBox.Show("DELETED SUCCESSFULLY !!!!!");
            con.Close();         
          
            txtPqty.Text = "";
            txtPrate.Text = "";
            txtReOrder.Text = "";
         
        }
    }
    protected void btnPupdate_Click(object sender, EventArgs e)
    {
        qty = Convert.ToInt32(txtPqty.Text);
        reorder = Convert.ToInt32(txtReOrder.Text);
        rate = Convert.ToDouble(txtPrate.Text);
        con.Open();

        if (qty > 0 && reorder < qty && rate > 0 && reorder > 0)
        {
            cmd = new OleDbCommand("UPDATE Product SET Qty = " + Convert.ToInt32(txtPqty.Text) + ",Reorder_qty=" + Convert.ToInt32(txtReOrder.Text) + ",Rate = " + Convert.ToInt32(txtPrate.Text) + " , Dealer_ID =" + Convert.ToInt32(ddlDealer.SelectedValue) + " WHERE Product_Name = '" + txtPname.Text + "' ", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Updated Successfully !!!!!!!!");
            da.Fill(ds, "Product");
            gvProduct.DataSource = ds.Tables["Bill_Master"];
            gvProduct.DataBind();
        }
        else
        {
            MessageBox.Show("Check Details Again");
        }
        con.Close();       
    
        txtReOrder.Text = "";
        txtPqty.Text = "";
        txtPrate.Text = "";
        txtPname.Text = "";
        ddlSubno.SelectedIndex = 0;
        ddlDealer.SelectedIndex = 0;

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {    
        txtPqty.Text = "";
        txtProductname.Text = "";
        txtPrate.Text = "";
        txtReOrder.Text = "";
        ddlDealer.SelectedIndex = 0;
    }
   
    protected void txtProductname_TextChanged(object sender, EventArgs e)
    {
       
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM Product WHERE Product_Name LIKE '" + txtProductname.Text.Trim() + "' + '%'", con);
            da = new OleDbDataAdapter(cmd);
            da.Fill(ds, "Bill_Master");
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == false)
            {
                txtPqty.Text = "";
                txtProductname.Text = "";
                txtPrate.Text = "";
                txtReOrder.Text = "";
                ddlDealer.SelectedIndex = 0;
            }

            gvProduct.DataSource = ds.Tables["Bill_Master"];
            gvProduct.DataBind();
            gvProduct.Enabled = true;
            con.Close();
        
    }
    protected void gvProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvProduct.SelectedRow;
        txtProductname.Text = "";
        txtPname.Text = row.Cells[2].Text;
        txtPqty.Text = row.Cells[3].Text;
        txtReOrder.Text = row.Cells[4].Text;
        txtPrate.Text = row.Cells[5].Text;
        ddlSubno.SelectedValue = row.Cells[6].Text;
        ddlDealer.SelectedValue = row.Cells[7].Text;
    }
}