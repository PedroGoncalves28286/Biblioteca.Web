namespace Biblioteca.Web.Data.Entities
{
    public class Author : IEntity
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
