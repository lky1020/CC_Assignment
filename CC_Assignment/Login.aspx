<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CC_Assignment.Login" %>
<%@ Register TagPrefix="SuperLogin" TagName="Login" Src="~/LoginControl.ascx" %>

<asp:Content ID="Login" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <%-- Login Control --%>
        <SuperLogin:Login ID="ctlLogin" runat="server" />

    </section>
</asp:Content>
