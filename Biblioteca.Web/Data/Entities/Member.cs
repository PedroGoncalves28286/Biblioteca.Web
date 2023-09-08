using System;

namespace Biblioteca.Web.Data.Entities
{
    public class Member : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        public User User { get; set; }
    }
}
