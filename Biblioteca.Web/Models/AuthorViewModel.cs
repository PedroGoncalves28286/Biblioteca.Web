using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Biblioteca.Web.Models
{
    public class AuthorViewModel : Author
    {
        [Display(Name = "Author")]
        public IFormFile ImageFile { get; set; }
    }
}
