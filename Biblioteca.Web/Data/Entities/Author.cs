using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Author : IEntity
    {

        public int Id { get; set; }

        [Display(Name = "Author Image")]
        public Guid AuthorImageId { get; set; }  

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageFullPath => AuthorImageId == Guid.Empty
            ? $"https://booksonline.azurewebsites.net/images/no_image.png"
            : $"https://bibliotecaarmazenamento.blob.core.windows.net/authors/{AuthorImageId}";
            



    }
}
