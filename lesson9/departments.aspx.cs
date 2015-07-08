using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//ref model binding
using lesson9.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace EC_Template_exercise1
{
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate departments list
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                getDepartments();
            }
        }

        protected void getDepartments()
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                String SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                var departments = from objS in db.Departments
                                  select objS;

                //bind the result to the gridview
                grdDepartments.DataSource = departments.AsQueryable().OrderBy(SortString).ToList();
                grdDepartments.DataBind();
            }

        }

        protected void grdDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            //store which row was clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected departmentID using the grid's data key collection
            Int32 departmentID = Convert.ToInt32(grdDepartments.DataKeys[selectedRow].Values["DepartmentID"]);

            //use EF to remove the selected student from the db
            using (comp2007Entities db = new comp2007Entities())
            {
                Department d = (from objS in db.Departments
                                where objS.DepartmentID == departmentID
                                select objS).FirstOrDefault();

                // do the deletion
                db.Departments.Remove(d);
                db.SaveChanges();
            }

            //refresh the grid
            getDepartments();

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDepartments.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            getDepartments();
        }

        protected void grdDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDepartments.PageIndex = e.NewPageIndex;
            getDepartments();
        }

        protected void grdDepartments_Sorting(object sender, GridViewSortEventArgs e)
        {
            //get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            getDepartments();


            //toggle the sort direction
            if (Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdDepartments.Columns.Count - 1; i++)
                    {
                        if (grdDepartments.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "images/desc.jpg";
                                SortImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                SortImage.ImageUrl = "images/asc.jpg";
                                SortImage.AlternateText = "Sort Ascending";
                            }

                            e.Row.Cells[i].Controls.Add(SortImage);

                        }
                    } // end of for
                }

            } // end of if
        }
    }
}