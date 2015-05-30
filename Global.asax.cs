using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.ComponentModel;
using System.IO;

namespace IDFWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(), 2); //by default added
            filters.Add(new HandleErrorAttribute { View = "Error" }, 1);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Override DefaultBinder for all models
            ModelBinders.Binders.DefaultBinder = new CustomBinder();
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(HttpContext.Current.Server.MapPath("log4net.config")));
            //log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            try
            {
                log4net.Config.XmlConfigurator.Configure();
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            Application[HttpContext.Current.Request.UserHostAddress.ToString()] = ex;
            Server.ClearError();
            //Response.Redirect("/Content/Error.html");
            var exception = Server.GetLastError();
            // TODO: Log the exception or something
            Response.Clear();
            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "HttpError";
            Response.StatusCode = 500;
            IController controller = new Controllers.ErrorController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            controller.Execute(rc);
        }
    }

    public class CustomBinder : DefaultModelBinder
    {
        // Automatically trim all fields
        protected override object GetPropertyValue(ControllerContext controllerContext,
                                                    ModelBindingContext bindingContext,
                                                    PropertyDescriptor propertyDescriptor,
                                                    IModelBinder propertyBinder)
        {
            // Override ConvertEmptyStringToNull behavior so that it sends a blank input field as an empty string rather than null
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;

            // Override returned field so it does a trim of all fields 
            object value = base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);

            string retval = value as string;

            return string.IsNullOrWhiteSpace(retval) ? value : retval.Trim();
        }

    }
    public class OnError
    {
        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
                if (requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    httpContext.Response.Clear();
                    string controllerName = requestContext.RouteData.GetRequiredString("controller");
                    IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                    IController controller = factory.CreateController(requestContext, controllerName);
                    ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                    JsonResult jsonResult = new JsonResult();
                    jsonResult.Data = new { success = false, serverError = "500" };
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    jsonResult.ExecuteResult(controllerContext);
                    httpContext.Response.End();
                }
                else
                {
                    httpContext.Response.Redirect("~/Error");
                }
            }
        }
    }
}

