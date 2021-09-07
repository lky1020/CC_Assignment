using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CC_Assignment
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        //DB
        string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;

        private string loginName;
        private string profilePicPath;
        private string userRole;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                //Auto insert username and password that get from param
                string username = Request.QueryString["username"];
                string password = Request.QueryString["password"];

                if (username != null && password != null)
                {
                    txtEmail_Username.Text = username;

                    txtPassword.TextMode = TextBoxMode.SingleLine;
                    txtPassword.Text = password;
                    txtPassword.Attributes["type"] = "password";

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Register Status", "alert('Successfully Registered')", true);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Prevent password gone after postback
            txtPassword.Attributes.Add("value", txtPassword.Text);
            base.OnPreRender(e);
        }

        private Boolean checkExistingUser()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE Name = '" + txtEmail_Username.Text + "' ", con);
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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean checkExistingEmail()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Email FROM [dbo].[User] WHERE Email = '" + txtEmail_Username.Text + "' ", con);
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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean checkPassword(string loginMethod)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da;
                    if (loginMethod.Equals("Name"))
                    {
                        da = new SqlDataAdapter("SELECT Name, Role, Password FROM [dbo].[User] WHERE " + "Name = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
                    }
                    else
                    {
                        da = new SqlDataAdapter("SELECT Name, Role, Password FROM [dbo].[User] WHERE " + "Email = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        loginName = dt.Rows[0]["Name"].ToString();
                        userRole = dt.Rows[0]["Role"].ToString();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean getProfilePicPath(string loginMethod)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da;
                    if (loginMethod.Equals("Name"))
                    {
                        da = new SqlDataAdapter("SELECT ProfileImg FROM [dbo].[User] WHERE " + "Name = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
                    }
                    else
                    {
                        da = new SqlDataAdapter("SELECT ProfileImg FROM [dbo].[User] WHERE " + "Email = '" + txtEmail_Username.Text + "' " + "AND Password = '" + txtPassword.Text + "' ", con);
                    }

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        profilePicPath = dt.Rows[0]["ProfileImg"].ToString();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private void ActivateProfileNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ProfileActive", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", loginName);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void DeactivateProfileNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", loginName);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void ActiveManagementNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActiveManagement", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void ActiveCustomerNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActiveCustomer", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "LoginpageDBError", "alert('Error Occur in Database. Please Contact Syasha Design!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        protected void cvEmail_Username_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (checkExistingUser() == false)
            {
                cvEmail_Username.ErrorMessage = "Username or Email Not Exist";
                args.IsValid = false;
            }
            else
            {
                cvEmail_Username.ErrorMessage = "";
                args.IsValid = true;
            }
        }

        protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string loginMethod = "";

            //Check db
            if (checkExistingUser() == true || checkExistingEmail() == true)
            {
                if (checkExistingUser() == true)
                {
                    //Username
                    loginMethod = "Name";
                }
                else
                {
                    //Email
                    loginMethod = "Email";
                }
            }

            if (checkPassword(loginMethod) == true)
            {

                //Get profile pic path
                if (getProfilePicPath(loginMethod) == true)
                {
                    //Activate the profile navigation + user account
                    ActivateProfileNavigation();

                    //Check whether the user is Management or not
                    //If, yes active the navigation to manage artworks
                    if (userRole.Equals("Management"))
                    {
                        ActiveManagementNavigation();
                    }
                    else
                    {
                        ActiveCustomerNavigation();
                    }

                    args.IsValid = true;

                    // Create a new ticket used for authentication
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // Ticket version
                        txtEmail_Username.Text, // username associated with ticket
                        DateTime.Now, // Date/time issued
                        DateTime.Now.AddDays(1), // Date/time to expire
                        true, // "true" for a persistent user cookie
                        userRole,
                        FormsAuthentication.FormsCookiePath); // Path cookie

                    // Encrypt the cookie using the machine key for secure transport
                    string hash = FormsAuthentication.Encrypt(ticket);

                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, // Name of auth cookie
                       hash); // Hashed ticket

                    // Set the cookie's expiration time to the tickets expiration time
                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }

                    // Add the cookie to the list for outgoing response
                    Response.Cookies.Add(cookie);

                    //Return to Homepage (To prevent returnurl been used)
                    Response.Redirect("Homepage.aspx");

                }
                //Fail to login
                else
                {
                    //Deactive the profile navigation + user account
                    DeactivateProfileNavigation();

                    //Active Customer Navigation (By Default)
                    ActiveCustomerNavigation();

                    args.IsValid = true;

                    //Return to Homepage
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Login Fail'); window.location ='Homepage.aspx';", true);

                }
            }
            else
            {
                cvPassword.ErrorMessage = "Invalid Password";
                args.IsValid = false;
            }
        }
    }
}