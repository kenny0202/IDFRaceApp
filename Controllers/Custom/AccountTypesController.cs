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
    public class accounttypesController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: accounttypes
        public ActionResult Index()
        {
            return View(db.accounttypes.ToList());
        }

        // GET: accounttypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            accounttype accounttype = db.accounttypes.Find(id);
            if (accounttype == null)
            {
                return HttpNotFound();
            }
            return View(accounttype);
        }

        // GET: accounttypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: accounttypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountTypeID,Sequence,Code,Name,Parameters")] accounttype accounttype)
        {
            // don't bind unum
            if (ModelState.IsValid)
            {
                accounttype.UnumSync = -1; //don't need
                db.accounttypes.Add(accounttype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accounttype);
        }

        // GET: accounttypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            accounttype accounttype = db.accounttypes.Find(id);
            if (accounttype == null)
            {
                return HttpNotFound();
            }
            return View(accounttype);
        }

        // POST: accounttypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountTypeID,Sequence,Code,Name,Parameters,Unum,UnumSync,UnumTime")] accounttype accounttype)
        {
            //bind unums, put it in a hidden field, increment values
            if (ModelState.IsValid)
            {
                accounttype.Unum = accounttype.Unum + 1;
                db.Entry(accounttype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accounttype);
        }

        // GET: accounttypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            accounttype accounttype = db.accounttypes.Find(id);
            if (accounttype == null)
            {
                return HttpNotFound();
            }
            return View(accounttype);
        }

        // POST: accounttypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            accounttype accounttype = db.accounttypes.Find(id);
            db.accounttypes.Remove(accounttype);
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
