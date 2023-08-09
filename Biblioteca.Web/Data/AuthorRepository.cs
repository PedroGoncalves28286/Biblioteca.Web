using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
