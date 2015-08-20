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
using System.Globalization;
using System.Web.Services;

namespace TimeSheet.Controllers
{
    public class TimeSheetController : Controller
    {
        private TimeSheetContext db = new TimeSheetContext();
        //public HttpContext context = new HttpContext();
        //
        // GET: /TimeSheet/
        [HttpGet]
        public ActionResult Index()
        {
            initEmpDDL();

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();

            //List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.ALLTimeSheets.ToList();
            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.TimeSheetsByNameorInTimeorOutTime(null, false, firstDayOfMonth, lastDayOfMonth, firstDayOfMonth, lastDayOfMonth).ToList();
            return View(timesheets);

        }

        [HttpPost]
        public ActionResult Index(String ddlAllEmployees, String ddlActiveEmployees, String ddlInactiveEmployees, 
            String rdbEmployee, String searchInTimeFrom, String searchInTimeTo, String searchOutTimeFrom, String searchOutTimeTo)
        {
            initEmpDDL();

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            
            Guid? GuidEmpID = new Guid();
            Boolean? Del = new Boolean();
            DateTime? StartInTime=new DateTime();
            DateTime? StopInTime = new DateTime();
            DateTime? StartOutTime = new DateTime();
            DateTime? StopOutTime = new DateTime();
            switch(rdbEmployee){
                case "All":
                    Del = null;
                    if (ddlAllEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlAllEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Active":
                    Del = false;
                    if (ddlActiveEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlActiveEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Inactive":
                    Del = true;
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
            timesheets = timesheetbusinesslayer.TimeSheetsByNameorInTimeorOutTime(GuidEmpID, Del, StartInTime, StopInTime, StartOutTime, StopOutTime).ToList();

            return View(timesheets);
        }


        [HttpGet]
        public ActionResult ViewChart()
        {
            initEmpDDL();
            initDateDDL();
            string[] TimeRangeArr=new string[48];
            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            List<tsChartData> cdList = new List<tsChartData>();

            for (int i = 0; i <= 23; i++) {
                TimeRangeArr[i * 2] = i.ToString() + ":00~" + i.ToString() + ":30";
                TimeRangeArr[i * 2 + 1] = i.ToString() + ":30~"+(i+1).ToString() + ":00";
            }

            foreach (string TimeRangeItem in TimeRangeArr)
            {
                char[] delimiterChars = {':', '~' };
                string[] timeElmt = TimeRangeItem.Split(delimiterChars);
                int CourtInTime = timesheetbusinesslayer.CountInTime(null, false, System.DateTime.Now.Year, System.DateTime.Now.Month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));
                //int CourtInTime = timesheetbusinesslayer.CountInTime(2015, 7, 11,0,11,30);

                int CourtOutTime = timesheetbusinesslayer.CountOutTime(null, false, System.DateTime.Now.Year, System.DateTime.Now.Month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));
                tsChartData cdItem = new tsChartData { TimeRange = TimeRangeItem, IntimeCount = CourtInTime, OuttimeCount = CourtOutTime };
                cdList.Add(cdItem);
            }
            return View(cdList);
        }

        [HttpPost]
        public ActionResult ViewChart(String ddlAllEmployees, String ddlActiveEmployees, String ddlInactiveEmployees,
            String rdbEmployee, string ddlYear, string ddlMonth)
        {
            initEmpDDL();
            initDateDDL();

            Guid? GuidEmpID = new Guid();
            Boolean? del = new Boolean();
            switch (rdbEmployee)
            {
                case "All":
                    del = null;
                    if (ddlAllEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlAllEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Active":
                    del = false;
                    if (ddlActiveEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlActiveEmployees);
                    else
                        GuidEmpID = null;
                    break;
                case "Inactive":
                    del = true;
                    if (ddlInactiveEmployees.Length > 0)
                        GuidEmpID = Guid.Parse(ddlInactiveEmployees);
                    else
                        GuidEmpID = null;
                    break;
            }

            string[] TimeRangeArr = new string[48];
            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            List<tsChartData> cdList = new List<tsChartData>();

            for (int i = 0; i <= 23; i++)
            {
                TimeRangeArr[i * 2] = i.ToString() + ":00~" + i.ToString() + ":30";
                TimeRangeArr[i * 2 + 1] = i.ToString() + ":30~" + (i + 1).ToString() + ":00";
            }

            int year = int.Parse(ddlYear);
            int? month;
            if (ddlMonth.Length > 0)
                month = int.Parse(ddlMonth);
            else
                month = null;
            
            foreach (string TimeRangeItem in TimeRangeArr)
            {
                char[] delimiterChars = { ':', '~' };
                string[] timeElmt = TimeRangeItem.Split(delimiterChars);
                int CourtInTime = timesheetbusinesslayer.CountInTime(GuidEmpID, del, year, month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));
                int CourtOutTime = timesheetbusinesslayer.CountOutTime(GuidEmpID, del, year, month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));
                tsChartData cdItem = new tsChartData { TimeRange = TimeRangeItem, IntimeCount = CourtInTime, OuttimeCount = CourtOutTime };
                cdList.Add(cdItem);
            }
            return View(cdList);
        }


