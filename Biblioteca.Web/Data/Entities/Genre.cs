using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Genre")]

        public string Name{ get; set; }


    }
}
