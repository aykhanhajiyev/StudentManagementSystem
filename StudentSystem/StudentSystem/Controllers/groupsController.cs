using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentSystem.Identity;
using StudentSystem.Models;

namespace StudentSystem.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class groupsController : Controller
    {
        private DataContext db = new DataContext();
        private UserManager<ApplicationUser> userManager;
        public groupsController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
        }

        // GET: groups
        public ActionResult index()
        {
            return View(db.Groups.ToList());
        }

        // GET: groups/Create
        public ActionResult create()
        {
            return View();
        }

        // POST: groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create([Bind(Include = "Id,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            var usersgroup = userManager.Users.Where(i => i.Group == group.GroupName).ToList();
            var usersgroup1 = userManager.Users.Where(i => i.Group1 == group.GroupName).ToList();
            if (group != null)
            {
                if (usersgroup != null)
                {
                    foreach (var item in usersgroup)
                    {
                        item.Group = "";
                        userManager.Update(item);
                    }
                }
                if (usersgroup1 != null)
                {
                    foreach (var item in usersgroup1)
                    {
                        item.Group1 = "";
                        userManager.Update(item);
                    }
                }

                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["SuccessDelete"] = "Qrup silindi";
                return RedirectToAction("index");
            }
            return HttpNotFound();
        }
        [HttpGet]
        public ActionResult edit(int? id)
        {
            if (id != null)
            {
                return View(db.Groups.Find(id));
            }
            return HttpNotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit(Group group)
        {
            if (ModelState.IsValid)
            {
                var groupdb = db.Groups.Find(group.Id);
                var usersgroup = userManager.Users.Where(i => i.Group == groupdb.GroupName).ToList();
                var usersgroup1 = userManager.Users.Where(i => i.Group1 == groupdb.GroupName).ToList();
                if (usersgroup != null)
                {
                    foreach (var item in usersgroup)
                    {
                        item.Group = group.GroupName;
                        userManager.Update(item);
                    }
                }
                if (usersgroup1 != null)
                {
                    foreach (var item in usersgroup1)
                    {
                        item.Group1 = group.GroupName;
                        userManager.Update(item);
                    }
                }
                groupdb.GroupName = group.GroupName;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(group);
        }

    }
}
