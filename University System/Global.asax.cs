﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using University_System.Controllers;

namespace University_System
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //protected void Application_EndRequest()
        //{
        //    if (Context.Response.StatusCode == 401.0)
        //    {
        //        Response.Clear();
        //        var rd = new RouteData();
        //        rd.DataTokens["area"] = "AreaName"; // In case controller is in another area
        //        rd.Values["controller"] = "Errors";
        //        rd.Values["action"] = "NotFound";

        //        IController c = new ErrorsController();
        //        c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
        //    }
        //}
    }
}
