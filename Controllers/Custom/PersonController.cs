using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IDFWebApp.Models.Custom;
using Microsoft.AspNet.Identity;

namespace IDFWebApp.Controllers.Custom
{
    public class PersonController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: Person
        public ActionResult Index(string sortOrder)
        {
            // Sorting each column
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewBag.FirstNameSortParm = sortOrder == "first_asc" ? "first_desc" : "first_asc";
            ViewBag.LastNameSortParm = sortOrder == "last_asc" ? "last_desc" : "last_asc";
            ViewBag.NicknameSortParm = sortOrder == "nick_asc" ? "nick_desc" : "nick_asc";
            ViewBag.BirthDateSortParm = sortOrder == "dob_asc" ? "dob_desc" : "dob_asc";
            ViewBag.GenderSortParm = sortOrder == "gender_asc" ? "gender_desc" : "gender_asc";
            ViewBag.CountryCodeSortParm = sortOrder == "code_asc" ? "code_desc" : "code_asc";

            var people = from p in db.people select p;

            switch (sortOrder)
            {
                case "first_asc":
                    people = people.OrderBy(p => p.FirstName);
                    break;
                case "first_desc":
                    people = people.OrderByDescending(p => p.FirstName);
                    break;
                case "last_asc":
                    people = people.OrderBy(p => p.LastName);
                    break;
                case "last_desc":
                    people = people.OrderByDescending(p => p.LastName);
                    break;
                case "nick_asc":
                    people = people.OrderBy(p => p.NickName);
                    break;
                case "nick_desc":
                    people = people.OrderByDescending(p => p.NickName);
                    break;
                case "dob_asc":
                    people = people.OrderBy(p => p.BirthDate);
                    break;
                case "dob_desc":
                    people = people.OrderByDescending(p => p.BirthDate);
                    break;
                case "gender_asc":
                    people = people.OrderBy(p => p.Gender);
                    break;
                case "gender_desc":
                    people = people.OrderByDescending(p => p.Gender);
                    break;
                case "code_asc":
                    people = people.OrderBy(p => p.country.Code);
                    break;
                case "code_desc":
                    people = people.OrderByDescending(p => p.country.Code);
                    break;
                case "email_asc":
                    people = people.OrderBy(p => p.Email);
                    break;
                case "email_desc":
                    people = people.OrderByDescending(p => p.Email);
                    break;
                default:
                    people = people.OrderByDescending(p => p.PersonID);
                    break;
            }
            return View(people.ToList());

            // Default code here
            //var people = db.people.Include(p => p.country);
            //return View(people.ToList());
        }

        // GET: Person/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            ViewBag.Nationality = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name");

            return View();
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,IsDeleted,SuperUser,Email,Password,FirstName,LastName,NickName,NationalityID,BirthDate,Gender")] person person)
        {
            if (ModelState.IsValid)
            {
                person.Password = "";
                person.UnumSync = -1;

                try {
                    db.people.Add(person);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Person already exists in the database.";
                    return RedirectToAction("Create");
                }

                // Get current ID of the user and insert it into identitykey
                var user = User.Identity.GetUserId();
                var key = new identitykey();

                person newUser = db.people.Find(person.PersonID);
                key.person = newUser;
                key.IdentityGuid = user;
                key.PersonID = person.PersonID;
                key.ProviderName = "";
                key.Unum = 0;
                key.UnumSync = -1;
                key.UnumTime = DateTime.Now;
                
                try
                {
                    db.identitykeys.Add(key);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Person already exists in the database.";
                    return RedirectToAction("Create");
                }
                
                // on sucess go to main page
                TempData["Sucess"] = "User created";
                return RedirectToAction("Details", "Person", new { id = person.PersonID});
            }

            ViewBag.Nationality = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name", person.NationalityID);
            return View(person);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.NationalityID = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name", person.NationalityID);
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,IsDeleted,SuperUser,Email,Password,FirstName,LastName,NickName,NationalityID,BirthDate,Gender,Unum,UnumSync,UnumTime")] person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Could not update.";
                    return RedirectToAction("Edit");
                    throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.NationalityID = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name", person.NationalityID);
            return View(person);
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            person person = db.people.Find(id);
            try
            {
                db.people.Remove(person);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "Could not delete.";
                return RedirectToAction("Delete");
                throw;
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [Authorize(Roles="Admin,SuperUser")]
        public ActionResult Find(String query, int CountryID = 0)
        {
            ViewBag.CountryID = new SelectList(db.countries.OrderBy(o=>o.Name), "CountryID", "Name");

            // NEEDS VALIDATION HERE AGAINST SQL INJECTION
            List<organization> adminOrgs = null;
            List<person> peopleManagedByAdmin = null;

            if (User.IsInRole("Admin"))
            {
                adminOrgs = (List<organization>)Session["AdminOrgs"];
                List<int> orgsIDs = new List<int>();

                peopleManagedByAdmin = new List<person>();
                //int numberOfOrgs = adminOrgs.Count();

                foreach (var item in adminOrgs)
                {
                    List<person> allUsers = (from p in db.people
                                             join r in db.personorganizationroles on p.PersonID equals r.PersonID
                                             join o in db.organizationroletypes on r.OrganizationRoleTypeID equals o.OrganizationRoleTypeID
                                             where o.OrganizationID == item.OrganizationID
                                             select p).ToList();

                    foreach (var p in allUsers)
                    {
                        peopleManagedByAdmin.Add(p);
                    }
                }        
            }
            if (User.IsInRole("SuperUser"))
            {
                adminOrgs = db.organizations.ToList();
                peopleManagedByAdmin = db.people.ToList();
            }
              

            // if query parameter is not null
            if ((query != null) && (CountryID == 0))
            {
                var usersByName = peopleManagedByAdmin.Where(q => q.Email.Contains(query) || q.FirstName.Contains(query) || q.LastName.Contains(query)).ToList();

                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Person/PartialView/_PersonPartialTable.cshtml", usersByName);
                }
            }
       
            
            // if query and country
            if ((query != null) && (CountryID > 0))
            {
                var usersByName = from p in peopleManagedByAdmin
                                            where p.Email.Contains(query) || p.FirstName.Contains(query) || p.LastName.Contains(query)
                                            select p;

                var filterByCountry = from u in usersByName where u.NationalityID == CountryID select u;

                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Person/PartialView/_PersonPartialTable.cshtml", filterByCountry);
                }
            }
            
            // if only country
            if ((query == null) && (CountryID > 0))
            {
                var filterByCountry = from u in peopleManagedByAdmin where u.NationalityID == CountryID select u;

                if (Request.IsAjaxRequest())
                {
                    return PartialView("~/Views/Person/PartialView/_PersonPartialTable.cshtml", filterByCountry);
                }
            }                           
            
            // default query: get's all users based on email, first name, or last name
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Person/PartialView/_PersonPartialTable.cshtml", peopleManagedByAdmin);
            }
            return View("Find", peopleManagedByAdmin);
        }
    }
}
