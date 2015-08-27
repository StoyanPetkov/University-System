using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerCourseVM
    {
        public int CourseID { get; set; }
        [Required(ErrorMessage ="Input course name")]
        public string Name { get; set; }
        public List<Course> courseList {get;set;}
        public int Code { get; set; }
    }
}