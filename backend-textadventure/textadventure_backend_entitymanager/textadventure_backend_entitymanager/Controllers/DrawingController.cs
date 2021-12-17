using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models.Requests;
using textadventure_backend_entitymanager.Services;
using textadventure_backend_entitymanager.Services.Interfaces;

namespace textadventure_backend_entitymanager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DrawingController : ControllerBase
    {
        private readonly IDrawingService drawingService;
        public DrawingController(IDrawingService _drawingService)
        {
            drawingService = _drawingService;
        }

        [HttpGet("get/{adventurerId}")]
        public async Task<IActionResult> GetDrawing([FromRoute] int adventurerId)
        {
            try
            {
                var result = await drawingService.GetDrawing(adventurerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("save/{adventurerId}")]
        public async Task<IActionResult> SaveDrawing([FromRoute] int adventurerId, [FromBody] SaveDrawingRequest drawing)
        {
            try
            {
                await drawingService.SaveDrawing(adventurerId, drawing.Drawing);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
