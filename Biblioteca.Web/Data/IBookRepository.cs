using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteca.Web.Data
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboBooks();
    }
}
