using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }

        public ICollection<Library> Libraries { get; set; }

        [Display(Name = "Number of libraries")]
        public int LibrariesNumber => Libraries == null ? 0 : Libraries.Count;

    }
}
