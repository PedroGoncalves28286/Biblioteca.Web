using System;

namespace Biblioteca.Web.Data.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Disable { get; set; }
        public int MembershipID { get; set; }

    }
}
