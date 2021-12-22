using Microsoft.AspNetCore.Mvc;
using System;
using textadventure_backend_entitymanager.Helpers;
using textadventure_backend_entitymanager.Services;

namespace textadventure_backend_entitymanager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnemyController : ControllerBase
    {
        private readonly IEnemyService enemyService;
        private readonly AccessTokenHelper accessTokenHelper;
        public EnemyController(IEnemyService _enemyService, AccessTokenHelper _accessTokenHelper)
        {
            enemyService = _enemyService;
            accessTokenHelper = _accessTokenHelper;
        }

        [HttpGet("generate/{experience}/{accessToken}")]
        public IActionResult GenerateEnemy([FromRoute] string accesstoken, int experience)
        {
            if (!accessTokenHelper.IsTokenValid(accesstoken))
            {
                return Unauthorized("Invalid accesstoken");
            }

            try
            {
                var result = enemyService.GenerateEnemy(experience);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
