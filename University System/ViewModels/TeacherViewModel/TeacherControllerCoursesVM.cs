using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.TeacherViewModel
{
    public class TeacherControllerCoursesVM
    {
        public List<CourseSubject> CourseSubjectList { get; set; }
        public List<Subject> SubjectList { get; set; }
        public List<Course> CourseList { get; set; }
    }
}