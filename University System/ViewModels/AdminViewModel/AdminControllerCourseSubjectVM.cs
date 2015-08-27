using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerCourseSubjectVM
    {
        public int CourseSubjectID { get; set; }
        [Required(ErrorMessage = "Select course")]
        public int courseID { get; set; }
        [Required(ErrorMessage = "Select subject")]
        public int subjectID { get; set; }
        public string StudentName { get; set; }
        public List<Subject> subjectList { get; set; }
        public List<CourseSubject> courseSubjectList { get; set; }
        public List<Student> studentList { get; set; }
        public Dictionary<string, List<Grade>> details { get; set; }
        public List<Course> courseList { get; set; }
        public IEnumerable<SelectListItem> ListItems { get; set; }
        public int StudentID { get; set; }
    }
}