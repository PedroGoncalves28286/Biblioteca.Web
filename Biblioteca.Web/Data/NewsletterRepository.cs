using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public class NewsletterRepository : GenericRepository<Newsletter>, INewsletterRepository
    {
        private readonly DataContext _context;

        public NewsletterRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Newsletters.Include(p => p.User);
        }
    }
}
