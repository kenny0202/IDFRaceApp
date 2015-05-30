using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IDFWebApp.Models.Custom;

namespace IDFWebApp.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class IdentityKeysController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: IdentityKeys
        public ActionResult Index()
        {
            var identitykeys = db.identitykeys.Include(i => i.person);
            return View(identitykeys.ToList());
        }

        // GET: IdentityKeys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identitykey identitykey = db.identitykeys.Find(id);
            if (identitykey == null)
            {
                return HttpNotFound();
            }
            return View(identitykey);
        }

        // GET: IdentityKeys/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email");
            return View();
        }

        // POST: IdentityKeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(identitykey identitykey)
        {
            if (ModelState.IsValid)
            {
                db.identitykeys.Add(identitykey);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", identitykey.PersonID);
            return View(identitykey);
        }

        // GET: IdentityKeys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identitykey identitykey = db.identitykeys.Find(id);
            if (identitykey == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", identitykey.PersonID);
            return View(identitykey);
        }

        // POST: IdentityKeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdentityKeyID,PersonID,IdentityGuid,ProviderName,Unum,UnumSync,UnumTime")] identitykey identitykey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(identitykey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", identitykey.PersonID);
            return View(identitykey);
        }

        // GET: IdentityKeys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            identitykey identitykey = db.identitykeys.Find(id);
            if (identitykey == null)
            {
                return HttpNotFound();
            }
            return View(identitykey);
        }

        // POST: IdentityKeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            identitykey identitykey = db.identitykeys.Find(id);
            db.identitykeys.Remove(identitykey);
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
