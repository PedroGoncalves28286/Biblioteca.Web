using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsletterController : Controller
    {
        private readonly INewsletterRepository _newsletterRepository;

        public NewsletterController(INewsletterRepository newsletterRepository)
        {
            _newsletterRepository = newsletterRepository;
        }

        [HttpGet]
        public IActionResult GetNewsletters()
        {
            return Ok(_newsletterRepository.GetAllWithUsers());
        }
    }
}
