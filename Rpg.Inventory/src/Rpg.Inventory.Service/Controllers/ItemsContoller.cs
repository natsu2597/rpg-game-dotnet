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
        private readonly IRepository<InventoryItem> inventoryItemsRepository;
        private readonly IRepository<CatalogItem> catalogItemsRepository;

        public ItemsContoller(IRepository<InventoryItem> inventoryItemsRepository,IRepository<CatalogItem> catalogItemsRepository)
        {
            this.inventoryItemsRepository = inventoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetItemsAsync(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            var inventoryItems = await inventoryItemsRepository.GetAllItemAsync(item => item.UserId == userId);
            var itemIds = inventoryItems.Select(item => item.CatalogItemId);

            var catalogItems = await catalogItemsRepository.GetAllItemAsync(item => itemIds.Contains(item.Id));
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
