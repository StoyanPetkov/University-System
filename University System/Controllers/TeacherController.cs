using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using University_System.Context;
using University_System.CustomAttribute;
using University_System.Entities;
using University_System.Models;
using University_System.Repositories;
using University_System.ViewModels.TeacherViewModel;


namespace University_System.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            if (AuthenticationManager.LoggedUser == null || !AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                return RedirectToAction("Login", "Default");
            }
            TeacherControllerTeacherVM model = new TeacherControllerTeacherVM();
            Teacher teacher = new Teacher();
            TeacherRepository teacherRepository = new TeacherRepository();
            teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);
            model.FirstName = teacher.FirstName;
            model.LastName = teacher.LastName;
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            CourseRepository courseRepository = new CourseRepository();
            SubjectRepository subjectRepository = new SubjectRepository();
            List<int> subjectList = new List<int>();
            Dictionary<string, List<Subject>> courseSubjectList = new Dictionary<string, List<Subject>>();
            List<Subject> subjects = new List<Subject>();
            List<int> courseList = new List<int>();
            foreach (var courseSubject in teacher.CourseSubject)
            {
                courseList.Add(courseSubject.Course.Id);
                subjectList.Add(courseSubject.Subject.Id);
            }
            subjectList = subjectList.Distinct().ToList();
            courseList = courseList.Distinct().ToList();
            Course course = new Course();
            foreach (var courseID in courseList)
            {
                course = courseRepository.GetById(courseID);
                subjects = courseSubjectRepository.GetAll(filter: c => c.Course.Id == courseID && subjectList.Contains(c.Subject.Id)).Select(s => s.Subject).ToList();
                courseSubjectList.Add(course.Name, subjects);
            }
            model.CourseSubjectList = courseSubjectList;
            return View(model);
        }

        public ActionResult Edit()
        {
            TeacherControllerTeacherVM teacherModel = new TeacherControllerTeacherVM();
            Teacher teacher = new Teacher();
            TeacherRepository teacherRepo = new TeacherRepository();

            teacherModel.Id = AuthenticationManager.LoggedUser.Id;
            teacher = teacherRepo.GetById(teacherModel.Id);
            teacherModel.FirstName = teacher.FirstName;
            teacherModel.LastName = teacher.LastName;
            teacherModel.Password = teacher.Password;

            return View(teacherModel);
        }

        [HttpPost]
        public ActionResult Edit(TeacherControllerTeacherVM teacherModel)
        {
            TryUpdateModel(teacherModel);
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher();
                TeacherRepository teacherRepo = new TeacherRepository();
                teacher = teacherRepo.GetById(teacherModel.Id);
                teacher.FirstName = teacherModel.FirstName;
                teacher.LastName = teacherModel.LastName;
                teacher.Password = teacherModel.Password;
                teacherRepo.Save(teacher);
                return RedirectToAction("Index");
            }
            return View(teacherModel);
        }

        public ActionResult Courses()
        {
            if (!AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                return RedirectToAction("Default", "Login");
            }
            TeacherControllerCoursesVM model = new TeacherControllerCoursesVM();
            CourseRepository courseRepository = new CourseRepository();

            Teacher teacher = new Teacher();
            TeacherRepository teacherRepository = new TeacherRepository();
            teacher = teacherRepository.GetById(AuthenticationManager.LoggedUser.Id);

            List<CourseSubject> courseSubjectList = teacher.CourseSubject.ToList();
            List<int> courseSubjectsId = courseSubjectList.Select(cs => cs.CourseID).Distinct().ToList();
            List<Course> courseList = new List<Course>();
            foreach (var item in courseSubjectsId)
            {
                courseList.Add(courseRepository.GetById(item));
            }
            model.CourseList = courseList;
            return View(model);
        }

        public JsonResult ShowStudents(int CourseID)
        {
            StudentRepository studentRepository = new StudentRepository();
            var students = studentRepository.GetAll(filter: s => s.CourseID == CourseID && s.IsActive == true);
            List<SelectListItem> studentList = new List<SelectListItem>();

            foreach (var item in students)
            {
                string name = item.FirstName + " " + item.LastName;
                studentList.Add(new SelectListItem() { Text = name, Value = item.Id.ToString() });
            }

            return Json(studentList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StudentDetails(int StudentID)
        {
            if (!AuthenticationManager.LoggedUser.GetType().BaseType.Equals(typeof(Teacher)))
            {
                return RedirectToAction("Default", "Login");
            }
            List<Grade> studentGrades = new List<Grade>();
            GradeRepository gradeRepository = new GradeRepository();
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(StudentID);
            TeacherControllerStudentsVM model = new TeacherControllerStudentsVM();
            studentGrades = gradeRepository.GetAll(filter: g => g.Student.Id == StudentID);

            model.FirstName = student.FirstName;
            model.LastName = student.LastName;
            model.FaculityNumber = student.FacultyNumber;
            model.Course = student.Course.Name;
            model.StudentID = student.Id;
            model.CourseID = student.Course.Id;

            Dictionary<string, List<Grade>> details = new Dictionary<string, List<Grade>>();
            List<string> subjectNameList = new List<string>();

            foreach (var item in studentGrades)
            {
                subjectNameList.Add(item.Subject.Name);
            }
            subjectNameList = subjectNameList.Distinct().ToList();

            foreach (var item in subjectNameList)
            {
                List<Grade> grades = new List<Grade>();
                grades = gradeRepository.GetAll(filter: s => s.Subject.Name == item && s.Student.Id == StudentID);
                details.Add(item, grades);
            }
            model.SubjectGradeList = details;

            List<Subject> subjects = new List<Subject>();
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository();
            List<CourseSubject> courseSubjectList = new List<CourseSubject>();
            courseSubjectList = courseSubjectRepo.GetAll(filter: c => c.CourseID == student.CourseID);
            foreach (var item in courseSubjectList)
            {
                subjects.Add(item.Subject);
            }
            model.SubjectList = subjects;
            return View(model);
        }

        public ActionResult ExportGrade(int studentId)
        {
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(studentId);
            Export export = new Export();
            DataTable gradeTable = export.StudentsGradesToExcel(student);

            GridView Grid = new GridView();
            Grid.DataSource = gradeTable;
            Grid.DataBind();

            string filename = "attachment; filename=" + student.FirstName + "_" + student.LastName + "_" + student.FacultyNumber + ".xls";
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", filename);
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        public JsonResult GetGrade(int gradeId)
        {
            Grade grade = new Grade();
            GradeRepository gradeRepository = new GradeRepository();
            grade = gradeRepository.GetById(gradeId);
            SelectListItem gradeItem = new SelectListItem() { Text = grade.GradeValue.ToString(), Value = grade.Id.ToString(), };
            return Json(gradeItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditGrade(int gradeId, double gradeValue, int subjectId, int studentId)
        {
            Grade grade = new Grade();
            GradeRepository gradeRepo = new GradeRepository();
            SelectListItem gradeItem = null;
            if (gradeId != 0)
            {
                grade = gradeRepo.GetById(gradeId);
                gradeValue = System.Math.Round(gradeValue, 2);
                grade.GradeValue = gradeValue;
                gradeRepo.Save(grade);
            }
            else
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                StudentRepository studentRepository = new StudentRepository(unitOfWork);
                GradeRepository gradeRepository = new GradeRepository(unitOfWork);
                SubjectRepository subjectRepository = new SubjectRepository(unitOfWork);
                Student student = new Student();
                student = studentRepository.GetById(studentId);
                Subject subject = new Subject();
                subject = subjectRepository.GetById(subjectId);
                grade.SubjectID = subjectId;
                grade.Subject = subject;
                grade.Student = student;
                gradeValue = System.Math.Round(gradeValue, 2);
                grade.GradeValue = gradeValue;
                gradeRepository.Save(grade);
                unitOfWork.Commit();
            }
            gradeItem = new SelectListItem() { Text = grade.GradeValue.ToString(), Value = grade.Id.ToString() };
            return Json(gradeItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGrade(int gradeId)
        {
            Grade grade = new Grade();
            UnitOfWork unitOfWork = new UnitOfWork();
            GradeRepository gradeRepository = new GradeRepository();
            grade = gradeRepository.GetById(gradeId);
            gradeRepository.Delete(grade);
            unitOfWork.Commit();
            return Json("successfully deleted", JsonRequestBehavior.AllowGet);
        }
    }
}