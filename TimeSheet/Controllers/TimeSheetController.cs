using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;
using BusinessLayer;
using Infragistics.Web.Mvc;

namespace TimeSheet.Controllers
{
    public class TimeSheetController : Controller
    {
        private TimeSheetContext db = new TimeSheetContext();

        //
        // GET: /TimeSheet/
        [HttpGet]
        public ActionResult Index()
        {
            //var tbltimesheets = db.tblTimeSheets.Include(t => t.tblEmployee);
            // return View(tbltimesheets.ToList());
            var ddlEmployees = db.tblEmployees
                                .ToList()
                                .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                                .Select(emp => new
                                {
                                    EmpID = emp.EmpID,
                                    FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                                });
            ViewBag.ddlEmployees = new SelectList(ddlEmployees, "EmpID", "FullName");
            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();

            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.ALLTimeSheets.ToList();
            return View(timesheets);
            //return Json(timesheets, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Index(String ddlEmployees, String searchInTimeFrom, String searchInTimeTo, String searchOutTimeFrom, String searchOutTimeTo)
        {

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();

            Guid? GuidEmpID = new Guid();
            DateTime? StartInTime=new DateTime();
            DateTime? StopInTime = new DateTime();
            DateTime? StartOutTime = new DateTime();
            DateTime? StopOutTime = new DateTime();
            if (ddlEmployees.Length > 0)
                GuidEmpID = Guid.Parse(ddlEmployees);
            else
                GuidEmpID = null;
            if (searchInTimeFrom.Length > 0)
                StartInTime = DateTime.Parse(searchInTimeFrom);
            else
                StartInTime = null;
            if (searchInTimeTo.Length > 0)
                StopInTime = DateTime.Parse(searchInTimeTo).AddDays(1).AddTicks(-1);
            else
                StopInTime = null;
            if (searchOutTimeFrom.Length > 0)
                StartOutTime = DateTime.Parse(searchOutTimeFrom);
            else
                StartOutTime = null;
            if (searchOutTimeTo.Length > 0)
                StopOutTime = DateTime.Parse(searchOutTimeTo).AddDays(1).AddTicks(-1);
            else
                StopOutTime = null;
            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.TimeSheetsByNameorInTimeofOutTime(GuidEmpID, StartInTime, StopInTime, StartOutTime, StopOutTime).ToList();
            var ddlemployees = db.tblEmployees
                    .ToList()
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            ViewBag.ddlEmployees = new SelectList(ddlemployees, "EmpID", "FullName");
            return View(timesheets);
        }
    }
}