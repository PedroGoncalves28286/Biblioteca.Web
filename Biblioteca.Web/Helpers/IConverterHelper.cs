using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using System.IO;

namespace Biblioteca.Web.Helpers
{
    public interface IConverterHelper
    {
        Author ToAuthor(AuthorViewModel model, string path, bool isNew);
        AuthorViewModel ToAuthorViewModel(Author author);


        Rental ToRental(RentalViewModel model, string path, bool isNew);

        RentalViewModel ToRentalViewModel(Rental rental);
    }
}
