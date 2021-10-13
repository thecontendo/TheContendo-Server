using System;
using System.Threading.Tasks;
using Contendo.Data;
using Contendo.Models.Challenges.Dto;
using Contendo.Models.ShotDto.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.admin
{
    public class ChallengeController : AdminBaseController
    {
        private readonly IChallengeService _challengeService;
        private readonly ILogger<ChallengeController> _logger;

        public ChallengeController(IChallengeService challengeService, ILogger<ChallengeController> logger)
        {
            _challengeService = challengeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _challengeService.Get());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserChallenges(Guid id)
        {
            return Ok(await _challengeService.Get(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(ChallengeDto model)
        {
            try
            {
                return Ok(await _challengeService.Add(model));
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
            return Ok(await _challengeService.Delete(id));
        }  
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(ChallengeDto model)
        {
            return Ok(await _challengeService.Put(model));
        }
    }
}