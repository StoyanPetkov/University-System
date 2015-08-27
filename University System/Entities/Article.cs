using System;
using System.Collections;
using System.Collections.Generic;

namespace University_System.Entities
{
    public class Article : BaseEntityWithId
    {
        public int SubjectID { get; set; }
        public int TeacherID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        //public virtual ICollection<Comment> Comment { get; set; }
        //public virtual ICollection<Like> Like { get; set; }
    }
}