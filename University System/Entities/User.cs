using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class User : BaseEntityWithId
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public enum UserType
        {
            Administrator = 1,
            Teacher = 2,
            Student = 3
        }
    }
     
}