using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerStudentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Input First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Input Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Input User name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Input Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Input Faculity number")]
        public int FacultyNumber { get; set; }
        [Required(ErrorMessage = "Select course")]
        public int CourseID { get; set; }
        public bool isActive { get; set; }
        public string SelectedCurse { get; set; }

        public IEnumerable<SelectListItem> CourseListItems { get; set; }
        public List<Student> studentList { get; set; }
        public List<Course> CourseList { get; set; }
       
    }
}