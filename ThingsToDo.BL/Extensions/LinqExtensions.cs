using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThingsToDo.BL.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> OrderByDirection<T, U>(this IQueryable<T> source, Expression<Func<T, U>> predicate, string orderDirection)
        {
            if (orderDirection == "asc")
            {
                source = source.OrderBy(predicate);
            }
            else
            {
                source = source.OrderByDescending(predicate);
            }
            return source;
        }

    }
}
