using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
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

        [Display(Name = "PDF")]
        public IFormFile PdfFile { get; set; } // Add the PDF file property

        public Guid PdfId { get; set; } // Add the property to store the PDF identifier
    }
}
