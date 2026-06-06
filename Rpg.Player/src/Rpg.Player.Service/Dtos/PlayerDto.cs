namespace Rpg.Player.Service.Dtos
{
    public record PlayerDto
    (
        Guid Id,
        Guid UserId,
        string Name,

        int Level,
        long Experience,

        int HP,
        int MP,
        int Strength,
        int Defense,
        int Agility,
        int Wisdom,
        int Luck,

        string CurrentMap,
        int PositionX,
        int PositionY,

        Guid? MasterId
        );
}
