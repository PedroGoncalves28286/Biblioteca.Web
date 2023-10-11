using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class LibraryViewModel
    {
        public int CityId { get; set; }

        public int LibraryId { get; set; }

        [Required]
        [Display(Name = "Library")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
