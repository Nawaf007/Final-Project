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
    public class StudentAttendanceController : Controller
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
        // GET: /StudentAttendance/
        public ActionResult Index(int? ida, string idt)
        {
            //var studentattendances = db.StudentAttendances.Include(s => s.Attendance).Include(s => s.Lookup).Include(s => s.Student);
            //return View(studentattendances.ToList());
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (ida == null && idt == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = true;
                    var studentattendances = db.StudentAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Student);
                    return View(studentattendances.ToList());
                }
                else if (ida == null)
                {
                    ViewBag.Showt = false;
                    ViewBag.Showa = true;
                    ViewBag.idt = idt;
                    var studentattendances = db.StudentAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Student).Where(ta => ta.StudentId == idt);
                    return View(studentattendances.ToList());
                }
                else
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    var studentattendances = db.StudentAttendances.Include(t => t.Attendance).Include(t => t.Lookup).Include(t => t.Student).Where(ta => ta.AttendanceId == ida);
                    return View(studentattendances.ToList());
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAttendance/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentattendance = db.StudentAttendances.Find(id);
            if (studentattendance == null)
            {
                return HttpNotFound();
            }
            return View(studentattendance);
        }

        // GET: /StudentAttendance/Create
        public ActionResult Create(int? ida, string idt)
        {
            /*ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Student"), "Id", "Date");
            ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo");
            ViewBag.AttendanceStatus = "";
            ViewBag.RegNoStatus = "";
            return View();*/
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (ida == null && idt == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = true;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Student"), "Id", "Date");
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo");
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                else if (ida == null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.StudentAttendances.Where(t => t.StudentId == idt).Count() == 0), "Id", "Date");
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => t.Id == idt), "Id", "RegistrationNo", idt);
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                else
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.Id == ida), "Id", "Date", ida);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values");
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => db.StudentAttendances.Where(ta => ta.AttendanceId == ida && ta.StudentId == t.Id).Count() == 0), "Id", "RegistrationNo");
                    ViewBag.AttendanceStatus = "";
                    ViewBag.UNStatus = "";
                }
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /StudentAttendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttendanceId,StudentId,Status,Id")] StudentAttendance studentattendance, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (ModelState.IsValid)
                {
                    db.StudentAttendances.Add(studentattendance);
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
                if (studentattendance.AttendanceId == 0)
                {
                    ViewBag.AttendanceStatus = "The Attendance Date field is required";
                }
                if (studentattendance.StudentId == "")
                {
                    ViewBag.RegNoStatus = "The Registration No. field is required";
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.StudentAttendances.Where(t => t.StudentId == idt).Count() == 0), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => t.Id == idt), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.Id == ida), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => db.StudentAttendances.Where(ta => ta.AttendanceId == ida && ta.StudentId == t.Id).Count() == 0), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Student"), "Id", "Date", studentattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", studentattendance.StudentId);
                return View(studentattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAttendance/Edit/5
        public ActionResult Edit(int? id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentAttendance studentattendance = db.StudentAttendances.Find(id);
                if (studentattendance == null)
                {
                    return HttpNotFound();
                }
                if (studentattendance.AttendanceId == 0)
                {
                    ViewBag.AttendanceStatus = "The Attendance Date field is required";
                }
                if (studentattendance.StudentId == "")
                {
                    ViewBag.RegNoStatus = "The Registration No. field is required";
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.StudentAttendances.Where(t => t.StudentId == idt).Count() == 0), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => t.Id == idt), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.Id == ida), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => db.StudentAttendances.Where(ta => ta.AttendanceId == ida && ta.StudentId == t.Id).Count() == 0), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Student"), "Id", "Date", studentattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", studentattendance.StudentId);
                return View(studentattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /StudentAttendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AttendanceId,StudentId,Status,Id")] StudentAttendance studentattendance, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(studentattendance).State = EntityState.Modified;
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
                if (studentattendance.AttendanceId == 0)
                {
                    ViewBag.AttendanceStatus = "The Attendance Date field is required";
                }
                if (studentattendance.StudentId == "")
                {
                    ViewBag.RegNoStatus = "The Registration No. field is required";
                }
                if (idt != null)
                {
                    ViewBag.Showa = true;
                    ViewBag.Showt = false;
                    ViewBag.idt = idt;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.StudentAttendances.Where(t => t.StudentId == idt).Count() == 0), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => t.Id == idt), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                if (ida != null)
                {
                    ViewBag.Showa = false;
                    ViewBag.Showt = true;
                    ViewBag.ida = ida;
                    ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => (a.Lookup.Values == "Both" || a.Lookup.Values == "Student") && a.Id == ida), "Id", "Date", studentattendance.AttendanceId);
                    ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                    ViewBag.StudentId = new SelectList(db.Students.Where(t => db.StudentAttendances.Where(ta => ta.AttendanceId == ida && ta.StudentId == t.Id).Count() == 0), "Id", "RegistrationNo", studentattendance.StudentId);
                }
                ViewBag.AttendanceId = new SelectList(db.Attendances.Where(a => a.Lookup.Values == "Both" || a.Lookup.Values == "Student"), "Id", "Date", studentattendance.AttendanceId);
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ATTENDANCESTATUS"), "Id", "Values", studentattendance.Status);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", studentattendance.StudentId);
                return View(studentattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAttendance/Delete/5
        public ActionResult Delete(int? id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StudentAttendance studentattendance = db.StudentAttendances.Find(id);
                if (studentattendance == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ida = ida;
                ViewBag.idt = idt;
                return View(studentattendance);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /StudentAttendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ida, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                StudentAttendance studentattendance = db.StudentAttendances.Find(id);
                db.StudentAttendances.Remove(studentattendance);
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
