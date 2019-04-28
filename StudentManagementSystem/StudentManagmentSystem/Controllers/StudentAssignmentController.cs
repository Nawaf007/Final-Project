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
    public class StudentAssignmentController : Controller
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
        // GET: /StudentAssignment/
        public ActionResult Index(int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (ida != null)
                {
                    var ssassignments = db.SSAssignments.Include(s => s.Assignment).Include(s => s.SectionStudent).Where(s => s.AssignmentId == ida);
                    return View(ssassignments.ToList());
                }
                if (idss != null)
                {
                    var ssassignments = db.SSAssignments.Include(s => s.Assignment).Include(s => s.SectionStudent).Where(s => s.SSId == idss);
                    return View(ssassignments.ToList());
                }
                var ssassignmentss = db.SSAssignments.Include(s => s.Assignment).Include(s => s.SectionStudent);
                return View(ssassignmentss.ToList());
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAssignment/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSAssignment ssassignment = db.SSAssignments.Find(id);
            if (ssassignment == null)
            {
                return HttpNotFound();
            }
            return View(ssassignment);
        }

        // GET: /StudentAssignment/Create
        public ActionResult Create(int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            ViewBag.Error = "";
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (adtype() == "Teacher" && db.Assignments.Where(a => a.Id == ida && a.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ida != null)
                {
                    ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ida);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id && sse.AssignmentId == ida).Count() == 0);
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
                if (idss != null)
                {
                    var look = db.Assignments.Where(a => db.SSAssignments.Where(ss => ss.AssignmentId == a.Id).Count() == 0);
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (Assignment item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Name;
                        m.Value = item.Id.ToString();
                        status.Add(m);
                    }
                    ViewBag.AssignmentId = status;
                    ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", idss);
                    return View();
                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name");
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId");
                return View();
            }
            else if(adtype() == "Student")
            {
                if (ida != null)
                {
                    ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ida);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id && sse.AssignmentId == ida && sse.SectionStudent.Student.AspNetUser.UserName == User.Identity.Name).Count() == 0);
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        m.Selected = true;
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /StudentAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentId,SSId,ObtainedMarks,FilePath,Id")] SSAssignment ssassignment, HttpPostedFileBase file, int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.Error = "";
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            Assignment var = db.Assignments.Find(ssassignment.AssignmentId);
            int? om = ssassignment.ObtainedMarks;
            if(om == null)
            {
                om = -999;
            }
           if (ModelState.IsValid && var.TotalMarks > om)
            {
                db.SSAssignments.Add(ssassignment);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Assignments/Solutions"), ssassignment.Id.ToString() + System.IO.Path.GetExtension(file.FileName));
                    ssassignment.FilePath = "/Assignments/Solutions/" + ssassignment.Id + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(path);
                    db.SaveChanges();
                }
                else
                {
                    ssassignment.FilePath = " ";
                    db.SaveChanges();
                }
                if (adtype() == "Student")
                {
                    return RedirectToAction("Index", "Assignment", new { idsc = idsc, ids = ids, idc = idc, ida = ida, idss = idss });
                }
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ida = ida, idss = idss });
            }
            if (var.TotalMarks < ssassignment.ObtainedMarks)
            {
                ViewBag.Error = "Obtained Marks cannot be greater than total marks.";
            }
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (adtype() == "Teacher" && db.Assignments.Where(a => a.Id == ida && a.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (ida != null)
                {
                    ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id).Count() == 0);
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        if (m.Value == ssassignment.SSId.ToString())
                        {
                            m.Selected = true;
                        }
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View(ssassignment);
                }
                if (idss != null)
                {
                    var look = db.Assignments.Where(e => db.SSAssignments.Where(ss => ss.AssignmentId == e.Id).Count() == 0);
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (Assignment item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Name;
                        m.Value = item.Id.ToString();
                        if (m.Value == ssassignment.AssignmentId.ToString())
                        {
                            m.Selected = true;
                        }
                        status.Add(m);
                    }
                    ViewBag.AssignmentId = status;
                    ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                    return View(ssassignment);
                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                return View(ssassignment);
            }
            else if (adtype() == "Student")
            {
                if (ida != null && db.Assignments.Where(a => a.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0).Count() > 0)
                {
                    ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ida);
                    ViewBag.SSId = new SelectList(db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id).Count() == 0), "Id", "Id");
                    var look = db.SectionStudents.Where(ss => db.SSAssignments.Where(sse => sse.SSId == ss.Id && sse.AssignmentId == ida && sse.SectionStudent.Student.AspNetUser.UserName == User.Identity.Name).Count() == 0);
                    IList<SelectListItem> status = new List<SelectListItem> { };
                    foreach (SectionStudent item in look)
                    {
                        SelectListItem m = new SelectListItem();
                        m.Text = item.Student.RegistrationNo;
                        m.Value = item.Id.ToString();
                        m.Selected = true;
                        status.Add(m);
                    }
                    ViewBag.SSId = status;
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAssignment/Edit/5
        public ActionResult Edit(int? id, int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            ViewBag.Error = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSAssignment ssassignment = db.SSAssignments.Find(id);
            if (ssassignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (adtype() == "Teacher" && db.Assignments.Where(a => a.Id == ida && a.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                return View(ssassignment);
            }
            else if(adtype() == "Student" && ida != null && db.Assignments.Where(a => a.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0).Count() > 0)
            {
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                return View(ssassignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /StudentAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignmentId,SSId,ObtainedMarks,FilePath,Id")] SSAssignment ssassignment, HttpPostedFileBase file, int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            Assignment var = db.Assignments.Find(ssassignment.AssignmentId);
            int? om = ssassignment.ObtainedMarks;
            if (om == null)
            {
                om = -999;
            }
            if (ModelState.IsValid && var.TotalMarks >= om)
            {
                db.Entry(ssassignment).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + ssassignment.FilePath))
                    {
                        System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + ssassignment.FilePath);
                    }
                    string path = System.IO.Path.Combine(Server.MapPath("~/Assignments/Solutions"), ssassignment.Id.ToString() + System.IO.Path.GetExtension(file.FileName));
                    ssassignment.FilePath = "/Assignments/Solutions/" + ssassignment.Id + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(path);
                    db.SaveChanges();
                }
                if (adtype() == "Student")
                {
                    return RedirectToAction("Index", "Assignment", new { idsc = idsc, ids = ids, idc = idc, ida = ida, idss = idss });
                }
                return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ida = ida, idss = idss });
            }
            ViewBag.Error = "";
            if (var.TotalMarks < ssassignment.ObtainedMarks)
            {
                ViewBag.Error = "Obtained Marks cannot be greater than total marks.";
            }
            ViewBag.UserType = adtype();
            if (adtype() == "Admin" || adtype() == "Teacher")
            {
                if (adtype() == "Teacher" && db.Assignments.Where(a => a.Id == ida && a.SectionCourse.Teacher.AspNetUser.UserName == User.Identity.Name).Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                return View(ssassignment);
            }
            else if (adtype() == "Student" && ida != null && db.Assignments.Where(a => a.SectionCourse.Section.SectionStudents.Where(ss => ss.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0).Count() > 0)
            {
                ViewBag.AssignmentId = new SelectList(db.Assignments, "Id", "Name", ssassignment.AssignmentId);
                ViewBag.SSId = new SelectList(db.SectionStudents, "Id", "StudentId", ssassignment.SSId);
                return View(ssassignment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /StudentAssignment/Delete/5
        public ActionResult Delete(int? id, int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SSAssignment ssassignment = db.SSAssignments.Find(id);
            if (ssassignment == null)
            {
                return HttpNotFound();
            }
            return View(ssassignment);
        }

        // POST: /StudentAssignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? ids, int? idsc, int? idc, int? ida, int? idss)
        {
            ViewBag.idss = idss;
            ViewBag.idc = idc;
            ViewBag.ids = ids;
            ViewBag.idsc = idsc;
            ViewBag.ida = ida;
            SSAssignment ssassignment = db.SSAssignments.Find(id);
            if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + ssassignment.FilePath))
            {
                System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + ssassignment.FilePath);
            }
            db.SSAssignments.Remove(ssassignment);
            db.SaveChanges();
            return RedirectToAction("Index", new { idsc = idsc, ids = ids, idc = idc, ida = ida, idss = idss });
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
