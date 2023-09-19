using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
