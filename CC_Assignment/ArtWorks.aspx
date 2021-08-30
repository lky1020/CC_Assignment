<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ArtWorks.aspx.cs" Inherits="CC_Assignment.ArtWorks.ArtWorks" %>

<asp:Content ID="ArtWorks" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Header Banner -->
    <div class="container">
        <img src="img/artwork/header_bg.jpg" alt="" class="artwork-gallery-header-bg" />

        <div id="artwork-gallery-header-text" class="artwork-gallery-header-text">
            Art Gallery
        </div>
    </div>

    <!-- Filter -->
    <div class="artwork-gallery-filter">

        <p class="text-f1">Filters</p>
        <hr class="filter-line" />

        <!-- Category -->
        <p class="text-f2">Category</p>
        <asp:RadioButtonList ID="rblCategory" runat="server" CssClass="cbl white-text" CellSpacing="10" DataSourceID="CategoryDataSource" DataTextField="CategoryName" DataValueField="CategoryName" OnSelectedIndexChanged="rblCategory_SelectedIndexChanged"></asp:RadioButtonList>
        <hr class="filter-line" />

        <!-- Price Range-->
        <!--   <p class="text-f2">Price Range</p>
            <table class="price-table">
                <tr class="margin-b10">
                    <td class="left">Min Price : RM</td>
                    <td>
                        <asp:TextBox ID="txtMinPrice" runat="server" CssClass="txtPrice"></asp:TextBox></td>
                </tr>

                <tr>
                    <td class="left">Max Price: RM</td>
                    <td>
                        <asp:TextBox ID="txtMaxPrice" runat="server" CssClass="txtPrice"></asp:TextBox></td>
                </tr>
            </table> 

            <hr class="filter-line" /> -->

        <!-- Search button & Reset button -->
        <div class="flex-parent jc-between">
            <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="btn-filter" OnClick="btnFilter_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn-filter" OnClick="btnReset_Click" />
        </div>

    </div>

    <!-- Gallery -->
    <div class="artwork-gallery">
        <!-- Sorting & Search -->
        <div class="art-search-sort">
            <!--   <div class="art-search">Search</div> -->

            <div class="art-sort">
                <asp:DropDownList ID="ddlArtSort" runat="server" CssClass="ddlArtSort" AutoPostBack="True" OnSelectedIndexChanged="ddlArtSort_SelectedIndexChanged">
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
            <asp:DataList ID="ArtWorkDataList" runat="server" DataKeyField="ArtId" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="35" HorizontalAlign="Center" CellPadding="3" OnItemCommand="ArtWorkDataList_ItemCommand">
                <ItemTemplate>
                    <table id="artwork-table" style="padding-bottom: 15px;">
                        <tr>
                            <td>
                                <a href="ArtWorkDetails.aspx?ArtId=<%#:Eval("ArtId")%>">
                                    <asp:Image ID="ArtImage" runat="server" CssClass="artwork-gallery-image" ImageUrl='<%# Eval("ArtImage") %>' />
                                </a>
                            </td>
                        </tr>
                        <tr class="text-a1 padding-b15">
                            <td>
                                <a href="ArtWorkDetails.aspx?ArtId=<%#:Eval("ArtId")%>" class="art-title">
                                    <asp:Label ID="ArtNameLabel" runat="server" Text='<%# Eval("ArtName") %>' />
                                </a>
                            </td>
                        </tr>

                        <tr class="text-a2">
                            <td>
                                <asp:Label ID="ArtDescriptionLabel" runat="server" CssClass="white-text" Text='<%# Eval("ArtDescription") %>' />
                            </td>
                        </tr>
                        <tr class="text-a3">
                            <td class="white-text">RM
                                <asp:Label ID="PriceLabel" CssClass="white-text" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Price"))  %>'/>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="art-to-cart-btn add-btn-medium" CommandArgument='<%# Eval("ArtId")%>' CommandName="addtocart" OnClick="addToCartBtn_Click" AutoPostback = false/>
                    <asp:ImageButton ID="loveBtn" runat="server" AlternateText="Add to WishList" OnClick="loveBtn_Click" ImageUrl="img/wishlist/heart-icon-inactive.png" ImageAlign="right" CssClass="love-btn" CommandArgument='<%# Eval("ArtId")%>' CommandName="addtowishlist" />
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
    <asp:SqlDataSource ID="ArtDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM Artist INNER JOIN Category ON Category.CategoryID = @Category">
        <SelectParameters>
            <asp:ControlParameter ControlID="ArtWorkDataList" Name="Category" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>

    <script type="text/javascript">
        const customerToggle = document.querySelector('.toggle');
        const customerGalleryFilter = document.querySelector('.artwork-gallery-filter');
        const customerGallery = document.querySelector('.artwork-gallery');
            
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
