using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.AdminViewModel
{
    public class AdminControllerTitleVM
    {
        public int TitleID { get; set; }
        [Required(ErrorMessage ="Input title")]
        public string Title { get; set; }
        public List<Title> titleList { get; set; }
    }
}