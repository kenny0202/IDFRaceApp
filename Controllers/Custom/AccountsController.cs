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
    [Authorize(Roles = "SuperUser, Admin")]
    public class AccountsController : Controller
    {
        private raceEntities db = new raceEntities();

        // GET: Accounts
        public ActionResult Index()
        {
            List<account> accounts = GetAccountPaymentForUser();
            //var accounts = db.accounts.Include(a => a.accounttype).Include(a => a.currency).Include(a => a.organization);
            return View(accounts.ToList());
        }

        public List<account> GetAccountPaymentForUser()
        {
            AdminController ac = new AdminController();
            string personID = User.Identity.GetUserId();
            List<organization> orgs = ac.GetPersonOrganizationAdmin(93055);
            List<account> accountsList = new List<account>();

            if (User.IsInRole("Admin"))
            {
                foreach (var item in orgs)
                {
                    var org = db.accounts.Where(o => o.OrganizationID == item.OrganizationID).ToList();
                    foreach (var i in org)
                    {
                        accountsList.Add(i);
                    }
                }
            }


            if (User.IsInRole("SuperUser"))
            {
                var accounts = db.accounts.Include(a => a.accounttype).Include(a => a.currency).Include(a => a.organization).ToList();

                foreach (var item in accounts)
                {
                    accountsList.Add(item);
                }
            }

            return accountsList;
        }



        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.AccountTypeID = new SelectList(db.accounttypes, "AccountTypeID", "Name");
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Name");
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Name");
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

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,OrganizationID,AccountTypeID,CurrencyID,Name,BillingNumber,Parameters")] account account)
        {
            if (ModelState.IsValid)
            {
                account.UnumSync = -1;
                db.accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountTypeID = new SelectList(db.accounttypes, "AccountTypeID", "Name", account.AccountTypeID);
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Name", account.CurrencyID);
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Name", account.OrganizationID);
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }
            
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountTypeID = new SelectList(db.accounttypes, "AccountTypeID", "Name", account.AccountTypeID);
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Name", account.CurrencyID);
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Name", account.OrganizationID);
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }
            
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,OrganizationID,AccountTypeID,CurrencyID,Name,BillingNumber,Parameters,Unum,UnumTime")] account account)
        {
            if (ModelState.IsValid)
            {
                account.Unum = account.Unum + 1;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountTypeID = new SelectList(db.accounttypes, "AccountTypeID", "Name", account.AccountTypeID);
            ViewBag.CurrencyID = new SelectList(db.currencies, "CurrencyID", "Name", account.CurrencyID);
            //ViewBag.OrganizationID = new SelectList(db.organizations, "OrganizationID", "Name", account.OrganizationID);
            if (User.IsInRole("Admin"))
            {
                List<organization> orgs = (List<organization>)Session["AdminOrgs"];
                ViewBag.OrganizationsForAdmin = new SelectList(orgs.OrderBy(o => o.Name), "OrganizationID", "Name");
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.OrganizationsForSuperUser = new SelectList(db.organizations.OrderBy(o => o.Name), "OrganizationID", "Name");

            }
            
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            account account = db.accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            account account = db.accounts.Find(id);
            db.accounts.Remove(account);
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
