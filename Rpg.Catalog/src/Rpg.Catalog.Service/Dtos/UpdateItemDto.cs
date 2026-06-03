using System.ComponentModel.DataAnnotations;

namespace Rpg.Catalog.Service.Dtos;

public record UpdateItemDto(
        [Required] string Name,
        string Description,
        [Range(0,50000)] decimal Price
    );