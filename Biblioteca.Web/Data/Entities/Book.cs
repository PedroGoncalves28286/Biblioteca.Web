using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace Biblioteca.Web.Data.Entities
{
    public class Book : IEntity
    {
        public int Id { get; set; }

        public string Borrower { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        [Display(Name = "Book Id")]
        public int BookId { get; set; }

        [Display(Name = "Selected Date")]
        public DateTime? SelectedDate { get; set; }

        [Display(Name ="Covers")]
        public Guid CoverId { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }
        
        public User User { get; set; }

        public string ImageFullPath => CoverId == Guid.Empty
            ? $"https://booksonline.azurewebsites.net/images/no_image.png"
            : $"https://bibliotecaarmazenamento.blob.core.windows.net/covers/{CoverId}";



    }
}
