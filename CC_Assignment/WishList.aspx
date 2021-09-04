<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="CC_Assignment.WishList" %>

<asp:Content ID="WishList" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>

        <div>
            <img src="img/wishlist/wishlist_header.png" alt="" class="artwork-gallery-header-bg" />

            <div id="wishlist-header-text" class="wishlist-header-text">
                WishList
            </div>
            <p class="wishlist-header-quoto">Life is short. Make every outfit count.</p>
            <!-- Continue Add Apparel -->
            <asp:Button ID="btnContinueWL" runat="server" Text="View Apparel" CssClass=".btn-wlApparel" OnClick="btnContinueWL_Click" />
        </div>


        <div class="wl-container">  
            <div class="wishlist-content">
                

                <!-- WishList Table -->
                <div class="wl-table-box">

                    <!-- Add to Cart Button -->
                    <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="float-right art-to-cart-btn add-btn-medium" CommandArgument='<%# Eval("ApparelID")%>' CommandName="addtocart" OnClick="addToCartBtn_Click" AutoPostback = false/>
                    
                    <br />
                    <br />
                    <br />

                    <!-- table -->
                    <asp:GridView ID="gvWishList" runat="server" AutoGenerateColumns="False" DataKeyNames="WishlistId" ShowHeaderWhenEmpty="True" CssClass="gv-wishList" AllowPaging="true" PageSize="5" OnRowDeleting="gvWishList_RowDeleting">
                        <PagerSettings Mode="NextPreviousFirstLast" Visible="false" />
                        <Columns>
                            <%-- WishList ID Col--%>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWishlistID" Text='<%# Eval("WishListId") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Checkbox --%>
                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkBoxHead" AutoPostBack="true" OnCheckedChanged="CheckBoxHead_CheckedChanged" runat="server" />
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItems" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView ApparelImage --%>
                            <asp:TemplateField HeaderText="Apparel" ItemStyle-Width="20%" HeaderStyle-Width="20%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                        <asp:ImageButton ID="wl_apparelImg" runat="server"  ImageUrl='<%# Eval("Image")%>' Height="160" Width="160" OnClick="wl_apparelImg_Click" CommandArgument='<%# Eval("ApparelID")%>' CommandName="viewdetails" CssClass="apparel-image"/>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView ApparelName & Desc --%>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="40%" ItemStyle-Width="40%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="wl_apparelName" runat="server"> 
                                <h3><%#Eval("Name")%></h3>
                                    </asp:Label>
                                    <br />
                                    <asp:Label ID="wl_apparelDes" runat="server"> 
                                  ( <%#Eval("Size")%> )
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView Apparel Price --%>
                            <asp:TemplateField HeaderText="Price" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    RM <asp:Label ID="wl_price" Text='<%# Eval("Price", "{0:0.00}") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView Data Added --%>
                            <asp:TemplateField HeaderText="Date Added" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="wl_dateAdded" Text='<%# Eval("DateAdded", "{0:dd/MM/yyyy}") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%-- Action --%>
                            <asp:TemplateField HeaderText=" " ItemStyle-Width="20%" HeaderStyle-Width="20%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton class="wl_page_deleteBtn" ImageUrl="img/wishlist/icons8-delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="40px" Height="40px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </div>
        </div>
        <!-- Paging -->
        <table class="paging" id="paging">
            <tr>
                <td>
                    <asp:Button ID="btnFirstWL" runat="server" Text="FIRST" OnClick="btnFirstWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnPreviousWL" runat="server" Text="PREV" OnClick="btnPreviousWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnNextWL" runat="server" Text="NEXT" OnClick="btnNextWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnLastWL" runat="server" Text="LAST" OnClick="btnLastWL_Click" CssClass="page-btn" />
                </td>
            </tr>
        </table>

    </section>
</asp:Content>
