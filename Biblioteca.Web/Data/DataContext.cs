using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Newsletter> Newsletters{ get; set; }


        public DbSet<Member> Members { get; set; }

        public DbSet<Lend> Lends { get; set; }

        public DbSet<LendDetail> LendDetails { get; set; }

        public DbSet<LendDetailTemp> LendsDetailTemp { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DataContext(DbContextOptions<DataContext>options ):base(options)
        {
            
        }
        
    




    }
}
