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
    public class RequestController : Controller
    {
        private DB34Entities db = new DB34Entities();
        public ActionResult RequestReport()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "RequestReport.rpt"));

            rd.Database.Tables[0].SetDataSource(db.Requests.Select(a => new { Topic = a.Topic, Description = a.AspNetUser.UserName, Acknowledged = a.AspNetUser1.UserName}).ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "RequestReport.pdf");
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
        // GET: /Request/
        public ActionResult Index()
        {
            ViewBag.CurrentTab = "Request";
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                var requests = db.Requests.Include(r => r.AspNetUser);
                return View(requests.ToList());
            }
            else if (adtype() == "Teacher" || adtype() == "Student")
            {
                var requests = db.Requests.Include(r => r.AspNetUser).Where(r => r.AspNetUser.UserName == User.Identity.Name);
                return View(requests.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // GET: /Request/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Request request = db.Requests.Find(id);
                if (request == null)
                {
                    return HttpNotFound();
                }
                var user = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First();
                request.Acknowledged = user.Id;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return View(request);
            }
            else if(adtype() == "Teacher" || adtype() == "Student")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Request request = db.Requests.Find(id);
                if (request == null && request.AspNetUser.UserName != User.Identity.Name)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Request/Create
        public ActionResult Create()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Teacher" || adtype() == "Student")
            {
                ViewBag.AspId = new SelectList(db.AspNetUsers, "Id", "Email", db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First().Id);
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AspId,Description,Id,Topic")] Request request)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Teacher" || adtype() == "Student")
            {
                if (ModelState.IsValid)
                {
                    db.Requests.Add(request);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.AspId = new SelectList(db.AspNetUsers, "Id", "Email", request.AspId);
                return View(request);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: /Request/Edit/5
        public ActionResult Edit(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspId = new SelectList(db.AspNetUsers, "Id", "Email", request.AspId);
            return View(request);
        }

        // POST: /Request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AspId,Description,Id")] Request request)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspId = new SelectList(db.AspNetUsers, "Id", "Email", request.AspId);
            return View(request);
        }

        // GET: /Request/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Request request = db.Requests.Find(id);
                if (request == null)
                {
                    return HttpNotFound();
                }
                return View(request);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: /Request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                Request request = db.Requests.Find(id);
                db.Requests.Remove(request);
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
