using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;
using Rpg.Common;
using Rpg.Player.Service.Dtos;
using Rpg.Player.Service.Extension;
using Rpg.Player.Service.Models;
using Rpg.Player.Service.Services;

namespace Rpg.Player.Service.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly IRepository<Ryu> playerRepository;
        private readonly IRepository<Master> masterRepository;
        private readonly LevellingService levellingService;

        public PlayersController(IRepository<Ryu> playerRepository, IRepository<Master> masterRepository, LevellingService levellingService)
        {
            this.playerRepository = playerRepository;
            this.masterRepository = masterRepository;
            this.levellingService = levellingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAllPlayers()
        {

            var players = (await playerRepository.GetAllItemAsync())
            .Select(player => player.AsDto());

            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id)
        {
            var player = await playerRepository.GetItemAsync(id);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            return Ok(player.AsDto());
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PlayerDto>> GetPlayerByUserId(Guid userId)
        {
            var player = await playerRepository.GetItemAsync(
                    p => p.UserId == userId
                );

            if(player == null)
            {
                return NotFound("User not Found");
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
        {
            var player = new Ryu
            {
                UserId = createPlayerDto.UserId,
                Name = createPlayerDto.Name,

                Level = 1,
                Experience = 0,

                HP = 250,
                MP = 20,

                Strength = 10,
                Defense = 10,
                Agility = 10,
                Wisdom = 10,
                Luck = 10,

                CreatedDate = DateTimeOffset.UtcNow,
            };

            await playerRepository.CreateItemAsync(player);

            return CreatedAtAction(
                    nameof(GetPlayer),
                    new { id = player.Id },
                    player.AsDto()
                );
        }

        [HttpPut("{id}/position")]
        public async Task<IActionResult> UpdatePosition(Guid id,UpdatePositionDto updatePositionDto)
        {
            var player = await playerRepository.GetItemAsync(id);
            
            if(player == null)
            {
                return NotFound("Player not found");
            }

            player.CurrentMap = updatePositionDto.CurrentMap;
            player.PositionX = updatePositionDto.PositionX;
            player.PositionY = updatePositionDto.PositionY;

            await playerRepository.UpdateItemAsync(player);

            return NoContent();
        }

        [HttpPut("{id}/experience")]
        public async Task<IActionResult> GainExperience(Guid id,GainedExperienceDto dto)
        {
            var player = await playerRepository.GetItemAsync(id);

            if(player == null)
            {
                return NotFound("Player not found");
            }

            player.Experience += dto.Experience;

            await playerRepository.UpdateItemAsync(player);

            return NoContent();
        }

        [HttpPut("{id}/master")]
        public async Task<IActionResult> AssignMaster(Guid id,AssignMasterDto assignMasterDto)
        {
            var player = await playerRepository.GetItemAsync(id);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            var master = await masterRepository.GetItemAsync(assignMasterDto.MasterId);
            if (master == null)
            {
                return NotFound("Master not found");
            }

            player.MasterId = master.Id;

            await playerRepository.UpdateItemAsync(player);

            return NoContent();
        }

        [HttpPost("{id}/level-up")]
        public async Task<IActionResult> LevelUp(Guid id)
        {
            var player = await playerRepository.GetItemAsync(id);
            if (player == null)
            {
                return NotFound("Player not found");
            }

            Master? master = null;

            if (player.MasterId.HasValue)
            {
                master = await masterRepository.GetItemAsync(player.MasterId.Value);
            }

            levellingService.LevelUp(player, master!);

            await playerRepository.UpdateItemAsync(player);

            return Ok(player.AsDto());

        }
    }
}
