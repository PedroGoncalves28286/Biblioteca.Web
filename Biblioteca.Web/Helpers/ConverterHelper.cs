using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Migrations;
using Biblioteca.Web.Models;
using System;

namespace Biblioteca.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Author ToAuthor(AuthorViewModel model,Guid authorImageId, bool isNew)
        { 
            return new Author
            {
                Id = isNew ? 0 : model.Id,
                AuthorImageId = authorImageId,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }
        public AuthorViewModel ToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                AuthorImageId = author.AuthorImageId,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }


        public Book ToLend(BookViewModel model,Guid coverId, bool isNew)
        { 
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                Borrower = model.Borrower,
                Author = model.Author,
                Title = model.Title,
                BookId = model.BookId,
                CoverId = coverId,
                ISBN = model.ISBN,
                Publisher = model.Publisher,
                User = model.User
            };
        }

        public BookViewModel ToLendViewModel(Book rental)
        {
            return new BookViewModel
            {
                Id = rental.Id,
                Borrower = rental.Borrower,
                Author = rental.Author,
                Title = rental.Title,
                BookId = rental.BookId,
                CoverId = rental.CoverId,
                ISBN = rental.ISBN,
                Publisher = rental.Publisher,
                User = rental.User
            };
        }
    }
}
