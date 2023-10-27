<%@ Page Title="" Language="C#" MasterPageFile="~/PartyProduct.Master" AutoEventWireup="true" CodeBehind="ProductRateAddEdit.aspx.cs" Inherits="PartyProductWebForm.ProductRate.ProductRateAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #<% =secTitle.ClientID%> {
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

        .container {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main-container">
        <asp:Label ID="secTitle" runat="server" Text="Assign Party Add"></asp:Label>
        <br />
        <div class="container">
            <asp:Label ID="lblAssignParty" runat="server" Text=""></asp:Label>
            <div class="container">
                <asp:DropDownList ID="ProductDDl" runat="server"></asp:DropDownList>
                <asp:TextBox ID="ProductRate" runat="server" TextMode="Number" placeholder="Rate" min="0"></asp:TextBox>
                <asp:TextBox ID="RateDate" runat="server" TextMode="Date"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Add" OnCommand="Button1_Command1" />
            </div>
        </div>
</asp:Content>
