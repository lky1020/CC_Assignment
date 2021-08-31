<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationControl.ascx.cs" Inherits="CC_Assignment.RegistrationControl" %>

<asp:ScriptManager ID="scriptmanagerRegister" runat="server"></asp:ScriptManager>  
<div class="registerbox" id="registerbox">
	<div class="avatar"></div>
	<h1>Register Here</h1>

	<asp:UpdatePanel ID="updatepnl" runat="server">  
		<ContentTemplate> 
			<div class="registerForm">
				<div>
					<asp:Label ID="lblUsername" runat="server" CssClass="registerLabel">Username</asp:Label>
					<asp:TextBox ID="txtUsername" runat="server" placeholder="Enter Your Username" CssClass="txtStyle"></asp:TextBox>

					<asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
						ControlToValidate="txtUsername" ErrorMessage="Please Enter Your Name" 
						ForeColor="Red" Display="Dynamic">
					</asp:RequiredFieldValidator>

					<%-- Server Side Validation only for existing user check --%>
					<asp:CustomValidator ID="cvUsername" runat="server" 
						ControlToValidate="txtUsername"
						OnServerValidate="cvUsername_ServerValidate"
						ForeColor="Red"
						Display="Dynamic">
					</asp:CustomValidator>
				</div>

				<div>
					<asp:Label ID="lblEmail" runat="server" CssClass="registerLabel">Email Address</asp:Label>
					<asp:TextBox ID="txtEmail" runat="server" placeholder="someone@exmaple.com" CssClass="txtStyle"></asp:TextBox>
							
					<asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
						ControlToValidate="txtEmail" ErrorMessage="Please Enter Your Email" 
						ForeColor="Red" Display="Dynamic">
					</asp:RequiredFieldValidator>

					<%-- Server Side Validation only for existing email --%>
					<asp:CustomValidator ID="cvEmail" runat="server" 
						ControlToValidate="txtEmail"
						ClientValidationFunction="ValidateEmail"
						OnServerValidate="cvEmail_ServerValidate"
						ForeColor="Red"
						Display="Dynamic">
					</asp:CustomValidator>
				</div>

				<div>
					<asp:Label ID="lblPassword" runat="server" CssClass="registerLabel">Password</asp:Label>
					<asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder="Enter Your Password" CssClass="txtStyle"></asp:TextBox>
					<i class="fas fa-eye" onclick="passwordFunction()"></i>	

					<asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
						ControlToValidate="txtPassword" ErrorMessage="Please Enter Your Password!" 
						ForeColor="Red" Display="Dynamic">
					</asp:RequiredFieldValidator>
				</div>
						
				<asp:Label ID="lblGender" runat="server" CssClass="registerLabel">Gender</asp:Label>
				<br/>
					<div class="gender">
						<asp:RadioButton GroupName="gender" ID="rdMale" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
						<label id="lblMale" class="registerGenderLabel" onClick="selectMale()">Male</label>

						<asp:RadioButton GroupName="gender" ID="rdFemale" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
						<label id="lblFemale" class="registerGenderLabel" onClick="selectFemale()">Female</label>
					</div>
					<asp:Label ID="lblGender_Validation" runat="server" CssClass="registerValidation"></asp:Label>
				<br/>

				<asp:Label ID="lblRole" runat="server" CssClass="registerLabel">Role</asp:Label>
				<br/>
					<div class="role">
						<asp:RadioButton GroupName="role" ID="rdManagement" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
						<label id="lblManagement" class="registerRoleLabel" onClick="selectManagement()">Management</label>

						<asp:RadioButton GroupName="role" ID="rdCustomer" runat="server" CssClass="registerRadio" AutoPostBack="false"/>
						<label id="lblCustomer" class="registerRoleLabel" onClick="selectCustomer()">Customer</label>
					</div>
					<asp:Label ID="lblRole_Validation" runat="server" CssClass="registerValidation"></asp:Label>
				<br/>
						
				<asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btnStyle" OnClick="btnRegister_Click"/>
			</div>
		</ContentTemplate>  
	</asp:UpdatePanel> 

	<div style="text-align: center; margin-top: 15px;">
		<a href="Login.aspx">Login</a><br/>
		<a href="Homepage.aspx">Cancel Register</a>
	</div>
</div>

<img src="img/login_register/nightCity_Register.jpg" />

<script type="text/javascript">
	//Window Load (change ui after register)
	window.addEventListener("load", function () {
		//Initialize the gender and role radio button
		selectMale();
		selectCustomer();

	})

    //Show Password
	function passwordFunction() {
        var x = document.getElementById('<%= txtPassword.ClientID %>');

		if (x.type == "password") {
			x.setAttribute("type", "text");

		} else {
            x.setAttribute("type", "password");
        }
	}

	//Select the gender
	function selectMale() {
		document.getElementById("<%=rdMale.ClientID %>").checked = true;
		document.getElementById("lblMale").classList.add("checked");

		document.getElementById("<%=rdFemale.ClientID %>").checked = false;
        document.getElementById("lblFemale").classList.remove("checked");
	}

	function selectFemale() {
		document.getElementById("<%=rdMale.ClientID %>").checked = false;
        document.getElementById("lblMale").classList.remove("checked");

		document.getElementById("<%=rdFemale.ClientID %>").checked = true;
        document.getElementById("lblFemale").classList.add("checked");
	}

	//Select the role
	function selectManagement() {
        document.getElementById("<%=rdManagement.ClientID %>").checked = true;
        document.getElementById("lblManagement").classList.add("checked");

        document.getElementById("<%=rdCustomer.ClientID %>").checked = false;
        document.getElementById("lblCustomer").classList.remove("checked");
	}

	function selectCustomer() {
        document.getElementById("<%=rdManagement.ClientID %>").checked = false;
        document.getElementById("lblManagement").classList.remove("checked");

        document.getElementById("<%=rdCustomer.ClientID %>").checked = true;
        document.getElementById("lblCustomer").classList.add("checked");
	}

    //Client-side validation
    function ValidateEmail(source, args) {
        var txtEmail = document.getElementById('<%=txtEmail.ClientID%>');
        var emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;

        if (emailRegex.test(txtEmail.value) == false) {
            alert('Invalid Email Format!');
            source.innerHTML = "Invalid Email Format!";
            args.IsValid = false;

        } else {
            args.IsValid = true;
        }
    }
</script>