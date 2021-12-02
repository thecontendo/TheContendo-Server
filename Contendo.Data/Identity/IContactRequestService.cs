using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contendo.Models;
using Contendo.Models.ContactRequests.Dto;
using Contendo.Models.Identity.Dto;

namespace Contendo.Data.Identity
{
    public interface IContactRequestService
    {
        Task<UiServerResponse<bool>> ContactRequest(ContactRequestDto model);
        
        Task<UiServerResponse<bool>> ContactRequestByEmail(UserEmail model);

        Task<UiServerResponse<List<ContactRequestDetailsDto>>> CheckContactRequest();
        
        Task<UiServerResponse<bool>> AcceptRequest(RequestResponse requestResponse);
        
        Task<UiServerResponse<bool>> AcceptUrlRequest(string url);
        Task<UiServerResponse<bool>> RemoveContact(Guid contactId);
    }
}