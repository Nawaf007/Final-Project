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
    public class StudentSectionController : Controller
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
        // GET: /StudentSection/
        public ActionResult Index(int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ide != null)
                {
                    var ssexams = db.SSExams.Include(s => s.Exam).Include(s => s.SectionStudent).Where(s => s.ExamId == ide);
                    return View(ssexams.ToList());
                }
                if (ids != null)
                {
                    var ssexams = db.SSExams.Include(s => s.Exam).Include(s => s.SectionStudent).Where(s => s.SSId == idss);
                    return View(ssexams.ToList());
                }
                var ssexamss = db.SSExams.Include(s => s.Exam).Include(s => s.SectionStudent);
                return View(ssexamss.ToList());
            }
            else if (adtype() == "Teacher")
            {
                if (ide != null && db.Exams.Where(a => a.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name && a.Id == ide).Count() > 0)
                {
                    var ssexams = db.SSExams.Include(s => s.Exam).Include(s => s.SectionStudent).Where(s => s.ExamId == ide);
                    return View(ssexams.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /StudentSection/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSExam ssexam = db.SSExams.Find(id);
            if (ssexam == null)
            {
                return HttpNotFound();
            }
            return View(ssexam);
        }

        // GET: /StudentSection/Create
        public ActionResult Create(int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            ViewBag.Error = "";
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ide != null && (ids != null || idc != null))
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ide);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0 && (ss.SectionId == ids || ss.Section.SectionCourses.Where(sc => sc.CourseId == idc).Count() > 0));
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            else if (adtype() == "Teacher")
            {
                if (ide != null && db.Exams.Where(e => e.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() > 0 && (ids != null || idc != null))
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ide);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0 && (ss.SectionId == ids || ss.Section.SectionCourses.Where(sc => sc.CourseId == idc).Count() > 0));
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /StudentSection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SSId,ExamId,ObtainedMarks,Id")] SSExam ssexam, int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            Exam var = db.Exams.Find(ssexam.ExamId);
            if (ModelState.IsValid && var.TotalMarks >= ssexam.ObtainedMarks)
            {
                db.SSExams.Add(ssexam);
                db.SaveChanges();
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ide = ide, idss = idss });
            }
            ViewBag.Error = "";
            if (var.TotalMarks < ssexam.ObtainedMarks)
            {
                ViewBag.Error = "Obtained Marks cannot be greater than total marks.";
            }
            if (adtype() == "Admin")
            {
                if (ide != null && (ids != null || idc != null))
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0 && (ss.SectionId == ids || ss.Section.SectionCourses.Where(sc => sc.CourseId == idc).Count() > 0));
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        if (m.Value == ssexam.SSId.ToString())
                        {
                            m.Selected = true;
                        }
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            else if (adtype() == "Teacher")
            {
                if (ide != null && (ids != null || idc != null) && db.Exams.Where(e => e.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() > 0)
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0 && ss.SectionId == ids), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSExams.Where(sse => sse.SSId == ss.Id && sse.ExamId == ide).Count() == 0 && (ss.SectionId == ids || ss.Section.SectionCourses.Where(sc => sc.CourseId == idc).Count() > 0));
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        if (m.Value == ssexam.SSId.ToString())
                        {
                            m.Selected = true;
                        }
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /StudentSection/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            ViewBag.Error = "";
            ViewBag.UserType = adtype();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSExam ssexam = db.SSExams.Find(id);
            if (ssexam == null)
            {
                return HttpNotFound();
            }
            if(adtype() == "Admin")
            {
                ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssexam.SSId);
                return View(ssexam);
            }
            else if(adtype() == "Teacher")
            {
                if(ssexam.Exam.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name)
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                    ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssexam.SSId);
                    return View(ssexam);
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /StudentSection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SSId,ExamId,ObtainedMarks,Id")] SSExam ssexam, int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            Exam var = db.Exams.Find(ssexam.ExamId);
            if (ModelState.IsValid && var.TotalMarks >= ssexam.ObtainedMarks) 
            {
                db.Entry(ssexam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ide = ide, idss = idss });
            }
            ViewBag.Error = "";
            if (var.TotalMarks < ssexam.ObtainedMarks)
            {
                ViewBag.Error = "Obtained Marks cannot be greater than total marks.";
            }
            if (adtype() == "Admin")
            {
                ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssexam.SSId);
                return View(ssexam);
            }
            else if (adtype() == "Teacher")
            {
                if (ssexam.Exam.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name)
                {
                    ViewBag.ExamId = new SelectList(db.Exams, "Id", "ExamType", ssexam.ExamId);
                    ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssexam.SSId);
                    return View(ssexam);
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /StudentSection/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            ViewBag.UserType = adtype();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSExam ssexam = db.SSExams.Find(id);
            if (ssexam == null)
            {
                return HttpNotFound();
            }
            if (adtype() == "Admin")
            {
                return View(ssexam);
            }
            else if (adtype() == "Teacher" && ssexam.Exam.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name)
            {
                return View(ssexam);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /StudentSection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idsc, int? idc, int? ide, int? idss)
        {
            ViewBag.UserType = adtype();
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ide = ide;
            SSExam ssexam = db.SSExams.Find(id);
            if (adtype() == "Admin")
            {
                db.SSExams.Remove(ssexam);
                db.SaveChanges();
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ide = ide, idss = idss });
            }
            else if (adtype() == "Teacher" && ssexam.Exam.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name)
            {
                return View(ssexam); db.SSExams.Remove(ssexam);
                db.SaveChanges();
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ide = ide, idss = idss });
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
