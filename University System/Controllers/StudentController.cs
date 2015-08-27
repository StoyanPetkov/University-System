using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using University_System.Entities;
using University_System.Models;
using University_System.Repositories;
using University_System.ViewModels.StudentViewModel;

namespace University_System.Controllers
{
    public class StudentController : Controller
    {
        public ActionResult Index()
        {
            if (AuthenticationManager.LoggedUser == null)
            {
                return RedirectToAction("Login", "Default");
            }
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(AuthenticationManager.LoggedUser.Id);
            StudentControllerStudentVM model = new StudentControllerStudentVM();
            Course course = new Course();
            CourseRepository courseRepository = new CourseRepository();
            course = courseRepository.GetAll(filter: c => c.Id == student.CourseID).FirstOrDefault();
            List<Subject> subjectList = new List<Subject>();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            subjectList = courseSubjectRepository.GetAll(filter: c => c.CourseID == course.Id).Select(s => s.Subject).ToList();
            List<string> subjectNames = new List<string>();
            foreach (var subject in subjectList)
            {
                subjectNames.Add(subject.Name);
            }
            model.Subjects = subjectNames;
            model.FirstName = student.FirstName;
            model.LastName = student.LastName;
            model.StudentID = AuthenticationManager.LoggedUser.Id;
            model.Course = course.Name;
            model.FaculityNumber = student.FacultyNumber;
            return View(model);
        }

        public ActionResult ChangePassword(int id)
        {
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(id);
            StudentControllerStudentVM model = new StudentControllerStudentVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(int id, StudentControllerStudentVM model)
        {
            TryUpdateModel(model);
            if (ModelState.IsValid)
            {
                Student student = new Student();
                StudentRepository studentRepository = new StudentRepository();
                student = studentRepository.GetById(id);
                student.Password = model.Password;
                studentRepository.Save(student);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult ShowDetails(int id)
        {
            StudentControllerStudentVM model = new StudentControllerStudentVM();
            List<Grade> gradeList = new List<Grade>();
            GradeRepository gradeRepository = new GradeRepository();
            gradeList = gradeRepository.GetAll(filter: s => s.Student.Id == id);
            Dictionary<string, List<string>> details = new Dictionary<string, List<string>>();
            var subjectList = new List<string>();

            foreach (var item in gradeList)
            {
                subjectList.Add(item.Subject.Name);
            }
            subjectList = subjectList.Distinct().ToList();


            foreach (var item in subjectList)
            {
                var gradeValueList = new List<string>();
                List<Grade> grades = new List<Grade>();
                grades = gradeRepository.GetAll(filter: s => s.Subject.Name == item && s.Student.Id == id);
                foreach (var grade in grades)
                {
                    gradeValueList.Add(grade.GradeValue.ToString());
                }
                details.Add(item, gradeValueList);
            }
            model.SubjectGradeList = details;
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(id);
            model.FirstName = student.FirstName;
            model.LastName = student.LastName;
            return View(model);
        }
    }
}