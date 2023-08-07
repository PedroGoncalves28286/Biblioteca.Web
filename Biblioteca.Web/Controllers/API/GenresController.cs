using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(_genreRepository.GetAll());
        }
    }
}
