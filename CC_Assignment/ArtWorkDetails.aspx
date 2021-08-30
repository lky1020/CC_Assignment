<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ArtWorkDetails.aspx.cs" Inherits="CC_Assignment.ArtWorks.ArtWorkDetails" %>

<asp:Content ID="ArtWorkDetails" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <img src="img/artwork/art_about.jpg" class="art-details-bg" />

    <div class="details-container">
        <div class="details-left">
            <asp:Image ID="dArtDetailsImage" runat="server" CssClass="details-image" />
        </div>

        <div class="details-right">
            <%-- Art Name --%>
            <asp:Label ID="dArtName" runat="server" CssClass="details-artname"></asp:Label>
            <p class="details-price-str">PRICE</p>

            <%-- Price --%>
            <asp:Label ID="dArtPrice" runat="server" CssClass="details-price"></asp:Label>

            <%-- Stock --%>
            <div class="details-stock-str">
                Current stock :
                <asp:Label ID="dArtStock" runat="server"></asp:Label>
            </div>


            <hr style="margin: 20px 2px 0px 2px;" />

            <%-- Add To Cart  --%>
            <div style="justify-content: center; width: 100%">
                <table class="details-btn">

                    <tr>
                        <td>
                            <asp:ImageButton ID="dPlusControl" runat="server" ImageUrl="~/img/icon/plus.png" Height="50px" Width="50px" OnClick="dPlusControl_Click" /></td>
                        <td>
                            <asp:TextBox ID="detailsQtyControl" runat="server" CssClass="d-qty-control" Text="1"></asp:TextBox></td>
                        <td>
                            <asp:ImageButton ID="dMinusControl" runat="server" ImageUrl="~/img/icon/minus.png" Height="50px" Width="50px" OnClick="dMinusControl_Click" /></td>

                        <td>
                            <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="art-to-cart-btn add-btn-large margin-l10" OnClick="addToCartBtn_Click" />
                        </td>

                    </tr>
                </table>
            </div>
            <br />
            <hr style="margin: 0px 2px 0px 2px;" />
        </div>

        <%-- Wishlist button & Close button --%>
        <div>
            <asp:ImageButton ID="detailsLoveBtn" runat="server" AlternateText="Add to WishList" ImageUrl="img/wishlist/heart-icon-inactive.png" CssClass="cancel-btn" CommandArgument='<%# Eval("ArtId")%>' CommandName="addtowishlist" OnClick="loveBtn_Click" />
            <asp:ImageButton ID="detailsCancelBtn" runat="server" ImageUrl="~/img/artwork/icons8-cancel.png" OnClick="detailsCancelBtn_Click" CssClass="cancel-btn" />
        </div>
    </div>

    <%-- Art Description--%>
    <div class="details-box">
        <table class="details-about">
            <tr>
                <td>
                    <img src="img/artwork/icons8-paint-brush.png" /></td>
                <td style="color: white;"><span>A</span>bout the art</td>
            </tr>
        </table>

        <div class="details-desc-box">
            <asp:Label ID="dAboutArt" runat="server" CssClass="details-artabout"></asp:Label>
        </div>
    </div>

    <%-- Artist Description--%>
    <div class="details-box">
        <div class="details-box-border">
            <table class="details-about">
                <tr>
                    <td>
                        <img src="img/artwork/icons8-artist.png" />
                    </td>
                    <td style="color: white;"><span>A</span>rtist</td>
                </tr>
            </table>

            <%-- Artist Image --%>
            <table style="width: 550px; margin-left: auto; margin-right: auto;">
                <tr>
                    <td style="width: 220px;">
                        <asp:Image ID="dArtistImage" runat="server" CssClass="details-artist-img" />
                    </td>

                    <%-- Artist Description --%>
                    <td style="height: 60px; padding-left: 5%;">
                        <%-- Artist Name --%>
                        <asp:Label ID="dArtistName" runat="server" CssClass="details-artist-name"></asp:Label><br />
                        <asp:Label ID="dBio" CssClass="details-bio" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <!-- Disable print screen key 
    <script language="javascript" type="text/javascript">
        function AccessClipboardData() {
            try {
                window.clipboardData.setData('text', "No print data");
            } catch (err) {
                txt = "There was an error on this page.\n\n";
                txt += "Error description: " + err.description + "\n\n";
                txt += "Click OK to continue.\n\n";
                alert(txt);
            }
            setInterval("AccessClipboardData()", 300);
            var ClipBoardText = "";
            if (window.clipboardData) {
                ClipBoardText = window.clipboardData.getData('text');
                if (ClipBoardText != "No print data") {
                    alert('Sorry you have to allow the page to access clipboard');
                    // hide the div which contains your data          
                    document.all("divmaster").style.display = "none"
                }
        }
    </script>-->

</asp:Content>
