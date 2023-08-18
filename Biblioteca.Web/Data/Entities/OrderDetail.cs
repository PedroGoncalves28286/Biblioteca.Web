using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }
    }
}
