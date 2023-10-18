using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Books.Include(p => p.User);
        }
        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.Books.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Book..)",
                Value = "0"
            });

            return list;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public Book GetBookById(int bookId)
        {
            // Retrieve the book with the given ID from your data source
            return _context.Books.FirstOrDefault(b => b.Id == bookId);
        }

        public async Task<List<Book>> GetBooksByGenreAsync(string genreName)
        {
            // Query the database to retrieve books by genre
            return await _context.Books
                .Where(b => b.GenreName == genreName)
                .ToListAsync();
        }

        public async Task<Book> GetBookByBookIdAsync(int bookId)
        {
            // Assuming ISBN is a unique property in your Book entity
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<int> Create(BookViewModel model)
        {
            var newBook = new Book()
            {
                BookPdfUrl = model.BookPdfUrl
            };

            await _context.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;
        }

        public async Task<BookViewModel> GetTheBookById(int id)
        {
            return await _context.Books.Where(x => x.Id == id)
                .Select(book => new BookViewModel()
                {
                    BookPdfUrl = book.BookPdfUrl
                }).FirstOrDefaultAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            // Find the book by its ID and remove it from the database
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Title == title);
        }

    }
}
