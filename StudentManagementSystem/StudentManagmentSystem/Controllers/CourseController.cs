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
    public class CourseController : Controller
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
        // GET: /Course/
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                ViewBag.CurrentTab = "Course";
                var courses = db.Courses.Include(c => c.Lookup).Include(c => c.Lookup1);
                return View(courses.ToList());
            }
            else if(adtype() == "Teacher")
            {
                ViewBag.CurrentTab = "Course";
                var courses = db.Courses.Include(c => c.Lookup).Include(c => c.Lookup1).Where(c => c.SectionCourses.Where(sc => sc.Teacher.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                return View(courses.ToList());
            }
            else if(adtype() == "Student")
            {
                ViewBag.CurrentTab = "Course";
                var courses = db.Courses.Include(c => c.Lookup).Include(c => c.Lookup1).Where(c => c.SectionCourses.Where(sc => sc.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0).Count() > 0);
                return View(courses.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Course/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.UserType = adtype();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: /Course/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "COURSESTATUS"), "Id", "Values");
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values");
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Title,Status,Class")] Course course)
        {
            if(adtype() == "Admin")
            {
                ViewBag.UserType = adtype();
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "COURSESTATUS"), "Id", "Values", course.Status);
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", course.Class);
                return View(course);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if(adtype() == "Admin")
            {
                ViewBag.UserType = adtype();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "COURSESTATUS"), "Id", "Values", course.Status);
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", course.Class);
                return View(course);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Title,Status,Class")] Course course)
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "COURSESTATUS"), "Id", "Category", course.Status);
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Category", course.Class);
                return View(course);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Course/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }
                return View(course);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (adtype() == "Admin")
            {
                ViewBag.UserType = adtype();
                Course course = db.Courses.Find(id);
                db.Courses.Remove(course);
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
