using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class subcategory : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd,cmd1;
    OleDbDataAdapter da,da1;
    DataSet ds = new DataSet();

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
                con.Open();
                cmd = new OleDbCommand("SELECT * FROM Category", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Category");
                ddlCategory.DataSource = ds.Tables["Category"];
                ddlCategory.DataTextField = "Category_Name";
                ddlCategory.DataValueField = "Category_ID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("--Select--", "-1"));
                con.Close();
            }
        }
    }
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        int i;
        con.Open();
        cmd1 = new OleDbCommand("SELECT COUNT(*) FROM Subcategory WHERE Subcategory_Name = '" + txtSubcategory.Text.Trim().ToUpper() + "' ", con);
        da1 = new OleDbDataAdapter(cmd1);
        i = Convert.ToInt32(cmd1.ExecuteScalar());
        if (i == 0)
        {
            cmd = new OleDbCommand("INSERT INTO Subcategory(Subcategory_Name,Category_ID) VALUES ('" + txtSubcategory.Text.ToUpper() + "'," + Convert.ToInt32(ddlCategory.SelectedValue) + ") ", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Inserted Successfully !!!!!!!!");
            con.Close();
        }
        else
        {
            Response.Write("<script>alert('Subategory is already exist !!!!!!!!')</script>");
        }
        txtSubcategory.Text = "";
        ddlCategory.SelectedIndex = 0;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            con.Open();
            cmd = new OleDbCommand("DELETE FROM Subcategory WHERE Subcategory_Name = '" + txtSub.Text.Trim() + "' ", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Deleted Successfully !!!!!!!!");
            con.Close();            
        }
        txtSub.Text = "";
        
    }
    protected void ddlSubname_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
   
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSubcategory.Text = "";
        ddlCategory.SelectedIndex = 0;
    }
    protected void txtSub_TextChanged(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("SELECT Subcategory_Name FROM Subcategory Where Subcategory_Name LIKE '" + txtSub.Text.Trim() + "' + '%' ", con);
        da = new OleDbDataAdapter(cmd);
        da.Fill(ds, "Subcategory");
        gvSubcategory.DataSource = ds.Tables["Subcategory"];
        gvSubcategory.DataBind();
        con.Close();
    }
    protected void gvSubcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvSubcategory.SelectedRow;
        txtSub.Text = row.Cells[1].Text;
    }
}