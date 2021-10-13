using System;

namespace Contendo.Models.ContactRequests.Dto
{
    public class RequestResponse
    {
        public Guid requestId { get; set; }

        public bool Accepted { get; set; }
    }
}