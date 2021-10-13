using System;
using System.Threading.Tasks;
using Contendo.Data;
using Contendo.Models.Challenges.Dto;
using Contendo.Models.ShotDto.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.Public
{
    public class ChallengeController : PublicBaseController
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
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _challengeService.Get(id));
        }
        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetUserChallenges(Guid id)
        {
            return Ok(await _challengeService.Get(id));
        }*/
        
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(ChallengeDto model)
        {
            return Ok(await _challengeService.Put(model));
        }
    }
}