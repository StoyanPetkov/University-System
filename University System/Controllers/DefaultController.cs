using System.Data.Entity.Core.Objects;
using System.Web.Mvc;
using University_System.Entities;
using University_System.Models;
using University_System.ViewModels.DefaultViewModel;

namespace University_System.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Login()
        {
            DefaultControllerLoginVM model = new DefaultControllerLoginVM();
            Administrator admin = new Administrator();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(DefaultControllerLoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                TryUpdateModel(model);
                AuthenticationManager.Authenticate(model.UserName, model.Password, model.UserType);
                if (AuthenticationManager.LoggedUser == null)
                {
                    return RedirectToAction("Login", "Default");
                }
                if (ObjectContext.GetObjectType(AuthenticationManager.LoggedUser.GetType()).Equals(typeof(Administrator)))
                {
                    return RedirectToAction("Home", "Admin");
                }
                if (ObjectContext.GetObjectType(AuthenticationManager.LoggedUser.GetType()).Equals(typeof(Teacher)))
                {
                    return RedirectToAction("Index", "Teacher");
                }
                if (ObjectContext.GetObjectType(AuthenticationManager.LoggedUser.GetType()).Equals(typeof(Student)))
                {
                    return RedirectToAction("Index", "Student");
                }
                return RedirectToAction("Login", "Default");
            }

        }

        public ActionResult Logout()
        {
            AuthenticationManager.Logout();
            Session["LoggedUser"] = null;
            return RedirectToAction("Login", "Default");
        }
    }
}