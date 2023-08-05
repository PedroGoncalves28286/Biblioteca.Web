using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Data
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository

    {
        private readonly DataContext _context;

        public MemberRepository(DataContext context ) :base(context) 
        {
            _context = context;

        }
    }
}
