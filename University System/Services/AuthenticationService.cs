using University_System.Entities;
using University_System.Repositories;
using System.Linq;

namespace University_System.Services
{
    public class AuthenticationService
    {
        public User LoggedUser { get; private set; }

        public bool AuthenticateUser(string username, string password, User.UserType userType)
        {
            switch (userType)
            {
                case User.UserType.Teacher:
                    TeacherRepository teacherRepository = new TeacherRepository();
                    LoggedUser = teacherRepository.GetAll(filter: u => u.UserName == username && u.Password == password && u.IsActive == true).FirstOrDefault();
                    break;
                case User.UserType.Administrator:
                    AdministratorRepository adminRepository = new AdministratorRepository();
                    LoggedUser = adminRepository.GetAll(filter: u => u.UserName == username && u.Password == password && u.IsActive == true).FirstOrDefault();
                    break;
                case User.UserType.Student:
                    StudentRepository studentRepository = new StudentRepository();
                    LoggedUser = studentRepository.GetAll(filter: u => u.UserName == username && u.Password == password && u.IsActive == true).FirstOrDefault();
                    break;
                default:
                    LoggedUser = null;
                    break;
            }
            return LoggedUser != null;
        }
    }
}