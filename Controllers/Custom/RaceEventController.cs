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

    public class RaceEventController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: RaceEvent
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Index()
        {

            // it loads only race events for admin
            List<raceevent> events = GetRaceEventsForUser();
            return View(events.ToList());
        }

        //*****
        public List<raceevent> GetRaceEventsForUser()
        {
            int personID = GetPersonID();
            List<raceevent> events = new List<raceevent>();
            AdminController ac = new AdminController();

            // load race events for admin
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = ac.GetPersonOrganizationAdmin(personID);
                foreach (var item in orgs)
                {
                    var e = db.raceevents.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                    foreach (var ndx in e)
                    {
                        events.Add(ndx);
                    }
                }
            }

            // load race events for super user
            if (User.IsInRole("SuperUser"))
            {
                List<organization> orgs = db.organizations.ToList();
                foreach (var item in orgs)
                {
                    var e = db.raceevents.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                    foreach (var ndx in e)
                    {
                        events.Add(ndx);
                    }
                }
            }
            return events;
        }

        private int GetPersonID()
        {
            raceEntities db = new raceEntities();
            var user = User.Identity.GetUserId();
            var person = db.identitykeys.Where(i => i.IdentityGuid.Equals(user)).First();
            return person.PersonID;
        }

        // GET: RaceEvent/Details/5
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceevent raceevent = db.raceevents.Find(id);
            if (raceevent == null)
            {
                return HttpNotFound();
            }

            List<eventclass> classes = db.eventclasses.Where(c => c.RaceEventID == id)
                                                        .Include("raceevent")
                                                        .Include("buildqualificationmethod")
                                                        .Include("raceclass")
                                                        .Include("buildbracketmethod").ToList();
            ViewBag.ClassesForEvent = classes;

            // it may need to be set to null
            // pass the id to the view. It is used to select the race classes for a particular event
            Session["CurrentEventID"] = id;

            return View(raceevent);
        }

        [Authorize(Roles = "Admin,SuperUser")]
        // GET: RaceEvent/Create
        public ActionResult Create()
        {

            if (User.IsInRole("Admin"))
            {
                // load orgs for admin user
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                // loads all organizations for super user
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");
            }

            ViewBag.Country = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name");
            ViewBag.Currency = new SelectList(db.currencies, "CurrencyID", "Name");
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name");
            ViewBag.Regions = new SelectList(db.regions.OrderBy(o => o.Name), "RegionID", "Name");

            return View();
        }

        // POST: RaceEvent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create([Bind(Include = "RegionID,OrganizationID,CountryID,EventLevelID,CurrencyID,City,Name,Description,StartDate,EndDate,Year,PriorityRegistrationDate,OpenRegistrationDate,ClosedRegistrationDate,MaximumCompetitorClasses,PrimaryClassCost,SecondaryClassCost")] raceevent raceevent)
        {
            if (ModelState.IsValid)
            {
                // save new race
                try
                {
                    db.raceevents.Add(raceevent);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race event could not be created.";
                    return RedirectToAction("Create");
                }

                // propagate new race data to other tables

                // raceeventregion table
                raceeventregion raceRegion = new raceeventregion();
                raceRegion.RaceEventID = raceevent.RaceEventID;
                raceRegion.RegionID = raceevent.RegionID;
                try
                {
                    db.raceeventregions.Add(raceRegion);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race event could not be created.";
                    return RedirectToAction("Index");
                }

                // eventorganization
                eventorganization eventOrg = new eventorganization();
                eventOrg.Description = raceevent.Description;
                eventOrg.OrganizationID = raceevent.OrganizationID;
                eventOrg.RaceEventID = raceevent.RaceEventID;
                try
                {
                    db.eventorganizations.Add(eventOrg);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race event could not be created.";
                    return RedirectToAction("Index");
                }

                TempData["Sucess"] = "Race event created.";
                return RedirectToAction("Index");
            }

            ViewBag.Country = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name");
            ViewBag.Currency = new SelectList(db.currencies, "CurrencyID", "Name");
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name");
            ViewBag.Regions = new SelectList(db.regions.OrderBy(o => o.Name), "RegionID", "Name");

            // load orgs for admin user
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            if (User.IsInRole("SuperUser"))
            {
                // loads all organizations for super user
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            return View(raceevent);
        }


        // GET: RaceEvent/Edit/5
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceevent raceevent = db.raceevents.Find(id);
            if (raceevent == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name", raceevent.CountryID);
            ViewBag.Currency = new SelectList(db.currencies, "CurrencyID", "Name", raceevent.CurrencyID);
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name", raceevent.EventLevelID);
            ViewBag.Regions = new SelectList(db.regions.OrderBy(o => o.Name), "RegionID", "Name");


            // load orgs for admin user
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            if (User.IsInRole("SuperUser"))
            {
                // loads all organizations for super user
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            return View(raceevent);
        }

        // POST: RaceEvent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Edit([Bind(Include = "RaceEventID,OrganizationID,CountryID,EventLevelID,CurrencyID,RaceIndex,City,Name,Description,StartDate,EndDate,Year,PriorityRegistrationDate,OpenRegistrationDate,ClosedRegistrationDate,MaximumCompetitorClasses,PrimaryClassCost,SecondaryClassCost,Unum,UnumSync,UnumTime")] raceevent raceevent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    raceevent.Unum = raceevent.Unum + 1;
                    db.Entry(raceevent).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race event could not be edited.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.Country = new SelectList(db.countries.OrderBy(x => x.Name), "CountryID", "Name", raceevent.CountryID);
            ViewBag.Currency = new SelectList(db.currencies, "CurrencyID", "Name", raceevent.CurrencyID);
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name", raceevent.EventLevelID);
            ViewBag.Regions = new SelectList(db.regions.OrderBy(o => o.Name), "RegionID", "Name");

            // load orgs for admin user
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name", raceevent.OrganizationID);
            }
            if (User.IsInRole("SuperUser"))
            {
                // loads all organizations for super user
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name", raceevent.OrganizationID);
            }

            return View(raceevent);
        }

        // GET: RaceEvent/Delete/5
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceevent raceevent = db.raceevents.Find(id);
            if (raceevent == null)
            {
                return HttpNotFound();
            }
            return View(raceevent);
        }

        // POST: RaceEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult DeleteConfirmed(int id)
        {
            raceevent raceevent = db.raceevents.Find(id);

            try
            {
                var raceRegionEvent = db.raceeventregions.Where(r => r.RaceEventID == id).FirstOrDefault();
                db.raceeventregions.Remove(raceRegionEvent);

                var eventOrg = db.eventorganizations.Where(e => e.RaceEventID == id).FirstOrDefault();
                db.eventorganizations.Remove(eventOrg);

                db.raceevents.Remove(raceevent);

                db.SaveChanges();
            }
            catch (Exception)
            {

                TempData["ErrorMsg"] = "Race event could not be deleted.";
                return RedirectToAction("Delete", "RaceEvent", new { id = raceevent.RaceEventID });
            }
            TempData["Sucess"] = "Race deleted";
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
    }
}