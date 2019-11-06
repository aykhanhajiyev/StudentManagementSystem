using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentSystem.Identity;
using StudentSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class roleadminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManagar;
        public roleadminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));
            userManagar = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
        }
        // GET: roleadmin
        public ActionResult index()
        {
            return View(roleManager.Roles);
        }
        [HttpGet]
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name));
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
            return View(name);
        }

        public ActionResult delete(string id)
        {
            var role = roleManager.FindById(id);
            if (role != null)
            {
                var result = roleManager.Delete(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
                else
                {
                    return View("error", result.Errors);
                }
            }
            else
            {
                return View("error", new string[] { "Rol can not be founded!" });
            }
        }
        [HttpGet]
        public ActionResult edit(string id)
        {
            var role = roleManager.FindById(id);
            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();
            foreach(var user in userManagar.Users.ToList())
            {
                var list = userManagar.IsInRole(user.Id, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            return View(new RoleEditModel() {
                Role=role,
                Members=members,
                NonMembers=nonmembers
            });
        }
        [HttpPost]
        public ActionResult edit(RoleUpdateModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach(var item in model.IdsToAdd?? new string[] { })
                {
                    result = userManagar.AddToRole(item, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("error", result.Errors);
                    }
                }
                foreach(var user in model.IdsToDelete?? new string[] { })
                {
                    result = userManagar.RemoveFromRole(user, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("error", result.Errors);
                    }
                }
                return RedirectToAction("index");
            }
            return View("error",new string[] {"Something went wrong!" });
        }
    }
}