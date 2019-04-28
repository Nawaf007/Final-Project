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
    public class ExamController : Controller
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
        // GET: /Exam/
        public ActionResult Index(int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.UserType = adtype();
            if(idsc != null)
            {
                 if (adtype() == "Admin")
                 {
                     var exams = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(s => s.SCId == idsc);
                     return View(exams.ToList());
                 }
                 else if (adtype() == "Teacher")
                 {
                     var exams = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(s => s.SCId == idsc && s.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name);
                     return View(exams.ToList());
                 }
                 else if (adtype() == "Student")
                 {
                     var exams = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(s => s.SCId == idsc && s.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                     return View(exams.ToList());
                 }
            }
            ViewBag.CurrentTab = "DateSheet";
            if (adtype() == "Admin")
            {
                var examss = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(e => e.ExamDate >= DateTime.Now);
                return View(examss.ToList());
            }
            else if (adtype() == "Teacher")
            {
                var examss = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(s => s.ExamDate >= DateTime.Now && s.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name);
                return View(examss.ToList());
            }
            else if (adtype() == "Student")
            {
                var examss = db.Exams.Include(e => e.Lookup).Include(e => e.SectionCourse).Where(s => s.ExamDate >= DateTime.Now && s.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                return View(examss.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Exam/Details/5
        public ActionResult Details(int? id, int? ids, int? idsc, int? idc)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // GET: /Exam/Create
        public ActionResult Create(int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.ExamType = new SelectList(db.Lookups.Where(l => l.Category == "EXAM" && db.Exams.Where(e => e.SCId == idsc && e.Lookup.Values == l.Values).Count() == 0), "Id", "Values");
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", idsc);
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Exam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SCId,ExamType,ExamDate,TotalMarks,Weightage")] Exam exam, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                int tw = db.Exams.Where(e => e.SCId == idsc).Sum(e => e.Weightage);
                if (ModelState.IsValid && (tw + exam.Weightage) <= 100)
                {
                    db.Exams.Add(exam);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                if ((tw + exam.Weightage) > 100)
                {
                    ViewBag.WError = "Weightage cannot be greater than " + (100 - tw).ToString() + ".";
                }
                ViewBag.ExamType = new SelectList(db.Lookups.Where(l => l.Category == "EXAM" && db.Exams.Where(e => e.SCId == idsc && e.Lookup.Values == l.Values).Count() == 0), "Id", "Values", exam.ExamType);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", exam.SCId);
                return View(exam);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Exam/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Exam exam = db.Exams.Find(id);
                if (exam == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ExamType = new SelectList(db.Lookups.Where(l => l.Category == "EXAM"), "Id", "Values", exam.ExamType);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", exam.SCId);
                return View(exam);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Exam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SCId,ExamType,ExamDate,TotalMarks,Weightage")] Exam exam, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                int tw = db.Exams.Where(e => e.SCId == idsc && e.Id != exam.Id).Sum(e => e.Weightage);
                if (ModelState.IsValid && (tw + exam.Weightage) <= 100)
                {
                    db.Entry(exam).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                if ((tw + exam.Weightage) > 100)
                {
                    ViewBag.WError = "Weightage cannot be greater than " + (100 - tw).ToString() + ".";
                }
                ViewBag.ExamType = new SelectList(db.Lookups.Where(l => l.Category == "EXAM"), "Id", "Values", exam.ExamType);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", exam.SCId);
                return View(exam);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Exam/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Exam exam = db.Exams.Find(id);
                if (exam == null)
                {
                    return HttpNotFound();
                }
                return View(exam);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                Exam exam = db.Exams.Find(id);
                db.Exams.Remove(exam);
                db.SaveChanges();
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
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
