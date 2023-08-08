using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Author : IEntity
    {

        public int Id { get; set; }

        public string AuthorImage { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageFullPath
        {
            get
            {
                if(string.IsNullOrEmpty(AuthorImage))
                {
                    return null;

                }
                return $"https://localhost:44354{AuthorImage.Substring(1)}";
            }
        }

        
    }
}
