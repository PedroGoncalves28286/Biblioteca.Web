using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IMembershipRepository : IGenericRepository<Membership>     
    {
        public IQueryable GetAllWithUsers();
    }
}
