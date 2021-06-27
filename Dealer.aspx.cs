using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class Dealer : System.Web.UI.Page
{

    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd,cmd1;
    OleDbDataAdapter da,da1;
    DataSet ds = new DataSet();
    OleDbDataReader dr;
    int d_id;
    int i;

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
                cmd = new OleDbCommand("Select MAX(Dealer_ID) as d_id From Dealer_Master", con);
                da = new OleDbDataAdapter(cmd);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetValue(0) != DBNull.Value)
                    {
                        d_id = Convert.ToInt32(dr.GetValue(0));
                        d_id = d_id + 1;
                    }
                    else
                    {
                        d_id = 1;
                    }
                }
                txtDealerID.Text = d_id.ToString();
                con.Close();
            }
        }
    }

    void clear_data()
    {
        txtDealerAddress.Text = "";
        txtDealerName.Text = "";
        txtDelaerMobNo.Text = "";
    }

    protected void gvDealer_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvDealer.SelectedRow;
        txtDealerID.Text = row.Cells[1].Text;
        txtDealerName.Text = row.Cells[2].Text;
        txtDelaerMobNo.Text = row.Cells[3].Text;
        txtDealerAddress.Text = row.Cells[4].Text;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        con.Open();

        cmd1 = new OleDbCommand("select count(*) from Dealer_Master where Dealer_Name='" + txtDealerName.Text.ToUpper() + "' OR Mob_No = " + Convert.ToDouble(txtDelaerMobNo.Text) + " ", con);
        da1 = new OleDbDataAdapter(cmd1);
        i = Convert.ToInt32(cmd1.ExecuteScalar());

        if (i == 0)
        {
            cmd = new OleDbCommand("insert into Dealer_Master(Dealer_ID,Dealer_Name,Mob_No,Address) values (" + Convert.ToInt32(txtDealerID.Text) + ",'" + txtDealerName.Text.ToUpper() + "'," + Convert.ToDouble(txtDelaerMobNo.Text) + ",'" + txtDealerAddress.Text.ToUpper() + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("INSERT SUCCESSFULLY");
        }
        else
        {
            Response.Write("<script>alert('Mobile number or Dealer name is already exist !!!!')</script>");
        }
            clear_data();
    }
    protected void txtDealerSearch_TextChanged(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("SELECT * FROM Dealer_Master WHERE Dealer_Name LIKE '" + txtDealerSearch.Text.Trim() + "' + '%'", con);
        da = new OleDbDataAdapter(cmd);
        da.Fill(ds, "Dealer_Master");
        OleDbDataReader dr = cmd.ExecuteReader();
        gvDealer.DataSource = ds.Tables["Dealer_Master"];
        gvDealer.DataBind();        
        con.Close();
        clear_data();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("update Dealer_Master set Dealer_Name='" + txtDealerName.Text.ToUpper() + "',Mob_No=" + Convert.ToDouble(txtDelaerMobNo.Text) + ",Address='" + txtDealerAddress.Text.ToUpper() + "' where Dealer_ID=" + Convert.ToInt32(txtDealerID.Text) + "", con);
        cmd.ExecuteNonQuery();
        con.Close();
        MessageBox.Show("UPDATED SUCCESSFULLY");
        clear_data();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("delete from Dealer_Master where Dealer_ID=" + Convert.ToInt32(txtDealerID.Text) + "", con);
        cmd.ExecuteNonQuery();
        con.Close();
        MessageBox.Show("DELETED SUCCESSFULLY");
        clear_data(); 
    }
}