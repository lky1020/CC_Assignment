<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="CC_Assignment.Homepage" %>

<asp:Content ID="Homepage" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Background --%>
    <section class="banner" id="banner">
        <div class="content">
            <h2>Score the Latest Trends!</h2>
            <p>
                In the road to capture all the best moments in the life.
                The best moments in life always surround us, helping us, supporting us, and giving us strength.
            </p>
            <a href="#apparel" class='btn'>Our Collections</a>
        </div>

        <img src="img/video/sky.gif"/>
    </section>

    <%-- About --%>
    <section class="about" id="about">
        <div class="row">
            <div class="col50">
                <h2 class="titleText"><span>A</span>bout Us</h2>
                <p> &emsp;&emsp;&emsp;
                    Syasha Design is an online shop selling plus size 
                    apparel for women. Syasha Design are using Microsoft
                    Azure to support its daily business. Syasha Design intend 
                    selling plus size apparel for women to promote to all the world. 
                    In addition, Syasha Design also intent to launch their plus size apparel
                    to lets the world explore the beauty of plus size apparel and
                    purchase them.
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    Syasha Design is founder by Syasya and it is design by 
                    5 students which is Lim Kah Yee, Joan Hau, 
                    Cheong Yin Lam, Lee Ling and Hiew Long Shun. 
                    Currently, we are studying in the software engineering 
                    course (RSF) of Tunku Abdul Rahman University College (TARUC).
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    This Syasha Design website is one of our assignment in RSF2 Year 3 
                    Sem 1. This website is used to selling plus size apparel for women. 
                    In this website, it is about the Apparel which is to promote plus size 
                    apparel for women. All of the data in the Syasha Design will store in
                    Microsoft Azure.
                </p>
            </div>

            <div class="col50">
                <div class="imgBox">
                    <img src="img/homepage/aboutUs/aboutUs.jpg" alt="About Us">
                </div>
            </div>
        </div>
    </section>

    <%-- Collections --%>
    <section class="apparel" id="apparel">
        <div class="title">
            <h2 class="titleText"><span>C</span>ollections</h2>

            <p> 
                The best apparel that launched by Syasha. Captured the 
                best moment in our life.
            </p>
        </div>

        <div class="content">
            <asp:DataList ID="ApparelDataList" runat="server" DataKeyField="Id" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="60" HorizontalAlign="Center" CellPadding="3">
                <ItemTemplate>
                    <table id="apparel-table">
                        <tr>
                            <td>
                                <asp:Image ID="Image" runat="server" CssClass="apparel-gallery-image" ImageUrl='<%# Eval("Image") %>' />
                            </td>
                        </tr>
                        <tr class="text-a1 padding-b15">
                            <td>
                                <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' />
                            </td>
                        </tr>
                        <tr class="text-a3">
                            <td>RM
                                <asp:Label ID="PriceLabel" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Price"))  %>' />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <div class="title">
            <asp:Button ID="btnViewAll" runat="server" CssClass="btnViewAll" Text="View All" OnClick="btnViewAll_Click" causesvalidation="false" />
        </div>
    </section>

    <%-- Contact --%>
    <section class="contact" id="contact">
        <div class="title">
            <h2 class="titleText"><span>C</span>ontact Us</h2>

            <p> 
                Feel free to Contact Us. 
            </p>
        </div>

        <asp:ScriptManager ID="scriptmanagerContact" runat="server"></asp:ScriptManager>  
        <div class="contactForm">
            <asp:UpdatePanel ID="updatepnl" runat="server">  
                <ContentTemplate> 
                    <h3>Send Message</h3>
                    <div class="inputBox">
                        <asp:TextBox ID="txtContactName" CssClass="contactInput" runat="server" placeholder="Your Name"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactName" runat="server" 
							ControlToValidate="txtContactName" ErrorMessage="Please Enter Your Name" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>
                    </div>

                    <div class="inputBox">
                        <asp:TextBox ID="txtContactEmail" CssClass="contactInput" runat="server" placeholder="Your Email"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactEmail" runat="server" 
							ControlToValidate="txtContactEmail" ErrorMessage="Please Enter Your Email" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>

						<asp:CustomValidator ID="cvContactEmail" runat="server" 
							ControlToValidate="txtContactEmail"
                            ClientValidationFunction="ValidateEmail"
							OnServerValidate="cvContactEmail_ServerValidate"
							ForeColor="Red"
							Display="Dynamic">
						</asp:CustomValidator>
                    </div>

                    <div class="inputBox">
                        <asp:TextBox ID="txtContactComment" TextMode="MultiLine" CssClass="contactInput" runat="server" placeholder="Your Comment"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactComment" runat="server" 
							ControlToValidate="txtContactComment" ErrorMessage="Please Enter Your Message" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>
                    </div>

                    <div class="inputBox">
                        <asp:Button ID="btnContactSubmit" CssClass="contactSubmit" runat="server" Text="Submit" OnClientClick="changeSubmitText()"  OnClick="btnContactSubmit_Click"/>
                    </div>

                </ContentTemplate>  
            </asp:UpdatePanel> 
        </div>

    </section>

    <script type="text/javascript">
        function changeSubmitText() {
            document.getElementById('<%=btnContactSubmit.ClientID%>').value = "Sending";
            document.getElementById('<%=btnContactSubmit.ClientID%>').style.cursor = "default";
        }

        //Client-side validation
        function ValidateEmail(source, args) {
            var txtEmail = document.getElementById('<%=txtContactEmail.ClientID%>');
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

</asp:Content>
