using Rpg.Catalog.Service.Models;

namespace Rpg.Catalog.Service.Repositories;

public interface IRepository<T> where T : IModel
{
    Task CreateItemAsync(T item);
    Task DeleteItemAsync(Guid id);
    Task<IReadOnlyCollection<T>> GetAllItemAsync();
    Task<T> GetItemAsync(Guid id);
    Task UpdateItemAsync(T item);
}
