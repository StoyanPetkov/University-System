using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerGradeVM
    {
        public int GradeID { get; set; }
        [Required(ErrorMessage ="Choose subject")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Assign to student")]
        public int StudentID { get; set; }
        [Required(ErrorMessage = "Input grade")]
        public string Grade { get; set; }
        public List<Grade> gradeList { get; set; }
        public List<Subject> subjectList { get; set; }
        public List<Student> studentList { get; set; }
        public List<Course> courseList { get; set; }
    }
}