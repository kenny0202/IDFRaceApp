
using IDFWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDFWebApp.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class UserController : Controller
    {
        // GET: ManageUser
        public ActionResult Index()
        {
            List<string> enabledUsers;
            List<string> disabledUsers;

            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                disabledUsers = (from u in userManager.Users
                                 where u.LockoutEndDateUtc > DateTime.Now
                                 select u.UserName).ToList();
            }

            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                enabledUsers = (from u in userManager.Users
                                where u.LockoutEndDateUtc < DateTime.Now || u.LockoutEndDateUtc == null
                                select u.UserName).ToList();
            }

            ViewBag.Lock = disabledUsers;
            ViewBag.Unlock = enabledUsers;

            return View();
        }

        public ActionResult Disable(string userName)
        {
            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(userName);

                user.LockoutEnabled = true;

                DateTime dt = new DateTime(5000, 01, 01);
                DateTimeOffset dto = new DateTimeOffset(dt);
                userManager.SetLockoutEndDate(user.Id, dto);

                db.SaveChanges();
            }

            ViewBag.ResultMessage = "Successfully disabled user.";

            return RedirectToAction("Index", "User");
        }

        public ActionResult Enable(string userName)
        {
            using (var db = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(userName);

                user.LockoutEnabled = true;

                DateTime dt = new DateTime(2000, 01, 01);
                DateTimeOffset dto = new DateTimeOffset(dt);
                userManager.SetLockoutEndDate(user.Id, dto);

                db.SaveChanges();
            }

            ViewBag.ResultMessage = "Successfully enabled user.";

            return RedirectToAction("Index", "User");
        }

    }
}