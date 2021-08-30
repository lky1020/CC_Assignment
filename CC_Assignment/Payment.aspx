<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="CC_Assignment.Payment" %>

<asp:Content ID="Payment" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="payment_wrap">
        <div class="payment_wrapper">
            <div class="order_info">
                <div class="order_info_content">
                    <p><b>Order Summary </b></p>

                    <asp:GridView ID="gvPayment" runat="server"  AutoGenerateColumns="false" ForeColor="White" DataKeyNames="orderDetailId"
                        GridLines ="none" CssClass="pay_gv">
                        <Columns>
                            <%-- GridView CheckBox --%>
                            <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="pay_gvItem1">
                                        <strong>  
                                            <asp:Label runat="server" ID="artItem_Name1" Text='<%# Eval("ArtName") %>' Width="60%"></asp:Label>
                                             <asp:TextBox runat="server"  ID="artItem_Name" Text='<%# Eval("ArtName") %>' Visible="false"></asp:TextBox>
                                           
                                        </strong>

                                        <br />

                                        <div class="pay_gvItem2">
                                            <asp:Label runat="server"  ID="item_order_summary_price1" >RM <%# Eval("Price") %> x</asp:Label>
                                            <asp:TextBox runat="server"  ID="item_order_summary_price" Text='<%# Eval("Price") %>' Visible="false"></asp:TextBox>
                                            <asp:Label runat="server"> <%# Eval("qtySelected") %></asp:Label>
                                            <asp:TextBox runat="server" ID="item_order_summary_qty" Text='<%# Eval("qtySelected") %>' Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <div class="pay_gvItem3">
                                        <strong> 
                                            <asp:Label runat="server">  RM <%# Eval("Subtotal") %></asp:Label>
                                            <asp:TextBox runat="server" ID="order_subtotal" Text='<%# Eval("Subtotal") %>' Visible="false">  </asp:TextBox>
                                        </strong> 
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField> 
                        </Columns>
                    </asp:GridView>
                      
                    <div >
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" style="color:antiquewhite">Total Art Price</asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" Width="40px" style="color:antiquewhite"> : </asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" ID="pay_subtotal" style="color:antiquewhite"> </asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label runat="server" style="color:antiquewhite"> Delivery Fees</asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" Width="40px" style="color:antiquewhite"> : </asp:Label>
                                </td>

                                <td>
                                    <asp:Label runat="server" ID="deliverly_fees" style="color:antiquewhite"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>
                                        <asp:Label runat="server" style="color:antiquewhite">Total Payment</asp:Label>
                                    </strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label runat="server" Width="40px" style="color:antiquewhite"> : </asp:Label>
                                    </strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label runat="server" ID="total_payment" style="color:antiquewhite"> </asp:Label>
                                    </strong>
                                </td>
                            </tr>
                        </table>
                    </div>   
                </div>
            </div>
    
            <div class="checkout_form">
                <p>Payment Section</p>

                <asp:RadioButtonList ID="CardRadioButtonList" runat="server" Font-Size="15px" ForeColor="#999999" RepeatDirection="Horizontal" Width="80%">
                    <asp:ListItem Selected="True">   Debit Card </asp:ListItem>
                    <asp:ListItem>   Credit Card</asp:ListItem>
                </asp:RadioButtonList>

                <div class="payment_section">
                    <p style="font-size:10px;margin-bottom:0px">Address</p>
                    <!--input type="text" ID="Address" placeholder="Address" /-->
                    <asp:TextBox ID="Address" runat="server" Height="40px" class="text_box"/>
                    <asp:RequiredFieldValidator ID="Address_RequiredField" runat="server" ErrorMessage="Address is Required." ForeColor="Red" ControlToValidate="Address">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Address_RegularExpression" runat="server" ErrorMessage="Invalid Address" 
                        ForeColor="Red" ControlToValidate="Address" ValidationExpression="^[a-zA-Z0-9/., -]*$" Font-Size="11px"></asp:RegularExpressionValidator>
                </div>

                <div class="payment_section">
                    <p style="font-size:10px;margin-bottom:0px">Card Number</p>
                    <!-- input type="text" placeholder="Card Number" /-->
                    <asp:TextBox ID="Card_Number" runat="server" Height="40px" class="text_box"/>
                    <asp:RequiredFieldValidator ID="CardNum_RequiredField" runat="server" ErrorMessage="Card Number is Required." ForeColor="Red" ControlToValidate="Card_Number">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="CardNum_RegularExpression" runat="server" ErrorMessage="Invalid Card Number (E.g. 4567 8738 4738 4596)" 
                        ForeColor="Red" ControlToValidate="Card_Number" ValidationExpression="^[4-5][0-9]{3} [0-9]{4} [0-9]{4} [0-9]{4}$" Font-Size="11px"></asp:RegularExpressionValidator>
                    
                </div>

                <div class="payment_section">
                    <p style="font-size:10px;margin-bottom:0px">Card Holder Name</p>
                    <!-- input type="text" placeholder="Cardholder Name" /-->
                    <asp:TextBox ID="CardHolderName" runat="server" Height="40px" class="text_box"/>
                    <asp:RequiredFieldValidator ID="CardHolderName_RequiredField" runat="server" ErrorMessage="Card Holder Name is Required." ForeColor="Red" ControlToValidate="CardHolderName">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="CardHolderName_RegularExpression" runat="server" ErrorMessage="Invalid Card Holder Name" 
                        ForeColor="Red" ControlToValidate="CardHolderName" ValidationExpression="^[a-zA-Z/ ]*$" Font-Size="11px"></asp:RegularExpressionValidator>
               
                </div>

                <div class="payment_section payment_last_section">
                    <div>
                        <p style="font-size:10px;margin-bottom:0px">Expiry Date (E.g. 02/20/2025)</p>
                        <!-- input type="text" placeholder="Expiry Date" /-->
                        <asp:TextBox ID="Exp_Date" runat="server" Height="40px" class="payment_item_box" TextMode="Date"/>
                        <asp:RequiredFieldValidator ID="ExpDate_RequiredField" runat="server" ErrorMessage="Expiry Date is Required." ForeColor="Red" ControlToValidate="Exp_Date" Font-Size="11px">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="ExpDate_RangeValidator" runat="server" ErrorMessage="Invalid Expiry Date!" ControlToValidate="Exp_Date" Type="Date"
                            ForeColor="Red" Font-Size="11px"></asp:RangeValidator>
                       
                    </div>
                    <div>
                        <p style="font-size:10px;margin-bottom:0px">CVV</p>
                        <!-- input type="password" placeholder="CVV" /-->
                        <asp:TextBox ID="CVV" runat="server" Height="40px" class="payment_item_box" TextMode="Password"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="CVV is Required." ForeColor="Red" ControlToValidate="CVV">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="CVV_RegularExpression" runat="server" ErrorMessage="Invalid CVV" 
                            ForeColor="Red" ControlToValidate="CVV" ValidationExpression="^[0-9]{3}$" Font-Size="11px"></asp:RegularExpressionValidator>
               </div>
                </div>

                <div>
                    <asp:Button ID="pay_Btn" Text="Pay" class="payment_btn" runat="server" OnClick="pay_Btn_Click"/>
                </div>   
                
            </div>
        </div>
    </div>

</asp:Content>
