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
    public class StudentController : Controller
    {
        private DB34Entities db = new DB34Entities();

        // GET: /Student/
        public ActionResult Index()
        {
            ViewBag.CurrentTab = "Student";
            var stue = db.Students.Include(s => s.AspNetUser);
            List<RegisterViewModel> slist = new List<RegisterViewModel>();
            foreach (Student stu in stue)
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
                s.EnrollementDate = stu.EnrollementDate;
                s.RegistrationNo = stu.RegistrationNo;
                ViewBag.Gender = student.Lookup.Values;
                slist.Add(s);
            }
            return View(slist);
        }

        // GET: /Student/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student stu = db.Students.Find(id);
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
            s.EnrollementDate = stu.EnrollementDate;
            s.RegistrationNo = stu.RegistrationNo;
            ViewBag.Gender = student.Lookup.Values;
            return View(s);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,EnrollementDate,RegistrationNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,EnrollementDate,RegistrationNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", student.Id);
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student stu = db.Students.Find(id);
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
            s.EnrollementDate = stu.EnrollementDate;
            s.RegistrationNo = stu.RegistrationNo;
            ViewBag.Gender = student.Lookup.Values;
            return View(s);
        }

        // POST: /Student/Delete/5
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
