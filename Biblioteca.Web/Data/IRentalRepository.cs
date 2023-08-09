using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        public IQueryable GetAllWithUsers();
    }
}
