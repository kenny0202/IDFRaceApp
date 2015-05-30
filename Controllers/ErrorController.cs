using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using log4net;
using System.Data;
using System.Data.SqlClient;

namespace IDFWebApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult HttpError()
        {
            Exception ex = null;
            try
            {
                ex = (Exception)HttpContext.Application[Request.UserHostAddress.ToString()];
            }
            catch
            {
            }
            System.Data.Entity.Validation.DbEntityValidationException dbException = null;
            try
            {
                dbException = (System.Data.Entity.Validation.DbEntityValidationException) ex;
            }
            catch (Exception)
            {
            }
            if (dbException != null)
            {
                var innerException = dbException;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<br/>");
                sb.AppendLine("<h2>Database Error</h2>");
                sb.AppendLine("<br/>");
                foreach (var eve in innerException.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("--- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:<br/>",
                        eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;Property: \"{0}\"<br/>&nbsp;&nbsp;&nbsp;&nbsp;Value: \"{1}\"<br/>&nbsp;&nbsp;&nbsp;&nbsp;Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage));
                    }
                }
                ViewData["DbException"] = sb.ToString();
            }
            else
            {
                ViewData["DbException"] = string.Empty;

                if (ex != null)
                {
                    log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    string exError = ex.ToString();
                    int atat = exError.IndexOf(" at ");
                    atat = exError.IndexOf(" at ", atat + 5);
                    logger.Fatal("Error " + exError.Substring(0, atat));
                    ViewData["ExceptionMessage"] = "";  // "<br/>" + ex.Message;
                    string ie = string.Empty;
                    if (ex.InnerException != null)
                    {
                        ie = ex.InnerException.ToString();
                        int term = ie.IndexOf("The statement has been terminated");
                        if (term > 10)
                        {
                            ie = ie.Substring(0, term);
                            term = ie.IndexOf("SqlException:");
                            if (term > 10)
                            {
                                ie = ie.Substring(term + 14);
                            }
                        }
                    }
                    ViewData["ExceptionMessage"] = ex.Message;
                    ViewData["InnerException"] = "<br/><br/>" + ie;
                    ViewData["BaseException"] = "";     // ex.GetBaseException();
                }
                else
                {
                    if (ex.Message != null)
                    {
                        ViewData["ExceptionMessage"] = ex.Message;
                    }
                    else
                    {
                        ViewData["ExceptionMessage"] = string.Empty;
                    }
                    ViewData["InnerException"] = string.Empty;
                    ViewData["BaseException"] = string.Empty;
                }
            }

            ViewData["Title"] = "Oops. We're sorry. An error occurred and we're on the case.";

            //MySql.Data.MySqlClient.MySqlConnection conn;
            //string myConnectionString;

            //myConnectionString = "server=127.0.0.1;uid=root;pwd=password;database=log4net;";

            //try
            //{
            //    conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            //    conn.Open();
            //}
            //catch (MySql.Data.MySqlClient.MySqlException ex2)
            //{
            //    var x = ex2.Message;
            //}

            return View("Error");
        }

        public ActionResult Http404()
        {
            ViewData["Title"] = "The page you requested was not found";

            return View("Error");
        }

        // (optional) Redirect to home when /Error is navigated to directly
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Test()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Test(string confirmButton)
        {
            throw new ApplicationException("Error handler test");
        }
    }
}