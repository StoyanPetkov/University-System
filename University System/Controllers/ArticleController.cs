using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using University_System.Entities;
using University_System.Models;
using University_System.Repositories;
using University_System.ViewModels.ArticleViewModel;

namespace University_System.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Articles()
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Default");
            }
			string test = "test";
            List<Article> articleList = new List<Article>();
            ArticleRepository articleRepository = new ArticleRepository();
            Dictionary<int, List<Comment>> comments = new Dictionary<int, List<Comment>>();
            CommentRepository commentRepository = new CommentRepository();
            Dictionary<int, string> userDictionary = new Dictionary<int, string>();
            List<int> subjectId = new List<int>();
            Teacher teacher = new Teacher();
            TeacherRepository teacherRepository = new TeacherRepository();
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            List<Article> list = new List<Article>();
            if (AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);
                foreach (var item in teacher.CourseSubject)
                {
                    subjectId.Add(item.Subject.Id);
                }
            }
            else if (AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Student)))
            {
                student = studentRepository.GetById(AuthenticationManager.LoggedUser.Id);
                List<CourseSubject> courseSubjectList = new List<CourseSubject>();
                CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
                courseSubjectList = courseSubjectRepository.GetAll(filter: cs => cs.CourseID == student.CourseID);
                foreach (var item in courseSubjectList)
                {
                    subjectId.Add(item.Subject.Id);
                }
            }
            subjectId = subjectId.Distinct().ToList();
            foreach (var item in subjectId)
            {
                List<Article> article = articleRepository.GetAll(filter: s => s.Subject.Id == item);
                if (article != null)
                {
                    articleList.AddRange(article);
                }
            }
            articleList = articleList.OrderBy(d => d.DateCreated.TimeOfDay).ToList();
            articleList.Reverse();
            ArticleControllerArticlesVM model = new ArticleControllerArticlesVM();
            LikeRepository likeRepository = new LikeRepository();
            Dictionary<Article, int> ArticlesAndLikeCount = new Dictionary<Article, int>();
            Dictionary<int, bool> Liked = new Dictionary<int, bool>();
            int articleId = 0;
            string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
            int start = type.LastIndexOf(".") + 1;
            int positions = type.Length - start;
            type = type.Substring(start, positions);
            foreach (var item in articleList)
            {
                List<Comment> commentedCommentList = new List<Comment>();
                commentedCommentList = commentRepository.GetAll(filter: c => c.Article.Id == item.Id);
                commentedCommentList.OrderBy(c => c.DateCreated.TimeOfDay).ToList();
                commentedCommentList.Reverse();
                foreach (var comment in commentedCommentList)
                {
                    string userName = "";
                    if (comment.UserType == "Teacher")
                    {
                        teacher = teacherRepository.GetById(comment.UserID);
                        if (teacher != null)
                        {
                            userName = teacher.FirstName + " " + teacher.LastName;
                            userDictionary.Add(comment.Id, userName);
                        }
                    }
                    else
                    {
                        student = studentRepository.GetById(comment.UserID);
                        userName = student.FirstName + " " + student.LastName;
                        userDictionary.Add(comment.Id, userName);
                    }
                }
                comments.Add(item.Id, commentedCommentList);
                int count = likeRepository.GetAll(filter: a => a.ArticleID == item.Id).Count;
                ArticlesAndLikeCount.Add(item, count);
                List<Like> likes = new List<Like>();

                likes = likeRepository.GetAll(l => l.ArticleID == item.Id);
                foreach (var like in likes.Where(l => l.UserID == AuthenticationManager.LoggedUser.Id && l.UserType == type))
                {
                    Liked.Add(item.Id, true);
                }
                model.ArticleId = item.Id;
                if (Liked.Count != ArticlesAndLikeCount.Count)
                {
                    foreach (var dictionary in ArticlesAndLikeCount.Where(a => a.Key.Id == item.Id))
                    {
                        articleId = item.Id;
                    }
                    Liked.Add(articleId, false);
                }
            }
            model.UserType = type;
            model.IsLiked = Liked;
            model.UserID = AuthenticationManager.LoggedUser.Id;
            model.Articles = ArticlesAndLikeCount;
            model.CommentList = comments;
            model.UserDictionary = userDictionary;
            return View(model);
        }

        public JsonResult Reply(int commentId, int replyId, string content)
        {
            CommentRepository commentRepository = new CommentRepository();
            Comment comment = new Comment();
            Comment replyComment = new Comment();
            if (replyId == 0)
            {
                comment = commentRepository.GetById(commentId);
                replyComment.Article = comment.Article;
                replyComment.ArticleID = comment.ArticleID;
                replyComment.DateCreated = DateTime.Now;
                replyComment.UserID = AuthenticationManager.LoggedUser.Id;
                replyComment.Content = content;
                replyComment.ParentComment = comment;
                string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
                int start = type.LastIndexOf(".") + 1;
                int positions = type.Length - start;
                type = type.Substring(start, positions);
                replyComment.UserType = type;
                commentRepository.Save(replyComment);
            }
            if (commentId == 0)
            {
                comment = commentRepository.GetById(replyId);
                comment.Content = content;
                comment.DateCreated = DateTime.Now;
                string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
                int start = type.LastIndexOf(".") + 1;
                int positions = type.Length - start;
                type = type.Substring(start, positions);
                comment.UserType = type;
                commentRepository.Save(comment);
            }
            SelectListItem item = new SelectListItem() { Text = commentId.ToString(), Value = comment.Article.Id.ToString() };
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteComment(int id)
        {
            List<Comment> comments = new List<Comment>();
            CommentRepository commentRepository = new CommentRepository();
            comments = commentRepository.GetAll(filter: c => c.Id == id || c.ParentComment.Id == id);
            foreach (var comment in comments)
            {
                commentRepository.Delete(comment);
            }
            return RedirectToAction("Articles");
        }

        public ActionResult DeleteReply(int id)
        {
            Comment comment = new Comment();
            CommentRepository commentRepository = new CommentRepository();
            comment = commentRepository.GetById(id);
            commentRepository.Delete(comment);
            return RedirectToAction("Articles");
        }

        public JsonResult Comment(int articleId, int commentId, string content)
        {
            ArticleRepository articleRepository = new ArticleRepository();
            CommentRepository commentRepository = new CommentRepository();
            Article article = new Article();
            Comment comment = new Comment();
            article = articleRepository.GetById(articleId);
            if (commentId == 0)
            {
                comment.ArticleID = articleId;
                comment.Content = content;
                comment.DateCreated = DateTime.Now;
                comment.UserID = AuthenticationManager.LoggedUser.Id;
                string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
                int start = type.LastIndexOf(".") + 1;
                int positions = type.Length - start;
                type = type.Substring(start, positions);
                comment.UserType = type;
            }
            else
            {
                comment = commentRepository.GetById(commentId);
                comment.Content = content;
                comment.DateCreated = DateTime.Now;
                string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
                int start = type.LastIndexOf(".") + 1;
                int positions = type.Length - start;
                type = type.Substring(start, positions);
                comment.UserType = type;
            }
            commentRepository.Save(comment);
            string comentator = AuthenticationManager.LoggedUser.FirstName + " " + AuthenticationManager.LoggedUser.LastName;
            SelectListItem commentContent = new SelectListItem() { Text = comment.Content, Value = comentator };
            return Json(commentContent, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditArticle(int id)
        {
            if (AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                ArticleControllerArticlesVM model = new ArticleControllerArticlesVM();
                TeacherRepository teacherRepository = new TeacherRepository();
                Article article = new Article();
                ArticleRepository articleRepository = new ArticleRepository();
                List<Subject> subjectList = new List<Subject>();
                SubjectRepository subjectRepository = new SubjectRepository();
                Teacher teacher = new Teacher();
                List<SelectListItem> listSubjects = new List<SelectListItem>();
                teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);
                List<int> subjectId = new List<int>();
                foreach (var item in teacher.CourseSubject)
                {
                    subjectId.Add(item.Subject.Id);
                }
                subjectId = subjectId.Distinct().ToList();
                foreach (var item in subjectId)
                {
                    subjectList.Add(subjectRepository.GetById(item));
                }

                if (id > 0)
                {
                    article = articleRepository.GetById(id);
                    model.ArticleId = article.Id;
                    model.TeacherID = teacher.Id;
                    model.Title = article.Title;
                    model.Content = article.Content;
                    model.DateCreated = article.DateCreated;
                    model.DateModified = article.DateModified;
                    model.Subject = article.Subject;
                    model.Teacher = teacher;
                    listSubjects.Add(new SelectListItem() { Text = article.Subject.Name, Value = article.Subject.Id.ToString(), Selected = true });
                }
                if (id == 0)
                {
                    model.ArticleId = 0;
                    listSubjects.Add(new SelectListItem() { Text = "Select subject", Value = "" });
                }
                foreach (var item in subjectList)
                {
                    if (item.Id != model.ArticleId)
                    {
                        listSubjects.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                    }
                }
                model.SubjectsListItems = listSubjects;
                return View(model);
            }
            return RedirectToAction("Articles");
        }

        [HttpPost]
        public ActionResult EditArticle(ArticleControllerArticlesVM model)
        {
            TryUpdateModel(model);
            if (model.SubjectID < 1 || !ModelState.IsValid)
            {
                model.ArticleId = 0;
                List<CourseSubject> courseSubject = new List<CourseSubject>();
                Teacher teacher = new Teacher();
                TeacherRepository teacherRepository = new TeacherRepository();
                teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);
                courseSubject = teacher.CourseSubject.ToList();
                List<SelectListItem> listSubjects = new List<SelectListItem>();
                listSubjects.Add(new SelectListItem() { Text = "Select subject", Value = "" });
                foreach (var item in courseSubject)
                {
                    if (item.Subject.Id != model.ArticleId)
                    {
                        listSubjects.Add(new SelectListItem() { Text = item.Subject.Name, Value = item.Subject.Id.ToString() });
                    }
                }
                model.SubjectsListItems = listSubjects;
            }
            if (ModelState.IsValid)
            {
                Article article = new Article();
                ArticleRepository articleRepository = new ArticleRepository();
                Subject subject = new Subject();
                SubjectRepository subjectRepository = new SubjectRepository();
                Teacher teacher = new Teacher();
                TeacherRepository teacherRepository = new TeacherRepository();
                teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);
                if (model.ArticleId > 0)
                {
                    article = articleRepository.GetById(model.ArticleId);
                    article.Content = model.Content;
                    article.DateCreated = model.DateCreated;
                    article.DateModified = DateTime.Now;
                    article.SubjectID = model.SubjectID;
                    article.TeacherID = teacher.Id;
                    article.Title = model.Title;
                }
                else
                {
                    article.Content = model.Content;
                    article.DateCreated = DateTime.Now;
                    article.SubjectID = model.SubjectID;
                    article.TeacherID = teacher.Id;
                    article.Title = model.Title;
                }
                articleRepository.Save(article);
                return RedirectToAction("Articles");
            }
            return View(model);
        }

        public ActionResult DeleteArticle(int id)
        {
            if (AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                Article article = new Article();
                ArticleRepository articleRepository = new ArticleRepository();
                article = articleRepository.GetById(id);
                articleRepository.Delete(article);
                return RedirectToAction("Articles");
            }
            return RedirectToAction("Articles");
        }

        public JsonResult Like(int id, string value)
        {
            bool success = false;
            Like like = new Like();
            Article article = new Article();
            ArticleRepository articleRepository = new ArticleRepository();
            LikeRepository likeRepository = new LikeRepository();
            article = articleRepository.GetById(id);
            List<Like> likeList = new List<Like>();
            string type = AuthenticationManager.LoggedUser.GetType().BaseType.ToString();
            int start = type.LastIndexOf(".") + 1;
            int positions = type.Length - start;
            type = type.Substring(start, positions);
            if (value == "Like")
            {
                like.ArticleID = id;
                like.UserID = AuthenticationManager.LoggedUser.Id;
               
                like.UserType = type;
                likeRepository.Save(like);
                success = true;
            }
            if (value == "UnLike")
            {
                like = likeRepository.GetAll(filter: l => l.ArticleID == id && l.UserID == AuthenticationManager.LoggedUser.Id && l.UserType == type).FirstOrDefault();
                likeRepository.Delete(like);
                success = true;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
    }
}