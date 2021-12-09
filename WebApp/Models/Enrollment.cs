using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public int TeachID { get; set; }
        public Teach Teach { get; set; }
        public Student Students { get; set; }
    }
}
