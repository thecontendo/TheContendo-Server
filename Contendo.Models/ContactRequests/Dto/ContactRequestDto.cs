using System;

namespace Contendo.Models.ContactRequests.Dto
{
    public class ContactRequestDto : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid ContactId { get; set; }
        

        public bool Accepted { get; set; }
    }
}