<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="department.aspx.cs" Inherits="EC_Template_exercise1.department" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Department Details</h1>
    <h3>All fields are required.</h3>
    <fieldset>
        <label for="txtDeptName" class="col-sm-2">Department Name: </label>
        <asp:TextBox ID="txtDeptName" runat="server" required MaxLength="50"></asp:TextBox>
    </fieldset>

    <fieldset>
        <label for="txtBudget" class="col-sm-2">Budget: </label>
       
        
         <asp:TextBox ID="txtBudget" runat="server" required MaxLength="50" TextMode="Number"></asp:TextBox>
        <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Please positive number less than 99999999" ControlToValidate="txtBudget" MinimumValue="0" MaximumValue="99999999" Display="Dynamic"></asp:RangeValidator>
    </fieldset>

    <div class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>


    <h2>Courses</h2>
    <asp:GridView ID="grdCourses" runat="server" AutoGenerateColumns="false" 
        OnRowDeleting="grdCourses_RowDeleting" DataKeyNames="CourseID">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Course" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>
