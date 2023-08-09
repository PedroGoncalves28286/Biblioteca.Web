using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Author ToAuthor(AuthorViewModel model, string path, bool isNew)
        { 
            return new Author
            {
                Id = isNew ? 0 : model.Id,
                AuthorImage = path,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }
        public AuthorViewModel ToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                AuthorImage = author.AuthorImage,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }


        public Rental ToRental(RentalViewModel model, string path, bool isNew)
        { 
            return new Rental
            {
                Id = isNew ? 0 : model.Id,
                Borrower = model.Borrower,
                Author = model.Author,
                Title = model.Title,
                BookId = model.BookId,
                ImageUrl = path,
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

        public RentalViewModel ToRentalViewModel(Rental rental)
        {
            return new RentalViewModel
            {
                Id = rental.Id,
                Borrower = rental.Borrower,
                Author = rental.Author,
                Title = rental.Title,
                BookId = rental.BookId,
                ImageUrl = rental.ImageUrl,
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
