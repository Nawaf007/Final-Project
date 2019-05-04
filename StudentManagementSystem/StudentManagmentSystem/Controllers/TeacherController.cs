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
    public class TeacherController : Controller
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
        // GET: /Teacher/
        public ActionResult TWCList()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "TeacherWithCourseReport.rpt"));

            rd.Database.Tables[0].SetDataSource(db.AspNetUsers.Select(a => new { Id = a.Id, FirstName = a.FirstName + " " + a.LastName }).ToList());
            rd.Database.Tables[1].SetDataSource(db.SectionCourses.Select(sc => new{Id = sc.Id, CourseId = sc.CourseId, TeacherId = sc.TeacherId}).ToList());
            rd.Database.Tables[2].SetDataSource(db.Courses.Select(sc => new{ Id = sc.Id, Title = sc.Title, Class = sc.Lookup1.Values}).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "TWCList.pdf");
        }
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Teacher/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Teacher/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Salary")] Teacher teacher)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Teacher/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Salary")] Teacher teacher)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Teacher/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                AspNetUser student = db.AspNetUsers.Find(id);
                db.AspNetUsers.Remove(student);
                db.SaveChanges();
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
