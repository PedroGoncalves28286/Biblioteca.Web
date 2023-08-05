using System.ComponentModel.DataAnnotations;
using System;

namespace Biblioteca.Web.Data.Entities
{
    public class Newsletter
    {
        public int Id { get; set; }
        public int NewsID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime AddDate { get; set; }
    }
}


