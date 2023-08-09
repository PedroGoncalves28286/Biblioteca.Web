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

        public int BookId { get; set; }

        public string ImageUrl { get; set; }

        public string Availability { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }

        public DateTime? StartDate { get; set; }

        [Display(Name = "Return Date")]
        public DateTime? ScheduleReturnDate { get; set; }

        [Display(Name = "Devolution")]
        public DateTime? ActualReturnDate { get; set; }   

        public int RentalDuration { get; set; } 


        public User User { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    return null;
                }

                return $"https://localhost:44354{ImageUrl.Substring(1)}";
            }
        }


    }
}
