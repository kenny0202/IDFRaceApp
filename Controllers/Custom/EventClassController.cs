using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IDFWebApp.Models.Custom;

namespace IDFWebApp.Controllers.Custom
{
 
    [Authorize(Roles="Admin,SuperUser")]
    public class EventClassController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: EventClass
        public ActionResult Index()
        {
            var eventclasses = db.eventclasses.Include(e => e.buildbracketmethod).Include(e => e.buildqualificationmethod).Include(e => e.raceclass).Include(e => e.raceevent);
            return View(eventclasses.ToList());
        }

        // GET: EventClass/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventclass eventclass = db.eventclasses.Find(id);
            if (eventclass == null)
            {
                return HttpNotFound();
            }
            return View(eventclass);
        }

        // GET: EventClass/Create
        public ActionResult Create()
        {

            // get event id to select race classes for dropdown 
            // it comes from the RaceEvent Details method
            int eventID = Convert.ToInt32(Session["CurrentEventID"]);
            int orgID = GetOrganizationIDForEvent(eventID);

            ViewBag.BuildBracketMethod = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Name");
            ViewBag.BuildQualificationMethod = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Name");
            ViewBag.RaceClass = new SelectList(GetRaceClassesForOrganization(orgID), "RaceClassID", "Name");

            return View();
        }


        // POST: EventClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventClassID,RaceEventID,RaceClassID,BuildQualificationMethodID,BuildBracketMethodID,RacersPerHeat,RacersPerClass,RepechagePlaces,RepechageQualifiers,PrimaryClassCost,SecondaryClassCost")] eventclass eventClass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.eventclasses.Add(eventClass);
                    db.SaveChanges();
                   
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race class could not be created.";
                    return RedirectToAction("Details", "RaceEvent", new { id = eventClass.RaceEventID });
                }

                Session["id"] = null;
                Session["RaceEventName"] = null;
                TempData["Sucess"] = "Event Class created";
                return RedirectToAction("Details", "RaceEvent", new { id = eventClass.RaceEventID });
            }

            ViewBag.BuildBracketMethod = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Name", eventClass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethod = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Name", eventClass.BuildQualificationMethodID);

            // display only the race classes associated with the organization
            int orgID = GetOrganizationIDForEvent(eventClass.RaceEventID);
            ViewBag.RaceClass = new SelectList(GetRaceClassesForOrganization(orgID), "RaceClassID", "Name", eventClass.RaceClassID);

            return View();
        }

        // GET: EventClass/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventclass eventclass = db.eventclasses.Find(id);
            if (eventclass == null)
            {
                return HttpNotFound();
            }

            ViewBag.BuildBracketMethod = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Name", eventclass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethod = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Name", eventclass.BuildQualificationMethodID);

            // display only the race classes associated with the organization
            int orgID = GetOrganizationIDForEvent(eventclass.RaceEventID);
            ViewBag.RaceClass = new SelectList(GetRaceClassesForOrganization(orgID), "RaceClassID", "Name", eventclass.RaceClassID);
            return View(eventclass);
        }

        // POST: EventClass/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventClassID,RaceEventID,RaceClassID,BuildQualificationMethodID,BuildBracketMethodID,LinkType,RacersPerHeat,RacersPerClass,RepechagePlaces,RepechageQualifiers,PrimaryClassCost,SecondaryClassCost,Unum,UnumSync,UnumTime")] eventclass eventclass)
        {
            if (ModelState.IsValid)
            {
                // CONFIRM !!!!!
                try
                {
                    db.Entry(eventclass).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Race class could not be edited.";
                    return RedirectToAction("Details", "RaceEvent", new { id = eventclass.RaceEventID });
                }

                // remove RaceEvendID from session for next create.
                Session["id"] = null;
                Session["RaceEventName"] = null;

                TempData["Sucess"] = "Event Class edited.";
                return RedirectToAction("Details", "RaceEvent", new { id = eventclass.RaceEventID });
            }
            ViewBag.BuildBracketMethod = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Name", eventclass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethod = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Name", eventclass.BuildQualificationMethodID);

            // display only the race classes associated with the organization
            int orgID = GetOrganizationIDForEvent(eventclass.RaceEventID);
            ViewBag.RaceClassID = new SelectList(GetRaceClassesForOrganization(orgID), "RaceClassID", "Name", eventclass.RaceClassID);
            return View(eventclass);
        }

        // GET: EventClass/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eventclass eventclass = db.eventclasses.Find(id);
            if (eventclass == null)
            {
                return HttpNotFound();
            }
            return View(eventclass);
        }

        // POST: EventClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eventclass eventclass = db.eventclasses.Find(id);
            try
            {
                db.eventclasses.Remove(eventclass);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "Race class could not be deleted.";
                return RedirectToAction("Details", "RaceEvent", new { id = eventclass.RaceEventID });                
            }
            TempData["Sucess"] = "Event class deleted";
            return RedirectToAction("Details", "RaceEvent", new { id = eventclass.RaceEventID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<raceclass> GetRaceClassesForOrganization(int orgID)
        {
            return db.raceclasses.Where(r => r.OrganizationID == orgID).OrderBy(o => o.Name).ToList();
        }

        public int GetOrganizationIDForEvent(int eventID)
        {
            return db.raceevents.Where(e => e.RaceEventID == eventID).Select(o => o.OrganizationID).FirstOrDefault();
        }
    }
}
