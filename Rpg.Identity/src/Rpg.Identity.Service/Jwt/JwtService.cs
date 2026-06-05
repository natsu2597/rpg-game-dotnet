using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rpg.Identity.Service.Models;
using Rpg.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rpg.Identity.Service.Jwt
{
    public class JwtService
    {
        private readonly JwtSettings jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            jwtSettings = options.Value;
        }

        public string GenerateToken(User user) { 

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Username),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            };

            var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)
                );

            var credentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                    issuer : jwtSettings.Issuer,
                    audience : jwtSettings.Audience,
                    claims : claims,
                    expires : DateTime.UtcNow.AddDays(1),
                    signingCredentials : credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
