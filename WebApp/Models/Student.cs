using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Display(Name = "學生姓名")]
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public ICollection<WebApp.Models.Enrollment> Enrollment { get; set; }
    }
}