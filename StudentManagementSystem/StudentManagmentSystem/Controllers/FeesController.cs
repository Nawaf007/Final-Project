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
    public class FeesController : Controller
    {
        private DB34Entities db = new DB34Entities();
        public ActionResult NPFReport()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "NotPaidFee.rpt"));
            DateTime time = (DateTime.Now);
            DateTime time2 = time.AddMonths(-1);
            rd.Database.Tables[0].SetDataSource(db.Students.Where(s => s.FeesStudents.Where(f => DateTime.Compare(f.SubmitDate, time2) > 0).Count() == 0).Select(a => new { Id = a.AspNetUser.UserName, RegistrationNo = a.RegistrationNo }).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "NotPaidFeeReport.pdf");
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
        // GET: /Fees/
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            ViewBag.CurrentTab = "Fees";
            if (adtype() == "Admin")
            {
                var fees = db.Fees.Include(f => f.Lookup);
                return View(fees.ToList());
            }
            else if(adtype() == "Student")
            {
                var fees = db.Fees.Include(f => f.Lookup).Where(f => f.FeesStudents.Where(fs => fs.Student.AspNetUser.UserName == User.Identity.Name).Count() > 0);
                return View(fees.ToList());
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Fees/Details/5
        public ActionResult Details(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = db.Fees.Find(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // GET: /Fees/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Fees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Amount,DueDate,Class")] Fee fee)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Fees.Add(fee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", fee.Class);
                return View(fee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Fees/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Fee fee = db.Fees.Find(id);
                if (fee == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", fee.Class);
                return View(fee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Fees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Amount,DueDate,Class")] Fee fee)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(fee).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Class = new SelectList(db.Lookups.Where(l => l.Category == "CLASS"), "Id", "Values", fee.Class);
                return View(fee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Fees/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Fee fee = db.Fees.Find(id);
                if (fee == null)
                {
                    return HttpNotFound();
                }
                return View(fee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                Fee fee = db.Fees.Find(id);
                db.Fees.Remove(fee);
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
