using Biblioteca.Web.Data.Entities;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IMemberRepository:IGenericRepository<Member>
    {
        public IQueryable GetAllWithUsers();
    }
}
