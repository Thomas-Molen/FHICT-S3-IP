using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
            Console.WriteLine("Got into the controller");
        }

        [HttpGet]
        public async Task<IEnumerable<users>> GetUsers()
        {
            return await userService.GetUsers();
        }
    }
}
