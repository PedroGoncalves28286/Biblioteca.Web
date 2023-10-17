using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Biblioteca.Web.Models
{
    public class BookViewModel : Book
    {
        [Display(Name = "Cover")]

        public IFormFile ImageFile { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }


        public IFormFile BookPdf { get; set; }

        public string BookPdfUrl { get; set; }
    }
}
