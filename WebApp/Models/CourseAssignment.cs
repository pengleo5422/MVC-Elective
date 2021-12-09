using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class CourseAssignment
    {
        public int CourseAssignmentID { get; set; }
        public int TeacherID { get; set; }
        public int TeachID { get; set; }
        public Teach Teach { get; set; }
        public Teacher Teachers { get; set; }
    }
}
