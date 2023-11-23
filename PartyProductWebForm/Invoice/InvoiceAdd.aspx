<%@ Page Title="" Language="C#" MasterPageFile="~/PartyProduct.Master" AutoEventWireup="true" CodeBehind="InvoiceAdd.aspx.cs" Inherits="PartyProductWebForm.Invoice.InvoiceAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #<% =secTitle.ClientID %> {
            font-size: 40px;
            display: flex;
            justify-content: center;
            margin: 20px 0;
        }

        .main-container {
            width: 30%;
            display: flex;
            justify-content: center;
            flex-direction: column;
            align-items: center;
        }

        .top-container {
            display: flex;
            flex-direction: row;
            gap: 10px;
        }
        .main-container{
            display:flex;
            flex-direction:column;
        }
        .input-container #<% =Panel1.ClientID %>{
            display:flex;
            flex-direction:column;
            gap:10px;
        }
        .output-container{
            display:flex;
            flex-direction:column;
            gap:10px
        }
        .lbl-container{
            display:flex;
            justify-content:space-between;
        }
        .table-head {
            text-align: center;
            margin:10px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-container">
        <asp:Label ID="secTitle" runat="server" Text="Add Invoice"></asp:Label>
        <br />
        <asp:Label ID="lblInvoice" runat="server" Text=""></asp:Label>
        <div class="input-container">
            <asp:Panel ID="Panel1" runat="server">
                Party :
                <asp:DropDownList ID="PartyDDl" runat="server" OnSelectedIndexChanged="PartyDDl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                Product :
                <asp:DropDownList ID="ProductDDl" runat="server" OnSelectedIndexChanged="ProductDDl_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                Rate :
                <asp:TextBox ID="txtRate" runat="server" TextMode="Number" min="0" ReadOnly="True" required></asp:TextBox>
                Quantity :
                <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" min="0" required ></asp:TextBox>
                <asp:Button ID="addBtn" runat="server" Text="Add To Invoice" OnCommand="addBtn_Command" />
            </asp:Panel>
        </div>
        <div class="output-container">
            <div class="tbl-container">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="ID" ReadOnly="true" HeaderText="ID" ControlStyle-CssClass="table-head">
                            <ControlStyle CssClass="table-head"></ControlStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Party_Name" ReadOnly="true" HeaderText="Party Name"  />
                        <asp:BoundField DataField="Product_Name" ReadOnly="true" HeaderText="Product Name" />
                        <asp:BoundField DataField="Rate" ReadOnly="true" HeaderText="Rate Of Product" />
                        <asp:BoundField DataField="Quantity" ReadOnly="true" HeaderText="Quantity" />
                        <asp:BoundField DataField="Total" ReadOnly="true" HeaderText="Total" />
                        <asp:TemplateField  HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="editBtn" runat="server" Text="Edit" OnCommand="editBtn_Command" CommandName="EditInvoice"
                                    CommandArgument='<%#DataBinder.Eval( Container,"DataItemIndex") %>' />
                                <asp:Button ID="deleteBtn" runat="server" Text="Delete" OnCommand="deleteBtn_Command" CommandName="DeleteInvoice"
                                    CommandArgument='<%#DataBinder.Eval( Container,"DataItemIndex") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="lbl-container">
                <span>
                    <b>Grand Total: </b>
                    <asp:Label ID="gtotalLbl" runat="server" Text=""></asp:Label></span>
                <asp:Button ID="closeBtn" runat="server" Text="Close Invoice" OnCommand="closeBtn_Command" CommandName="CloseInvoice" />
            </div>
    </div>
    </div>
</asp:Content>
