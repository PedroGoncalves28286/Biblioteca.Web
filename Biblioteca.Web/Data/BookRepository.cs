using Biblioteca.Web.Data.Entities;
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
    }
}
