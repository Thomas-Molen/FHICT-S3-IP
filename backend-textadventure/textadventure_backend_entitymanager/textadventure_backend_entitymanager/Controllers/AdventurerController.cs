using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Context;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services.Interfaces;

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
    }
}
