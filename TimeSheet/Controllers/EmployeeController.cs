using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeSheet.Models;

namespace TimeSheet.Controllers
{
    public class EmployeeController : Controller
    {
        private TimeSheetContext db = new TimeSheetContext();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View(db.tblEmployees.ToList()
                .OrderBy(emp => emp.del)
                .ThenBy(emp => emp.LastName)
                .ThenBy(emp => emp.FirstName));
        }

        //
        // GET: /Employee/Details/5

        public ActionResult Details(Guid id)
        {
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            return View(tblemployee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblEmployee tblemployee)
        {
            if (ModelState.IsValid)
            {
                tblemployee.EmpID = Guid.NewGuid();
                tblemployee.CreateTime = System.DateTime.Now;
                tblemployee.LatestModifiedTime= System.DateTime.Now;
                tblemployee.InOffice = false;
                tblemployee.del = false;
                db.tblEmployees.Add(tblemployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblemployee);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(Guid id)
        {
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            return View(tblemployee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblEmployee tblemployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblemployee).State = EntityState.Modified;
                tblemployee.LatestModifiedTime = System.DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblemployee);
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(Guid id)
        {
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            if (tblemployee == null)
            {
                return HttpNotFound();
            }
            return View(tblemployee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            tblEmployee tblemployee = db.tblEmployees.Find(id);
            tblemployee.LatestModifiedTime = System.DateTime.Now;
            tblemployee.del = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}