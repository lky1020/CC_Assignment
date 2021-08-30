<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="CC_Assignment.Homepage" %>

<asp:Content ID="Homepage" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Background --%>
    <section class="banner" id="banner">
        <div class="content">
            <h2>Always Choose The Best</h2>
            <p>
                In the road to capture all the best moments in the life.
                The best moments in life always surround us, helping us, supporting us, and giving us strength.
            </p>
            <a href="#artworks" class='btn'>Our ArtWorks</a>
        </div>

        <video src="img/video/sky.mp4" muted loop autoplay></video>
    </section>

    <%-- About --%>
    <section class="about" id="about">
        <div class="row">
            <div class="col50">
                <h2 class="titleText"><span>A</span>bout Us</h2>
                <p> &emsp;&emsp;&emsp;
                    Quad-Core ArtWork Sales and Display Gallery
                    is a company that provides a platform for all
                    artists around the world to launch their artworks, 
                    explore the beauty of artworks and collect them.
                    Quad-Core ArtWork Sales and Display Gallery intend
                    to collect and capture all the beauty moments in
                    the life to promote to all the world. 
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    Quad-Core ArtWork Sales and Display Gallery (ASG)
                    is founder by 4 students which is Lim Kah Yee, Joan Hau, 
                    Cheong Yin Lam & Lee Ling. Currently, we are studying in
                    the software engineering course (RSF) of Tunku Abdul Rahman 
                    University College (TARUC).
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    This Quad-Core ASG. website is one of our assignment in RSF2 Year 2 
                    Sem 3. This website is used to apply all the things that we learn 
                    about the Asp.Net website. In this website, it is about the Artworks
                    Sales and Display gallery which is to promote Artwork and also offers 
                    the opportunity for Art Lover to purchase them.
                </p>
            </div>

            <div class="col50">
                <div class="imgBox">
                    <img src="img/homepage/aboutUs/aboutUs.jpg" alt="About Us">
                </div>
            </div>
        </div>
    </section>

    <%-- Artwork --%>
    <section class="artworks" id="artworks">
        <div class="title">
            <h2 class="titleText"><span>A</span>rtWorks</h2>

            <p> 
                The best ArtWorks that launched by Artists. Captured the 
                best moment in our life.
            </p>
        </div>

        <div class="content">
            <asp:DataList ID="ArtWorkDataList" runat="server" DataKeyField="ArtId" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="60" HorizontalAlign="Center" CellPadding="3">
                <ItemTemplate>
                    <table id="artwork-table">
                        <tr>
                            <td>
                                <asp:Image ID="ArtImage" runat="server" CssClass="artwork-gallery-image" ImageUrl='<%# Eval("ArtImage") %>' />
                            </td>
                        </tr>
                        <tr class="text-a1 padding-b15">
                            <td>
                                <asp:Label ID="ArtNameLabel" runat="server" Text='<%# Eval("ArtName") %>' />
                            </td>
                        </tr>
                        <tr class="text-a2">
                            <td>
                                <asp:Label ID="ArtDescriptionLabel" runat="server" Text='<%# Eval("ArtDescription") %>' />
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
