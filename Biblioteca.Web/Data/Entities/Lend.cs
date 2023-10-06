using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Linq;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Data.Entities
{
    public class Lend : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Lend date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
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
            set
            {
                // You can add logic to handle the setter here if needed
            }
        }


        [Display(Name = "Lend date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? LendDateLocal => this.LendDate == null ? null : this.LendDate.ToLocalTime();


        [Display(Name = "Devolution date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DevolutionDate { get; set; }

        public Lend()
        {
            // Initialize the DevolutionDate property with a default value
            DevolutionDate = DateTime.Now.AddDays(14); // Default to 14 days from today
        }

        [Display(Name = "Extended Devolution date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? ExtendedDevolutionDate { get; set; }

        public bool FirstExtensionDone { get; set; }
    }
}
