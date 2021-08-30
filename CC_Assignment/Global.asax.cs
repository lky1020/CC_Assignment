using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CC_Assignment
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            HttpException httpException = (HttpException)ex;
            int httpCode = httpException.GetHttpCode();

            //store the error for later
            Application["exception"] = ex;

            //Store the location of file that made error
            Application["location"] = Request.Url.ToString();
            Application["Message"] = ex.Message;

            if(ex.InnerException != null)
            {
                Application["InnerException"] = ex.InnerException.ToString();
            }


            //clear the error so we can continue onwards
            Server.ClearError();


            //send user to error page
            if (httpCode == 404)
            {
                Response.Redirect("ErrorPages/FileNotFound.htm");
            }

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}