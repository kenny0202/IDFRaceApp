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
    [Authorize(Roles="SuperUser, Admin")]
    public class OrganizationsController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: organizations
        public ActionResult Index()
        {

            List<organization> orgs = GetOrganizationsForUser();
            //var organizations = db.organizations.Include(o => o.organizationtype).ToList();

            return View(orgs.ToList());
        }

        public List<organization> GetOrganizationsForUser()
        {
            AdminController ac = new AdminController();
            string personID = User.Identity.GetUserId();
            List<organization> orgs = ac.GetPersonOrganizationAdmin(93055);
            List<organization> orgList = new List<organization>();

            if (User.IsInRole("Admin"))
            {
                foreach (var item in orgs)
                {
                    var org = db.organizations.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                    foreach (var i in org)
                    {
                        orgList.Add(i);
                    }
                }
            }
            

            if (User.IsInRole("SuperUser"))
            {
                var organizations = db.organizations.OrderBy(o => o.Sequence).Include(o => o.organizationtype).ToList();

                foreach (var item in organizations)
                {
                    orgList.Add(item);
                }
            }

            return orgList;
        }

        // GET: organizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organization organization = db.organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: organizations/Create
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create()
        {
            ViewBag.OrganizationTypeID = new SelectList(db.organizationtypes, "OrganizationTypeID", "Name");
            return View();
        }

        // POST: organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser")]
        public ActionResult Create([Bind(Include = "OrganizationID,OrganizationTypeID,IsDeleted,Sequence,Code,Name,Description")] organization organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    organization.UnumSync = -1;
                    db.organizations.Add(organization);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "The organization could not be created.";
                    return RedirectToAction("Index");
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationTypeID = new SelectList(db.organizationtypes, "OrganizationTypeID", "Name", organization.OrganizationTypeID);
            return View(organization);
        }

        // GET: organizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organization organization = db.organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationTypeID = new SelectList(db.organizationtypes, "OrganizationTypeID", "Name", organization.OrganizationTypeID);
            return View(organization);
        }

        // POST: organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationID,OrganizationTypeID,IsDeleted,Sequence,Code,Name,Description,Unum,UnumTime")] organization organization)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    organization.Unum = organization.Unum + 1;
                    db.Entry(organization).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "The organization could not be edited.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationTypeID = new SelectList(db.organizationtypes, "OrganizationTypeID", "Name", organization.OrganizationTypeID);
            return View(organization);
        }

        // GET: organizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organization organization = db.organizations.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            organization organization = db.organizations.Find(id);

            try
            {
                db.organizations.Remove(organization);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "The organization could not be deleted.";
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
    }
}
