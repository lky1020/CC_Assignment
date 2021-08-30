<%@ Page Language="C#" MasterPageFile="~/WAD.Master"  AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="CC_Assignment.Registration" %>
<%@ Register TagPrefix="SuperRegistration" TagName="Registration" Src="~/RegistrationControl.ascx" %>

<asp:Content ID="Registration" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

		<%--Registration Control--%>
		<SuperRegistration:Registration ID="ctlRegistration" runat="server"/>

	</section>
</asp:Content>
