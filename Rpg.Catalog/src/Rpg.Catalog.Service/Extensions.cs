using Rpg.Catalog.Service.Dtos;
using Rpg.Catalog.Service.Models;

namespace Rpg.Catalog.Service;

public static class Extensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
    }

    public static Item AsModel(this ItemDto itemDto)
    {
        return new Item(itemDto.Id, itemDto.Name, itemDto.Description, itemDto.Price, itemDto.CreatedDate);
    }

    public static void UpdateFromDto(this Item item, UpdateItemDto dto)
    {
        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Price = dto.Price;
    }
}