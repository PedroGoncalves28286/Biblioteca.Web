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


        public Book ToRental(BookViewModel model,Guid coverId, bool isNew)
        { 
            return new Book
            {
                Id = isNew ? 0 : model.Id,
                Borrower = model.Borrower,
                Author = model.Author,
                Title = model.Title,
                BookId = model.BookId,
                CoverId = coverId,
                Availability = model.Availability,
                ISBN = model.ISBN,
                Publisher = model.Publisher,
                StartDate = model.StartDate,
                ScheduleReturnDate = model.ScheduleReturnDate,
                ActualReturnDate = model.ActualReturnDate,
                RentalDuration = model.RentalDuration,
                User = model.User
            };
        }

        public BookViewModel ToRentalViewModel(Book rental)
        {
            return new BookViewModel
            {
                Id = rental.Id,
                Borrower = rental.Borrower,
                Author = rental.Author,
                Title = rental.Title,
                BookId = rental.BookId,
                CoverId = rental.CoverId,
                Availability = rental.Availability,
                ISBN = rental.ISBN,
                Publisher = rental.Publisher,
                StartDate = rental.StartDate,
                ScheduleReturnDate = rental.ScheduleReturnDate,
                ActualReturnDate = rental.ActualReturnDate,
                RentalDuration = rental.RentalDuration,
                User = rental.User
            };
        }
    }
}
