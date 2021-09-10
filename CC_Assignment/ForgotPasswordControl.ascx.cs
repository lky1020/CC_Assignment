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
    public partial class ForgotPasswordControl : System.Web.UI.UserControl
    {
        //DB
        string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;

        private string resetPin;
        private string username;
        private string userEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == true)
            {
                //Execute after get the user new password
                if (!hiddenResetPasswordValue.Value.Equals(""))
                {
                    //Get user's name from db
                    if (GetUsername() == true)
                    {
                        if (UpdateUserPasswordServer() == true)
                        {
                            //Reset the resetPin
                            UpdateResetPin();

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Password", "alert('Password Update Success, Proceed to Login Page');", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Direct to Login.aspx", "window.location = 'Login.aspx';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Password", "alert('Password Update Unsuccess, Proceed to Homepage');", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Direct to Login.aspx", "window.location = 'Homepage.aspx';", true);
                        }
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Email Entered
            if (Page.IsValid)
            {
                //Request PIN First
                if (txtResetPin.Text.Equals(""))
                {
                    //Check is exist email or not
                    if (checkExistingEmail() == true)
                    {
                        //Get the user reset pin
                        GetUserResetPin();

                        //Use team email to send reset pin to user
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("quadCoreTest@gmail.com");
                            mail.To.Add(userEmail);
                            mail.Subject = "Your Reset Pin";
                            mail.Body = "Hi " + username + ", this is your reset pin for your password: <b>" + resetPin + "</b><br/> Enter it to Reset Your Password.";
                            mail.IsBodyHtml = true;
                            mail.BodyEncoding = Encoding.UTF8;

                            using (SmtpClient smtp = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587)))
                            {
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                                smtp.EnableSsl = true;

                                try
                                {
                                    smtp.Send(mail);
                                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Pin", "alert('Reset PIN Sent. Please Check Your Email!');", true);
                                    btnReset.Text = "Enter New Password";
                                }
                                catch (Exception)
                                {
                                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Pin", "alert('Sorry, Syasya Design Email Account Down!');", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Pin", "alert('Email Not Exist!!! Please Enter Again');", true);
                    }
                }
                //Enter PIN by user without know the pin
                else if(!txtResetPin.Text.Equals("") && btnReset.Text.Equals("Request Pin"))
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Error PIN", "alert('Please Put the PIN Empty before Email Been Sent!!');", true);
                }
                //PIN Entered
                else
                {
                    //Call the get PIN function first, to prevent error when user did not click request pin btn
                    GetUserResetPin();

                    //Validate the PIN
                    if (txtResetPin.Text.Equals(resetPin))
                    {
                        if (btnReset.Text.Equals("Enter New Password"))
                        {
                            //Will execute when the the user want to enter new password
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Password", "var newPassword = window.prompt('Please Enter Your New Password: '); storeResetPassword(newPassword);", true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Confirm Reset Password", "alert('Click Below Reset Password Button to Confirm');", true);
                            btnReset.Text = "Reset Password";
                        }
                    }
                    else
                    {
                        //To prevent alert after user enter new password
                        if (hiddenResetPasswordValue.Value.Equals(""))
                        {
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Reset Pin", "alert('PIN Incorrect')", true);
                        }
                    }
                }
            }
            else
            {
                btnReset.Text = "Request Pin";
            }
        }

        private Boolean GetUserResetPin()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Name, Email, ResetPin FROM [dbo].[User] WHERE " + "Email = '" + txtEmail.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        username = dt.Rows[0]["Name"].ToString();
                        userEmail = dt.Rows[0]["Email"].ToString();
                        resetPin = dt.Rows[0]["ResetPin"].ToString();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ForgotPasswordpageDBError", "alert('Error Occur in Database. Please Contact Syasya Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;

        }

        private Boolean GetUsername()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da;

                    da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE " + "Email = '" + txtEmail.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        username = dt.Rows[0]["Name"].ToString();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ForgotPasswordpageDBError", "alert('Error Occur in Database. Please Contact Syasya Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean checkExistingEmail()
        {
            if (!txtEmail.Text.Equals(""))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT Email FROM [dbo].[User] WHERE Email = '" + txtEmail.Text + "' ", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count >= 1)
                        {
                            return true;
                        }

                        return false;
                    }
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ForgotPasswordpageDBError", "alert('Error Occur in Database. Please Contact Syasya Design!');", true);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                }
            }

            //Email invalid
            return false;
        }

        private Boolean UpdateUserPasswordServer()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE[dbo].[User] " +
                                            "SET[Password] = '" + hiddenResetPasswordValue.Value + "' " +
                                            "WHERE [Name] = '" + username + "' ", con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    con.Close();

                    if (k != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ForgotPasswordpageDBError", "alert('Error Occur in Database. Please Contact Syasya Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean UpdateResetPin()
        {
            //Generate 6 digit random number for the new resetPin
            Random generator = new Random();
            string resetPin = generator.Next(0, 1000000).ToString("D6");

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE[dbo].[User] " +
                                            "SET[ResetPin] = '" + resetPin + "' " +
                                            "WHERE [Name] = '" + username + "' ", con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    con.Close();

                    if (k != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ForgotPasswordpageDBError", "alert('Error Occur in Database. Please Contact Syasya Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        protected void cvEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                cvEmail.ErrorMessage = "Invalid Email Format!";
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}