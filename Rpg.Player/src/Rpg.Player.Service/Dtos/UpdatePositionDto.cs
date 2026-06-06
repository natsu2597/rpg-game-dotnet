namespace Rpg.Player.Service.Dtos
{
    public record UpdatePositionDto
    (
           string CurrentMap,
           int PositionX,
           int PositionY
        );
}
