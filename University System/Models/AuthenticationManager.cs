using System.Web;
using University_System.Entities;
using University_System.Services;

namespace University_System.Models
{
    public class AuthenticationManager
    {
        private static AuthenticationService AuthenticationServiceInstance
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session[typeof(AuthenticationService).Name] == null)
                {
                    HttpContext.Current.Session[typeof(AuthenticationService).Name] = new AuthenticationService();
                }
                return (AuthenticationService)HttpContext.Current.Session[typeof(AuthenticationService).Name];
            }
        }

        public static User LoggedUser
        {
            get
            {
                return AuthenticationServiceInstance.LoggedUser;
            }
        }

        public static void Authenticate(string username, string password, User.UserType userType)
        {
            AuthenticationServiceInstance.AuthenticateUser(username, password, userType);
        }

        public static void Logout()
        {
            AuthenticationServiceInstance.AuthenticateUser(null, null, 0);
            HttpContext.Current.Session[typeof(AuthenticationManager).Name] = null;
        }
    }
}