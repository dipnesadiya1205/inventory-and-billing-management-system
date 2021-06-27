using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class PrintBill : System.Web.UI.Page
{

    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\Project\Dip_Project.accdb");
    OleDbCommand cmd;
    OleDbDataAdapter da;
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
                lblBno.Text = Session["bill_no"].ToString();
                con.Open();
                cmd = new OleDbCommand("SELECT p.Product_Name,d.CGST,d.SGST,d.RATE,d.QUANTITY,d.DISCOUNT,d.AMOUNT FROM Product_Detail d,Product p where d.Product_ID=p.Product_ID and d.Bill_No = " + Convert.ToInt32(lblBno.Text) + " ", con);
                da = new OleDbDataAdapter(cmd);
                da.Fill(ds, "Product_Detail");
                gvBillDisplay.DataSource = ds.Tables["Product_Detail"];
                gvBillDisplay.DataBind();
                con.Close();
            }


            lblCustname.Text = Session["cust_name"].ToString();
            lblMobno.Text = Session["m_no"].ToString();
            lblTotalamt.Text = Session["total_amount"].ToString();
            lblBilldate.Text = DateTime.Now.ToString();
        }
    }
   

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        this.Page.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0.0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();        
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }
}