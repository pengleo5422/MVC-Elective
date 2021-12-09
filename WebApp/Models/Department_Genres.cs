using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.Models
{
    public class Department_Genres
    {
        public PaginatedList<Teach> teachs { get; set; }
        public Student MyCourse { get; set; }
    }
}
