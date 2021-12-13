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
    public class EnemyController : ControllerBase
    {
        private readonly EnemyService enemyService;
        private readonly AccessTokenHelper accessTokenHelper;
        public EnemyController(EnemyService _enemyService, AccessTokenHelper _accessTokenHelper)
        {
            enemyService = _enemyService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpGet("generate/{adventurerId}/{accessToken}")]
        public async Task<IActionResult> GenerateEnemy([FromRoute] string accesstoken, int adventurerId)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = await enemyService.GenerateEnemy(adventurerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
