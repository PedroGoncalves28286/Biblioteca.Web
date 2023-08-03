using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
