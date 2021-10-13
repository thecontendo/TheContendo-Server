using System;
using Contendo.Models.Enums;

namespace Contendo.Models.ContactRequests
{
    public class ContactRequest: BaseModel
    {
        public Guid UserId { get; set; }
        
        public Guid ContactId { get; set; }

        public ContactRequestStatus ContactRequestStatus { get; set; }

        public string Uri { get; set; }
    }
}