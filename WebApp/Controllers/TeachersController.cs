using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly WebAppContext _context;

        public TeachersController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind("ID,Name,Account,Password")] String Password, String Account)
        {
            var member = _context.Teacher
                .Where(m => m.Account == Account && m.Password == Password)
                .FirstOrDefault();

            //驗證密碼
            if (member == null)
            {
                ViewBag.message = "帳密錯誤，登入失敗";
                return View();
            }
            HttpContext.Session.SetString("Welcome", "教師 " + member.Name);
            HttpContext.Session.SetInt32("member", member.ID);
            HttpContext.Session.SetString("Name", member.Name);
            HttpContext.Session.SetString("Teacher", "teacher");
            return RedirectToAction("index", "Teaches");
        }
        public async Task<IActionResult> PDetails()
        {
            var Member = HttpContext.Session.GetInt32("member");
            var Teacher = HttpContext.Session.GetString("Teacher");
            if (Teacher == null)
            {
                return NotFound();
            }
            if (Member == null)
            {
                return NotFound();
            }
            
            var teacher = await _context.Teacher
           .Include(s => s.CourseAssignment)
               .ThenInclude(e => e.Teach)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == Member);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }
        public async Task<IActionResult> Index()
        {
            var Teacher = HttpContext.Session.GetString("Teacher");
            if (Teacher == null)
            {
                return NotFound();
            }
            return View(await _context.Teacher.ToListAsync());
        }

        // GET: Teachers/Details/5
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

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }
        public async Task<IActionResult> List(int? id)
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

            var teacher =  await _context.Teach
                            .Include(s => s.Enrollment)
               .ThenInclude(e => e.Students)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }
        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Account,Password")] Teacher teacher)
        {
            
            if (ModelState.IsValid)
            {

                var stu = _context.Teacher
                         .Where(m => m.Account == teacher.Account)
                         .FirstOrDefault();
                if (stu == null)
                {
                    _context.Add(teacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
                ViewBag.msg = "此帳號已有人使用註冊失敗";
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit()
        {
            var Teacher = HttpContext.Session.GetString("Teacher");
            var Member = HttpContext.Session.GetInt32("member");
            if (Teacher == null)
            {
                return NotFound();
            }
            if (Member == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(Member);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID,Name,Account,Password")] Teacher teacher)
        {
            var Member = HttpContext.Session.GetInt32("member");
            teacher.ID = (int)Member;
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.ID))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
