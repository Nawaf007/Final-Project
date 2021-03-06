﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StudentManagmentSystem.Models;

namespace StudentManagmentSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DB34Entities db = new DB34Entities();
        private string adtype()
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).First();
                int id = Convert.ToInt32(user.Admin);
                string ad = db.Lookups.Where(l => l.Id == id).First().Values.ToString();
                return ad;
            }
            return null;
        }
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(string UserName)
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                AspNetUser user = db.AspNetUsers.Where(a => a.UserName == UserName).First();
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                if (UserName == null || UserName != User.Identity.Name)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                AspNetUser user = db.AspNetUsers.Where(a => a.UserName == UserName).First();
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        public ActionResult Delete(string id)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                AspNetUser User = db.AspNetUsers.Find(id);
                if (User == null)
                {
                    return HttpNotFound();
                }
                return View(User);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // POST: /Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                AspNetUser User = db.AspNetUsers.Find(id);
                var sses = db.SSExams.Where(sse => sse.SectionStudent.Student.Id == id);
                var ssas = db.SSAssignments.Where(ssa => ssa.SectionStudent.Student.Id == id);
                foreach(SSExam sse in sses)
                {
                    if (db.SSExams.Find(sse) != null)
                    {
                        db.SSExams.Remove(sse);
                    }
                }
                foreach(SSAssignment ssa in ssas)
                {
                    if (db.SSAssignments.Find(ssa) != null)
                    {
                        db.SSAssignments.Remove(ssa);
                    }
                }
                db.AspNetUsers.Remove(User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.UserType = adtype();
            if(adtype() == "Admin")
            {
                ViewBag.CurrentTab = "IndexAccount";
                var users = db.AspNetUsers;
                return View(users.ToList());
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }
        public ActionResult Register()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                var per2 = db.Lookups.Where(a => a.Category == "ADMIN" && a.Values == "Student");
                foreach (Lookup item in per2)
                {
                    ViewBag.Student = item.Id;
                }
                per2 = db.Lookups.Where(a => a.Category == "ADMIN" && a.Values == "Teacher");
                foreach (Lookup item in per2)
                {
                    ViewBag.Teacher = item.Id;
                }
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        string admintype = "";
                        int admin = Convert.ToInt32(model.Admin);
                        var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                        var per2 = db.Lookups.Where(a => a.Id == admin);
                        foreach (Lookup item in per2)
                        {
                            admintype = item.Values;
                        }
                        foreach (AspNetUser item in per)
                        {
                            db.AspNetUsers.Find(item.Id).Email = model.Email;
                            db.AspNetUsers.Find(item.Id).Admin = model.Admin;
                            db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                            db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                            db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                            db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                            db.AspNetUsers.Find(item.Id).EmailConfirmed = false;
                            db.AspNetUsers.Find(item.Id).PhoneNumberConfirmed = false;
                            if (admintype == "Student")
                            {
                                Student m = new Student();
                                m.AspNetUser = item;
                                m.Id = item.Id;
                                m.EnrollementDate = (DateTime)model.EnrollementDate;
                                m.RegistrationNo = model.RegistrationNo;
                                db.Students.Add(m);
                                db.AspNetUsers.Find(item.Id).Student = m;
                            }
                            else if (admintype == "Teacher")
                            {
                                Teacher t = new Teacher();
                                t.AspNetUser = item;
                                t.Id = item.Id;
                                t.Salary = Convert.ToInt32(model.Salary);
                                db.Teachers.Add(t);
                                db.AspNetUsers.Find(item.Id).Teacher = t;
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }
        //
        // GET: /Account/Register
/*        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditRegister(string idd)
        {
            if (idd == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            AspNetUser student = db.AspNetUsers.Find(idd);
            if (student == null)
            {
                return HttpNotFound();
            }
            EditRegisterViewModel s = new EditRegisterViewModel();
            s.UserName = student.UserName;
            s.Admin = student.Admin;
            s.Email = student.Email;
            s.FirstName = student.FirstName;
            s.LastName = student.LastName;
            s.Gender = student.Gender.ToString();
            s.PhoneNumber = student.PhoneNumber;
            if(student.Teacher != null)
            {
                EditRegisterStudent(idd);
                return null;
            }
            if(student.Student != null)
            {

            }
            s.EnrollementDate = stu.EnrollementDate;
            return View(s);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string admintype = "";
                    await SignInAsync(user, isPersistent: false);
                    int admin = Convert.ToInt32(model.Admin);
                    var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                    var per2 = db.Lookups.Where(a => a.Id == admin);
                    foreach(Lookup item in per2)
                    {
                        admintype = item.Values;
                    }
                    foreach (AspNetUser item in per)
                    {
                        db.AspNetUsers.Find(item.Id).Email = model.Email;
                        db.AspNetUsers.Find(item.Id).Admin= model.Admin;
                        db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                        db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                        db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                        db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                        db.AspNetUsers.Find(item.Id).EmailConfirmed = false;
                        db.AspNetUsers.Find(item.Id).PhoneNumberConfirmed = false;
                        if(admintype == "Student")
                        {
                            Student m = new Student();
                            m.AspNetUser = item;
                            m.Id = item.Id;
                            m.EnrollementDate = (DateTime)model.EnrollementDate;
                            m.RegistrationNo = model.RegistrationNo;
                            db.Students.Add(m);
                            db.AspNetUsers.Find(item.Id).Student = m;
                        }
                        else
                        {
                            Teacher t = new Teacher();
                            t.AspNetUser = item;
                            t.Id = item.Id;
                            t.Salary = Convert.ToInt32(model.Salary);
                            db.Teachers.Add(t);
                            db.AspNetUsers.Find(item.Id).Teacher = t;
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
*/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterStudent()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                ViewBag.stuexi = "";
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterStudent(RegisterViewModel model)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                bool notexi = true;
                if (ModelState.IsValid)
                {
                    var stuexi = db.Students.Where(s => s.RegistrationNo == model.RegistrationNo);
                    foreach (var item in stuexi)
                    {
                        notexi = false;
                    }
                    if (notexi)
                    {
                        var user = new ApplicationUser() { UserName = model.UserName };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded && notexi)
                        {
                            Student m = new Student();
                            int admin = Convert.ToInt32(model.Admin);
                            var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                            var per2 = db.Lookups.Where(a => a.Values == "Student" && a.Category == "ADMIN");
                            foreach (Lookup item in per2)
                            {
                                model.Admin = item.Id.ToString();
                            }
                            foreach (AspNetUser item in per)
                            {
                                db.AspNetUsers.Find(item.Id).Email = model.Email;
                                db.AspNetUsers.Find(item.Id).Admin = model.Admin;
                                db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                                db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                                db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                                db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                                db.AspNetUsers.Find(item.Id).EmailConfirmed = false;
                                db.AspNetUsers.Find(item.Id).PhoneNumberConfirmed = false;
                                m.AspNetUser = item;
                                m.Id = item.Id;
                                m.EnrollementDate = (DateTime)model.EnrollementDate;
                                m.RegistrationNo = model.RegistrationNo;
                                db.Students.Add(m);
                                db.AspNetUsers.Find(item.Id).Student = m;
                            }
                            db.SaveChanges();
                            return RedirectToAction("Index", "Student");
                        }
                        else
                        {
                            AddErrors(result);
                            ViewBag.stuexi = "";
                        }
                    }
                    else
                    {
                        ViewBag.stuexi = "Registration No. already exists.";
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditRegisterStudent(string idd)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idd == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                AspNetUser student = db.AspNetUsers.Find(idd);
                Student stu = db.Students.Find(idd);
                if (student == null || stu == null)
                {
                    return HttpNotFound();
                }
                EditRegisterViewModel s = new EditRegisterViewModel();
                s.UserName = student.UserName;
                s.Admin = student.Admin;
                s.Email = student.Email;
                s.FirstName = student.FirstName;
                s.LastName = student.LastName;
                s.Gender = student.Gender.ToString();
                s.PhoneNumber = student.PhoneNumber;
                s.EnrollementDate = stu.EnrollementDate;
                return View(s);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRegisterStudent(EditRegisterViewModel model)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    Student m = new Student();
                    int admin = Convert.ToInt32(model.Admin);
                    var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                    var per2 = db.Lookups.Where(a => a.Values == "Student" && a.Category == "ADMIN");
                    foreach (Lookup item in per2)
                    {
                        model.Admin = item.Id.ToString();
                    }
                    foreach (AspNetUser item in per)
                    {
                        db.AspNetUsers.Find(item.Id).Email = model.Email;
                        db.AspNetUsers.Find(item.Id).Admin = model.Admin;
                        db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                        db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                        db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                        db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                        db.Students.Find(item.Id).EnrollementDate = (DateTime)model.EnrollementDate;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Student");
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterTeacher()
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                return View();
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterTeacher(RegisterViewModel model)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        Teacher m = new Teacher();
                        int admin = Convert.ToInt32(model.Admin);
                        var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                        var per2 = db.Lookups.Where(a => a.Values == "Teacher" && a.Category == "ADMIN");
                        foreach (Lookup item in per2)
                        {
                            model.Admin = item.Id.ToString();
                        }
                        foreach (AspNetUser item in per)
                        {
                            db.AspNetUsers.Find(item.Id).Email = model.Email;
                            db.AspNetUsers.Find(item.Id).Admin = model.Admin;
                            db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                            db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                            db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                            db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                            db.AspNetUsers.Find(item.Id).EmailConfirmed = false;
                            db.AspNetUsers.Find(item.Id).PhoneNumberConfirmed = false;
                            m.AspNetUser = item;
                            m.Id = item.Id;
                            m.Salary = Convert.ToInt32(model.Salary);
                            db.Teachers.Add(m);
                            db.AspNetUsers.Find(item.Id).Teacher = m;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index", "Teacher");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditRegisterTeacher(string idd)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (idd == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                AspNetUser student = db.AspNetUsers.Find(idd);
                Teacher stu = db.Teachers.Find(idd);
                if (student == null || stu == null)
                {
                    return HttpNotFound();
                }
                EditRegisterViewModel s = new EditRegisterViewModel();
                s.UserName = student.UserName;
                s.Admin = student.Admin;
                s.Email = student.Email;
                s.FirstName = student.FirstName;
                s.LastName = student.LastName;
                s.Gender = student.Gender.ToString();
                s.PhoneNumber = student.PhoneNumber;
                s.Salary = stu.Salary.ToString();
                return View(s);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRegisterTeacher(EditRegisterViewModel model)
        {
            ViewBag.UserType = adtype();
            if (adtype() == "Admin")
            {
                if (ModelState.IsValid)
                {
                    Teacher m = new Teacher();
                    int admin = Convert.ToInt32(model.Admin);
                    var per = db.AspNetUsers.Where(a => a.UserName == model.UserName);
                    var per2 = db.Lookups.Where(a => a.Values == "Teacher" && a.Category == "ADMIN");
                    foreach (Lookup item in per2)
                    {
                        model.Admin = item.Id.ToString();
                    }
                    foreach (AspNetUser item in per)
                    {
                        db.AspNetUsers.Find(item.Id).Email = model.Email;
                        db.AspNetUsers.Find(item.Id).Admin = model.Admin;
                        db.AspNetUsers.Find(item.Id).PhoneNumber = model.PhoneNumber;
                        db.AspNetUsers.Find(item.Id).FirstName = model.FirstName;
                        db.AspNetUsers.Find(item.Id).LastName = model.LastName;
                        db.AspNetUsers.Find(item.Id).Gender = Convert.ToInt32(model.Gender);
                        db.Teachers.Find(item.Id).Salary = Convert.ToInt32(model.Salary);
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Teacher");
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ViewBag.UserType = adtype();
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.UserType = adtype();
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            ViewBag.UserType = adtype();
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            ViewBag.UserType = adtype();
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ViewBag.UserType = adtype();
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            ViewBag.UserType = adtype();
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            ViewBag.UserType = adtype();
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            ViewBag.UserType = adtype();
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpGet]
        public ActionResult LogOff2()
        {
            ViewBag.UserType = adtype();
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ViewBag.UserType = adtype();
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            ViewBag.UserType = adtype();
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            ViewBag.UserType = adtype();
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            ViewBag.UserType = adtype();
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}