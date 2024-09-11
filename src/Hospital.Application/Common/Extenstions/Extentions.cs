using Hospital.Application.Common.Enums;
using System.Linq.Expressions;

namespace Hospital.Application.Common.Extenstions;

public static class Extentions
{
    public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, OrderDirection direction)
    {
        return direction == OrderDirection.Ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }
    public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey> (this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, OrderDirection direction)
    {
        return direction == OrderDirection.Ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }
}
