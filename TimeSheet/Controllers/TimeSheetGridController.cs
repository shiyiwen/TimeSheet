using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class TimeSheetGridController : Controller
    {

        private DataTable GetTimeSheets {
            get {
                if (Session["TimeSheets"] == null) {
                    Session["TimeSheets"] = GetTimesheetDataTable();
                }
                return (DataTable)Session["TimeSheets"];
            }
        }

        [GridDataSourceAction]
        [ActionName("datatable-binding")]
        public ActionResult BasicMvcHelper()
        {
            DataTable customers = this.GetTimeSheets;
            NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            // check the query string for sorting expressions
            List<SortExpression> sortExpressions = BuildSortExpressions(queryString, "sort");
            DataView dv = customers.DefaultView;
            if (sortExpressions.Count > 0)
            {
                String sortExpression = "";
                foreach (SortExpression expr in sortExpressions)
                {
                    sortExpression += expr.Key + " " + (expr.Mode == SortMode.Ascending ? "asc" : "desc") + ",";
                }
                dv.Sort = sortExpression.Substring(0, sortExpression.Length - 1);
            }
            return View("datatable-binding", dv.ToTable());
        }

        public List<SortExpression> BuildSortExpressions( NameValueCollection queryString, string sortKey)
        {
            List<SortExpression> expressions = new List<SortExpression>();
            List<string> sortKeys = new List<string>();
            foreach (string key in queryString.Keys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith(sortKey))
                {
                    SortExpression e = new SortExpression();
                    e.Key = key.Substring(key.IndexOf("(")).Replace("(", "").Replace(")", "");
                    e.Logic = "AND";
                    e.Mode = queryString[key].ToLower().StartsWith("asc") ? SortMode.Ascending : SortMode.Descending;
                    expressions.Add(e);
                    sortKeys.Add(key);
                }
            }
            if (sortKeys.Count > 0)
            {
                foreach (string sortedKey in sortKeys)
                {
                    queryString.Remove(sortedKey);
                }
                string url = Request.Url.AbsolutePath;
                string updatedQueryString = "?" + queryString.ToString();
                Response.Redirect(url + updatedQueryString);
            }
            return expressions;
        }

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspnetMvcHelper()
        {
            return View("aspnet-mvc-helper");
        }


        [ActionName("append-rows-on-demand")]
        public ActionResult AppendRowsOnDemand()
        {
            return View("append-rows-on-demand");
        }

        [ActionName("sorting-remote")]
        public ActionResult SortingRemote()
        {
            return View("sorting-remote");
        }

        [ActionName("summaries-remote")]
        public ActionResult SummariesRemote()
        {
            return View("summaries-remote");
        }

        [ActionName("bind-web-api")]
        public ActionResult BindToWebAPI()
        {
            return View("bind-web-api");
        }







        private DataTable GetTimesheetDataTable()
        {
            TimeSheetContext ctx = new TimeSheetContext();
            SqlConnection conn = (SqlConnection)ctx.Database.Connection;
            DataTable dt = new DataTable();
            using (SqlConnection con = conn)
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tblTimeSheet", con))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }


/*

        private TimeSheetContext db = new TimeSheetContext();
        //
        // GET: /TimeSheetGrid/

        public ActionResult Index()
        {
            var tbltimesheets = db.tblTimeSheets.Include(t => t.tblEmployee);
            return View(tbltimesheets.ToList());
        }

        //
        // GET: /TimeSheetGrid/Details/5

        public ActionResult Details(Guid id)
        {
            tblTimeSheet tbltimesheet = db.tblTimeSheets.Find(id);
            if (tbltimesheet == null)
            {
                return HttpNotFound();
            }
            return View(tbltimesheet);
        }

        //
        // GET: /TimeSheetGrid/Create

        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.tblEmployees, "EmpID", "FirstName");
            return View();
        }

        //
        // POST: /TimeSheetGrid/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblTimeSheet tbltimesheet)
        {
            if (ModelState.IsValid)
            {
                tbltimesheet.ID = Guid.NewGuid();
                db.tblTimeSheets.Add(tbltimesheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.tblEmployees, "EmpID", "FirstName", tbltimesheet.EmpID);
            return View(tbltimesheet);
        }

        //
        // GET: /TimeSheetGrid/Edit/5

        public ActionResult Edit(Guid id)
        {
            tblTimeSheet tbltimesheet = db.tblTimeSheets.Find(id);
            if (tbltimesheet == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.tblEmployees, "EmpID", "FirstName", tbltimesheet.EmpID);
            return View(tbltimesheet);
        }

        //
        // POST: /TimeSheetGrid/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblTimeSheet tbltimesheet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbltimesheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.tblEmployees, "EmpID", "FirstName", tbltimesheet.EmpID);
            return View(tbltimesheet);
        }

        //
        // GET: /TimeSheetGrid/Delete/5

        public ActionResult Delete(Guid id)
        {
            tblTimeSheet tbltimesheet = db.tblTimeSheets.Find(id);
            if (tbltimesheet == null)
            {
                return HttpNotFound();
            }
            return View(tbltimesheet);
        }

        //
        // POST: /TimeSheetGrid/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            tblTimeSheet tbltimesheet = db.tblTimeSheets.Find(id);
            db.tblTimeSheets.Remove(tbltimesheet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
 */
    }

}