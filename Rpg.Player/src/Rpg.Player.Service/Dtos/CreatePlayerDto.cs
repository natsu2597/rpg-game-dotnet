namespace Rpg.Player.Service.Dtos
{
    public record CreatePlayerDto
    (
        Guid UserId,
        string Name
    );
}
