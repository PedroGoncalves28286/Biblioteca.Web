using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository

    {
        private readonly DataContext _context;

        public MemberRepository(DataContext context ) :base(context) 
        {
            _context = context;

        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Members.Include(p => p.User);
        }
    }
}
