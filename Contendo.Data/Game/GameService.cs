using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Data.Identity;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.Games;
using Contendo.Models.Games.Dto;
using Microsoft.Extensions.Logging;

namespace Contendo.Data.Game
{
    public class GameService: IGameService
    {
        private readonly CDbContext _db;
        private readonly ILogger<GameService> _logger;

        public GameService(CDbContext db, ILogger<GameService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public Task<UiServerResponse<List<GameDto>>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<GameDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<Guid>> Add(GameDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Put(GameDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        
        
    }
}