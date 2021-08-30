using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;

namespace CC_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        //DB
        private string cs = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            string previousUrl = Request.UrlReferrer.ToString();

            // If unautorized access, go back to previous page
            if (Session["username"] != null)
            {

                if (previousUrl != null)
                {
                    Response.Write("<script>alert('Unauthorized Access !'); window.location = '" + previousUrl + "';</script>");
                }

            }

        }
    }
}