using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IGenreRepository _genreRepository;
        private readonly DataContext _context;

        public BooksController(IBookRepository bookRepository, IUserHelper userHelper,
            IConverterHelper converterHelper,
             IBlobHelper blobHelper, IGenreRepository genreRepository,
             DataContext context)
        {
            _bookRepository = bookRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _genreRepository = genreRepository;
            _context = context;
        }

        // GET: Books
        public IActionResult Index(string search)
        {
            return View(_context.Books.Where(x => x.Title.Contains(search) || x.Author.Contains(search) || x.GenreName.Contains(search)
            || x.ISBN.Contains(search) || x.Publisher.Contains(search) || x.BookId.ToString().Contains(search) || search == null).ToList());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(book);
        }

        // GET: Books/Create
        [RoleAuthorization("Admin", "Staff", "Reader")]
        public IActionResult Create()
        {
            var genres = _genreRepository.GetAll().ToList();
            ViewBag.Genres = new SelectList(genres, "Name", "Name");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Admin", "Staff", "Reader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid coverId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    coverId = await _blobHelper.UploadBlobAsync(model.ImageFile, "covers");

                }

                var book = _converterHelper.ToBook(model, coverId, true);

                var existingBookWithSameBookId = await _bookRepository.GetBookByBookIdAsync(model.BookId);
                if (existingBookWithSameBookId != null && existingBookWithSameBookId.Id != model.Id)
                {
                    ModelState.AddModelError("BookId", "A book with the same ISBN already exists.");
                    // Repopulate the genre dropdown if needed
                    var genres = _genreRepository.GetAll().ToList();
                    ViewBag.Genres = new SelectList(genres, "Name", "Name");
                    return View(model);
                }

                // Genre name based on the user's selection
                book.GenreName = model.GenreName;

                book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _bookRepository.CreateAsync(book);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: Books/Edit/5
        [RoleAuthorization("Staff", "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToBookViewModel(book);

            // Retrieve the book's genre and set it in the model
            var bookGenre = await _genreRepository.GetGenreByNameAsync(book.GenreName);
            if (bookGenre != null)
            {
                model.GenreName = bookGenre.Name;
            }

            // Ensure that the "IsAvailable" property in the model matches the book's availability status
            model.IsAvailable = book.IsAvailable;

            // Retrieve the list of genres and update ViewBag.Genres
            var genres = _genreRepository.GetAll().ToList();
            ViewBag.Genres = new SelectList(genres, "Name", "Name");

            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Staff", "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    Guid coverId = model.CoverId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        coverId = await _blobHelper.UploadBlobAsync(model.ImageFile, "covers");
                    }

                    var book = await _bookRepository.GetBookByIdAsync(model.Id);

                    if (book == null)
                    {
                        return NotFound(); // Handle the case where the book is not found
                    }

                    //if (_context.Books.Any(a => a.BookId.Equals(model.BookId)))
                    //{
                    //    ModelState.AddModelError("BookId ", "Book already exists!!");
                    //}

                    // Check for duplicate ISBN
                    var existingBookWithSameBookId = await _bookRepository.GetBookByBookIdAsync(model.BookId);
                    if (existingBookWithSameBookId != null && existingBookWithSameBookId.Id != model.Id)
                    {
                        ModelState.AddModelError("BookId", "A book with the same Id already exists.");
                        // Repopulate the genre dropdown if needed
                        var genres = _genreRepository.GetAll().ToList();
                        ViewBag.Genres = new SelectList(genres, "Name", "Name");
                        return View(model);
                    }

                    // Update genre name based on the user's selection
                    book.GenreName = model.GenreName;
                    book.CoverId = coverId;
                    book.Author = model.Author;
                    book.Title = model.Title;
                    book.BookId = model.BookId;
                    book.Borrower = model.Borrower;
                    book.ISBN = model.ISBN;
                    book.Publisher = model.Publisher;
                    book.LoanLimitQuantity = model.LoanLimitQuantity;

                    // Update the availability status based on the checkbox value
                    book.IsAvailable = model.IsAvailable;

                    book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Check for concurrency exception based on the book's BookId
                    if (!await _bookRepository.ExistAsync(model.BookId))
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

            // If there are validation errors, repopulate the genre dropdown
            var genreList = _genreRepository.GetAll().ToList();
            ViewBag.Genres = new SelectList(genreList, "Name", "Name");

            //// Check if the selected genre already exists
            //var selectedGenre = await _genreRepository.GetGenreByNameAsync(model.GenreName);
            //if (selectedGenre == null)
            //{
            //    // If the genre doesn't exist, you can choose how to handle it
            //    // For example, you could create a new genre here or show an error message
            //    ModelState.AddModelError("GenreName", "The selected genre does not exist.");
            //    return View(model);
            //}

            //// Update the book's genre reference to the existing genre
            //model.GenreId = selectedGenre.Id;

            return View(model);
        }


        // GET: Books/Delete/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var genres = _genreRepository.GetAll().ToList();
            ViewBag.Genres = new SelectList(genres, "Name", "Name");

            return View(book);
        }

        // POST: Books/Delete/5
        [RoleAuthorization("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            try
            {
                await _bookRepository.DeleteAsync(book);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorTitle = $"{book.Title} is probably being used";
                ViewBag.ErrorMessage = $"{book.Title} cannot be deleted since there are ongoing lends.</br></br> " +
                    $"Try deleting the ongoing orders first and come back again ";
                return View("Error");
            }
        }

        [HttpGet]
        [Route("Books/CheckAvailability/{bookId}")]
        public async Task<IActionResult> CheckAvailability(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            // Check if the book is available
            var available = book != null && book.IsAvailable;

            return Json(new { available });
        }
    }
}
