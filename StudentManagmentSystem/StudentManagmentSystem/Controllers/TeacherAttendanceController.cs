using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    public class TeacherAttendanceController : Controller
    {
        private DB34Entities db = new DB34Entities();

        // GET: /TeacherAttendance/
        public ActionResult Index()
        {
            var teacherattendances = db.TeacherAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Teacher);
            return View(teacherattendances.ToList());
        }

        // GET: /TeacherAttendance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAttendance teacherattendance = db.TeacherAttendances.Find(id);
            if (teacherattendance == null)
            {
                return HttpNotFound();
            }
            return View(teacherattendance);
        }

        // GET: /TeacherAttendance/Create
        public ActionResult Create()
        {
            ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher"), "Id", "Date");
            ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
            ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
            return View();
        }

        // POST: /TeacherAttendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AttendanceId,TeacherId,Status")] TeacherAttendance teacherattendance)
        {
            if (ModelState.IsValid)
            {
                db.TeacherAttendances.Add(teacherattendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" && a.Lookup.Values == "Teacher"), "Id", "Date", teacherattendance.AttendanceId);
            ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
            ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
            return View(teacherattendance);
        }

        // GET: /TeacherAttendance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAttendance teacherattendance = db.TeacherAttendances.Find(id);
            if (teacherattendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" && a.Lookup.Values == "Teacher"), "Id", "Id", teacherattendance.AttendanceId);
            ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
            ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
            return View(teacherattendance);
        }

        // POST: /TeacherAttendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,AttendanceId,TeacherId,Status")] TeacherAttendance teacherattendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherattendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" && a.Lookup.Values == "Teacher"), "Id", "Id", teacherattendance.AttendanceId);
            ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
            ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
            return View(teacherattendance);
        }

        // GET: /TeacherAttendance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAttendance teacherattendance = db.TeacherAttendances.Find(id);
            if (teacherattendance == null)
            {
                return HttpNotFound();
            }
            return View(teacherattendance);
        }

        // POST: /TeacherAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TeacherAttendance teacherattendance = db.TeacherAttendances.Find(id);
            db.TeacherAttendances.Remove(teacherattendance);
            db.SaveChanges();
            return RedirectToAction("Index");
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
