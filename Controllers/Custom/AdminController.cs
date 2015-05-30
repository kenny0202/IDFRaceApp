using IDFWebApp.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using IDFWebApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;


namespace IDFWebApp.Controllers.Custom
{
    public class AdminController : Controller
    {

        private static ApplicationDbContext context = new ApplicationDbContext();
        private static RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
        private static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);
        private static UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

        // GET: Admin
        public ActionResult Index()
        {
            int id = Convert.ToInt32(Session["PersonID"]);

            return View();
        }

        private int GetPersonID()
        {
            raceEntities db = new raceEntities();
            var user = User.Identity.GetUserId();
            var person = db.identitykeys.Where(i => i.IdentityGuid.Equals(user)).First();
            return person.PersonID;
        }

        public preference getUserPreference(int id)
        {
            raceEntities db = new raceEntities();

            // makes the user's preferences available to the view
            Dictionary<String, String> preferences = new Dictionary<String, String>();

            // get preference object
            preference pref = db.preferences.Where(pid => pid.PersonID == id).FirstOrDefault();
            if (pref == null)
            {
                return null;
            }

            // split the parameters to key value pairs
            string[] values = pref.Parameters.Split(new Char[] { ' ', '\r', '\t', '\n' });

            // add the key value pairs to the dictionary
            for (int i = 0; i < values.Length; i++)
            {
                int prefLenght = values[i].Length;
                int ndx = values[i].IndexOf('=');
                string key = values[i].Substring(0, ndx);
                string value = values[i].Substring(ndx + 1);
                preferences.Add(key, value);
            }

            return pref;
        }

        // return a list of organizations that the current admin is reponsible for
        public List<organization> GetPersonOrganizationAdmin(int id)
        {
            raceEntities db = new raceEntities();

            // get all roles of a user in organizations
            var orgRoleTypes = db.personorganizationroles.Where(p => p.PersonID == id).ToList();

            // get only organizations where user is admin
            var filterByAdmin = (from o in orgRoleTypes join t in db.organizationroletypes on o.OrganizationRoleTypeID equals t.OrganizationRoleTypeID where t.Code.Equals("ADMIN") select o).ToList();

            // holds the ids of the orgs
            List<int> orgs = new List<int>();
            foreach (var item in filterByAdmin)
            {
                // get the id of the orgs 
                int orgID = db.organizationroletypes.Where(t => t.OrganizationRoleTypeID == item.OrganizationRoleTypeID).Select(o => o.OrganizationID).First();
                orgs.Add(orgID);
            }

            // load the orgs for admin
            List<organization> orgsList = new List<organization>();
            for (int i = 0; i < orgs.Count; i++)
            {
                var org = db.organizations.Find(orgs.ElementAt(i));
                orgsList.Add(org);
            }
            return orgsList;
        }

        // returns email of admin. Use to check if user is admin
        public string IsAdminUser(int personID)
        {
            raceEntities db = new raceEntities();

            // get all roles of a user in organizations
            var orgRoleTypes = db.personorganizationroles.Where(p => p.PersonID == personID).ToList();

            if (orgRoleTypes.Count() == 0) return null; // user has no roles

            var filterByAdmin = (from o in orgRoleTypes join t in db.organizationroletypes on o.OrganizationRoleTypeID equals t.OrganizationRoleTypeID where t.Code.Equals("ADMIN") select o).ToList();

            // user is admin
            if (filterByAdmin.Count() != 0)
            {
                // load user data
                var user = db.people.Where(p => p.PersonID == personID).FirstOrDefault();
                return user.Email;
            }
            // not admin
            return null;
        }

