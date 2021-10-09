using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Data.Sport;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.ShotDto.Dto;
using Contendo.Models.Shots;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;

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

        public Task<UiServerResponse<List<ShotDto>>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<ShotDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<Guid>> Add(ShotDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Put(ShotDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Delete(Guid id)
        {
            new Shot();
            throw new NotImplementedException();
        }
        
        /*private static SportDto GetUserDto(Sport sport)
        {
            return new SportDto
            {
                Id = sport.Id,
                Icon = sport.Icon,
                Name = sport.Name,
                Description = sport.Description
            };
        }*/
        
        
    }
}