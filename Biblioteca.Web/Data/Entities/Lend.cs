using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Linq;

namespace Biblioteca.Web.Data.Entities
{
    public class Lend : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lend date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime LendDate { get; set; }

        [Required]
        public User User { get; set; }

        public Book Book { get; set; }

        public IEnumerable<LendDetail> Items { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle
        {
            get
            {
                if (Items != null && Items.Any())
                {
                    // Assuming you have a single book associated with the Lend
                    var firstItem = Items.FirstOrDefault();
                    if (firstItem != null && firstItem.Book != null)
                    {
                        return firstItem.Book.Title;
                    }
                }
                return "Unavailable"; // or any default value you prefer
            }
        }


        [Display(Name = "Lend date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? LendDateLocal => this.LendDate == null ? null : this.LendDate.ToLocalTime();


        [Display(Name = "Devolution date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DevolutionDate
        {
            get
            {
                // Calculate the devolution date based on your logic here
                // For example, add a fixed number of days to the LendDate
                if (LendDate != null)
                {
                    // You can change this logic based on your requirements
                    return LendDate.AddDays(14); // 14 days after LendDate
                }

                // Return a default value if LendDate is not set
                return DateTime.MinValue;
            }
        }
    }
}
