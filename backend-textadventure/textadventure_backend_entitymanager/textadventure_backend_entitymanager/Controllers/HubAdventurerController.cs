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
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HubAdventurerController : ControllerBase
    {
        private readonly HubAdventurerService adventurerService;
        private readonly AccessTokenHelper accessTokenHelper;
        public HubAdventurerController(HubAdventurerService _adventurerService, AccessTokenHelper _accessTokenHelper)
        {
            adventurerService = _adventurerService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpGet("get/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GetAdventurer([FromRoute] int adventurerId, string accesstoken)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var response = await adventurerService.Get(adventurerId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
