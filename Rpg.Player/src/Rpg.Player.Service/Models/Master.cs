using Rpg.Common;

namespace Rpg.Player.Service.Models
{
    public class Master : IModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int HpModifier { get; set; }
        public int MpModifier { get; set; }

        public int StrengthModifier { get; set; }
        public int DefenseModifier {  get; set; }
        public int AgliltyModifier { get; set; }
        public int WisdomModifier { get; set; }
        public int LuckModifier { get; set; }
    }
}
