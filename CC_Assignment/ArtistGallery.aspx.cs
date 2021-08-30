using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CC_Assignment
{
    public partial class ArtistGallery : System.Web.UI.Page
    {
        int CurrentPage;
        SqlConnection conn;
        string sqlconn;
        SqlDataAdapter dataAdapter;
        DataTable dt;
        SqlCommand command;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindList();
                ViewState["PageCount"] = 0;
            }
            CurrentPage = (int)this.ViewState["PageCount"];
        }


        private void connection()
        {
            sqlconn = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
            conn = new SqlConnection(sqlconn);

        }

        void bindList()
        {
            connection();

            // string strSelect = "Select * from Artist";
            // SqlCommand cmdSelect = new SqlCommand(strSelect, conn);
            // cmdSelect.CommandType = CommandType.StoredProcedure;
            //SELECT * FROM Artist INNER JOIN Category ON Category.CategoryID = 1 AND Artist.Category = 1;
            //"SELECT * FROM Artist ORDER BY Price ASC"

            //sorting feature
            dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1'", conn);
            if (rblCategory.SelectedIndex != -1)
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category" + " AND Availability='1'", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name asscending
                    case 1:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability='1' ORDER BY ArtName ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name descending
                    case 2:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability='1'ORDER BY ArtName DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price asscending
                    case 3:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability='1' ORDER BY Price ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price descending
                    case 4:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability='1' ORDER BY Price DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                }


            }
            else
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1'", conn);
                        break;

                    //Sort by name asscending
                    case 1:
                        dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1' ORDER BY ArtName ASC", conn);
                        break;

                    //Sort by name descending
                    case 2:
                        dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1' ORDER BY ArtName DESC", conn);
                        break;

                    //Sort by price asscending
                    case 3:
                        dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1' ORDER BY Price ASC", conn);
                        break;

                    //Sort by price descending
                    case 4:
                        dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability='1' ORDER BY Price DESC", conn);
                        break;
                }
            }

            dt = new DataTable();
            conn.Open();
            dataAdapter.Fill(dt);

            conn.Close();

            //paging feature
            DataListPaging(dt);
        }

        private void DataListPaging(DataTable dt)
        {
            //PagedDataSource setting
            PagedDataSource PD = new PagedDataSource();

            PD.DataSource = dt.DefaultView;
            PD.PageSize = 6;
            PD.AllowPaging = true;
            PD.CurrentPageIndex = CurrentPage;
            btnFirst.Enabled = !PD.IsFirstPage;
            btnPrevious.Enabled = !PD.IsFirstPage;
            btnNext.Enabled = !PD.IsLastPage;
            btnLast.Enabled = !PD.IsLastPage;
            ViewState["TotalCount"] = PD.PageCount;

            ArtWorkDataList.DataSource = PD;
            ArtWorkDataList.DataBind();
            ViewState["PagedDataSurce"] = dt;
        }

        protected void btnFirstClick_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            ViewState["PageCount"] = CurrentPage;

            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void btnPreviousClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];

            if (CurrentPage != 0)
                CurrentPage -= 1;
            ViewState["PageCount"] = CurrentPage;

            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void btnNextClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];
            CurrentPage += 1;
            ViewState["PageCount"] = CurrentPage;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }



        protected void btnLastClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["TotalCount"] - 1;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);
        }

        protected void ddlArtSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
        }

        protected void rblCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            bindList();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rblCategory.SelectedIndex = -1;
            bindList();
        }
    }
}