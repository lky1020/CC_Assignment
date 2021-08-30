<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="CC_Assignment.LoginControl" %>

<div class="loginForm">
	<div class="loginbox">
		<h1>Login Account</h1>
		<div class="userPic"></div>	
				
		<asp:ScriptManager ID="scriptmanagerLogin" runat="server"></asp:ScriptManager>
				
		<asp:UpdatePanel ID="updatepnl" runat="server">  
			<ContentTemplate> 
				<div class="loginFormContent">
					<div style="margin-bottom: 15px;">
						<p>Email or Username</p>
						<asp:TextBox ID="txtEmail_Username" runat="server" placeholder="Enter Email/Username"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfvEmail_Username" runat="server" 
							ControlToValidate="txtEmail_Username" ErrorMessage="Please Enter Email or Username" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>

						<%-- Server Side Validation only for existing user check --%>
						<asp:CustomValidator ID="cvEmail_Username" runat="server" 
							ControlToValidate="txtEmail_Username"
							OnServerValidate="cvEmail_Username_ServerValidate" ForeColor="Red"
							Display="Dynamic">
						</asp:CustomValidator>
					</div>
							
					<div style="margin-bottom: 15px;">
						<p>Password</p> 
						<div>
							<asp:TextBox ID="txtPassword" runat="server" placeholder="Enter Your Password" TextMode="Password" CssClass="inlinePassword"></asp:TextBox>
							<i class="fas fa-eye inlinePassword" onclick="passwordFunction()"></i>	
						</div>
							
						<asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
							ControlToValidate="txtPassword" ErrorMessage="Please Enter Password" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>

						<%-- Server Side Validation only for password check --%>
						<asp:CustomValidator ID="cvPassword" runat="server" 
							ControlToValidate="txtPassword"
							OnServerValidate="cvPassword_ServerValidate"
							ForeColor="Red"
							Display="Dynamic">
						</asp:CustomValidator>
					</div>

					<asp:Button ID="btnLogin" runat="server" Text="Login"/>	
				</div>
			</ContentTemplate>  
		</asp:UpdatePanel> 
					
		<a href="ForgotPassword.aspx">Forget Password</a>
		<br/>
		<a href="Registration.aspx">Create a new Account</a>
	</div>
</div>

<img src="img/login_register/nightCity_Login.jpg" />

<script type="text/javascript">
    //Show Password
    function passwordFunction() {
        var x = document.getElementById('<%= txtPassword.ClientID %>');

		if (x.type == "password") {
			x.setAttribute("type", "text");

		} else {
            x.setAttribute("type", "password");
        }
	}
</script>