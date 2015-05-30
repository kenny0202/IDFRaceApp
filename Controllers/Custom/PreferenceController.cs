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
    public class PreferenceController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: Preference
        public ActionResult Index()
        {
            int personID = GetPersonID();
            Session["PersonID"] = personID;
            var preferences = db.preferences.Where(i=>i.PersonID == personID).Include(p => p.organization).Include(p => p.person);

            int size = preferences.Count();
            if (size == 0)
            {
                Session["NoPref"] = 0;
            }
            
            return View(preferences.ToList());
        }

      
        // GET: Preference/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            preference preference = db.preferences.Find(id);
            if (preference == null)
            {
                return HttpNotFound();
            }
            return View(preference);
        }

        // GET: Preference/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Code");
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name");

            return View();
        }

        // POST: Preference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PreferenceID,PersonID,OrganizationID,Year,BracketMethod,RaceEvent," +
                                                    "RaceClass,QualificationMethod,RacesPerHeat,TweetQualifications," +
                                                    "TweetBrackets,TweetForm,HotSpotWebSite,Organization,EventLevelID")] preference preference)
        {
            if (ModelState.IsValid)
            {
                List<string> valuesList =  new List<string>();
                List<string> anotherList = new List<string>();


                foreach (var item in Request.Form.AllKeys)
                {
                    anotherList.Add(Request.Form[item]);
                    valuesList.Add(item);
                }
                Session["ValuesList"] = valuesList;
                Session["Another"] = anotherList;

               string pref = "RaceEventID=" + preference.RaceEvent + " " +
                                "RaceClassID=" + preference.RaceClass + " " +
                                "RacesPerHeat=" + preference.RacesPerHeat + " " +
                                "RacersPerClass=" + preference.RacersPerClass + " " +
                                "TweetQualifications=" + preference.TweetQualifications + " " +
                                "TweetBrackets=" + preference.TweetBrackets + " " +
                                "TweetForm=" + preference.TweetForm + " " +
                                "HotSpotWebSite=" + preference.HotSpotWebSite + " " +
                                "Organization=" + preference.OrganizationID + " " +
                                "BuildQualificationMethod=" + preference.QualificationMethod + " " +
                                "Year=" + preference.Year + " " +
                                "BuildBracketMethod=" + preference.BracketMethod + " " +
                                "EventLevelID=" + preference.EventLevelID;
                preference.Parameters = pref;

               
                try
                {
                    db.preferences.Add(preference);
                    db.SaveChanges();

                    // to disable link in menu that creates new preferences
                    Session["AdminPref"] = null;
                }
                catch (Exception)
                {
                    Session["ErrorMsg"] = "Preferences could not be created.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Code", preference.OrganizationID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", preference.PersonID);
            ViewBag.EventLevel = new SelectList(db.eventlevels, "EventLevelID", "Name");

            return View(preference);
        }

        // GET: Preference/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            preference preference = db.preferences.Find(id);
            if (preference == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Code", preference.OrganizationID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", preference.PersonID);
            return View(preference);
        }

        // POST: Preference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreferenceID,PersonID,OrganizationID,Parameters,Unum,UnumSync,UnumTime")] preference preference)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preference).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Session["ErrorMsg"] = "Preferences could not be edited.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Code", preference.OrganizationID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", preference.PersonID);
            return View(preference);
        }

        // GET: Preference/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            preference preference = db.preferences.Find(id);
            if (preference == null)
            {
                return HttpNotFound();
            }
            return View(preference);
        }

        // POST: Preference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            preference preference = db.preferences.Find(id);
            try
            {
                db.preferences.Remove(preference);
                db.SaveChanges();
            }
            catch (Exception)
            {
                Session["ErrorMsg"] = "Preference could not be deleted.";
                return RedirectToAction("Index");
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

        public int GetPersonID()
        {
            raceEntities db = new raceEntities();
            aspnetmysqlEntities aspdb = new aspnetmysqlEntities();
            var user = User.Identity.GetUserId();
            var person = db.identitykeys.Where(i => i.IdentityGuid.Equals(user)).First();

            return person.PersonID;
        }

         

    }
}
