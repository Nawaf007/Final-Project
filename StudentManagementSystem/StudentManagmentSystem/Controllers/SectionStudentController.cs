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
    public class SectionStudentController : Controller
    {
        private DB34Entities db = new DB34Entities();
        public ActionResult AttendanceReport(int? ids)
        {
            if (ids == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "AtteReport.rpt"));

            rd.Database.Tables[2].SetDataSource(db.Students.Where(s => s.SectionStudents.Where(ss => ss.Section.Id == ids).Count() > 0).Select(a => new { Id = a.Id, RegistrationNo = a.RegistrationNo }).ToList());
            rd.Database.Tables[0].SetDataSource(db.Attendances.Where(a => a.Lookup.Values != "Teacher").ToList());
            rd.Database.Tables[1].SetDataSource(db.StudentAttendances.Select(sa => new { Id = sa.Id, Status = sa.Lookup.Values, StudentId = sa.StudentId, AttendanceId = sa.AttendanceId}).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "AttendanceReport.pdf");
        }

        public ActionResult Datesheet(int? ids)
        {
            if (ids == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Datesheet.rpt"));

            rd.Database.Tables[0].SetDataSource(db.Exams.Where(e => e.SectionCourse.Section.Id == ids).Select(a => new { SCId = a.SectionCourse.Course.Title, ExamDate = a.ExamDate, ExamType = a.Lookup.Values }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Datesheet.pdf");
        }
        public ActionResult Timetable(int? ids)
        {
            if (ids == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Timetable.rpt"));

            rd.Database.Tables[0].SetDataSource(db.SCTimes.Where(e => e.SectionCourse.Section.Id == ids).Select(a => new { SectionId = a.SectionCourse.Course.Title, DayOfWeek = a.Lookup.Values, TimeOfDay = a.TimeOfDay, Id = a.SectionCourse.Teacher.AspNetUser.FirstName + " " + a.SectionCourse.Teacher.AspNetUser.LastName}).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Timetable.pdf");
        }
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
        // GET: /SectionStudent/
        public ActionResult Index(int? ids)
        {
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ids != null)
                {
                    var sectionstudents = db.SectionStudents.Include(s => s.Section).Include(s => s.Student).Where(s => s.SectionId == ids);
                    return View(sectionstudents.ToList());
                }
                var sectionstudentss = db.SectionStudents.Include(s => s.Section).Include(s => s.Student).Where(s => s.SectionId == ids);
                return View(sectionstudentss.ToList());
            }
            else if(adtype() == "Teacher")
            {
                if (ids != null)
                {
                    var sectionstudents = db.SectionStudents.Include(s => s.Section).Include(s => s.Student).Where(s => s.SectionId == ids && s.Section.SectionCourses.Where(sc => sc.Teacher.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                    return View(sectionstudents.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SectionStudent/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectionStudent sectionstudent = db.SectionStudents.Find(id);
            if (sectionstudent == null)
            {
                return HttpNotFound();
            }
            return View(sectionstudent);
        }

        // GET: /SectionStudent/Create
        public ActionResult Create(int? ids)
        {
            ViewBag.error = "";
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ids != null)
                {
                    ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Id == ids), "Id", "Name", ids);
                    ViewBag.StudentId = new SelectList(db.Students.Where(s => db.SectionStudents.Where(ss => ss.StudentId == s.Id && ss.SectionId == ids).Count() == 0), "Id", "RegistrationNo");
                    return View();
                }
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name");
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SectionStudent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,SectionId,StudentId")] SectionStudent sectionstudent, int? ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                StudentManagmentSystem.Models.Section var = db.Sections.Find(ids);
                if (db.SectionStudents.Where(s => s.SectionId == ids).Count() == var.MaxCount)
                {
                    ViewBag.error = "Cannot add student. Maxcount ecxeeded.";
                    ViewBag.ids = ids;
                    if (ids != null)
                    {
                        ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Id == ids), "Id", "Name", sectionstudent.SectionId);
                        ViewBag.StudentId = new SelectList(db.Students.Where(s => db.SectionStudents.Where(ss => ss.StudentId == s.Id && ss.SectionId == ids).Count() == 0), "Id", "RegistrationNo", sectionstudent.StudentId);
                        return View(sectionstudent);
                    }
                    ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectionstudent.SectionId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", sectionstudent.StudentId);
                    return View(sectionstudent);
                }
                if (ModelState.IsValid)
                {
                    db.SectionStudents.Add(sectionstudent);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ids = ids });
                }
                ViewBag.ids = ids;
                if (ids != null)
                {
                    ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Id == ids), "Id", "Name", sectionstudent.SectionId);
                    ViewBag.StudentId = new SelectList(db.Students.Where(s => db.SectionStudents.Where(ss => ss.StudentId == s.Id && ss.SectionId == ids).Count() == 0), "Id", "RegistrationNo", sectionstudent.StudentId);
                    return View(sectionstudent);
                }
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectionstudent.SectionId);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", sectionstudent.StudentId);
                ViewBag.error = "";
                return View(sectionstudent);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SectionStudent/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SectionStudent sectionstudent = db.SectionStudents.Find(id);
                if (sectionstudent == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectionstudent.SectionId);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", sectionstudent.StudentId);
                return View(sectionstudent);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SectionStudent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,SectionId,StudentId")] SectionStudent sectionstudent)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sectionstudent).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectionstudent.SectionId);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", sectionstudent.StudentId);
                return View(sectionstudent);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SectionStudent/Delete/5
        public ActionResult Delete(int? id, int? ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SectionStudent sectionstudent = db.SectionStudents.Find(id);
                if (sectionstudent == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ids = ids;
                return View(sectionstudent);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SectionStudent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                SectionStudent sectionstudent = db.SectionStudents.Find(id);
                var sses = db.SSExams.Where(sse => sse.SectionStudent.Id == id);
                var ssas = db.SSAssignments.Where(ssa => ssa.SectionStudent.Id == id);
                foreach (SSExam sse in sses)
                {
                    if (db.SSExams.Find(sse) != null)
                    {
                        db.SSExams.Remove(sse);
                    }
                }
                foreach (SSAssignment ssa in ssas)
                {
                    if (db.SSAssignments.Find(ssa) != null)
                    {
                        db.SSAssignments.Remove(ssa);
                    }
                }
                db.SectionStudents.Remove(sectionstudent);
                db.SaveChanges();
                return RedirectToAction("Index", new { ids = ids });
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
