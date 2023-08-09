using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IReservationRepository : IGenericRepository<Reservation> 
    {
        public IQueryable GetAllWithUsers();
    }
}
