using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        private readonly DataContext _context;

        public RentalRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Rentals.Include(p => p.User);
        }
    }
}
