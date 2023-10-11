using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Library : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Library")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
