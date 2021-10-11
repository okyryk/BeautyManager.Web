using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace BeautyManager.Web.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetItemAsync(string id);

        Task<IEnumerable<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetItemsAsync();

        Task<TEntity> AddItemAsync(TEntity item);

        Task DeleteItemAsync(string id);

        Task<TEntity> UpdateItemAsync(string id, TEntity item);
    }
}
