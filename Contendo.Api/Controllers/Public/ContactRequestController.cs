using System;
using System.Threading.Tasks;
using Contendo.Data;
using Contendo.Data.Identity;
using Contendo.Models.Challenges.Dto;
using Contendo.Models.ContactRequests.Dto;
using Contendo.Models.Identity.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contendo.Api.Controllers.Public
{
    public class ContactRequestController: PublicBaseController
    {
        private readonly IContactRequestService _contactRequest;
        private readonly ILogger<ContactRequestController> _logger;

        public ContactRequestController(ILogger<ContactRequestController> logger, IContactRequestService contactRequest)
        {
            _logger = logger;
            _contactRequest = contactRequest;
        }
        
        [HttpPost("ContactRequest")]
        public async Task<IActionResult> ContactRequest(ContactRequestDto model)
        {
            return Ok(await _contactRequest.ContactRequest(model));
        }
        
        [HttpPost("ContactRequestByEmail")]
        public async Task<IActionResult> ContactRequestByEmail(UserEmail model)
        {
            return Ok(await _contactRequest.ContactRequestByEmail(model));
        }
        
        [HttpGet("CheckContactRequests")]
        public async Task<IActionResult> CheckContactRequests()
        {
            return Ok(await _contactRequest.CheckContactRequest());
        }
        
        [HttpPost("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest(RequestResponse requestResponse)
        {
            return Ok(await _contactRequest.AcceptRequest(requestResponse));
        }
        
        [HttpPost("AcceptUrlRequest")]
        public async Task<IActionResult> AcceptUrlRequest(string url)
        {
            return Ok(await _contactRequest.AcceptUrlRequest(url));
        }
        
        [HttpDelete("RemoveContact/{contactId:guid}")]
        public async Task<IActionResult> RemoveContact(Guid contactId)
        {
            return Ok(await _contactRequest.RemoveContact(contactId));
        }
    }
}