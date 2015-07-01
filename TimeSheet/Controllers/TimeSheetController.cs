﻿using System;
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

        public ActionResult Index()
        {
           // var tbltimesheets = db.tblTimeSheets.Include(t => t.tblEmployee);
           // return View(tbltimesheets.ToList());
            TimeSheetBusinessLayer timesheetbusinesslayer = new TimeSheetBusinessLayer();
            List<BusinessLayer.TimeSheet> timesheets = timesheetbusinesslayer.TimeSheets.ToList();
            return View(timesheets);

        }

        //
        // GET: /TimeSheet/Details/5

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
        // GET: /TimeSheet/Create

        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.tblEmployees, "EmpID", "FirstName");
            return View();
        }

        //
        // POST: /TimeSheet/Create

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
        // GET: /TimeSheet/Edit/5

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
        // POST: /TimeSheet/Edit/5

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
        // GET: /TimeSheet/Delete/5

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
        // POST: /TimeSheet/Delete/5

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
    }
}