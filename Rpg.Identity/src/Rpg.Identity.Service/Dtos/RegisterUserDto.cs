using System.ComponentModel.DataAnnotations;

namespace Rpg.Identity.Service.Dtos
{
    public record RegisterUserDto
    (
        [Required]string Username,
        [Required] string Email,
        [Required] string Password
    );
}
