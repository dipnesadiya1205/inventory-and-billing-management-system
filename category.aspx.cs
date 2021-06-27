using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class category : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd;
    OleDbDataAdapter da;
    OleDbCommand cmd1;
    OleDbDataAdapter da1;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null && Session["pass"] == null)
        {
            MessageBox.Show("FIRST DO LOGIN");
            Response.Redirect("login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                ddlCgst.Items.Insert(0, new ListItem("--Select--", " "));
            }
        }
    }

    public void add_display()
    {
        txtCname.Visible = true;
        txtCsgst.Visible = true;
        ddlCgst.Visible = true;
        lblCname.Visible = true;
        lblCcgst.Visible = true;
        lblCsgst.Visible = true;
        lblSi.Visible = true;
        lblSi0.Visible = true;
        lblSi1.Visible = true;
        btnClear.Visible = true;
        btnInsert.Visible = true;
    }

    public void del_display()
    {
        btnDelete.Visible = true;
        btnUpdate.Visible = true;
        lblCnameud.Visible = true;
        ddlCnameud.Visible = true;
        lblCsgstd.Visible = true;
        lblCcgstd.Visible = true;
        txtCgst.Visible = true;
        txtSgst.Visible = true;
    }

    public void add_hide()
    {
        txtCname.Visible = false;
        txtCsgst.Visible = false;
        ddlCgst.Visible = false;
        lblCname.Visible = false;
        lblCcgst.Visible = false;
        lblCsgst.Visible = false;
        lblSi.Visible = false;
        lblSi0.Visible = false;
        lblSi1.Visible = false;
        btnInsert.Visible = false;
        btnClear.Visible = false;
        btnTdelete.Visible = false;
    }

    public void del_hide()
    {
        lblCsgstd.Visible = false;
        lblCcgstd.Visible = false;
        lblCnameud.Visible = false;
        ddlCnameud.Visible = false;
        txtSgst.Visible = false;
        txtCgst.Visible = false;
        btnDelete.Visible = false;
        btnUpdate.Visible = false;
        btnCsave.Visible = false;

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            con.Open();
            cmd = new OleDbCommand("DELETE FROM Category WHERE Category_ID = " + ddlCnameud.SelectedValue + " ", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Deleted Successfully !!!!!!!!");
            con.Close();
            del_hide();
            add_display();
            btnTdelete.Visible = true;
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        int i;
        con.Open();
        cmd1 = new OleDbCommand("select count(*) from Category where Category_Name = '" + txtCname.Text.Trim() + "' ", con);
        da1 = new OleDbDataAdapter(cmd1);
        i = Convert.ToInt32(cmd1.ExecuteScalar());
        if (i == 0)
        {
            cmd = new OleDbCommand("INSERT INTO Category(Category_Name,CGST,SGST) VALUES ('" + txtCname.Text.ToUpper() + "'," + ddlCgst.SelectedValue + ","+ Convert.ToDouble(txtCsgst.Text) +") ", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Inserted Successfully !!!!!!!!");
            ddlCgst.SelectedIndex = 0;
         
        }
        else
        {
            Response.Write("<script>alert('Category is already exist !!!!!!!!')</script>");
        }
        con.Close();
        txtCname.Text = "";
        txtCsgst.Text = "";
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("UPDATE Category SET CGST = " + Convert.ToDouble(ddlCgst.SelectedValue) + " WHERE Category_ID = " + ddlCnameud.SelectedValue + " ", con);
        da = new OleDbDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Updated Successfully !!!!!!!!");
        con.Close();
        btnDelete.Visible = false;
        btnUpdate.Visible = false;
        lblCnameud.Visible = false;
        ddlCnameud.Visible = false;
    }
    protected void btnTdelete_Click(object sender, EventArgs e)
    {
        lblAdd.Visible = false;
        btnCsave.Visible = true;
        ddlCgst.SelectedIndex = 0;
        txtCsgst.Text = "";
        txtCname.Text = "";
        add_hide();
        del_display();
        lblDelete.Visible = true;
            con.Open();               
            cmd = new OleDbCommand("SELECT * FROM Category", con);
            da = new OleDbDataAdapter(cmd);
            da.Fill(ds, "Category");
            ddlCnameud.DataSource = ds.Tables["Category"];
            ddlCnameud.DataTextField = "Category_Name";
            ddlCnameud.DataValueField = "Category_ID";
            ddlCnameud.DataBind();
            ddlCnameud.Items.Insert(0, new ListItem("---SELECT---", " "));
           
            //ddlCgstd.DataSource = ds.Tables["Category"];
            //ddlCgstd.DataTextField = "CGST";
            //ddlCgstd.DataValueField = "CGST";
            //ddlCgstd.DataBind();

            //ddlSgstd.DataSource = ds.Tables["Category"];
            //ddlSgstd.DataTextField = "SGST";
            //ddlSgstd.DataValueField = "SGST";
            //ddlSgstd.DataBind();
            con.Close();      
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtCname.Text = " ";
        ddlCgst.SelectedIndex = 0;
        txtCsgst.Text = "";
    }
  
    protected void ddlCnameud_SelectedIndexChanged(object sender, EventArgs e)
    {
            con.Open();
            cmd1 = new OleDbCommand("SELECT * FROM Category where Category_ID = " + ddlCnameud.SelectedValue + " ", con);
            da1 = new OleDbDataAdapter(cmd1);
            da1.Fill(ds1, "Category");
            txtCgst.Text = ds1.Tables[0].Rows[0].ItemArray.GetValue(2).ToString();
            txtSgst.Text = ds1.Tables[0].Rows[0].ItemArray.GetValue(3).ToString();            
            con.Close();
        
    }
    protected void ddlCgst_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtCsgst.Text = ddlCgst.SelectedValue;
    }
    protected void txtCname_TextChanged(object sender, EventArgs e)
    {
       
    }
    protected void btnCsave_Click(object sender, EventArgs e)
    {
        btnTdelete.Visible = true;
        del_hide();
        add_display();
        lblAdd.Visible = true;
        lblDelete.Visible = false;
    }
}