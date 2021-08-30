using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CC_Assignment
{
    public partial class PostArts : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            btnFirstArt.Enabled = false;
            btnPreviousArt.Enabled = false;

            if (!IsPostBack)
            {
                PopulateGridView();
            }
        }

        void PopulateGridView()
        {
            Int32 userID = 0;


            if (Session["username"] != null)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
                {

                    con.Open();

                    string query = "Select UserId FROM [dbo].[User] WHERE Name = '" + Session["username"].ToString() + "'";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        userID = ((Int32?)cmd.ExecuteScalar()) ?? 0;
                    }



                }
            }

            DataTable dtbl = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
            {
                con.Open();
                String query = "SELECT a.ArtId, a.ArtName, a.ArtDescription, a.ArtImage, c.CategoryName, a.Price, a.Quantity FROM [Artist] a INNER JOIN [Category] c ON c.CategoryID = a.Category WHERE a.UserId =@UserId AND a.Availability='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userID);

                SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);

                sqlDa.Fill(dtbl);

            }
            if (dtbl.Rows.Count > 0)
            {
                gvEditImageInfo.DataSource = dtbl;
                gvEditImageInfo.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvEditImageInfo.DataSource = dtbl;
                gvEditImageInfo.DataBind();
                gvEditImageInfo.Rows[0].Cells.Clear();
                gvEditImageInfo.Rows[0].Cells.Add(new TableCell());
                gvEditImageInfo.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvEditImageInfo.Rows[0].Cells[0].Text = "No Image Uploaded Found ...!";
                gvEditImageInfo.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }



        }

        protected void gvEditImageInfo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvEditImageInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEditImageInfo.EditIndex = e.NewEditIndex;
            PopulateGridView();
        }

        protected void gvEditImageInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEditImageInfo.EditIndex = -1;
            PopulateGridView();
        }

        protected void gvEditImageInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
                {

                    var price = (gvEditImageInfo.Rows[e.RowIndex].FindControl("txtPrice") as TextBox).Text.Trim();
                    int qty = int.Parse((gvEditImageInfo.Rows[e.RowIndex].FindControl("txtQuantity") as TextBox).Text.Trim());

                    
                    con.Open();
                    String query = "Update Artist SET ArtName=@ArtName, ArtDescription=@ArtDescription, Price=@Price, Quantity=@Quantity WHERE ArtId =@ArtId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ArtName", (gvEditImageInfo.Rows[e.RowIndex].FindControl("txtArtName") as TextBox).Text.Trim());
                    cmd.Parameters.AddWithValue("@ArtDescription", (gvEditImageInfo.Rows[e.RowIndex].FindControl("txtArtDescription") as TextBox).Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.Parameters.AddWithValue("@ArtId", Convert.ToInt32(gvEditImageInfo.DataKeys[e.RowIndex].Value.ToString()));
                    cmd.ExecuteNonQuery();
                    gvEditImageInfo.EditIndex = -1;
                    PopulateGridView();

                    Response.Write("<script>alert('Congratulation, Art Information Updated Successfully')</script>");
                   
                   

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, Fail to Update the Art Information. Prica and Quantity cannot be 0')</script>");
            }
        }

        protected void gvEditImageInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
                {
                    con.Open();
                    String query = "Update Artist SET Availability='0' WHERE ArtId =@ArtId";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@ArtId", Convert.ToInt32(gvEditImageInfo.DataKeys[e.RowIndex].Value.ToString()));
                    cmd.ExecuteNonQuery();
                    PopulateGridView();

                    Response.Write("<script>alert('Congratulation, Art Information Deleted Successfully')</script>");

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, Fail to Delete the Art Information')</script>");
            }
        }

        protected void gvEditImageInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            this.PopulateGridView();
        }

        protected void btnFirstArt_Click(object sender, EventArgs e)
        {
            gvEditImageInfo.PageIndex = 0;
            btnFirstArt.Enabled = false;
            btnPreviousArt.Enabled = false;
            btnLastArt.Enabled = true;
            btnNextArt.Enabled = true;
            this.PopulateGridView();
        }

        protected void btnPreviousArt_Click(object sender, EventArgs e)
        {

            int i = gvEditImageInfo.PageCount;
            if (gvEditImageInfo.PageIndex > 0)
            {

                gvEditImageInfo.PageIndex = gvEditImageInfo.PageIndex - 1;
                btnLastArt.Enabled = true;
            }

            if (gvEditImageInfo.PageIndex == 0)
            {
                btnFirstArt.Enabled = false;
            }
            if (gvEditImageInfo.PageCount - 1 == gvEditImageInfo.PageIndex + 1)
            {
                btnNextArt.Enabled = true;
            }
            if (gvEditImageInfo.PageIndex == 0)
            {
                btnPreviousArt.Enabled = false;
            }
            this.PopulateGridView();
        }

        protected void btnNextArt_Click(object sender, EventArgs e)
        {
            int i = gvEditImageInfo.PageIndex + 1;
            if (i <= gvEditImageInfo.PageCount)
            {
                gvEditImageInfo.PageIndex = i;
                btnLastArt.Enabled = true;
                btnPreviousArt.Enabled = true;
                btnFirstArt.Enabled = true;
            }

            if (gvEditImageInfo.PageCount - 1 == gvEditImageInfo.PageIndex)
            {
                btnNextArt.Enabled = false;
                btnLastArt.Enabled = false;
            }
            this.PopulateGridView();
        }

        protected void btnLastArt_Click(object sender, EventArgs e)
        {
            gvEditImageInfo.PageIndex = gvEditImageInfo.PageCount;
            btnLastArt.Enabled = false;
            btnNextArt.Enabled = false;
            btnFirstArt.Enabled = true;
            this.PopulateGridView();
        }
    }
}