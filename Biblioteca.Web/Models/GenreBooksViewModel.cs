using Biblioteca.Web.Data.Entities;
using System.Collections.Generic;

namespace Biblioteca.Web.Models
{
    public class GenreBooksViewModel
    {
        public Genre Genre { get; set; }

        public string GenreName { get; set; }

        public List<BookViewModel> Books { get; set; }
    }
}
