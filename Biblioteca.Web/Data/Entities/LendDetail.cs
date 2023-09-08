using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class LendDetail : IEntity
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; }

        public User User { get; set; }

        [Display(Name = "Lend Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime LendDate { get; set; }

        public int LendId { get; set; }

        public Lend Lend { get; set; }


    }
}
