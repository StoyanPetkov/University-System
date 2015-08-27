using System.Collections.Generic;

namespace University_System.Entities
{
    public class Teacher : User
    {
        public int TitleID { get; set; }
        public virtual Title Title { get; set; }
        public virtual ICollection<CourseSubject> CourseSubject { get; set; }
    }
}