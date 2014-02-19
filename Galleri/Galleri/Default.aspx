<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Galleri.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="styles/Style.css"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Box">
            <h1>Galleri</h1>
                <div id="largepicture">
                    <asp:Image ID="Image2" runat="server" />
                </div>
                <div id="Thumbs">
                    <asp:Repeater ID="ThumbRepeater" runat="server" ItemType="Galleri.Model.Images" SelectMethod="ThumbRepeater_GetData" OnItemDataBound="ThumbRepeater_ItemDataBound">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink" NavigateUrl='<%# Item.thumbPath %>' runat="server">
                            <asp:Image ID="Image1" ImageUrl='<%# Item.thumbnailUrl %>' runat="server" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            
        
            <asp:FileUpload ID="FileUpload" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Ingen fil har valts" ControlToValidate="FileUpload" Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Fileändelsen är inte godkänd, måste vara .gif .jpg eller .png" 
                ControlToValidate="FileUpload" ValidationExpression=".*.(gif|jpg|png|GIF|JPG|PNG)$" Display="None"></asp:RegularExpressionValidator>
            <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" />    

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <div>
                <asp:Label ID="SuccessLabel" runat="server" visible="false">Uppladdningen lyckades! <asp:Button ID="Button1" runat="server" Text="close" CausesValidation="false" OnClick="Button1_Click"/></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
