namespace Rpg.Identity.Service.Dtos
{
    public record AuthResponseDto
    (
        Guid UserId,
        string Username,
        string Token
    );
}
