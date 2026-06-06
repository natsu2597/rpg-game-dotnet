using Rpg.Player.Service.Dtos;
using Rpg.Player.Service.Models;

namespace Rpg.Player.Service.Extension
{
    public static class Extensions
    {
        public static int TotalHP(this Ryu player)
        => player.HP + player.BonusHP;

        public static int TotalStrength(this Ryu player)
            => player.Strength + player.BonusStrength;

        public static int TotalDefense(this Ryu player)
            => player.Defense + player.BonusDefense;

        public static int TotalSpeed(this Ryu player)
            => player.Agility + player.BonusAgility;
        public static int TotalWisdom(this Ryu player)
            => player.Wisdom + player.BonusWisdom;
        public static double CriticalChance(this Ryu player)
            => Math.Min(5 + player.Luck * 0.15, 50);

        public static double ParryChance(
            this Ryu player,
            int enemySpeed)
            => Math.Min(
                Math.Max(
                    0,
                    (player.Agility - enemySpeed) * 0.5),
                35);

        public static PlayerDto AsDto(this Ryu player)
        {
            return new PlayerDto(
                    player.Id,
                    player.UserId,
                    player.Name,

                    player.Level,
                    player.Experience,

                    player.HP,
                    player.MP,

                    player.Strength,
                    player.Defense,
                    player.Agility,
                    player.Wisdom,
                    player.Luck,

                    player.CurrentMap,
                    player.PositionX,
                    player.PositionY,

                    player.MasterId
                );
        }
    }
}
