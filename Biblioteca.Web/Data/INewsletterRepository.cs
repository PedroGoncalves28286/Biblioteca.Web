using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface INewsletterRepository : IGenericRepository<Newsletter>
    {
        public IQueryable GetAllWithUsers();
    }
}
