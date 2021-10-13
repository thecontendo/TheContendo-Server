using System;
using Contendo.Models.Identity;

namespace Contendo.Models.Challenges
{
    public class Participation: BaseModel
    {
        public Guid ParticipantId { get; set; }
        public virtual User Participant { get; set; }
        
        public Guid ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; }
    }
}