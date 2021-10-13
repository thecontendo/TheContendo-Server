using System;

namespace Contendo.Models.Identity.Dto
{
    public class UserContactsDto
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ContactId { get; set; } // In lack of better name.
        public User Contact { get; set; }
    }
}