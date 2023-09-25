using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IBookRepository _bookRepository;

        public GenresController(IGenreRepository genreRepository, IBookRepository bookRepository)
        {
            _genreRepository = genreRepository;
            _bookRepository = bookRepository;
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
        public async Task<IActionResult> Edit(int id, Genre genre)
        {
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

        public async Task<IActionResult> ViewBooks(string genreName)
        {
            if (string.IsNullOrEmpty(genreName))
            {
                return NotFound();
            }

            // Retrieving the genre by name
            var genre = await _genreRepository.GetGenreByNameAsync(genreName);

            if (genre == null)
            {
                return NotFound();
            }

            // Retrieving the books associated with the genre
            var books = await _bookRepository.GetBooksByGenreAsync(genreName);

            // Create a view model to pass the genre and its associated books to the view
            var viewModel = new GenreBooksViewModel
            {
                Genre = genre, // Pass the genre entity
                Books = books.Select(book => new BookViewModel
                {
                    // Mapping book properties to BookViewModel properties as needed
                    Title = book.Title,
                    Author = book.Author,
                    // Other properties as needed
                }).ToList()
            };

            return View(viewModel);
        }


    }
}
