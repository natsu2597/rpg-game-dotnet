using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Rpg.Catalog.Service.Dtos;
using Rpg.Catalog.Service.Models;
using Rpg.Common;
using Rpg.Contracts;

namespace Rpg.Catalog.Service.Controllers;


[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public ItemController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
    {
       

        var items = (await itemsRepository.GetAllItemAsync())
            .Select(item => item.AsDto());

        return Ok(items);
    }


    [HttpGet("{id}",Name = "GetItemById")]
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

        try
        {
            await publishEndpoint.Publish(
                new CatalogItemCreated(item.Id, item.Name, item.Description));

            Console.WriteLine("Published successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return CreatedAtAction("GetItemById", new { id = item.Id }, item.AsDto());
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

        await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var item = await itemsRepository.GetItemAsync(id);
        if(item == null) { return NotFound(); }

        await itemsRepository.DeleteItemAsync(item.Id);

        await publishEndpoint.Publish(new CatalogItemDeleted(id));

        return NoContent();
    }
}