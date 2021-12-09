using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace WebApp.Controllers
{
    public class TeachesController : Controller
    {
        private readonly WebAppContext _context;

        public TeachesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Teaches
        public async Task<IActionResult> Index(string searchString)
        {
            IQueryable<Teach> teachs = from m in _context.Teach
                                       select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                teachs = teachs.Where(s => s.Course.Contains(searchString));
            }

            return View(await teachs.ToListAsync());
        }
        public IActionResult Page()
        {


            return View();
        }
        public async Task<IActionResult> Sindex(int id, int? pageNumber, string currentFilter, string sortOrder, string MainContent_Course, string MainContent_TeacherName, string MainContent_Date, string MainContent_Rating, string MainContent_Category)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["CourseSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewData["WeekSortparm"] = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";

            var teachs = from m in _context.Teach
                         select m;

            if (id == 98)
            {
                TempData["error"] = "98";
            }
            else if (id == 99)
            {
                TempData["error"] = "99";
            }
            else if (id == 97)
            {
                TempData["error"] = "97";
            }

            if (MainContent_Course != null || MainContent_TeacherName != null || MainContent_Date != null || MainContent_Rating != null || MainContent_Category != null)
            {
                pageNumber = 1;
            }
            else
            {
                MainContent_Course = currentFilter;
            }
            if (!String.IsNullOrEmpty(MainContent_Course))
            {
                teachs = teachs.Where(s => s.Course.Contains(MainContent_Course));
            }
            if (!String.IsNullOrEmpty(MainContent_TeacherName))
            {
                teachs = teachs.Where(s => s.Name.Contains(MainContent_TeacherName));
            }
            if (!String.IsNullOrEmpty(MainContent_Date))
            {
                teachs = teachs.Where(s => s.ClassDate.Contains(MainContent_Date));
            }
            if (!String.IsNullOrEmpty(MainContent_Rating))
            {
                teachs = teachs.Where(s => s.Rating.Contains(MainContent_Rating));
            }
            if (!String.IsNullOrEmpty(MainContent_Category))
            {
                teachs = teachs.Where(s => s.Category.Contains(MainContent_Category));
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-tw");


            switch (sortOrder)
            {
                case "name_desc":
                    teachs = teachs.OrderBy(s => s.Course);
                    break;
                case "rating_desc":
                    teachs = teachs.OrderBy(s => s.Rating);
                    break;
                case "Date_desc":
                    teachs = teachs.OrderBy(s => s.ClassDate);
                    break;
                default:
                    teachs = teachs.OrderBy(s => s.ID);
                    break;
            }
            int pageSize = 9;
            return View(await PaginatedList<Teach>.CreateAsync(teachs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Pindex(int id, int? pageNumber, string currentFilter, string sortOrder, string MainContent_Course, string MainContent_TeacherName, string MainContent_Date, string MainContent_Rating, string MainContent_Category)
        {
            var Member = HttpContext.Session.GetInt32("member");
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CourseSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewData["WeekSortparm"] = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            IQueryable<Teach> teachs = from m in _context.Teach
                                       select m;

            var student = await _context.Students
           .Include(s => s.Enrollment)
               .ThenInclude(e => e.Teach)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == Member);
            if (MainContent_Course != null || MainContent_TeacherName != null || MainContent_Date != null || MainContent_Rating != null || MainContent_Category != null)
            {
                pageNumber = 1;
            }
            else
            {
                MainContent_Course = currentFilter;
            }
            if (!String.IsNullOrEmpty(MainContent_Course))
            {
                teachs = teachs.Where(s => s.Course.Contains(MainContent_Course));
            }
            if (!String.IsNullOrEmpty(MainContent_TeacherName))
            {
                teachs = teachs.Where(s => s.Name.Contains(MainContent_TeacherName));
            }
            if (!String.IsNullOrEmpty(MainContent_Date))
            {
                teachs = teachs.Where(s => s.ClassDate.Contains(MainContent_Date));
            }
            if (!String.IsNullOrEmpty(MainContent_Rating))
            {
                teachs = teachs.Where(s => s.Rating.Contains(MainContent_Rating));
            }
            if (!String.IsNullOrEmpty(MainContent_Category))
            {
                teachs = teachs.Where(s => s.Category.Contains(MainContent_Category));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    teachs = teachs.OrderBy(s => s.Course);
                    break;
                case "rating_desc":
                    teachs = teachs.OrderBy(s => s.Rating);
                    break;
                case "Date_desc":
                    teachs = teachs.OrderBy(s => s.ClassDate);
                    break;
                default:
                    teachs = teachs.OrderBy(s => s.ID);
                    break;
            }

            if (id == 98)
            {
                TempData["error"] = "98";
            }
            else if (id == 99)
            {
                TempData["error"] = "99";
            }
            else if (id == 97)
            {
                TempData["error"] = "97";
            }
            int pageSize = 9;
            var departmentgenres = new Department_Genres
            {
                teachs = await PaginatedList<Teach>.CreateAsync(teachs.AsNoTracking(), pageNumber ?? 1, pageSize),
                MyCourse = student
            };



            return View(departmentgenres);
        }


        // GET: Teaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teach = await _context.Teach
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teach == null)
            {
                return NotFound();
            }

            return View(teach);
        }

        // GET: Teaches/Create
        public IActionResult Create()
        {
            var Teacher = HttpContext.Session.GetString("Teacher");
            if (Teacher == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Teaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Course,ClassDate,Classroom,Rating,Category,Attend,credit")] Teach teach)
        {


            if (ModelState.IsValid)
            {

                _context.Add(teach);
                await _context.SaveChangesAsync();
                var Member = HttpContext.Session.GetInt32("member");
                var CC = new CourseAssignment { TeacherID = (int)Member, TeachID = teach.ID };
                _context.Add(CC);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(teach);
        }


        // GET: Teaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var teach = await _context.Teach.FindAsync(id);
            if (teach == null)
            {
                return NotFound();
            }
            return View(teach);
        }

        // POST: Teaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Course,ClassDate,Classroom,Rating,Category,Attend,credit")] Teach teach)
        {
            if (id != teach.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachExists(teach.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teach);
        }


        // GET: Teaches/Delete/5
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

            var teach = await _context.Teach
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teach == null)
            {
                return NotFound();
            }

            return View(teach);
        }

        // POST: Teaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var te = _context.CourseAssignment
                .Where(m => m.TeachID == id)
                .FirstOrDefault();
            if (te != null)
            {
                _context.CourseAssignment.Remove(te);
            }
            var teach = await _context.Teach.FindAsync(id);
            _context.Teach.Remove(teach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teach = await _context.Teach
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teach == null)
            {
                return NotFound();
            }

            return View(teach);
        }
        public async Task<IActionResult> sdindex(int? pageNumber, string currentFilter, int id)
        {
            var Member = HttpContext.Session.GetInt32("member");

            IQueryable<Teach> teachs = from m in _context.Teach
                                       select m;

            var student = await _context.Students
           .Include(s => s.Enrollment)
               .ThenInclude(e => e.Teach)
           .AsNoTracking()
           .FirstOrDefaultAsync(m => m.ID == Member);
            if (id == 98)
            {
                TempData["error"] = "98";
            }
            else if (id == 99)
            {
                TempData["error"] = "99";
            }
            else if (id == 97)
            {
                TempData["error"] = "97";
            }
            int pageSize = 9;
            var departmentgenres = new Department_Genres
            {
                teachs = await PaginatedList<Teach>.CreateAsync(teachs.AsNoTracking(), pageNumber ?? 1, pageSize),
                MyCourse = student
            };

            return View(departmentgenres);
        }
        [HttpPost]
        public JsonResult GetCarOwners( string MainContent_Course, string MainContent_TeacherName, string MainContent_Date, string MainContent_Rating, string MainContent_Category)
        {
            
            IQueryable<Teach> teachs = from m in _context.Teach
                                       select m;

            List<Teach> teacs = new List<Teach> { };
            if (!string.IsNullOrEmpty(MainContent_Course))
            {
                 teacs = teachs.Where(s => s.Course.Contains(MainContent_Course)).ToList();
            }
            if (!string.IsNullOrEmpty(MainContent_TeacherName))
            {
                 teacs = teachs.Where(s => s.Name.Contains(MainContent_TeacherName)).ToList();
            }
            if (!string.IsNullOrEmpty(MainContent_Date))
            {
                teacs = teachs.Where(s => s.ClassDate.Contains(MainContent_Date)).ToList();
            }
            if (!string.IsNullOrEmpty(MainContent_Rating))
            {
                teacs = teachs.Where(s => s.Rating.Contains(MainContent_Rating)).ToList();
            }
            if (!string.IsNullOrEmpty(MainContent_Category))
            {
                teacs = teachs.Where(s => s.Category.Contains(MainContent_Category)).ToList();
            }
            return Json(teacs);
        }
        private bool TeachExists(int id)
        {
            return _context.Teach.Any(e => e.ID == id);
        }

    }
}
