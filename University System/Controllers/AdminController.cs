using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using University_System.CustomAttribute;
using University_System.Entities;
using University_System.Repositories;
using University_System.ViewModels.AdminViewModel;
using University_System.Context;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using University_System.Models;

namespace University_System.Controllers
{
    [CustomAuthorize]
    public class AdminController : Controller
    {
        public ActionResult Home()
        {
            AdminControllerAdminVM model = new AdminControllerAdminVM();
            StudentRepository studentRepository = new StudentRepository();
            TeacherRepository teacherRepository = new TeacherRepository();
            CourseRepository courseRepository = new CourseRepository();
            SubjectRepository subjectRepository = new SubjectRepository();
            TitleRepository titleRepository = new TitleRepository();
            model.ActiveStudentCount = studentRepository.GetAll(filter: s => s.IsActive == true).Count;
            model.InActiveStudentCount = studentRepository.GetAll(filter: s => s.IsActive == false).Count;
            model.ActiveTeacherCount = teacherRepository.GetAll(filter: t => t.IsActive == true).Count;
            model.InActiveTeacherCount = teacherRepository.GetAll(filter: t => t.IsActive == false).Count;
            model.CourseCount = courseRepository.GetAll().Count;
            model.SubjectCount = subjectRepository.GetAll().Count;
            model.TitleCount = titleRepository.GetAll().Count();
            Administrator admin = new Administrator();
            AdministratorRepository adminRepository = new AdministratorRepository();
            admin = adminRepository.GetById(AuthenticationManager.LoggedUser.Id);
            model.FirstName = admin.FirstName;
            model.LastName = admin.LastName;
            return View(model);
        }

        #region ManageAdministrators
        public ActionResult ManageAdministrators()
        {
            AdminControllerAdminVM adminModel = new AdminControllerAdminVM();
            AdministratorRepository adminRepository = new AdministratorRepository();
            adminModel.administratorList = adminRepository.GetAll();
            return View(adminModel);
        }

        public ActionResult Edit(int id)
        {
            AdminControllerAdminVM adminModel = new AdminControllerAdminVM();
            AdministratorRepository adminRepository = new AdministratorRepository();
            if (id > 0)
            {
                Administrator admin = adminRepository.GetById(id);
                adminModel.FirstName = admin.FirstName;
                adminModel.LastName = admin.LastName;
            }
            return View(adminModel);
        }

