using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using lesson9.Models;
using System.Web.ModelBinding;

namespace EC_Template_exercise1
{
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't click AND we have a studentID in the url
            if (!IsPostBack && (Request.QueryString.Count > 0))
            {
                getDepartment();
            }
        }

        protected void getDepartment()
        {
            //populate form with existing student record
            Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            //connect to db via EF
            using (comp2007Entities db = new comp2007Entities())
            {
                //populate a student instance with the student ID from the url parameter
                Department d = (from objS in db.Departments
                                where objS.DepartmentID == DepartmentID
                                select objS).FirstOrDefault();

                //map the student to the form controls when s found
                if (d != null)
                {
                    txtDeptName.Text = d.Name;
                    txtBudget.Text = Convert.ToString(d.Budget);

                }


                //enrollments - this code goes in the same method that populates the student form but below the existing code that's already in GetStudent()               
                var objE = (from c in db.Courses
                            where c.DepartmentID == DepartmentID
                            select new { c.CourseID, c.Title, c.Credits});

                grdCourses.DataSource = objE.ToList();
                grdCourses.DataBind();



            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // use EF to connect to SQL
            using (comp2007Entities db = new comp2007Entities())
            {
                //use the student model to save the new model
                Department d = new Department();
                Int32 DepartmentID = 0;

                //check save? or update??
                if (Request.QueryString["DepartmentID"] != null)
                {
                    //get the id from the url
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);
                    //get the current student from EF   
                    d = (from objS in db.Departments
                         where objS.DepartmentID == DepartmentID
                         select objS).FirstOrDefault();
                }

                d.Name = txtDeptName.Text;
                d.Budget = Convert.ToDecimal(txtBudget.Text);

                if (DepartmentID == 0)
                {
                    db.Departments.Add(d);
                }

                db.SaveChanges();

                //redirect to the updated list page
                Response.Redirect("departments.aspx");

            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["CourseID"]);

            using (comp2007Entities db = new comp2007Entities())
            {

                Course c = (from objS in db.Courses
                            where objS.CourseID == CourseID
                             select objS).FirstOrDefault();

                //do the delete
                db.Courses.Remove(c);
                db.SaveChanges();

                getDepartment();
            }
        }
    }
}