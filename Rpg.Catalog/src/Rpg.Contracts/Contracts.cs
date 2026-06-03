using System;
using System.Collections.Generic;
using System.Text;

namespace Rpg.Contracts
{
    public record CatalogItemCreated(Guid ItemId, string Name, string Description);
    public record CatalogItemUpdated(Guid ItemId, string Name, string Description); 
    public record CatalogItemDeleted(Guid ItemId);
}
