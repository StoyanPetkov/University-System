using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.StudentViewModel
{
    public class StudentControllerStudentVM
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage ="Password is invalid")]
        public string Password { get; set; }
        public Dictionary<string,List<string>> SubjectGradeList { get; set; }
        public string Course { get; set; }
        public List<string> Subjects { get; set; }
        public int FaculityNumber { get; set; }
    }
}