using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context) : base(context)
        {
            _context = context;
        }

    }
}
