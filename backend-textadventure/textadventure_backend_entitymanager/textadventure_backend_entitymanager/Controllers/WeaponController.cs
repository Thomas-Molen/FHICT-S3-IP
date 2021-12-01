using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services;

namespace textadventure_backend_entitymanager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly WeaponService weaponService;
        private readonly AccessTokenHelper accessTokenHelper;
        public WeaponController(WeaponService _weaponService, AccessTokenHelper _accessTokenHelper)
        {
            weaponService = _weaponService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpGet("generate/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GenerateWeapon([FromRoute] string accesstoken, int adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await weaponService.GenerateWeapon(adventurerId);

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
        public async Task<IActionResult> EquipWeapon([FromRoute] string accesstoken, [FromBody]EquipWeaponRequest request)
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
