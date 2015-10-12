using University_System.Entities;
using University_System.Repositories;
using System.Linq;

namespace University_System.Services
{
    public class AuthenticationService
    {
        public User LoggedUser { get; private set; }
        private User user = null;

        public bool AuthenticateUser(string username, string password, User.UserType userType)
        {
            switch (userType)
            {
                case User.UserType.Teacher:
                    TeacherRepository teacherRepository = new TeacherRepository();
                    user = teacherRepository.GetAll(filter: u => u.UserName == username && u.IsActive == true).FirstOrDefault();
                    break;
                case User.UserType.Administrator:
                    AdministratorRepository adminRepository = new AdministratorRepository();
                    user = adminRepository.GetAll(filter: u => u.UserName == username && u.IsActive == true).FirstOrDefault();
                    break;
                case User.UserType.Student:
                    StudentRepository studentRepository = new StudentRepository();
                    user = studentRepository.GetAll(filter: u => u.UserName == username && u.IsActive == true).FirstOrDefault();
                    break;
                default:
                    LoggedUser = null;
                    break;
            }
            if (SecurityService.ValidatePassword(password, user.Password))
                LoggedUser = user;

            return LoggedUser != null;
        }
    }
}