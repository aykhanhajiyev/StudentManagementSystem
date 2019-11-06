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
    [Authorize(Roles = "Teacher")]
    public class adminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private DataContext db = new DataContext();
        public adminController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
        }
        // GET: admin
        public ActionResult index()
        {
            return View(userManager.Users.ToList());
        }

        [HttpGet]
        public ActionResult create()
        {
            ViewBag.GroupNames = db.Groups.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create(NewStudent model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.Username;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Group = model.Group;
                user.Group1 = model.Group1;
                user.Sinif = model.Sinif;
                user.PhoneNumber = model.PhoneNumber;
                user.Parent = model.Parent;
                var result = userManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult edit(string id)
        {
            var student = userManager.FindById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.GroupNames = db.Groups.ToList();
                return View(student);
            }
        }

        [HttpPost]
        public ActionResult edit(ApplicationUser model, string id)
        {
            if (ModelState.IsValid)
            {
                var student = userManager.FindById(id);
                var examuserresult = db.ExamUserResults.Where(i => i.Ad == student.Name && i.Soyad == student.Surname).ToList();
                var attendances = db.Attendances.Where(i => i.Ad == student.Name && i.Soyad == student.Surname).ToList();
                if (student != null)
                {
                    student.Name = model.Name;
                    student.UserName = model.UserName;
                    student.Surname = model.Surname;
                    student.Group = model.Group;
                    student.Group1 = model.Group1;
                    student.Sinif = model.Sinif;
                    student.Parent = model.Parent;
                    student.PhoneNumber = model.PhoneNumber;
                    userManager.Update(student);
                    foreach (var item in examuserresult)
                    {
                        item.Ad = model.Name;
                        item.Soyad = model.Surname;
                        item.QrupAdi = model.Group;
                        item.Sinif = model.Sinif;
                        db.SaveChanges();
                    }
                    foreach (var item in attendances)
                    {
                        item.Ad = model.Name;
                        item.Soyad = model.Surname;
                        item.GroupName = model.Group;
                        db.SaveChanges();
                    }
                    TempData["Success"] = "İstifadəçi məlumatları dəyişildi.";
                    return RedirectToAction("index");
                }
            }
            return View(model);
        }
        public ActionResult delete(string Id)
        {
            var student = userManager.FindById(Id);
            var examuserresult = db.ExamUserResults.Where(i => i.Ad == student.Name && i.Soyad == student.Surname).ToList();
            var attendances = db.Attendances.Where(i => i.Ad == student.Name && i.Soyad == student.Surname).ToList();
            var anouncements = db.Anouncements.Where(i => i.ShareWith == student.Id).ToList();
            if (student != null)
            {
                var result = userManager.Delete(student);
                if (result.Succeeded)
                {
                    foreach (var item in examuserresult)
                    {
                        db.ExamUserResults.Remove(item);
                    }
                    foreach (var item in attendances)
                    {
                        db.Attendances.Remove(item);
                    }
                    foreach (var item in anouncements)
                    {
                        db.Anouncements.Remove(item);
                    }
                    db.SaveChanges();
                    TempData["SuccessDelete"] = "İstifadəçi silindi.";
                    return RedirectToAction("index");
                }

            }
            return HttpNotFound();
        }

        public ActionResult attendances()
        {
            ViewBag.Latest = (db.Attendances.OrderByDescending(i => i.Tarix).FirstOrDefault() == null) ? null : db.Attendances.OrderByDescending(i => i.Tarix).FirstOrDefault();
            ViewBag.GroupNames = db.Groups.ToList();
            return View(db.Attendances.ToList());
        }

        public PartialViewResult editattendances()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editattendances(Attendances model)
        {
            var attend = db.Attendances.Find(model.Id);
            if (attend != null)
            {
                attend.IsJoin = model.IsJoin;
                attend.Tarix = model.Tarix;
                db.SaveChanges();
                TempData["SuccessEdit"] = attend.Ad + " " + attend.Soyad;
                return RedirectToAction("attendances");
            }
            return HttpNotFound();
        }
        public ActionResult deleteattendance(int? id)
        {
            if (id != null)
            {
                var attend = db.Attendances.Find(id);
                if (attend != null)
                {
                    TempData["Successdelete"] = attend.Ad + " " + attend.Soyad;
                    db.Attendances.Remove(attend);
                    db.SaveChanges();
                    return RedirectToAction("attendances");
                }
            }
            return HttpNotFound();
        }
        public ActionResult exams()
        {
            ViewBag.GroupNames = db.Groups.ToList();
            return View(db.Exams.ToList());
        }
        [HttpGet]
        public ActionResult newexam()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newexam(Exam model)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(model);
                db.SaveChanges();
                return RedirectToAction("exams");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult addtoexam(int? id, string groupname)
        {
            if (id != null)
            {
                var exam = db.Exams.Find(id);
                if (exam != null)
                {
                    ViewBag.Exam = exam;
                    ViewBag.Groupname = groupname;
                    return View(userManager.Users.Where(i => i.Group == groupname || i.Group1 == groupname).ToList());
                }
            }
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addtoexam(string[] QrupAdi, string[] Ad, string[] Soyad, int[] DuzgunSualSayi, int ExamId, string[] Sinif, bool flag,DateTime Tarix)
        {
            if (QrupAdi.Length != 0)
            {
                var exam = db.Exams.Find(ExamId);
                for (int i = 0; i < QrupAdi.Length; i++)
                {
                    ExamUserResult examUserResult = new ExamUserResult();
                    //attendances
                    Attendances attendances = new Attendances();
                    attendances.GroupName = QrupAdi[i];
                    attendances.Ad = Ad[i];
                    attendances.Soyad = Soyad[i];
                    attendances.Tarix = Tarix; // ////
                    //end attendances
                    examUserResult.QrupAdi = QrupAdi[i];
                    examUserResult.Ad = Ad[i];
                    examUserResult.Soyad = Soyad[i];
                    examUserResult.Sinif = Sinif[i];
                    if (DuzgunSualSayi[i] == -1)
                    {
                        examUserResult.DuzgunSualSayi = -1;
                        examUserResult.Faiz = -1;
                        attendances.IsJoin = false;
                    }
                    else
                    {
                        examUserResult.DuzgunSualSayi = DuzgunSualSayi[i];
                        examUserResult.Faiz = Convert.ToDecimal((Convert.ToDecimal(DuzgunSualSayi[i]) / Convert.ToDecimal(exam.CemiSualSayi)) * 100);
                        attendances.IsJoin = true;
                    }
                    examUserResult.ImtahanAdi = exam.ImtahanAdi;
                    examUserResult.Kateqoriya = exam.Kateqoriya;
                    examUserResult.Tarix = Tarix; // examtarix
                    examUserResult.CemiSualSayi = exam.CemiSualSayi;
                    if (flag == true)
                    {
                        db.Attendances.Add(attendances);
                    }
                    db.ExamUserResults.Add(examUserResult);
                    db.SaveChanges();
                }
                return RedirectToAction("exams");
            }
            return View();
        }
        public ActionResult examresult(string groupname)
        {
            ViewBag.GroupName = groupname;
            return View(db.ExamUserResults.Where(i => i.QrupAdi == groupname).ToList());
        }

        public ActionResult deleteexam(int? id)
        {
            if (id != null)
            {
                var exam = db.Exams.Find(id);
                if (exam != null)
                {
                    db.Exams.Remove(exam);
                    db.SaveChanges();
                    TempData["successdelete"] = "İmtahan silindi.";
                    return RedirectToAction("exams");
                }
            }
            return HttpNotFound();
        }
        [HttpGet]
        public ActionResult editexamresult(int? id)
        {
            if (id != null)
            {
                var examresult = db.ExamUserResults.Find(id);
                if (examresult != null)
                {
                    return View(examresult);
                }

            }
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editexamresult(ExamUserResult model)
        {
            if (ModelState.IsValid)
            {
                var examresult = db.ExamUserResults.Find(model.Id);
                if (examresult != null)
                {
                    if (model.DuzgunSualSayi == -1)
                    {
                        examresult.DuzgunSualSayi = model.DuzgunSualSayi;
                        examresult.Faiz = -1;
                    }
                    else
                    {
                        examresult.DuzgunSualSayi = model.DuzgunSualSayi;
                        examresult.Faiz = Convert.ToDecimal(model.DuzgunSualSayi) / Convert.ToDecimal(examresult.CemiSualSayi) * 100;
                    }
                    db.SaveChanges();
                    TempData["successedit"] = "Düzəliş edildi";
                    return Redirect($"/admin/examresult?groupname={examresult.QrupAdi}");
                }
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult resetpassword(string id, string newpassword)
        {
            if (id != null)
            {
                var user = userManager.FindById(id);
                if (user != null)
                {
                    userManager.RemovePassword(user.Id);
                    userManager.AddPassword(user.Id, newpassword);
                    TempData["successreset"] = user.Name + " " + user.Surname;
                    return RedirectToAction("index");
                }
            }
            return HttpNotFound();
        }

        public ActionResult anouncements()
        {
            ViewBag.Users = userManager.Users;
            return View(db.Anouncements.OrderByDescending(i => i.AnounceDate).ToList());
        }

        public PartialViewResult modal()
        {
            var users = userManager.Users;
            ViewBag.Users = users;
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newanouncement(Anouncements model)
        {
            if (ModelState.IsValid)
            {
                db.Anouncements.Add(model);
                db.SaveChanges();
                TempData["Success"] = "Elan əlavə edildi.";
                return RedirectToAction("anouncements");
            }
            return RedirectToAction("anouncements");
        }

        public ActionResult deleteanouncement(int? id)
        {
            if (id != null)
            {
                var anouncement = db.Anouncements.Find(id);
                if (anouncement != null)
                {
                    db.Anouncements.Remove(anouncement);
                    db.SaveChanges();
                    return RedirectToAction("anouncements");
                }
            }
            return HttpNotFound();
        }

        public PartialViewResult editanouncement()
        {
            var users = userManager.Users;
            ViewBag.Users = users;
            return PartialView();
        }
        [HttpPost]
        public ActionResult editanouncement(Anouncements model)
        {
            if (ModelState.IsValid)
            {
                var mymodel = db.Anouncements.Find(model.Id);
                mymodel.AnounceName = model.AnounceName;
                mymodel.AnounceDescription = model.AnounceDescription;
                mymodel.AnounceDate = model.AnounceDate;
                mymodel.ShareWith = model.ShareWith;
                db.SaveChanges();
                TempData["SuccessEdit"] = "Düzəliş edildi.";
                return RedirectToAction("anouncements");
            }
            return HttpNotFound();
        }

        public ActionResult progress(string id)
        {
            if (id != null)
            {
                var user = userManager.FindById(id);
                ViewBag.UserId = user.Id;
                ViewBag.UserAdSoyad = user.Name + ' ' + user.Surname;
                return View();
            }
            return HttpNotFound();
        }
        public JsonResult getexamresults(string id)
        {
            var user = userManager.FindById(id);
            var results = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əsas" && i.DuzgunSualSayi != -1).OrderBy(i => i.Tarix).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getexamresults1(string id)
        {
            var user = userManager.FindById(id);
            var results = db.ExamUserResults.Where(i => i.Ad == user.Name && i.Soyad == user.Surname && i.Kateqoriya == "Əlavə" && i.DuzgunSualSayi != -1).OrderBy(i => i.Tarix).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}