        // returns email of super user. Use to check if user is super user
        public string IsSuperUser(int personID)
        {
            raceEntities db = new raceEntities();
            var foundSuperUser = db.people.Where(p => p.PersonID == personID && p.SuperUser == true).FirstOrDefault();

            if (foundSuperUser == null)
            {
                return null;
            }
            return foundSuperUser.Email;
        }

        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult SetUserRole(int id, int OrganizationID = 0)
        {
            raceEntities db = new raceEntities();

            // passes the id of the selected user to the hidden field in the view
            Session["UserToAddRole"] = id;
            Session["Org"] = OrganizationID;

            if (User.IsInRole("Admin"))
            {
                // dropdown list with organizations that the admin belongs to
                List<organization> orgsForAdmin = (List<organization>)Session["AdminOrgs"];
                ViewBag.Organizations = new SelectList(orgsForAdmin, "OrganizationID", "Name");
            }

            if (User.IsInRole("SuperUser"))
            {
                // dropdown list with organizations that the admin belongs to
                ViewBag.Organizations = new SelectList(db.organizations.ToList(), "OrganizationID", "Name");
            }

            // after the admin chose the organization, load the right role types for that org
            if (Request.IsAjaxRequest())
            {
                List<organizationroletype> listOfRolesForEachOrg = db.organizationroletypes.Where(o => o.OrganizationID == OrganizationID).ToList();
                ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name"); // load the dropdown

                return PartialView("~/Views/Admin/PartialView/_SetUserRolePartial.cshtml");   // return the form
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult SetUserRole([Bind(Include = "OrganizationID,PersonID,OrganizationRoleTypeID,CompetitorLevel,MemberNumber,AnnualDuesPaid,AccountNumber,TransactionNumber,AutoRenewSubscription,PriorityStatus")] personorganizationrole personorganizationrole)
        {
            raceEntities db = new raceEntities();
            personorganizationrole.Unum = 0;
            personorganizationrole.UnumSync = -1;
            personorganizationrole.UnumTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                   
                    db.personorganizationroles.Add(personorganizationrole);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    int userID = (int)Session["UserToAddRole"];
                    TempData["ErrorMsg"] = "User has already the role. Please, chose another one.";
                    return RedirectToAction("SetUserRole", new { id = "" + userID });
                }

                TempData["Sucess"] = "Role added to user";
                return RedirectToAction("Details", "Person", new { id = personorganizationrole.PersonID });
            }

            // dropdowns code
            List<organizationroletype> listOfRolesForEachOrg = new List<organizationroletype>();
            List<organization> orgsForAdmin = null;

            if (User.IsInRole("Admin"))
            {
                orgsForAdmin = (List<organization>)Session["AdminOrgs"];
                ViewBag.Organizations = new SelectList(orgsForAdmin, "OrganizationID", "Name");
            }
            if (User.IsInRole("SuperUser"))
            {
                orgsForAdmin = db.organizations.ToList();
                ViewBag.Organizations = new SelectList(db.organizations.ToList(), "OrganizationID", "Name");
            }

            foreach (var item in orgsForAdmin)
            {
                var roleTypes = db.organizationroletypes.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                foreach (var i in roleTypes)
                {
                    listOfRolesForEachOrg.Add(i);
                }
            }
            ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name");

            return View(personorganizationrole);
        }


