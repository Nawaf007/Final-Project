using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagmentSystem.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace StudentManagmentSystem.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private DB34Entities db = new DB34Entities();
        private string adtype()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First();
                int id = Convert.ToInt32(user.Admin);
                string ad = db.Lookups.Where(l => l.Id == id).First().Values.ToString();
                return ad;
            }
            return null;
        }
        // GET: /Attendance/
        public ActionResult Index()
        {
            ViewBag.CurrentTab = "Attendance";
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                var attendances = db.Attendances.Include(a => a.Lookup);
                return View(attendances.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Attendance/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: /Attendance/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.Type = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCETYPE"), "Id", "Values");
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Attendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Date,Type")] Attendance attendance)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Attendances.Add(attendance);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Type = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCETYPE"), "Id", "Values", attendance.Type);
                return View(attendance);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Attendance/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Attendance attendance = db.Attendances.Find(id);
                if (attendance == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Type = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCETYPE"), "Id", "Values", attendance.Type);
                return View(attendance);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Attendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Date,Type")] Attendance attendance)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    if (attendance.Type == db.Lookups.First(l => l.Category == "ATTENDANCETYPE" && l.Values == "Student").Id)
                    {
                        var tea = db.TeacherAttendances.Where(t => t.AttendanceId == attendance.Id);
                        foreach (TeacherAttendance item in tea)
                        {
                            db.TeacherAttendances.Remove(item);
                        }
                    }
                    else if (attendance.Type == db.Lookups.First(l => l.Category == "ATTENDANCETYPE" && l.Values == "Teacher").Id)
                    {
                        var tea = db.StudentAttendances.Where(t => t.AttendanceId == attendance.Id);
                        foreach (StudentAttendance item in tea)
                        {
                            db.StudentAttendances.Remove(item);
                        }
                    }
                    db.Entry(attendance).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Type = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCETYPE"), "Id", "Values", attendance.Type);
                return View(attendance);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Attendance/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Attendance attendance = db.Attendances.Find(id);
                if (attendance == null)
                {
                    return HttpNotFound();
                }
                return View(attendance);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                Attendance attendance = db.Attendances.Find(id);
                db.Attendances.Remove(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
