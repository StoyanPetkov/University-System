using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.TeacherViewModel
{
    public class TeacherControllerTeacherVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Input first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Input last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Input password")]
        public string Password { get; set; }
        public Dictionary<string, List<Subject>> CourseSubjectList { get; set; }
    }
}