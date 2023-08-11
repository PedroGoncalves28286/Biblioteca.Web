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


        Rental ToRental(RentalViewModel model,Guid coverId, bool isNew);

        RentalViewModel ToRentalViewModel(Rental rental);
    }
}
