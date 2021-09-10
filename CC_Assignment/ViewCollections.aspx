<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ViewCollections.aspx.cs" Inherits="CC_Assignment.ViewCollections" %>

<asp:Content ID="ViewCollections" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <!-- Header Banner -->
        <div id="collections-header" class="container">
            <img style="filter: brightness(50%);" src="img/Collections/collectionBanner.jpg" alt="" class="collections-header-bg" />

            <div id="collections-header-text" class="collections-header-text">
                 <h3 class="collectionTitle"><span>A</span>pparel<span> C</span>ollections</h3>
            </div>
        </div>

    <section id="collectionSection" style="padding-right: 0px; width: 85%;">
        <!-- Filter -->
        <div class="collection-filter">
            <p class="text-f1">Filters</p>
            <hr class="filter-line" />

            <!-- Category -->
            <p class="text-f2">Category</p>
            <asp:RadioButtonList ID="rblCategory" runat="server" CssClass="cbl" CellSpacing="10" DataSourceID="CategoryDataSource" DataTextField="CategoryName" DataValueField="CategoryName" OnSelectedIndexChanged="rblCategory_SelectedIndexChanged"></asp:RadioButtonList>
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
        <div class="collections-gallery">
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
                <asp:DataList ID="ApparelDataList" runat="server" DataKeyField="Id" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="35" HorizontalAlign="Center" CellPadding="3">
                    <ItemTemplate>
                        <table class="padding-b15" id="collection-table">
                            <tr>
                                <td>
                                    <asp:Image ID="ApparelImage" runat="server" CssClass="collections-gallery-image" ImageUrl='<%# Eval("Image") %>' />
                                </td>
                            </tr>
                            <tr class="text-a1 padding-b15">
                                <td>
                                    <asp:Label ID="ApparelNameLabel" runat="server" Text='<%# Eval("Name") %>' />
                                </td>
                            </tr>
                            <tr class="text-a2">
                                <td>
                                    <asp:Label ID="AppareltSizeLabel" runat="server" Text='<%# Eval("Size") %>' />
                                </td>
                            </tr>
                            <tr class="text-a3">
                                <td>RM
                                    <asp:Label ID="PriceLabel" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Price"))  %>' />
                                </td>
                            </tr>
                        </table>
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
        <asp:SqlDataSource ID="collectionDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM Seller INNER JOIN Category ON Category.CategoryID = @Category">
            <SelectParameters>
                <asp:ControlParameter ControlID="ApparelDataList" Name="Category" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>
        
    <script type="text/javascript">
        
        const toggle = document.querySelector('.toggle');
        const collectionsFilter = document.querySelector('.collection-filter');
        const collectionsGallery = document.querySelector('.collections-gallery');
            
        window.setInterval(function () {

            if (toggle.classList.contains('active')) {

                document.getElementById('collectionSection').style.width = "auto";
                collectionsFilter.setAttribute("style", "display: none;");
                collectionsGallery.setAttribute("style", "margin-left: 0%;");
                document.getElementById('collectionSection').style.paddingRight = "100px";

            } else {

                document.getElementById('collectionSection').style.width = "85%";
                collectionsFilter.setAttribute("style", "display: block;");
                collectionsGallery.setAttribute("style", "margin-left: 14%;");
                document.getElementById('collectionSection').style.paddingRight = "0px";
                
            }
        }, 500);

    </script>
</asp:Content>