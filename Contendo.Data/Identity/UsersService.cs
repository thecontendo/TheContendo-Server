using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contendo.Db.Context;
using Contendo.Models;
using Contendo.Models.ContactRequests;
using Contendo.Models.ContactRequests.Dto;
using Contendo.Models.Enums;
using Contendo.Models.Identity;
using Contendo.Models.Identity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contendo.Data.Identity
{
    public class UsersService: IUsersService
    {
        private readonly CDbContext _db;
        private readonly ILogger<UsersService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(CDbContext db, ILogger<UsersService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UiServerResponse<List<UserDto>>> Get()
        {
            var result = new UiServerResponse<List<UserDto>>();

            try
            {
                result.Dictionaries = new Dictionary<string, object>();
                //result.Dictionaries.Add("UserStatus", _globalService.GetUserStatusIds());
                result.Data = await _db.Users
                    .Include(e => e.Address)
                    .Select(user =>  GetUserDto(user))
                    .ToListAsync();
                //_logger.LogInformation($"Get Users: Complete data prepared succesfully");
                result.AddSuccess();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"There is an error getting users: {ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<UserDto>> Get(Guid id)
        {
            var result = new UiServerResponse<UserDto>();

            try
            {
                var user = await _db.Users.FindAsync(id);
                
                result.Data = GetUserDto(user);
                result.AddSuccess();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"There is an error getting users: {ex.Message}");
                return result;
            }
        }

        public async Task<UiServerResponse<Guid>> Add(UserDto model)
        {
            var result = new UiServerResponse<Guid>();
            try
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Status = (UserStatus)model.Status,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ValidFrom = DateTime.Parse(model.ValidFrom ?? DateTime.Now.ToString(CultureInfo.InvariantCulture)).ToUniversalTime(),
                    ValidTo = DateTime.Parse(model.ValidTo ?? DateTime.MaxValue.ToString(CultureInfo.InvariantCulture)).ToUniversalTime(),
                    Title = model.Title,
                    Address = model.Address,
                    Gender = (Gender)model.Gender,
                    Height = model.Height,
                    Weight = model.Weight,
                    Description = model.Description
                };
                
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Add User: Saved Changes");
                result.Data = user.Id;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                result.Success = false;
                _logger.LogInformation($"Add User: There is an error creating new user {model.Username} : {ex.Message}");
                return result;
            }
        }
        
        public async Task<UiServerResponse<bool>> Put(UserDto model)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var user = await _db.Users.FindAsync(model.Id);

                if (user == null) return await Task.FromResult(result);

                user.Description = model.Description;
                user.Username = model.Username;
                user.Email = model.Email;
                user.Status = (UserStatus)model.Status;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.ValidFrom = DateTime.Parse(model.ValidFrom ?? DateTime.Now.ToString()).ToUniversalTime();
                user.ValidTo = DateTime.Parse(model.ValidTo ?? DateTime.MaxValue.ToString()).ToUniversalTime();
                user.Title = model.Title;
                user.Address = model.Address;
                user.Gender = (Gender)model.Gender;
                user.Height = model.Height;
                user.Weight = model.Weight;

                
                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Update User: Saved Changes");
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"Update User: {model.Id}: {ex.Message}");
                result.Data = false;
                return result;
            }
        }

        public async Task<UiServerResponse<bool>> Delete(Guid id)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var user = await _db.Users.FindAsync(id);

                if (user == null)
                {
                    result.AddNotFound();
                    _logger.LogInformation($"UserService, Delete: {id} not found");
                    return await Task.FromResult(result);
                }

                _db.Users.Remove(user);

                await _db.SaveChangesAsync();
                //result.AddSuccess(_stringLocalizer["201"]);
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"Delete User : There is an error deleting users {id}: {ex.Message}");
                result.Data = false;
                return result;
            }
        }

        public async Task<UiServerResponse<List<UserDto>>> GetContacts()
        {
            var result = new UiServerResponse<List<UserDto>>();

            try
            {
                var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                
                
                if (value?.Value != null)
                {
                    var claims = value.Subject?.Claims.FirstOrDefault(e => e.Type == "jti");
                    if (claims == null)
                    {
                        result.Success = false;
                        return await Task.FromResult(result);
                    }
                    
                    var userId = Guid.Parse(claims.Value);
                    var users = await _db.UserContacts
                        .Include(e => e.Contact)
                        .Include(e => e.User).Where(e => e.UserId == userId)
                        .Select(e => GetUserDto(e.Contact))
                        .ToListAsync();

                    result.Data = users;
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                }

                result.AddSuccess();
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"There is an error getting users: {ex.Message}");
                return result;
            }
        }
        
        private static UserDto GetUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Status = (int)user.Status,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ValidFrom = user.ValidFrom.ToString(CultureInfo.InvariantCulture),
                ValidTo = user.ValidTo.ToString(CultureInfo.InvariantCulture),
                Title = user.Title,
                Photo = user.Photo,
                AuthType = (int)user.AuthType,
                Address = user.Address
            };
        }
    }
}