using System.ComponentModel.DataAnnotations;

namespace Rpg.Catalog.Service.Dtos;

public record CreateItemDto(
        [Required]string Name,
        [Required]string Description,
        [Required][Range(0,50000)]decimal Price
    );