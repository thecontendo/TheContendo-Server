using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.identity.Dto;

namespace Contendo.Data.Identity
{
    public class UserService: IUserService
    {
        public Task<UiServerResponse<List<UserDto>>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<UserDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<Guid>> Add(UserDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Put(UserDto model)
        {
            throw new NotImplementedException();
        }

        public Task<UiServerResponse<bool>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}