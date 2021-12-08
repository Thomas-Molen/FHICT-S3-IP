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
        private readonly AdventurerService adventurerService;
        private readonly JWTHelper JWT;
        private readonly AccessTokenHelper accessTokenHelper;
        public AdventurerController(AdventurerService _adventurerService, JWTHelper JWThelper, AccessTokenHelper _accessTokenHelper)
        {
            adventurerService = _adventurerService;
            JWT = JWThelper;
            accessTokenHelper = _accessTokenHelper;
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
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAdventurers()
        {
            try
            {
                var response = await adventurerService.Get(JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{adventurerId}")]
        public async Task<IActionResult> GetAdventurer([FromRoute] string adventurerId)
        {
            try
            {
                var response = await adventurerService.Get(JWT.GetUserIdFromJWT(Request.Headers[HeaderNames.Authorization]), Convert.ToInt32(adventurerId));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-drawing/{adventurerId}")]
        public async Task<IActionResult> GetDrawing([FromRoute] int adventurerId)
        {
            try
            {
                var result = await adventurerService.GetDrawing(adventurerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-drawing/{adventurerId}")]
        public async Task<IActionResult> SaveDrawing([FromRoute] int adventurerId, [FromBody] SaveDrawingRequest drawing)
        {
            try
            {
                await adventurerService.SaveDrawing(adventurerId, drawing.Drawing);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
            }
        }

        //Game endpoints
        [AllowAnonymous]
        [HttpGet("get/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GetAdventurer([FromRoute] int adventurerId, string accesstoken)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var response = await adventurerService.Find(adventurerId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("set-health/{adventurerId}/{health}/{accessToken}")]
        public async Task<IActionResult> SetAdventurerHealth([FromRoute] int adventurerId, int health, string accesstoken)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                await adventurerService.SetHealth(adventurerId, health);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("set-experience/{adventurerId}/{experience}/{accessToken}")]
        public async Task<IActionResult> SetAdventurerExperience([FromRoute] int adventurerId, int experience, string accesstoken)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                await adventurerService.SetExperience(adventurerId, experience);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
