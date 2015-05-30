using IDFWebApp.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDFWebApp.Controllers.Custom
{
    public class RegistrantsController : Controller
    {
        private raceEntities db = new raceEntities();
        // GET: Registrants
        public ActionResult Index(string name)
        {
            ViewBag.RaceEvent = new SelectList(db.raceevents, "Name", "Name");
            var result = db.raceevents;

            var registrants = (from per in db.people
                               join personorgrole in db.personorganizationroles on per.PersonID equals personorgrole.PersonID
                               join personeve in db.personevents on personorgrole.PersonOrganizationRoleID equals personeve.PersonOrganizationRoleID
                               join eventcla in db.eventclasses on personeve.RaceEventID equals eventcla.RaceEventID
                               join personeventcla in db.personeventclasses on eventcla.EventClassID equals personeventcla.EventClassID
                               join raceeve in db.raceevents on eventcla.RaceEventID equals raceeve.RaceEventID
                               join organizationrolety in db.organizationroletypes on personorgrole.OrganizationRoleTypeID equals organizationrolety.OrganizationRoleTypeID
                               where raceeve.Name == name
                               select new Registrants { LastName = per.LastName, FirstName = per.FirstName, RoleType = organizationrolety.Name }).Distinct().OrderBy(per => per.LastName);

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Registrants/partial.cshtml", registrants);
            }

            return View(registrants);
        }

        // GET: Registrants/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Registrants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registrants/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Registrants/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Registrants/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Registrants/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Registrants/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
