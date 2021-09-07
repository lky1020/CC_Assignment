using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CC_Assignment
{
    public partial class Homepage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // For the artwork
            bindList();
        }

        protected void btnContactSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("quadCoreTest@gmail.com");

                    // Take the user logged in email if userEmail not null
                    if(Session["userEmail"] != null)
                    {
                        mail.To.Add(Session["userEmail"].ToString());
                    }
                    else
                    {
                        //Get the user typed email
                        mail.To.Add(txtContactEmail.Text);
                    }

                    mail.To.Add("quadCoreTest@gmail.com");
                    mail.Subject = "Customer's Comment";

                    if(Session["username"] != null && Session["userEmail"] != null)
                    {
                        mail.Body = txtContactComment.Text + "<br/> From: " + Session["username"].ToString() + " (" + Session["userEmail"].ToString() + ")";
                    }
                    else
                    {
                        mail.Body = txtContactComment.Text + "<br/> From: " + txtContactName.Text + " (" + txtContactEmail.Text + ")";
                    }

                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.UTF8;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("quadCoreTest@gmail.com", "quad_core");
                        smtp.EnableSsl = true;

                        try
                        {
                            smtp.Send(mail);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Email Status", "alert('Email Sent')", true);
                            txtContactName.Text = "";
                            txtContactEmail.Text = "";
                            txtContactComment.Text = "";
                            btnContactSubmit.Text = "Submit";
                            btnContactSubmit.Style.Add("cursor", "pointer");
                        }
                        catch (Exception)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Email Status", "alert('Sorry, Quad-Core ASG Email Account Down. Please Contact Quad-Core AWS!')", true);
                        }
                    }
                }
            }
        }


        private void bindList()
        {
            string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    //sorting feature
                    SqlDataAdapter da = new SqlDataAdapter("Select TOP 6 * from Seller", con);


                    con.Open();
                    da.Fill(dt);

                    con.Close();
                }

                //paging feature
                DataListPaging(dt);

            }
            catch(Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "HomepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }

        }

        private void DataListPaging(DataTable dt)
        {
            //PagedDataSource setting
            PagedDataSource PD = new PagedDataSource();

            PD.DataSource = dt.DefaultView;
            ProductDataList.DataSource = PD;
            ProductDataList.DataBind();

        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            if (Session["userRole"] != null)
            {
                //Direct to gallery
                if (Session["userRole"].ToString().Equals("Management"))
                {
                    Response.Write("<script>window.location = 'ViewCollections.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>window.location = 'Product.aspx';</script>");
                }
            }
            else
            {
                Response.Write("<script>window.location = 'Product.aspx';</script>");
            }

        }

        protected void cvContactEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!Regex.IsMatch(txtContactEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                cvContactEmail.ErrorMessage = "Invalid Email Format!";
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}