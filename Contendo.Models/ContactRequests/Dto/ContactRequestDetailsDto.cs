using System;

namespace Contendo.Models.ContactRequests.Dto
{
    public class ContactRequestDetailsDto : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid ContactId { get; set; }

        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}