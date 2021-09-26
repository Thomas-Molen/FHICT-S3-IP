using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestingController : ControllerBase
    {

        public TestingController()
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok("hello!");
        }
    }
}
