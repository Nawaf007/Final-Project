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
    public class CategoryController : Controller
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
        // GET: /Category/
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                return View(db.Lookups.Where(lookup => lookup.Category != "ADMIN" && lookup.Category != "COURSESTATUS" && lookup.Category != "ATTENDANCETYPE" && lookup.Category != "ATTENDANCESTATUS" && lookup.Category != "SALARYSTATUS" && lookup.Category != "ASSIGNMENTSTATUS").ToList());
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Category/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Find(id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            return View(lookup);
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                IList<SelectListItem> status = new List<SelectListItem> { };
                SelectListItem m1 = new SelectListItem();
                m1.Text = "Gender";
                m1.Value = "GENDER";
                status.Add(m1);
                SelectListItem m2 = new SelectListItem();
                m2.Text = "Class";
                m2.Value = "CLASS";
                status.Add(m2);
                SelectListItem m7 = new SelectListItem();
                m7.Text = "Working Days";
                m7.Value = "WORKINGDAYS";
                status.Add(m7);
                SelectListItem m8 = new SelectListItem();
                m8.Text = "Exam";
                m8.Value = "EXAM";
                status.Add(m8);
                ViewBag.Category = status;
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Category,Values")] Lookup lookup)
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid && lookup.Category != "ADMIN")
                {
                    db.Lookups.Add(lookup);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                IList<SelectListItem> status = new List<SelectListItem> { };
                SelectListItem m1 = new SelectListItem();
                m1.Text = "Gender";
                m1.Value = "GENDER";
                status.Add(m1);
                SelectListItem m2 = new SelectListItem();
                m2.Text = "Class";
                m2.Value = "CLASS";
                status.Add(m2);
                SelectListItem m7 = new SelectListItem();
                m7.Text = "Working Days";
                m7.Value = "WORKINGDAYS";
                status.Add(m7);
                SelectListItem m8 = new SelectListItem();
                m8.Text = "Exam";
                m8.Value = "EXAM";
                status.Add(m8);
                ViewBag.Category = status;
                return View(lookup);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lookup lookup = db.Lookups.Find(id);
                if (lookup == null)
                {
                    return HttpNotFound();
                }
                return View(lookup);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Category,Values")] Lookup lookup)
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid && lookup.Category != "ADMIN")
                {
                    db.Entry(lookup).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(lookup);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Category/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lookup lookup = db.Lookups.Find(id);
                if (lookup == null)
                {
                    return HttpNotFound();
                }
                return View(lookup);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Category";
            if (adtype() == "Admin")
            {
                Lookup lookup = db.Lookups.Find(id);
                if (lookup.Category != "ADMIN" && lookup.Category != "COURSESTATUS" && lookup.Category != "ATTENDANCETYPE" && lookup.Category != "ATTENDANCESTATUS" && lookup.Category != "SALARYSTATUS" && lookup.Category != "ASSIGNMENTSTATUS" && db.AspNetUsers.Where(a => a.Gender == id).Count() == 0 && db.Exams.Where(a => a.ExamType == id).Count() == 0 && db.Fees.Where(a => a.Class == id).Count() == 0 && db.StudentAttendances.Where(a => a.Status == id).Count() == 0 && db.Sections.Where(a => a.Class == id).Count() == 0 && db.Attendances.Where(a => a.Type == id).Count() == 0 && db.TeacherAttendances.Where(a => a.Status == id).Count() == 0 && db.Assignments.Where(a => a.Status == id).Count() == 0 && db.SalaryPaids.Where(a => a.Status == id).Count() == 0 && db.Courses.Where(a => a.Class == id).Count() == 0 && db.Courses.Where(a => a.Status == id).Count() == 0 && db.SCTimes.Where(a => a.DayOfWeek == id).Count() == 0)
                {
                    db.Lookups.Remove(lookup);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Error = "Cannot delete. Items are assigned to it.";
                    return View(lookup);
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
