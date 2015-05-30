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
    public class RaceClassController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: RaceClass
        public ActionResult Index()
        {
            var raceclasses = db.raceclasses.Include(r => r.organization);

            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                List<raceclass> raceClassesForAdmin = (from o in orgs join r in raceclasses on o.OrganizationID equals r.OrganizationID select r).ToList();

                return View(raceClassesForAdmin);
            }
            else
            {
                return View(raceclasses.ToList());
            }            
        }

        // GET: RaceClass/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceclass raceclass = db.raceclasses.Find(id);
            if (raceclass == null)
            {
                return HttpNotFound();
            }
            return View(raceclass);
        }

        // GET: RaceClass/Create
        public ActionResult Create()
        {

            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.Organization = new SelectList(orgs, "OrganizationID", "Name");
            }

            ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations, "OrganizationID", "Name");

            return View();
        }

        // POST: RaceClass/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationID,Sequence,Code,Name,Description,CompetitorLevel,MinimumAge,MaximumAge,Gender")] raceclass raceclass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.raceclasses.Add(raceclass);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    
                    Session["ErrorMsg"] = "Race class already exists."; 
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }

            List<organization> orgs = (List<organization>)Session["AdminOrgs"];
            ViewBag.OrganizationID = new SelectList(orgs, "OrganizationID", "Name");

            ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations, "OrganizationID", "Name");
            
            return View(raceclass);
        }

        // GET: RaceClass/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceclass raceclass = db.raceclasses.Find(id);
            if (raceclass == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationID = new SelectList(orgs, "OrganizationID", "Name", raceclass.RaceClassID);
            }
            if(User.IsInRole("SuperUser")){
                ViewBag.OrganizationID = new SelectList(db.organizations.ToList(), "OrganizationID", "Name", raceclass.RaceClassID);
            }

            ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations, "OrganizationID", "Name", raceclass.RaceClassID);
            
            return View(raceclass);
        }

        // POST: RaceClass/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RaceClassID,OrganizationID,Sequence,Code,Name,Description,CompetitorLevel,MinimumAge,MaximumAge,Gender,Unum,UnumSync,UnumTime")] raceclass raceclass)
        {
            if (ModelState.IsValid)
            {
                raceclass.Unum = raceclass.Unum + 1;
                try
                {
                    db.Entry(raceclass).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Session["ErrorMsg"] = "It was not possible to edit the race class."; 
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            List<organization> orgs = (List<organization>)Session["AdminOrgs"];
            ViewBag.OrganizationID = new SelectList(orgs, "OrganizationID", "Name", raceclass.RaceClassID);

            ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations, "OrganizationID", "Name", raceclass.RaceClassID);
            
            return View(raceclass);
        }

        // GET: RaceClass/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            raceclass raceclass = db.raceclasses.Find(id);
            if (raceclass == null)
            {
                return HttpNotFound();
            }
            return View(raceclass);
        }

        // POST: RaceClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            raceclass raceclass = db.raceclasses.Find(id);
            db.raceclasses.Remove(raceclass);
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

        //public List<raceclass> GetRaceClassesForAdminOrgs()
        //{
        //    List<raceclass> raceClassList;


        //    return raceClassList;
        //}
    }
}
