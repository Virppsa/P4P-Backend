using System.Linq.Expressions;
using P4P.Filters;
using P4P.Wrappers;

namespace P4P.Repositories.Interfaces;

public interface IGenericRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<string>? requiredFields = null);

    PaginatedList<List<TEntity>> GetPaginated(
        PaginationFilter paginationFilter,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<string>? requiredFields = null);
    
    Task<TEntity?> GetFirstAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        List<string>? requiredFields = null);

    Task<bool> Exists(Expression<Func<TEntity, bool>> filter);

    Task<TEntity?> GetByIdAsync(object id);

    Task<TEntity> InsertAsync(TEntity entity);

    Task<TEntity> DeleteAsync(object id);

    Task<TEntity> DeleteAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    int Count(Expression<Func<TEntity, bool>>? filter = null);
}
