using Biblioteca.Web.Data.Entities;
using System.Collections.Generic;

namespace Biblioteca.Web.Models
{
    public class UserWithRolesViewModel
    {
        public List<string> Roles { get; set; }

        public User User { get; set; }

        public string UserListUrl { get; set; }
    }
}
