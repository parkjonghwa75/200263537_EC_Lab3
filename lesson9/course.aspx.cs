using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//model references for EF
using lesson9.Models;
using System.Web.ModelBinding;

namespace lesson9
{
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                GetCourses();
            }
            else if (!IsPostBack)
            {
                using (comp2007Entities db = new comp2007Entities())
                {
                    var departmentDDL = from d in db.Departments select new { d.DepartmentID, d.Name };
                    ddlDepartment.DataSource = departmentDDL.ToList();
                    ddlDepartment.DataValueField = "DepartmentID";
                    ddlDepartment.DataTextField = "Name";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, "Select");
                }
            }
        }

        protected void GetCourses()
        {
            //
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

            //connect to db via EF
            using (comp2007Entities db = new comp2007Entities())
            {
                //
                Course c = (from objS in db.Courses
                            where objS.CourseID == CourseID
                             select objS).FirstOrDefault();

                //map the student properties to the form controls if we found a match
                if (c != null)
                {

                    ddlDepartment.SelectedIndex = c.DepartmentID;
                    //ddlDepartment.SelectedValue=
                    txtCourseTitle.Text = c.Title;
                    txtCredits.Text = c.Credits.ToString();
                }



                //enrollments - this code goes in the same method that populates the student form but below the existing code that's already in GetStudent()               
                var objE = (from en in db.Enrollments
                            join s in db.Students on en.StudentID equals s.StudentID
                            where en.CourseID == CourseID
                            select new { en.EnrollmentID, s.LastName, s.FirstMidName, s.EnrollmentDate });

                grdStudents.DataSource = objE.ToList();
                grdStudents.DataBind();

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //use EF to connect to SQL Server
            using (comp2007Entities db = new comp2007Entities())
            {

                //use the Student model to save the new record
                Course c = new Course();
                Int32 CourseID = 0;

                //check the querystring for an id so we can determine add / update
                if (Request.QueryString["CourseID"] != null)
                {
                    //get the id from the url
                    CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    //get the current student from EF
                    c = (from objS in db.Courses
                         where objS.CourseID == CourseID
                         select objS).FirstOrDefault();
                }

                c.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedIndex);
 
                c.Title = txtCourseTitle.Text;
                c.Credits = Convert.ToInt32(txtCredits.Text);

                //call add only if we have no student ID
                if (CourseID == 0)
                {
                    db.Courses.Add(c);
                }

                //run the update or insert
                db.SaveChanges();

                //redirect to the updated students page
                Response.Redirect("courses.aspx");
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 EnrollmentID = Convert.ToInt32(grdStudents.DataKeys[e.RowIndex].Values["EnrollmentID"]);

            using (comp2007Entities db = new comp2007Entities())
            {

                Enrollment en = (from objS in db.Enrollments
                                 where objS.EnrollmentID == EnrollmentID 
                                select objS).FirstOrDefault();

                //do the delete
                db.Enrollments.Remove(en);
                db.SaveChanges();

                GetCourses();
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}