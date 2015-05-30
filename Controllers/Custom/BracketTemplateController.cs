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
    public class BracketTemplateController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: BracketTemplate
        public ActionResult Index()
        {
            return View(db.brackettemplates.ToList());
        }

        // GET: BracketTemplate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brackettemplate brackettemplate = db.brackettemplates.Find(id);
            if (brackettemplate == null)
            {
                return HttpNotFound();
            }
            return View(brackettemplate);
        }

        // GET: BracketTemplate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BracketTemplate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BracketTemplateID,BracketSize,BracketRiders,BracketNumber,BracketSeed,Unum,UnumSync,UnumTime")] brackettemplate brackettemplate)
        {
            if (ModelState.IsValid)
            {
                db.brackettemplates.Add(brackettemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brackettemplate);
        }

        // GET: BracketTemplate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brackettemplate brackettemplate = db.brackettemplates.Find(id);
            if (brackettemplate == null)
            {
                return HttpNotFound();
            }
            return View(brackettemplate);
        }

        // POST: BracketTemplate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BracketTemplateID,BracketSize,BracketRiders,BracketNumber,BracketSeed,Unum,UnumSync,UnumTime")] brackettemplate brackettemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brackettemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brackettemplate);
        }

        // GET: BracketTemplate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brackettemplate brackettemplate = db.brackettemplates.Find(id);
            if (brackettemplate == null)
            {
                return HttpNotFound();
            }
            return View(brackettemplate);
        }

        // POST: BracketTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            brackettemplate brackettemplate = db.brackettemplates.Find(id);
            db.brackettemplates.Remove(brackettemplate);
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

        public ActionResult FindTemplate(int pattern = 0)
        {

            var result = db.brackettemplates;


            switch (pattern)
            {
                
                    // 64 X 4
                case 1:
                    {
                        var result2 = result.Where(x => x.BracketSize == 64 && x.BracketRiders == 4).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                    // 32 X 4
                case 2:
                    {
                        var result2 = result.Where(x => x.BracketSize == 32 && x.BracketRiders == 4).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }

                case 3:
                    {
                        var result2 = result.Where(x => x.BracketSize == 16 && x.BracketRiders == 4).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 4:
                    {
                        var result2 = result.Where(x => x.BracketSize == 8 && x.BracketRiders == 4).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 5:
                    {
                        var result2 = result.Where(x => x.BracketSize == 96 && x.BracketRiders == 6).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 6:
                    {
                        var result2 = result.Where(x => x.BracketSize == 48 && x.BracketRiders == 6).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 7:
                    {
                        var result2 = result.Where(x => x.BracketSize == 24 && x.BracketRiders == 6).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 8:
                    {
                        var result2 = result.Where(x => x.BracketSize == 12 && x.BracketRiders == 6).ToList();
                        return PartialView("~/Views/BracketTemplate/PartialView/_BracketTemplatePartialTable.cshtml", result2);
                    }
                case 99:
                    {
                        return PartialView("~/Views/BracketTemplate/PartialView/_NotFound.cshtml");
                    }
                default:
                    {
                        return View("FindTemplate");
                    }
            }
        }
    }
}
