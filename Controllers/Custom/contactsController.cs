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
    public class ContactsController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: contacts
        public ActionResult Index()
        {
            var userLoggedIn = User.Identity.Name;
            var GrabPersonID = db.people.Where(p => p.Email == userLoggedIn).Select(p => p.PersonID).FirstOrDefault();
            ViewBag.ID = GrabPersonID;
            var personCom = from per in db.contacts
                            where per.PersonID == GrabPersonID
                            select per;
            return View(personCom.ToList());
        }

        // GET: contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contact contact = db.contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: contacts/Create
        public ActionResult Create()
        {
            var userLoggedIn = User.Identity.Name;
            var grabPersonID = db.people.Where(p => p.Email == userLoggedIn).Select(p => p.PersonID).FirstOrDefault();
            ViewBag.ID = grabPersonID;
            ViewBag.CountryID = new SelectList(db.countries.OrderBy(p => p.Name), "CountryID", "Name");
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email");
            return View();
        }

        // POST: contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactID,PersonID,ContactRoleID,Name,Email,Phone,AddressLine1,AddressLine2,City,State,PostalCode,CountryID,Unum,UnumSync,UnumTime")] contact contact)
        {
            if (ModelState.IsValid)
            {
                db.contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.countries.OrderBy(p => p.Name), "CountryID", "Name", contact.CountryID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", contact.PersonID);
            return View(contact);
        }

        // GET: contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contact contact = db.contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.countries.OrderBy(p => p.Name), "CountryID", "Name", contact.CountryID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", contact.PersonID);
            return View(contact);
        }

        // POST: contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactID,PersonID,ContactRoleID,Name,Email,Phone,AddressLine1,AddressLine2,City,State,PostalCode,CountryID,Unum,UnumSync,UnumTime")] contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.countries.OrderBy(p => p.Name), "CountryID", "Name", contact.CountryID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", contact.PersonID);
            return View(contact);
        }

        // GET: contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contact contact = db.contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contact contact = db.contacts.Find(id);
            db.contacts.Remove(contact);
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