        public void initDateDDL()
        {
            
            ViewBag.ddlYear = Enumerable.Range(2015, System.DateTime.Now.Year-2014)
                           .Select(x => new SelectListItem
                           {
                                Text=x.ToString(),
                                Value=x.ToString()
                           });
            ViewBag.ddlMonth = new List<SelectListItem> { 
                new SelectListItem { Text = "January", Value = "1" },
                new SelectListItem { Text = "February", Value = "2" },
                new SelectListItem { Text = "March", Value = "3" },
                new SelectListItem { Text = "April", Value = "4" },
                new SelectListItem { Text = "May", Value = "5" },
                new SelectListItem { Text = "June", Value = "6" },
                new SelectListItem { Text = "July", Value = "7" },
                new SelectListItem { Text = "August", Value = "8" },
                new SelectListItem { Text = "September", Value = "9" },
                new SelectListItem { Text = "October", Value = "10" },
                new SelectListItem { Text = "November", Value = "11" },
                new SelectListItem { Text = "December", Value = "12" }
            };
        }

        public void initEmpDDL(){
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
        }

        [HttpGet]
        public ActionResult ViewChart1()
        {
            initEmpDDL();
            initDateDDL();

            return View();
        }



        public JsonResult makechartdata(){
            string[] TimeRangeArr = new string[48];
            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            List<tsChartData> cdList = new List<tsChartData>();

            for (int i = 0; i <= 23; i++)
            {
                TimeRangeArr[i * 2] = i.ToString() + ":00~" + i.ToString() + ":30";
                TimeRangeArr[i * 2 + 1] = i.ToString() + ":30~" + (i + 1).ToString() + ":00";
            }

            foreach (string TimeRangeItem in TimeRangeArr)
            {
                char[] delimiterChars = { ':', '~' };
                string[] timeElmt = TimeRangeItem.Split(delimiterChars);
                int CourtInTime = timesheetbusinesslayer.CountInTime(null, false, System.DateTime.Now.Year, System.DateTime.Now.Month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));

                int CourtOutTime = timesheetbusinesslayer.CountOutTime(null, false, System.DateTime.Now.Year, System.DateTime.Now.Month, Int32.Parse(timeElmt[0]), Int32.Parse(timeElmt[1]), Int32.Parse(timeElmt[2]), Int32.Parse(timeElmt[3]));
                tsChartData cdItem = new tsChartData { TimeRange = TimeRangeItem, IntimeCount = CourtInTime, OuttimeCount = CourtOutTime };
                cdList.Add(cdItem);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonStr = js.Serialize(cdList);
            //HttpContext.Response.Write(jsonStr);
            return Json(cdList,JsonRequestBehavior.AllowGet);
        }

    }
}