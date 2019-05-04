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
    public class StudentController : Controller
    {
        private DB34Entities db = new DB34Entities();
        private int ExamMarks(int eid, int ssid)
        {
            SSExam var = db.SSExams.Where(e => e.ExamId == eid && e.SSId == ssid).First();
            return var.ObtainedMarks;
        }
        private int AssignmentMarks(int aid, int ssid)
        {
            SSAssignment var = db.SSAssignments.Where(e => e.AssignmentId == aid && e.SSId == ssid).First();
            return (int)var.ObtainedMarks;
        }

        public ActionResult StudentsListWithMarks()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "StudentWithClassMarks.rpt"));

            rd.Database.Tables[2].SetDataSource(db.Students.Select(a => new { Id = a.Id, RegistrationNo = a.RegistrationNo }).ToList());
            //rd.Database.Tables[1].SetDataSource(db.SectionStudents.Select(sc => new { Id = ((sc.Section.SectionCourses.Sum(s => (s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) != null) ? s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) : 0) != null) ? (sc.Section.SectionCourses.Sum(s => (s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) != null) ? s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) : 0)) : 0) + ((sc.Section.SectionCourses.Sum(s => (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage)))))) != null) ? (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage))))))) : 0) != null) ? (sc.Section.SectionCourses.Sum(s => (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage)))))) != null) ? (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage))))))) : 0)) : 0), SectionId = sc.SectionId, StudentId = sc.StudentId }).ToList());
            rd.Database.Tables[0].SetDataSource(db.SectionStudents.Select(ss => new{ Id = (ss.SSExams.Sum(sse => sse.ObtainedMarks) != null) ? ss.SSExams.Sum(sse => sse.ObtainedMarks) : 0 , SectionId = ss.SectionId, StudentId = ss.StudentId }).ToList());
            rd.Database.Tables[1].SetDataSource(db.Sections.Select(sc => new { Id = sc.Id, Class = sc.Lookup.Values }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "SLMList.pdf");
        }
        public ActionResult StudentsMarks(string id)
        {
            if(id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "ResultReport.rpt"));

            rd.Database.Tables[0].SetDataSource(db.Students.Where(s => s.Id == id).Select(a => new { Id = a.Id, RegistrationNo = a.RegistrationNo }).ToList());
            rd.Database.Tables[1].SetDataSource(db.SectionStudents.ToList());
            rd.Database.Tables[2].SetDataSource(db.SSAssignments.Select(a => new { Id = a.Id, AssignmentId = a.AssignmentId, SSId = a.SSId, FilePath = (int)a.ObtainedMarks }).ToList());
            rd.Database.Tables[3].SetDataSource(db.SSExams.ToList());
            rd.Database.Tables[4].SetDataSource(db.Exams.Select(e => new { Id = e.Id, SCId = e.SCId, TotalMarks = e.TotalMarks, Weightage = e.Weightage, ExamType = e.Lookup.Values, ExamDate = e.ExamDate}).ToList());
            rd.Database.Tables[5].SetDataSource(db.Assignments.ToList());
            rd.Database.Tables[6].SetDataSource(db.SectionCourses.ToList());
            rd.Database.Tables[7].SetDataSource(db.Sections.Select(s => new { Id = s.Id, Name = s.Name, MaxCount = s.MaxCount, Class = s.Lookup.Values}).ToList());
            rd.Database.Tables[8].SetDataSource(db.Courses.ToList());
            /*
            //rd.Database.Tables[1].SetDataSource(db.SectionStudents.Select(sc => new { Id = ((sc.Section.SectionCourses.Sum(s => (s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) != null) ? s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) : 0) != null) ? (sc.Section.SectionCourses.Sum(s => (s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) != null) ? s.Exams.Sum(e => ((sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks) != null) ? (sc.SSExams.Where(sse => sse.ExamId == e.Id).Sum(sse => sse.ObtainedMarks / e.TotalMarks)) : 0) * e.Weightage) : 0)) : 0) + ((sc.Section.SectionCourses.Sum(s => (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage)))))) != null) ? (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage))))))) : 0) != null) ? (sc.Section.SectionCourses.Sum(s => (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage)))))) != null) ? (s.Assignments.Sum(e => ((sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks) != null) ? (sc.SSAssignments.Where(sse => sse.AssignmentId == e.Id).Sum(sse => ((sse.ObtainedMarks != null) ? sse.ObtainedMarks : 0) / e.TotalMarks)) : 0) * (100 * (((e.Weightage / ((s.Assignments.Sum(a => a.Weightage) != null) ? (s.Assignments.Sum(a => a.Weightage)) : 0)) * 100) / (100 - ((s.Exams.Sum(ex => ex.Weightage) == null) ? 0 : s.Exams.Sum(ex => ex.Weightage))))))) : 0)) : 0), SectionId = sc.SectionId, StudentId = sc.StudentId }).ToList());
            rd.Database.Tables[0].SetDataSource(db.SectionStudents.Select(ss => new { Id = (ss.SSExams.Sum(sse => sse.ObtainedMarks) != null) ? ss.SSExams.Sum(sse => sse.ObtainedMarks) : 0, SectionId = ss.SectionId, StudentId = ss.StudentId }).ToList());
            rd.Database.Tables[1].SetDataSource(db.Sections.Select(sc => new { Id = sc.Id, Class = sc.Lookup.Values }).ToList());*/
            //rd.Database.Tables[1].SetDataSource(db.SectionStudents.Where(ss => ss.StudentId == id).Select(ss => new { Id = (((ss.SSExams.Sum(sse => (sse.ObtainedMarks / sse.Exam.TotalMarks) * sse.Exam.Weightage) != null) ? (ss.SSExams.Sum(sse => (sse.ObtainedMarks / sse.Exam.TotalMarks) * sse.Exam.Weightage)) : 0) + (((ss.SSAssignments.Sum(ssa => (ssa.ObtainedMarks / ssa.Assignment.TotalMarks) * ((ssa.Assignment.Weightage * 100) / ssa.Assignment.SectionCourse.Assignments.Sum(a => a.Weightage))) * 100 / ((ss.SSExams.Sum(sse => sse.Exam.Weightage) != null) ? (ss.SSExams.Sum(sse => sse.Exam.Weightage)) : 1)) != null) ? ((ss.SSAssignments.Sum(ssa => (ssa.ObtainedMarks / ssa.Assignment.TotalMarks) * ((ssa.Assignment.Weightage * 100) / ssa.Assignment.SectionCourse.Assignments.Sum(a => a.Weightage))) * 100 / ((ss.SSExams.Sum(sse => sse.Exam.Weightage) != null) ? (ss.SSExams.Sum(sse => sse.Exam.Weightage)) : 1))) : 0)) }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Result.pdf");
        }
        public ActionResult StudentsList()
        {
            List<Student> allCustomer = new List<Student>();
            allCustomer = db.Students.ToList();

            List<AspNetUser> allCustomer2 = new List<AspNetUser>();
            var pers = db.AspNetUsers.Select(a => new { Id = a.Id, FirstName = a.FirstName + " " + a.LastName}).ToList();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "StudentsList.rpt"));

            rd.Database.Tables[0].SetDataSource(allCustomer);
            rd.Database.Tables[1].SetDataSource(pers);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "StudentList.pdf");
        }
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
        // GET: /Student/
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Student/Details/5
        public ActionResult Details(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Student/Create
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

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,EnrollementDate,RegistrationNo")] Student student)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,EnrollementDate,RegistrationNo")] Student student)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                AspNetUser student = db.AspNetUsers.Find(id);
                var sses = db.SSExams.Where(sse => sse.SectionStudent.Student.Id == id);
                var ssas = db.SSAssignments.Where(ssa => ssa.SectionStudent.Student.Id == id);
                foreach (SSExam sse in sses)
                {
                    if (db.SSExams.Find(sse) != null)
                    {
                        db.SSExams.Remove(sse);
                    }
                }
                foreach (SSAssignment ssa in ssas)
                {
                    if (db.SSAssignments.Find(ssa) != null)
                    {
                        db.SSAssignments.Remove(ssa);
                    }
                }
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
