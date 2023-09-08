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
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime LendDate { get; set; }
    }
}
