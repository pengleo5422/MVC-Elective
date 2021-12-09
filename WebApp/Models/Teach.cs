using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Teach
    { 
        public int ID { get; set; }
        [Display(Name = "教師姓名")]
        public string Name { get; set; }
        [Display(Name = "課程")]
        public string Course { get; set; }
        [Display(Name = "時段")]

        [RegularExpression(@"^[A-Z]+[A-Z]")]
        [Required]
        public string ClassDate { get; set; }
        [Display(Name = "教室")]

        public string Classroom { get; set; }
        [Display(Name = "星期")]
        public string Rating { get; set; }
        [Display(Name = "類別")]
        public string Category { get; set; }
        [Display(Name = "選課")]
        public string Attend { get; set; }
        [Display(Name = "學分")]
        public int credit { get; set; }
        public ICollection<WebApp.Models.Enrollment> Enrollment { get; set; }
        public ICollection<WebApp.Models.CourseAssignment> CourseAssignment { get; set; }

        
    }
}