        public JsonResult CheckForExistingName(string name, string type, int id)
        {
            bool exist = false;
            switch (type)
            {
                case "Admin":
                    Administrator admin = new Administrator();
                    AdministratorRepository adminRepository = new AdministratorRepository();
                    admin = adminRepository.GetAll(filter: a => a.UserName == name && a.Id != id).FirstOrDefault();
                    if (admin != null)
                    {
                        exist = true;
                    };
                    break;
                case "Student":
                    Student student = new Student();
                    StudentRepository studentRepository = new StudentRepository();
                    student = studentRepository.GetAll(filter: s => s.UserName == name && s.Id != id).FirstOrDefault();
                    if (student != null)
                    {
                        exist = true;
                    };
                    break;
                case "Teacher":
                    Teacher teacher = new Teacher();
                    TeacherRepository teacherRepository = new TeacherRepository();
                    teacher = teacherRepository.GetAll(filter: t => t.UserName == name && t.Id != id).FirstOrDefault();
                    if (teacher != null)
                    {
                        exist = true;
                    };
                    break;
                case "Course":
                    Course course = new Course();
                    CourseRepository courseRepository = new CourseRepository();
                    course = courseRepository.GetAll(filter: c => c.Name == name).FirstOrDefault();
                    if (course != null)
                    {
                        exist = true;
                    };
                    break;
                case "Title":
                    Title title = new Title();
                    TitleRepository titleRepository = new TitleRepository();
                    title = titleRepository.GetAll(filter: t => t.Name == name).FirstOrDefault();
                    if (title != null)
                    {
                        exist = true;
                    };
                    break;
                case "Subject":
                    Subject subject = new Subject();
                    SubjectRepository subjectRepository = new SubjectRepository();
                    subject = subjectRepository.GetAll(filter: s => s.Name == name).FirstOrDefault();
                    if (subject != null)
                    {
                        exist = true;
                    };
                    break;
            }

            return Json(exist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(int id, AdminControllerAdminVM model)
        {
            AdministratorRepository adminRepository = new AdministratorRepository();
            TryUpdateModel(model);
            if (ModelState.IsValid)
            {
                Administrator admin = null;
                admin = adminRepository.GetAll(filter: a => a.UserName == model.UserName).FirstOrDefault();
                if (admin == null)
                {
                    admin = new Administrator();
                    admin.UserName = model.UserName;
                    admin.Password = model.Password;
                    admin.FirstName = model.FirstName;
                    admin.LastName = model.LastName;
                    admin.IsActive = true;
                    adminRepository.Save(admin);
                    return RedirectToAction("ManageAdministrators");
                }
                else
                {
                    throw new ArgumentException("Invalid username !");
                }
            }
            return View(model);
        }

        public ActionResult DeleteAdministrator(int id)
        {
            AdministratorRepository adminRepository = new AdministratorRepository();
            if (id == AuthenticationManager.LoggedUser.Id)
            {
                return RedirectToAction("ManageAdministrators");
            }
            else
            {
                Administrator admin = adminRepository.GetById(id);
                adminRepository.Delete(admin);
                return RedirectToAction("ManageAdministrators");
            }
        }
        #endregion

        #region ManageTeachers
        public ActionResult ManageTeachers()
        {
            TeacherRepository teacherRepository = new TeacherRepository();
            AdminControllerTeacherVM teacherModel = new AdminControllerTeacherVM();
            List<CourseSubject> courseSubject = new List<CourseSubject>();
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository();

            teacherModel.teacherList = teacherRepository.GetAll();

            return View(teacherModel);
        }

        public ActionResult EditTeachers(int id)
        {
            TeacherRepository teacherRepository = new TeacherRepository();
            TitleRepository titleRepository = new TitleRepository();
            Teacher teacher = new Teacher();
            AdminControllerTeacherVM teacherModel = new AdminControllerTeacherVM();
            List<SelectListItem> SelectListTitle = new List<SelectListItem>();
            SelectListItem select = null;
            Title title = new Title();
            teacher.Title = title;

            if (id > 0)
            {
                teacher = teacherRepository.GetById(id);
                select = new SelectListItem() { Text = teacher.Title.Name, Value = teacher.Title.Id.ToString() };
                SelectListTitle.Add(select);
            }

            teacherModel.TitleList = titleRepository.GetAll();
            foreach (var item in teacherModel.TitleList)
            {
                if (item.Id != teacher.Title.Id)
                {
                    select = new SelectListItem() { Text = item.Name, Value = item.Id.ToString() };
                    SelectListTitle.Add(select);
                }
            }

            if (id > 0)
            {
                teacher = teacherRepository.GetById(id);
                teacherModel.FirstName = teacher.FirstName;
                teacherModel.LastName = teacher.LastName;
                teacherModel.UserName = teacher.UserName;
                teacherModel.Password = teacher.Password;
                teacherModel.isActive = teacher.IsActive;
                teacherModel.Id = id;
            }

            if (id == 0)
            {
                teacher.FirstName = teacherModel.FirstName;
                teacher.LastName = teacherModel.LastName;
                teacher.UserName = teacherModel.UserName;
                teacher.Password = teacherModel.Password;
                teacher.IsActive = teacherModel.isActive;
            }
            teacherModel.ListItems = SelectListTitle;
            return View(teacherModel);
        }

        [HttpPost]
        public ActionResult EditTeachers(int id, AdminControllerTeacherVM teacherModel)
        {
            UnitOfWork uOw = new UnitOfWork();
            Teacher teacher = null;
            Title title = new Title();
            TitleRepository titleRepository = new TitleRepository(uOw);
            TeacherRepository teacherRepository = new TeacherRepository(uOw);

            TryUpdateModel(teacherModel);
            if (teacherModel.ListItems == null)
            {
                teacherModel.TitleList = titleRepository.GetAll();
                List<SelectListItem> SelectListTitle = new List<SelectListItem>();

                foreach (var item in teacherModel.TitleList)
                {
                    SelectListTitle.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                teacherModel.ListItems = SelectListTitle;
            }

            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    teacher = teacherRepository.GetById(id);
                    teacher.FirstName = teacherModel.FirstName;
                    teacher.LastName = teacherModel.LastName;
                    teacher.UserName = teacherModel.UserName;
                    teacher.Password = teacherModel.Password;
                    title = titleRepository.GetById(teacherModel.TitleID);
                    teacher.Title = title;
                    teacher.IsActive = teacherModel.isActive;
                    teacherRepository.Save(teacher);
                    uOw.Commit();
                    return RedirectToAction("ManageTeachers");
                }
                else
                {
                    teacher = teacherRepository.GetAll(filter: t => t.UserName == teacherModel.UserName).FirstOrDefault();
                    if (teacher == null)
                    {
                        teacher = new Teacher();
                        teacher.FirstName = teacherModel.FirstName;
                        teacher.LastName = teacherModel.LastName;
                        teacher.UserName = teacherModel.UserName;
                        teacher.Password = teacherModel.Password;
                        title = titleRepository.GetById(teacherModel.TitleID);
                        teacher.Title = title;
                        teacher.IsActive = teacherModel.isActive;
                        teacherRepository.Save(teacher);
                        uOw.Commit();
                        return RedirectToAction("ManageTeachers");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid username !");
                    }
                }
            }
            return View(teacherModel);
        }

        public ActionResult AssignToCourse(int id)
        {
            TeacherRepository teacherRepo = new TeacherRepository();
            Teacher teacher = teacherRepo.GetById(id);
            AdminControllerTeacherVM teacherModel = new AdminControllerTeacherVM();
            CourseRepository courseRepo = new CourseRepository();
            List<Course> courses = new List<Course>();

            teacherModel.FirstName = teacher.FirstName;
            teacherModel.LastName = teacher.LastName;
            teacherModel.Id = id;
            courses = courseRepo.GetAll().OrderBy(c => c.Name).ToList();

            List<SelectListItem> SelectList = new List<SelectListItem>();
            SelectListItem select = null;
            teacherModel.CourseSubjectList = teacher.CourseSubject.ToList();

            foreach (var item in courses)
            {
                select = new SelectListItem() { Text = item.Name, Value = item.Id.ToString() };
                SelectList.Add(select);
            }
            teacherModel.ListItems = SelectList;

            return View(teacherModel);
        }

        public JsonResult AssignTo(int CourseID, int TeacherID)
        {
            CourseRepository courseRepository = new CourseRepository();
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository();
            TeacherRepository teacherRepository = new TeacherRepository();
            Teacher teacher = new Teacher();
            teacher = teacherRepository.GetById(TeacherID);
            List<SelectListItem> listSubjects = new List<SelectListItem>();
            var allSubjects = courseSubjectRepo.GetAll(filter: s => s.CourseID == CourseID);

            foreach (var item in allSubjects)
            {
                if (teacher.CourseSubject.Any(cs => cs.CourseID == CourseID && cs.SubjectID == item.SubjectID))
                {
                    listSubjects.Add(new SelectListItem() { Text = item.Subject.Name, Value = item.SubjectID.ToString(), Selected = true });
                }
                else
                {
                    listSubjects.Add(new SelectListItem() { Text = item.Subject.Name, Value = item.SubjectID.ToString(), Selected = false });
                }

            }

            return Json(listSubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignToCourse(AdminControllerTeacherVM model, string[] checkedSubjects)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            TeacherRepository teacherRepository = new TeacherRepository(unitOfWork);
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository(unitOfWork);
            TryUpdateModel(model);
            Teacher teacher = teacherRepository.GetById(model.Id);

            CourseSubject courseSubject = null;

            try
            {
                List<CourseSubject> courseSubjectsList = courseSubjectRepo.GetAll(c => c.CourseID == model.CourseID && c.Teacher.Any(t => t.Id == model.Id));
                foreach (var item in courseSubjectsList)
                {
                    teacher.CourseSubject.Remove(item);
                }

                if (checkedSubjects != null)
                {
                    foreach (var item in checkedSubjects)
                    {
                        courseSubject = courseSubjectRepo.GetAll(filter: cs => cs.CourseID == model.CourseID && cs.SubjectID.ToString() == item).FirstOrDefault();
                        teacher.CourseSubject.Add(courseSubject);
                    }
                }
                teacherRepository.Save(teacher);
                unitOfWork.Commit();
            }
            catch (Exception)
            {
                unitOfWork.RollBack();
            }

            return RedirectToAction("ManageTeachers", "Admin");
        }

        public ActionResult ShowTeachingSubjects(int id)
        {
            Teacher teacher = new Teacher();
            TeacherRepository teacherRepository = new TeacherRepository();
            teacher = teacherRepository.GetById(id);
            AdminControllerTeacherVM model = new AdminControllerTeacherVM();
            model.FirstName = teacher.FirstName + " " + teacher.LastName;
            model.CourseSubjectList = teacher.CourseSubject.ToList();
            return View(model);
        }

        public ActionResult DeleteTeacher(int id)
        {
            Teacher teacher = new Teacher();
            TeacherRepository teacherRepository = new TeacherRepository();
            teacher = teacherRepository.GetById(id);
            teacher.IsActive = false;
            teacherRepository.Save(teacher);
            return RedirectToAction("ManageTeachers");
        }
        #endregion

        #region ManageStudents
        public ActionResult ManageStudents()
        {
            StudentRepository studentRepository = new StudentRepository();
            AdminControllerStudentVM studentModel = new AdminControllerStudentVM();
            studentModel.studentList = studentRepository.GetAll();
            return View(studentModel);
        }

        private int GenerateFaculityNumber(int CourseID)
        {
            StringBuilder sb = new StringBuilder();
            StudentRepository studentRepository = new StudentRepository();
            CourseRepository courseRepository = new CourseRepository();
            Course course = courseRepository.GetById(CourseID);
            int year = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2));
            int studentNumber = studentRepository.GetAll().Count + 1;
            int faculityNumber = 0;

            sb.Append(studentNumber.ToString());
            while (sb.Length < 4)
            {
                sb.Insert(0, 0);
            }
            sb.Insert(0, course.Code.ToString());
            sb.Insert(0, year);

            if (sb.Length == 8)
            {
                faculityNumber = Convert.ToInt32(sb.ToString());
            }

            return faculityNumber;
        }

