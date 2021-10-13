using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.Challenges;
using Contendo.Models.Challenges.Dto;

namespace Contendo.Data
{
    public interface IChallengeService
    {
        Task<UiServerResponse<List<Challenge>>> Get();

        Task<UiServerResponse<List<RecievedChallengeDto>>> Get(Guid challengeId);
        
        Task<UiServerResponse<Guid>> Add(ChallengeDto model);
        
        Task<UiServerResponse<bool>> Put(ChallengeDto model);
        
        Task<UiServerResponse<bool>> Delete(Guid id);
    }
}