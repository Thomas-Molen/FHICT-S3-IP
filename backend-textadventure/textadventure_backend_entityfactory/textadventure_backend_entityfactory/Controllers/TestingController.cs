using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace textadventure_backend_entityfactory.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestingController : ControllerBase
    {

        public TestingController()
        {
        }

        [AllowAnonymous]
        [HttpGet]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> test()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return "hello!";
        }
    }
}
