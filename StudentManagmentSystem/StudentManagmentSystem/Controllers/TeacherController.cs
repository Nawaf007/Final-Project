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
    public class TeacherController : Controller
    {
        private DB34Entities db = new DB34Entities();

        // GET: /Teacher/
        public ActionResult Index()
        {
            ViewBag.CurrentTab = "Teacher";
            var tea = db.Teachers.Include(t => t.AspNetUser);
            List<RegisterViewModel> slist = new List<RegisterViewModel>();
            foreach (Teacher stu in tea)
            {
                AspNetUser student = db.AspNetUsers.Find(stu.Id);
                RegisterViewModel s = new RegisterViewModel();
                s.Id = student.Id;
                s.UserName = student.UserName;
                s.Admin = student.Admin;
                s.Email = student.Email;
                s.FirstName = student.FirstName;
                s.LastName = student.LastName;
                s.Gender = student.Gender.ToString();
                s.PhoneNumber = student.PhoneNumber;
                s.Salary = stu.Salary.ToString();
                ViewBag.Gender = student.Lookup.Values;
                slist.Add(s);
            }
            return View(slist);
        }

        // GET: /Teacher/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher stu = db.Teachers.Find(id);
            AspNetUser student = db.AspNetUsers.Find(id);
            if (stu == null || student == null)
            {
                return HttpNotFound();
            }
            RegisterViewModel s = new RegisterViewModel();
            s.Id = student.Id;
            s.UserName = student.UserName;
            s.Admin = student.Admin;
            s.Email = student.Email;
            s.FirstName = student.FirstName;
            s.LastName = student.LastName;
            s.Gender = student.Gender.ToString();
            s.PhoneNumber = student.PhoneNumber;
            s.Salary = stu.Salary.ToString();
            ViewBag.Gender = student.Lookup.Values;
            return View(s);
        }

        // GET: /Teacher/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: /Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Salary")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", teacher.Id);
            return View(teacher);
        }

        // GET: /Teacher/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", teacher.Id);
            return View(teacher);
        }

        // POST: /Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Salary")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", teacher.Id);
            return View(teacher);
        }

        // GET: /Teacher/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher stu = db.Teachers.Find(id);
            AspNetUser student = db.AspNetUsers.Find(id);
            if (stu == null || student == null)
            {
                return HttpNotFound();
            }
            RegisterViewModel s = new RegisterViewModel();
            s.UserName = student.UserName;
            s.Admin = student.Admin;
            s.Email = student.Email;
            s.FirstName = student.FirstName;
            s.LastName = student.LastName;
            s.Gender = student.Gender.ToString();
            s.PhoneNumber = student.PhoneNumber;
            s.Salary = stu.Salary.ToString();
            ViewBag.Gender = student.Lookup.Values;
            return View(s);
        }

        // POST: /Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser student = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(student);
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
