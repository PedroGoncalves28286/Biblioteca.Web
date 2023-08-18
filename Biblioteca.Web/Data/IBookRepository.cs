using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public IQueryable GetAllWithUsers();
    }
}
