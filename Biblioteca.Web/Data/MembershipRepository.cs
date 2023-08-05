using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Data
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {

        private readonly DataContext _context;

        public MembershipRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
    
}
