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
    public class PersonOrganizationRolesController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: personorganizationroles
        public ActionResult Index()
        {
            var personorganizationroles = db.personorganizationroles.Include(p => p.organizationroletype).Include(p => p.person);
            return View(personorganizationroles.ToList());
        }


        // GET: personorganizationroles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personorganizationrole personorganizationrole = db.personorganizationroles.Find(id);
            if (personorganizationrole == null)
            {
                return HttpNotFound();
            }
            return View(personorganizationrole);
        }

        // GET: personorganizationroles/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationRoleTypeID = new SelectList(db.organizationroletypes, "OrganizationRoleTypeID", "Code");
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email");
            return View();
        }

        // POST: personorganizationroles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonOrganizationRoleID,PersonID,OrganizationRoleTypeID,CompetitorLevel,MemberNumber,AnnualDuesPaid,ExpiryDate,AccountNumber,TransactionNumber,AutoRenewSubscription,PriorityStatus,Unum,UnumSync,UnumTime")] personorganizationrole personorganizationrole)
        {
            if (ModelState.IsValid)
            {
                db.personorganizationroles.Add(personorganizationrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationRoleTypeID = new SelectList(db.organizationroletypes, "OrganizationRoleTypeID", "Code", personorganizationrole.OrganizationRoleTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personorganizationrole.PersonID);
            return View(personorganizationrole);
        }

        // GET: personorganizationroles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personorganizationrole personorganizationrole = db.personorganizationroles.Find(id);
            if (personorganizationrole == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationRoleTypeID = new SelectList(db.organizationroletypes, "OrganizationRoleTypeID", "Code", personorganizationrole.OrganizationRoleTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personorganizationrole.PersonID);
            return View(personorganizationrole);
        }

        // POST: personorganizationroles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonOrganizationRoleID,PersonID,OrganizationRoleTypeID,CompetitorLevel,MemberNumber,AnnualDuesPaid,ExpiryDate,AccountNumber,TransactionNumber,AutoRenewSubscription,PriorityStatus,Unum,UnumSync,UnumTime")] personorganizationrole personorganizationrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personorganizationrole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationRoleTypeID = new SelectList(db.organizationroletypes, "OrganizationRoleTypeID", "Code", personorganizationrole.OrganizationRoleTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personorganizationrole.PersonID);
            return View(personorganizationrole);
        }

        // GET: personorganizationroles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personorganizationrole personorganizationrole = db.personorganizationroles.Find(id);
            if (personorganizationrole == null)
            {
                return HttpNotFound();
            }
            return View(personorganizationrole);
        }

        // POST: personorganizationroles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personorganizationrole personorganizationrole = db.personorganizationroles.Find(id);
            db.personorganizationroles.Remove(personorganizationrole);
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