        public ActionResult EditStudents(int id)
        {
            CourseRepository courseRepository = new CourseRepository();
            StudentRepository studentRepository = new StudentRepository();
            AdminControllerStudentVM studentModel = new AdminControllerStudentVM();
            Student student = new Student();
            List<SelectListItem> List = new List<SelectListItem>();
            studentModel.CourseList = courseRepository.GetAll();

            foreach (var item in studentModel.CourseList)
            {
                List.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            if (id > 0)
            {
                student = studentRepository.GetById(id);
                List = List.Where(c => c.Text != student.Course.Name).ToList();
                studentModel.FirstName = student.FirstName;
                studentModel.LastName = student.LastName;
                studentModel.UserName = student.UserName;
                studentModel.Password = student.Password;
                studentModel.FacultyNumber = student.FacultyNumber;
                studentModel.CourseID = student.CourseID;
                studentModel.isActive = student.IsActive;
                studentModel.SelectedCurse = student.Course.Name;
                studentModel.Id = id;
            }
            if (id == 0)
            {

                student.FirstName = studentModel.FirstName;
                student.LastName = studentModel.LastName;
                student.UserName = studentModel.UserName;
                student.Password = studentModel.Password;
                student.CourseID = studentModel.CourseID;
            }
            studentModel.CourseListItems = List;
            return View(studentModel);
        }

        [HttpPost]
        public ActionResult EditStudents(int id, AdminControllerStudentVM studentModel)
        {
            TryUpdateModel(studentModel);
            if (studentModel.CourseListItems == null)
            {
                CourseRepository courseRepository = new CourseRepository();
                List<SelectListItem> List = new List<SelectListItem>();
                studentModel.CourseList = courseRepository.GetAll();

                foreach (var item in studentModel.CourseList)
                {
                    List.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                studentModel.CourseListItems = List;
            }

            if (ModelState.IsValid)
            {
                StudentRepository studentRepository = new StudentRepository();
                Student student = null;
                if (id > 0)
                {
                    student = studentRepository.GetById(id);
                    if (student.Course.Id != studentModel.CourseID)
                    {
                        CourseRepository courseRepository = new CourseRepository();
                        int newCode = courseRepository.GetById(studentModel.CourseID).Code;
                        int oldCode = courseRepository.GetById(student.Course.Id).Code;
                        StringBuilder sb = new StringBuilder();
                        sb.Append(student.FacultyNumber.ToString());
                        sb.Replace(oldCode.ToString(), newCode.ToString());
                        student.FacultyNumber = Convert.ToInt32(sb.ToString());
                    }
                    student.FirstName = studentModel.FirstName;
                    student.LastName = studentModel.LastName;
                    student.UserName = studentModel.UserName;
                    student.Password = studentModel.Password;
                    student.IsActive = studentModel.isActive;
                    student.CourseID = studentModel.CourseID;
                    studentRepository.Save(student);
                    return RedirectToAction("ManageStudents");
                }
                else
                {
                    student = studentRepository.GetAll(filter: s => s.UserName == studentModel.UserName).FirstOrDefault();
                    if (student == null)
                    {
                        student = new Student();
                        student.FirstName = studentModel.FirstName;
                        student.LastName = studentModel.LastName;
                        student.UserName = studentModel.UserName;
                        student.Password = studentModel.Password;
                        student.IsActive = studentModel.isActive;
                        student.FacultyNumber = GenerateFaculityNumber(studentModel.CourseID);
                        student.CourseID = studentModel.CourseID;
                        studentRepository.Save(student);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid username !");
                    }
                    return RedirectToAction("ManageStudents");
                }
            }
            return View(studentModel);
        }

        public ActionResult DeleteStudent(int id)
        {
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(id);
            student.IsActive = false;
            studentRepository.Save(student);
            return RedirectToAction("ManageStudents");
        }
        #endregion

        #region ManageCourses
        public ActionResult ManageCourses()
        {
            AdminControllerCourseVM courseModel = new AdminControllerCourseVM();
            CourseRepository courseRepository = new CourseRepository();
            courseModel.courseList = courseRepository.GetAll();
            return View(courseModel);
        }

        public ActionResult EditCourses(int id)
        {
            Course course = new Course();
            CourseRepository courseRepository = new CourseRepository();
            AdminControllerCourseVM courseModel = new AdminControllerCourseVM();

            if (id > 0)
            {
                course = courseRepository.GetById(id);
                courseModel.Name = course.Name;
                courseModel.Code = course.Code;
                courseModel.CourseID = id;
            }

            if (id == 0)
            {
                courseModel.Code = GenerateCode();
                course.Name = courseModel.Name;
                course.Code = courseModel.Code;
            }
            return View(courseModel);
        }

        private int GenerateCode()
        {
            Random rand = new Random();
            int code = 0;
            CourseRepository courseRepo = new CourseRepository();
            Course course = new Course();
            code = rand.Next(0, 99);
            course = courseRepo.GetAll(filter: c => c.Code == code).FirstOrDefault();

            while (course != null)
            {
                code = rand.Next(0, 99);
                course = courseRepo.GetAll(filter: c => c.Code == code).FirstOrDefault();
            }
            return code;
        }

        [HttpPost]
        public ActionResult EditCourses(int id, AdminControllerCourseVM courseModel)
        {
            TryUpdateModel(courseModel);
            if (ModelState.IsValid)
            {
                Course course = null;
                CourseRepository coureseRepository = new CourseRepository();
                if (id > 0)
                {
                    course = coureseRepository.GetById(id);
                    course.Name = courseModel.Name;
                    course.Code = courseModel.Code;
                    coureseRepository.Save(course);
                    return RedirectToAction("ManageCourses");
                }
                else
                {
                    course = coureseRepository.GetAll(filter: c => c.Name == courseModel.Name).FirstOrDefault();
                    if (course == null)
                    {
                        course = new Course();
                        course.Name = courseModel.Name;
                        course.Code = courseModel.Code;
                        coureseRepository.Save(course);
                        return RedirectToAction("ManageCourses");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid course name");
                    }
                }
            }
            return View(courseModel);
        }

        public JsonResult DeleteCourses(int id)
        {
            bool courseInUse = false;
            List<Student> studentList = new List<Student>();
            StudentRepository studentRepository = new StudentRepository();
            studentList = studentRepository.GetAll(filter: s => s.Course.Id == id);
            if (studentList.Count > 0)
            {
                courseInUse = true;
            }
            else
            {
                Course course = new Course();
                CourseRepository courseRepository = new CourseRepository();
                course = courseRepository.GetById(id);
                courseRepository.Delete(course);
            }
            return Json(courseInUse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManageGrades
        public ActionResult ShowCourses()
        {
            AdminControllerCourseVM courseModel = new AdminControllerCourseVM();
            CourseRepository courseRepository = new CourseRepository();
            courseModel.courseList = courseRepository.GetAll();
            return View(courseModel);
        }

        public ActionResult ShowCourseStudents(int id)
        {
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            StudentRepository studentRepository = new StudentRepository();
            List<Student> studentList = new List<Student>();
            courseSubjectModel.CourseSubjectID = id;
            if (id > 0)
            {
                courseSubjectModel.studentList = studentRepository.GetAll(filter: s => s.CourseID == id);
            }
            return View(courseSubjectModel);
        }

        public ActionResult ShowGrades(int id, int courseSubjectID)
        {
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            GradeRepository gradeRepository = new GradeRepository();
            List<Grade> studentGrades = gradeRepository.GetAll(filter: s => s.Student.Id == id);


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
                grades = gradeRepository.GetAll(filter: s => s.Subject.Name == item && s.Student.Id == id);
                details.Add(item, grades);
            }

            courseSubjectModel.details = details;

            courseSubjectModel.StudentID = id;
            Student student = new Student();
            StudentRepository studentRepository = new StudentRepository();
            student = studentRepository.GetById(id);
            courseSubjectModel.StudentName = student.FirstName + " " + student.LastName;
            courseSubjectModel.CourseSubjectID = courseSubjectID;
            return View(courseSubjectModel);
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

        #endregion

        #region ManageTitles
        public ActionResult ManageTitles()
        {
            TitleRepository titleRepository = new TitleRepository();
            AdminControllerTitleVM titleModel = new AdminControllerTitleVM();
            titleModel.titleList = titleRepository.GetAll();
            return View(titleModel);
        }

        public ActionResult EditTitles(int id)
        {
            Title title = new Title();
            TitleRepository titleRepository = new TitleRepository();
            AdminControllerTitleVM titleModel = new AdminControllerTitleVM();

            if (id > 0)
            {
                title = titleRepository.GetById(id);
                titleModel.Title = title.Name;
                titleModel.TitleID = id;
            }

            if (id == 0)
            {
                title.Name = titleModel.Title;
            }
            return View(titleModel);
        }

        [HttpPost]
        public ActionResult EditTitles(int id, AdminControllerTitleVM titleModel)
        {
            TryUpdateModel(titleModel);
            if (ModelState.IsValid)
            {
                Title title = null;
                TitleRepository titleRepository = new TitleRepository();
                if (id > 0)
                {
                    title = titleRepository.GetById(id);
                    title.Name = titleModel.Title;
                    titleRepository.Save(title);
                    return RedirectToAction("ManageTitles");
                }
                else
                {
                    title = titleRepository.GetAll(filter: t => t.Name == titleModel.Title).FirstOrDefault();
                    if (title == null)
                    {
                        title = new Title();
                        title.Name = titleModel.Title;
                        titleRepository.Save(title);
                        return RedirectToAction("ManageTitles");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Title");
                    }
                }
            }
            return View(titleModel);
        }

        public JsonResult DeleteTitle(int id)
        {
            bool TitleInUse = false;
            Title title = new Title();
            TitleRepository titleRepository = new TitleRepository();
            TeacherRepository teacherRepository = new TeacherRepository();
            title = titleRepository.GetById(id);
            Teacher teacher = teacherRepository.GetAll(filter: t => t.Title.Id == id).FirstOrDefault();
            if (teacher == null)
            {
                titleRepository.Delete(title);
            }
            else
            {
                TitleInUse = true;
            }

            //return RedirectToAction("ManageTitles");
            return Json(TitleInUse, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult DeleteTitle(int id)
        //{
        //    Title title = new Title();
        //    TitleRepository titleRepository = new TitleRepository();
        //    TeacherRepository teacherRepository = new TeacherRepository();
        //    title = titleRepository.GetById(id);
        //    if (teacherRepository.GetAll(filter: t => t.TitleID == title.Id) == null)
        //    {
        //        titleRepository.Delete(title);
        //    }
        //    else
        //    {
        //    }

        //    return RedirectToAction("ManageTitles");
        //}
        #endregion

        #region ManageSubjects
        public ActionResult ManageSubjects()
        {
            SubjectRepository subjectRepository = new SubjectRepository();
            AdminControllerSubjectVM subjectModel = new AdminControllerSubjectVM();
            subjectModel.subjectList = subjectRepository.GetAll();

            return View(subjectModel);
        }

        public ActionResult ExportSubjectGrade(int id)
        {
            Subject subject = new Subject();
            SubjectRepository subjectRepository = new SubjectRepository();
            subject = subjectRepository.GetById(id);
            Export export = new Export();
            DataTable gradeTable = export.SubjectGrades(subject);

            GridView Grid = new GridView();
            Grid.DataSource = gradeTable;
            Grid.DataBind();

            string filename = "attachment; filename=" + subject.Name + "_" + ".xls";
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

        public ActionResult EditSubjects(int id)
        {
            Subject subject = new Subject();
            SubjectRepository subjectRepository = new SubjectRepository();
            AdminControllerSubjectVM subjectModel = new AdminControllerSubjectVM();

            if (id > 0)
            {
                subject = subjectRepository.GetById(id);
                subjectModel.Name = subject.Name;
                subjectModel.SubjectID = id;
            }
            if (id == 0)
            {
                subject.Name = subjectModel.Name;
            }
            return View(subjectModel);
        }

        [HttpPost]
        public ActionResult EditSubjects(int id, AdminControllerSubjectVM subjectModel)
        {
            TryUpdateModel(subjectModel);
            if (ModelState.IsValid)
            {

                Subject subject = null;
                SubjectRepository subjectRepository = new SubjectRepository();
                if (id > 0)
                {
                    subject = subjectRepository.GetById(id);
                    subject.Name = subjectModel.Name;
                    subjectRepository.Save(subject);
                    return RedirectToAction("ManageSubjects");
                }
                else
                {
                    subject = subjectRepository.GetAll(filter: s => s.Name == subjectModel.Name).FirstOrDefault();
                    if (subject == null)
                    {
                        subject = new Subject();
                        subject.Name = subjectModel.Name;
                        subjectRepository.Save(subject);
                        return RedirectToAction("ManageSubjects");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid subject name");
                    }
                }
            }
            return View(subjectModel);
        }

        public JsonResult DeleteSubject(int id)
        {
            bool subjectInUse = false;
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            List<Course> courseList = new List<Course>();
            courseList = courseSubjectRepository.GetAll(filter: cs => cs.Subject.Id == id).Select(c => c.Course).ToList();
            if (courseList.Count > 0)
            {
                subjectInUse = true;
            }
            else
            {
                SubjectRepository subjectRepository = new SubjectRepository();
                Subject subject = new Subject();
                subject = subjectRepository.GetById(id);
                subjectRepository.Delete(subject);
            }
            return Json(subjectInUse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManageCourseSubject
        public ActionResult ManageCourseSubject()
        {
            AdminControllerCourseSubjectVM courseSubModel = new AdminControllerCourseSubjectVM();
            SubjectRepository subjectRepository = new SubjectRepository();
            CourseRepository courseRepository = new CourseRepository();
            courseSubModel.courseList = courseRepository.GetAll();
            courseSubModel.subjectList = subjectRepository.GetAll();
            return View(courseSubModel);
        }

        public ActionResult ShowSubjects(int id)
        {
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            courseSubjectModel.courseSubjectList = courseSubjectRepository.GetAll();
            List<Subject> subjectList = new List<Subject>();
            courseSubjectModel.courseID = id;
            foreach (var item in courseSubjectModel.courseSubjectList)
            {
                if (item.Course.Id == id)
                {
                    subjectList.Add(item.Subject);
                }
            }
            courseSubjectModel.subjectList = subjectList;
            return View(courseSubjectModel);
        }

        public ActionResult ShowCourse(int id)
        {
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            courseSubjectModel.courseSubjectList = courseSubjectRepository.GetAll();
            List<Course> courseList = new List<Course>();
            courseSubjectModel.subjectID = id;
            foreach (var item in courseSubjectModel.courseSubjectList)
            {
                if (item.Subject.Id == id)
                {
                    courseList.Add(item.Course);
                }
            }
            courseSubjectModel.courseList = courseList;
            return View(courseSubjectModel);
        }

        public ActionResult EditCourseSubject(int courseID)
        {
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            SubjectRepository subjectRepository = new SubjectRepository();
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository();
            List<Subject> subjectList = courseSubjectRepo.GetAll(filter: cs => cs.Course.Id == courseID).Select(s => s.Subject).ToList();
            courseSubjectModel.subjectList = subjectRepository.GetAll();//Except method is overriden
            List<SelectListItem> List = new List<SelectListItem>();

            foreach (var item in courseSubjectModel.subjectList)
            {
                List.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            courseSubjectModel.ListItems = List;
            courseSubjectModel.subjectList = subjectRepository.GetAll();
            courseSubjectModel.courseID = courseID;
            courseSubjectModel.CourseSubjectID = courseID;
            return View(courseSubjectModel);
        }

        public ActionResult EditSubjectCourse(int id)
        {
            AdminControllerCourseSubjectVM courseSubjectModel = new AdminControllerCourseSubjectVM();
            CourseRepository courseRepository = new CourseRepository();
            courseSubjectModel.courseList = courseRepository.GetAll();

            List<SelectListItem> List = new List<SelectListItem>();
            foreach (var course in courseSubjectModel.courseList)
            {
                List.Add(new SelectListItem() { Text = course.Name, Value = course.Id.ToString() });
            }
            courseSubjectModel.ListItems = List;
            courseSubjectModel.courseList = courseRepository.GetAll();
            courseSubjectModel.subjectID = id;
            courseSubjectModel.CourseSubjectID = id;
            return View(courseSubjectModel);
        }

        public JsonResult CheckForAddedSubjects(int subjectId, int courseId)
        {
            bool isAdded = false;
            CourseSubject courseSubject = new CourseSubject();
            CourseSubjectRepository courseSubjectRepo = new CourseSubjectRepository();
            courseSubject = courseSubjectRepo.GetAll(filter: cs => cs.Course.Id == courseId && cs.Subject.Id == subjectId).FirstOrDefault();
            if (courseSubject != null)
            {
                isAdded = true;
            }
            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        public ActionResult EditSubjectCourse(AdminControllerCourseSubjectVM subjectCourseModel)
        {
            CourseSubject courseSubject = new CourseSubject();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            TryUpdateModel(subjectCourseModel);
            if (ModelState.IsValid && subjectCourseModel.CourseSubjectID > 0)
            {
                courseSubject.CourseID = subjectCourseModel.courseID;
                courseSubject.SubjectID = subjectCourseModel.subjectID;
                courseSubjectRepository.Save(courseSubject);
                return RedirectToAction("ShowCourse", "Admin", new { @id = subjectCourseModel.subjectID });
            }
            if (subjectCourseModel.ListItems == null)
            {
                List<SelectListItem> List = new List<SelectListItem>();
                CourseRepository courseRepository = new CourseRepository();
                subjectCourseModel.courseList = courseRepository.GetAll();
                foreach (var item in subjectCourseModel.courseList)
                {
                    List.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                subjectCourseModel.ListItems = List;
            }
            return View(subjectCourseModel);
        }

        [HttpPost]
        public ActionResult EditCourseSubject(AdminControllerCourseSubjectVM courseSubjectModel)
        {
            CourseSubject courseSubject = new CourseSubject();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            TryUpdateModel(courseSubjectModel);
            if (ModelState.IsValid && courseSubjectModel.CourseSubjectID > 0)
            {
                courseSubject.CourseID = courseSubjectModel.courseID;
                courseSubject.SubjectID = courseSubjectModel.subjectID;
                courseSubjectRepository.Save(courseSubject);
                return RedirectToAction("ShowSubjects", "Admin", new { @id = courseSubjectModel.courseID });
            }
            if (courseSubjectModel.ListItems == null)
            {
                List<SelectListItem> List = new List<SelectListItem>();
                SubjectRepository subjectRepository = new SubjectRepository();
                courseSubjectModel.subjectList = subjectRepository.GetAll();
                foreach (var item in courseSubjectModel.subjectList)
                {
                    List.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
                }
                courseSubjectModel.ListItems = List;
            }
            return View(courseSubjectModel);
        }

        public ActionResult DeleteCourseSubject(int id, int courseId)
        {
            CourseSubject courseSubject = new CourseSubject();
            CourseSubjectRepository courseSubjectRepository = new CourseSubjectRepository();
            courseSubject = courseSubjectRepository.GetAll(filter: s => s.SubjectID == id && s.Course.Id == courseId).FirstOrDefault();
            courseSubjectRepository.Delete(courseSubject);
            return RedirectToAction("ShowSubjects", "Admin", new { @id = courseId });
        }

        #endregion
    }
}
