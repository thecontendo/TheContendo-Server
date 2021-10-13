using System;
using Contendo.Models.Enums;
using Contendo.Models.Identity;
using Contendo.Models.Shots;

namespace Contendo.Models.Challenges
{
    public class Challenge: BaseModel
    {
        public int Points { get; set; }
        public int Duration { get; set; }
        
        public int TimeLimit { get; set; }

        public Guid ShotId { get; set; }
        public virtual Shot Shot { get; set; }

        public Guid ChallengerId { get; set; }
        public virtual User Challenger { get; set; }

        public Guid WinnerId { get; set; }

        public Guid DefenderId { get; set; }
        public virtual User Defender { get; set; }
        
        
        public ChallengeStatus ChallengeStatus { get; set; }
        public ChallengeStatus ChallengerStatus { get; set; }
        public ChallengeStatus DefenderStatus { get; set; }
    }
}