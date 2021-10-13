using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.ShotDto.Dto;

namespace Contendo.Data
{
    public interface IShotService
    {
        Task<UiServerResponse<List<ShotDto>>> Get();
        
        Task<UiServerResponse<ShotDto>> Get(Guid id);
        
        Task<UiServerResponse<Guid>> Add(ShotDto model);
        
        Task<UiServerResponse<Guid>> AddShots(List<ShotDto> model);
        
        Task<UiServerResponse<bool>> Put(ShotDto model);
        
        Task<UiServerResponse<bool>> Delete(Guid id);
    }
}