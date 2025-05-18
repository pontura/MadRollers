using System;
namespace Catoff
{
    [Serializable]
    public class CreateChallengeDto
    {
        public string ChallengeName;
        public string ChallengeDescription;
        public long StartDate;
        public long EndDate;
        public int GameID;
        public int MaxParticipants;
        public int Wager;
        public int Target;
        public int ChallengeCreator;
        public bool AllowSideBets;
        public int SideBetsWager;
        public string Unit;
        public bool IsPrivate;
        public string Currency;
        public string ChallengeCategory;
        public string NFTMedia;
        public string Media;
        public long ActualStartDate;
        public string UserAddress;
    }
}