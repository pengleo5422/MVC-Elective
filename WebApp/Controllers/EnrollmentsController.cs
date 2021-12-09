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
    public class EnrollmentsController : Controller
    {
        private readonly WebAppContext _context;

        public EnrollmentsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.Enrollment.Include(e => e.Students).Include(e => e.Teach);
            return View(await webAppContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Students)
                .Include(e => e.Teach)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID");
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "Course");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentID,StudentID,TeachID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "Course", enrollment.TeachID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "Course", enrollment.TeachID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentID,StudentID,TeachID")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentID))
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
            ViewData["StudentID"] = new SelectList(_context.Students, "ID", "ID", enrollment.StudentID);
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "Course", enrollment.TeachID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var Teacher = HttpContext.Session.GetString("Student");
            if (Teacher == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Students)
                .Include(e => e.Teach)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Add1([Bind("EnrollmentID,StudentID,TeachID")] int id)
                {
                    var Teacher = HttpContext.Session.GetString("Student");
                    if (Teacher == null)
                    {
                        return NotFound();
                    }
            var Member = HttpContext.Session.GetInt32("member");        
                    var _Enrollment = await _context.Enrollment
                                    .Where(Enrollment => Enrollment.StudentID ==Member)
                                    .Include(Enrollment => Enrollment.Students)
                                    .Include(Enrollment => Enrollment.Teach)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
                    var student = await _context.Students
                                .Include(s => s.Enrollment)
                                    .ThenInclude(e => e.Teach)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.ID == Member);
                  
            int tcr = 0;
                    Teach _teach = _context.Teach.Find(id);
                    foreach (var ch in student.Enrollment)
                    {
                        tcr = tcr + ch.Teach.credit;
                    }
                    if (tcr+_teach.credit > 25)
                    {

                        return RedirectToAction("Pindex", "Teaches", new { id = 98 });
                    }
            
                  if (_Enrollment != null)
                    {
                    foreach(var AD in _context.Enrollment.Where(Enrollment => Enrollment.StudentID == Member)) {
                    if (AD.TeachID == id)
                    {
                        return RedirectToAction("Pindex", "Teaches", new { id = 99 });
                    }

                        }
                    char[] classDate = _teach.ClassDate.ToCharArray();
                    string rating = _teach.Rating;
                    foreach (var ch in classDate)
                    {
                    foreach (var item in _context.Teach)
                    {
                        if (item.Enrollment != null)
                        {
                            foreach (var cc in item.Enrollment)
                            {
                                if (item.ClassDate.Contains(ch) && item.Rating == rating && cc.StudentID == Member)
                                {
                                    return RedirectToAction("Pindex", "Teaches", new { id = 97 });
                                }
                            }
                        }
                    }
                    }

                var CC = new Enrollment { StudentID = (int)Member, TeachID = id };
                        _context.Add(CC);
                        if (ModelState.IsValid)
                        {
                            _context.Entry(_teach).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                    else {
                        var CC = new Enrollment { StudentID = (int)Member, TeachID = id };
                        _context.Add(CC);
                        if (ModelState.IsValid)
                        {
                            _context.Entry(_teach).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                tcr = 0;
                return RedirectToAction("Pindex", "Teaches");
                }
        public async Task<IActionResult> Rmove(int id)
        {
            var Teacher = HttpContext.Session.GetString("Student");
            if (Teacher == null)
            {
                return NotFound();
            }
            var _Enrollment = await _context.Enrollment
                    .Include(Enrollment => Enrollment.Students)
                    .Include(Enrollment => Enrollment.Teach)

                    .AsNoTracking()
                    .FirstOrDefaultAsync(m=>m.EnrollmentID==id);
            var enrollment = await _context.Enrollment.FindAsync(id);
            _Enrollment.Teach.Attend = null;
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToRoute(new { controller = "Students", action = "Curriculum"});
        }
 
        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.EnrollmentID == id);
        }
    }
}
