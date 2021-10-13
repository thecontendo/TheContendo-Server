using System;
using Contendo.Models.Enums;

namespace Contendo.Models.Challenges.Dto
{
    public class ChallengeDto: BaseModel
    {
        public int Points { get; set; }
        
        public int ChallengeStatus { get; set; }
        public int ChallengerStatus { get; set; }
        public int DefenderStatus { get; set; }
        
        public int Duration { get; set; }

        public Guid ShotId { get; set; }

        public Guid ChallengerId { get; set; }

        public Guid DefenderId { get; set; }
        
        public string Defender { get; set; }
    }
}