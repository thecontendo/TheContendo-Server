using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contendo.Data.Identity;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.Challenges;
using Contendo.Models.Challenges.Dto;
using Contendo.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contendo.Data
{
    public class ChallengeService: IChallengeService
    {
        private readonly CDbContext _db;
        private readonly ILogger<ChallengeService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChallengeService(CDbContext db, ILogger<ChallengeService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UiServerResponse<List<Challenge>>> Get()
        {
            var result = new UiServerResponse<List<Challenge>>();
            try
            {
                var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                var userId = Guid.Empty;
                if (value?.Value != null)
                {
                    var claims = value.Subject?.Claims.FirstOrDefault(e => e.Type == "jti");
                    if (claims == null)
                    {
                        result.Success = false;
                        return await Task.FromResult(result);
                    }

                    userId = Guid.Parse(claims.Value);
                }
                
                result.Success = true;

                if (value?.Value == "SuperAdmin")
                {
                    result.Data =  await _db.Challenges
                        .Include(e => e.Challenger)
                        .Include(e => e.Defender)
                        .Include(e => e.Shot)
                        .ToListAsync();
                }
                else
                {
                    result.Data =  await _db.Challenges
                        .Include(e => e.Challenger)
                        .Include(e => e.Defender)
                        .Include(e => e.Shot)
                        .Where(e => e.DefenderId == userId || e.ChallengerId == userId).ToListAsync();
                }
               
                return await Task.FromResult(result);
                
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                _logger.LogInformation($"Add Challenge: There is an error creating new challenge{ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<List<RecievedChallengeDto>>> Get(Guid challengeId)
        {
            var result = new UiServerResponse<List<RecievedChallengeDto>>();
            try
            {
                var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                var userId = Guid.Empty;
                if (value?.Value != null)
                {
                    var claims = value.Subject?.Claims.FirstOrDefault(e => e.Type == "jti");
                    if (claims == null)
                    {
                        result.Success = false;
                        return await Task.FromResult(result);
                    }

                    userId = Guid.Parse(claims.Value);
                }


                var challenges = await _db.Challenges
                    .Include(e => e.Defender)
                    .Include(e => e.Shot)
                    .Select(e => new RecievedChallengeDto
                    {
                        ChallengerId = e.ChallengerId,
                        Defender = e.Defender,
                        Points = e.Points,
                        Duration = e.Duration,
                        Shot = e.Shot,
                        ChallengeStatus = (int)e.ChallengerStatus,
                        ChallengerStatus = (int)e.ChallengerStatus,
                        DefenderStatus = (int)e.DefenderStatus
                    })
                    .Where(e => e.ChallengerId == userId).ToListAsync();
                
                
                result.Success = true;
                result.Data = challenges;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                _logger.LogInformation($"Add Challenge: There is an error creating new challenge{ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<Guid>> Add(ChallengeDto model)
        {
            var result = new UiServerResponse<Guid>();
            try
            {
                var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                var userId = Guid.Empty;
                if (value?.Value != null)
                {
                    var claims = value.Subject?.Claims.FirstOrDefault(e => e.Type == "jti");
                    if (claims == null)
                    {
                        result.Success = false;
                        return await Task.FromResult(result);
                    }

                    userId = Guid.Parse(claims.Value);
                }
                var challenge = new Challenge
                {
                    ChallengerId = userId,
                    DefenderId = model.DefenderId,
                    ShotId = model.ShotId,
                    ChallengerStatus = ChallengeStatus.Ready,
                    Duration = model.Duration
                };
                await _db.Challenges.AddAsync(challenge);
                await _db.SaveChangesAsync();
                result.Success = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                return result;
            }
        }

        public Task<UiServerResponse<bool>> Put(ChallengeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
