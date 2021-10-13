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

        /*[HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(UserFilterDto model)
        {
            return Ok(await _shotsService.GetByFilter(model));
        }*/
    }
}