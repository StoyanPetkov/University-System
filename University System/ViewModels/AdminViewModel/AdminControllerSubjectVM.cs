using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerSubjectVM
    {
        public int SubjectID { get; set; }
        [Required(ErrorMessage ="Input subject name")]
        public string Name { get; set; }
        public List<Subject> subjectList{ get; set; }
    }
}