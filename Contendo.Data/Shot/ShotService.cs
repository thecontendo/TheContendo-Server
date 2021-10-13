using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.ShotDto.Dto;
using Contendo.Models.Shots;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contendo.Data
{
    public class ShotService: IShotService
    {
        private readonly CDbContext _db;
        private readonly ILogger<ShotService> _logger;

        public ShotService(CDbContext db, ILogger<ShotService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<UiServerResponse<List<ShotDto>>> Get()
        {
            var result = new UiServerResponse<List<ShotDto>>();

            try
            {
                result.Data = await _db.Shots.Select(shot => GetShotDto(shot)).ToListAsync();;
                result.AddSuccess();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"There is an error getting Shots: {ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<ShotDto>> Get(Guid id)
        {
            var result = new UiServerResponse<ShotDto>();

            try
            {
                result.Data = GetShotDto(await _db.Shots.FindAsync(id));
                result.AddSuccess();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"There is an error getting Shots: {ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<Guid>> Add(ShotDto model)
        {
            var result = new UiServerResponse<Guid>();
            try
            {
                var Shot = new Shot
                {
                    Icon = model.Icon,
                    Name = model.Name,
                    Description = model.Description
                };
                
                _db.Shots.Add(Shot);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Add Shot: Saved Changes");
                result.Data = Shot.Id;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                _logger.LogInformation($"Add Shot: There is an error creating new Shot {ex.Message}");
                return result;
            }
        }
        
        public async Task<UiServerResponse<Guid>> AddShots(List<ShotDto> model)
        {
            var result = new UiServerResponse<Guid>();
            try
            {
                var shots = new List<Shot>();

                model.ForEach(e =>
                {
                    shots.Add(new Shot
                    {
                        Icon = e.Icon,
                        Name = e.Name,
                        Description = e.Description
                    });
                });


                if (!shots.Any()) return await Task.FromResult(result);
                    
                await _db.Shots.AddRangeAsync(shots);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Add Shot: Saved Changes");
                result.Success = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                _logger.LogInformation($"Add Shot: There is an error creating new Shot {ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<bool>> Put(ShotDto model)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var shot = await _db.Shots.FindAsync(model.Id);

                if (shot == null) return await Task.FromResult(result);
                shot.Name = model.Name;
                shot.Icon = model.Icon;
                shot.Description = model.Description;

                _db.Entry(shot).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                _logger.LogInformation($"ShotService, Update: Saved Changes");
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ShotService, Update: {model.Id}: {ex.Message}");
                result.Data = false;
                return result;
            }
        }

        public async Task<UiServerResponse<bool>> Delete(Guid id)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var shot = await _db.Shots.FindAsync(id);

                if (shot == null)
                {
                    result.AddNotFound();
                    _logger.LogInformation($"ShotService, Delete : {id} not found");
                    return await Task.FromResult(result);
                }

                _db.Shots.Remove(shot);

                await _db.SaveChangesAsync();
                //result.AddSuccess(_stringLocalizer["201"]);
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ShotService, Delete : There is an error deleting Shots {id}: {ex.Message}");
                result.Data = false;
                return result;
            }
        }
        
        private static ShotDto GetShotDto(Shot shot)
        {
            return new ShotDto
            {
                Id = shot.Id,
                Icon = shot.Icon,
                Name = shot.Name,
                Description = shot.Description
            };
        }
        
        
    }
}