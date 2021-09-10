using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace CC_Assignment.CustApparel
{
    public partial class ApparelDetails : System.Web.UI.Page
    {

        string constr = ConfigurationManager.ConnectionStrings["SyasyaDb"].ConnectionString;
        Int32 wishlistID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ImageButton loveBtn = FindControl("detailsLoveBtn") as ImageButton;
                this.GetApparelDetails();
            }
        }


        private void GetApparelDetails()
        {
            try
            {
                // create connection
                SqlConnection con = new SqlConnection(constr);
                con.Open();

                // retrieve data
                SqlCommand cmd = new SqlCommand("SELECT Name, Availability, Image, Price, Size, Quantity from[Seller] Where Id = @ApparelId", con);
                cmd.Parameters.AddWithValue("@ApparelId", Request.QueryString["Id"]);

                SqlDataReader dtrApparel = cmd.ExecuteReader();

                if (dtrApparel.HasRows)
                {
                    while (dtrApparel.Read())
                    {

                        dApparelDetailsImage.ImageUrl = dtrApparel["Image"].ToString();

                        dApparelName.Text = dtrApparel["Name"].ToString().ToUpper();

                        dApparelSize.Text = "Size: "+ dtrApparel["Size"].ToString().ToUpper();

                        dApparelPrice.Text = "RM " + String.Format("{0:0.00}", dtrApparel["Price"]);


                        if(Convert.ToInt32(dtrApparel["Availability"]) == 0){
                            dApparelStock.Text = "-";
                        }
                        else
                        {
                            dApparelStock.Text = dtrApparel["Quantity"].ToString();
                        }
                      
                    }

                    con.Close();
                    con.Open();

                    //Check wishlist
                    string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ApparelID ='" + Request.QueryString["Id"] + "'";
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
                    if (dApparelStock.Text.Equals("-"))
                    {
                        //disable button
                        addToCartBtn.Enabled = false;
                        addToCartBtn.Text = "Not Available";
                        addToCartBtn.BackColor = Color.DarkGray;
                    }else if (dApparelStock.Text.Equals("0"))
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

                        //check existing apparel in wishlist
                        string query = "SELECT WishlistId FROM [dbo].[Wishlist] WHERE UserId = '" + Session["userId"] + "' AND ApparelID ='" + Request.QueryString["Id"] + "'";
                        using (SqlCommand cmdUser = new SqlCommand(query, con))
                        {
                            wishlistID = ((Int32?)cmdUser.ExecuteScalar()) ?? 0;
                        }

                        if (wishlistID == 0)
                        {
                            //Insert Apparel into Wishlist
                            string sql = "INSERT into Wishlist (ApparelID, UserId, DateAdded) values('" + Request.QueryString["Id"] + "', '" + Session["userId"] + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";

                            SqlCommand cmd = new SqlCommand();

                            cmd.Connection = con;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;


                            cmd.ExecuteNonQuery();

                            //active the icon
                            imgButton.ImageUrl = "/img/wishlist/heart-icon-active.png";

                            //Response.Write("<script>alert('Congratulation, Art Added into Wishlist Successfully')</script>");
                            System.Diagnostics.Debug.WriteLine("[MSG][WISHLIST] --> Congratulation, Apparel Added into Wishlist Successfully");
                        }
                        else
                        {
                            //Delete the apparel in wishlist

                            query = "DELETE FROM [dbo].[Wishlist] WHERE WishlistId = @wishlistID";

                            SqlCommand cmd = new SqlCommand(query, con);

                            cmd.Parameters.AddWithValue("@wishlistID", wishlistID);
                            cmd.ExecuteNonQuery();

                            //unactive the icon
                            imgButton.ImageUrl = "/img/wishlist/heart-icon-inactive.png";

                            System.Diagnostics.Debug.WriteLine("[MSG][WISHLIST] --> Congratulation, Apparel in Wishlist Deleted Successfully");

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
                subtotal = qtySelected * Convert.ToDecimal(dApparelPrice.Text.Substring(3));

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
                                else if (qtySelected > Convert.ToInt32(dApparelStock.Text))
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

                                    SqlCommand cmdOrderDetailID = new SqlCommand("SELECT OrderDetailId, qtySelected, Subtotal from [OrderDetails] Where CartId = @CartId AND ApparelId = @ApparelId", con);
                                    cmdOrderDetailID.Parameters.AddWithValue("@CartId", cartID);
                                    cmdOrderDetailID.Parameters.AddWithValue("@ApparelId", Request.QueryString["Id"]);

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

                                    //check whether exist same apparel (order detail)
                                    if (orderDetailID != 0)
                                    {
                                        //update order details
                                        qtyOrderDetail += qtySelected;
                                        subtotalOrderDetail += subtotal;

                                        string sqlUpdatetOrder = "UPDATE OrderDetails SET qtySelected = " + qtyOrderDetail + ", Subtotal = " + subtotalOrderDetail + " WHERE OrderDetailId = " + orderDetailID;

                                        SqlCommand cmdInsertOrder = new SqlCommand();

                                        cmdInsertOrder.Connection = con;
                                        cmdInsertOrder.CommandType = CommandType.Text;
                                        cmdInsertOrder.CommandText = sqlUpdatetOrder;


                                        cmdInsertOrder.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        //insert order details based on cartid

                                        string sqlInsertOrder = "INSERT into OrderDetails (CartId, ApparelId, qtySelected, Subtotal) values('" + cartID + "', '" + Request.QueryString["Id"] + "', '" + qtySelected + "', '" + subtotal + "')";

                                        SqlCommand cmdInsertOrder = new SqlCommand();

                                        cmdInsertOrder.Connection = con;
                                        cmdInsertOrder.CommandType = CommandType.Text;
                                        cmdInsertOrder.CommandText = sqlInsertOrder;


                                        cmdInsertOrder.ExecuteNonQuery();

                                    }

                                    con.Close();


                                    Response.Write("<script>alert('Congratulation, Apparel Added into Cart Successfully')</script>");
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
            Response.Redirect("/CustApparel.aspx");
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