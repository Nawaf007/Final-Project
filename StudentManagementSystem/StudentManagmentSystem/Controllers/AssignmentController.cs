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
    public class AssignmentController : Controller
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
        // GET: /Assignment/
        public FileResult Download(string path)
        {
            if (path != "Null" && System.IO.File.Exists(Server.MapPath("~") + path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~") + path);
                string fileName = path.Substring(11);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }
        public ActionResult Index(int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.UserType = adtype();
            if (idsc != null)
            {
                if (adtype() == "Admin")
                {
                    var assignments = db.Assignments.Include(a => a.Lookup).Include(a => a.SectionCourse).Where(s => s.SCId == idsc);
                    return View(assignments.ToList());
                }
                else if (adtype() == "Teacher")
                {
                    var assignments = db.Assignments.Include(a => a.Lookup).Include(a => a.SectionCourse).Where(s => s.SCId == idsc && s.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name);
                    return View(assignments.ToList());
                }
                else if(adtype() == "Student")
                {
                    var assignments = db.Assignments.Include(a => a.Lookup).Include(a => a.SectionCourse).Where(s => s.SCId == idsc && s.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                    return View(assignments.ToList());
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Assignment/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: /Assignment/Create
        public ActionResult Create(int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if ((adtype() == "Admin") || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ASSIGNMENTSTATUS"), "Id", "Values");
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", idsc);
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Assignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SCId,Name,DueDate,Status,TotalMarks,Weightage,FilePath")] Assignment assignment, HttpPostedFileBase file, int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            int tw = db.Assignments.Where(e => e.SCId == idsc).Sum(e => e.Weightage);
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                if (ModelState.IsValid && (tw + assignment.Weightage) <= 100)
                {
                    db.Assignments.Add(assignment);
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/Assignments/Manuals"), assignment.Id.ToString() + System.IO.Path.GetExtension(file.FileName));
                        assignment.FilePath = "/Assignments/Manuals/" + assignment.Id + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(path);
                        db.SaveChanges();
                    }
                    else
                    {
                        assignment.FilePath = " ";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                if ((tw + assignment.Weightage) > 100)
                {
                    ViewBag.WError = "Weightage cannot be greater than " + (100 - tw).ToString() + ".";
                }
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ASSIGNMENTSTATUS"), "Id", "Values", assignment.Status);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", assignment.SCId);
                return View(assignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Assignment/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Assignment assignment = db.Assignments.Find(id);
                if (assignment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ASSIGNMENTSTATUS"), "Id", "Values", assignment.Status);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", assignment.SCId);
                return View(assignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Assignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SCId,Name,DueDate,Status,TotalMarks,Weightage,FilePath")] Assignment assignment, HttpPostedFileBase file, int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            int tw = db.Assignments.Where(e => e.SCId == idsc && e.Id != assignment.Id).Sum(e => e.Weightage);
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                if (ModelState.IsValid && (tw + assignment.Weightage) <= 100)
                {
                    db.Entry(assignment).State = EntityState.Modified;
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + assignment.FilePath))
                        {
                            System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + assignment.FilePath);
                        }
                        string path = System.IO.Path.Combine(Server.MapPath("~/Assignments/Manuals"), assignment.Id.ToString() + System.IO.Path.GetExtension(file.FileName));
                        assignment.FilePath = "/Assignments/Manuals/" + assignment.Id + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(path);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc });
                }
                if ((tw + assignment.Weightage) > 100)
                {
                    ViewBag.WError = "Weightage cannot be greater than " + (100 - tw).ToString() + ".";
                }
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "ASSIGNMENTSTATUS"), "Id", "Values", assignment.Status);
                ViewBag.SCId = new SelectList(db.SectionCourses, "Id", "SectionId", assignment.SCId);
                return View(assignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Assignment/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Assignment assignment = db.Assignments.Find(id);
                if (assignment == null)
                {
                    return HttpNotFound();
                }
                return View(assignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idsc, int? idc)
        {
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            Assignment assignment = db.Assignments.Find(id);
            var sc = db.SectionCourses.Find(idsc);
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || (adtype() == "Teacher" && sc.Course.Lookup.Values == "Active" && sc.Teacher.AspNetUser.UserName == User.Identity.Name))
            {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + assignment.FilePath))
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + assignment.FilePath);
                }
                db.Assignments.Remove(assignment);
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
