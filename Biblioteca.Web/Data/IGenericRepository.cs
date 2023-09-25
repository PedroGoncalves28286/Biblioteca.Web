using Biblioteca.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int id);

        Task<Book> GetByNameAsync(string name);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }
}
