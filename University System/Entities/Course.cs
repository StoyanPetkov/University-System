using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class Course : BaseEntityWithId
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }
}