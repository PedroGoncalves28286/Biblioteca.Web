using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Data
{
    public class NewsletterRepository : GenericRepository<Newsletter>, INewsletterRepository
    {
        private readonly DataContext _context;

        public NewsletterRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
