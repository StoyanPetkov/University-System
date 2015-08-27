using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;
using University_System.Models;

namespace University_System.CustomAttribute
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AuthenticationManager.LoggedUser == null || !AuthenticationManager.LoggedUser.GetType().Equals(typeof(Administrator)))
            {
                filterContext.Result = new RedirectResult("/Default/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}