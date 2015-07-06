using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;
using BusinessLayer;

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
        }



        [HttpPost]
        public ActionResult Index(String ddlEmployees)
        {

            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();

            
            //if (formCollection["ddlEmployees"].ToString()==null)
            //{
            //    List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.ALLTimeSheets.ToList();
            //    return View(timesheets);
            //}
            //else
            //{
                //Guid empID = Guid.Parse(formCollection["ddlEmployees"]);
            Guid GuidEmpID = Guid.Parse(ddlEmployees);
            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.TimeSheetsByName(GuidEmpID).ToList();
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
            //}
        }
    }
}