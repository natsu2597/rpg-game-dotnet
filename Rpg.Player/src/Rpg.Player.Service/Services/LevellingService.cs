using Rpg.Player.Service.Models;

namespace Rpg.Player.Service.Services
{
    public class LevellingService
    {
        private readonly GrowthTableService growthTableService;

        public LevellingService(GrowthTableService growthTableService)
        {
            this.growthTableService = growthTableService;
        }

        public void LevelUp(Ryu player, Master master)
        {
            var nextLevel = player.Level + 1;

            var growth = growthTableService.GetGrowth(nextLevel);

            player.HP += growth.Hp + (master?.HpModifier ?? 0);
            player.MP += growth.Mp + (master?.MpModifier ?? 0);

            player.Strength += growth.Strength + (master?.StrengthModifier ?? 0);
            player.Defense += growth.Defense + (master?.DefenseModifier ?? 0);
            player.Agility += growth.Agility + (master?.AgliltyModifier ?? 0);
            player.Wisdom += growth.Wisdom + (master?.WisdomModifier ?? 0);

            player.Luck += growth.Luck + (master?.LuckModifier ?? 0);

            player.Level++;
            player.ExperienceToNextLevel = CalculateRequiredExperience(player.Level);
        }

        private long CalculateRequiredExperience(int level)
        {
            return 100 * level * level;
        }
    }
}
