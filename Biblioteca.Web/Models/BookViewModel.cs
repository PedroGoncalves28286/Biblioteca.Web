using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class BookViewModel : Book
    {
        [Display(Name = "Cover")]

        public IFormFile ImageFile { get; set; }


    }
}
