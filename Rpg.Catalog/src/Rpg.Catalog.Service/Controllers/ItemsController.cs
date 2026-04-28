using Microsoft.AspNetCore.Mvc;
using Rpg.Catalog.Service.Dtos;
using Rpg.Catalog.Service.Models;
using Rpg.Catalog.Service.Repositories;

namespace Rpg.Catalog.Service.Controllers;


[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;

    public ItemController(IRepository<Item> itemsRepository)
    {
        this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
        var items = (await itemsRepository.GetAllItemAsync())
            .Select(item => item.AsDto());
        return items;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
    {
        var item = (await itemsRepository.GetItemAsync(id));

        if(item == null)
        {
            return NotFound();
        }

        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateNewItemAsync(CreateItemDto newItem)
    {
        var item = new Item
        {
            Name = newItem.Name,
            Description = newItem.Description,
            Price = newItem.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        
        await itemsRepository.CreateItemAsync(item);

        return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(Guid id, UpdateItemDto updatedItemDto)
    {
        var existingItem = await itemsRepository.GetItemAsync(id);
        if(existingItem == null)
        {
            return NotFound();
        }

        existingItem.UpdateFromDto(updatedItemDto);

        await itemsRepository.UpdateItemAsync(existingItem);

        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var item = await itemsRepository.GetItemAsync(id);
        if(item == null) { return NotFound(); }

        await itemsRepository.DeleteItemAsync(item.Id);
        return NoContent();
    }
}