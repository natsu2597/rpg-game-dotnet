namespace Rpg.Catalog.Service.Dtos;

public record CreateItemDto(
        string Name,
        string Description,
        decimal Price
    );