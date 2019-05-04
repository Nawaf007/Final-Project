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
    public class SCTimeController : Controller
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
        public ActionResult TimeTable()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.CurrentTab = "TimeTable";
                var sctimess = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionCourse.Course.Lookup.Values == "Active");
                return View(sctimess.ToList());
            }
            else if(adtype() == "Teacher")
            {
                ViewBag.CurrentTab = "TimeTable";
                var sctimess = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionCourse.Course.Lookup.Values == "Active" && s.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name);
                return View(sctimess.ToList());
            }
            else if(adtype() == "Student")
            {
                ViewBag.CurrentTab = "TimeTable";
                var sctimess = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionCourse.Course.Lookup.Values == "Active" && s.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                return View(sctimess.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SCTime/
        public ActionResult Index(int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            if(idsc != null)
            {
                ViewBag.UserType = adtype();
                if (adtype() == "Admin")
                {
                    var sctimes = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionId == idsc);
                    return View(sctimes.ToList());
                }
                else if (adtype() == "Teacher")
                {
                    var sctimes = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionId == idsc && s.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name);
                    return View(sctimes.ToList());
                }
                else if (adtype() == "Student")
                {
                    var sctimes = db.SCTimes.Include(s => s.Lookup).Include(s => s.SectionCourse).Where(s => s.SectionId == idsc && s.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                    return View(sctimes.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SCTime/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SCTime sctime = db.SCTimes.Find(id);
            if (sctime == null)
            {
                return HttpNotFound();
            }
            return View(sctime);
        }

        // GET: /SCTime/Create
        public ActionResult Create(int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                ViewBag.idsc = idsc;
                ViewBag.DayOfWeek = new SelectList(db.Lookups.Where(l => l.Category == "WORKINGDAYS"), "Id", "Values");
                ViewBag.SectionId = new SelectList(db.SectionCourses, "Id", "SectionId", idsc);
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SCTime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionId,DayOfWeek,TimeOfDay,Id")] SCTime sctime, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (ModelState.IsValid)
                {
                    db.SCTimes.Add(sctime);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                ViewBag.idsc = idsc;
                ViewBag.DayOfWeek = new SelectList(db.Lookups.Where(l => l.Category == "WORKINGDAYS"), "Id", "Values", sctime.DayOfWeek);
                ViewBag.SectionId = new SelectList(db.SectionCourses, "Id", "SectionId", sctime.SectionId);
                return View(sctime);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SCTime/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SCTime sctime = db.SCTimes.Find(id);
                if (sctime == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idsc = idsc;
                ViewBag.DayOfWeek = new SelectList(db.Lookups.Where(l => l.Category == "WORKINGDAYS"), "Id", "Values", sctime.DayOfWeek);
                ViewBag.SectionId = new SelectList(db.SectionCourses, "Id", "SectionId", sctime.SectionId);
                return View(sctime);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SCTime/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,DayOfWeek,TimeOfDay,Id")] SCTime sctime, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (ModelState.IsValid)
                {
                    db.Entry(sctime).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                ViewBag.idsc = idsc;
                ViewBag.DayOfWeek = new SelectList(db.Lookups.Where(l => l.Category == "WORKINGDAYS"), "Id", "Values", sctime.DayOfWeek);
                ViewBag.SectionId = new SelectList(db.SectionCourses, "Id", "SectionId", sctime.SectionId);
                return View(sctime);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SCTime/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SCTime sctime = db.SCTimes.Find(id);
                if (sctime == null)
                {
                    return HttpNotFound();
                }

                ViewBag.idsc = idsc;
                return View(sctime);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SCTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idsc, int? idc)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idc = idc;
                ViewBag.ids = ids;
                SCTime sctime = db.SCTimes.Find(id);
                db.SCTimes.Remove(sctime);
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
