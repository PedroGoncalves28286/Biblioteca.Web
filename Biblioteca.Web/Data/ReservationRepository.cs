using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Data
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
    {

        private readonly DataContext _context;

        public ReservationRepository(DataContext context) : base(context) 
        {
            _context = context;
        }
    }
}
