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
using System.Collections;
using System.Web.Script.Serialization;

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
            var ddlAllEmployees = db.tblEmployees
                                .ToList()
                                .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                                .Select(emp => new
                                {
                                    EmpID = emp.EmpID,
                                    FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                                });
            var ddlActiveEmployees = db.tblEmployees
                    .ToList()
                    .Where(emp => emp.del == false)
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            var ddlInactiveEmployees = db.tblEmployees
                    .ToList()
                    .Where(emp => emp.del == true)
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            ViewBag.ddlAllEmployees = new SelectList(ddlAllEmployees, "EmpID", "FullName");
            ViewBag.ddlActiveEmployees = new SelectList(ddlActiveEmployees, "EmpID", "FullName");
            ViewBag.ddlInactiveEmployees = new SelectList(ddlInactiveEmployees, "EmpID", "FullName");

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();

            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.ALLTimeSheets.ToList();
            return View(timesheets);

        }

        [HttpPost]
        public ActionResult Index(String ddlAllEmployees, String ddlActiveEmployees, String ddlInactiveEmployees, 
            String rdbEmployee, String searchInTimeFrom, String searchInTimeTo, String searchOutTimeFrom, String searchOutTimeTo)
        {

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            
            Guid? GuidEmpID = new Guid();
            DateTime? StartInTime=new DateTime();
            DateTime? StopInTime = new DateTime();
            DateTime? StartOutTime = new DateTime();
            DateTime? StopOutTime = new DateTime();
            switch(rdbEmployee){
                case "All":
                    if (ddlAllEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlAllEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Active":
                    if (ddlActiveEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlActiveEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Inactive":
                    if (ddlInactiveEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlInactiveEmployees);
                    else
                        GuidEmpID = null;
                    break;
            }

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
            List<BusinessLayer.TimeSheet> timesheets=new List<BusinessLayer.TimeSheet>();
            if (rdbEmployee == "Active" && GuidEmpID == null)
                timesheets = timesheetbusinesslayer.ActiveEmpTimeSheetsByInTimeofOutTime(StartInTime, StopInTime, StartOutTime, StopOutTime).ToList();
            else if(rdbEmployee == "Inactive" && GuidEmpID == null)
                timesheets = timesheetbusinesslayer.InactiveEmpTimeSheetsByInTimeofOutTime(StartInTime, StopInTime, StartOutTime, StopOutTime).ToList();
            else
                timesheets = timesheetbusinesslayer.TimeSheetsByNameorInTimeofOutTime(GuidEmpID, StartInTime, StopInTime, StartOutTime, StopOutTime).ToList();
            var ddlemployees = db.tblEmployees
                    .ToList()
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            var ddlactiveEmployees = db.tblEmployees
                    .ToList()
                    .Where(emp => emp.del == false)
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            var ddlinactiveEmployees = db.tblEmployees
                    .ToList()
                    .Where(emp => emp.del == true)
                    .OrderBy(emp => emp.LastName).ThenBy(emp => emp.FirstName)
                    .Select(emp => new
                    {
                        EmpID = emp.EmpID,
                        FullName = string.Format("{0} {1}", emp.FirstName, emp.LastName)
                    });
            ViewBag.ddlAllEmployees = new SelectList(ddlemployees, "EmpID", "FullName");
            ViewBag.ddlActiveEmployees = new SelectList(ddlactiveEmployees, "EmpID", "FullName");
            ViewBag.ddlInactiveEmployees = new SelectList(ddlinactiveEmployees, "EmpID", "FullName");
            return View(timesheets);
        }


        [HttpGet]
        public ActionResult ViewChart()
        {
            tsChartData cd1 = new tsChartData {TimeRange="Pini",IntimeCount=111,OuttimeCount=30};
            tsChartData cd2 = new tsChartData {TimeRange="Yaniv",IntimeCount=15,OuttimeCount=222};
            tsChartData cd3 = new tsChartData {TimeRange="Yoni",IntimeCount=11,OuttimeCount=111};

            List<tsChartData> cdList = new List<tsChartData>() { cd1, cd2, cd3 };

            string jsonstr = new JavaScriptSerializer().Serialize(cd1);
            Response.Write(jsonstr);


            ViewBag.dataJSON = jsonstr;

            return View();
        }

        [HttpGet]
        public ActionResult MakeChart()
        {
            ViewBag.xline = new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" };
            ViewBag.yline = new[] { "2", "6", "10", "5", "3" };
            return View();
        }
    }
}