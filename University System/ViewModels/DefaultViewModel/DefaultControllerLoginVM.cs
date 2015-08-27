using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using University_System.Entities;

namespace University_System.ViewModels.DefaultViewModel
{
    public class DefaultControllerLoginVM
    {
        [Required(ErrorMessage ="Input username")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Input password")]
        public string Password { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select a correct UserType")]
        public User.UserType UserType { get; set; }
    }
}