using Microsoft.AspNetCore.Http.Features;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class LendDetailTemp : IEntity
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Book Book { get; set; }

        [Display(Name = "Lend Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LendDate { get; set; }

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
