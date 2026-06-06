using Rpg.Common;

namespace Rpg.Player.Service.Models
{
    public class Ryu : IModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public string Name { get; set; } = string.Empty;

        public int Level { get; set; } = 1;
        public long Experience { get; set;  }
        public long ExperienceToNextLevel { get; set; } = 100;

        public int HP { get; set; } = 250;
        public int MP { get; set; } = 50;

        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Agility { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Luck { get; set; } = 10;

        public int BonusHP { get; set; }
        public int BonusMP { get; set; }

        public int BonusStrength { get; set; }
        public int BonusDefense { get; set; }
        public int BonusAgility{ get; set; }
        public int BonusWisdom { get; set; }
        public int BonusLuck { get; set; }

        public string CurrentMap { get; set; } = "grasslands";

        public int PositionX { get; set;  }
        public int PositionY { get; set;  }

        public Guid? MasterId { get; set;  }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
