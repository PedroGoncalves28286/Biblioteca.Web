using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using System;
using System.IO;

namespace Biblioteca.Web.Helpers
{
    public interface IConverterHelper
    {
        Author ToAuthor(AuthorViewModel model, Guid authorImageId, bool isNew);
        AuthorViewModel ToAuthorViewModel(Author author);


        Book ToBook(BookViewModel model, Guid coverId, bool isNew);

        BookViewModel ToBookViewModel(Book book);
    }
}
