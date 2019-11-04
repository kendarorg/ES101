<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Inventory.Projections.InventoryItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Assembly Name="netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"%>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
        <% using (Html.BeginForm())
       {%>
        <%: Html.Hidden("Id",Model.Id) %>
        <%: Html.Hidden("Version",Model.Version) %>
        Number:<%: Html.TextBox("Number") %><br />
        <button name="submit">Submit</button>

    <%
       }%>

</asp:Content>
