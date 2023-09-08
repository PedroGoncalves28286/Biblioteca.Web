using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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

        [Display(Name = "Lend Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime LendDate { get; set; }

        public IEnumerable<SelectListItem> Books { get; set; }

        
    }
}
