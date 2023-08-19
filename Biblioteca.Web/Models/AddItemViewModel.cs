using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Biblioteca.Web.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Book")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Book.")]
        public int BookId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Books { get; set; }

        
    }
}
