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
        private int availableCopies;

        public int Id { get; set; }

        public string Borrower { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string GenreName { get; set; }

        [Display(Name = "Selected Date")]
        public DateTime? SelectedDate { get; set; }

        [Display(Name = "Covers")]
        public Guid CoverId { get; set; }

        public string ISBN { get; set; }

        public string Publisher { get; set; }

        public string BookPdfUrl { get; set; }

        public bool IsAvailable { get; set; }

        public User User { get; set; }


        public IEnumerable<LendDetail> Items { get; set; }

        public int AvailableCopies
        {
            get
            {
                return availableCopies;
            }
            set
            {
                availableCopies = value;

                // Update the IsAvailable property based on the availableCopies value
                IsAvailable = availableCopies >= 1;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (availableCopies < 1)
            {
                yield return new ValidationResult("The book is currently unavailable.", new[] { nameof(AvailableCopies) });
            }
        }
        public string ImageFullPath => CoverId == Guid.Empty
            ? $"https://booksonline.azurewebsites.net/images/no_image.png"
            : $"https://bibliotecaarmazenamento.blob.core.windows.net/covers/{CoverId}";

        public Guid PdfId { get; set; } // Add the PdfId property

        public string PdfFilePath { get; set; }


    }
}
