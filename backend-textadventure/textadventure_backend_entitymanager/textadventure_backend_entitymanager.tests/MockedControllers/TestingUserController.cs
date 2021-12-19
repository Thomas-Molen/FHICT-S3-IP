using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services;
using textadventure_backend_entitymanager.Services.Interfaces;

namespace textadventure_backend_entitymanager.Controllers
{
    public class TestingUserController : ControllerBase
    {
        private readonly IUserService userService;

        public TestingUserController(IUserService _userService)
        {
            userService = _userService;
        }

        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var response = await userService.Register(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var response = await userService.Login(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public async Task<IActionResult> RenewToken(string refreshToken)
        {
            try
            {
                var response = await userService.RenewToken(refreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public async Task<IActionResult> DeactivateToken(string refreshToken)
        {
            try
            {
                await userService.DeactivateToken(refreshToken);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
