using System;
using Contendo.Models.Enums;
using Contendo.Models.Identity;
using Contendo.Models.Shots;

namespace Contendo.Models.Challenges.Dto
{
    public class RecievedChallengeDto
    {
        public int Points { get; set; }
        
        public ChallengeStatus ChallengeStatus { get; set; }
        
        public DateTime Duration { get; set; }
        
        public Shot Shot { get; set; }
        
        public Guid ChallengerId { get; set; }

        public User Participant { get; set; }
    }
}