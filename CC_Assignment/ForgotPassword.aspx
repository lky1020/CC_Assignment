<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CC_Assignment.ForgotPassword" %>
<%@ Register TagPrefix="SuperForgotPassowrd" TagName="ForgotPassword" Src="~/ForgotPasswordControl.ascx" %>

<asp:Content ID="ForgotPassword" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">
        
    <%-- Forgot Password Control --%>
    <SuperForgotPassowrd:ForgotPassword ID="ctlForgotPassword" runat="server" />

    </section>
</asp:Content>
