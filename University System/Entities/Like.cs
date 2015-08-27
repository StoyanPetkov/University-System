using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class Like : BaseEntityWithId
    {
        public int UserID { get; set; }
        public int ArticleID { get; set; }
        public string UserType { get; set; }
        public virtual Article Article { get; set; }
    }
}