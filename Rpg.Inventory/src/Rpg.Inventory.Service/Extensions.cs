using Rpg.Inventory.Service.Dtos;
using Rpg.Inventory.Service.Models;

namespace Rpg.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string Name, string Description)
        {
            return new InventoryItemDto(item.CatalogItemId,Name,Description,item.Quantity,item.AcquiredDate);
        }
    }
}
