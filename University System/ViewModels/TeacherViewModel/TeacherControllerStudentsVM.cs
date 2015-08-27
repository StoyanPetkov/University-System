using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.TeacherViewModel
{
    public class TeacherControllerStudentsVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FaculityNumber { get; set; }
        public string Course { get; set; }
        public List<Grade> Grades { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public List<Subject> SubjectList { get; set; }
        public Dictionary<string, List<Grade>> SubjectGradeList { get; set; }
    }
}