using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace Biblioteca.Web.Data.Entities
{
    public class Rental : IEntity
    {
        public int Id { get; set; }

        public string Borrower { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        [Display(Name = "Book ID")]
        public int BookId { get; set; }

        [Display(Name ="Covers")]
        public Guid CoverId { get; set; }

        public string Availability { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }

        [Display(Name = "Start date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Return date")]
        public DateTime? ScheduleReturnDate { get; set; }

        [Display(Name = "Devolution")]
        public DateTime? ActualReturnDate { get; set; }

        [Display(Name = "Rental duration")]
        public int RentalDuration { get; set; } 


        public User User { get; set; }

        public string ImageFullPath => CoverId == Guid.Empty
            ? $"https://booksonline.azurewebsites.net/images/no_image.png"
            : $"https://bibliotecaarmazenamento.blob.core.windows.net/covers/{CoverId}";



    }
}
