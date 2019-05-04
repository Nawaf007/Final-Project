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
    [Authorize]
    public class TeacherAttendanceController : Controller
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
        // GET: /TeacherAttendance/
        public ActionResult Index(int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ida == null && idt == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = true;
                    var teacherattendances = db.TeacherAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Teacher);
                    return View(teacherattendances.ToList());
                }
                else if (ida == null)
                {
                    ViewBag.Showt = false;
                    ViewBag.Showa = true;
                    ViewBag.idt = idt;
                    var teacherattendances = db.TeacherAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Teacher).Where(ta => ta.TeacherId == idt);
                    return View(teacherattendances.ToList());
                }
                else
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    var teacherattendances = db.TeacherAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Teacher).Where(ta => ta.AttendanceId == ida);
                    return View(teacherattendances.ToList());
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /TeacherAttendance/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
        public ActionResult Create(int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ida == null && idt == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = true;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher"), "Id", "Date");
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                else if (ida == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && a.TeacherAttendances.Where(t => t.TeacherId == idt).Count() == 0), "Id", "Date");
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == idt).Count() > 0), "Id", "UserName", idt);
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                else
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && a.Id == ida), "Id", "Date", ida);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id && db.TeacherAttendances.Where(ta => ta.AttendanceId == ida && ta.TeacherId == t.Id).Count() == 0).Count() > 0), "Id", "UserName");
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /TeacherAttendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AttendanceId,TeacherId,Status")] TeacherAttendance teacherattendance, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.TeacherAttendances.Add(teacherattendance);
                    db.SaveChanges();
                    if (ida != null)
                    {
                        return RedirectToAction("Index", new { ida = ida });
                    }
                    if (idt != null)
                    {
                        return RedirectToAction("Index", new { idt = idt });
                    }
                    return RedirectToAction("Index");
                }
                if (teacherattendance.AttendanceId == 0)
                {
                    ViewBag.AttendanceStatus = "The Attendance Date field is required";
                }
                if (teacherattendance.TeacherId == "")
                {
                    ViewBag.UNStatus = "The User Name field is required";
                }
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && a.Id == ida), "Id", "Date", ida);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id && db.TeacherAttendances.Where(ta => ta.AttendanceId == ida && ta.TeacherId == t.Id).Count() == 0).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && db.StudentAttendances.Where(st => st.StudentId == idt && st.AttendanceId == a.Id).Count() == 0), "Id", "Date", teacherattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == idt).Count() > 0), "Id", "UserName", idt);
                }
                ViewBag.Showa = true;
                ViewBag.Showt = true;
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher"), "Id", "Date", teacherattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", idt);
                return View(teacherattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /TeacherAttendance/Edit/5
        public ActionResult Edit(int? id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && a.Id == ida), "Id", "Date", ida);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id && db.TeacherAttendances.Where(ta => ta.AttendanceId == ida && ta.TeacherId == t.Id).Count() == 0).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && db.StudentAttendances.Where(st => st.StudentId == idt && st.AttendanceId == a.Id).Count() == 0), "Id", "Date", teacherattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == idt).Count() > 0), "Id", "UserName", idt);
                }
                ViewBag.Showa = true;
                ViewBag.Showt = true;
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher"), "Id", "Date", teacherattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", idt);
                return View(teacherattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /TeacherAttendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,AttendanceId,TeacherId,Status")] TeacherAttendance teacherattendance, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(teacherattendance).State = EntityState.Modified;
                    db.SaveChanges();
                    if (ida != null)
                    {
                        return RedirectToAction("Index", new { ida = ida });
                    }
                    if (idt != null)
                    {
                        return RedirectToAction("Index", new { idt = idt });
                    }
                    return RedirectToAction("Index");
                }
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && a.Id == ida), "Id", "Date", ida);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id && db.TeacherAttendances.Where(ta => ta.AttendanceId == ida && ta.TeacherId == t.Id).Count() == 0).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher") && db.StudentAttendances.Where(st => st.StudentId == idt && st.AttendanceId == a.Id).Count() == 0), "Id", "Date", teacherattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == idt).Count() > 0), "Id", "UserName", idt);
                }
                ViewBag.Showa = true;
                ViewBag.Showt = true;
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Teacher"), "Id", "Date", teacherattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", teacherattendance.Status);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", teacherattendance.TeacherId);
                return View(teacherattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        // GET: /TeacherAttendance/Delete/5
        public ActionResult Delete(int? id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
                ViewBag.ida = ida;
                ViewBag.idt = idt;
                return View(teacherattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        // POST: /TeacherAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                TeacherAttendance teacherattendance = db.TeacherAttendances.Find(id);
                db.TeacherAttendances.Remove(teacherattendance);
                db.SaveChanges();
                if (ida != null)
                {
                    return RedirectToAction("Index", new { ida = ida });
                }
                if (idt != null)
                {
                    return RedirectToAction("Index", new { idt = idt });
                }
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
