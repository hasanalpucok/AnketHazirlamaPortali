using System;
namespace BusinessLayer.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable)
        {
            return await queryable.ToListAsync();
        }
    }
}

