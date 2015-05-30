using IDFWebApp.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace IDFWebApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This application allows race administrators to set up race events and for competitors to register for race events";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact is by email only";

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
    }
}