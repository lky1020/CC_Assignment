<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="PostArts.aspx.cs" Inherits="CC_Assignment.PostArts"%>

<asp:Content ID="PostArts" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
     <section class="postArt" id="post">

         <!--Image Slider Start-->
         <div class="ArtSlider">
             <div class="ArtSlides">

                 <!--Radio Buttons-->
                 <input type="radio" name="radio-btn" id="radioNavImage1" />
                 <input type="radio" name="radio-btn" id="radioNavImage2" />
                 <input type="radio" name="radio-btn" id="radioNavImage3" />
                 <input type="radio" name="radio-btn" id="radioNavImage4" />
                 <!--Radio Buttons-->

                 <!--Slide Images Start-->
                      <!--Content-->
                      <div class="Art">
                          <h2 class="artTitle"><span>A</span>rtist</h2>

                          <div class="ArtQuote">
                              <p>Art is the teasure house of virtue</p>
                          </div>
                      </div>
                     <!--Content-->

                     <div class="slide first">
                         <img src="img/Artist/hl1.jpg" alt="" />
                     </div> 
                     <div class="slide">
                         <img src="img/Artist/hl8.jpg" alt="" />
                     </div> 
                      <div class="slide">
                         <img src="img/Artist/hl9.jpg" alt="" />
                     </div> 
                      <div class="slide">
                         <img src="img/Artist/hl7.jpg" alt="" />
                     </div> 
                 <!--Slide Images End-->

                 <!--Automatic navigation start-->
                 <div class="auto-nav-image">
                     <div class="auto-slide1"></div>
                     <div class="auto-slide2"></div>
                     <div class="auto-slide3"></div>
                     <div class="auto-slide4"></div>
                 </div>
                 <!--Automatic navigation End-->
             </div>
         </div>
         <!--Image Slider End-->

         <script type="text/javascript">
             var counter = 1;
             var end = false;
             setInterval(function () {
                 document.getElementById('radioNavImage' + counter).checked = true;
           
                 if (!end) {
                     counter++;
                     if (counter > 4) {
                         end = true;
                         counter = 4;
                     }
                 }
                 if (end) {
                     counter--;
                     if (counter < 1) {
                         end = false;
                         counter = 2;
                     }   
                 }
             }, 3000);
         </script>
     </section>

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/jquery.elevateZoom-3.0.8.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=gvEditImageInfo] img").elevateZoom({
                cursor: 'pointer',
                imageCrossfade: true,
            });
        });
    </script>

    <section class="displayArtSec" id="displayArtSec">
        
        <div class="GridCorner">

             <asp:GridView ID="gvEditImageInfo" runat="server" AutoGenerateColumns="False" DataKeyNames="ArtId"  CssClass="setGrid"
            ShowHeaderWhenEmpty="True" ForeColor="White" AllowPaging="true" PageSize="5" 

            OnRowEditing="gvEditImageInfo_RowEditing" OnRowCancelingEdit="gvEditImageInfo_RowCancelingEdit" OnRowUpdating="gvEditImageInfo_RowUpdating"

            OnRowDeleting="gvEditImageInfo_RowDeleting" OnSelectedIndexChanged="gvEditImageInfo_SelectedIndexChanged" style="margin-left: 0" >
            
                <%--Theme Properties --%>
        
                <headerstyle height="80px" />
                <PagerSettings  Mode="NextPreviousFirstLast" Visible="false"/>

                <Columns>
                  <%-- Art ID Col--%>

                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("ArtId") %>' runat="server" Width="70px" style="text-align: center"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Art Image Col--%>

                    <asp:TemplateField HeaderText="Image" HeaderStyle-Width="150px" >
                        <ItemTemplate>
                            <img src="<%# Eval("ArtImage") %>" width="150px" height="150px" class="thumbnail" data-image-zoom="<%# Eval("ArtImage") %>"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Art Name Col--%>

                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate> 
                            <asp:Label Text='<%# Eval("ArtName") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtArtName" Text='<%# Eval("ArtName") %>' runat="server" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtArtName"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <%-- Art Description Col--%>

                    <asp:TemplateField HeaderText="Description" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("ArtDescription") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtArtDescription" Text='<%# Eval("ArtDescription") %>' runat="server" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtArtDescription"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
 
                    <%-- Art Category Col--%>

                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="CategoryName" Text='<%# Eval("CategoryName") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Art Price Col--%>

                    <asp:TemplateField HeaderText="Price" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%--<asp:Label Text='<%# Eval("Price") %>' runat="server" type="number" Width="150px" style="text-align: center"/>--%>
                            <asp:Label runat="server" type="number" Width="150px" style="text-align: center"> 
                               RM <%# Eval("Price") %>
                            </asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtPrice" Text='<%# Eval("Price") %>' runat="server" type="number" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtPrice"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidatortxtPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Please Enter Minumum Price RM 15.00"
                            MaximumValue="99999" MinimumValue="15" Type="Double" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <%-- Art Quantity Col--%>

                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" type="number" Width="150px" style="text-align: center"/>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" Text='<%# Eval("Quantity") %>' runat="server" type="number" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtQuantity"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="RangeValidatortxtQuan" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Enter Quantity 1 - 10"
                            MaximumValue="10" MinimumValue="1" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                            
                        </EditItemTemplate>
                    </asp:TemplateField>

                     <%-- Art Action Col--%>

                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/img/Artist/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" style="text-align: center"/>
                            <asp:ImageButton ImageUrl="~/img/Artist/delete-symbol-png-7.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:ImageButton ImageUrl="~/img/Artist/save.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                            <asp:ImageButton ImageUrl="~/img/Artist/cancel-icon.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="btnArtCss">
                <asp:Button ID="btnFirstArt" runat="server" Text="<<"  CssClass="btnArt" OnClick="btnFirstArt_Click" ToolTip="First Page"/>
                <asp:Button ID="btnPreviousArt" runat="server" Text="<" CssClass="btnArt" OnClick="btnPreviousArt_Click" ToolTip="Previous Page"/>
                <asp:Button ID="btnNextArt" runat="server" Text=">" CssClass="btnArt" OnClick="btnNextArt_Click" ToolTip="Next Page"/>
                <asp:Button ID="btnLastArt" runat="server" Text=">>" CssClass="btnArt" OnClick="btnLastArt_Click" ToolTip="Last Page"/>
            </div>
        </div>
    </section>

</asp:Content>


