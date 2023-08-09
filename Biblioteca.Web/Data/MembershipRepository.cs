using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {

        private readonly DataContext _context;

        public MembershipRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Memberships.Include(p => p.User);
        }
    }
    
}
