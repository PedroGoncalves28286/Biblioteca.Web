using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}
