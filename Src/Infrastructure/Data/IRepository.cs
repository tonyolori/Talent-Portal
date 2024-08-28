using System.Linq.Expressions;

namespace Infrastructure.Data;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<List<TEntity?>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
}