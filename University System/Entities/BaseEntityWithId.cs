using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class BaseEntityWithId
    {
        [Key]
        public int Id { get; set; }
    }
}