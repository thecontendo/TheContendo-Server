using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.ContactRequests.Dto;
using Contendo.Models.Identity.Dto;

namespace Contendo.Data.Identity
{
    public interface IUsersService
    {
        Task<UiServerResponse<List<UserDto>>> Get();
        Task<UiServerResponse<UserDto>> Get(Guid id);
        
        Task<UiServerResponse<List<UserDto>>> GetContacts();
        
        Task<UiServerResponse<Guid>> Add(UserDto model);
        
        Task<UiServerResponse<bool>> Put(UserDto model);
        
        Task<UiServerResponse<bool>> Delete(Guid id);
    }
}