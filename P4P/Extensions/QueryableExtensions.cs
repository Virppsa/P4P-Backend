using Microsoft.EntityFrameworkCore;
using P4P.Filters;
using P4P.Wrappers;

namespace P4P.Extensions;

public static class QueryableExtensions
{
    // 3. Generics (in delegates, events and methods)(at least two)
    public static PaginatedList<List<TEntity>> ToPaginatedList<TEntity>(this IQueryable<TEntity> source, PaginationFilter filter)
    {
        if (source == null)
        {
            throw new ArgumentException("Parameter cannot be null", nameof(source));
        }

        var count = source.Count();
        var list = source.Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .ToList();
        
        return PaginatedList<TEntity>.Create(list, count, filter);
    }
}
