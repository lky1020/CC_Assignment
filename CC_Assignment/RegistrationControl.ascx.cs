using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CC_Assignment
{
    public partial class RegistrationControl : System.Web.UI.UserControl
    {
        //DB
        string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            string successRegister = Request.QueryString["successRegister"];

            if (IsPostBack == true)
            {

                //Initialize the Gender
                if (rdMale.Checked)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeGender", "selectMale();", true);
                }
                else if (rdFemale.Checked)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeGender", "selectFemale();", true);
                }

                //Initialize the Role
                if (rdManagement.Checked)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeRole", "selectManagement();", true);
                }
                else if (rdCustomer.Checked)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "initializeRole", "selectCustomer();", true);
                }
            }

            // Proceed after success register
            if (successRegister != null)
            {
                if (successRegister.Equals("false"))
                {
                    // Remain in the registration if register fail
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Register Status", "alert('UnSuccessfully Registered')", true);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //Prevent password gone after postback
            txtPassword.Attributes.Add("value", txtPassword.Text);
            base.OnPreRender(e);
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                // Insert the data to db and proceed
                insertRegisterData();
            }
        }

        private void insertRegisterData()
        {
            string gender;

            if (rdMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            string role;

            if (rdManagement.Checked)
            {
                role = "Management";
            }
            else
            {
                role = "Customer";
            }

            Random generator = new Random();
            string resetPin = generator.Next(0, 1000000).ToString("D6");

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_registerUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", txtUsername.Text);
                    cmd.Parameters.AddWithValue("password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("gender", gender);
                    cmd.Parameters.AddWithValue("profilePic", "../img/userPic/user_default.png");   //initialize the profile pic
                    cmd.Parameters.AddWithValue("resetPin", resetPin);
                    cmd.Parameters.AddWithValue("role", role);

                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    con.Close();
                    if (k != 0)
                    {
                        //Pass param to login.aspx, so user not need to type it username and password again
                        Response.Redirect("~/login.aspx?username=" + txtUsername.Text + "&password=" + txtPassword.Text);
                    }
                    else
                    {
                        Response.Redirect("~/registration.aspx?username=" + txtUsername.Text + "&password=" + txtPassword.Text + "&successRegister=false");
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "RegistrationpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

        }

        private Boolean checkExistingUser()
        {
            if (!txtUsername.Text.Equals(""))
            {
                //Check whether username already exist
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT Name FROM [dbo].[User] WHERE Name = '" + txtUsername.Text + "' ", con);
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
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "RegistrationpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                }
            }

            //return true means it is invalid
            return true;
        }

        private Boolean checkExistingEmail()
        {
            //Check whether email already exist
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
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "RegistrationpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                }
            }

            //return true means it is invalid
            return true;
        }

        protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Check the username
            if (checkExistingUser() == true)
            {
                cvUsername.ErrorMessage = "Username already exist!";
                args.IsValid = false;
            }
            else
            {
                cvUsername.ErrorMessage = "";
                args.IsValid = true;
            }
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
                //Check the Email
                if (checkExistingEmail() == true)
                {
                    cvEmail.ErrorMessage = "Email already exist!";
                    args.IsValid = false;
                }
                else
                {
                    cvEmail.ErrorMessage = "";
                    args.IsValid = true;
                }
            }
        }
    }
}