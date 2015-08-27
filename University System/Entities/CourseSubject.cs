using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class CourseSubject : BaseEntityWithId
    {
        public int CourseID { get; set; }
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Teacher> Teacher { get; set; }
    }
}