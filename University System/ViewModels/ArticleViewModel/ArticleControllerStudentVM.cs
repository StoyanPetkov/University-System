using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.ArticleViewModel
{
    public class ArticleControllerStudentVM
    {
        public DateTime CommentDateCreated { get; set; }
        public DateTime? CommentDateModified { get; set; }
        public Dictionary<int, List<Comment>> CommentList { get; set; }
        public Dictionary<Article, int> Articles { get; set; }
        public string CommentContent { get; set; }
        public int UserID { get; set; }
        public int CommentID { get; set; }
        public Subject Subject { get; set; }
    }
}