<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="CustApparel.aspx.cs" Inherits="CC_Assignment.CustApparel.CustApparel" %>

<asp:Content ID="CustApparel" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Header Banner -->
    <div class="container">
        <img src="img/apparel/header_bg1.jpeg" alt="" class="apparel-gallery-header-bg" />

        <div id="apparel-gallery-header-text" class="apparel-gallery-header-text">
                 <h3 class="collectionTitle"><span>A</span>pparel<span> C</span>ollections</h3>
         </div>
    </div>

    <!-- Filter -->
    <div class="apparel-gallery-filter">

        <p class="text-f1">Filters</p>
        <hr class="filter-line" />

        <!-- Category -->
        <p class="text-f2">Category</p>
        <asp:RadioButtonList ID="rblCategory" runat="server" CssClass="cbl white-text" CellSpacing="10" DataSourceID="CategoryDataSource" DataTextField="CategoryName" DataValueField="CategoryName" OnSelectedIndexChanged="rblCategory_SelectedIndexChanged"></asp:RadioButtonList>
        <hr class="filter-line" />

        <!-- Search button & Reset button -->
        <div class="flex-parent jc-between">
            <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="btn-filter" OnClick="btnFilter_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn-filter" OnClick="btnReset_Click" />
        </div>

    </div>

    <!-- Gallery -->
    <div class="apparel-gallery">
        <!-- Sorting & Search -->
        <div class="apparel-search-sort">
            <div class="apparel-sort">
                <asp:DropDownList ID="ddlApparelSort" runat="server" CssClass="ddlApparelSort" AutoPostBack="True" OnSelectedIndexChanged="ddlApparelSort_SelectedIndexChanged">
                    <asp:ListItem>Sort by</asp:ListItem>
                    <asp:ListItem>Name A - Z</asp:ListItem>
                    <asp:ListItem>Name Z - A</asp:ListItem>
                    <asp:ListItem>Price Low to High</asp:ListItem>
                    <asp:ListItem>Price High to Low</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>

        <div class="dlImg">
            <!-- Data list -->
            <asp:DataList ID="ApparelDataList" runat="server" DataKeyField="Id" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="35" HorizontalAlign="Center" CellPadding="3" OnItemCommand="ApparelDataList_ItemCommand">
                <ItemTemplate>
                    <table id="apparel-table" style="padding-bottom: 15px;">
                        <tr>
                            <td>
                                <a href="ApparelDetails.aspx?Id=<%#:Eval("Id")%>">
                                    <asp:Image ID="ApparelImage" runat="server" CssClass="apparel-gallery-image" ImageUrl='<%# Eval("Image") %>' />
                                </a>
                            </td>
                        </tr>
                        <tr class="text-a1 padding-b15">
                            <td>
                                <a href="ApparelDetails.aspx?Id=<%#:Eval("Id")%>" class="apparel-title">
                                    <asp:Label ID="ApparelNameLabel" runat="server" Text='<%# Eval("Name") %>' />
                                </a>
                            </td>
                        </tr>

                        <tr class="text-a3">
                            <td class="white-text">
                                <asp:Label ID="ApparelSizeLabel" CssClass="white-text" runat="server" Text='<%# Eval("Size") %>'/> size
                            </td>
                        </tr>

                        <tr class="text-a3">
                            <td class="white-text">RM
                                <asp:Label ID="PriceLabel" CssClass="white-text" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Price"))  %>'/>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="add-to-cart-btn add-btn-medium" CommandArgument='<%# Eval("Id")%>' CommandName="addtocart" OnClick="addToCartBtn_Click" AutoPostback = false/>
                    <asp:ImageButton ID="loveBtn" runat="server" AlternateText="Add to WishList" OnClick="loveBtn_Click" ImageUrl="img/wishlist/heart-icon-inactive.png" ImageAlign="right" CssClass="love-btn" CommandArgument='<%# Eval("Id")%>' CommandName="addtowishlist" />
                    <br /><br />
                </ItemTemplate>
            </asp:DataList>

            <!-- Paging -->
            <table class="paging" id="paging">
                <tr>
                    <td>
                        <asp:Button ID="btnFirst" runat="server" Text="FIRST" OnClick="btnFirstClick_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnPrevious" runat="server" Text="PREV" OnClick="btnPreviousClick_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnNext" runat="server" Text="NEXT" OnClick="btnNextClick_Click" CssClass="page-btn" />
                    </td>

                    <td>
                        <asp:Button ID="btnLast" runat="server" Text="LAST" OnClick="btnLastClick_Click" CssClass="page-btn" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <!-- Data Source -->
    <asp:SqlDataSource ID="CategoryDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="ApparelDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM [Seller] INNER JOIN Category ON Category.CategoryID = @Category">
        <SelectParameters>
            <asp:ControlParameter ControlID="ApparelDataList" Name="Category" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <script type="text/javascript">
        const customerToggle = document.querySelector('.toggle');
        const customerGalleryFilter = document.querySelector('.product-gallery-filter');
        const customerGallery = document.querySelector('.product-gallery');
            
        window.setInterval(function () {

            if (customerToggle.classList.contains('active')) {

                customerGalleryFilter.setAttribute("style", "display: none;");
                customerGallery.setAttribute("style", "margin-left: 0%; margin-top: 25%;");

            } else {

                customerGalleryFilter.setAttribute("style", "display: block;");
                customerGallery.setAttribute("style", "margin-left: 14%; margin-top: 20%;");

            }
        }, 500);
    </script>
</asp:Content>
