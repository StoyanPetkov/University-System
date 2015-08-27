using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;

namespace University_System.ViewModels.ArticleViewModel
{
    public class ArticleControllerArticlesVM
    {
        [Required(ErrorMessage = "Select subject !")]
        public int SubjectID { get; set; }
        public int ArticleId { get; set; }
        public int TeacherID { get; set; }
        [Required(ErrorMessage = "Input title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Input content")]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public IEnumerable<SelectListItem> SubjectsListItems { get; set; }
        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }
        public Dictionary<Article, int> Articles { get; set; }
        public string CommentContent { get; set; }
        public int UserID { get; set; }
        public int CommentID { get; set; }
        public DateTime CommentDateCreated { get; set; }
        public DateTime? CommentDateModified { get; set; }
        public Dictionary<int, List<Comment>> CommentList { get; set; }
        public Dictionary<int, bool> IsLiked { get; set; }
        public Dictionary<int, string> UserDictionary { get; set; }
        public string UserType { get; set; }
    }
}