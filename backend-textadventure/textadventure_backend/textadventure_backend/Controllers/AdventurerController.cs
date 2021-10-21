using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;
using textadventure_backend.Services.Interfaces;

namespace textadventure_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdventurerController : ControllerBase
    {
        private readonly IAdventurerService adventurerService;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public AdventurerController(IAdventurerService _adventurerService)
        {
            adventurerService = _adventurerService;
            tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            string authHeader = Request.Headers[HeaderNames.Authorization];
            var JWTToken = tokenHandler.ReadJwtToken(authHeader.Replace("Bearer ", ""));
            try
            {
                var response = await adventurerService.Create(int.Parse(JWTToken.Claims.First(t => t.Type == "id").Value));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            string authHeader = Request.Headers[HeaderNames.Authorization];
            var JWTToken = tokenHandler.ReadJwtToken(authHeader.Replace("Bearer ", ""));
            try
            {
                var response = await adventurerService.Get(int.Parse(JWTToken.Claims.First(t => t.Type == "id").Value));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
