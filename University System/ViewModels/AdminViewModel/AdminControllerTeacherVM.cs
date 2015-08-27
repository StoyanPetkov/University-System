using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerTeacherVM
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
        [Required(ErrorMessage = "Select Title")]
        public int TitleID { get; set; }
        public int CourseID { get; set; }
        public string SelectedTitle { get; set; }
        public int teachingSubjects { get; set; }
        public List<Teacher> teacherList { get; set; }
        public bool isActive { get; set; }
        public List<Title> TitleList { get; set; }
        public List<CourseSubject> CourseSubjectList { get; set; }

        public IEnumerable<SelectListItem> ListItems { get; set; }
    }
}