<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="AddCollections.aspx.cs" Inherits="CC_Assignment.AddCollections" %>

<asp:Content ID="AddCollections" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Script for preview Image Start-->
    <script type="text/javascript">  
  
        function showimagepreview(input) {  
  
            if (input.files && input.files[0]) {  
                var reader = new FileReader();  
                reader.onload = function (e) {  
  
                    document.getElementsByTagName("img")[0].setAttribute("src", e.target.result);                     
                }  
                reader.readAsDataURL(input.files[0]);  
            }  
        }  
  
    </script>  
     <!--Script for preview Image End-->

     <section class="addCollectionsTbl" id="addCollectionsTbl">

         <div class="addCollectionsform" id="addCollectionsform">
              <h2 class="addCollectionsttl">Add New Apparel</h2>
              <p class="addCollectionsSubTtl">Please choose the right category for your apparel
              </p>
             <hr class="addCollectionsSepline"/>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>
         </div>
            
         
        <div class="formAddCollections" id="formAddCollections">

            <table class="tblCollectionsInfo">
                <tr>
                    <td>
                        <p> Name of Apparel </p>
                    </td>
                </tr>

                <tr>
                    <td>
                    <asp:TextBox ID="txtBoxApparelName" runat="server" CssClass="txtBoxApparelName" OnTextChanged="txtBoxApparelName_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxApparelName"
                    runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>

                    <td rowspan="12">
                        <div class="apparelImgBox">
                            <img id="apparelImg" src="img/Collections/collection_1.png" alt="Plus Lantern Sleeve Floral Print Belted Dress">
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Category of Apparel </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCatApparel" runat="server" CssClass="ddlCatApparel" OnSelectedIndexChanged="ddlCatApparel_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="CategoryName" DataValueField="CategoryID"></asp:DropDownList>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="ddlCatApparel"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Size </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:DropDownList ID="ddlSizeApparel" runat="server" CssClass="ddlCatApparel" OnSelectedIndexChanged="ddlSizeApparel_SelectedIndexChanged">
                            <asp:ListItem>L</asp:ListItem>
                            <asp:ListItem>XL</asp:ListItem>
                            <asp:ListItem>XXL</asp:ListItem>
                            <asp:ListItem>XXXL</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="ddlSizeApparel"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Price (RM)</p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxApparelPrice" type="number" runat="server" CssClass="txtBoxApparelName" OnTextChanged="txtBoxApparelPrice_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxApparelPrice"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:RangeValidator ID="RangeValidatorPrice" runat="server" ControlToValidate="txtBoxApparelPrice" ErrorMessage="Please Enter Minumum Price RM 5.00"
                        MaximumValue="99999" MinimumValue="5" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Quantity </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxApparelQuantity" type="number" runat="server" CssClass="txtBoxApparelName" OnTextChanged="txtBoxApparelPrice_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxApparelQuantity"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:RangeValidator ID="RangeValidatorQuan" runat="server" ControlToValidate="txtBoxApparelQuantity" ErrorMessage="Please Enter Quantity 1 - 999"
                        MaximumValue="999" MinimumValue="1" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                    </td>
                </tr>


                <tr>
                    <td>
                        <p> Upload Apparel </p>
                    </td>
                </tr>

                <tr>
                    <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" onchange="showimagepreview(this)"/>
                    <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="FileUpload1"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.jpeg|.JPEG|.gif|.GIF|.png|.PNG|.JPG|.jpg|.bitmap|.BITMAP)$"
                        ControlToValidate="FileUpload1" runat="server" ForeColor="Red" ErrorMessage="Please select a valid image format "
                        Display="Dynamic" />
                    <br />
                        
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Button class="btnViewApparelSubmit" ID="btnViewApparelSubmit" runat="server" Text="Submit" ForeColor="#FFFFFF" OnClick="btnViewApparelSubmit_Click1" />
                    </td>
                </tr>

            </table>
        </div>
     </section>

    <script>
        const apparelToggle = document.querySelector('.toggle');

        window.setInterval(function () {

            if (apparelToggle.classList.contains('active')) {

                $(".formAddCollections").animate({ left: '150px' });

            } else {

                if (document.getElementById('formAddCollections').style.left === "150px") {
                    $(".formAddCollections").animate({ left: '300px' });
                }

            }
        }, 500);

    </script>

</asp:Content>
