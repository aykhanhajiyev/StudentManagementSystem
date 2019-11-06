using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StudentSystem.Identity;
using StudentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentSystem.Controllers
{
    
    public class accountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private DataContext db = new DataContext();
        public accountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
        }
        [NonAction]
        private void CallAnouncementsCount()
        {
            var userid = userManager.FindByName(User.Identity.Name).Id;
            var list = db.Anouncements.Where(i => i.ShareWith == userid || i.ShareWith == "Everyone").ToList();
            var count = list.Count();
            ViewBag.CountAnouncements = count;
        }
        // GET: account
        [HttpGet]
        public ActionResult login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/home/index");
            }
            ViewBag.returnUrl = returnUrl;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "İstifadəçi adı və yaxud şifrə yanlışdır.");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    };
                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("login");
        }
        [Authorize]
        [HttpGet]
        public ActionResult settings()
        {
            CallAnouncementsCount();
            return View(userManager.Users.Where(i=>i.UserName==User.Identity.Name).FirstOrDefault());
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult settings(ApplicationUser model)
        {
            var user = userManager.Users.Where(i => i.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.Parent = model.Parent;
                user.PhoneNumber = model.PhoneNumber;
                userManager.Update(user);
                TempData["Success"] = "Məlumatlar dəyişildi.";
                return RedirectToAction("settings");
            }
            return HttpNotFound();
        }
    }
}