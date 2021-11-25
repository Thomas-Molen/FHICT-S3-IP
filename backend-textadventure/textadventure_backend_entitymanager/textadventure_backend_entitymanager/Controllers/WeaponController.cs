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
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService weaponService;
        private readonly AccessTokenHelper accessTokenHelper;
        public WeaponController(IWeaponService _weaponService, AccessTokenHelper _accessTokenHelper)
        {
            weaponService = _weaponService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpGet("generate/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GenerateWeapon([FromRoute] string accesstoken, string adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await weaponService.GenerateWeapon(Convert.ToInt32(adventurerId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GetAllWeapons([FromRoute] string accesstoken, string adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await weaponService.GetAllWeapons(Convert.ToInt32(adventurerId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("equip/{accessToken}")]
        public async Task<IActionResult> GetAllWeapons([FromRoute] string accesstoken, [FromBody]EquipWeaponRequest request)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                await weaponService.EquipWeapon(request.AdventurerId, request.WeaponId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
