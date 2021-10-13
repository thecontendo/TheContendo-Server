using System;
using Contendo.Models.Enums;
using Contendo.Models.Identity;
using Contendo.Models.Shots;

namespace Contendo.Models.Challenges.Dto
{
    public class RecievedChallengeDto
    {
        public int Points { get; set; }
        
        public int ChallengeStatus { get; set; }
        public int ChallengerStatus { get; set; }
        public int DefenderStatus { get; set; }
        
        public int Duration { get; set; }
        
        public Shot Shot { get; set; }
        
        public Guid ChallengerId { get; set; }

        public User Defender { get; set; }
    }
}