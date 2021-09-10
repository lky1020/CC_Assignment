using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CC_Assignment
{
    public partial class AddCollections : System.Web.UI.Page
    {
        string FormatType = string.Empty;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnViewApparel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageCollections.aspx");
        }

        protected void btnViewApparelSubmit_Click1(object sender, EventArgs e)
        {
            Int32 userID = 0;

            try
            {
                if (Session["username"] != null)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
                    {
                        con.Open();

                        string query = "Select UserId FROM [dbo].[User] WHERE Name = '" + Session["username"].ToString() + "'";
                        using (SqlCommand cmdUser = new SqlCommand(query, con))
                        {
                            userID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, No User Login Found')</script>");
            }

            try
            {

                string path;

                if (FileUpload1.HasFile)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(FileUpload1.FileContent);

                    FileUpload1.SaveAs(Server.MapPath("~/img/Collections/") + FileUpload1.FileName);

                    path = "img/Collections/" + FileUpload1.FileName;

                    string sql = "insert into Seller (Name, Size, Image, UserId, Category, Price, Quantity) values('" + txtBoxApparelName.Text + "', '"
                        + ddlSizeApparel.SelectedValue + "', '" + path + "', '" + userID + "', '" + ddlCatApparel.SelectedValue + "', '"
                        + txtBoxApparelPrice.Text + "', '" + txtBoxApparelQuantity.Text + "')";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;



                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();



                    Response.Write("<script>alert('Congratulation, Apparel Added Successfully')</script>");
                    txtBoxApparelName.Text = "";
                    txtBoxApparelPrice.Text = "";
                    txtBoxApparelQuantity.Text = "";

                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, No File Uploaded')</script>");
            }

        }

        protected void txtBoxApparelName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCatApparel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSizeApparel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtBoxApparelPrice_TextChanged(object sender, EventArgs e)
        {

        }
    }
}