using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;

namespace CC_Assignment
{
    public partial class Payment : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
        double totalPay = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                payment_refreshdata();
               
                double paySubtotal = subtotalPayment();
                pay_subtotal.Text = "RM " + paySubtotal.ToString("F");

                totalPay += subtotalPayment() + (gvPayment.Rows.Count * 3);
                delivery_fees.Text = (gvPayment.Rows.Count * 3).ToString("F");
                total_payment.Text = totalPay.ToString("F");

                //set validate date for expiry card
                ExpDate_RangeValidator.MinimumValue = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + (DateTime.Now.Day + 1).ToString();
                ExpDate_RangeValidator.MaximumValue = (DateTime.Now.Year + 10).ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                
            }
            else
                payment_refreshdata();

        }
        
        public void payment_refreshdata()
        {
            try
            {
                //assign selected item to gridview
                SqlConnection con = new SqlConnection(cs);
                con.Open();
                String queryGetData = "Select s.Id, s.Name, s.Price, s.Size, o.OrderDetailId, o.qtySelected, o.Subtotal from [OrderDetails] o " +
                        "INNER JOIN [Seller] s on o.ApparelId = s.Id INNER JOIN [Cart] c on o.CartId = c.CartId Where c.UserId = @userid AND c.status = 'cart' AND o.Checked = 'True'";
                SqlCommand cmd = new SqlCommand(queryGetData, con);
                cmd.Parameters.AddWithValue("@userid", Session["userID"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                gvPayment.DataSource = dt;
                gvPayment.DataBind();

                con.Close();
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Server down, please contact Syasya Design. Customer Services.')</script>");
            }
        }

       public double subtotalPayment()
        {
            double total_Payment = 0;

            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
               total_Payment += double.Parse((gvPayment.Rows[i].FindControl("order_subtotal") as TextBox).Text.ToString());

            }
            return total_Payment;
        }

        protected void pay_Btn_Click(object sender, EventArgs e)
        {
            //validate cart num with card type
            if (!validatedCardNumWithType())
            {
                Response.Write("<script>alert('Please check your card type. Card number validation according to card type failed. (E.g. 4567 8738 4738 4596 for Credit Card & 5567 8738 4738 4596 for Debit Card)')</script>");

            }
            else
            {
                SqlConnection con = new SqlConnection(cs);
                con.Open();

                try
                {
                    //update selected item with the cartid 
                    for (int i = 0; i < gvPayment.Rows.Count; i++)
                    {
                        String queryUpdate = "Update OrderDetails SET CartId=@cartid, Subtotal=@subtotal WHERE OrderDetailId =@detailid";

                        SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con);

                        double deliverFees = double.Parse((gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim()) * 3;
                        double subtotal = deliverFees + double.Parse((gvPayment.Rows[i].FindControl("order_subtotal") as TextBox).Text.Trim());

                        cmdUpdate.Parameters.AddWithValue("@cartid", Session["pendingCart"]);
                        cmdUpdate.Parameters.AddWithValue("@subtotal", subtotal);
                        cmdUpdate.Parameters.AddWithValue("@detailid", Convert.ToInt32(gvPayment.DataKeys[i].Value.ToString()));
                        cmdUpdate.ExecuteNonQuery();

                        gvPayment.EditIndex = -1;
                    }
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Server down, please contact Syasya Design. Customer Services.')</script>");
                }

                //insert data to payment table 
                string cardType = "Debit";
                if (CardRadioButtonList.SelectedIndex == 1)
                {
                    cardType = "Credit";
                }

                //calculate total payment
                totalPay += subtotalPayment() + (gvPayment.Rows.Count * 3);

                try
                {
                    string sqlPayment = "INSERT into Payment (cartid, datepaid, total,cardType) values('" + Session["pendingCart"] + "','" + DateTime.Now.ToString() + "','" + totalPay + "','" + cardType + "')";

                    SqlCommand cmdPayment = new SqlCommand();

                    cmdPayment.Connection = con;
                    cmdPayment.CommandType = CommandType.Text;
                    cmdPayment.CommandText = sqlPayment;
                    cmdPayment.ExecuteNonQuery();

                    for (int i = 0; i < gvPayment.Rows.Count; i++)
                    {
                        String queryUpdateCart = "Update Cart SET status = 'ordered' WHERE CartId=@cartid";

                        SqlCommand cmdUpdate = new SqlCommand(queryUpdateCart, con);

                        cmdUpdate.Parameters.AddWithValue("@cartid", Session["pendingCart"]);

                        cmdUpdate.ExecuteNonQuery();

                        gvPayment.EditIndex = -1;
                    }


                    //retrieve art qty left 
                    int quantity = 0;
                    for (int i = 0; i < gvPayment.Rows.Count; i++)
                    {
                        quantity = 0;
                        string queryApparelQty = "SELECT Quantity FROM Seller WHERE Id = (SELECT ApparelId FROM OrderDetails WHERE OrderDetailId = @od_Id); ";

                        using (SqlCommand cmdApparelQty = new SqlCommand(queryApparelQty, con))
                        {
                            cmdApparelQty.Parameters.AddWithValue("@od_Id", gvPayment.DataKeys[i].Value.ToString());
                            quantity = ((Int32?)cmdApparelQty.ExecuteScalar()) ?? 0;
                            quantity -= int.Parse((gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim());

                        }

                        //update art qty left 
                        String queryUpdateQty = "Update Seller SET Quantity = @qty WHERE Id = (SELECT ApparelId FROM OrderDetails WHERE OrderDetailId = @od_Id);";
                        SqlCommand cmdUpdateApparelQty = new SqlCommand(queryUpdateQty, con);

                        cmdUpdateApparelQty.Parameters.AddWithValue("@qty", quantity);
                        cmdUpdateApparelQty.Parameters.AddWithValue("@od_Id", gvPayment.DataKeys[i].Value.ToString());

                        cmdUpdateApparelQty.ExecuteNonQuery();

                        gvPayment.EditIndex = -1;
                        

                    }
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Something went wrong, please login and try again, thank you.')</script>");
                }
                con.Close();

                //send email then redirect to payment history
                sendEmail();

            }
        }


        //validate card num with card type
        private bool validatedCardNumWithType()
        {
            string str = Card_Number.Text.ToString();

            if ((CardRadioButtonList.SelectedIndex == 0 && str[0] == '5') || (CardRadioButtonList.SelectedIndex == 1 && str[0] == '4'))
            {
                return true;
            }
            return false;
        }

        //send receipt to customer through email
        private void sendEmail()
        {
            if (Page.IsValid) { 
                try
                {
                    String emailOrderInfo = "";
                    String apparelName, unitPrice, qty;
                    for (int i = 0; i < gvPayment.Rows.Count; i++)
                    {
                        apparelName = (gvPayment.Rows[i].FindControl("Item_Name") as TextBox).Text.Trim();
                        unitPrice = (gvPayment.Rows[i].FindControl("item_order_summary_price") as TextBox).Text.Trim();
                        qty = (gvPayment.Rows[i].FindControl("item_order_summary_qty") as TextBox).Text.Trim();
                        emailOrderInfo += "<br/><br/>" + (i + 1).ToString() + ". Apparel Name : " + apparelName + "<br/>&nbsp;&nbsp;&nbsp;&nbsp;Details  : RM " + unitPrice + " x " + qty;

                    }
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();
                            // for product details
                            sb.Append("<h4 style='text-align: center;'><b>Syasya Design</b>" +
                                "<br/>========================</h4>" +
                                "<br/><b><u>Purchase Information</u></b>" + emailOrderInfo +
                                "<br/><br/><br/>----------------------------");
                            // for summary
                            sb.Append(@"<table>
                                        <thead><tr><th><b>Order Summary</b></th><th></th><th style='text-align:right;'><b>RM</b></th></tr></thead>
                                        <tbody><tr><td>----------------------------</td><td style='text-align:center;'>---</td><td style='text-align:right;'>----</td></tr><tr>
                                                   <td>Delivery Fees</td><td style='text-align:center;'>:</td><td style='text-align:right;'>" + delivery_fees.Text + "</td></tr><tr>" +
                                                  "<td><u>Total</u></td><td style='text-align:center;'>:</td><td style='text-align:right;'><u>" + total_payment.Text + "</u></td></tr></tbody></table>" +
                                                  "<br/><br/><br/>  Thank you!");
                            StringReader sr = new StringReader(sb.ToString());

                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                pdfDoc.Open();
                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                byte[] bytes = memoryStream.ToArray();
                                memoryStream.Close();

                                MailMessage mm = new MailMessage("quadCoreTest@gmail.com", "syasyadesigncc@gmail.com");
                                mm.Subject = "Syasya Design Apparel Gallery Receipt";
                                mm.Body = "Thanks for your order! <br/>Your total payment is: " + total_payment.Text +
                                    "<br/><br/>Details of the payment information is in the PDF document below." +
                                "<br/><br/><br/>  Thank you!" +
                                "<br/><br/> Receipt Generated By: Syasya Design Auto System"; ;
                                mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Syasya_Design_Apparel_Gallery_Receipt.pdf"));
                                mm.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.sendgrid.net";
                                smtp.EnableSsl = true;
                                NetworkCredential NetworkCred = new NetworkCredential();
                                NetworkCred.UserName = "apikey";
                                NetworkCred.Password = "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";
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = NetworkCred;
                                smtp.Port = 587;
                                try
                                {
                                    smtp.Send(mm);

                                    //pop up massage then redirect to payment history
                                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                                                        "Email Status",
                                                        "alert('Your receipt has been sent to your email.');window.location ='PayHistory.aspx';",
                                                        true);

                                }
                                catch (Exception)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Email Status", "alert('Sorry, Syasya Design Email Account Down. Please Contact Syasya Design!')", true);
                                }
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Server down, please contact Syasya Design. Customer Service to obtain your digital receipt.')</script>");
                }
            }
        }

    }
}