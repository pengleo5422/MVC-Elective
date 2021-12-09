using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        public DbSet<WebApp.Models.Teach> Teach { get; set; }
        public DbSet<WebApp.Models.Enrollment> Enrollment{ get; set; }
        public DbSet<WebApp.Models.Student> Students{ get; set; }
        public DbSet<WebApp.Models.CourseAssignment> CourseAssignment { get; set; }
        public DbSet<WebApp.Models.Teacher> Teacher { get; set; }
       
    }
    
}
