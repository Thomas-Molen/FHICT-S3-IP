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
        private readonly ICRUDService<Users> crudService;

        public TestingController(ICRUDService<Users> _crudService)
        {
            crudService = _crudService;
        }

       [HttpGet]
       public async Task<IActionResult> GetUsers()
        {
            var users = await crudService.Get();
            return Ok(users);
        }
    }
}
