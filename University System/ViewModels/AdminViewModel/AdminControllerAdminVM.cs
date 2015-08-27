using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerAdminVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Input First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Input Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Input User name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Input Password")]
        public string Password { get; set; }
        public List<Administrator> administratorList { get; set; }
        public bool isActive { get; set; }
        public int ActiveStudentCount { get; set; }
        public int InActiveStudentCount { get; set; }
        public int ActiveTeacherCount { get; set; }
        public int InActiveTeacherCount { get; set; }
        public int CourseCount { get; set; }
        public int SubjectCount { get; set; }
        public int TitleCount { get; set; }
    }
}