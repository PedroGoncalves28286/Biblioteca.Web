using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Genre> GetGenreByNameAsync(string genreName)
        {
            // Query the database to retrieve a genre by its name
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name == genreName);
        }
    }
}
