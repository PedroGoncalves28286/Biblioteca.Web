using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Member : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public User User { get; set; }
    }
}
