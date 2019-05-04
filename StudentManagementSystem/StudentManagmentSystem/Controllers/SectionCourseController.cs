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
    public class SectionCourseController : Controller
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
        // GET: /SectionCourse/
        public ActionResult Index(int? ids, int? idc, string idt)
        {
            ViewBag.idt = idt;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idt != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Teacher.Id == idt);
                    return View(sectioncourses.ToList());
                }
                if (idc != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Course.Id == idc);
                    return View(sectioncourses.ToList());
                }
                if (ids != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Section.Id == ids);
                    return View(sectioncourses.ToList());
                }
                var sectioncoursess = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher);
                return View(sectioncoursess.ToList());
            }
            else if(adtype() == "Teacher")
            {
                if (idt != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Teacher.Id == idt);
                    return View(sectioncourses.ToList());
                }
                if (idc != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Course.Id == idc && s.Teacher.AspNetUser.UserName == User.Identity.Name);
                    return View(sectioncourses.ToList());
                }
                if (ids != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Section.Id == ids && s.Teacher.AspNetUser.UserName == User.Identity.Name);
                    return View(sectioncourses.ToList());
                }
            }
            else if(adtype() == "Student")
            {
                if (idt != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Teacher.Id == idt);
                    return View(sectioncourses.ToList());
                }
                if (idc != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Course.Id == idc && s.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                    return View(sectioncourses.ToList());
                }
                if (ids != null)
                {
                    var sectioncourses = db.SectionCourses.Include(s => s.Course).Include(s => s.Section).Include(s => s.Teacher).Where(s => s.Section.Id == ids && s.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                    return View(sectioncourses.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SectionCourse/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SectionCourse sectioncourse = db.SectionCourses.Find(id);
            if (sectioncourse == null)
            {
                return HttpNotFound();
            }
            return View(sectioncourse);
        }

        // GET: /SectionCourse/Create
        public ActionResult Create(int? ids, int? idc, string idt)
        {
            ViewBag.idt = idt;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ids != null)
                {
                    Section var = db.Sections.Find(ids);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.Class == var.Class && (c.SectionCourses.Where(sc => sc.CourseId == c.Id).Count()) == 0), "Id", "Title");
                    ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", ids);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
                    return View();
                }
                if (idc != null)
                {
                    Course var = db.Courses.Find(idc);
                    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", idc);
                    ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Class == var.Class && s.SectionCourses.Where(sc => sc.SectionId == s.Id).Count() == 0), "Id", "Name");
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
                    return View();
                }
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", idc);
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", ids);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", idt);
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /SectionCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,SectionId,CourseId,TeacherId")] SectionCourse sectioncourse, int? ids, int? idc, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.SectionCourses.Add(sectioncourse);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ids = ids, idc = idc, idt = idt });
                }
                ViewBag.idt = idt;
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (ids != null)
                {
                    Section var = db.Sections.Find(ids);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.Class == var.Class && (c.SectionCourses.Where(sc => sc.CourseId == c.Id).Count()) == 0), "Id", "Title");
                    ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", ids);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
                    return View();
                }
                if (idc != null)
                {
                    Course var = db.Courses.Find(idc);
                    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", idc);
                    ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Class == var.Class && s.SectionCourses.Where(sc => sc.SectionId == s.Id).Count() == 0), "Id", "Name");
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName");
                    return View();
                }
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", sectioncourse.CourseId);
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectioncourse.SectionId);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                return View(sectioncourse);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SectionCourse/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idc, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SectionCourse sectioncourse = db.SectionCourses.Find(id);
                if (sectioncourse == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idt = idt;
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (ids != null)
                {
                    Section var = db.Sections.Find(ids);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.Class == var.Class), "Id", "Title", sectioncourse.CourseId);
                    ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectioncourse.SectionId);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                    return View();
                }
                if (idc != null)
                {
                    Course var = db.Courses.Find(idc);
                    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", sectioncourse.CourseId);
                    ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Class == var.Class), "Id", "Name", sectioncourse.SectionId);
                    ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                    return View();
                }
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", sectioncourse.CourseId);
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectioncourse.SectionId);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                return View(sectioncourse);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /SectionCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SectionId,CourseId,TeacherId")] SectionCourse sectioncourse, int? ids, int? idc, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
            {
                db.Entry(sectioncourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ids = ids, idc = idc, idt = idt });
            }
            ViewBag.idt = idt;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            if (ids != null)
            {
                Section var = db.Sections.Find(ids);
                ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.Class == var.Class), "Id", "Title", sectioncourse.CourseId);
                ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectioncourse.SectionId);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                return View();
            }
            if (idc != null)
            {
                Course var = db.Courses.Find(idc);
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", sectioncourse.CourseId);
                ViewBag.SectionId = new SelectList(db.Sections.Where(s => s.Class == var.Class), "Id", "Name", sectioncourse.SectionId);
                ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
                return View();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", sectioncourse.CourseId);
            ViewBag.SectionId = new SelectList(db.Sections, "Id", "Name", sectioncourse.SectionId);
            ViewBag.TeacherId = new SelectList(db.AspNetUsers.Where(a => db.Teachers.Where(t => t.Id == a.Id).Count() > 0), "Id", "UserName", sectioncourse.TeacherId);
            return View(sectioncourse);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SectionCourse/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idc, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SectionCourse sectioncourse = db.SectionCourses.Find(id);
                if (sectioncourse == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idt = idt;
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                return View(sectioncourse);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /SectionCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idc, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                SectionCourse sectioncourse = db.SectionCourses.Find(id);
                db.SectionCourses.Remove(sectioncourse);
                db.SaveChanges();
                return RedirectToAction("Index", new { ids = ids, idc = idc, idt = idt });
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
