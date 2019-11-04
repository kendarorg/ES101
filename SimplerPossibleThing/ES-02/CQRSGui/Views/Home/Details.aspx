<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Inventory.Projections.InventoryItemDetailsDto>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Assembly Name="netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"%>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<h2>Details:</h2>
Id: <%:Model.Id%><br />
Name: <%:Model.Name%><br />
Count: <%: Model.CurrentCount %><br /><br />

<%: Html.ActionLink("Rename","ChangeName", new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Deactivate","Deactivate",new{Id=Model.Id, Version=Model.Version}) %><br />
<%: Html.ActionLink("Check in","CheckIn", new{Id=Model.Id}) %><br />
<%: Html.ActionLink("Remove","Remove", new{Id=Model.Id,Version=Model.Version}) %>
</asp:Content>
