using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Data
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        private readonly DataContext _context;

        public RentalRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
