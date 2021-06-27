using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public partial class bill : System.Web.UI.Page
{
    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd, cmd1,cmd2,cmd3;
    OleDbDataAdapter da,da1,da2,da3;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    OleDbDataReader dr;
    int cust_id = 0;
    int b_id;    
    int available_qty;
    int selling_qty;
    int remain_qty;
    int qty;

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
                cmd = new OleDbCommand("SELECT * FROM Subcategory", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Subcategory");
                ddlSname.DataSource = ds.Tables["Subcategory"];
                ddlSname.DataTextField = "Subcategory_Name";
                ddlSname.DataValueField = "Subcategory_ID";
                ddlSname.DataBind();
                ddlSname.Items.Insert(0, new ListItem("--Select--", "-1"));


                cmd1 = new OleDbCommand("Select MAX(Bill_No) as max_bid From Bill_Master", con);
                da1 = new OleDbDataAdapter(cmd1);
                dr = cmd1.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetValue(0) != DBNull.Value)
                    {
                        b_id = Convert.ToInt32(dr.GetValue(0));
                        b_id = b_id + 1;
                    }
                    else
                    {
                        b_id = 1;
                    }
                }
                txtBillno.Text = b_id.ToString();
            }
            txtDate.Text = DateTime.Now.Date.ToShortDateString();
        }
    }    

    public void refresh()
    {
        ddlPname.SelectedIndex=0;
        ddlSname.SelectedIndex=0;
        txtAmount.Text="";
        txtCgst.Text="";
        txtDiscount.Text="";
        txtQty.Text="";
        txtSgst.Text="";
        txtRate.Text="";
    }
 
    protected void ddlSname_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmd = new OleDbCommand("SELECT * FROM Product where Subcategory_ID = " + ddlSname.SelectedValue + " ", con);
        da = new OleDbDataAdapter(cmd);
        da.Fill(ds1, "Product");
        ddlPname.DataSource = ds1.Tables["Product"];
        ddlPname.DataTextField = "Product_Name";
        ddlPname.DataValueField = "Product_ID";
        ddlPname.DataBind();
        ddlPname.Items.Insert(0, new ListItem("--Select--", "-1"));
        con.Close();

        con.Open();
        cmd1 = new OleDbCommand("SELECT CGST,SGST FROM Category WHERE Category_ID = (SELECT Category_ID FROM Subcategory where Subcategory_ID = " + ddlSname.SelectedValue + ") ", con);
        OleDbDataReader dr = cmd1.ExecuteReader();
        if (dr.Read())
        {
            txtCgst.Text = dr["CGST"].ToString();
            txtSgst.Text = dr["SGST"].ToString();
        }
        con.Close();
    }
    protected void ddlPname_SelectedIndexChanged(object sender, EventArgs e)
    {          
        con.Open();
        cmd = new OleDbCommand("SELECT Rate FROM Product WHERE Product_ID = " + ddlPname.SelectedValue + " ", con);
        OleDbDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            txtRate.Text = dr["Rate"].ToString();
        }
        con.Close();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        qty = Convert.ToInt32(txtQty.Text);

        con.Open();
        cmd3 = new OleDbCommand("SELECT Qty FROM Product WHERE Product_ID = " + ddlPname.SelectedValue + " ", con);
        OleDbDataReader dr = cmd3.ExecuteReader();
        if (dr.Read())
        {
            available_qty = Convert.ToInt32(dr["Qty"].ToString());
        }
        selling_qty = Convert.ToInt32(txtQty.Text);
        remain_qty = available_qty - selling_qty;
        if (remain_qty >= 0 && qty > 0)
        {
            cmd1 = new OleDbCommand("UPDATE Product SET Qty = " + remain_qty + " WHERE Product_ID = " + ddlPname.SelectedValue + " ", con);
            da1 = new OleDbDataAdapter(cmd1);
            cmd1.ExecuteNonQuery();

            cmd = new OleDbCommand("Insert into Product_Detail (PRODUCT_ID,CGST,SGST,RATE,QUANTITY,DISCOUNT,AMOUNT,Bill_No) VALUES (" + Convert.ToInt32(ddlPname.SelectedValue.ToString()) + ", '" + txtCgst.Text + "','" + txtSgst.Text + "','" + txtRate.Text + "','" + txtQty.Text + "','" + txtDiscount.Text + "','" + txtAmount.Text + "'," + Convert.ToInt32(txtBillno.Text) + ")", con);
            da = new OleDbDataAdapter(cmd);
            cmd.ExecuteNonQuery();


            cmd1 = new OleDbCommand("SELECT p.Product_Name,d.CGST,d.SGST,d.RATE,d.QUANTITY,d.DISCOUNT,d.AMOUNT FROM Product_Detail d,Product p where d.Product_ID=p.Product_ID and d.Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ", con);
            da1 = new OleDbDataAdapter(cmd1);
            cmd2 = new OleDbCommand("SELECT Sum(Val(AMOUNT)) FROM Product_Detail Where Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ", con);
            da2 = new OleDbDataAdapter(cmd2);
            da1.Fill(ds, "Product_Detail");
            da2.Fill(ds2, "Product_Detail");
            gvShow.DataSource = ds.Tables["Product_Detail"];
            gvShow.DataBind();

            //calculate total amount

            string query = "SELECT Sum(Val(AMOUNT)) FROM Product_Detail Where Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ";
            using (System.Data.IDbCommand command = new System.Data.OleDb.OleDbCommand(query, con))
            {
                object result = command.ExecuteScalar();
                txtPamount.Text = Convert.ToString(result);
            }

        }
        else
        {
            MessageBox.Show("Check Your Quantity Field");
        }
        
        //cmd = new OleDbCommand("Insert into Product_Detail (PRODUCT_ID,CGST,SGST,RATE,QUANTITY,DISCOUNT,AMOUNT,Bill_No) VALUES (" + Convert.ToInt32(ddlPname.SelectedValue.ToString()) + ", '" + txtCgst.Text + "','" + txtSgst.Text + "','" + txtRate.Text + "','" + txtQty.Text + "','" + txtDiscount.Text + "','" + txtAmount.Text + "'," + Convert.ToInt32(txtBillno.Text) + ")", con);
        //da = new OleDbDataAdapter(cmd);
        //cmd.ExecuteNonQuery();


        //cmd1 = new OleDbCommand("SELECT p.Product_Name,d.CGST,d.SGST,d.RATE,d.QUANTITY,d.DISCOUNT,d.AMOUNT FROM Product_Detail d,Product p where d.Product_ID=p.Product_ID and d.Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ", con);
        //da1 = new OleDbDataAdapter(cmd1);
        //cmd2 = new OleDbCommand("SELECT Sum(Val(AMOUNT)) FROM Product_Detail Where Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ", con);
        //da2 = new OleDbDataAdapter(cmd2);
        //da1.Fill(ds, "Product_Detail");
        //da2.Fill(ds2, "Product_Detail");
        //gvShow.DataSource = ds.Tables["Product_Detail"];
        //gvShow.DataBind();

        ////calculate total amount

        //string query = "SELECT Sum(Val(AMOUNT)) FROM Product_Detail Where Bill_No = " + Convert.ToInt32(txtBillno.Text) + " ";
        //using (System.Data.IDbCommand command = new System.Data.OleDb.OleDbCommand(query, con))
        //{
        //    object result = command.ExecuteScalar();
        //    txtPamount.Text = Convert.ToString(result);
        //}

        con.Close();
        refresh();
        txtDiscount.Visible = false;
        ddlDiscount.SelectedIndex = 0;
       
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        refresh();        
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("SELECT Qty FROM Product WHERE Product_ID = " + ddlPname.SelectedValue + " ", con);
        OleDbDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            available_qty = Convert.ToInt32(dr["Qty"].ToString());
        }
        selling_qty = Convert.ToInt32(txtQty.Text);
        remain_qty = available_qty - selling_qty;

        if (remain_qty < 0)
        {            
            MessageBox.Show("Available Quantity : " + available_qty);
        }
       
        con.Close();        
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        if (txtDiscount.Visible == true)
        {
            double cgst = Convert.ToDouble(txtCgst.Text);
            double sgst = Convert.ToDouble(txtSgst.Text);
            double rate = Convert.ToDouble(txtRate.Text);
            double qty = Convert.ToDouble(txtQty.Text);
            double dis = Convert.ToDouble(txtDiscount.Text);
            double a;
            double b;
            double c;

            a = rate + (rate * (cgst / 100)) + (rate * (sgst / 100));
            b = a - ((a * dis) / 100);
            c = b * qty;

            txtAmount.Text = Convert.ToString(c);
        }       
    }    

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("Update Bill_Master Set Total_amount = " + Convert.ToDouble(txtPamount.Text) + " Where Bill_No = " + Convert.ToInt32(txtBillno.Text) + "", con);
        da = new OleDbDataAdapter(cmd);
        cmd.ExecuteNonQuery();
        con.Close();
        Session["bill_no"] = txtBillno.Text;
        Session["cust_name"] = txtName.Text;
        Session["m_no"] = txtNo.Text;
        Session["total_amount"] = txtPamount.Text;

        if (ddlCustname.SelectedIndex == 0)
        {
            Session["cust_name"] = txtName.Text;
        }
        else
        {
            Session["cust_name"] = ddlCustname.SelectedItem;
        }
        Response.Redirect("PrintBill.aspx");        
    }

    protected void ddlDiscount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(Convert.ToInt32(ddlDiscount.SelectedValue)==1)
        {
            txtDiscount.Visible = true;
        }
        else if(Convert.ToInt32(ddlDiscount.SelectedValue) == 2)
        {
                txtDiscount.Visible = false;
                double cgst = Convert.ToDouble(txtCgst.Text);
                double sgst = Convert.ToDouble(txtSgst.Text);
                double rate = Convert.ToDouble(txtRate.Text);
                double qty = Convert.ToDouble(txtQty.Text);
                double a;
                double c;

                a = rate + (rate * (cgst / 100)) + (rate * (sgst / 100));
                c = a * qty;

                txtDiscount.Text = "0.00";
                txtAmount.Text = Convert.ToString(c);               
        }
        else
        {
           MessageBox.Show("Select Yes or No option in Discount Box");
        }
    }

    protected void btnCsave_Click(object sender, EventArgs e)
    {
        con.Open();
        cmd = new OleDbCommand("Select Cust_ID From Cust_Detail Where (MOB_NO='" + txtNo.Text.Trim() + "')", con);//(Cust_Name='" + txtName.Text.Trim() + "') or (MOB_NO='" + txtNo.Text.Trim() + "')", con);
        da = new OleDbDataAdapter(cmd);
        dr = cmd.ExecuteReader();
        
        if (dr.HasRows == false)
        {                       
            cmd1 = new OleDbCommand("Insert into Cust_Detail(Cust_Name,MOB_NO) Values ('" + txtName.Text.Trim().ToUpper() + "','" + txtNo.Text + "') ", con);
            da1 = new OleDbDataAdapter(cmd1);
            cmd1.ExecuteNonQuery();         
        }
        else if(dr.HasRows==true)
        {
            if (dr.Read())
            {
                cust_id = Convert.ToInt32(dr["Cust_ID"].ToString());
                Response.Write("<script> alert('Customer is already exists ..... !!!!!!') </script> ");

                if (ddlCustname.SelectedIndex == 0)
                {
                    cmd3 = new OleDbCommand("INSERT INTO Bill_Master(Bill_No,Cust_ID,Date_of_Purchase) VALUES (" + Convert.ToInt32(txtBillno.Text) + "," + cust_id + ",'" + txtDate.Text + "')", con);
                    da3 = new OleDbDataAdapter(cmd3);
                    cmd3.ExecuteNonQuery();
                }
                else
                {
                    cmd3 = new OleDbCommand("INSERT INTO Bill_Master(Bill_No,Cust_ID,Date_of_Purchase) VALUES (" + Convert.ToInt32(txtBillno.Text) + ",'" + ddlCustname.SelectedValue + "','" + txtDate.Text + "')", con);
                    da3 = new OleDbDataAdapter(cmd3);
                    cmd3.ExecuteNonQuery();
                }
            }
        }
        con.Close(); 
    }
    protected void txtNo_TextChanged(object sender, EventArgs e)
    {
       
        con.Open();
        cmd = new OleDbCommand("select * from Cust_Detail where MOB_NO='" + txtNo.Text + "' ", con);
       

            da = new OleDbDataAdapter(cmd);
            da.Fill(ds3, "Cust_Detail");
            ddlCustname.DataSource = ds3.Tables["Cust_Detail"];
            ddlCustname.DataTextField = "Cust_Name";
            ddlCustname.DataValueField = "Cust_ID";
            ddlCustname.DataBind();
            ddlCustname.Items.Insert(0, new ListItem("--Select--", "-1"));
        
        con.Close();
       
    }
}