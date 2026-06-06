using Rpg.Player.Service.Models;

namespace Rpg.Player.Service.Services
{
    public class GrowthTableService
    {
        private readonly Dictionary<int, GrowthTable> growthTable =
            new();

        public GrowthTableService()
        {
            var random = new Random(2597);

            for(int level = 2; level<=99;level++)
            {
                growthTable[level] = new GrowthTable(
                        Hp : random.Next(32,45),
                        Mp : random.Next(6,10),
                        Strength: random.Next(2, 4),
                        Defense: random.Next(2, 4),
                        Agility: random.Next(1, 4),
                        Wisdom: random.Next(2, 4),
                        Luck: random.Next(1, 3)
                    );
            }
        }
        public GrowthTable GetGrowth(int level)
        {
            return growthTable[level];
        }
    }
}
