using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using University_System.Entities;
using University_System.Repositories;

namespace University_System.CustomAttribute
{
    public class Export
    {
        public DataTable SubjectGrades(Subject subject)
        {
            List<Grade> gradeList = new List<Grade>();
            GradeRepository gradeRepository = new GradeRepository();
            gradeList = gradeRepository.GetAll(filter: s => s.Subject.Id == subject.Id);
            StudentRepository studentRepository = new StudentRepository();
            var gradeTable = new DataTable("SubjectsGrades");
            gradeTable.Columns.Add("StudentName",typeof(string));
            gradeTable.Columns.Add("GradeValue", typeof(string));
            Dictionary<string, string> details = new Dictionary<string, string>();
            List<string> gradeValues = new List<string>();
            List<int> studentList = new List<int>();
            foreach (var item in gradeList)
            {
                studentList.Add(item.Student.Id);
            }
            studentList = studentList.Distinct().ToList();
            foreach (var item in studentList)
            {
                StringBuilder sb = new StringBuilder();
                List<Grade> grades = new List<Grade>();
                grades = gradeRepository.GetAll(filter: s => s.Student.Id == item && s.Subject.Id == subject.Id);
                foreach (var grade in grades)
                {
                    sb.Append(grade.GradeValue);
                    sb.Append(",");
                }
                sb.Length -= 1;
                Student student = new Student();
                student = studentRepository.GetAll(filter: s=> s.Id == item).FirstOrDefault();
                string fullName = student.FirstName + " " + student.LastName;
                details.Add(fullName, sb.ToString());
            }

            foreach (var item in details)
            {
                gradeTable.Rows.Add(item.Key, item.Value);
            }
            return gradeTable;
        }
        public DataTable StudentsGradesToExcel(Student student)
        {
            List<Grade> gradeList = new List<Grade>();
            GradeRepository gradeRepository = new GradeRepository();
            gradeList = gradeRepository.GetAll(filter: g => g.Student.Id == student.Id);
            var gradeTable = new DataTable("StudentsGrades");
            gradeTable.Columns.Add("SubjectName", typeof(string));
            gradeTable.Columns.Add("GradeValue", typeof(string));
            Dictionary<string, string> details = new Dictionary<string, string>();
            List<string> subjectNameList = new List<string>();
            List<string> gradeValues = new List<string>();
            foreach (var item in gradeList)
            {
                subjectNameList.Add(item.Subject.Name);
            }
            subjectNameList = subjectNameList.Distinct().ToList();
            foreach (var item in subjectNameList)
            {
                StringBuilder sb = new StringBuilder();
                List<Grade> grades = new List<Grade>();
                grades = gradeRepository.GetAll(filter: s => s.Subject.Name == item && s.Student.Id == student.Id);
                foreach (var grade in grades)
                {
                    sb.Append(grade.GradeValue);
                    sb.Append(",");
                }
                sb.Length -= 1;
                details.Add(item, sb.ToString());
            }
            foreach (var item in details)
            {
                gradeTable.Rows.Add(item.Key, item.Value);
            }
            return gradeTable;
        }
    }
}