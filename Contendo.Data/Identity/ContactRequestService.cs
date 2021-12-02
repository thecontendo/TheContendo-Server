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
    public class ContactRequestService: IContactRequestService
    {
        private readonly CDbContext _db;
        private readonly ILogger<ContactRequestService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactRequestService(CDbContext db, ILogger<ContactRequestService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<UiServerResponse<bool>> ContactRequest(ContactRequestDto model)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                await _db.ContactRequests.AddAsync(new ContactRequest
                {
                    UserId = model.UserId,
                    ContactId = model.ContactId,
                    ContactRequestStatus = ContactRequestStatus.Pending
                });
                
                await _db.SaveChangesAsync();
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ContactRequest User : T{ex.Message}");
                result.Data = false;
                return result;
            }
        }
        
        public async Task<UiServerResponse<bool>> ContactRequestByEmail(UserEmail model)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var reciever = await _db.Users.FirstOrDefaultAsync(e => e.Email == model.Email);

                if (reciever != null)
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
                        await _db.ContactRequests.AddAsync(new ContactRequest
                        {
                            UserId = userId,
                            ContactId = reciever.Id,
                            ContactRequestStatus = ContactRequestStatus.Pending
                        });
               
                        await _db.SaveChangesAsync();
                        result.Data = true;
                        result.Success = true;
                    }
                    
                }
                else
                {
                    result.Data = false;
                    result.AddSuccess("Email invitation has been sent");
                }
                
               
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ContactRequest User : T{ex.Message}");
                result.Data = false;
                return result;
            }
        }
        
        public async Task<UiServerResponse<List<ContactRequestDetailsDto>>> CheckContactRequest()
        {
            var result = new UiServerResponse<List<ContactRequestDetailsDto>>();
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
                    
                    var requests = await _db.ContactRequests.Where(e => e.ContactId == userId 
                                                                        && e.ContactRequestStatus == ContactRequestStatus.Pending).ToListAsync();

                    var finalRequests = new List<ContactRequestDetailsDto>();

                    foreach (var item in requests)
                    {
                        var contact = await _db.Users.FindAsync(item.ContactId);
                        var user = await _db.Users.FindAsync(item.UserId);
                        finalRequests.Add(new ContactRequestDetailsDto
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            ContactId = item.ContactId,
                            ContactName = $"{contact.FirstName} {contact.LastName}",
                            ContactEmail = contact.Email,
                            UserName = $"{user.FirstName} {user.LastName}",
                            UserEmail = user.Email
                        });
                    }
                
                    await _db.SaveChangesAsync();
                    result.Data = finalRequests;
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                }

             
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"CheckContactRequest User : T{ex.Message}");
                result.Success = false;
                return result;
            }
        }

        public async Task<UiServerResponse<bool>> AcceptRequest(RequestResponse requestResponse)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var request = await _db.ContactRequests.FindAsync(requestResponse.requestId);

                if (request == null)
                {
                    result.Success = false;
                    return await Task.FromResult(result);
                }

                request.ContactRequestStatus = requestResponse.Accepted ? ContactRequestStatus.Accepted : ContactRequestStatus.Rejected;

                if (requestResponse.Accepted)
                {
                    await _db.UserContacts.AddAsync(new UserContact
                    {
                        ContactId = request.ContactId,
                        UserId = request.UserId
                    });
                    await _db.UserContacts.AddAsync(new UserContact
                    {
                        ContactId = request.UserId,
                        UserId = request.ContactId
                    });
                }

                _db.ContactRequests.Update(request);
                
                await _db.SaveChangesAsync();
                result.Data = true;
                result.Success = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ContactRequest User : T{ex.Message}");
                result.Data = false;
                return result;
            }
        }

        public async Task<UiServerResponse<bool>> AcceptUrlRequest(string url)
        {
            var result = new UiServerResponse<bool>(false);
            try
            {
                var builder = new UriBuilder(url);
                var requestId = Guid.Parse(builder.Path);
                var request = await _db.ContactRequests.FindAsync(requestId);

                if (request == null)
                {
                    result.Success = false;
                    return await Task.FromResult(result);
                }

                request.ContactRequestStatus = ContactRequestStatus.Accepted;
                
                _db.ContactRequests.Update(request);
                
                await _db.SaveChangesAsync();
                result.Data = true;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ContactRequest User : T{ex.Message}");
                result.Data = false;
                return result;
            }
        }
        public async Task<UiServerResponse<bool>> RemoveContact(Guid contactId)
        {
            var result = new UiServerResponse<bool>(false);
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
                    var request = await _db.UserContacts.Where(e => (e.UserId == userId && e.ContactId == contactId)
                    || e.UserId == contactId && e.ContactId == userId).ToListAsync();

                    if (request == null)
                    {
                        result.Success = false;
                        return await Task.FromResult(result);
                    }

                    _db.UserContacts.RemoveRange(request);
                
                    await _db.SaveChangesAsync();
                    result.Data = true;
                    result.AddSuccess("User removed successfully!");
                    return await Task.FromResult(result);
                }
                else
                {
                    result.Success = false;
                    return await Task.FromResult(result);
                }

               
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                _logger.LogInformation($"ContactRequest User : T{ex.Message}");
                result.Data = false;
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
                Title = user.Title
            };
        }
    }
}