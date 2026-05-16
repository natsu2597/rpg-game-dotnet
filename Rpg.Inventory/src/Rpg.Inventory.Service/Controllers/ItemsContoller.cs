using Microsoft.AspNetCore.Mvc;
using Rpg.Common;
using Rpg.Inventory.Service.Clients;
using Rpg.Inventory.Service.Dtos;
using Rpg.Inventory.Service.Models;

namespace Rpg.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsContoller : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemsRepository;
        private readonly CatalogClient catalogClient;

        public ItemsContoller(IRepository<InventoryItem> itemsRepository,CatalogClient catalogClient)
        {
            this.itemsRepository = itemsRepository;
            this.catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetItemsAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemsAsync();
            var inventoryItems = await itemsRepository.GetAllItemAsync(item => item.UserId == userId);
            var items = inventoryItems.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> GrantItemAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await itemsRepository.GetItemAsync(
                    item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId
                );

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow,
                };

                await itemsRepository.CreateItemAsync(inventoryItem);
            }

            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateItemAsync(inventoryItem);
            }

            return Ok(inventoryItem);
        }
    }
}
