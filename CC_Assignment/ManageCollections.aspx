<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="ManageCollections.aspx.cs" Inherits="CC_Assignment.ManageCollections"%>

<asp:Content ID="ManageCollections" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
     <section class="manageApparel" id="manage">

         <!--Image Slider Start-->
         <div class="Slider">
             <div class="Slides">

                 <!--Radio Buttons-->
                 <input type="radio" name="radio-btn" id="radioNavImage1" />
                 <input type="radio" name="radio-btn" id="radioNavImage2" />
                 <input type="radio" name="radio-btn" id="radioNavImage3" />
                 <input type="radio" name="radio-btn" id="radioNavImage4" />
                 <!--Radio Buttons-->

                 <!--Slide Images Start-->
                      <!--Content-->
                      <div class="Apparel">
                          <h2 class="apparelTitle"><span>A</span>pparel</h2>

                          <div class="ApparelQuote">
                              <p>Score the Latest Trends!</p>
                          </div>
                      </div>
                     <!--Content-->

                     <div class="slide first">
                         <img src="img/Collections/banner_1.png" alt="" />
                     </div> 
                     <div class="slide">
                         <img src="img/Collections/banner_2.png" alt="" />
                     </div> 
                      <div class="slide">
                         <img src="img/Collections/banner_3.png" alt="" />
                     </div> 
                      <div class="slide">
                         <img src="img/Collections/banner_4.jpg" alt="" />
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

    <section class="displayApparelSec" id="displayApparelSec">
        
        <div class="GridCorner">

             <asp:GridView ID="gvEditImageInfo" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"  CssClass="setGrid"
            ShowHeaderWhenEmpty="True" ForeColor="White" AllowPaging="true" PageSize="5" 

            OnRowEditing="gvEditImageInfo_RowEditing" OnRowCancelingEdit="gvEditImageInfo_RowCancelingEdit" OnRowUpdating="gvEditImageInfo_RowUpdating"

            OnRowDeleting="gvEditImageInfo_RowDeleting" OnSelectedIndexChanged="gvEditImageInfo_SelectedIndexChanged" style="margin-left: 0" >
            
                <%--Theme Properties --%>
        
                <headerstyle height="80px" />
                <PagerSettings  Mode="NextPreviousFirstLast" Visible="false"/>

                <Columns>
                  <%-- ID Col--%>

                    <asp:TemplateField HeaderText="ID" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Id") %>' runat="server" Width="70px" style="text-align: center"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Image Col--%>

                    <asp:TemplateField HeaderText="Image" HeaderStyle-Width="150px" >
                        <ItemTemplate>
                            <img src="<%# Eval("Image") %>" width="150px" height="150px" class="thumbnail" data-image-zoom="<%# Eval("Image") %>"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Name Col--%>

                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate> 
                            <asp:Label Text='<%# Eval("Name") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtApparelName" Text='<%# Eval("Name") %>' runat="server" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtApparelName"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <%-- Size Col--%>

                    <asp:TemplateField HeaderText="Size" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Size") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>
                    </asp:TemplateField>
 
                    <%-- Category Col--%>

                    <asp:TemplateField HeaderText="Category" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="CategoryName" Text='<%# Eval("CategoryName") %>' runat="server" Width="150px" style="text-align: center"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Price Col--%>

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
                            <asp:RangeValidator ID="RangeValidatortxtPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Please Enter Minumum Price RM 5.00"
                            MaximumValue="99999" MinimumValue="5" Type="Double" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <%-- Quantity Col--%>

                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" type="number" Width="150px" style="text-align: center"/>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" Text='<%# Eval("Quantity") %>' runat="server" type="number" Width="150px" style="text-align: center"/>
                            <asp:RequiredFieldValidator ErrorMessage="Required" ControlToValidate="txtQuantity"
                            runat="server" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:RangeValidator ID="RangeValidatortxtQuan" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Enter Quantity 1 - 999"
                            MaximumValue="999" MinimumValue="1" Type="Integer" Display="Dynamic" ForeColor="Red"></asp:RangeValidator>
                            
                        </EditItemTemplate>
                    </asp:TemplateField>

                     <%-- Action Col--%>

                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/img/Collections/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" style="text-align: center"/>
                            <asp:ImageButton ImageUrl="~/img/Collections/delete-symbol-png-7.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:ImageButton ImageUrl="~/img/Collections/save.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                            <asp:ImageButton ImageUrl="~/img/Collections/cancel-icon.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="btnApparelCss">
                <asp:Button ID="btnFirstApparel" runat="server" Text="<<"  CssClass="btnApparel" OnClick="btnFirstApparel_Click" ToolTip="First Page"/>
                <asp:Button ID="btnPreviousApparel" runat="server" Text="<" CssClass="btnApparel" OnClick="btnPreviousApparel_Click" ToolTip="Previous Page"/>
                <asp:Button ID="btnNextApparel" runat="server" Text=">" CssClass="btnApparel" OnClick="btnNextApparel_Click" ToolTip="Next Page"/>
                <asp:Button ID="btnLastApparel" runat="server" Text=">>" CssClass="btnApparel" OnClick="btnLastApparel_Click" ToolTip="Last Page"/>
            </div>
        </div>
    </section>

</asp:Content>


