using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class login : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd,cmd1;
    OleDbDataAdapter da,da1;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        int i;
        con.Open();
        cmd = new OleDbCommand("Select Count(*) From Admin Where Username = '" + txtUsername.Text + "' AND Password_1 = '" + txtPassword_1.Text + "'", con);
        da = new OleDbDataAdapter(cmd);
        i = Convert.ToInt32(cmd.ExecuteScalar());
        if (i == 1)
        {
            Session["user"] = txtUsername.Text;
            Session["pass"] = txtPassword_1.Text;
            Response.Redirect("product.aspx");
            MessageBox.Show("Login Successfully");
        }
        else
        {
            Response.Write("<script>alert('Login Fail !!!!!!!')</script>");
        }
        con.Close();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtUsername.Text = " ";
        txtPassword_1.Text = " ";
    }
    protected void lblForgotpassword_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int i;
        con.Open();
        cmd = new OleDbCommand("Select Count(*) From Admin Where Username = '" + txtUsername0.Text + "' AND Password_1 = '" + txtCurrentpass.Text + "'", con);
        da = new OleDbDataAdapter(cmd);
        i = Convert.ToInt32(cmd.ExecuteScalar());
        if (i == 1)
        {
            cmd1 = new OleDbCommand("update Admin set Password_1 = '" + txtNewpass.Text + "' where Username = '" + txtUsername0.Text + "'", con);
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Updated Successfully !!!");
            Response.Redirect("Login.aspx");
        }
        con.Close();
    }
    protected void lbtnChangepass_Click(object sender, EventArgs e)
    {
        pnlLogin.Visible = false;
        pnlChangepass.Visible = true;
    }
}