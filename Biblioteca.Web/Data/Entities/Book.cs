using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Linq;

namespace Biblioteca.Web.Data.Entities
{
    public class Book : IEntity, IValidatableObject
    {
        public int Id { get; set; }

        public string Borrower { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string GenreName { get; set; }

        [Display(Name = "Book Id")]
        public int BookId { get; set; }

        [Display(Name = "Selected Date")]
        public DateTime? SelectedDate { get; set; }

        [Display(Name = "Covers")]
        public Guid CoverId { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }

        public bool IsAvailable { get; set; }

        public User User { get; set; }


        public IEnumerable<LendDetail> Items { get; set; }

        [Required]
        //[MaxLength(20)]
       
        public int LoanLimitQuantity { get;set; }

        public IEnumerable <ValidationResult> Validate(ValidationContext validationContext)
        {
            if(LoanLimitQuantity < 1 || LoanLimitQuantity > 20)
            {
                yield return new ValidationResult("Max quatity must be between 1 and 20 " ,new[] { nameof(LoanLimitQuantity)});
            }
        }




    public string ImageFullPath => CoverId == Guid.Empty
            ? $"https://booksonline.azurewebsites.net/images/no_image.png"
            : $"https://bibliotecaarmazenamento.blob.core.windows.net/covers/{CoverId}";


       
    }
}
