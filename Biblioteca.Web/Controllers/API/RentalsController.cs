using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : Controller
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalsController(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        public IActionResult GetRentals()
        {
            return Ok(_rentalRepository.GetAllWithUsers());
        }
    }
}
