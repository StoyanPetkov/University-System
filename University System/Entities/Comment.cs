using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class Comment : BaseEntityWithId
    {
        public string Content { get; set; }
        public int ArticleID { get; set; }
        public int UserID { get; set; }
        public string UserType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Article Article { get; set; }
        public virtual Comment ParentComment { get; set; }
    }
}