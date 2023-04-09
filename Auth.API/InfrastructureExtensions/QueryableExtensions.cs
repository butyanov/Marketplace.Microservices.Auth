using System.Linq.Expressions;
using Auth.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.InfrastructureExtensions;

public static class QueryableExtensions
{
    public static async Task<T> FirstOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(cancellationToken) 
               ?? throw new NotFoundException<T>();
    }
    
    public static async Task<T> FirstOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken) 
               ?? throw new NotFoundException<T>();
    }
    
    public static async Task<bool> AnyOrNotFoundAsync<T>(
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.AnyAsync(predicate, cancellationToken) 
            ? true 
            : throw new NotFoundException<T>();
    }
}