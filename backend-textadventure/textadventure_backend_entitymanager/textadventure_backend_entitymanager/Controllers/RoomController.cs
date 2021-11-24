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
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;
        private readonly AccessTokenHelper accessTokenHelper;
        public RoomController(IRoomService _roomService, AccessTokenHelper _accessTokenHelper)
        {
            roomService = _roomService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpPost("enter/{accessToken}")]
        public async Task<IActionResult> EnterRoom([FromRoute] string accesstoken, [FromBody] EnterRoomRequest createSpawnRequest)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await roomService.MoveToRoom(createSpawnRequest.AdventurerId, createSpawnRequest.Direction.ToLower());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("load/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> LoadRoom([FromRoute] string accesstoken, string adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await roomService.LoadRoom(Convert.ToInt32(adventurerId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("spawn/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> CreateSpawn([FromRoute] string accesstoken, string adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await roomService.CreateSpawn(Convert.ToInt32(adventurerId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
