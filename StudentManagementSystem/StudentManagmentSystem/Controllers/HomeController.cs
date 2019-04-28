using StudentManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StudentManagmentSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.CurrentTab = "Dashboard";
                ViewBag.SalaryPaid = db.SalaryPaids.Sum(sp => sp.Teacher.Salary);
                ViewBag.ActiveCourses = db.Courses.Where(c => c.Lookup.Values == "Active").Count();
                ViewBag.TotalCourses = db.Courses.Count();
                ViewBag.Sections = db.Sections.Count();
                ViewBag.Teachers = db.Teachers.Count();
                ViewBag.Students = db.Students.Count();
                ViewBag.Requests = db.Requests.Where(r => r.Acknowledged == null).Count();
                ViewBag.Users = db.AspNetUsers.Count();
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        public ActionResult About()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}