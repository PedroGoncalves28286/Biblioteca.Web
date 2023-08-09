using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {

        private readonly DataContext _context;

        public ReservationRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Reservations.Include(p => p.User);
        }
    }
}
