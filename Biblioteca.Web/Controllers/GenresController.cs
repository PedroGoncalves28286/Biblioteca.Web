using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // GET: Genres
        public IActionResult Index()
        {
            return View(_genreRepository.GetAll().OrderBy(n => n.Name));
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _genreRepository.GetByIdAsync(id.Value);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreRepository.CreateAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _genreRepository.GetByIdAsync(id.Value);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _genreRepository.UpdateAsync(genre);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _genreRepository.ExistAsync(genre.Id))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _genreRepository.GetByIdAsync(id.Value);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            await _genreRepository.DeleteAsync(genre);
            return RedirectToAction(nameof(Index));
        }

    }
}
