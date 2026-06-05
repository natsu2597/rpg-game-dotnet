using MassTransit;
using Rpg.Common;
using Rpg.Contracts;
using Rpg.Inventory.Service.Models;

namespace Rpg.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {

        private readonly IRepository<CatalogItem> repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await repository.GetItemAsync(message.ItemId);

            if (item == null) {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };

                await repository.CreateItemAsync(item);
            }

            else {
                item.Name = message.Name;
                item.Description = message.Description;

                await repository.UpdateItemAsync(item);
            }

            
        }
    }
}
