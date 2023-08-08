using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class RentalViewModel : Rental
    {
        [Display(Name ="Cover")]

        public IFormFile ImageFile { get; set; }


    }
}
