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
    public class AdminPersonController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: AdminPerson
        public ActionResult Index()
        {
            var people = db.people.Include(p => p.country);
            return View(people.ToList());
        }

        // GET: AdminPerson/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: AdminPerson/Create
        public ActionResult Create()
        {
            ViewBag.NationalityID = new SelectList(db.countries, "CountryID", "Code");
            return View();
        }

        // POST: AdminPerson/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,IsDeleted,SuperUser,Email,Password,FirstName,LastName,NickName,NationalityID,BirthDate,Gender,Unum,UnumSync,UnumTime")] person person)
        {
            if (ModelState.IsValid)
            {
                db.people.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NationalityID = new SelectList(db.countries, "CountryID", "Code", person.NationalityID);
            return View(person);
        }

        // GET: AdminPerson/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.NationalityID = new SelectList(db.countries, "CountryID", "Code", person.NationalityID);
            return View(person);
        }

        // POST: AdminPerson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,IsDeleted,SuperUser,Email,Password,FirstName,LastName,NickName,NationalityID,BirthDate,Gender,Unum,UnumSync,UnumTime")] person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NationalityID = new SelectList(db.countries, "CountryID", "Code", person.NationalityID);
            return View(person);
        }

        // GET: AdminPerson/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = db.people.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: AdminPerson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            person person = db.people.Find(id);
            db.people.Remove(person);
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
