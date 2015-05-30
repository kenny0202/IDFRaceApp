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
    public class OrganizationRoleTypesController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: organizationroletypes
        public ActionResult Index()
        {
            
            //var organizationroletypes = db.organizationroletypes.Include(o => o.currency).Include(o => o.organization);
            List<organizationroletype> roleTypes = GetOrgRolesForUser();

            return View(roleTypes.ToList());
        }

        public List<organizationroletype> GetOrgRolesForUser()
        {
            AdminController ac = new AdminController();
            string personID = User.Identity.GetUserId();
            List<organization> orgs = ac.GetPersonOrganizationAdmin(93055);
            List<organizationroletype> orgRoleTypes = new List<organizationroletype>();

            if (User.IsInRole("Admin"))
            {
                foreach (var item in orgs)
                {
                    var t = db.organizationroletypes.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                    foreach (var i in t)
                    {
                        orgRoleTypes.Add(i);
                    }
                }
            }
            

            if (User.IsInRole("SuperUser"))
            {
                var allOrgTypes = db.organizationroletypes.OrderBy(o => o.OrganizationRoleTypeID).Include(o => o.currency).Include(o => o.organization);

                foreach (var item in allOrgTypes)
                {
                    orgRoleTypes.Add(item);
                }
            }

            return orgRoleTypes;
        }


        // GET: organizationroletypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organizationroletype organizationroletype = db.organizationroletypes.Find(id);
            if (organizationroletype == null)
            {
                return HttpNotFound();
            }
            return View(organizationroletype);
        }

        // GET: organizationroletypes/Create
        public ActionResult Create()
        {
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Code");
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Code");

            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }

            return View();
        }

        // POST: organizationroletypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganizationRoleTypeID,OrganizationID,UsageType,CompetitorLevel,Sequence,Code,Name,Parameters,CurrencyID,DuesAmount,RenewalType")] organizationroletype organizationroletype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    organizationroletype.UnumSync = -1;
                    db.organizationroletypes.Add(organizationroletype);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "The role could not be created.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Code", organizationroletype.CurrencyID);
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Code", organizationroletype.OrganizationID);
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }

            return View(organizationroletype);
        }

        // GET: organizationroletypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organizationroletype organizationroletype = db.organizationroletypes.Find(id);
            if (organizationroletype == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Code", organizationroletype.CurrencyID);
            ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Code", organizationroletype.OrganizationID);

            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }
            
            return View(organizationroletype);
        }

        // POST: organizationroletypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationRoleTypeID,OrganizationID,UsageType,CompetitorLevel,Sequence,Code,Name,Parameters,CurrencyID,DuesAmount,RenewalType,Unum,UnumTime")] organizationroletype organizationroletype)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    organizationroletype.Unum = organizationroletype.Unum + 1;
                    db.Entry(organizationroletype).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "The role could not be edited.";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Code", organizationroletype.CurrencyID);
            ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Code", organizationroletype.OrganizationID);

            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }
            
            return View(organizationroletype);
        }

        // GET: organizationroletypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            organizationroletype organizationroletype = db.organizationroletypes.Find(id);
            if (organizationroletype == null)
            {
                return HttpNotFound();
            }
            return View(organizationroletype);
        }

        // POST: organizationroletypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            organizationroletype organizationroletype = db.organizationroletypes.Find(id);

            try
            {
                db.organizationroletypes.Remove(organizationroletype);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["Error"] = "The role could not be deleted.";
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
