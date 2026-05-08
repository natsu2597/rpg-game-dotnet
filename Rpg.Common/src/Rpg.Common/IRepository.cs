

using System.Linq.Expressions;

namespace Rpg.Common;

public interface IRepository<T> where T : IModel
{
    Task CreateItemAsync(T item);
    Task DeleteItemAsync(Guid id);
    Task<IReadOnlyCollection<T>> GetAllItemAsync();
    Task<IReadOnlyCollection<T>> GetAllItemAsync(Expression<Func<T, bool>> filter);
    Task<T> GetItemAsync(Guid id);
    Task<T> GetItemAsync(Expression<Func<T, bool>> filter);
    Task UpdateItemAsync(T item);
}
