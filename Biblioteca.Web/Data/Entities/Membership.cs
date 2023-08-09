using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Data.Entities
{
    public class Membership : IEntity
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [DisplayName("Sign Up Fee")]
        [DataType(DataType.Currency)]
        public byte SignUpFee { get; set; }

        [DisplayName("6 month rate")]
        [DataType(DataType.Currency)]
        public byte ChargeRateSixMonth { get; set; }


        [DisplayName("12 month rate")]
        [DataType(DataType.Currency)]
        public byte ChargeRateTwelveMonth { get; set; }

        public User User { get; set; }

    }
}
