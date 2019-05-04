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
    public class SalaryPaidController : Controller
    {
        private DB34Entities db = new DB34Entities();
        public ActionResult NRSReport()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "NotRecievedSalaryReport.rpt"));

            rd.Database.Tables[0].SetDataSource(db.SalaryPaids.Where(s => s.Lookup.Values == "NotPaid").Select(a => new { TeacherId = a.Teacher.AspNetUser.UserName, Date = a.Date }).ToList());
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "NRSReport.pdf");
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
        // GET: /SalaryPaid/
        public ActionResult Index(string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idt == null)
                {
                    ViewBag.CurrentTab = "Salary";
                    var salarypaidss = db.SalaryPaids.Include(s => s.Lookup).Include(s => s.Teacher);
                    return View(salarypaidss.ToList());
                }
                ViewBag.idt = idt;
                var salarypaids = db.SalaryPaids.Include(s => s.Lookup).Include(s => s.Teacher).Where(s => s.TeacherId == idt);
                return View(salarypaids.ToList());
            }
            else if(adtype() == "Teacher")
            {
                if (idt == null)
                {
                    ViewBag.CurrentTab = "Salary";
                    var salarypaidss = db.SalaryPaids.Include(s => s.Lookup).Include(s => s.Teacher).Where(s => s.Teacher.AspNetUser.UserName == User.Identity.Name);
                    return View(salarypaidss.ToList());
                }
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /SalaryPaid/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalaryPaid salarypaid = db.SalaryPaids.Find(id);
            if (salarypaid == null)
            {
                return HttpNotFound();
            }
            return View(salarypaid);
        }

        // GET: /SalaryPaid/Create
        public ActionResult Create(string idt)
        {
             ViewBag.UserType = adtype();
             if (adtype() == "Admin")
             {
                 ViewBag.idt = idt;
                 ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "SALARYSTATUS"), "Id", "Values");
                 ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Id", idt);
                 return View();
             }
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SalaryPaid/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TeacherId,Date,Status,Id")] SalaryPaid salarypaid, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.SalaryPaids.Add(salarypaid);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idt = idt });
                }
                ViewBag.idt = idt;
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "SALARYSTATUS"), "Id", "Values", salarypaid.Status);
                ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Id", salarypaid.TeacherId);
                return View(salarypaid);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SalaryPaid/Edit/5
        public ActionResult Edit(int? id, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SalaryPaid salarypaid = db.SalaryPaids.Find(id);
                if (salarypaid == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idt = idt;
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "SALARYSTATUS"), "Id", "Values", salarypaid.Status);
                ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Id", salarypaid.TeacherId);
                return View(salarypaid);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SalaryPaid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TeacherId,Date,Status,Id")] SalaryPaid salarypaid, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(salarypaid).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idt = idt });
                }
                ViewBag.idt = idt;
                ViewBag.Status = new SelectList(db.Lookups.Where(l => l.Category == "SALARYSTATUS"), "Id", "Values", salarypaid.Status);
                ViewBag.TeacherId = new SelectList(db.Teachers, "Id", "Id", salarypaid.TeacherId);
                return View(salarypaid);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /SalaryPaid/Delete/5
        public ActionResult Delete(int? id, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SalaryPaid salarypaid = db.SalaryPaids.Find(id);
                if (salarypaid == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idt = idt;
                return View(salarypaid);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /SalaryPaid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string idt)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                SalaryPaid salarypaid = db.SalaryPaids.Find(id);
                db.SalaryPaids.Remove(salarypaid);
                db.SaveChanges();
                return RedirectToAction("Index", new { idt = idt });
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
