using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboBooks();

        Task<Book> GetBookByIdAsync(int bookId);

        Task UpdateBookAsync(Book book);

        Task DeleteAsync(Book book);

        Task<List<Book>> GetBooksByGenreAsync(string genreName);

        Task<Book> GetBookByBookIdAsync(int bookId);


    }
}
