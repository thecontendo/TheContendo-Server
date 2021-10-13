using System;
using System.Threading.Tasks;
using Contendo.Data.Identity;
using Contendo.Models.Identity.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.Admin
{
    public class UsersController : AdminBaseController
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
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
    }
}