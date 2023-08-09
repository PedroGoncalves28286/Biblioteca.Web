using Biblioteca.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController : Controller
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipsController(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        [HttpGet]
        public IActionResult GetMemberships()
        {
            return Ok(_membershipRepository.GetAllWithUsers());
        }
    }
}
