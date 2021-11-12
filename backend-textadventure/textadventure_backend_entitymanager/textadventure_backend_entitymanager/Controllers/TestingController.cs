using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Services.Interfaces;

namespace textadventure_backend_entitymanager.Controllers
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
