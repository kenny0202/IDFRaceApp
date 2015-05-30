using IDFWebApp.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace IDFWebApp.Controllers.Custom
{
    [Authorize]
    public class JoinOrganizationController : Controller
    {
        // GET: JoinOrganization
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JoinOrg(int OrganizationID = 0)
        {
            raceEntities db = new raceEntities();

            var user = User.Identity.GetUserId();
            identitykey key = db.identitykeys.Where(k => k.IdentityGuid.Equals(user)).FirstOrDefault();

            Session["UserID"] = key.PersonID;

            Session["OrgID"] = OrganizationID;
            ViewBag.Organizations = new SelectList(db.organizations.ToList(), "OrganizationID", "Name");

            if (Request.IsAjaxRequest())
            {
                List<organizationroletype> listOfRolesForEachOrg = db.organizationroletypes
                    .Where(o => o.OrganizationID == OrganizationID && ((o.Code == "RACER") || (o.Code == "VOL") || (o.Code == "FLWR"))).ToList();

                // load the dropdown
                ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name");

                Session["OrgRoles"] = listOfRolesForEachOrg;
                // return the form
                return PartialView("~/Views/JoinOrganization/_JoinOrganizationPartial.cshtml");
            }

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult JoinOrg([Bind(Include = "OrganizationID,PersonID,OrganizationRoleTypeID")] personorganizationrole personorganizationrole)
        {
            raceEntities db = new raceEntities();

            if (ModelState.IsValid)
            {
                try
                {
                    personorganizationrole.CompetitorLevel = 0;
                    personorganizationrole.MemberNumber = 0;
                    personorganizationrole.AnnualDuesPaid = 0;
                    personorganizationrole.AutoRenewSubscription = false;
                    personorganizationrole.PriorityStatus = false;
                    personorganizationrole.Unum = 0;
                    personorganizationrole.UnumSync = -1;
                    personorganizationrole.UnumTime = DateTime.Now;

                    db.personorganizationroles.Add(personorganizationrole);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Cannot be saved. Please choose another.";
                    return RedirectToAction("JoinOrg", "JoinOrganization");
                }

                TempData["Success"] = "Successfully joined";
                return RedirectToAction("JoinOrg", "JoinOrganization");
            }

            List<organizationroletype> roles = (List<organizationroletype>)Session["OrgRoles"];
            List<organizationroletype> listOfRolesForEachOrg = new List<organizationroletype>();

            foreach (var item in roles)
            {
                listOfRolesForEachOrg.Add(item);
            }

            //List<organizationroletype> listOfRolesForEachOrg = db.organizationroletypes
            //        .Where(o => o.OrganizationID == OrganizationID && ((o.Code == "RACER") || (o.Code == "VOL") || (o.Code == "FLWR"))).ToList();

            ViewBag.Organizations = new SelectList(db.organizations.ToList(), "OrganizationID", "Name");
            ViewBag.OrganizationRoleTypes = new SelectList(listOfRolesForEachOrg, "OrganizationRoleTypeID", "Name");

            return View(personorganizationrole);
        }

    }
}