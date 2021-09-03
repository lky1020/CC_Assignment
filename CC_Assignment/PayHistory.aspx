<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="PayHistory.aspx.cs" Inherits="CC_Assignment.PayHistory" %>

<asp:Content ID="Payment" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div class="payHis_page">
        <div class="payHis_page_title">
            <h1> PURCHASE HISTORY </h1>
        </div>
        <br /> <br />
        <%-- Payment History GridView (Table) --%>
        <asp:Label ID ="historyEmpty" runat="server" Text="No Payment History ... You haven't purchase any apparel yet" Visible="false">  </asp:Label>

        <asp:GridView ID="gvPayHistory" runat="server"  AutoGenerateColumns="false" ForeColor="White" DataKeyNames="paymentId" ShowHeaderWhenEmpty="false"
             GridLines ="none" CssClass="payHis_gv">
            <Columns>
                <%-- GridView ApparelImage --%>
                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Date Paid" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate >  
                        <asp:Label ID="his_apparelImg" runat="server">
                            <%# Eval("datePaid", "{0:MM/dd/yyyy}") %>
                        </asp:Label>  
                    </ItemTemplate>
                </asp:TemplateField> 

                <%-- GridView ApparelImage --%>
                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Tracking Number" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate >  
                        <asp:Label ID="his_orderID" runat="server" Text="-">
                        </asp:Label>  
                        <asp:TextBox ID="his_orderID1" runat="server" Text='<%# Eval("OrderDetailId") %>' Visible="false">
                        </asp:TextBox>  

                    </ItemTemplate>
                </asp:TemplateField> 

                <%-- Apparel Name --%>
                <asp:TemplateField ItemStyle-Width="20%" HeaderText="Apparel Name" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                        <asp:Label ID="his_apparelName" runat="server"> 
                            <p style="font-size:18px"><%#Eval("ApparelName")%></p>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <%-- Unit Price --%>
                <asp:TemplateField ItemStyle-Width="20%" HeaderText="Unit Price" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                            <asp:Label runat="server"> RM <%# Eval("Price") %></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                 <%-- Qty --%>
                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Qty" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                        <asp:Label ID="his_itemqty" runat="server"> 
                            <%#Eval("qtySelected")%>
                        </asp:Label> 
                        <asp:TextBox ID="his_qty" runat="server" Text='<%# Eval("qtySelected") %>' Visible="false">
                        </asp:TextBox>  
                    </ItemTemplate>
                </asp:TemplateField>

               
                <%-- GridView Apparel Subtotal --%>
                <asp:TemplateField ItemStyle-Width="11%" HeaderText="Paid Amount" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" 
                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="payHis_gv_item">
                    <ItemTemplate>
                         <asp:Label runat="server"> RM <%# Eval("Subtotal") %></asp:Label>
                        <asp:TextBox ID="his_subtotal" runat="server" Text='<%# Eval("Subtotal") %>' Visible="false">
                        </asp:TextBox>  
                        </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>

