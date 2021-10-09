using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.Games;
using Contendo.Models.Games.Dto;

namespace Contendo.Data.Game
{
    public interface IGameService
    {
        Task<UiServerResponse<List<GameDto>>> Get();
        
        Task<UiServerResponse<GameDto>> Get(Guid id);
        
        Task<UiServerResponse<Guid>> Add(GameDto model);
        
        Task<UiServerResponse<bool>> Put(GameDto model);
        
        Task<UiServerResponse<bool>> Delete(Guid id);
    }
}