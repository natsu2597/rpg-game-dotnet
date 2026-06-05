using Microsoft.AspNetCore.Mvc;
using Rpg.Common;
using Rpg.Identity.Service.Dtos;
using Rpg.Identity.Service.Jwt;
using Rpg.Identity.Service.Models;

namespace Rpg.Identity.Service.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> userRepository;
        private readonly JwtService jwtService;

        public UserController(IRepository<User> userRepository,JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
        }

        [HttpPost("register")]
            public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
            {
            var existingUser =
            (await userRepository.GetAllItemAsync())
            .FirstOrDefault(x => x.Username == registerUserDto.Username);

            if (existingUser != null) {
                return BadRequest("Username already exist");
            }

            var user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password),
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await userRepository.CreateItemAsync(user);

            return Ok(user);
            }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> LoginUser(UserLoginDto loginUser)
        {
            var user = (await userRepository.GetAllItemAsync())
                .FirstOrDefault(x => x.Username == loginUser.Username);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(
                        loginUser.Password,
                        user.Password
                    );

            if (!validPassword)
            {
                return Unauthorized("Incorrect Password");
            }

            var token = jwtService.GenerateToken(user);

            return Ok(
                    new AuthResponseDto
                    (
                        user.Id,
                        user.Username,
                        token
                    )
                );
        }

            
    }
}
