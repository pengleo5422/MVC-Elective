using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CourseAssignmentsController : Controller
    {
        private readonly WebAppContext _context;

        public CourseAssignmentsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: CourseAssignments
        public async Task<IActionResult> Index()
        {
            var webAppContext = _context.CourseAssignment.Include(c => c.Teach).Include(c => c.Teachers);
            return View(await webAppContext.ToListAsync());
        }

        // GET: CourseAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignment
                .Include(c => c.Teach)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(m => m.CourseAssignmentID == id);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // GET: CourseAssignments/Create
        public IActionResult Create()
        {
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "Course");
            ViewData["TeacherID"] = new SelectList(_context.Set<Teacher>(), "ID", "ID");
            return View();
        }

        // POST: CourseAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseAssignmentID,TeacherID,TeachID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "ClassDate", courseAssignment.TeachID);
            ViewData["TeacherID"] = new SelectList(_context.Set<Teacher>(), "ID", "ID", courseAssignment.TeacherID);
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignment.FindAsync(id);
            if (courseAssignment == null)
            {
                return NotFound();
            }
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "ClassDate", courseAssignment.TeachID);
            ViewData["TeacherID"] = new SelectList(_context.Set<Teacher>(), "ID", "ID", courseAssignment.TeacherID);
            return View(courseAssignment);
        }

        // POST: CourseAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseAssignmentID,TeacherID,TeachID")] CourseAssignment courseAssignment)
        {
            if (id != courseAssignment.CourseAssignmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseAssignmentExists(courseAssignment.CourseAssignmentID))
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
            ViewData["TeachID"] = new SelectList(_context.Teach, "ID", "ClassDate", courseAssignment.TeachID);
            ViewData["TeacherID"] = new SelectList(_context.Set<Teacher>(), "ID", "ID", courseAssignment.TeacherID);
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAssignment = await _context.CourseAssignment
                .Include(c => c.Teach)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(m => m.CourseAssignmentID == id);
            if (courseAssignment == null)
            {
                return NotFound();
            }

            return View(courseAssignment);
        }

        // POST: CourseAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseAssignment = await _context.CourseAssignment.FindAsync(id);
            _context.CourseAssignment.Remove(courseAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseAssignmentExists(int id)
        {
            return _context.CourseAssignment.Any(e => e.CourseAssignmentID == id);
        }
        public async Task<IActionResult> Rmove(int id)
        {
            var _CourseAssignment = await _context.CourseAssignment
                    .Include(CourseAssignment => CourseAssignment.Teachers)
                    .Include(CourseAssignment => CourseAssignment.Teach)

                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.CourseAssignmentID == id);
            var enrollment = await _context.CourseAssignment.FindAsync(id);
            _context.CourseAssignment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToRoute(new { controller = "Teachers", action = "PDetails" });
        }
    }
}