        [Authorize(Roles = "SuperUser")]
        public ActionResult SetSuperUser(int personID = 0)
        {
            raceEntities db = new raceEntities();
            try
            {
                var user = db.people.Find(personID);
                return View("ConfirmNewSuperUser", user);
            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "It was not possible to find user.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser")]
        public ActionResult SetSuperUser([Bind(Include = "PersonID")] person person)
        {
            raceEntities db = new raceEntities();
            try
            {
                var user = db.people.Find(person.PersonID);
                user.SuperUser = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Person", new { id = person.PersonID });
            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "It was not possible set user as super user.";
                return RedirectToAction("Details", "Person", new { id = person.PersonID });
            }
        }

        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult RemoveUserRole(int id, int OrganizationID = 0)
        {
            raceEntities db = new raceEntities();

            // passes the id of the selected user to the hidden field in the view
            Session["UserToAddRole"] = id;

            // dropdown list with organizations that the admin belongs to
            if (User.IsInRole("Admin"))
            {
                // dropdown list with organizations that the admin belongs to
                List<organization> orgsForAdmin = (List<organization>)Session["AdminOrgs"];
                ViewBag.Organizations = new SelectList(orgsForAdmin, "OrganizationID", "Name");
            }

            if (User.IsInRole("SuperUser"))
            {
                // dropdown list with organizations that the admin belongs to
                ViewBag.Organizations = new SelectList(db.organizations.ToList(), "OrganizationID", "Name");
            }


            // after the admin chose the organization, load the right role types for that org
            if (Request.IsAjaxRequest())
            {
                List<organizationroletype> listOfRolesForEachOrg = db.organizationroletypes.Where(o => o.OrganizationID == OrganizationID).ToList();

                List<personorganizationrole> rolesForUser = GetUserRoles(id, OrganizationID);
                ViewBag.RolesForUser = new List<personorganizationrole>(rolesForUser);

                // load the dropdown
                ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name");

                // return the form
                return PartialView("~/Views/Admin/PartialView/_RemoveUserRolePartial.cshtml");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult RemoveUserRole([Bind(Include = "OrganizationID,PersonID,OrganizationRoleTypeID")] personorganizationrole personorganizationrole)
        {
            raceEntities db = new raceEntities();

            if (ModelState.IsValid)
            {
                try
                {
                    var personRole = db.personorganizationroles.Where(p => p.PersonID == personorganizationrole.PersonID && p.OrganizationRoleTypeID == personorganizationrole.OrganizationRoleTypeID).FirstOrDefault();
                    db.personorganizationroles.Remove(personRole);
                    db.SaveChanges();

                    if (personorganizationrole.OrganizationRoleTypeID == 10)
                    {
                        var userID = db.identitykeys.Where(p => p.PersonID == personorganizationrole.PersonID).Select(i => i.IdentityGuid).FirstOrDefault();
                        userManager.RemoveFromRole(userID, "Admin");
                    }
                }
                catch (Exception)
                {
                    int userID = (int)Session["UserToAddRole"];
                    TempData["ErrorMsg"] = "User does not have such role. Please, chose another one.";
                    return RedirectToAction("RemoveUserRole", new { id = userID });
                }
                TempData["Sucess"] = "Role removed from user";
                return RedirectToAction("Details", "Person", new { id = personorganizationrole.PersonID });
            }

            // dropdowns
            List<organizationroletype> listOfRolesForEachOrg = new List<organizationroletype>();
            List<organization> orgsForAdmin = (List<organization>)Session["AdminOrgs"];
            foreach (var item in orgsForAdmin)
            {
                var roleTypes = db.organizationroletypes.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                foreach (var i in roleTypes)
                {
                    listOfRolesForEachOrg.Add(i);
                }
            }

            ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name");
            ViewBag.Organizations = new SelectList(orgsForAdmin, "OrganizationID", "Name");
            ViewBag.RolesForUser = null;

            return View(personorganizationrole);
        }

        // returns a list of roles for a user given an org id
        private List<personorganizationrole> GetUserRoles(int personID, int orgID)
        {
            raceEntities db = new raceEntities();
            var listOfOrRoleType = db.organizationroletypes;
            List<personorganizationrole> rolesList = db.personorganizationroles.Where(p => p.PersonID == personID).Include(r => r.organizationroletype).ToList();
            List<personorganizationrole> userRolesForOrg = (from r in rolesList join t in listOfOrRoleType on r.OrganizationRoleTypeID equals t.OrganizationRoleTypeID where t.OrganizationID == orgID select r).ToList();

            return userRolesForOrg;
        }


        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult AdminCreatePerson()
        {
            raceEntities db = new raceEntities();
            ViewBag.Nationality = new SelectList(db.countries, "CountryID", "Name");
            return View();
        }

        // POST: AdminPerson/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult AdminCreatePerson([Bind(Include = "PersonID,IsDeleted,SuperUser,Email,Password,FirstName,LastName,NickName,NationalityID,BirthDate,Gender")] person person)
        {
            raceEntities db = new raceEntities();
            ApplicationDbContext ctx = new ApplicationDbContext();
            if (ModelState.IsValid)
            {
                try
                {
                    db.people.Add(person);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "It was not possible to create new user. Chose other email.";
                    return View();
                }

                // create user in the indentity db
                var user = new ApplicationUser { UserName = person.Email, Email = person.Email };
                try
                {
                    userManager.Create(user, person.Password);
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "It was not possible to create new user. Chose other email.";
                    // roll back creation of user. Avoids to have one user without credentials to access the system.
                    var removeUser = db.people.Find(person);
                    db.people.Remove(removeUser);
                    db.SaveChanges();
                    return View();
                }

                // Get current ID of the user and insert it into identitykey table
                var newUser = userManager.FindByEmail(person.Email);

                identitykey key = new identitykey();
                key.IdentityGuid = newUser.Id;
                key.PersonID = person.PersonID;
                key.ProviderName = "";
                key.UnumSync = -1;

                try
                {
                    db.identitykeys.Add(key);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "It was not possible to create new user. Chose other email.";

                    // roll back creation of user. Avoids to have one user without credentials to access the system.
                    var removeUser = db.people.Find(person);
                    db.people.Remove(removeUser);
                    db.SaveChanges();

                    // remove identiy credentials
                    var removeIdentityUser = userManager.FindByEmail(person.Email);
                    userManager.Delete(removeIdentityUser);

                    return View();
                }

                TempData["Sucess"] = "User created";
                return RedirectToAction("Details", "Person", new { id = person.PersonID });
            }
            ViewBag.Nationality = new SelectList(db.countries, "CountryID", "Code");
            return View(person);
        }
    }
}