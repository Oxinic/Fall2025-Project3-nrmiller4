using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_nrmiller4.Data;
using Fall2025_Project3_nrmiller4.Models;

namespace Fall2025_Project3_nrmiller4.Controllers
{
    public class MovieActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieActors
        public async Task<IActionResult> Index()
        {
            var movieActors = await _context.MovieActors
                .Include(ma => ma.Movie)
                .Include(ma => ma.Actor)
                .ToListAsync();
            return View(movieActors);
        }

        // GET: MovieActors/Create
        public IActionResult Create()
        {
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: MovieActors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,ActorId")] MovieActor movieActor)
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Movie");
            ModelState.Remove("Actor");

            // Check for duplicate relationship
            var exists = await _context.MovieActors
                .AnyAsync(ma => ma.MovieId == movieActor.MovieId && ma.ActorId == movieActor.ActorId);

            if (exists)
            {
                ModelState.AddModelError("", "This actor is already associated with this movie.");
                ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", movieActor.ActorId);
                ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieActor.MovieId);
                return View(movieActor);
            }

            if (ModelState.IsValid)
            {
                _context.Add(movieActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", movieActor.ActorId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieActor.MovieId);
            return View(movieActor);
        }

        // GET: MovieActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieActor = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Include(ma => ma.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieActor == null)
            {
                return NotFound();
            }

            return View(movieActor);
        }

        // POST: MovieActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieActor = await _context.MovieActors.FindAsync(id);
            if (movieActor != null)
            {
                _context.MovieActors.Remove(movieActor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
