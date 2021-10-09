using System;
using System.Threading.Tasks;
using Contendo.Data;
using Contendo.Models.ShotDto.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.Public
{
    public class ShotsController : PublicBaseController
    {
        private readonly IShotService _shotsService;
        private readonly ILogger<ShotsController> _logger;

        public ShotsController(IShotService shotService, ILogger<ShotsController> logger)
        {
            _shotsService = shotService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _shotsService.Get());
        }  
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _shotsService.Get(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(ShotDto model)
        {
            try
            {
                return Ok(await _shotsService.Add(model));
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is an error creating new user: {ex.Message}");
                return BadRequest("");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _shotsService.Delete(id));
        }  
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(ShotDto model)
        {
            return Ok(await _shotsService.Put(model));
        }
        
        /*[HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(UserFilterDto model)
        {
            return Ok(await _shotsService.GetByFilter(model));
        }*/
    }
}