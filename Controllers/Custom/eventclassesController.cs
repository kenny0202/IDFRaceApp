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
    public class eventclassesController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: eventclasses
        public ActionResult Index()
        {
            var eventclasses = db.eventclasses.Include(e => e.buildbracketmethod).Include(e => e.buildqualificationmethod).Include(e => e.raceclass).Include(e => e.raceevent);
            return View(eventclasses.ToList());
        }

        // GET: eventclasses/Details/5
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

        // GET: eventclasses/Create
        public ActionResult Create()
        {
            ViewBag.BuildBracketMethodID = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Code");
            ViewBag.BuildQualificationMethodID = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Code");
            ViewBag.RaceClass = new SelectList(db.raceclasses, "RaceClassID", "Code");
            ViewBag.RaceEventID = new SelectList(db.raceevents, "RaceEventID", "City");
            return View();
        }

        // POST: eventclasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventClassID,RaceEventID,RaceClassID,BuildQualificationMethodID,BuildBracketMethodID,LinkType,RacersPerHeat,RacersPerClass,RepechagePlaces,RepechageQualifiers,PrimaryClassCost,SecondaryClassCost,Unum,UnumSync,UnumTime")] eventclass eventclass)
        {
            if (ModelState.IsValid)
            {
                db.eventclasses.Add(eventclass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildBracketMethodID = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Code", eventclass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethodID = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Code", eventclass.BuildQualificationMethodID);
            ViewBag.RaceClass = new SelectList(db.raceclasses, "RaceClassID", "Code", eventclass.RaceClassID);
            ViewBag.RaceEventID = new SelectList(db.raceevents, "RaceEventID", "City", eventclass.RaceEventID);
            return View(eventclass);
        }

        // GET: eventclasses/Edit/5
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
            ViewBag.BuildBracketMethodID = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Code", eventclass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethodID = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Code", eventclass.BuildQualificationMethodID);
            ViewBag.RaceClassID = new SelectList(db.raceclasses, "RaceClassID", "Code", eventclass.RaceClassID);
            ViewBag.RaceEventID = new SelectList(db.raceevents, "RaceEventID", "City", eventclass.RaceEventID);
            return View(eventclass);
        }

        // POST: eventclasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventClassID,RaceEventID,RaceClassID,BuildQualificationMethodID,BuildBracketMethodID,LinkType,RacersPerHeat,RacersPerClass,RepechagePlaces,RepechageQualifiers,PrimaryClassCost,SecondaryClassCost,Unum,UnumSync,UnumTime")] eventclass eventclass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventclass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildBracketMethodID = new SelectList(db.buildbracketmethods, "BuildBracketMethodID", "Code", eventclass.BuildBracketMethodID);
            ViewBag.BuildQualificationMethodID = new SelectList(db.buildqualificationmethods, "BuildQualificationMethodID", "Code", eventclass.BuildQualificationMethodID);
            ViewBag.RaceClassID = new SelectList(db.raceclasses, "RaceClassID", "Code", eventclass.RaceClassID);
            ViewBag.RaceEventID = new SelectList(db.raceevents, "RaceEventID", "City", eventclass.RaceEventID);
            return View(eventclass);
        }

        // GET: eventclasses/Delete/5
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

        // POST: eventclasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eventclass eventclass = db.eventclasses.Find(id);
            db.eventclasses.Remove(eventclass);
            db.SaveChanges();
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
