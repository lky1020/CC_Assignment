<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Art.aspx.cs" Inherits="CC_Assignment.Art" %>

<asp:Content ID="Art" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--Script for preview Image Start-->
    <script type="text/javascript">  
  
        function showimagepreview(input) {  
  
            if (input.files && input.files[0]) {  
                var reader = new FileReader();  
                reader.onload = function (e) {  
  
                    document.getElementsByTagName("img")[1].setAttribute("src", e.target.result);                     
                }  
                reader.readAsDataURL(input.files[0]);  
            }  
        }  
  
    </script>  
     <!--Script for preview Image End-->

    <div class="bg-Art">
        <img src="img/Artist/bg1.jpg" alt="A Men">
    </div>

     <section class="postArtTbl" id="postTbl">

         <div class="addpostform" id="addpostform">
              <h2 class="postttl">Add a New Art</h2>
              <p class="postSubTtl">Please choose the right category for your art</p>
             <hr class="ArtSepline"/>
              <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SyasyaDb %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>
         </div>
            
         
        <div class="formAddArt" id="formAddArt">

            <table class="tblArtInfo">
                <tr>
                    <td>
                        <p> Name of Art </p>
                    </td>
                </tr>

                <tr>
                    <td>
                    <asp:TextBox ID="txtBoxArtName" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtName_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxArtName"
                    runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>

                    <td rowspan="12">
                        <div class="artImgBox">
                            <img id="artImg" src="img/homepage/artWork/beautiful_women.jpg" alt="Beautiful Women">
                        </div>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Category of Art </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCatArt" runat="server" CssClass="ddlCatArt" OnSelectedIndexChanged="ddlCatArt_SelectedIndexChanged" DataSourceID="SqlDataSource1" DataTextField="CategoryName" DataValueField="CategoryID"></asp:DropDownList>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="ddlCatArt"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Description </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxArtDesc" runat="server"  textmode="Multiline"  CssClass="txtBoxArtDesc" OnTextChanged="txtBoxArtDesc_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxArtDesc"
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
                        <asp:TextBox ID="txtBoxArtPrice" type="number" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtPrice_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxArtPrice"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:RangeValidator ID="RangeValidatorPrice" runat="server" ControlToValidate="txtBoxArtPrice" ErrorMessage="Please Enter Minumum Price RM 15.00"
                        MaximumValue="99999" MinimumValue="15" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                    </td>
                </tr>

                <tr>
                    <td>
                        <p> Quantity </p>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:TextBox ID="txtBoxArtQuantity" type="number" runat="server" CssClass="txtBoxArtName" OnTextChanged="txtBoxArtPrice_TextChanged"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtBoxArtQuantity"
                        runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:RangeValidator ID="RangeValidatorQuan" runat="server" ControlToValidate="txtBoxArtQuantity" ErrorMessage="Please Enter Quantity 1 - 10"
                        MaximumValue="10" MinimumValue="1" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                    </td>
                </tr>


                <tr>
                    <td>
                        <p> Upload Art </p>
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
                        <asp:Button class="btnViewArtSubmit" ID="btnViewArtSubmit" runat="server" Text="Submit" ForeColor="#FFFFFF" OnClick="btnViewArtSubmit_Click1" />
                    </td>
                </tr>

            </table>
        </div>
     </section>

    <script>
        const artToggle = document.querySelector('.toggle');

        window.setInterval(function () {

            if (artToggle.classList.contains('active')) {

                $(".formAddArt").animate({ left: '150px' });

            } else {

                if (document.getElementById('formAddArt').style.left === "150px") {
                    $(".formAddArt").animate({ left: '300px' });
                }

            }
        }, 500);

    </script>

</asp:Content>
