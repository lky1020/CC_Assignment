using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;

namespace CC_Assignment
{
    public partial class WAD : System.Web.UI.MasterPage
    {

        //DB
        private string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Initialize webpage
            if (IsPostBack == false)
            {
                BindMenu();
            }

            //Always check active user account
            if (RetrieveActiveUserAccount() == true)
            {
                //Always check the user's role (Set up navigation menu)
                SetUpNavigationMenu();

                // Create Forms Authetication for active user
                createTicket();
            }
            else
            {
                //Active Customer Navigation(By Default)
                ActiveCustomerNavigation();
            }

        }

        private void createTicket()
        {
            // Create a new ticket used for authentication
            if(Session["username"] != null && Session["userRole"] != null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, // Ticket version
                Session["username"].ToString(), // username associated with ticket
                DateTime.Now, // Date/time issued
                DateTime.Now.AddDays(1), // Date/time to expire
                true, // "true" for a persistent user cookie
                Session["userRole"].ToString(),
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
            }
        }

        private void BindMenu()
        {
            if(GetMenuItems() != null)
            {
                DataSet menuItem = GetMenuItems();

                //Bind Menu for Homepage
                bindMenuItem(menuItem, siteMenu);

                //Bind Menu for Not Homepage
                bindMenuItem(menuItem, headerSiteMenu);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }
            
        }

        private DataSet GetMenuItems()
        {
            //Retrieve the menu data for menu control
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("sp_GetMenuData", con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    // Establish relationship (foreign keys)
                    ds.Relations.Add("ChildRows", ds.Tables[0].Columns["Id"], ds.Tables[1].Columns["ParentId"]);

                    return ds;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }

            return null;
        }

        private void bindMenuItem(DataSet ds, Menu menuID)
        {
            //Bind the menu item to menu control
            foreach (DataRow level1DataRow in ds.Tables[0].Rows)
            {
                MenuItem item = new MenuItem();
                item.Text = level1DataRow["MenuText"].ToString();
                item.NavigateUrl = level1DataRow["NavigateUrl"].ToString();

                DataRow[] level2DataRows = level1DataRow.GetChildRows("ChildRows");
                foreach (DataRow level2DataRow in level2DataRows)
                {
                    MenuItem childItem = new MenuItem();
                    childItem.Text = level2DataRow["MenuText"].ToString();
                    childItem.NavigateUrl = level2DataRow["NavigateUrl"].ToString();
                    item.ChildItems.Add(childItem);
                }

                menuID.Items.Add(item);

            }
        }

        private Boolean RetrieveActiveUserAccount()
        {
            //Retrieve the account that is active
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT UserId, Name, Email, ProfileImg, Role FROM [dbo].[User] WHERE LoginStatus = 'Active'", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //Update the sidebar with user account
                    if (dt.Rows.Count >= 1)
                    {
                        Session["userId"] = dt.Rows[0]["userId"].ToString();

                        Session["username"] = dt.Rows[0]["Name"].ToString();
                        lblLoginName.Text = Session["username"].ToString();

                        Session["userPicPath"] = dt.Rows[0]["ProfileImg"].ToString();

                        //Update Profile Pic
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateProfilePicPath", "document.getElementById('profileImg').src ='" + Session["userPicPath"].ToString() + "';", true);

                        //User Role
                        Session["userRole"] = dt.Rows[0]["Role"].ToString();

                        //Email - for contact form
                        Session["userEmail"] = dt.Rows[0]["Email"].ToString();

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }

            return false;
        }

        private void SetUpNavigationMenu()
        {

            if (Session["userRole"].ToString().Equals("Artist"))
            {
                ActiveArtistNavigation();
            }
            else
            {
                ActiveCustomerNavigation();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //Deactive the account
            DeactivateProfileNavigation();

            //Active Customer Navigation (By Default)
            ActiveCustomerNavigation();

            //Reset Authentication (Cookie)
            FormsAuthentication.SignOut();

            //Reset the lblLoginName
            lblLoginName.Text = "";

            //Reset Profile
            Session["username"] = "";
            Session["userPicPath"] = "";
            Session["userRole"] = "";
            Session["userEmail"] = "";

            //Clear Session
            Session.Abandon();
            Session.Clear();

            //Update menu (Need to reset siteMenu && headerSiteMenu first)
            siteMenu.Items.Clear();
            headerSiteMenu.Items.Clear();
            BindMenu();

            //Return to homepage
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Homepage",
            "window.location ='Homepage.aspx';alert('Logout Success');", true);

        }

        private void DeactivateProfileNavigation()
        {
            //Deactive the user account once the user logout
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblLoginName.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }
        }

        private void ActiveArtistNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActiveArtist", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MasterpageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
            }
        }
    }
}