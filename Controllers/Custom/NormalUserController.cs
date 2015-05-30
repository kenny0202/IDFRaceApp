using IDFWebApp.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace IDFWebApp.Controllers.Custom
{
    public class NormalUserController : Controller
    {
        // GET: NormalUser
        public ActionResult Index()
        {
            return View();
        }

        public int GetPersonID()
        {
            raceEntities db = new raceEntities();
            aspnetmysqlEntities aspdb = new aspnetmysqlEntities();
            var user = User.Identity.GetUserId();
            var person = db.identitykeys.Where(i => i.IdentityGuid.Equals(user)).First();
            return person.PersonID;
        }

        public List<organization> GetPersonOrganization(int id)
        {
            raceEntities db = new raceEntities();

            // get all roles of a user in organizations
            var orgRoleTypes = db.personorganizationroles.Where(p => p.PersonID == id).ToList();

            List<int> orgs = new List<int>();
            foreach (var item in orgRoleTypes)
            {
                int orgID = db.organizationroletypes.Where(t => t.OrganizationRoleTypeID == item.OrganizationRoleTypeID).Select(o => o.OrganizationID).First();
                orgs.Add(orgID);
            }

            List<organization> orgsList = new List<organization>();
            for (int i = 0; i < orgs.Count; i++)
            {
                var org = db.organizations.Find(orgs.ElementAt(i));
                orgsList.Add(org);
            }
            return orgsList;
        }      
    }
}