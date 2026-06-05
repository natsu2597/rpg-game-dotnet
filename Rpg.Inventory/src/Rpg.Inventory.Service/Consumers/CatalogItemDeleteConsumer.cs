using MassTransit;
using Rpg.Common;
using Rpg.Contracts;
using Rpg.Inventory.Service.Models;

namespace Rpg.Inventory.Service.Consumers
{
    public class CatalogItemDeleteConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeleteConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await repository.GetItemAsync(message.ItemId);

            if (item == null)
            {
                return;
            }

            await repository.DeleteItemAsync(item.Id);
        }
    }
}
