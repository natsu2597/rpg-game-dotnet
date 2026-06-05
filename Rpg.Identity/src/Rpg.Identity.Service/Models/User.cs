using Rpg.Common;

namespace Rpg.Identity.Service.Models
{
    public class User : IModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
