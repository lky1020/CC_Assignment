<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="CC_Assignment.Cart" %>

<asp:Content ID="Cart" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cart_page_not_overflow">
        <div class="cart_page">

            <div class="cart_page_title">
                <h1> CART</h1>
            </div>
            <br /> 

            <%-- Cart GridView (Table) --%>
            <asp:Label ID ="cartEmpty" runat="server" Text="Your Cart is Empty ..." Visible="false">  </asp:Label>

            <asp:GridView ID="gvCart" runat="server"  AutoGenerateColumns="false" ForeColor="White" DataKeyNames="orderDetailId" 
                ShowHeaderWhenEmpty="false" OnRowEditing="gvCart_RowEditing" OnRowCancelingEdit="gvCart_RowCancelingEdit"
               OnRowUpdating="gvCart_RowUpdating" OnRowDeleting="gvCart_RowDeleting">
                <Columns>
                     <%-- GridView CheckBox --%>
                    <asp:TemplateField ItemStyle-Width="5%" ItemStyle-Height="120px" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>  
                          <asp:CheckBox ID="CheckBoxHead" AutoPostBack="true" OnCheckedChanged="CheckBoxHead_CheckedChanged" runat="server" /> 
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:CheckBox ID="chkItems" runat="server" Font-Size="XX-Large" AutoPostBack="True" OnCheckedChanged="chkItems_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView ApparelImage --%>
                    <asp:TemplateField ItemStyle-Width="14%" HeaderText="Apparel" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>  
                            <asp:ImageButton ID="cart_apparelImg" runat="server"  ImageUrl='<%# Eval("ArtImage")%>' Height="100" Width="150" OnClick="cart_apparelImg_click" CommandArgument='<%# Eval("ApparelId")%>' CommandName="viewdetails" CssClass="margin_5"/>
                        </ItemTemplate>
                    </asp:TemplateField> 

                    <%-- GridView ApparelName & Desc --%>
                    <asp:TemplateField ItemStyle-Width="20%" HeaderText="Description" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="cart_apparelName" runat="server"> 
                                <p style="font-size:18px; margin-bottom:10px;"><%#Eval("ArtName")%></p>
                            </asp:Label>

                            <asp:Label ID="cart_apparelDes" runat="server"> 
                                <p style="font-size:15px; color:lightgrey "><%#Eval("ArtDescription")%></p>
                            </asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView Apparel Price --%>
                    <asp:TemplateField ItemStyle-Width="11%" HeaderText="Price" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center"> 
                        <ItemTemplate >
                            <asp:Label runat="server"> RM <%# Eval("Price") %></asp:Label>
                            <asp:TextBox ID="cart_apparelPrice"  Text='<%# Eval("Price") %>' runat="server" Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- GridView Apparel Qty Selected --%>
                    <asp:TemplateField ItemStyle-Width="8%" HeaderText="Qty" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate >
                            <asp:Label runat="server"  type="number"> 
                                <%#Eval("qtySelected")%> 
                            </asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox  ID="cart_qtySelect" Text='<%#Eval("qtySelected")%>' runat="server"  type="number"> 
                            </asp:TextBox>
                        </EditItemTemplate>

                    </asp:TemplateField>
           
                    <%-- GridView Apparel Subtotal --%>
                    <asp:TemplateField ItemStyle-Width="11%" HeaderText="Subtotal" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                             <asp:Label runat="server"> RM <%# Eval("SubTotal") %></asp:Label>
                            <asp:TextBox ID="cart_apparelSubPrice"  Text='<%# Eval("SubTotal") %>' runat="server" Visible="false"></asp:TextBox>
                          </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Edit Qty / Delete Apparel --%>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#484848">
                        <ItemTemplate>
                            <asp:ImageButton ID="editBtn" ImageUrl="~/img/icon/edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" style="text-align: center"/>
                            &nbsp;
                            <asp:ImageButton class="cart_page_deleteBtn" ImageUrl="~/img/icon/dusbin.png" runat="server" CommandName="Delete" ToolTip="Delete" />
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:ImageButton ImageUrl="~/img/icon/done.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button Text=" X " runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <%-- GridView Apparel TotalPrice --%>
            <div class="cart_page_totalPrice" >
                <asp:Label ID="totalPrice" runat="server" Text="Total : RM 0.00"></asp:Label>
                <br />
                <asp:Button class="cart_orderbtn" ID="cart_orderBtn" runat="server" Text=" Order " OnClick="cart_orderBtn_Click"/>
            </div>
        </div>
    </div>
    
</asp:Content>

