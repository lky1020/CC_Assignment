using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace CC_Assignment.ArtWorks
{
    public partial class ArtWorks : System.Web.UI.Page
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

            checkAvailability();
            setLoveIcon();


        }

        private void checkAvailability()
        {
            foreach (DataListItem item in ArtWorkDataList.Items)
            {
                var currentKey = ArtWorkDataList.DataKeys[item.ItemIndex];

                Int32 artId = Convert.ToInt32(currentKey);
                int stock = 0;

                connection();
                conn.Open();

                //Check stock
                string query = "SELECT Quantity FROM [dbo].[Artist] WHERE ArtId =" + artId;

                using (SqlCommand cmdUser = new SqlCommand(query, conn))
                {
                    stock = ((int)cmdUser.ExecuteScalar());
                }

                conn.Close();

                if (stock == 0)
                {
                    //if no add in wishlist, inactive icon
                    Button btn = item.FindControl("addToCartBtn") as Button;
                    btn.Enabled = false;
                    btn.Text = "SOLD OUT";
                    btn.BackColor = Color.DarkGray;
                }
                
            }
        }

        private void setLoveIcon()
        {
            Int32 wishlistID;

            foreach (DataListItem item in ArtWorkDataList.Items)
            {
                var currentKey = ArtWorkDataList.DataKeys[item.ItemIndex];

                Int32 artId = Convert.ToInt32(currentKey);

                connection();
                conn.Open();

                //Check wishlist
                string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ArtId ='" + artId + "'";
                // System.Diagnostics.Debug.WriteLine("[DEBUG][ArtName] --> " + artId);
                using (SqlCommand cmdUser = new SqlCommand(query, conn))
                {
                    wishlistID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                }

                conn.Close();

                if (wishlistID == 0)
                {
                    //if no add in wishlist, inactive icon
                    (item.FindControl("loveBtn") as ImageButton).ImageUrl = "/img/wishlist/heart-icon-inactive.png";
                }

                else
                {
                    //active icon
                    (item.FindControl("loveBtn") as ImageButton).ImageUrl = "/img/wishlist/heart-icon-active.png";
                }
            }
        }

        private void connection()
        {
            sqlconn = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
            conn = new SqlConnection(sqlconn);

        }

        void bindList()
        {

            connection();

            //sorting feature
            dataAdapter = new SqlDataAdapter("Select * from Artist WHERE Availability = '1' ORDER BY ArtId DESC", conn);
            if (rblCategory.SelectedIndex != -1)
            {
                switch (ddlArtSort.SelectedIndex)
                {
                    //Display all
                    case 0:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtId DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name asscending
                    case 1:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtName ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by name descending
                    case 2:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY ArtName DESC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price asscending
                    case 3:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY Price ASC", conn);
                        command.Parameters.AddWithValue("@Category", rblCategory.SelectedIndex + 1);
                        dataAdapter.SelectCommand = command;
                        break;

                    //Sort by price descending
                    case 4:
                        command = new SqlCommand("Select * from Artist " + "WHERE Category = @Category AND Availability = '1' ORDER BY Price DESC", conn);
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
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtId DESC", conn);
                        break;

                    //Sort by name asscending
                    case 1:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtName ASC", conn);
                        break;

                    //Sort by name descending
                    case 2:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY ArtName DESC", conn);
                        break;

                    //Sort by price asscending
                    case 3:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY Price ASC", conn);
                        break;

                    //Sort by price descending
                    case 4:
                        dataAdapter = new SqlDataAdapter("Select * from Artist where Availability = '1' ORDER BY Price DESC", conn);
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

            checkAvailability();
            setLoveIcon();
        }

        protected void btnPreviousClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];

            if (CurrentPage != 0)
                CurrentPage -= 1;
            ViewState["PageCount"] = CurrentPage;

            DataListPaging((DataTable)ViewState["PagedDataSurce"]);

            checkAvailability();
            setLoveIcon();
        }

        protected void btnNextClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["PageCount"];
            CurrentPage += 1;
            ViewState["PageCount"] = CurrentPage;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);

            checkAvailability();
            setLoveIcon();
        }


        protected void btnLastClick_Click(object sender, EventArgs e)
        {
            CurrentPage = (int)ViewState["TotalCount"] - 1;
            DataListPaging((DataTable)ViewState["PagedDataSurce"]);

            checkAvailability();
            setLoveIcon();
        }

        protected void ddlArtSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
            checkAvailability();
            setLoveIcon();
        }

        protected void rblCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindList();
            checkAvailability();
            setLoveIcon();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            bindList();
            checkAvailability();
            setLoveIcon();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rblCategory.SelectedIndex = -1;
            bindList();
            checkAvailability();
            setLoveIcon();
        }

        protected void loveBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;
            Int32 wishlistID;
            Int32 artID = Convert.ToInt32(imgButton.CommandArgument.ToString());

            try
            {
                if (Session["userId"] != null)
                {
                    try
                    {
                        //Get current user id
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString))
                        {
                            con.Open();

                            //check existing art in wishlist
                            string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ArtId ='" + artID + "'";
                            using (SqlCommand cmdUser = new SqlCommand(query, con))
                            {
                                wishlistID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                            }

                            if (wishlistID == 0)
                            {
                                string sql = "INSERT into Wishlist (ArtId, UserId, DateAdded) values('" + artID + "', '" + Session["userId"] + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";

                                SqlCommand cmd = new SqlCommand();

                                cmd.Connection = con;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = sql;


                                cmd.ExecuteNonQuery();
                                imgButton.ImageUrl = "/img/wishlist/heart-icon-active.png";
                                //Response.Write("<script>alert('Congratulation, Art Added into Wishlist Successfully')</script>");
                                System.Diagnostics.Debug.WriteLine("[MSG][WISHLIST] --> Congratulation, Art Added into Wishlist Successfully");
                            }
                            else
                            {
                                //Delete the art in wishlist

                                query = "DELETE FROM [dbo].[Wishlist] WHERE WishlistId = @wishlistID";

                                SqlCommand cmd = new SqlCommand(query, con);

                                cmd.Parameters.AddWithValue("@wishlistID", wishlistID);
                                cmd.ExecuteNonQuery();

                                //unactive the icon
                                imgButton.ImageUrl = "/img/wishlist/heart-icon-inactive.png";

                                System.Diagnostics.Debug.WriteLine("[MSG][WISHLIST] --> Congratulation, Art in Wishlist Deleted Successfully");
                            }
                            con.Close();

                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Sorry, please try again later')</script>");
                        System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
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
                System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
            }


        }

        protected void addToCartBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            Int32 artID = Convert.ToInt32(btn.CommandArgument.ToString());
            Int32 cartID = 0;
            Int32 orderDetailID = 0;

            int qtyOrderDetail = 0;
            decimal subtotalOrderDetail = 0;


            Button BuyNowButton = (Button)sender;
            DataListItem item = (DataListItem)btn.NamingContainer;
            Label unitPrice = (Label)item.FindControl("PriceLabel");

            try
            {
                if (Session["userId"] == null)
                {
                    Response.Write("<script>alert('Please Login first!')</script>");
                }
                else
                {
                    connection();
                    conn.Open();


                    string queryCheckCart = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + Session["userId"] + "'AND status = 'cart'";

                    using (SqlCommand cmdCheckCart = new SqlCommand(queryCheckCart, conn))
                    {
                        cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                    }
                    conn.Close();

                    if (cartID == 0)
                    {
                        //insert to create a new cart
                        String status = "cart";
                        string sql = "INSERT into Cart (UserId, status) values('" + Session["username"] + "', '" + status + "')";

                        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString);
                        SqlCommand cmd = new SqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        //search the new cartid
                        conn.Open();
                        string queryFindCartID = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + Session["username"] + "'AND status = 'cart'";

                        using (SqlCommand cmdCheckCart = new SqlCommand(queryFindCartID, conn))
                        {
                            cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                        }
                        conn.Close();



                    }

                    //get exist order detail

                    conn.Open();

                    SqlCommand cmdOrderDetailID = new SqlCommand("SELECT OrderDetailId, qtySelected, Subtotal from [OrderDetails] Where CartId = @CartId AND ArtId = @ArtId", conn);
                    cmdOrderDetailID.Parameters.AddWithValue("@CartId", cartID);
                    cmdOrderDetailID.Parameters.AddWithValue("@ArtId", artID);

                    SqlDataReader dtrOrderDetail = cmdOrderDetailID.ExecuteReader();
                    if (dtrOrderDetail.HasRows)
                    {
                        while (dtrOrderDetail.Read())
                        {
                            orderDetailID = (Int32)dtrOrderDetail["OrderDetailId"];
                            qtyOrderDetail = (int)dtrOrderDetail["qtySelected"];
                            subtotalOrderDetail = (decimal)dtrOrderDetail["Subtotal"];
                        }

                    }
                    conn.Close();

                    conn.Open();

                    //check whether exist same art (order detail)
                    if (orderDetailID != 0)
                    {
                        //update order details
                        qtyOrderDetail++;
                        subtotalOrderDetail += Decimal.Parse(unitPrice.Text);

                        string sqlUpdatetOrder = "UPDATE  OrderDetails SET qtySelected = " + qtyOrderDetail + ", Subtotal = " + subtotalOrderDetail + " WHERE OrderDetailId = " + orderDetailID ;

                        SqlCommand cmdInsertOrder = new SqlCommand();

                        cmdInsertOrder.Connection = conn;
                        cmdInsertOrder.CommandType = CommandType.Text;
                        cmdInsertOrder.CommandText = sqlUpdatetOrder;


                        cmdInsertOrder.ExecuteNonQuery();
                    }
                    else
                    {
                        //insert order details based on cartid

                        string sqlInsertOrder = "INSERT into OrderDetails (CartId, ArtId, qtySelected, Subtotal) values('" + cartID + "', '" + artID + "', '" + 1 + "', '" + Decimal.Parse(unitPrice.Text) + "')";

                        SqlCommand cmdInsertOrder = new SqlCommand();

                        cmdInsertOrder.Connection = conn;
                        cmdInsertOrder.CommandType = CommandType.Text;
                        cmdInsertOrder.CommandText = sqlInsertOrder;


                        cmdInsertOrder.ExecuteNonQuery();

                    }

                    conn.Close();


                    Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
                }

            }


            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, Fail to Add Cart. Please try again')</script>");
                System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
            }
        }

        protected void ArtWorkDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {

            if (e.CommandName == "addtocart")
            {

            }

        }
    }
}