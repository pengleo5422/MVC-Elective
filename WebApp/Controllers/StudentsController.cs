using System;
using System.Linq;
using System.Threading.Tasks;
using LearnASPNETCoreMVC5.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;




namespace WebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly WebAppContext _context;

        public StudentsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Students
        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind("ID,Name,Account,Password")] String Password,String Account)
        {
            var member = _context.Students
                .Where(m => m.Account == Account && m.Password == Password)
                .FirstOrDefault();

            //驗證密碼
            if (member == null)
            {
                ViewBag.message = "帳密錯誤，登入失敗";
                return View();
            }
            HttpContext.Session.SetString("Welcome", "學生 "+member.Name);
            HttpContext.Session.SetInt32("member", member.ID);
            HttpContext.Session.SetString("Student", "學生 ");
            if (member.Account.Contains("B")) {
                return RedirectToAction("Pindex", "Teaches");
            }
            else
                return RedirectToAction("index", "Teaches");
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("sindex", "Teaches");
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            var Teacher = HttpContext.Session.GetString("Teacher");
            if (Teacher == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

             var student = await _context.Students
            .Include(s => s.Enrollment)
                .ThenInclude(e => e.Teach)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        public async Task<IActionResult> PDetails()
        {
            var Teacher = HttpContext.Session.GetString("Student");
            if (Teacher == null)
            {
                return NotFound();
            }
            var Member = HttpContext.Session.GetInt32("member");
            if (Member == null)
            {
                return NotFound();
            }

            var student = await _context.Students
           .Include(s => s.Enrollment)
               .ThenInclude(e => e.Teach)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == Member);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        public async Task<IActionResult> Curriculum()
        {
            var Teacher = HttpContext.Session.GetString("Student");
            if (Teacher == null)
            {
                return NotFound();
            }
            var Member = HttpContext.Session.GetInt32("member");
            if (Member == null)
            {
                return NotFound();
            }

            var student = await _context.Students
           .Include(s => s.Enrollment)
               .ThenInclude(e => e.Teach)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == Member);
            
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            
           return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Account,Password")] Student student)
        {
            if (ModelState.IsValid)
            {
                var stu = _context.Students
                         .Where(m => m.Account == student.Account)
                         .FirstOrDefault();
                if (stu == null)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
                ViewBag.message = "此帳號已有人使用註冊失敗";
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit()
        {
            var Member = HttpContext.Session.GetInt32("member");
            if (Member == null)
            {
                return NotFound();
            }
            
            var student = await _context.Students
            .Include(s => s.Enrollment)
                .ThenInclude(e => e.Teach)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == Member);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("ID,Name,Account,Password")] Student student)
        {
            var Member = HttpContext.Session.GetInt32("member");
            student.ID = (int)Member;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.message = "修改成功";
                

            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
