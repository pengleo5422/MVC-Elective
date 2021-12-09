using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Teacher
    {
        public int ID { get; set; }
        [Display(Name = "教師姓名")]
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public ICollection<WebApp.Models.CourseAssignment> CourseAssignment { get; set; }
    }
}
