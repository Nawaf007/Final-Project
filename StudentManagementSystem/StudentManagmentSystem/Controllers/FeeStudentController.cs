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
    public class FeeStudentController : Controller
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
        // GET: /FeeStudent/
        public ActionResult Index(int? idf, string ids)
        {
            ViewBag.idf = idf;
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idf != null)
                {
                    ViewBag.Showf = false;
                    ViewBag.shows = true;
                    var feesstudents = db.FeesStudents.Include(f => f.Fee).Include(f => f.Student).Where(f => f.FeeId == idf);
                    return View(feesstudents.ToList());
                }
                else if (ids != null)
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = false;
                    var feesstudents = db.FeesStudents.Include(f => f.Fee).Include(f => f.Student).Where(f => f.StudentId == ids);
                    return View(feesstudents.ToList());
                }
                else
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = true;
                    var feesstudents = db.FeesStudents.Include(f => f.Fee).Include(f => f.Student);
                    return View(feesstudents.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /FeeStudent/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeesStudent feesstudent = db.FeesStudents.Find(id);
            if (feesstudent == null)
            {
                return HttpNotFound();
            }
            return View(feesstudent);
        }

        // GET: /FeeStudent/Create
        public ActionResult Create(int? idf, string ids)
        {
            ViewBag.idf = idf;
            ViewBag.ids = ids;
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idf != null)
                {
                    ViewBag.Showf = false;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", idf);
                    ViewBag.StudentId = new SelectList(db.Students.Where(s => s.FeesStudents.Where(f => f.FeeId == idf).Count() == 0), "Id", "RegistrationNo");
                }
                else if (ids != null)
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = false;
                    ViewBag.FeeId = new SelectList(db.Fees.Where(f => f.FeesStudents.Where(fs => fs.StudentId == ids).Count() == 0), "Id", "Name");
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", ids);
                }
                else
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Id");
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo");
                }
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /FeeStudent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="FeeId,StudentId,AmountPaid,SubmitDate,Id")] FeesStudent feesstudent, int? idf, string ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.FeesStudents.Add(feesstudent);
                    db.SaveChanges();
                    if (idf != null)
                    {
                        return RedirectToAction("Index", new { idf = idf });
                    }
                    if (ids != null)
                    {
                        return RedirectToAction("Index", new { ids = ids });
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.idf = idf;
                ViewBag.ids = ids;
                if (idf != null)
                {
                    ViewBag.Showf = false;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students.Where(s => s.FeesStudents.Where(f => f.FeeId == idf).Count() == 0), "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else if (ids != null)
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = false;
                    ViewBag.FeeId = new SelectList(db.Fees.Where(f => f.FeesStudents.Where(fs => fs.StudentId == ids).Count() == 0), "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Id", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                return View(feesstudent);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /FeeStudent/Edit/5
        public ActionResult Edit(int? id, int? idf, string ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.idf = idf;
                ViewBag.ids = ids;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FeesStudent feesstudent = db.FeesStudents.Find(id);
                if (feesstudent == null)
                {
                    return HttpNotFound();
                }
                if (idf != null)
                {
                    ViewBag.Showf = false;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else if (ids != null)
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = false;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                return View(feesstudent);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /FeeStudent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeeId,StudentId,AmountPaid,SubmitDate,Id")] FeesStudent feesstudent, int? idf, string ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(feesstudent).State = EntityState.Modified;
                    db.SaveChanges();
                    if (idf != null)
                    {
                        return RedirectToAction("Index", new { idf = idf });
                    }
                    if (ids != null)
                    {
                        return RedirectToAction("Index", new { ids = ids });
                    }
                    return RedirectToAction("Index");
                }
                ViewBag.idf = idf;
                ViewBag.ids = ids;
                if (idf != null)
                {
                    ViewBag.Showf = false;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else if (ids != null)
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = false;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                else
                {
                    ViewBag.Showf = true;
                    ViewBag.shows = true;
                    ViewBag.FeeId = new SelectList(db.Fees, "Id", "Name", feesstudent.FeeId);
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "RegistrationNo", feesstudent.StudentId);
                }
                return View(feesstudent);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            
        }

        // GET: /FeeStudent/Delete/5
        public ActionResult Delete(int? id, int? idf, string ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FeesStudent feesstudent = db.FeesStudents.Find(id);
                if (feesstudent == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idf = idf;
                ViewBag.ids = ids;
                return View(feesstudent);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /FeeStudent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? idf, string ids)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                FeesStudent feesstudent = db.FeesStudents.Find(id);
                db.FeesStudents.Remove(feesstudent);
                db.SaveChanges();
                if (idf != null)
                {
                    return RedirectToAction("Index", new { idf = idf });
                }
                if (ids != null)
                {
                    return RedirectToAction("Index", new { ids = ids });
                }
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
