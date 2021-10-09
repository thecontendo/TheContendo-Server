using System;
using System.Threading.Tasks;
using Contendo.Data.Identity;
using Contendo.Models.Identity.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.Public
{
    public class ShotsController : PublicBaseController
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<ShotsController> _logger;

        public ShotsController(IUsersService usersService, ILogger<ShotsController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _usersService.Get());
        }  
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _usersService.Get(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(UserDto model)
        {
            try
            {
                return Ok(await _usersService.Add(model));
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
            return Ok(await _usersService.Delete(id));
        }  
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UserDto model)
        {
            return Ok(await _usersService.Put(model));
        }
        
        /*[HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter(UserFilterDto model)
        {
            return Ok(await _usersService.GetByFilter(model));
        }*/
    }
}