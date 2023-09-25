using Biblioteca.Web.Data.Entities;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<Genre> GetGenreByNameAsync(string genreName);
    }
}
