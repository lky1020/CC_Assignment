using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace CC_Assignment.ArtWorks
{
    public partial class ArtWorkDetails : System.Web.UI.Page
    {

        string constr = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
        Int32 wishlistID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ImageButton loveBtn = FindControl("detailsLoveBtn") as ImageButton;
                this.GetArtDetails();
            }
        }


        private void GetArtDetails()
        {
            try
            {
                // create connection
                SqlConnection con = new SqlConnection(constr);
                con.Open();

                // retrieve data
                SqlCommand cmd = new SqlCommand("SELECT a.ArtName, a.Availability, a.ArtImage, a.Price, a.ArtDescription, a.Quantity, u.Name, u.Bio, u.ProfileImg from[Artist] a INNER JOIN[User] u on a.UserId = u.UserId Where a.ArtId = @ArtId", con);
                cmd.Parameters.AddWithValue("@ArtId", Request.QueryString["ArtId"]);

                SqlDataReader dtrArt = cmd.ExecuteReader();

                if (dtrArt.HasRows)
                {
                    while (dtrArt.Read())
                    {

                        dArtDetailsImage.ImageUrl = dtrArt["ArtImage"].ToString();

                        dArtName.Text = dtrArt["ArtName"].ToString().ToUpper();

                        dArtPrice.Text = "RM " + String.Format("{0:0.00}", dtrArt["Price"]);

                        dAboutArt.Text = dtrArt["ArtDescription"].ToString();

                        if(Convert.ToInt32(dtrArt["Availability"]) == 0){
                            dArtStock.Text = "-";
                        }
                        else
                        {
                            dArtStock.Text = dtrArt["Quantity"].ToString();
                        }
                        

                        string profileImage = dtrArt["ProfileImg"].ToString();
                        if (!String.IsNullOrEmpty(profileImage))
                            dArtistImage.ImageUrl = profileImage;
                        else
                            dArtistImage.ImageUrl = "/img/userPic/user_default.png";

                        dArtistName.Text = dtrArt["Name"].ToString();

                        string bio = dtrArt["Bio"].ToString();
                        if (!String.IsNullOrEmpty(bio))
                            dBio.Text = bio;
                        else
                            dBio.Text = "The artist didn't leave any bio...";


                    }

                    con.Close();
                    con.Open();

                    //Check wishlist
                    string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ArtId ='" + Request.QueryString["ArtId"] + "'";
                    using (SqlCommand cmdUser = new SqlCommand(query, con))
                    {
                        wishlistID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                    }

                    if (wishlistID == 0)
                    {
                        //if no add in wishlist, inactive icon
                        detailsLoveBtn.ImageUrl = "/img/wishlist/heart-icon-inactive.png";
                    }
                    else
                    {
                        //active icon
                        detailsLoveBtn.ImageUrl = "/img/wishlist/heart-icon-active.png";
                    }

                    con.Close();

                    //Check stock
                    if (dArtStock.Text.Equals("-"))
                    {
                        //disable button
                        addToCartBtn.Enabled = false;
                        addToCartBtn.Text = "Not Available";
                        addToCartBtn.BackColor = Color.DarkGray;
                    }else if (dArtStock.Text.Equals("0"))
                    {
                        addToCartBtn.Enabled = false;
                        addToCartBtn.Text = "SOLD OUT";
                        addToCartBtn.BackColor = Color.DarkGray;
                    }

                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, Not Able to Load the Details')</script>");
                System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
            }
        }

        protected void loveBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;
            Int32 wishlistID;

            try
            {
                if (Session["userId"] != null)
                {
                    try
                    {
                        // create connection
                        SqlConnection con = new SqlConnection(constr);
                        con.Open();

                        //check existing art in wishlist
                        string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ArtId ='" + Request.QueryString["ArtId"] + "'";
                        using (SqlCommand cmdUser = new SqlCommand(query, con))
                        {
                            wishlistID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                        }

                        if (wishlistID == 0)
                        {
                            //Insert Art into Wishlist
                            string sql = "INSERT into Wishlist (ArtId, UserId, DateAdded) values('" + Request.QueryString["ArtId"] + "', '" + Session["userId"] + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";

                            SqlCommand cmd = new SqlCommand();

                            cmd.Connection = con;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;


                            cmd.ExecuteNonQuery();

                            //active the icon
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

        //Add to Cart button
        protected void addToCartBtn_Click(object sender, EventArgs e)
        {
            int qtySelected = 0;
            decimal subtotal = 0;
            Int32 cartID;
            Int32 orderDetailID = 0;
            int qtyOrderDetail = 0;
            decimal subtotalOrderDetail = 0;

            try
            {
                qtySelected = Convert.ToInt32(detailsQtyControl.Text);
                subtotal = qtySelected * Convert.ToDecimal(dArtPrice.Text.Substring(3));

                try
                {
                    if (Session["userId"] != null)
                    {

                        //Insert database
                        try
                        {
                            using (SqlConnection con = new SqlConnection(constr))
                            {

                                //Check whether valid input and enough quantity
                                if (qtySelected == 0)
                                {
                                    Response.Write("<script>alert('The quantity cannot be 0, please enter your quantity.')</script>");
                                }
                                else if (qtySelected > Convert.ToInt32(dArtStock.Text))
                                {
                                    Response.Write("<script>alert('Sorry, not enough stock, please enter your quantity.')</script>");
                                }
                                else
                                {
                                    con.Open();
                                    string queryCheckCart = "Select CartId FROM [dbo].[Cart] WHERE UserId = '" + Session["userId"] + "'AND status = 'cart'";

                                    using (SqlCommand cmdCheckCart = new SqlCommand(queryCheckCart, con))
                                    {
                                        cartID = ((Int32?)cmdCheckCart.ExecuteScalar()) ?? 0;
                                    }
                                    con.Close();

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

                                    con.Open();

                                    SqlCommand cmdOrderDetailID = new SqlCommand("SELECT OrderDetailId, qtySelected, Subtotal from [OrderDetails] Where CartId = @CartId AND ArtId = @ArtId", con);
                                    cmdOrderDetailID.Parameters.AddWithValue("@CartId", cartID);
                                    cmdOrderDetailID.Parameters.AddWithValue("@ArtId", Request.QueryString["ArtId"]);

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
                                    con.Close();

                                    con.Open();

                                    //check whether exist same art (order detail)
                                    if (orderDetailID != 0)
                                    {
                                        //update order details
                                        qtyOrderDetail += qtySelected;
                                        subtotalOrderDetail += subtotal;

                                        string sqlUpdatetOrder = "UPDATE  OrderDetails SET qtySelected = " + qtyOrderDetail + ", Subtotal = " + subtotalOrderDetail + " WHERE OrderDetailId = " + orderDetailID;

                                        SqlCommand cmdInsertOrder = new SqlCommand();

                                        cmdInsertOrder.Connection = con;
                                        cmdInsertOrder.CommandType = CommandType.Text;
                                        cmdInsertOrder.CommandText = sqlUpdatetOrder;


                                        cmdInsertOrder.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        //insert order details based on cartid

                                        string sqlInsertOrder = "INSERT into OrderDetails (CartId, ArtId, qtySelected, Subtotal) values('" + cartID + "', '" + Request.QueryString["ArtId"] + "', '" + qtySelected + "', '" + subtotal + "')";

                                        SqlCommand cmdInsertOrder = new SqlCommand();

                                        cmdInsertOrder.Connection = con;
                                        cmdInsertOrder.CommandType = CommandType.Text;
                                        cmdInsertOrder.CommandText = sqlInsertOrder;


                                        cmdInsertOrder.ExecuteNonQuery();

                                    }

                                    con.Close();


                                    Response.Write("<script>alert('Congratulation, Art Added into Cart Successfully')</script>");
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            Response.Write("<script>alert('Sorry, Fail to Add Cart. Please try again')</script>");
                            System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Please Login first!')</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Sorry, No User Login Found')</script>");
                    System.Diagnostics.Debug.WriteLine("[DEBUG][EXCEPTION] --> " + ex.Message);
                }
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Sorry, quantity cannot be blank.')</script>");
            }
        }

        protected void detailsCancelBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/ArtWorks.aspx");
        }

        protected void dPlusControl_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int qty;
                if (detailsQtyControl.Text.Equals(null))
                    qty = 0;
                else
                    qty = Convert.ToInt32(detailsQtyControl.Text);
                qty += 1;
                detailsQtyControl.Text = qty.ToString();
            }
            catch
            {
                Response.Write("<script>alert('Sorry, please enter your quantity.')</script>");
            }
            

        }

        protected void dMinusControl_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int qty;
                if (detailsQtyControl.Text.Equals(null))
                    qty = 0;
                else
                    qty = Convert.ToInt32(detailsQtyControl.Text);

                if (qty != 0)
                    qty -= 1;
                detailsQtyControl.Text = qty.ToString();
            }
            catch
            {
                Response.Write("<script>alert('Sorry, please enter your quantity.')</script>");
            }
           
        }
    }
}