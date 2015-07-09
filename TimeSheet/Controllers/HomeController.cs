using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;
using BusinessLayer;

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
                
                tblEmployee tblemployee = db.tblEmployees.Find(id);
                tblemployee.InOffice = true;
                db.SaveChanges();
                
                return RedirectToAction("Index");

            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine(ex.EntityValidationErrors);
                return RedirectToAction("Index");
            }
        }



        //
        // GET: /Employee/Edit/5
        [HttpGet]
        public ActionResult CheckOut(Guid id)
        {
            tblTimeSheet timesheet = db.tblTimeSheets.Where(ts => ts.EmpID == id && ts.OutTime == null).OrderBy(ts => ts.InTime).FirstOrDefault();
            if (timesheet == null)
            {
                return HttpNotFound();
            }
            ModelState.Clear();
            return View(timesheet);
        }
        
        //
        // POST: /Home/CheckOut/

        [HttpPost]
        public ActionResult CheckOut(tblTimeSheet timesheet)
        {
            try
            {

                if (timesheet.TextBox1 == null || timesheet.TextBox1.Trim().Length == 0)
                    ModelState.AddModelError("TextBox1", "Description of Work is required.");

                if (ModelState.IsValid)
                {
                    db.Entry(timesheet).State = EntityState.Modified;
                    timesheet.OutTime = System.DateTime.Now;
                    tblEmployee tblemployee = db.tblEmployees.Find(timesheet.EmpID);
                    tblemployee.InOffice = false;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else {
                    timesheet.tblEmployee = db.tblEmployees.Find(timesheet.EmpID);
                    return View(timesheet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return RedirectToAction("Index");
            }
        }
    }
}
