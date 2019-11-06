using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentSystem.Identity;
using StudentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentSystem.Controllers
{
    [Authorize]
    public class homeController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private DataContext db = new DataContext();

        public homeController()
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
        // GET: home
        public ActionResult index()
        {
            if (User.Identity.IsAuthenticated)
            {
                CallAnouncementsCount();
                var user = userManager.FindByName(User.Identity.Name);
                var attends = (db.Attendances.Where(i => i.Ad == user.Name && i.Soyad == user.Surname).ToList() == null) ? null : db.Attendances.Where(i => i.Ad == user.Name && i.Soyad == user.Surname).ToList();
                var result = db.Attendances.Where(i => i.Ad == user.Name).OrderByDescending(i => i.Tarix).FirstOrDefault();
                ViewBag.UpdateDate = result;
                return View(attends.OrderByDescending(i => i.Tarix));
            }
            else { return Redirect("account/login"); }
        }

        public ActionResult exams()
        {
            CallAnouncementsCount();
            var user = userManager.FindByName(User.Identity.Name);
            var examresult = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname).OrderByDescending(i => i.Tarix).ToList();
            var updatedate = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname).OrderByDescending(i => i.Tarix).FirstOrDefault();
            if (updatedate != null && db.ExamUserResults.Where(i => i.Tarix == updatedate.Tarix && i.Kateqoriya == "Əsas" && i.Sinif == user.Sinif).Count() != 0)
            {
                var highscore = db.ExamUserResults.Where(i => i.Tarix == updatedate.Tarix && i.Kateqoriya == "Əsas" && i.Sinif == user.Sinif).Max(i => i.Faiz);
                var firstdegree = db.ExamUserResults.Where(i => i.Tarix == updatedate.Tarix && i.Faiz == highscore && i.Kateqoriya == "Əsas" && i.Sinif == user.Sinif).ToList();
                ViewBag.HighScores = firstdegree;

            }
            ViewBag.UpdateDate = updatedate;
            return View(examresult);
        }

        public ActionResult progress()
        {
            CallAnouncementsCount();
            var user = userManager.FindByName(User.Identity.Name);
            var results = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əsas").OrderByDescending(i => i.Tarix).FirstOrDefault();
            var results1 = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əlavə").OrderByDescending(i => i.Tarix).FirstOrDefault();
            ViewBag.UpdateDate = results;
            ViewBag.UpdateDateElave = results1;
            return View();
        }

        public JsonResult getexamresults()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var results = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əsas" && i.DuzgunSualSayi != -1).OrderBy(i => i.Tarix).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getexamresults1()
        {
            var user = userManager.FindByName(User.Identity.Name);
            var results = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əlavə" && i.DuzgunSualSayi != -1).OrderBy(i => i.Tarix).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult anouncements()
        {
            CallAnouncementsCount();
            var user = userManager.FindByName(User.Identity.Name);
            return View(db.Anouncements.Where(i => i.ShareWith == user.Id || i.ShareWith == "Everyone").OrderByDescending(i => i.AnounceDate).ToList());
        }
        public ActionResult error()
        {
            return View();
        }
    }
}