<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="lesson9.course" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>Course Detail</h1>
    <h5>All fields are required</h5>

    <fieldset>
        <label for="txtDepartment" class="col-sm-2">Department:</label>
        <asp:DropDownList ID="ddlDepartment" runat="server" >
        </asp:DropDownList>
    </fieldset>
    <fieldset>
        <label for="txtCourseTitle" class="col-sm-2">Course Title:</label>
        <asp:TextBox ID="txtCourseTitle" runat="server" required MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtCredits" class="col-sm-2">Credits:</label>
        <asp:TextBox ID="txtCredits" runat="server" required TextMode="Number" />
        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Must be a positive integer"
            ControlToValidate="txtCredits" CssClass="alert alert-danger"
            Type="Integer" MinimumValue="1" MaximumValue="9"></asp:RangeValidator>
    </fieldset>
    

    <div class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
             OnClick="btnSave_Click" />
    </div>
    
    <h2>Students</h2>
    <asp:GridView ID="grdStudents" runat="server" AutoGenerateColumns="false" 
        OnRowDeleting="grdStudents_RowDeleting" DataKeyNames="EnrollmentID">
        <Columns>
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="FirstMidName" HeaderText="First Name" />
            <asp:BoundField DataField="EnrollmentDate" HeaderText="Enrollment Date" DataFormatString ="{0:d}" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>
