using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Models
{
    public class HomeController : Controller
    {
        private TimeSheetContext db = new TimeSheetContext();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var tblemployees = db.tblEmployees.Where(emp => emp.del == false);
            return View(tblemployees.ToList());
        }


        public ActionResult CheckIn(Guid id)
        {
            try
            {
                tblTimeSheet timesheet = new tblTimeSheet();
                timesheet.ID = Guid.NewGuid();
                timesheet.EmpID = id;
                timesheet.InTime = System.DateTime.Now;
                db.tblTimeSheets.Add(timesheet);
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return View();
            }
        }

    }
}
