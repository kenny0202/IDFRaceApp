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
    public class PersonCommentsController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: personcomments
        public ActionResult Index()
        {
            var userLoggedIn = User.Identity.Name;
            var GrabPersonID = db.people.Where(p => p.Email == userLoggedIn).Select(p => p.PersonID).FirstOrDefault();
            ViewBag.ID = GrabPersonID;
            var personCom = from per in db.personcomments
                            where per.PersonID == GrabPersonID
                            select per;
            return View(personCom.ToList());
        }

        // GET: personcomments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personcomment personcomment = db.personcomments.Find(id);
            if (personcomment == null)
            {
                return HttpNotFound();
            }
            return View(personcomment);
        }

        // GET: personcomments/Create
        public ActionResult Create()
        {
            var userLoggedIn = User.Identity.Name;
            var grabPersonID = db.people.Where(p => p.Email == userLoggedIn).Select(p => p.PersonID).FirstOrDefault();
            ViewBag.ID = grabPersonID;
            ViewBag.CommentTypeID = new SelectList(db.commenttypes, "CommentTypeID", "Name");
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email");
            return View();
        }

        // POST: personcomments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonCommentID,CommentTypeID,PersonID,Comment,Unum,UnumSync,UnumTime")] personcomment personcomment)
        {
            if (ModelState.IsValid)
            {
                db.personcomments.Add(personcomment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommentTypeID = new SelectList(db.commenttypes, "CommentTypeID", "Code", personcomment.CommentTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personcomment.PersonID);
            return View(personcomment);
        }

        // GET: personcomments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personcomment personcomment = db.personcomments.Find(id);
            if (personcomment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommentTypeID = new SelectList(db.commenttypes, "CommentTypeID", "Code", personcomment.CommentTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personcomment.PersonID);
            return View(personcomment);
        }

        // POST: personcomments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonCommentID,CommentTypeID,PersonID,Comment,Unum,UnumSync,UnumTime")] personcomment personcomment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personcomment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommentTypeID = new SelectList(db.commenttypes, "CommentTypeID", "Code", personcomment.CommentTypeID);
            ViewBag.PersonID = new SelectList(db.people, "PersonID", "Email", personcomment.PersonID);
            return View(personcomment);
        }

        // GET: personcomments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            personcomment personcomment = db.personcomments.Find(id);
            if (personcomment == null)
            {
                return HttpNotFound();
            }
            return View(personcomment);
        }

        // POST: personcomments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            personcomment personcomment = db.personcomments.Find(id);
            db.personcomments.Remove(personcomment);
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
