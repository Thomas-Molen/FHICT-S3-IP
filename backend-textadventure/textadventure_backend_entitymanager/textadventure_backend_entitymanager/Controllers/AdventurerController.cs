using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services;

namespace textadventure_backend_entitymanager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdventurerController : ControllerBase
    {
        private readonly IAdventurerService adventurerService;
        private readonly JWTHelper JWT;
        public AdventurerController(IAdventurerService _adventurerService, JWTHelper JWThelper)
        {
            adventurerService = _adventurerService;
            JWT = JWThelper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAdventurerRequest request)
        {
            try
            {
                await adventurerService.Create(request.Name, JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteAdventurerRequest request)
        {
            try
            {
                await adventurerService.Delete(JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]), request.adventurerId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllAdventurersFromUser()
        {
            try
            {
                var response = await adventurerService.GetAllFromUser(JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get/{adventurerId}")]
        public async Task<IActionResult> GetAdventurer([FromRoute] int adventurerId)
        {
            try
            {
                var response = await adventurerService.Get(adventurerId, JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("get-leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            try
            {
                var response = await adventurerService.GetLeaderboard();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
