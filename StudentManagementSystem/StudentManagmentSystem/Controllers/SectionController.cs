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
    public class SectionController : Controller
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
        // GET: /Section/
        public ActionResult Index()
        {
            ViewBag.CurrentTab = "Section";
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                var sections = db.Sections.Include(s => s.Lookup);
                return View(sections.ToList());
            }
            else if (adtype() == "Teacher")
            {
                var sections = db.Sections.Include(s => s.Lookup).Where(s => s.SectionCourses.Where(sc => sc.Teacher.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                return View(sections.ToList());
            }
            else if (adtype() == "Student")
            {
                var sections = db.Sections.Include(s => s.Lookup).Where(c => c.SectionCourses.Where(sc => sc.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0).Count() > 0);
                return View(sections.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Section/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: /Section/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values");
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,MaxCount,Class")] Section section)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Sections.Add(section);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", section.Class);
                return View(section);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Section/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Section section = db.Sections.Find(id);
                if (section == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", section.Class);
                return View(section);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,MaxCount,Class")] Section section)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(section).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", section.Class);
                return View(section);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Section/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Section section = db.Sections.Find(id);
                if (section == null)
                {
                    return HttpNotFound();
                }
                return View(section);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                Section section = db.Sections.Find(id);
                db.Sections.Remove(section);
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
