using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class Student : User
    {
        public int FacultyNumber { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
    }
}